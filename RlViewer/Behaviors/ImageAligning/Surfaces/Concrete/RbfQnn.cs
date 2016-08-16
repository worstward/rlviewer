using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    class RbfQnn : Abstract.RbfSurface
    {
        public RbfQnn(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider)
            : base(selector, rcsProvider)
        {
            
        }


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

            alglib.rbfsetalgoqnn(model);
            alglib.rbfbuildmodel(model, out rep);

            double[] x = Enumerable.Range(area.X, area.Width).Select(val => (double)val).ToArray();
            double[] y = Enumerable.Range(area.Y, area.Height).Select(val => (double)val).ToArray();

            double[,] result;
            alglib.rbfgridcalc2(model, x, x.Length, y, y.Length, out result);

            return result;
        }
    }
}
