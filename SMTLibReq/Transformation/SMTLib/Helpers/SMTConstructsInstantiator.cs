using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.SMTLib.Helpers
{
    public class SMTConstructsInstantiator
    {
        public static string instantiateNaryOperator(string _operator, string[] expressions)
        {
            string nAryTemplate = SMTConstructsTemplateGenerator.generateNaryOperatorTemplate();
            string expression = "";
            foreach (string exp in expressions)
            {
                if (!string.IsNullOrWhiteSpace(exp)) {
                    expression += exp + " ";
                }                
            }
            return nAryTemplate.Replace("#operator#", _operator)
                                .Replace("#expression#", expression.Trim());
        }

        /*specialization of the N-ary operator since it is widely used*/
        public static string instantiateUnaryOperator(string _operator, string exp)
        {
            string[] expressions = new string[] { exp };
            return instantiateNaryOperator(_operator, expressions);
        }

        /*specialization of the N-ary operator since it is widely used*/
        public static string instantiateBinaryOperator(string _operator, string expr1, string expr2){
            string[] expressions = new string[] { expr1, expr2 };
            return instantiateNaryOperator(_operator, expressions);
        }

        public static string instantiateQuantifier(string quantifier, string quantifiedVariables, string expression) {
            string quantifierTemplate = SMTConstructsTemplateGenerator.generateQuantifierTemplate();
            return quantifierTemplate.Replace("#quantifier#", quantifier)
                .Replace("#quantifiedVariables#", quantifiedVariables)
                .Replace("#expression#", expression);
        }

        private static string instantiateUnboundedQuantifier(string quantifier, int timeIndex, string expression)
        {
            string quantifiedVariable = "time";
            if (timeIndex > 0) {
                quantifiedVariable = "t" + timeIndex;
            }     
            string template = SMTConstructsTemplateGenerator.generateQuantifierTemplate();
            string templateInstance = template.Replace("#quantifier#", quantifier);
            templateInstance = templateInstance.Replace("#quantifiedVariable#", quantifiedVariable);
            templateInstance = templateInstance.Replace("#expression#", expression);
            return templateInstance;
        }

        private static string instantiateBoundedQuantifier(string quantifier, string lowerBound, string upperBound, int timeIndex, string expression)
        {
            return null;
        }

        public static string instantiateConstantDeclaration(string constantName, string constantType) {
            string template = SMTConstructsTemplateGenerator.generateConstantDeclarationTemplate();
            string templateInstance = template.Replace("#constantName#", constantName);
            templateInstance = templateInstance.Replace("#type#", constantType);
            return templateInstance;
        }

        public static string instantiateFunctionDeclaration(string functionName, string inputs, string output)  {
            string template = SMTConstructsTemplateGenerator.generateFunctionDeclarationTemplate();
            string templateInstance = template.Replace("#functionName#", functionName);
            templateInstance = templateInstance.Replace("#inputs#", inputs);
            templateInstance = templateInstance.Replace("#output#", output);
            return templateInstance;
        }

        public static string instantiateAssertion(string expression) {
            string template = SMTConstructsTemplateGenerator.generateNonLabeledAssertionTemplate();
            return template.Replace("#constraint#", expression);
        }



    }
}
