using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    public class RbfSurface : Abstract.Surface
    {
        public RbfSurface(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider)
            : base(selector)
        {

        }
        

        protected override IInterpolationProvider RcsProvider
        {
            get { throw new NotImplementedException(); }
        }

        private double[,] GetAmplitudeSolution(System.Drawing.Rectangle area)
        {
            return GetSolution(area, Selector.Select(x => x.Value).ToArray());
        }

        private double[,] GetRcsSolution(System.Drawing.Rectangle area)
        {
            return GetSolution(area, Selector.Select(x => x.Rcs).ToArray());
        }

        private double[,] GetSolution(System.Drawing.Rectangle area, float[] values)
        {
            alglib.rbfmodel model;
            alglib.rbfreport rep;

            alglib.rbfcreate(2, 1, out model);

            var xy = new double[3, Selector.Count()];
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



        public override byte[] ResampleImage(Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            float[] image = new float[area.Width * area.Height];

            float[] imageArea = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);

            int toInclusiveX = area.Location.X + area.Width;
            toInclusiveX = toInclusiveX > file.Width ? file.Width : toInclusiveX;

            int toInclusiveY = area.Location.Y + area.Height;
            toInclusiveY = toInclusiveY > file.Height ? file.Height : toInclusiveY;
            int counter = 0;

            var rcsSolution = GetRcsSolution(area);
            var amplitudeSolution = GetAmplitudeSolution(area);

            Parallel.For(area.Location.X, toInclusiveX, (i, loopState) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {

                    var oldAmplVal = imageArea[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var newAmplVal = (float)amplitudeSolution[i, j];
                    var newRcsVal = (float)rcsSolution[i, j];
                    var diff = oldAmplVal / newAmplVal * newRcsVal;
                    //var ls = RcsProvider.GetValueAt(diff);
                    //diff *= ls;

                    diff = diff < 0 ? 0 : diff;
                    image[(j - area.Location.Y) * area.Width + (i - area.Location.X)] = diff;
                }

                System.Threading.Interlocked.Increment(ref counter);
                OnProgressReport((int)(counter / Math.Ceiling((double)(toInclusiveX - area.Location.X)) * 100));

                if (OnCancelWorker())
                {
                    loopState.Break();
                }

            });

            if (Cancelled)
            {
                return null;
            }

            byte[] imageB = new byte[image.Length * 4];

            Buffer.BlockCopy(image, 0, imageB, 0, imageB.Length);

            return imageB;
        }


    }
}
