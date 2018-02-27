using SMTLibReq.Transformation.BaseTransformationStructures;
using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SMTLibReq.Transformation.SMTLib.Helpers
{
    public class DeclarationGenerator
    {
     

        private static IDeclaration generatePropositionDeclaration(string proposition) {

            string name = PropositionParser.extractName(proposition);
            ICollection<string> functionParameters = PropositionParser.extractParameters(proposition);
            string returnType = "Real";
            string value = "";

            return new Declaration(returnType, name, value, functionParameters);
        }

        private static ICollection<IDeclaration> generateDependencyDeclarations(ICollection<string> functionParameters) {
            List<IDeclaration> dependencyDeclarations = new List<IDeclaration>();
            Declaration dec;
            foreach (string param in functionParameters) {
                dec = new Declaration("Real", param.Trim(), "", null);
                dependencyDeclarations.Add(dec);            
            }
            return dependencyDeclarations;
        }

        private static ICollection<IDeclaration> generateAllDeclarations(string proposition)
        {
            List<IDeclaration> declarations = new List<IDeclaration>();
            IDeclaration mainDec = generatePropositionDeclaration(proposition);
            declarations.Add(mainDec);
            if (mainDec.getInputParameters() != null)
            {
                declarations.AddRange(generateDependencyDeclarations(mainDec.getInputParameters()));
            }
            return declarations;
        }

        public static ICollection<IDeclaration> generateDeclarations(string proposition) {
            ICollection<IDeclaration> declarations = new List<IDeclaration>();
            if (!PropositionParser.isNonDeclarableType(proposition)) {
                declarations = generateAllDeclarations(proposition);
            }
            return declarations;
        }
    }
}
