using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace RlViewer.Behaviors.ImageAligning
{
    public static class LinearEquation
    {

        public static float[] SolveEquation(float[] leftSide, float[] rightSide)
        {         
            float[,] _leftSide = new float[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _leftSide[i, j] = (float)Math.Pow(leftSide[i], 3 - j);
                }
            }


            var leftSideMatrix = Matrix<float>.Build.DenseOfArray(_leftSide);
            var rightSideMatrix = Vector<float>.Build.Dense(rightSide);
            return leftSideMatrix.Solve(rightSideMatrix).ToArray();
        }


    }
}
