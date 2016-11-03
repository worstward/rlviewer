using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class RulerDistanceChangedEventArgs : EventArgs
    {
        public RulerDistanceChangedEventArgs(string distanceString)
        {
            _distanceString = distanceString;
        }

        private string _distanceString;

        public string DistanceString
        {
            get
            { 
                return _distanceString; 
            }
        }

       
    }
}
