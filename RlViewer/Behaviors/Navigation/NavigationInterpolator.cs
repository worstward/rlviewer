using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation
{
    class NavigationInterpolator
    {
        public NavigationInterpolator(float initialRange, float step)
        {
            _initialRange = initialRange;
            _step = step;
        }

        private float _initialRange;
        private float _step;


        public Tuple<double, double> Interpolate(int sampleNum, float aircraftLongtitude, float aircraftLatitude, float aircraftHeight, float track, byte board)
        {
            var alpha = Alpha(_initialRange, _step, sampleNum, aircraftHeight);

            var latitude = InterpolateLatitude(aircraftLatitude, track, alpha, board);

            var longtitude = InterpolateLongtitude(aircraftLongtitude, aircraftLatitude, track, alpha, board);

            return new Tuple<double, double>(latitude, longtitude);
        }


        public double InterpolateLatitude(int sampleNum, float aircraftLatitude, float aircraftHeight, float track, byte board)
        {
            var alpha = Alpha(_initialRange, _step, sampleNum, aircraftHeight);
            return GetSampleLatitude(aircraftLatitude, track, alpha, board);
        }

        public double InterpolateLongtitude(int sampleNum, float aircraftLongtitude, float aircraftLatitude, float aircraftHeight, float track, byte board)
        {
            var alpha = Alpha(_initialRange, _step, sampleNum, aircraftHeight);

            return GetSampleLongtitude(aircraftLongtitude, aircraftLatitude, track, alpha, board);
        }

        private double InterpolateLatitude(float aircraftLat, float track, float alpha, byte board)
        {
            return GetSampleLatitude(aircraftLat, track, alpha, board);
        }

        private double InterpolateLongtitude(float aircraftLon, float aircraftLat, float track, float alpha, byte board)
        {
            return GetSampleLongtitude(aircraftLon, aircraftLat, track, alpha, board);
        }
    
        private float Alpha(float initialRange, float step, int sampleNum, float aircraftHeight, float earthRadius = 6372795)
        {
            float Lpoint = initialRange + step * sampleNum;
            float Rsq = 2 * earthRadius * (earthRadius + aircraftHeight);
            float Hsq = aircraftHeight * aircraftHeight;
            float LZ = (Hsq - Lpoint * Lpoint);
            float arcosalpha = 1 + LZ / Rsq;
            float alpha = 0;

            if ((arcosalpha < -1) || (arcosalpha > 1))
                alpha = 0;
            else
                alpha = (float)(Math.Acos(arcosalpha) * 180 / Math.PI);

            return alpha;
        }

        private double GetSampleLongtitude(float aircraftLon, float aircraftLat, float track, float alpha, byte board)
        {
            //board == 0 ? left : right
            double dp;
            if (board == 0)
            {
                dp = aircraftLon - alpha * Math.Cos(track * Math.PI / 180f) / Math.Cos(aircraftLat * Math.PI / 180f);
            }
            else 
            {
                dp = aircraftLon + alpha * Math.Cos(track * Math.PI / 180f) / Math.Cos(aircraftLat * Math.PI / 180f);
            }
            return dp;
        }


        private double GetSampleLatitude(float aircraftLat, float track, float alpha, byte board)
        {
            double dp;
            if (board == 0)
            {
                dp = aircraftLat + alpha * Math.Sin(track * Math.PI / 180f);
            }
            else
            {
                dp = aircraftLat - alpha * Math.Sin(track * Math.PI / 180f);
            }
            return dp;
        }

    }
}
