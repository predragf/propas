using SMTLibReq.Parsers.BaseExpression.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SMTLibReq.Parsers.BaseExpression
{
    public class BaseExpressionParser : IExpressionParser  {
        private string[] operators;
        private string searchTerm;

        public BaseExpressionParser() {
            operators = new string[] { };
            searchTerm = "";
        }

        public BaseExpressionParser(string[] _operators, string _searchTerm) {
            operators = _operators;
            searchTerm = _searchTerm;
        }

        private string[] divideExrepssionByOperator(string expression, string _operator)
        {
            /*for binary operators, both lhs and rhs will be non empty strings
             * for unary on the other hand, the lhs shall always be empty (null)
             * which means that the expression is always in the rhs node 
             * (ex: negation, and all path-specific temporal operators).
             */
            int lastIndexOfOperator = expression.LastIndexOf(_operator);
            string[] expressions = new string[] { "", "" };
            if (lastIndexOfOperator >= 0) {
                string lhsExpression = expression.Substring(0, lastIndexOfOperator).Trim();
                string rhsExpression = expression.Substring(lastIndexOfOperator + _operator.Length, 
                                                            expression.Length - (lastIndexOfOperator + _operator.Length)
                                                            ).Trim();
                expressions = new string[] { lhsExpression, rhsExpression };
            }
            return expressions;
        }

        private Match getLastMatchByOperator(string expression, string _operator)
        {
            Match result = null;
            Regex searchExpression = new Regex(_operator);
            var matches = searchExpression.Matches(expression);

            foreach (Match match in matches) {
                if (result is null) {
                    if (!String.IsNullOrEmpty(match.Value)) {
                        result = match;
                    }                    
                }
                else if (match.Index > result.Index) {
                        result = match;
                    }
            }
            return result;
        }

        private string getNextOperatorForParsing(string _expression)  {                     
            int operatorIndex = 0;
            string operatorForParsing = "";
            Match matchedOperatorForParsing;
            while (operatorIndex < operators.Length)
            {
                string _operator = operators[operatorIndex];
                matchedOperatorForParsing = getLastMatchByOperator(_expression, _operator);
                if (!(matchedOperatorForParsing is null)) {
                    operatorForParsing = matchedOperatorForParsing.Value;
                    break;
                }
                operatorIndex++;
            }
            return operatorForParsing;
        }

        private Node parseLeaf(string _expression, Dictionary<string, string> mappings) {
            Node node;
            string replacedExpression;
            if (mappings.TryGetValue(_expression, out replacedExpression)) {
                node = parse(replacedExpression, mappings);
            } else {
                node = new LeafNode(_expression);
            }
            return node;
        }

        private Node parseInternalNode(string _expression, string nextOperatorForParsing, Dictionary<string, string> mappings) {

            /*put this into a separate function and override it.*/
            string[] expressions = divideExrepssionByOperator(_expression, nextOperatorForParsing);
            Node node;
            nextOperatorForParsing = nextOperatorForParsing.Trim();
            Node lhs = parse(expressions[0], mappings);
            Node rhs = parse(expressions[1], mappings);
            /*eliminate W node during parsing
             temporary hack. in the subsequent code refactoring move this piece of code
             as it is relevant only for temporal logics
             */
            if (nextOperatorForParsing.StartsWith("W"))
            {
                string substituteUntil = nextOperatorForParsing.Replace("W", "U");
                string substituteAG = nextOperatorForParsing.Replace("W", "AG");
                Node uNode = new InternalNode(substituteUntil, lhs, rhs);
                Node terminal = new LeafNode();
                Node agNode = new InternalNode(substituteAG, terminal, lhs);
                node = new InternalNode("||", uNode, agNode);

            }
            else
            {
                node = new InternalNode(nextOperatorForParsing, lhs, rhs);
            }
            return node;

        }

        private Node parse(string _expression, Dictionary<string, string> mappings)
        {
            //this casuses bug with functions
            //_expression = _expression.Replace("(", "").Replace(")", "");

            /*
             *new version. we assume that the number of parenthesis match, so if an
             * expression starts with a "(" it must end with ")".
             */
            if (_expression.StartsWith("(")) {
                _expression = _expression.Substring(1, _expression.Length - 2);
            }
            
            string nextOperatorForParsing = getNextOperatorForParsing(_expression);
            Node node = null;

            /*This is an atomic expression*/
            if (String.IsNullOrEmpty(nextOperatorForParsing)) {
                node = parseLeaf(_expression, mappings);
            }
            /*This is for operator node*/
            else  {
                node = parseInternalNode(_expression, nextOperatorForParsing, mappings);
            }
            return node;
        }

        public virtual IFormula parseExpression(string expression, string expressionId)
        {
            FlatExpression flatExpression = new FlatExpression(expression, searchTerm);
            Node rootNode = parse(flatExpression.getExpression(), flatExpression.getMappings());
            Formula baseFormula = new Formula(rootNode, expressionId);
            return baseFormula;
        }
    }
}
