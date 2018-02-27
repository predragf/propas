using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.BaseTransformationStructures.Interfaces
{
    public interface ITransformedPropertySpecification
    {
        ICollection<IDeclaration> getDeclarations();

        void addDeclaration(IDeclaration declaration);

        string getProperty();

        string getPropertyId();

    }
}
