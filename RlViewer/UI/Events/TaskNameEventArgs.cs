using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class TaskNameEventArgs : EventArgs
    {
        public TaskNameEventArgs(string taskName)
        {
            _taskName = taskName;
        }

        private string _taskName;

        public string TaskName
        {
            get { return _taskName; }
        }

    }
}
