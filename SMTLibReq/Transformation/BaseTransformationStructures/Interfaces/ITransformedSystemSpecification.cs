using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.BaseTransformationStructures.Interfaces
{
    public interface ITransformedSystemSpecification
    {
        ICollection<IDeclaration> getDeclarations();

        void addDeclaration(IDeclaration declaration);

        void addDeclarations(ICollection<IDeclaration> declarations);

        ICollection<ITransformedPropertySpecification> getProperties();

        void addProperty(ITransformedPropertySpecification property);

        void addProperties(ICollection<ITransformedPropertySpecification> properties);

        string generateExecutable();
        
    }
}
