using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Parsers.BaseExpression.Interfaces
{
    public interface IFormula
    {
        int getDepth();

        Node getRoot();

        ICollection<Node> getAtomicPropositionsOnLevel(int level);

        ICollection<Node> getLeafNodes();

        string getId();
    }
}
