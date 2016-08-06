using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Security.Principal;

namespace RlViewer
{
    public class Adminizer
    {
        public Adminizer()
        {
            _executablePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        private string _executablePath;
        private const int _cancelledByUser = 1223;

        public bool CheckIfAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void EvaluateToAdminProcess()
        {
            ProcessStartInfo info = new ProcessStartInfo(_executablePath);
            info.Arguments = Environment.GetCommandLineArgs().Aggregate((x, y) => x + " " + y);
            info.UseShellExecute = true;
            info.Verb = "runas";

            try
            {
                Process.Start(info);
            }
            catch (Win32Exception w32ex)
            {
                if (w32ex.NativeErrorCode == _cancelledByUser)
                {
                    throw new OperationCanceledException("Process status evaluation has been cancelled by user", w32ex);
                }
            }
        }

    }
}
