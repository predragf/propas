using SMTLibReq.Transformation.SMTLib.Transformers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SMTLibReq.DataStructures;
using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using SMTLibReq.Parsers.BaseExpression;
using SMTLibReq.Parsers.BaseExpression.Interfaces;
using SMTLibReq.Transformation.BaseTransformationStructures;
using SMTLibReq.Transformation.SMTLib.Helpers;
using SMTLibReq.Transformation.SMTLib.DataStructures;
using System.Text.RegularExpressions;

namespace SMTLibReq.Transformation.SMTLib.Transformers
{
    public class SMTPropertyTransformer : ISMTLibPropertyTransformer
    {
        private static List<string> binaryLogicalOperators = new List<string> { "==", "<>", "<", "<=", ">", ">=", "+", "-", "/", "=>", "&&", "||" };
        private static List<string> unaryLogicalOperators = new List<string> { "!" };
        private static List<string> pathOperators = new List<string> { "AG", "AG<", "AF", "AF<", "U", "U<", "W", "W<", "A" };

        private BaseExpressionParser expressionParser;

        public SMTPropertyTransformer(BaseExpressionParser _expressionParser) {
            expressionParser = _expressionParser;
        }

        private ICollection<IDeclaration> generateFormulaDeclarations(IFormula formula) {
            List<IDeclaration> formulaDeclations = new List<IDeclaration>();
            foreach (Node proposition in formula.getLeafNodes()) {
                ICollection<IDeclaration> declarations = DeclarationGenerator.generateDeclarations(proposition.getExpression()); //createDeclarationFromAtomicProposition(proposition);
                formulaDeclations.AddRange(declarations);
            }
            return formulaDeclations;
        }

        private ParsingResult parseLeaf(Node leaf, int timeIndex, int intervalDuration) {            
            string leafExpression = leaf.getExpression();
            string result = leafExpression;
            double number;
            if (!double.TryParse(result, out number)) {
                string timeVariable = timeIndex == 0 ? "time" : string.Format("t{0}", timeIndex);

                if (intervalDuration > 0) {
                    timeVariable = string.Format("(+ {0} {1})", timeVariable, intervalDuration);
                }

                List<string> expressions = new List<string> { timeVariable };
                ICollection<string> functionInputs = PropositionParser.extractParameters(leafExpression);
                string functionName = PropositionParser.extractName(leafExpression);
                foreach (string fInput in functionInputs) {
                    expressions.Add(fInput);
                }
                result = SMTConstructsInstantiator.instantiateNaryOperator(functionName, expressions.ToArray());
            }      
            return new ParsingResult(result);
        }

        private string determineOperatorType(InternalNode node) {
            string operatorType = "";
            string operatorString = node.getExpression().Trim();            

            if (binaryLogicalOperators.Contains(operatorString))
            {
                operatorType = OperatorTypes.BINARY;
            }
            else if (unaryLogicalOperators.Contains(operatorString))
            {
                operatorType = OperatorTypes.UNARY;
            }
            else if (pathOperators.Exists(op => operatorString.StartsWith(op))) {
                operatorType = OperatorTypes.PATH;
            }

            return operatorType;
        }

        private int extractUpperBound(string expression) {
            //string numberPattern = @"[0-9]+(\.[0-9]+)?";
            string numberPattern = @"[0-9]+";
            Regex searchExpression = new Regex(numberPattern);
            var match = searchExpression.Match(expression);
            int result = 0;
            if (match.Success) {
                int.TryParse(match.Value, out result);
            }
            return result;
        }

        private ParsingResult parseUniaryOperator(InternalNode _operator, int timeIndex, int intervalDuration) {
            /*unary node in the tree structure, 
             * which means that the next expression for parsing is 
             always in the right-hand-side node
             */
            string operatorString = _operator.getExpression();
            operatorString = PropositionParser.trnasformOperatorToSMTLIB(operatorString);
            string result = SMTConstructsInstantiator.instantiateUnaryOperator(operatorString, parseNode(_operator.getRightHandSide(), timeIndex, intervalDuration).SmtExpression);
            return new ParsingResult(result);
        }

        private ParsingResult parseBinaryOperator(InternalNode _operator, int timeIndex, int intervalDuration)
        {
            string operatorString = _operator.getExpression();                     
            ParsingResult leftSide = parseNode(_operator.getLeftHandSide(), timeIndex, intervalDuration);
            /*only in a case of implication the interval duration is transferred to the right-hand size, otherwise not*/
            if (operatorString.Equals("=>") && leftSide.IntervalDuration > intervalDuration) {
                intervalDuration = leftSide.IntervalDuration;
            }
            ParsingResult rightSide = parseNode(_operator.getRightHandSide(), timeIndex, leftSide.IntervalDuration);            
            operatorString = PropositionParser.trnasformOperatorToSMTLIB(operatorString);
            string expression = SMTConstructsInstantiator.instantiateBinaryOperator(operatorString, leftSide.SmtExpression,
                       rightSide.SmtExpression);
            return new ParsingResult(expression);
        }

