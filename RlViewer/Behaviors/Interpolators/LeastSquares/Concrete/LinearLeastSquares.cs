using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Interpolators.LeastSquares.Concrete
{

    public class LinearLeastSquares : Abstract.LeastSquares
    {
        public LinearLeastSquares(Behaviors.PointSelector.PointSelector selector) : base(selector)
        {


            var points = selector.Select(x => new PointF(x.Value, x.Rcs));

            //var ps = MathNet.Numerics.Fit.Polynomial(selector.Select(x => (double)x.Value).ToArray(), selector.Select(x => (double)x.Rcs).ToArray(),
            //    selector.Count(), MathNet.Numerics.LinearRegression.DirectRegressionMethod.NormalEquations);

            //var p = MathNet.Numerics.LinearRegression.SimpleRegression.Fit(selector.Select(x => (double)x.Value).ToArray(), selector.Select(x => (double)x.Rcs).ToArray());

            _slope = SlopeOfPoints(points); //(float)p.Item2;
            _yIntercept = YInterceptOfPoints(points, _slope); //(float)p.Item1;
        }

        public LinearLeastSquares(IEnumerable<PointF> selectedPoints) : base(selectedPoints)
        {
            _slope = SlopeOfPoints(selectedPoints);
            _yIntercept = YInterceptOfPoints(selectedPoints, _slope); 
        }



        private float _slope;
        private float _yIntercept;

        /// <summary>
        /// Gets the value at a given X using the line of best fit (Least Square Method) to determine the equation
        /// </summary>
        /// <param name="points">Points to calculate the value from</param>
        /// <param name="x">Function input</param>
        /// <returns>Value at X in the given points</returns>
        public static float LeastSquaresValueAt(IEnumerable<PointF> points, float x)
        {
            float slope = SlopeOfPoints(points);
            float yIntercept = YInterceptOfPoints(points, slope);
            return (slope * x) + yIntercept;
        }


        public override float GetValueAt(float x)
        {
            return LeastSquaresValueAt(x);
        }

        protected override float LeastSquaresValueAt(float x)
        {
            return (_slope * x) + _yIntercept;
        }

        /// <summary>
        /// Gets the slope for a set of points using the formula:
        /// m = SUM(x-AVG(x)(y-AVG(y)) / SUM(x-AVG(x))^2
        /// </summary>
        /// <param name="points">Points to calculate the Slope from</param>
        /// <returns>SlopeOfPoints</returns>
        private static float SlopeOfPoints(IEnumerable<PointF> points)
        {
            float avgX = points.Average(p => p.X);
            float avgY = points.Average(p => p.Y);

            float dividend = points.Sum(p => (p.X - avgX) * (p.Y - avgY));
            float divisor = (float)points.Sum(p => Math.Pow(p.X - avgX, 2));

            return dividend / divisor;
        }

        /// <summary>
        /// Gets the Y-Intercept for a set of points using the formula:
        /// b = AVG(y) - m( AVG(x) )
        /// </summary>
        /// <param name="points">Points to calculate the intercept from</param>
        /// <returns>Y-Intercept</returns>
        private static float YInterceptOfPoints(IEnumerable<PointF> points, float slope)
        {
            float avgX = points.Average(p => p.X);
            float avgY = points.Average(p => p.Y);

            return avgY - (slope * avgX);
        }
    }
}
