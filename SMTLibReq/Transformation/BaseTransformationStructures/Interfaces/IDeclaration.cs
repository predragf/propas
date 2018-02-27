using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.BaseTransformationStructures.Interfaces
{
    public interface IDeclaration
    {
        string getType();

        string getName();

        string getDefaultValue();

        ICollection<string> getInputParameters();

        string toString();
    }
}
