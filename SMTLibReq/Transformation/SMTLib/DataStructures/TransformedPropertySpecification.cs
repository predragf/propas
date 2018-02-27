using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.SMTLib.DataStructures
{
    public class TransformedPropertySpecification : ITransformedPropertySpecification
    {
        private List<IDeclaration> declarations;
        private string propertyId;
        private string assertion;

        public TransformedPropertySpecification(ICollection<IDeclaration> _declarations, string _propertyId, string _assertion) {
            declarations = new List<IDeclaration>();
            if (_declarations != null) {
                declarations.AddRange(_declarations);
            }            
            propertyId = !string.IsNullOrEmpty(_propertyId) ? _propertyId : "";
            assertion = !string.IsNullOrEmpty(_assertion) ? _assertion : "";

        }

        public void addDeclaration(IDeclaration declaration)
        {
            if (declaration is null)
            {
                string declaredParameterName = declaration.getName();
                if (string.IsNullOrWhiteSpace(declaredParameterName))
                {
                    declarations.RemoveAll(dec => dec.getName().Equals(declaredParameterName));
                    declarations.Add(declaration);
                }
            }
        }

        public ICollection<IDeclaration> getDeclarations()
        {
            return declarations;
        }

        public string getPropertyId()
        {
            return propertyId;
        }

        public string getProperty()
        {
            return string.Format("(assert (! ({0}) :named {1}))", assertion, propertyId);
        }
    }
}
