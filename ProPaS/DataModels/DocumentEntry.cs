using ProPaS.DataModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProPaS.DataModels
{
    [Serializable]
    public class DocumentEntry : IDocumentEntry
    {
        [XmlElementAttribute(IsNullable = false)]
        public string entryId;
        [XmlElementAttribute(IsNullable = false)]
        public string formalEncoding;
        [XmlElementAttribute(IsNullable = false)]
        public string cnlEncoding;

        public DocumentEntry() {
            entryId = "";
            formalEncoding = "";
            cnlEncoding = "";
        }

        public DocumentEntry(string _entryId, string _formalEncoding, string _cnlEncoding) {
            entryId = _entryId;
            formalEncoding = _formalEncoding;
            cnlEncoding = _cnlEncoding;
        }

        public string getCNLEncoding()
        {
            return cnlEncoding;
        }

        public string getEntryId()
        {
            return entryId;
        }

        public void setEntryId(string _entryId)
        {
            entryId = _entryId;
        }

        public string getFormalEncoding()
        {
            return formalEncoding;
        }

        public void setCNLEncoding(string _cnlEncoding)
        {
            cnlEncoding = _cnlEncoding;
        }

        public void setFormalEncoding(string _formalEncoding)
        {
            formalEncoding = _formalEncoding;
        }
    }
}
