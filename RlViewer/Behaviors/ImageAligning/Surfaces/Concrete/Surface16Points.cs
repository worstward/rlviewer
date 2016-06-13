﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.ImageAligning;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    /// <summary>
    /// Incapsulates surface made from 16 points
    /// </summary>
    class Surface16Points : Surfaces.Abstract.Surface
    {
        public Surface16Points(PointSelector.PointSelector selector, IInterpolationProvider rcsProvider)
            : base(selector)
        {
            _rcsProvider = rcsProvider;
        
            for (int i = 0; i < 4; i++)
            {
                _zCoefficients[i] = new LinearEquation(
                        Selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        Selector.Skip(i * 4).Take(4).Select(x => x.Value).ToArray())
                    .Solution;

                _yCoefficients[i] = new LinearEquation(
                        Selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        Selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.Y).ToArray())
                    .Solution;

            } 
        }


        private IInterpolationProvider _rcsProvider;
        protected override IInterpolationProvider RcsProvider
        {
            get
            {
                return _rcsProvider;
            }

        }

        /// <summary>
        /// Contains x-z linear equation coefficients (A*x^3 + B*x^2 + C*x + D = z)
        /// /// </summary>
        private float[][] _zCoefficients = new float[4][];

        /// <summary>
        /// Contains x-y linear equation coefficients (A*x^3 + B*x^2 + C*x + D = y)
        /// </summary>
        private float[][] _yCoefficients = new float[4][];


        public override byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            float[] image = new float[area.Width * area.Height];
           
            //iterate over X axis

            int toInclusiveX = area.Location.X + area.Width;
            toInclusiveX = toInclusiveX > file.Width ? file.Width : toInclusiveX;

            int toInclusiveY = area.Location.Y + area.Height;
            toInclusiveY = toInclusiveY > file.Height ? file.Height : toInclusiveY;
            int counter = 0;


            float[] imageArea = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);

            Parallel.For(area.Location.X, toInclusiveX, (i, loopState) =>
            {
                var zValues = _zCoefficients.Select(x => Extrapolate(i, x)).ToArray();
                var yValues = _yCoefficients.Select(x => Extrapolate(i, x)).ToArray();
                var _zCoefs = new LinearEquation(yValues, zValues).Solution;

                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {
                    var oldVal = imageArea[(j - area.Location.Y)
                        * area.Width + (i - area.Location.X)];
                    var newVal = Extrapolate(j, _zCoefs);

                    var diff = oldVal / newVal * RcsProvider.GetValueAt(oldVal);
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



        private float Extrapolate(int sample, float[] solution)
        {
            if (solution.Length != 4)
            {
                throw new ArgumentOutOfRangeException("solution.Length");//not 16 points provided
            }

            //(A*x^3 + B*x^2 + C*x + D = z) to find z with provided x sample and ABCD coef in solution[]
            float extrapolatedValue = 0;

            for (int j = 3; j >= 0; j--)
            {
                extrapolatedValue += (float)Math.Pow(sample, 3 - j) * solution[j];
            }
            return extrapolatedValue;
        }

    }
}
