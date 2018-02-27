using SMTLibReq.Parsers.BaseExpression.Interfaces;

namespace SMTLibReq.Parsers.BaseExpression
{
    public class InternalNode : Node {
        private Node left;
        private Node right;

        private InternalNode() {
        }

        public InternalNode(string expression, Node leftHandSide, Node rightHandSide): base(expression) {
            left = leftHandSide;
            right = rightHandSide;
        }

        public Node getLeftHandSide() {
            return left;
        }

        public Node getRightHandSide() {
            return right;
        }

        public bool hasLeft() {
            return left != null;
        }

        public bool hasRight() {
            return right != null;
        }
    }
}
