using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.ImageAligning.LeastSquares.Concrete
{
    class PolynomialLeastSquares : Abstract.LeastSquares
    {
        public PolynomialLeastSquares(Behaviors.PointSelector.PointSelector selector)
            : base(selector)
        {
            _coefs = MathNet.Numerics.Fit.Polynomial(selector.Select(x => (double)x.Value).ToArray(), selector.Select(x => (double)x.Rcs).ToArray(),
                selector.Count() - 1);
        }

        double[] _coefs;


        public override float GetRcsValueAt(float x)
        {
            return LeastSquaresValueAt(x);
        }

        protected override float LeastSquaresValueAt(float x)
        {
            double y = 0;
            for(int i = 0; i < _coefs.Length; i++)
            {
                y += _coefs[i] * Math.Pow(x, i);
            }
            return (float)y;
        }

    }
}
