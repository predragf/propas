using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SMTLibReq.Transformation.SMTLib.Helpers
{
    public class PropositionParser
    {
        public static bool isNonDeclarableType(string variable)
        {
            /*if it is True, False or number then a declaration shall not 
             be generated for this proposition*/

            /*just to make sure that if the user uses . instead of , in 
             * the number declaration in the requirements, it is still fine. 
             */
            variable = variable.Replace(".", ",");
            List<string> constantNames = new List<string>() { "True", "False" };
            double number;
            return constantNames.Contains(variable) || Double.TryParse(variable, out number);
        }

        public static ICollection<string> extractParameters(string proposition)
        {
            string functionPattern = @"\(.*\)";

            List<string> parameters = new List<string>();
            Regex functionMatcher = new Regex(functionPattern);
            var match = functionMatcher.Match(proposition);

            if (match.Success)
            {
                string matchedPart = match.Value;
                matchedPart = matchedPart.Replace("(", "").Replace(")", "");
                parameters.AddRange(matchedPart.Split(','));
            }

            return parameters;
        }

        public static string extractName(string proposition)
        {
            string name = proposition;
            if (proposition.Contains("("))
            {
                name = proposition.Substring(0, proposition.IndexOf("("));
            }
            return name;
        }

        public static string trnasformOperatorToSMTLIB(string _operator) {
            string smtLibVersion = "";
            switch (_operator.Trim()) {                
                case "||":
                    smtLibVersion = "or";
                    break;
                case "&&":
                    smtLibVersion = "and";
                    break;
                case "!":
                    smtLibVersion = "not";
                    break;
                case "==":
                    smtLibVersion = "=";
                    break;
                case "<>": smtLibVersion = 
                        "!=";
                    break;
                default :
                    smtLibVersion = _operator;
                    break;
            }
            return smtLibVersion;
        }
    }
}
