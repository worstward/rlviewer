using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    class Aligning
    {
        public Aligning(PointSelector.PointSelector selector)
        {
            for (int i = 0; i < 4; i++)
            {
                solutionsZ[i] = new LinearEquation(
                //new float[]{10,20,30,40},
                //new float[]{4,3,2,1})
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        selector.Skip(i * 4).Take(4).Select(x => x.Value).ToArray())
                    .Solution;

                solutionsY[i] = new LinearEquation(
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.Y).ToArray())
                    .Solution;

            }

            var xSample = selector.First().Location;
            var a = Extrapolate(xSample.X, solutionsZ);
            var b = Extrapolate(xSample.X, solutionsY);
        }

        /// <summary>
        /// Contains x-z linear equation coefficients (A*x^3 + B*x^2 + C*x + D = z)
        /// /// </summary>
        private float[][] solutionsZ = new float[4][];

        /// <summary>
        /// Contains x-y linear equation coefficients (A*x^3 + B*x^2 + C*x + D = y)
        /// </summary>
        private float[][] solutionsY = new float[4][];



        private float[] Extrapolate(int x, float[][] solution)
        {
            if (solution.GetLength(0) != 4)
            {
                throw new ArgumentException();
            }

            var amplitudes = new float[4];

            for (int i = 0; i < 4; i++)
            {
                for(int j = 3; j >= 0; j--)
                {
                    amplitudes[i] += (float)Math.Pow(x, j) * solution[i][3 - j];
                }
            }
            return amplitudes;
        }






    }
}
