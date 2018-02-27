using SMTLibReq.Parsers.BaseExpression;
using SMTLibReq.Parsers.BaseExpression.Interfaces;

namespace SMTLibReq.Parsers.TCTLExpression
{
    public class TCTLExpressionParser : BaseExpressionParser {
        private static string[] operators = new string[] { @"AG((<=|<|>=|>)\d+)?", @"AF((<=|<|>=|>)\d+)?", "A", @"\s+U((<=|<|>=|>)\d+)?\s+", @"\s+W((<=|<|>=|>)\d+)?\s+", "=>", "||", "&&", "!", "==", "<>", @"\s+\s", @"\s-\s", @"\s\*\s", @"\s\/\s" };
        //private static string searchTerm = @"(AG|AF|A)((<=|<|>=|>)\d+)?\(([^\(\)]*)\)";
        private static string searchTerm = @"((AG|AF|A)((<=|<|>=|>)\d+)?)?\(([^\(\)]*)\)";

        public TCTLExpressionParser() : base(operators, searchTerm) {

        }


        private Node parseNode(Node n) {
            Node outNode = n;
            if (n is InternalNode)
            {
                InternalNode opNode = outNode as InternalNode;
                string expression = opNode.getExpression();
                if (expression.Equals("<>"))
                {
                    opNode = new InternalNode("==", parseNode(opNode.getLeftHandSide()), parseNode(opNode.getRightHandSide()));
                    outNode = new InternalNode("!", new LeafNode(), opNode);
                }
                else {
                    outNode = new InternalNode(opNode.getExpression(), parseNode(opNode.getLeftHandSide()), parseNode(opNode.getRightHandSide()));
                }        
            }
            return outNode;
        }


        private IFormula parseTCTLFormula(IFormula input) {
            return new Formula(parseNode(input.getRoot()), input.getId());
        }

        public override IFormula parseExpression(string expression, string expressionId)
        {
            IFormula originalTree = base.parseExpression(expression, expressionId);
            IFormula parsedTree = parseTCTLFormula(originalTree);
            return parsedTree;
           
        }


    }
}
