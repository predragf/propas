using ProPaS.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProPaS.ViewModels
{
    public class SpecifierViewModel
    {
        public List<Document> specifications;
        public List<DocumentEntry> requirements;
        public Document activeSpeccification;
        public DocumentEntry activeRequirement;

        public SpecifierViewModel() {
            specifications = new List<Document>();
            requirements = new List<DocumentEntry>();
            activeSpeccification = new Document();
            activeRequirement = new DocumentEntry();
        }

        public SpecifierViewModel(List<Document> _documents)
        {
            specifications = _documents;
            activeSpeccification = _documents.ElementAt(0);
            requirements = (List<DocumentEntry>)activeSpeccification.getAllEntries();
            activeRequirement = requirements.ElementAt(0);
        }

        void rebind(string newActiveDocumentName, string newActiveRequirementId) {
            if (!activeSpeccification.getDocumentName().Equals(newActiveDocumentName))
            {
                activeSpeccification = specifications.Find(spec => spec.getDocumentName().Equals(newActiveDocumentName));
            }
            else {

            }
        }
    }
}
