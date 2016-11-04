using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class ErrorOccuredEventArgs : EventArgs
    {
        public ErrorOccuredEventArgs(string errorText)
        {
            _errorText = errorText;
        }

        private string _errorText;

        public string ErrorText
        {
            get 
            {
                return _errorText; 
            }
        }
    }
}
