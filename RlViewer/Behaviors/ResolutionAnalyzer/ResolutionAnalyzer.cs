using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ResolutionAnalyzer
{
    public class ResolutionAnalyzer
    {
        public ResolutionAnalyzer(Files.LocatorFile file,
            int interpolationRange, int analyzedSamples)
        {         
            _file = file;
            _interpolationRange = interpolationRange;
            _analyzedSamples = analyzedSamples;
        }

        private Files.LocatorFile _file;
        private Behaviors.ImageAligning.IInterpolationProvider _interpolator;
        private int _interpolationRange;
        private int _analyzedSamples;


        public float[] GetRangeResolution(System.Drawing.Point initialPoint)
        {
            int fromInclusive = initialPoint.X - _analyzedSamples / 2 - 1;
            fromInclusive = fromInclusive < 0 ? 0 : fromInclusive;

            int toInclusive = initialPoint.X + _analyzedSamples / 2;
            toInclusive = toInclusive > _file.Width ? _file.Width : toInclusive;

            List<float> amplitudes = new List<float>(_analyzedSamples);

            amplitudes.AddRange(new float[toInclusive - fromInclusive - _analyzedSamples]);

            for (int i = fromInclusive; i < toInclusive; i++)
            {
                amplitudes.Add(_file.GetSample(
                    new System.Drawing.Point(i, initialPoint.Y)).ToSample<float>(_file.Header.BytesPerSample));
            }


            float interpolationStep = _analyzedSamples / _interpolationRange;

            List<float> interpolatedValues = new List<float>(_interpolationRange);

            for (float i = fromInclusive; i < _interpolationRange; i += interpolationStep)
            {
                interpolatedValues.Add(_interpolator.GetValueAt(i));
            }

            throw new NotImplementedException();
        }

        public IEnumerable<System.Drawing.PointF> GetAzimuthResolution(System.Drawing.Point initialPoint)
        {
            int fromInclusive = initialPoint.Y - _analyzedSamples / 2;
            fromInclusive = fromInclusive < 0 ? 0 : fromInclusive;

            int toInclusive = initialPoint.Y + _analyzedSamples / 2;
            toInclusive = toInclusive > _file.Height ? _file.Height : toInclusive;

            List<float> amplitudes = new List<float>(_analyzedSamples);

            amplitudes.AddRange(new float[Math.Abs(toInclusive - fromInclusive - _analyzedSamples)]);

            for (int i = fromInclusive; i < toInclusive; i++)
            {
                amplitudes.Add(_file.GetSample(
                    new System.Drawing.Point(initialPoint.X, i)).ToSample<float>(_file.Header.BytesPerSample));
            }

            float interpolationStep = _analyzedSamples / (float)_interpolationRange;

            List<System.Drawing.PointF> interpolatedValues = new List<System.Drawing.PointF>(_interpolationRange);

            _interpolator = new Behaviors.Interpolators.LeastSquares.Concrete.PolynomialLeastSquares(
                amplitudes.Select((x, i) => new System.Drawing.PointF(i, x)));

            for (float i = fromInclusive; i < _interpolationRange; i += interpolationStep)
            {
                interpolatedValues.Add(new System.Drawing.PointF(i, _interpolator.GetValueAt(i)));
            }

            return interpolatedValues.ToArray();
        }



    }
}
