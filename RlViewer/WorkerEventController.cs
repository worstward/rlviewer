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

        public virtual event ReportProgress Report;

        private bool _cancelled = false;

        public bool Cancelled
        {
            get { return _cancelled; }
        }



        protected virtual void OnProgressReport(int percentage)
        {
            if (Report != null)
            {
                Report(percentage);
            }
        }

        public virtual event EventHandler<CancelEventArgs> CancelJob;

        protected virtual bool OnCancelWorker()
        {
            EventHandler<CancelEventArgs> handler = CancelJob;

            if (handler != null)
            {
                var e = new CancelEventArgs();
                handler(null, e);
                _cancelled = e.Cancel;
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
