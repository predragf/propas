using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.DataStructures.Implementation
{
    public class SystemSpecification : ISystemSpecification
    {
        private string formalism;
        private ICollection<IPropertySpecification> properties;

        private SystemSpecification() {
            formalism = "";
            properties = new List<IPropertySpecification>();
        }

        public SystemSpecification(string _fomalismName)
        {
            formalism = string.IsNullOrEmpty(_fomalismName) ? "" : _fomalismName;
            properties = new List<IPropertySpecification>();
        }

        public string getFormalismName() {
            return formalism;
        }

        public ICollection<IPropertySpecification> getProperties() {
            return properties;
        }

        public void addOrReplaceProperty(IPropertySpecification property) {
            IPropertySpecification existing = null;
            foreach (IPropertySpecification prop in properties) {
                if (prop.getPropertyId().Equals(property.getPropertyId())) {
                    existing = prop;
                }
            }
            if (existing != null) {
                properties.Remove(existing);
            }
            properties.Add(property);
        }
    }
}
