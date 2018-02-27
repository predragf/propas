using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using SMTLibReq.Transformation.SMTLib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.BaseTransformationStructures
{
    public class Declaration : IDeclaration
    {
        private string type;
        private string name;
        /*value is given if it is constant. No use for now.*/
        private string value;
        private ICollection<string> inputParameters;

        public Declaration(string _type, string _name, string _value, ICollection<string> _inputParameters) {
            type = !string.IsNullOrEmpty(_type) ? _type : "";
            name = !string.IsNullOrEmpty(_name) ? _name : "";
            value = !string.IsNullOrEmpty(_value) ? _value : "";
            inputParameters = !(_inputParameters is null) ? _inputParameters : new List<string>();
        }

        public string getDefaultValue()
        {
            return value;
        }

        public string getName()
        {
            return name;
        }

        public ICollection<string> getInputParameters()
        {
            return inputParameters;
        }

        public string getType()
        {
            return type;
        }

        private string generateConstantDeclaration() {            
            return SMTConstructsInstantiator.instantiateFunctionDeclaration(name, "Real", "Real");
        }

        private string generateFunctionDeclaration() {   
            /*first real is always for the time variable, and rest are the current parameters*/
            string inputs = "Real ";
            foreach (string param in inputParameters) {
                inputs += "Real ";
            }
            return SMTConstructsInstantiator.instantiateFunctionDeclaration(name, inputs.Trim(), "Real");
        }

        public string toString()
        {
            string declaration = "";
            if (inputParameters == null || (inputParameters != null && inputParameters.Count < 1))
            {
                declaration = generateConstantDeclaration();
            }
            else if ((inputParameters != null && inputParameters.Count > 0)) {
                declaration = generateFunctionDeclaration();
            }
            return declaration;
        }
    }
}
