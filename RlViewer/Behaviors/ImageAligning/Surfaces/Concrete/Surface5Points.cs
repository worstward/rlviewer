using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    /// <summary>
    /// Incapsulates surface made from 5 points
    /// </summary>
    class Surface5Points : Surface3Points
    {
        public Surface5Points(PointSelector.PointSelector selector)
            : base(selector)
        {

        }

        private float[][] _solution;
        /// <summary>
        /// Contains coefficients for 4 planes
        /// </summary>
        private float[][] Solution
        {
            get
            {
                return _solution = _solution ?? InitPlanes();
            }
        } 

        private LeastSquares _lSquares;
        protected override LeastSquares LSquares
        {
            get
            {
                return _lSquares = _lSquares ?? new LeastSquares(Selector);
            }
        
        }


        public override byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            float[] image = new float[area.Width * area.Height];

            float[] imageArea = Behaviors.FileReader.GetArea(file, area);

            int toInclusiveX = area.Location.X + area.Width;
            toInclusiveX = toInclusiveX > file.Width ? file.Width : toInclusiveX;

            int toInclusiveY = area.Location.Y + area.Height;
            toInclusiveY = toInclusiveY > file.Height ? file.Height : toInclusiveY;
            int counter = 0;


            Parallel.For(area.Location.X, toInclusiveX, (i,loopState) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {
                    var oldVal = imageArea[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var newVal = GetAmplitude(i, j);
                    var diff = oldVal / newVal * LSquares.LeastSquaresValueAtX(oldVal);
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

        /// <summary>
        /// Gets Z coordinate of a point
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        protected float GetAmplitude(int x, int y)
        {
            return GetAmplitude(x, y, PointToPlane(new System.Drawing.Point(x, y)));
        }


        private float[] PointToPlane(Point p)
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
                return Solution[0];
            }
            else if (firstLineRelative && !secondLineRelative)
            {
                return Solution[1];
            }
            else if (firstLineRelative && secondLineRelative)
            {
                return Solution[2];
            }
            else if (!firstLineRelative && secondLineRelative)
            {
                return Solution[3];
            }
            throw new ArgumentException();
        }

    }
}
