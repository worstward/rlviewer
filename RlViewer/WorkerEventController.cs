using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{
    public class WorkerEventController
    {

        public delegate void ReportProgress(int percentage);

        public event ReportProgress Report;

        protected virtual void OnProgressReport(int percentage)
        {
            if (Report != null)
            {
                Report(percentage);
            }
        }

        public event EventHandler<CancelEventArgs> CancelJob;

        protected virtual bool OnCancelWorker()
        {
            EventHandler<CancelEventArgs> handler = CancelJob;

            if (handler != null)
            {
                var e = new CancelEventArgs();
                handler(null, e);

                return e.Cancel;
            }
            return false;
        }

        public class CancelEventArgs : EventArgs
        {

            public bool Cancel;
        }

    }
}
