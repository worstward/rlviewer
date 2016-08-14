using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILNumerics;
using ILNumerics.Toolboxes;



namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    public class KrigingInterpolatedSurface : Abstract.Surface
    {
        public KrigingInterpolatedSurface(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider)
            : base(selector)
        {
            _rcsProvider = rcsProvider;
        }

        private IInterpolationProvider _rcsProvider;
        protected override IInterpolationProvider RcsProvider
        {
            get { return _rcsProvider; }
        }

        private float[] GetAmplitudeSolution(System.Drawing.Rectangle area)
        {
            return GetSolution(area, Selector.Select(x => x.Value).ToArray());
        }

        private float[] GetRcsSolution(System.Drawing.Rectangle area)
        {
            return GetSolution(area, Selector.Select(x => x.Rcs).ToArray());
        }


        private float[] GetSolution(System.Drawing.Rectangle area, float[] values)
        {
            int index;
            for (index = 1; index < values.Length; index++)
            {
                if (values[0] == values[index])
                {
                    continue;
                }
                break;
            }

            if (index == values.Length)
            {
                return Enumerable.Repeat<float>(values[0], area.Width * area.Height).ToArray();
            }


            ILArray<float> Y = 1, X = ILMath.meshgrid(ILMath.vec<float>(area.X, 1, area.X + area.Width - 1),
                ILMath.vec<float>(area.Y, 1, area.Y + area.Height - 1), Y);

            var scatteredPositionFloat = new float[2, Selector.Count()];
            for (int i = 0; i < Selector.Count(); i++)
            {
                scatteredPositionFloat[0, i] = Selector[i].Location.X;
                scatteredPositionFloat[1, i] = Selector[i].Location.Y;
            }

            ILArray<float> scatteredPositions = scatteredPositionFloat;
            ILArray<float> scatteredValues = values;
            ILArray<float> interpPositions = X[":"].T.Concat(Y[":"].T, 0);
            ILArray<float> interpValues = Interpolation.kriging(scatteredValues.T, scatteredPositions.T, interpPositions);
            var reshaped = ILMath.reshape(interpValues, X.S).T;

            return reshaped.ToArray();
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
                    var newAmplVal = amplitudeSolution[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var newRcsVal = rcsSolution[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var diff = oldAmplVal / newAmplVal * newRcsVal;
                    //var ls = RcsProvider.GetValueAt(diff);
                    //diff *= ls;
                    if (Selector.Any(x => new System.Drawing.Point(i, j) == x.Location))
                    {
                        int a = 1;
                        int b = a++;
                    }



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
