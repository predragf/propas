using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Parsers.BaseExpression.Interfaces
{
    public interface IFlatExpression
    {
        string getExpression();

        Dictionary<string, string> getMappings();
    }
}
