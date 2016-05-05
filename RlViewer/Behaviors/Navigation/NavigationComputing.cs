using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation
{
    public class NavigationComputing
    {
        public NavigationComputing(double initialRange, double step)
        {
            _initialRange = initialRange;
            _step = step;
        }

        private double _initialRange;

        public double InitialRange
        {
            get { return _initialRange; }
        }


        private double _step;

        public double Step
        {
            get { return _step; }
        }


        public Tuple<double, double> Interpolate(int sampleNum, double aircraftLongitude,
            double aircraftLatitude, double aircraftHeight, double track, byte board)
        {
            var alpha = ComputeAlpha(_initialRange, _step, sampleNum, aircraftHeight);

            var latitude = InterpolateLatitude(aircraftLatitude, track, alpha, board);

            var longitude = InterpolateLongtitude(aircraftLongitude, aircraftLatitude, track, alpha, board);

            return new Tuple<double, double>(latitude, longitude);
        }


        public double InterpolateLatitude(int sampleNum, double aircraftLatitude, double aircraftHeight, double track, byte board)
        {
            var alpha = ComputeAlpha(_initialRange, _step, sampleNum, aircraftHeight);
            return GetSampleLatitude(aircraftLatitude, track, alpha, board);
        }

        public double InterpolateLongtitude(int sampleNum, double aircraftLongitude, double aircraftLatitude, double aircraftHeight, double track, byte board)
        {
            var alpha = ComputeAlpha(_initialRange, _step, sampleNum, aircraftHeight);
            return GetSampleLongtitude(aircraftLongitude, aircraftLatitude, track, alpha, board);
        }

        private double InterpolateLatitude(double aircraftLat, double track, double alpha, byte board)
        {
            return GetSampleLatitude(aircraftLat, track, alpha, board);
        }

        private double InterpolateLongtitude(double aircraftLon, double aircraftLat, double track, double alpha, byte board)
        {
            return GetSampleLongtitude(aircraftLon, aircraftLat, track, alpha, board);
        }

        public double ComputeAlpha(double initialRange, double step, int sampleNum, double aircraftHeight, double earthRadius = 6372795)
        {
            double Lpoint = initialRange + step * sampleNum;
            double R2 = 2 * earthRadius * (earthRadius + aircraftHeight);
            double H2 = aircraftHeight * aircraftHeight;
            double LZ = (H2 - Lpoint * Lpoint);
            double arcosalpha = 1 + LZ / R2;
            double alpha = 0;

            if ((arcosalpha < -1) || (arcosalpha > 1))
                alpha = 0;
            else
                alpha = Math.Acos(arcosalpha);

            return alpha;
        }

        public double GetSampleLongtitude(double aircraftLon, double aircraftLat, double track, double alpha, byte board)
        {
            return board == 0 ? aircraftLon - alpha * Math.Cos(track) / Math.Cos(aircraftLat) :
                                aircraftLon + alpha * Math.Cos(track) / Math.Cos(aircraftLat);
        }


        public double GetSampleLatitude(double aircraftLat, double track, double alpha, byte board)
        {
            return board == 0 ? aircraftLat + alpha * Math.Sin(track) :
                                aircraftLat - alpha * Math.Sin(track);
        }
    }
}