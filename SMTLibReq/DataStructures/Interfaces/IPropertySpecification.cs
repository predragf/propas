using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.DataStructures
{
    public interface IPropertySpecification
    {
        string getPropertyId();

        string getFormalRepresentation();

        string getCNLRepresentation();
    }
}
