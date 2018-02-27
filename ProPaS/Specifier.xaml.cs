
using ProPaS.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProPaS
{
    /// <summary>
    /// Interaction logic for Specifier.xaml
    /// </summary>
    //
    public partial class Specifier : Window, INotifyPropertyChanged
    {
        private string activeDocumentId;
        private string activeEntryId;
        private Document activeDocument;
        private DocumentEntry activeEntry;
        private string specificationLocation = "";

        public event PropertyChangedEventHandler PropertyChanged;

        private string testing;
        
        public string Testing {
            get { return testing; }
            set
            {
                if (value != testing)
                {
                    testing = value;
                    System.Diagnostics.Debug.WriteLine(testing);
                    System.Windows.MessageBox.Show(testing);
                }
            }

        }
        public Specifier()
        {
            InitializeComponent();
            testing = "Predrag";
            DataContext = this;
        }

        private List<string> getAvailableSpecifications() {
            List<string> availableSpecifications = new List<string>();
            string[] fileEntries = Directory.GetFiles(specificationLocation, "*.xml");
            availableSpecifications.AddRange(fileEntries);           
            return availableSpecifications;
        }

        private void updateBindings() {

        }
    }
}
