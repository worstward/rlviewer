using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    class RbfMultilayerCoef : RbfMultilayer
    {
        public RbfMultilayerCoef(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider,
           int baseRadius, int layers, double lambda = 0.01) : base(selector, rcsProvider, baseRadius, layers, lambda)
        {
 
        }


        private float _maxCoef;

        protected double[,] GetCoefSolution(System.Drawing.Rectangle area)
        {
            var coefs = Selector.Select(x => x.Value / x.Rcs);
            _maxCoef = coefs.Max();
            coefs = coefs.Select(x => x / _maxCoef);
            return GetSolution(area, coefs.ToArray());
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

            var coefSolution = GetCoefSolution(area);

            Parallel.For(area.Location.X, toInclusiveX, (i, loopState) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {

                    var oldAmplVal = imageArea[(j - area.Y) * area.Width + (i - area.X)];
                    var coefVal = (float)coefSolution[i - area.X, j - area.Y];
                    var diff = oldAmplVal / coefVal / _maxCoef;
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
