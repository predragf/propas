using SMTLibReq.Transformation.SMTLib.DataStructures.Interfaces;
using System.Collections.Generic;
using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;

namespace SMTLibReq.Transformation.SMTLib.DataStructures
{
    public class SMTLibScript : ISMTLibScript
    {
        private List<string> header;
        private List<IDeclaration> declarations;
        private List<ITransformedPropertySpecification> constraints;
        private List<string> executionDirectives;

        public SMTLibScript() {
            header = new List<string>();
            declarations = new List<IDeclaration>();
            constraints = new List<ITransformedPropertySpecification>();
            executionDirectives = new List<string>();
        }

        private void addAssertion(string assertion) {
        }

        public void addDeclaration(IDeclaration declaration)
        {
            if (!(declaration is null)) {
                string declaredParameterName = declaration.getName();
                if (!string.IsNullOrWhiteSpace(declaredParameterName)) {
                    declarations.RemoveAll(dec => dec.getName().Equals(declaredParameterName));
                    declarations.Add(declaration);
                 }
            }
        }

        public void addDeclarations(ICollection<IDeclaration> declarations)
        {
            if (declarations is null) {
                return;
            }

            foreach (IDeclaration declaration in declarations)
            {
                addDeclaration(declaration);
            }
        }

        public void addExecutionDirective(string executionDirective)
        {
            if (!(executionDirectives is null) 
                    && !string.IsNullOrWhiteSpace(executionDirective)
                    && executionDirectives.Contains(executionDirective)) {
                executionDirectives.Add(executionDirective);
            }
        }

        public void addHeader(string headerLine) {
            if (!(header is null) 
                    && !string.IsNullOrWhiteSpace(headerLine)
                    && !header.Contains(headerLine)) {
                header.Add(headerLine);
            }
        }

        public void addProperty(ITransformedPropertySpecification property)
        {
            if (!(constraints is null) 
                    && !(property is null)) {
                string newPropertyId = string.IsNullOrWhiteSpace(property.getPropertyId()) ? "" : property.getPropertyId();
                if (!string.IsNullOrWhiteSpace(newPropertyId)) {
                    constraints.RemoveAll(prop => prop.getPropertyId().Equals(newPropertyId));                
                    constraints.Add(property);
                }                
            }

            if (property != null) {
                addDeclarations(property.getDeclarations());
            }
        }

        public void addProperties(ICollection<ITransformedPropertySpecification> properties) {
            if (properties != null) {
                foreach (ITransformedPropertySpecification prop in properties) {
                    addProperty(prop);
                }
            }
        }

        public string generateExecutable()
        {
            return "";
        }

        public ICollection<IDeclaration> getDeclarations()
        {
            return declarations;
        }

        public ICollection<string> getExectionDirectives()
        {
            return executionDirectives;
        }

        public ICollection<string> getHeader()
        {
            return header;
        }

        public ICollection<ITransformedPropertySpecification> getProperties()
        {
            return constraints;
        }

        public void addExecutionDirectives(ICollection<string> eDirectives)
        {
            if (eDirectives is null) {
                return;
            }
            foreach (string eDirective in eDirectives) {
                addExecutionDirective(eDirective);
            }
        }

    }
}
