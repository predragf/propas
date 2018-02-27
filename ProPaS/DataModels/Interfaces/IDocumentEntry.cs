using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProPaS.DataModels.Interfaces
{
    public interface IDocumentEntry
    {
        string getEntryId();

        void setEntryId(string _entryId);

        string getFormalEncoding();

        void setFormalEncoding(string _formalEncoding);

        string getCNLEncoding();

        void setCNLEncoding(string _cnlEncoding);
    }
}
