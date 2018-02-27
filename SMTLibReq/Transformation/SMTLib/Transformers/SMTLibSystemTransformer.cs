using System.Collections.Generic;
using SMTLibReq.DataStructures;
using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using SMTLibReq.Transformation.SMTLib.Transformers.Interfaces;
using SMTLibReq.Transformation.SMTLib.DataStructures;
using SMTLibReq.Parsers.TCTLExpression;
using SMTLibReq.Parsers.BaseExpression;

namespace SMTLibReq.Transformation.SMTLib.Transformers
{
    public class SMTLibSystemTransformer : ISMTLibTransformer
    {
        public ITransformedSystemSpecification transform(ISystemSpecification systemSpecification)
        {
            ITransformedSystemSpecification transformedSystemSpecification = new SMTLibScript();
            BaseExpressionParser expressionParser = new TCTLExpressionParser();
            IPropertyTransformer propertyTransformer = new SMTPropertyTransformer(expressionParser);
            ITransformedPropertySpecification transformedProp;
            foreach (IPropertySpecification prop in systemSpecification.getProperties()) {
                transformedProp = propertyTransformer.transform(prop);
                transformedSystemSpecification.addProperty(transformedProp);
            }

            return transformedSystemSpecification;
        }
    }
}
