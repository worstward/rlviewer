using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Abstract
{
    public abstract class RbfSurface : Abstract.Surface
    {
        public RbfSurface(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider)
            : base(selector)
        {
            _rcsProvider = rcsProvider;
        }


        private IInterpolationProvider _rcsProvider;
        protected override IInterpolationProvider RcsProvider
        {
            get
            {
                return _rcsProvider;
            }
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

                    var oldAmplVal = imageArea[(j - area.Y) * area.Width + (i - area.X)];
                    var newAmplVal = (float)amplitudeSolution[i - area.X, j - area.Y];
                    var newRcsVal = (float)rcsSolution[i - area.X, j - area.Y];
                    var diff = oldAmplVal / newAmplVal * newRcsVal;

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


        protected abstract double[,] GetSolution(System.Drawing.Rectangle area, float[] values);



        private double[,] GetAmplitudeSolution(System.Drawing.Rectangle area)
        {
            return GetSolution(area, Selector.Select(x => x.Value).ToArray());
        }

        private double[,] GetRcsSolution(System.Drawing.Rectangle area)
        {
            return GetSolution(area, Selector.Select(x => x.Rcs).ToArray());
        }

        



    }

}
