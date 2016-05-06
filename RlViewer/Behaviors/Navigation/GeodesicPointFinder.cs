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


        //TODO: REFACTOR ASAP
        private int GetNaviShift()
        {
            var t = _file.GetType();
            if (t == typeof(Files.Rli.Concrete.Brl4))
            {
                var head = _file.Header as Headers.Concrete.Brl4.Brl4Header;
                return (int)head.HeaderStruct.rlParams.sx;
            }
            else if (t == typeof(Files.Rli.Concrete.Rl4))
            {
                var head = _file.Header as Headers.Concrete.Rl4.Rl4Header;
                return (int)head.HeaderStruct.rlParams.sx;
            }
            else if(t == typeof(Files.Rli.Concrete.R))
            {
                throw new NotSupportedException("Point finder for .R is not supported");
            }
            return 0;
        }


        public Point GetCoordinates(double latitude, double longitude)
        {
            int fromInclusive = 0;
            int toInclusive = _file.Navigation.Count();

            Point foundPoint = new Point();
            var shift = GetNaviShift();
            int counter = 0;

            List<Point> lst = new List<Point>();

            //Parallel.For(fromInclusive, toInclusive, 
            //    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 2 }, (i, loopState) =>
            //{
            for(int i = 0; i < toInclusive; i++)
            {
                for (int j = 0; j < _file.Width; j++)
                {
                    var alpha = _file.Navigation.Computer.ComputeAlpha(
                        _file.Navigation.Computer.InitialRange, _file.Navigation.Computer.Step,
                        j + shift, _file.Navigation[i].AircraftHeight);
                    var localLat = _file.Navigation.Computer.GetSampleLatitude(_file.Navigation[i].AircraftLatitude,
                        _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);


                    if (Math.Abs(localLat - latitude) < 0.00000242406)
                    {
                        var localLong = _file.Navigation.Computer.GetSampleLongtitude(_file.Navigation[i].AircraftLongitude,
                            _file.Navigation[i].AircraftLatitude, _file.Navigation[i].Track, alpha, _file.Navigation[i].Board);
                        if (Math.Abs(localLong - longitude) < 0.00000242406)
                        {
                            foundPoint = new Point(j, i);
                           // loopState.Break();

                        }
                    }
                }


               // System.Threading.Interlocked.Increment(ref counter);
                OnProgressReport((int)((double)i / (double)_file.Navigation.Count() * 100));
                if (OnCancelWorker())
                {
                    return foundPoint;
                    //loopState.Break();
                }

            }

            return foundPoint;
        }




    }
}
