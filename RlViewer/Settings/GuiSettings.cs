using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace RlViewer.Settings
{
    public class GuiSettings : XmlSerialized
    {
        protected override string SavingPath
        {
            get
            {
                var settingsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "guiSettings");
                var fileName = Path.ChangeExtension(settingsPath, SavingExtension);
                return fileName;
            }
        }

        private bool _useCustomFileOpenDlg = false;

        [DataMember(IsRequired = true)]
        public bool UseCustomFileOpenDlg
        {
            get { return _useCustomFileOpenDlg; }
            set { _useCustomFileOpenDlg = value; }
        }

        private bool _showRhgSynthesisHeaderParamsTab = false;

        [DataMember(IsRequired = true)]
        public bool ShowRhgSynthesisHeaderParamsTab
        {
            get { return _showRhgSynthesisHeaderParamsTab; }
            set { _showRhgSynthesisHeaderParamsTab = value; }
        }


        private bool _showServerSar = false;

        [DataMember(IsRequired = true)]
        public bool ShowServerSar
        {
            get { return _showServerSar; }
            set { _showServerSar = value; }
        }

    }
}
