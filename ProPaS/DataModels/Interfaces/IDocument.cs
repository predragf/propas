using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProPaS.DataModels.Interfaces
{
    public interface IDocument
    {
        string getDocumentName();

        void setDocumentName(string newName);

        string getFormalism();

        void setFormalism(string newFormalism);

        void addEntry(IDocumentEntry property);

        IDocumentEntry getEntryById(string entryId);

        ICollection<IDocumentEntry> getAllEntries();
    }
}
