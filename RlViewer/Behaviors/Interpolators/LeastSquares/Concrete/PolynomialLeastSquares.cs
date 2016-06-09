using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Interpolators.LeastSquares.Concrete
{
    class PolynomialLeastSquares : Abstract.LeastSquares
    {
        public PolynomialLeastSquares(Behaviors.PointSelector.PointSelector selector)
            : base(selector)
        {
            
            _coefs = MathNet.Numerics.Fit.Polynomial(selector.Select(x => (double)x.Value).ToArray(), selector.Select(x => (double)x.Rcs).ToArray(),
                selector.Count() - 1);
        }

        public PolynomialLeastSquares(IEnumerable<System.Drawing.PointF> points)
            : base(points)
        {
            _coefs = MathNet.Numerics.Fit.Polynomial(points.Select(x => (double)x.X).ToArray(), points.Select(x => (double)x.Y).ToArray(),
                points.Count() - 1);
        }


        double[] _coefs;


        public override float GetValueAt(float x)
        {
            return LeastSquaresValueAt(x);
        }

        protected override float LeastSquaresValueAt(float x)
        {
            return (float)MathNet.Numerics.Evaluate.Polynomial((double)x, _coefs);
            //double y = 0;
            //for(int i = 0; i < _coefs.Length; i++)
            //{
            //    y += _coefs[i] * Math.Pow(x, i);
            //}
            //return (float)y;
        }

    }
}
