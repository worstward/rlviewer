using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace RlViewer.Settings
{

    [DataContract]
    class SynthesisSettings : Settings
    {
        protected override string SettingsPath
        {
            get
            {
                var settingsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "synthesisSettings");
                var fileName = Path.ChangeExtension(settingsPath, SettingsExtension);
                return fileName;
            }
        }

    }
}
