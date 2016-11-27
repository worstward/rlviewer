using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace RlViewer.Behaviors.RecentOpened
{

    [DataContract]
    class RecentFiles : XmlSerializable
    {
        private Stack<string> _recentFiles = new Stack<string>();

        [DataMember(IsRequired = true)]
        public Stack<string> RecentOpenedFiles
        {
            get { return _recentFiles; }
            set { _recentFiles = value; }
        }

        protected override string SavingPath
        {
            get
            {
                var savingPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "recentFiles");
                var fileName = Path.ChangeExtension(savingPath, SavingExtension);
                return fileName;
            }
        }
    }
}
