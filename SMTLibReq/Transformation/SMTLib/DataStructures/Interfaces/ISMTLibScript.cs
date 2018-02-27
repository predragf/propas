using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using System.Collections.Generic;

namespace SMTLibReq.Transformation.SMTLib.DataStructures.Interfaces
{
    public interface ISMTLibScript : ITransformedSystemSpecification
    {
        void addHeader(string headerLine);        

        void addExecutionDirective(string executionDirective);

        void addExecutionDirectives(ICollection<string> executionDirectives);

        ICollection<string> getExectionDirectives();

        ICollection<string> getHeader();
    }
}
