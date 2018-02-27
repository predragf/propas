using SMTLibReq.Parsers.BaseExpression.Interfaces;

namespace SMTLibReq.Parsers.BaseExpression
{
    public class Node : INode
    {
        protected string expression;

        public Node() {
            expression = "";
        }

        public Node(Node node) {
            expression = node.getExpression();
        }

        public Node(string _expression) {
            expression = _expression;
        }

        public string getExpression() {
            return expression;
        }
    }
}