        private ParsingResult parseAGPathOperator(InternalNode agOperator, int lastUsedTimeIndex, int intervalDuration) {
            string operatorString = agOperator.getExpression();
            int myTimeIndex = lastUsedTimeIndex + 1;
            string result = "";            
            bool isBounded = operatorString.Contains("<");
            string quantifiedVariable = myTimeIndex <= 0 ? "time" : string.Format("t{0}", myTimeIndex);
            
            /*special case only for the outmost AG (if any)*/
            if (myTimeIndex < 1 && !isBounded)
            {
                ParsingResult innerParsingResult = parseNode(agOperator.getRightHandSide(), myTimeIndex, intervalDuration);
                intervalDuration = innerParsingResult.IntervalDuration;
                result = SMTConstructsInstantiator
                    .instantiateQuantifier("forall",
                    SMTConstructsInstantiator.instantiateUnaryOperator(quantifiedVariable, "Real"), innerParsingResult.SmtExpression);
            }
            else {
                ParsingResult innerExpressionParsingResult = parseNode(agOperator.getRightHandSide(), myTimeIndex, intervalDuration);
                string inmostExpression = innerExpressionParsingResult.SmtExpression;                
                intervalDuration = innerExpressionParsingResult.IntervalDuration;
                string negation = SMTConstructsInstantiator.instantiateUnaryOperator("not", inmostExpression);

                if (isBounded) {

                    if (intervalDuration == 0) {
                        intervalDuration = extractUpperBound(operatorString);
                    }
                    
                    List<string> agExpressions = new List <string>();
                    string lowerBoundVariable = "";
                    if (myTimeIndex <= 1) {
                        lowerBoundVariable = "time";
                    }
                    if (myTimeIndex > 1) {
                        lowerBoundVariable = string.Format("t{0}", lastUsedTimeIndex);
                    }
                    string upperBound = SMTConstructsInstantiator.instantiateBinaryOperator("<",
                                                                    quantifiedVariable,
                                                                    SMTConstructsInstantiator.instantiateBinaryOperator("+",
                                                                                lowerBoundVariable,
                                                                                extractUpperBound(operatorString).ToString()));                    
                    string lowerBound = SMTConstructsInstantiator.instantiateBinaryOperator(">", quantifiedVariable, lowerBoundVariable);
                    agExpressions.Add(lowerBound);
                    agExpressions.Add(upperBound);
                    agExpressions.Add(negation);
                    result = SMTConstructsInstantiator.instantiateUnaryOperator("not",
                        SMTConstructsInstantiator.instantiateQuantifier("exists", SMTConstructsInstantiator.instantiateUnaryOperator(quantifiedVariable, "Real"),
                            SMTConstructsInstantiator.instantiateNaryOperator("and", agExpressions.ToArray())
                        )
                      );
                     
                     //lowerBound, upperBound, newTimeIndex, negation));

                    /*SMTConstructsInstantiator.instantiateUnaryOperator("not",
                     SMTConstructsInstantiator.instantiateBoundedQuantifier("exists", lowerBound, upperBound, newTimeIndex, negation));*/
                }
                else {
                    if (intervalDuration <= 0 && innerExpressionParsingResult.IntervalDuration > 0) {
                        intervalDuration = 0;
                    }
                    result = SMTConstructsInstantiator.instantiateUnaryOperator("not",
                        SMTConstructsInstantiator.instantiateQuantifier("exists", SMTConstructsInstantiator.instantiateUnaryOperator(quantifiedVariable, "Real"), negation));
                        //SMTConstructsInstantiator.instantiateUnboundedQuantifier("exists", newTimeIndex, negation));
                }                       
            }

            return new ParsingResult(result, intervalDuration);
        }

