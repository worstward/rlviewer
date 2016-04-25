using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    /// <summary>
    /// Incapsulates surface made from 3 points (plane)
    /// </summary>
    public class Surface3Points : Surfaces.Abstract.Surface
    {
        public Surface3Points(PointSelector.PointSelector selector)
            : base(selector)
        {

        }

        private float[][] _solution;
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
            
   
            Parallel.For(area.Location.X, toInclusiveX, (i, loopState) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {
                    var oldVal = imageArea[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var newVal = GetAmplitude(i, j, Solution.First());
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
        /// Builds a plane through each 3 selected points
        /// </summary>
        /// <returns></returns>
        protected float[][] InitPlanes()
        {
            var planes = Selector.Combinations<PointSelector.SelectedPoint>(3).ToList();

            float[][] solution = new float[planes.Count][];

            for (int i = 0; i < planes.Count; i++)
            {
                solution[i] = Solve(planes[i].ToList());
            }
            return solution;
        }



        /// <summary>
        /// Gets amplitude of given point for provided plane
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Amplitude</returns>
        protected virtual float GetAmplitude(int x, int y, float[] solution)
        {
            //Ax + By + Cz + D = 0
            //z = (-Ax - By - D) / C
            return (-solution[0] * x - solution[1] * y - solution[3]) / solution[2];
        }
        

        /// <summary>
        /// Gets coefficients of plane from 3 points
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        protected float[] Solve(List<PointSelector.SelectedPoint> selector)
        {
            if (selector.Count() != 3)
            {
                throw new ArgumentOutOfRangeException("selector.Count()");//not 3 points provided
            }

            var p1 = selector[0];
            var p2 = selector[1];
            var p3 = selector[2];

            float A = p1.Location.Y * (p2.Value - p3.Value) + p2.Location.Y * (p3.Value - p1.Value) + p3.Location.Y * (p1.Value - p2.Value);
            float B = p1.Value * (p2.Location.X - p3.Location.X) + p2.Value * (p3.Location.X - p1.Location.X) + p3.Value * (p1.Location.X - p2.Location.X);
            float C = p1.Location.X * (p2.Location.Y - p3.Location.Y) + p2.Location.X * (p3.Location.Y - p1.Location.Y) + p2.Location.X * (p1.Location.Y - p2.Location.Y);
            float D = -(p1.Location.X * (p2.Location.Y * p3.Value - p3.Location.Y * p2.Value) + p2.Location.X * 
                (p3.Location.Y * p1.Value - p1.Location.Y * p3.Value) + p3.Location.X * (p1.Location.Y * p2.Value - p2.Location.Y * p1.Value));

            return new float[] { A, B, C, D };
        }

    }
}
