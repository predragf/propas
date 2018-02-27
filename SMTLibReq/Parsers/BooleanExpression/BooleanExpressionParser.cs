using SMTLibReq.Parsers.BaseExpression;

namespace SMTLibReq.Parsers.BooleanExpression
{
    public class BooleanExpressionParser : BaseExpressionParser
    {
        private static string[] operators = new string[] { "=>", @"\|\|", "&&", "!", "==", "<>" };
        private static string searchTerm = @"\(([^\(\)]*)\)";

        public BooleanExpressionParser() : base(operators, searchTerm) {
        }

    }
       
}
