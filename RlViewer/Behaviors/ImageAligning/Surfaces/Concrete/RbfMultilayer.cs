using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    class RbfMultilayer : Abstract.RbfSurface
    {
        /// <summary>
        /// Radial basis function interpolated surface
        /// </summary>
        /// <param name="selector">Points to build surface on</param>
        /// <param name="rcsProvider">Radio cross section provider</param>
        /// <param name="baseRadius">Average distance between points</param>
        /// <param name="layers">Layers number</param>
        /// <param name="lambda">Regularization coef</param>
        public RbfMultilayer(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider,
            int baseRadius, int layers, double lambda = 0.01)
            : base(selector, rcsProvider)
        {
            _baseRadius = baseRadius;
            _layers = layers;
            _lambda = lambda;
        }

        int _baseRadius;
        int _layers;
        double _lambda;

        protected override double[,] GetSolution(System.Drawing.Rectangle area, float[] values)
        {
            alglib.rbfmodel model;
            alglib.rbfreport rep;

            alglib.rbfcreate(2, 1, out model);

            var xy = new double[Selector.Count(), 3];
            for (int i = 0; i < Selector.Count(); i++)
            {
                xy[i, 0] = Selector[i].Location.X;
                xy[i, 1] = Selector[i].Location.Y;
                xy[i, 2] = values[i];
            }

            alglib.rbfsetpoints(model, xy);

            alglib.rbfsetalgomultilayer(model, _baseRadius, _layers, _lambda);
            alglib.rbfbuildmodel(model, out rep);

            double[] x = Enumerable.Range(area.X, area.Width).Select(val => (double)val).ToArray();
            double[] y = Enumerable.Range(area.Y, area.Height).Select(val => (double)val).ToArray();

            double[,] result;
            alglib.rbfgridcalc2(model, x, x.Length, y, y.Length, out result);

            return result;
        }
    }
}