        private ParsingResult parseAFPathOperator(InternalNode afOperator, int lastTimeIndex, int intervalDuration) {
            string operatorString = afOperator.getExpression();
            int myTimeIndex = lastTimeIndex + 1;
            bool isBounded = operatorString.Contains("<");
            string result = "";
            string quantifiedVariable = myTimeIndex < 1 ? "time" : string.Format("t{0}", myTimeIndex);
            string lowerBoundTimeVariable = "";
            if (myTimeIndex == 1) {
                lowerBoundTimeVariable = "time";
            }
            if (myTimeIndex > 1) {
                lowerBoundTimeVariable = string.Format("t{0}", myTimeIndex - 1);
            }
           
            List<string> afExpressions = new List<string>();
            string innerExpression = parseNode(afOperator.getRightHandSide(), myTimeIndex, intervalDuration).SmtExpression;
            string lowerBound = "";
            if (myTimeIndex > 0) {
                lowerBound = SMTConstructsInstantiator.instantiateBinaryOperator(">", quantifiedVariable, lowerBoundTimeVariable);
            }
             
            /*afExpressions[afExpressions.Length] = innerExpression;
            afExpressions[afExpressions.Length] = lowerBound;     
            */
            afExpressions.Add(innerExpression);
            afExpressions.Add(lowerBound);
            if (isBounded) {
                               
                string upperBound = SMTConstructsInstantiator.instantiateBinaryOperator("<", quantifiedVariable, SMTConstructsInstantiator.instantiateBinaryOperator("+",
                                                                lowerBoundTimeVariable,
                                                                extractUpperBound(operatorString).ToString()));

                afExpressions.Add(upperBound);
                //afExpressions[afExpressions.Length] = upperBound; 
            }
            result = SMTConstructsInstantiator.instantiateQuantifier("exists", SMTConstructsInstantiator.instantiateUnaryOperator(quantifiedVariable, "Real"),
                        SMTConstructsInstantiator.instantiateNaryOperator("and", afExpressions.ToArray()));

            return new ParsingResult(result, intervalDuration);
        }

        private ParsingResult parseUPathOperator(InternalNode uOperator, int timeIndex, int intervalDuration) {
            string operatorString = uOperator.getExpression().Trim();
            bool isBounded = operatorString.Contains("<");
            string result = "";
            
            
            /* Unitl operator encoded as SMT-LIB assertion
             * AG(p -> A(q U r))
             * (exists ((t1 Real)) (and (> t1 t) (< t1 (+ t k))
             * (r t1)
             * (not (exists ((t2 Real)) (and (>= t2 t) (< t2 t1) (not (q t2)))))))
             */

            string baseTimeVariable = timeIndex <= 0 ? "time" : string.Format("t{0}", timeIndex);
            string outerQuantifiedTimeVariable = string.Format("t{0}", (timeIndex) + 1);
            string innerQuantifiedTimeVariable = string.Format("t{0}", (timeIndex) + 2);
            
            
            string outerQLowerBound = SMTConstructsInstantiator.instantiateBinaryOperator(">", outerQuantifiedTimeVariable, intervalDuration <= 0 ? baseTimeVariable :
                                                                                            SMTConstructsInstantiator.instantiateBinaryOperator("+",
                                                                                                                            baseTimeVariable,
                                                                                                                            intervalDuration.ToString()));
            string outerQUpperBound = "";
            if (isBounded) {
                outerQUpperBound = SMTConstructsInstantiator.instantiateBinaryOperator("<",
                                                     outerQuantifiedTimeVariable,
                                                     SMTConstructsInstantiator.instantiateBinaryOperator("+",
                                                                                intervalDuration <= 0 ? baseTimeVariable : 
                                                                                                       SMTConstructsInstantiator
                                                                                                        .instantiateBinaryOperator("+", baseTimeVariable, 
                                                                                                        intervalDuration.ToString()),
                                                                                extractUpperBound(operatorString).ToString()));                  
            }

            string innerQLowerBound = SMTConstructsInstantiator.instantiateBinaryOperator(">", innerQuantifiedTimeVariable, 
                                                                                            intervalDuration <= 0 ? baseTimeVariable : 
                                                                                            SMTConstructsInstantiator.instantiateBinaryOperator("+", 
                                                                                                                            baseTimeVariable, 
                                                                                                                            intervalDuration.ToString()));
            string innerQUpperBound = SMTConstructsInstantiator.instantiateBinaryOperator("<", innerQuantifiedTimeVariable, outerQuantifiedTimeVariable);
            //string[] innerExistsExpressions = new string[] { };
            List<string> innerExistsExpressions = new List<string>();
            string innerExpression = SMTConstructsInstantiator.instantiateUnaryOperator("not",
                                                    parseNode(uOperator.getLeftHandSide(), timeIndex + 2, 0).SmtExpression);            
            innerExistsExpressions.Add(innerExpression);
            innerExistsExpressions.Add(innerQLowerBound);
            innerExistsExpressions.Add(innerQUpperBound);

            string innerExsists = SMTConstructsInstantiator.instantiateUnaryOperator("not",
                                        SMTConstructsInstantiator.instantiateQuantifier("exists", 
                                        SMTConstructsInstantiator.instantiateUnaryOperator(innerQuantifiedTimeVariable, "Real"), 
                                        SMTConstructsInstantiator.instantiateNaryOperator("and", innerExistsExpressions.ToArray())));


            //string[] outerExistsExpressions = new string[] { };
            List<string> outerExistsExpressions = new List<string>();
            /*outerExistsExpressions[outerExistsExpressions.Length] = innerExsists;
            outerExistsExpressions[outerExistsExpressions.Length] = outerQLowerBound;*/
            outerExistsExpressions.Add(innerExsists);
            Node rhsNode = uOperator.getRightHandSide();
            string rightHandSide = parseNode(uOperator.getRightHandSide(), timeIndex+1, 0).SmtExpression;
            outerExistsExpressions.Add(rightHandSide);
            outerExistsExpressions.Add(outerQLowerBound);
            outerExistsExpressions.Add(outerQUpperBound);



            result = SMTConstructsInstantiator.instantiateQuantifier("exists",
                                                SMTConstructsInstantiator.instantiateUnaryOperator(outerQuantifiedTimeVariable, "Real"),
                                                SMTConstructsInstantiator.instantiateNaryOperator("and", outerExistsExpressions.ToArray())
                                                );
            if (timeIndex < 0) {
                result = SMTConstructsInstantiator.instantiateQuantifier("forall", SMTConstructsInstantiator.instantiateUnaryOperator("time", "Real"), result);
            }
            return new ParsingResult(result);
        }

