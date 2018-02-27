using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.SMTLib.Helpers
{
    public class SMTConstructsTemplateGenerator
    {
        public static string generateNaryOperatorTemplate() {
            return "(#operator# #expression#)";
        }

        public static string generateQuantifierTemplate() {
            return "(#quantifier# (#quantifiedVariables#) #expression#)";
        }

        public static string generateNonLabeledAssertionTemplate() {
            return "(assert #constraint#)";
        }

        public static string generateLabeledAssertionTemplate()
        {            
            return "(assert #constraint# :named #assertionId#)";
        }

        public static string generateFunctionDeclarationTemplate() {
            return "(declare-fun #functionName# (#inputs#) #output#)";
        }

        public static string generateConstantDeclarationTemplate() {
            return "(declare-const #constantName# () #type#)";
        }
    }
}
