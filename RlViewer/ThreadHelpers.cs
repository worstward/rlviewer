using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace RlViewer
{
    public static class ThreadHelper
    {
        public static BackgroundWorker InitWorker(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler completed)
        {
            var worker = new System.ComponentModel.BackgroundWorker() 
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true     
            };

            worker.DoWork += doWork;
            worker.RunWorkerCompleted += completed;

            return worker;
        }

        public  static T ThreadSafeUpdate<T>(T control) where T : Control
        {
            return control.InvokeRequired ? (T)control.Invoke((Func<Control>)(() => control)) : control;
        }

        public static void ThreadSafeUpdate<T>(T control, Action<T> action) where T : ToolStripItem
        {
            ToolStrip parent = control.GetCurrentParent();
            if (parent != null && parent.InvokeRequired)
            {
                parent.Invoke((Delegate)action, new object[] { control });
            }
            else
            {
                action(control);
            }
        }
    }
}
