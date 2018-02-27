using SMTLibReq.Parsers.BaseExpression.Interfaces;
using System.Collections.Generic;

namespace SMTLibReq.Parsers.BaseExpression
{
    public class Formula : IFormula
    {
        private Node root;
        private string Id;

        public Formula(Node _root, string formulaId)
        {
            root = _root;
            Id = formulaId;
        }

        private int calculateDepth(Node rootNode, int depth)
        {
            if (rootNode == null || rootNode is LeafNode)
            {
                depth = depth > 0 ? depth - 1 : depth;
            }
            else if (rootNode is InternalNode)
            {
                InternalNode opNode = (InternalNode)rootNode;
                depth = calculateDepth(opNode.getLeftHandSide(), depth + 1);
                int rhsDepth = calculateDepth(opNode.getRightHandSide(), depth + 1);
                if (rhsDepth > depth)
                {
                    depth = rhsDepth;
                }
            }
            return depth;
        }

        public int getDepth()
        {
            return calculateDepth(root, 0);
        }

        public Node getRoot()
        {
            return root;
        }

        private ICollection<Node> recursiveTraversal(int currentLevel, int requestedLevel, Node currentNode)
        {
            List<Node> result = new List<Node>();
            if (currentNode is InternalNode)
            {
                InternalNode currentOperator = (InternalNode)currentNode;
                if (currentLevel <= requestedLevel)
                {
                    result.AddRange(recursiveTraversal(currentLevel + 1, requestedLevel, currentOperator.getLeftHandSide()));
                    result.AddRange(recursiveTraversal(currentLevel + 1, requestedLevel, currentOperator.getRightHandSide()));
                }
            }
            else if (currentNode is LeafNode)
            {
                if (currentLevel == 0 || currentLevel == requestedLevel + 1)
                {
                    result.Add(currentNode);
                }
            }


            return result;
        }

        public ICollection<Node> getAtomicPropositionsOnLevel(int level)
        {
            return recursiveTraversal(0, level, root);
        }

        private ICollection<Node> getLeafsRecursively(Node node) {
            List<Node> result = new List<Node>();
            if (node is LeafNode)  {
                if (!string.IsNullOrWhiteSpace(node.getExpression())) { 
                result.Add(node);
                }
            }
            else if(node is InternalNode) {
                InternalNode operatorNode = (InternalNode)node;
                result.AddRange(getLeafsRecursively(operatorNode.getLeftHandSide()));
                result.AddRange(getLeafsRecursively(operatorNode.getRightHandSide()));
            }
            return result;
        }

        public ICollection<Node> getLeafNodes()
        {
            return getLeafsRecursively(root);
        }

        public string getId()
        {
            return Id;
        }

    }
}
