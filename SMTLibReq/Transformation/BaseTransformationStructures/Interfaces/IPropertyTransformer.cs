using SMTLibReq.DataStructures;
using SMTLibReq.Parsers.BaseExpression;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.BaseTransformationStructures.Interfaces
{
    public interface IPropertyTransformer
    {
        ITransformedPropertySpecification transform(IPropertySpecification property);
    }
}
