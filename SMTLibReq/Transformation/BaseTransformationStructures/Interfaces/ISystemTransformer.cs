using SMTLibReq.DataStructures;

namespace SMTLibReq.Transformation.BaseTransformationStructures.Interfaces
{
    public interface ISystemTransformer
    {
        ITransformedSystemSpecification transform(ISystemSpecification systemSpecification);
    }
}
