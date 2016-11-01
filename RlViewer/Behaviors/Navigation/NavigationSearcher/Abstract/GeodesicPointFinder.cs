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

        public Point GetCoordinates(double latitude, double longitude, double error)
        {
            OnNameReport("Поиск точки");
            Point foundPoint = new Point();
            int progress = 0;
            var navigationLength = _file.Navigation.Count();

            Parallel.For(0, navigationLength, (i, loopstate) =>
                {
                    for (int j = 0; j < _file.Width; j++)
                    {
                        var alpha = _file.Navigation.Computer.ComputeAlpha(
                            _file.Navigation.Computer.InitialRange, _file.Navigation.Computer.Step,
                            j + NaviShift, _file.Navigation[i].AircraftHeight);
                        var localLat = _file.Navigation.Computer.GetSampleLatitude(_file.Navigation[i].AircraftLatitude,
                            _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);


                        if (Math.Abs(localLat - latitude) < error)
                        {
                            var localLong = _file.Navigation.Computer.GetSampleLongtitude(_file.Navigation[i].AircraftLongitude,
                                _file.Navigation[i].AircraftLatitude, _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);

                            if (Math.Abs(localLong - longitude) < error)
                            {
                                foundPoint = new Point(j, i);
                                loopstate.Break();
                            }
                        }
                    }

                    System.Threading.Interlocked.Increment(ref progress);
                    OnProgressReport((int)((double)progress / (double)navigationLength * 100));
                    if (OnCancelWorker())
                    {
                        loopstate.Break();
                    }

                });
            return foundPoint;
        }




    }
}
