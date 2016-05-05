using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace RlViewer.Behaviors.Navigation
{
    public class GeodesicPointFinder : WorkerEventController
    {
        public GeodesicPointFinder(Files.LocatorFile file)
        {
            _file = file;           
        }

        private Files.LocatorFile _file;

        //0.79371599702881679, 0.63309294071067668
        public Point GetCoordinates(double latitude, double longitude)
        {
            for (int i = 0; i < _file.Navigation.Count() - 1; i++)
            {
                for (int j = 0; j < _file.Width; j++)
                {
                    var alpha = _file.Navigation.Computer.ComputeAlpha(_file.Navigation.Computer.InitialRange, _file.Navigation.Computer.Step, j + 2798, _file.Navigation[i].AircraftHeight);
                    var localLat = _file.Navigation.Computer.GetSampleLatitude(_file.Navigation[i].AircraftLatitude, _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);

                    if (Math.Round(localLat, 6) == Math.Round(latitude, 6))
                    {
                        var localLong = _file.Navigation.Computer.GetSampleLongtitude(_file.Navigation[i].AircraftLongitude,
                            _file.Navigation[i].AircraftLatitude, _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);
                        if(Math.Round(localLong, 5) == Math.Round(longitude, 5))
                        { 
                            if(i != 0 && j != 56)
                            return new Point(i, j);
                        }
                    }
                }


                OnProgressReport((int)((double)i / (double)_file.Navigation.Count() * 100));
                if (OnCancelWorker())
                {
                    break;
                }

            }

            return new Point();
        }


    }
}
