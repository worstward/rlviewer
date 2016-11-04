using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RlViewer.Navigation
{
    public class NavigationItem : INotifyPropertyChanged
    {
        public NavigationItem(string parameterName, string parameterValue)
        {
            ParameterName = parameterName;
            _parameterValue = parameterValue;
        }

        public string ParameterName
        {
            get;
            set;
        }


        private string _parameterValue;

        public string ParameterValue
        {
            get 
            {
                return _parameterValue; 
            }
            set 
            {
                SetField(ref _parameterValue, value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
