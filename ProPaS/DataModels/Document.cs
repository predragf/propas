using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProPaS.DataModels.Interfaces;

namespace ProPaS.DataModels
{
    [Serializable]
    [XmlInclude(typeof(Document))]
    [XmlRootAttribute("Document")]
    public class Document : IDocument
    {
        [XmlElementAttribute(IsNullable = false)]
        public string documentName;
        [XmlElementAttribute(IsNullable = false)]
        public string formalism;
        [XmlArrayAttribute("Entries")]
        public List<DocumentEntry> entries;
        [XmlElementAttribute(IsNullable = false)]
        public string documentId;

        public Document() {
            documentName = "";
            formalism = "";
            entries = new List<DocumentEntry>();
        }

        public Document(string docName, string formalismName) {
            documentName = docName;
            formalism = formalismName;
            entries = new List<DocumentEntry>();
            documentId = Guid.NewGuid().ToString();
        }

        public Document(string docName, string formalismName, ICollection<IDocumentEntry> _entries)
        {
            documentName = docName;
            formalism = formalismName;
            entries = new List<DocumentEntry>();
            foreach (IDocumentEntry iEntry in _entries){
                DocumentEntry dEntry = (DocumentEntry)iEntry;
                entries.Add(dEntry);
            }
            documentId = Guid.NewGuid().ToString();

        }

        public void addEntry(IDocumentEntry entry)
        {
            entries.RemoveAll(ent => ent.getEntryId().Equals(entry.getEntryId()));
            DocumentEntry dEntry = (DocumentEntry)entry;
            entries.Add(dEntry);
        }

        public string getDocumentName()
        {
            return documentName;
        }

        public void setDocumentName(string newName)
        {
            documentName = newName;
        }

        public IDocumentEntry getEntryById(string entryId)
        {
            return entries.Find(ent => ent.getEntryId().Equals(entryId));
        }

        public string getFormalism()
        {
            return formalism;
        }

        public void setFormalism(string newFormalism)
        {
            formalism = newFormalism;
        }

        public ICollection<IDocumentEntry> getAllEntries()
        {
            return (ICollection<IDocumentEntry>)entries;
        }
    }
}
