using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    class Aligning : PointReader
    {
        public Aligning(PointSelector.PointSelector selector, Files.LocatorFile file)
        {
            _file = file;

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

            ResampleImage();
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

        public void ResampleImage()
        {
            float[] image = new float[_file.Width * _file.Height];
            byte[] imageB = new byte[image.Length * 4];

            //iterate over X axis
            Parallel.For(0, _file.Width, (i) =>
            {

                var zValues = _zCoefficients.Select(x => Extrapolate(i, x)).ToArray();
                var yValues = _yCoefficients.Select(x => Extrapolate(i, x)).ToArray();
                var _zCoefs = new LinearEquation(yValues, zValues).Solution;
             
                for (int j = 0; j < _file.Height; j++)
                {

                    
                        var oldVal = GetValue(_file, new System.Drawing.Point(i, j));
                        var newVal = Extrapolate(j, _zCoefs);

                        var diff = oldVal / newVal;
                        diff = diff < 0 ? 0 : diff;
                        image[j * _file.Width + i] = diff;
                }

            });

            Buffer.BlockCopy(image, 0, imageB, 0, imageB.Length);

            System.IO.File.WriteAllBytes("img.raw", imageB);
        }



        private float Extrapolate(int sample, float[] solution)
        {
            if (solution.GetLength(0) != 4)
            {
                throw new ArgumentException();
            }

            //(A*x^3 + B*x^2 + C*x + D = z) to find z with provided x sample and ABCD coef in solution[]
            float extrapolatedValue = 0;

            for(int j = 3; j >= 0; j--)
            {
                extrapolatedValue += (float)Math.Pow(sample, 3 - j) * solution[j];
            }
            return extrapolatedValue;
        }






    }
}
