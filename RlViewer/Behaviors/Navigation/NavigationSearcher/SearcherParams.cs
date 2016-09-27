using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.NavigationSearcher
{
    class SearcherParams
    {
        public SearcherParams(int x, int y, double lat, double lon, double error, bool isNavigationSearcher)
        {
            _x = x;
            _y = y;
            _lat = lat;
            _lon = lon;
            _error = error;
            _isNavigationSearcher = isNavigationSearcher;
        }


        private int _x;

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private double _lat;

        public double Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        private double _lon;

        public double Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }

        private double _error;
        public double Error
        {
            get { return _error; }
        }

        private bool _isNavigationSearcher = false;
        public bool IsNavigationSearcher
        {
            get { return _isNavigationSearcher; }
        }

    }
}
