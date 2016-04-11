using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    class Aligning : WorkerEventController
    {
        public Aligning(Files.LocatorFile file)
        {
            _file = file;         
        }


        public void Resample(PointSelector.PointSelector selector, string fileName)
        {
            for (int i = 0; i < 4; i++)
            {
                _zCoefficients[i] = new LinearEquation(
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        selector.Skip(i * 4).Take(4).Select(x => x.Value).ToArray())
                    .Solution;

                _yCoefficients[i] = new LinearEquation(
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.Y).ToArray())
                    .Solution;

            }
   
            _areaBorders = GetArea(selector);
   
            System.IO.File.WriteAllBytes(fileName, ResampleImage());
        }


        /// <summary>
        /// Contains x-z linear equation coefficients (A*x^3 + B*x^2 + C*x + D = z)
        /// /// </summary>
        private float[][] _zCoefficients  = new float[4][];

        /// <summary>
        /// Contains x-y linear equation coefficients (A*x^3 + B*x^2 + C*x + D = y)
        /// </summary>
        private float[][] _yCoefficients = new float[4][];
        private Files.LocatorFile _file;
        private System.Drawing.Rectangle _areaBorders;
        private const int _workingAreaSize = 4000;

        private System.Drawing.Rectangle GetArea(PointSelector.PointSelector selector)
        {

            var minX = selector.Min(p => p.Location.X);
            var maxX = selector.Max(p => p.Location.X);
            var minY = selector.Min(p => p.Location.Y);
            var maxY = selector.Max(p => p.Location.Y);

            int areaWidth = maxX - minX;
            int areaHeight = maxY - minY;


            if (areaWidth < _workingAreaSize)
            {
                minX = minX - (_workingAreaSize - areaWidth) / 2;
                minX = minX < 0 ? 0 : minX;
                areaWidth = _workingAreaSize;
            }

            if (areaHeight < _workingAreaSize)
            {
                minY = minY - (_workingAreaSize - areaHeight) / 2;
                minY = minY < 0 ? 0 : minY;
                areaHeight = _workingAreaSize;
            }

            return new System.Drawing.Rectangle(minX, minY, areaWidth, areaHeight);

        }


        private byte[] ResampleImage()
        {
            float[] image = new float[_areaBorders.Width * _areaBorders.Height];
            byte[] imageB = new byte[image.Length * 4];

            //iterate over X axis

            int toInclusiveX = _areaBorders.Location.X + _areaBorders.Width;
            toInclusiveX = toInclusiveX > _file.Width ? _file.Width : toInclusiveX;

            int toInclusiveY = _areaBorders.Location.Y + _areaBorders.Height;
            toInclusiveY = toInclusiveY > _file.Height ? _file.Height : toInclusiveY;
            int counter = 0;


            float[] imageArea = Behaviors.FileReader.GetArea(_file, _areaBorders);

            Parallel.For(_areaBorders.Location.X, toInclusiveX, (i) =>
            {
                var zValues = _zCoefficients.Select(x => Extrapolate(i, x)).ToArray();
                var yValues = _yCoefficients.Select(x => Extrapolate(i, x)).ToArray();
                var _zCoefs = new LinearEquation(yValues, zValues).Solution;

                for (int j = _areaBorders.Location.Y; j < toInclusiveY; j++)
                {
                    var oldVal = imageArea[(j - _areaBorders.Location.Y) 
                        * _areaBorders.Width + (i - _areaBorders.Location.X)];
                    var newVal = Extrapolate(j, _zCoefs);

                    var diff = oldVal / newVal;
                    image[(j - _areaBorders.Location.Y) * _areaBorders.Width + (i - _areaBorders.Location.X)] = diff;
                    diff = diff < 0 ? 0 : diff;
                }

                System.Threading.Interlocked.Increment(ref counter);
                OnProgressReport((int)(counter / Math.Ceiling((double)(toInclusiveX - _areaBorders.Location.X)) * 100));
                if (OnCancelWorker())
                {
                    return;
                }

            });

            if (Cancelled)
            {
                return null;
            }


            Buffer.BlockCopy(image, 0, imageB, 0, imageB.Length);

            return imageB;
        }



        private float Extrapolate(int sample, float[] solution)
        {
            if (solution.GetLength(0) != 4)
            {
                throw new ArgumentException();
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