        private ParsingResult parsePathOperator(InternalNode pathOperator, int timeIndex, int intervalDuration)
        {
            /*path operator is unary node in the tree structure, 
             * which means that the next expression for parsing is
             * always in the right-hand-side node
             */
            ParsingResult result = new ParsingResult();
            string operatorString = pathOperator.getExpression().Trim();

            /*A is ignored (it is kind of implicit).
             * it is an unary operator, so only right hand size shall be processed 
             */
            if (operatorString.Equals("A"))
            {              
                result = parseNode(pathOperator.getRightHandSide(), timeIndex, intervalDuration);
            }
            else if (operatorString.StartsWith("AG")){
                result = parseAGPathOperator(pathOperator, timeIndex, intervalDuration);
                string a = "";
            }
            else if (operatorString.StartsWith("AF")) {
                result = parseAFPathOperator(pathOperator, timeIndex, intervalDuration);
            }
            else if (operatorString.StartsWith("U")) {
                result = parseUPathOperator(pathOperator, timeIndex, intervalDuration);
            }          
            
            return result;
        }

        private ParsingResult parseOperator(InternalNode _operator, int timeIndex, int intervalDuration)
        {
            string operatorString = _operator.getExpression().Trim();
            string operatorType = determineOperatorType(_operator);
            ParsingResult result = new ParsingResult();
            switch (operatorType) {
                case "unary":                    
                    result = parseUniaryOperator(_operator, timeIndex, intervalDuration);
                    break;
                case "binary":
                    result = parseBinaryOperator(_operator, timeIndex, intervalDuration);
                    break;
                case "path":
                    result = parsePathOperator(_operator, timeIndex, intervalDuration);
                    break;
            }
            return result;
        }

        private ParsingResult parseNode(Node node, int timeIndex, int intervalDuration) {
            string result = "";
            if (node is LeafNode)
            {
                result = parseLeaf(node, timeIndex, intervalDuration).SmtExpression;
            }
            else if (node is InternalNode) {
                InternalNode _opNode = node as InternalNode;
                ParsingResult pr = parseOperator(_opNode, timeIndex, intervalDuration);
                result = pr.SmtExpression;
                intervalDuration = pr.IntervalDuration;
            }
            return new ParsingResult(result, intervalDuration);
        }

        private ITransformedPropertySpecification transformFormula(IFormula formula)
        {            
            string assertion = parseNode(formula.getRoot(), -1, 0).SmtExpression;
            ICollection<IDeclaration> declarations = generateFormulaDeclarations(formula);
            string propertyId = formula.getId();
            ITransformedPropertySpecification transformedProperty = new TransformedPropertySpecification(declarations, propertyId, assertion);
            return transformedProperty;
        }

        public ITransformedPropertySpecification transform(IPropertySpecification property)
        {
            IFormula formulaForTransformation = expressionParser.parseExpression(property.getFormalRepresentation(), property.getPropertyId());
            return transformFormula(formulaForTransformation);
        }
    }
}
