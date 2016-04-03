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
                solutions[i] = new LinearEquation(
                //new float[]{10,20,30,40},
                //new float[]{4,3,2,1})
                        selector.Skip(i * 4).Take(4).Select(x => (float)x.Location.X).ToArray(),
                        selector.Skip(i * 4).Take(4).Select(x => x.Value).ToArray())
                    .Solution;
            }

            var xSample = selector.First().Location.X;
            var a = SolveForZ(xSample);
        }

        private float[][] solutions = new float[4][];


        private float[] SolveForZ(int x)
        {
            var amplitudes = new float[4];

            for (int i = 0; i < 4; i++)
            {
                for(int j = 3; j >= 0; j--)
                {
                    amplitudes[i] += (float)Math.Pow(solutions[i][3 - j], j);
                }
            }
            return amplitudes;
        }

    }
}
