using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    /// <summary>
    /// Surface made from 3 points = plane
    /// </summary>
    public class Surface3Points : Surfaces.Abstract.Surface
    {
        public Surface3Points(PointSelector.PointSelector selector)
            : base(selector)
        {
            _solution = Solve(Selector);
            //var normalizedPoints = selector.Select(x => new System.Drawing.PointF(x.Rcs, x.Value / GetAmplitude(x.Location.X, x.Location.Y)));
            _lSquares = new LeastSquares(selector);
        }

        private float[] _solution;
        private LeastSquares _lSquares;



        public override byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            float[] image = new float[area.Width * area.Height];
            
            float[] imageArea = Behaviors.FileReader.GetArea(file, area);

            int toInclusiveX = area.Location.X + area.Width;
            toInclusiveX = toInclusiveX > file.Width ? file.Width : toInclusiveX;

            int toInclusiveY = area.Location.Y + area.Height;
            toInclusiveY = toInclusiveY > file.Height ? file.Height : toInclusiveY;
            int counter = 0;
            
   
            Parallel.For(area.Location.X, toInclusiveX, (i) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {
                    var oldVal = imageArea[(j - area.Location.Y) * area.Width + (i - area.Location.X)];

                    var newVal = GetAmplitude(i, j);

                    var diff = oldVal / newVal * _lSquares.LeastSquaresValueAtX((float)Math.Log10(oldVal));

                    diff = diff < 0 ? 0 : diff;

                    image[(j - area.Location.Y) * area.Width + (i - area.Location.X)] = diff;
                }

                System.Threading.Interlocked.Increment(ref counter);
                OnProgressReport((int)(counter / Math.Ceiling((double)(toInclusiveX - area.Location.X)) * 100));
                if (OnCancelWorker())
                {
                    return;
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
        /// Gets amplitude of given point for provided plane
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Amplitude</returns>
        private float GetAmplitude(int x, int y)
        {
            //Ax + By + Cz + D = 0
            //z = (-Ax - By - D) / C
            return (-_solution[0] * x - _solution[1] * y - _solution[3]) / _solution[2];
        }
        

        /// <summary>
        /// Gets coefficients of plane from 3 points
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        private float[] Solve(PointSelector.PointSelector selector)
        {
            if (selector.Count() != 3)
            {
                throw new ArgumentOutOfRangeException("not 3 points provided");
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
