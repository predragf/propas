using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.DataStructures
{
    public interface ISystemSpecification
    {
        string getFormalismName();

        ICollection<IPropertySpecification> getProperties();

        void addOrReplaceProperty(IPropertySpecification propertySpecification);
    }
}
