using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace RlViewer.Behaviors.Navigation.NavigationSearcher.Abstract
{
    public abstract class GeodesicPointFinder : WorkerEventController
    {
        public GeodesicPointFinder(Files.LocatorFile file)
        {
            _file = file;           
        }


        private Files.LocatorFile _file;

        protected int NaviShift { get; set; }


        public Point GetCoordinates(double latitude, double longitude)
        {
            Point foundPoint = new Point();
            var naviSecondError = 0.00000242406;

            for(int i = 0; i < _file.Navigation.Count(); i++)
            {
                for (int j = 0; j < _file.Width; j++)
                {
                    var alpha = _file.Navigation.Computer.ComputeAlpha(
                        _file.Navigation.Computer.InitialRange, _file.Navigation.Computer.Step,
                        j + NaviShift, _file.Navigation[i].AircraftHeight);
                    var localLat = _file.Navigation.Computer.GetSampleLatitude(_file.Navigation[i].AircraftLatitude,
                        _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);


                    if (Math.Abs(localLat - latitude) < naviSecondError)
                    {
                        var localLong = _file.Navigation.Computer.GetSampleLongtitude(_file.Navigation[i].AircraftLongitude,
                            _file.Navigation[i].AircraftLatitude, _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);

                        if (Math.Abs(localLong - longitude) < naviSecondError)
                        {
                            return new Point(j, i);
                        }
                    }
                }

                OnProgressReport((int)((double)i / (double)_file.Navigation.Count() * 100));
                if (OnCancelWorker())
                {
                    return foundPoint;
                }

            }

            return foundPoint;
        }




    }
}
