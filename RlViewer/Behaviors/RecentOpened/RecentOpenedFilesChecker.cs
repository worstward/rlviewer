using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RlViewer.Behaviors.RecentOpened
{
    public class RecentOpenedFilesChecker
    {
        public RecentOpenedFilesChecker()
        {
            _recentFiles = XmlSerialized.LoadData<RecentFiles>();
        }

        private RecentFiles _recentFiles;

        public void RegisterFileOpening(string fileName)
        {
            if (_recentFiles.RecentOpenedFiles.Contains(fileName))
            {
                return;
            }

            _recentFiles.RecentOpenedFiles.Add(fileName);
            _recentFiles.RecentOpenedFiles = _recentFiles.RecentOpenedFiles.TakeWhile((x, i) => !string.IsNullOrEmpty(x) && i < 5).ToList();
            _recentFiles.ToXml<RecentFiles>();
        }

        public List<string> RecentFiles
        {
            get
            {
                return _recentFiles.RecentOpenedFiles;
            }
        }

    }
}
