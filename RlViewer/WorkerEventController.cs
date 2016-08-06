﻿using System;
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
