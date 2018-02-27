using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Parsers.BaseExpression.Interfaces
{
    public interface IExpressionParser
    {
        IFormula parseExpression(string expression, string expressionId);
    }
}
