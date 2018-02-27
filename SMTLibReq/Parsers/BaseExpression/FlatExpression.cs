using SMTLibReq.Parsers.BaseExpression.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SMTLibReq.Parsers.BaseExpression
{
    public class FlatExpression : IFlatExpression
    {
        private Regex searchTerm;
        private string expression;
        private Dictionary<string, string> mappings;        

        private FlatExpression() {
            /*forbig empty construtctor as such type of flat expression is useless.*/
            expression = "";
            mappings = new Dictionary<string, string>();
            searchTerm = new Regex("");
        }

        public FlatExpression(string _expression, string _searchTerm) {
            initialize(_expression, _searchTerm);
            makeFlatExpression();
        }

        private void initialize(string _expression, string _searchTerm) {
            this.expression = !String.IsNullOrEmpty(_expression) ? _expression : "";
            mappings = new Dictionary<string, string>();
            searchTerm = new Regex(_searchTerm);
        }

        private void makeFlatExpression()  {            
            while (true) {
                var match = searchTerm.Match(expression);
                if (match.Success)  {
                    string key = $"++exp{mappings.Count + 1}++";
                    mappings.Add(key, match.Value);
                    expression = expression.Replace(match.Value, key);
                } else {
                    break;
                }
            }
            if (mappings.ContainsKey(expression)) {
                mappings.TryGetValue(expression, out expression);
            }
        }

        public string getExpression() {
            return expression;
        }

        public Dictionary<string, string> getMappings() {
            return mappings;
        }
    }
}
