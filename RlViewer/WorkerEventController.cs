using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{
    public class WorkerEventController
    {
        public class ProgressEventArgs : EventArgs
        {
            private int _percent;
            public int Percent
            {
                get { return _percent; }
            }

            public ProgressEventArgs(int percent)
            {
                _percent = percent;
            }
        }


        public delegate void ReportProgress(object sender, ProgressEventArgs e);

        public virtual event ReportProgress Report = delegate { };

        private bool _cancelled = false;

        public virtual bool Cancelled
        {
            get
            {
                return _cancelled;
            }
            set
            {
                _cancelled = value;
            }
        }

        protected virtual void OnProgressReport(int percentage)
        {
            Report(null, new ProgressEventArgs(percentage));
        }


        public delegate void CancelEventHandler(object sender, CancelEventArgs e);

        public virtual event CancelEventHandler CancelJob = delegate { };

        protected virtual bool OnCancelWorker()
        {
            if (CancelJob != null)
            {
                if (Cancelled)
                {
                    throw new OperationCanceledException(this.ToString());
                }
            }
            return false;
        }

        public class CancelEventArgs : EventArgs
        {

            public bool Cancel;
        }


        public delegate void ReportTaskName(object sender, TaskNameEventArgs e);

        public virtual event ReportTaskName ReportName = delegate { };

        protected void OnNameReport(string name)
        {
            ReportName(null, new TaskNameEventArgs(name));
        }

        public class TaskNameEventArgs : EventArgs
        {
            public TaskNameEventArgs(string name)
            {
                Name = name;
            }

            public string Name 
            {
                get; 
                private set;
            }
        }

    }
}
