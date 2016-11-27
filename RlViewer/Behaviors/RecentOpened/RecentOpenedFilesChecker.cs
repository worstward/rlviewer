using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RlViewer.Behaviors.RecentOpened
{
    public class RecentOpenedFilesChecker
    {
        public RecentOpenedFilesChecker(int recentFilesToDisplay)
        {
            _recentFiles = XmlSerializable.LoadData<RecentFiles>();
            _recentFilesToDisplayCount = recentFilesToDisplay;
        }

        private RecentFiles _recentFiles;
        private int _recentFilesToDisplayCount;

        public void RegisterFileOpening(string fileName)
        {
            if (_recentFiles.RecentOpenedFiles.Contains(fileName))
            {
                return;
            }

            _recentFiles.RecentOpenedFiles.Push(fileName);
            _recentFiles.RecentOpenedFiles = new Stack<string>(
                _recentFiles.RecentOpenedFiles.Take(Math.Min(_recentFiles.RecentOpenedFiles.Count, _recentFilesToDisplayCount)).Reverse());

            _recentFiles.ToXml<RecentFiles>();
        }

        public IEnumerable<string> RecentFiles
        {
            get
            {
                return _recentFiles.RecentOpenedFiles;
            }
        }

    }
}
