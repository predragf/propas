using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.DataStructures.Implementation
{
    public class PropertySpecification : IPropertySpecification
    {
        private string propertyId;
        private string formalRepresentation;
        private string cnlRepresentation;

        public PropertySpecification() {
            propertyId = "";
            formalRepresentation = "";
            cnlRepresentation = "";
        }

        public PropertySpecification(string _propertyId, string _formalRepresentation, string _cnlRepresentation) {
            propertyId = _propertyId;
            formalRepresentation = _formalRepresentation;
            cnlRepresentation = _cnlRepresentation;            
        }

        public string getCNLRepresentation() {
            return cnlRepresentation;
        }

        public string getFormalRepresentation() {
            return formalRepresentation;
        }

        public string getPropertyId() {
            return propertyId;
        }

    }
}
