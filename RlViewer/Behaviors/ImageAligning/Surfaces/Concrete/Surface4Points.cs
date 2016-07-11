﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    /// <summary>
    /// Incapsulates surface made from 4 points
    /// </summary>
    public class Surface4Points : Surface3Points
    {
        public Surface4Points(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider)
            : base(selector, rcsProvider)
        {
            _rcsProvider = rcsProvider;
        }


        private object _amplitudeSolutionLocker = new object();

        private float[][] _amplitudeSolution;
        private float[][] AmplitudeSolution
        {
            get
            {
                lock (_amplitudeSolutionLocker)
                {
                    return _amplitudeSolution = _amplitudeSolution ?? InitAmplitudePlanes();
                }
            }
        }

        private object _rscSolutionLocker = new object();

        private float[][] _rcsSolution;
        private float[][] RcsSolution
        {
            get
            {
                lock (_rscSolutionLocker)
                {
                    return _rcsSolution = _rcsSolution ?? InitRcsPlanes();
                }
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




        public override byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            float[] image = new float[area.Width * area.Height];

            float[] imageArea = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);

            int toInclusiveX = area.Location.X + area.Width;
            toInclusiveX = toInclusiveX > file.Width ? file.Width : toInclusiveX;

            int toInclusiveY = area.Location.Y + area.Height;
            toInclusiveY = toInclusiveY > file.Height ? file.Height : toInclusiveY;
            int counter = 0;

         
            Parallel.For(area.Location.X, toInclusiveX, (i, loopState) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {
                    var oldAmplVal = imageArea[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var newAmplVal = GetPlaneValue(i / Selector.RangeCompressionCoef, j / Selector.AzimuthCompressionCoef,
                        PointToPlane(new System.Drawing.Point(i / Selector.RangeCompressionCoef, j / Selector.AzimuthCompressionCoef),
                        AmplitudeSolution));
                    var newRcsVal = GetPlaneValue(i / Selector.RangeCompressionCoef, j / Selector.AzimuthCompressionCoef,
                        PointToPlane(new System.Drawing.Point(i / Selector.RangeCompressionCoef,
                            j / Selector.AzimuthCompressionCoef), RcsSolution));
                    var diff = (float)(Math.Round(oldAmplVal, 2) / Math.Round(newAmplVal, 2) * newRcsVal);

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

        private float[] PointToPlane(Point p, float[][] solution)
        {
           
            //in order to belong to plane 1, point has to be to the left of (i1-i4) and to the left of (i2-i3)
            //same method applies for other combinations
            // i1    i2
            //  \ 2 /
            //   \ /
            // 3  X  1 plane
            //   / \
            //  / 4 \
            //i3     i4
            bool firstLineRelative =  GeometryHelper.MutualPosition(Selector[0].Location, Selector[3].Location, p);//i1-i4
            bool secondLineRelative = GeometryHelper.MutualPosition(Selector[1].Location, Selector[2].Location, p);//i2-i3

            if (!firstLineRelative && !secondLineRelative)
            {
                return solution[0];
            }
            else if (firstLineRelative && !secondLineRelative)
            {
                return solution[1];
            }
            else if (firstLineRelative && secondLineRelative)
            {
                return solution[2];
            }
            else if (!firstLineRelative && secondLineRelative)
            {
                return solution[3];
            }
            throw new ArgumentException();
        }

    }
}
