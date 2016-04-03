using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace RlViewer.Behaviors.ImageAligning
{
    public class LinearEquation
    {
        public LinearEquation(float[] leftSide, float[] rightSide)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _leftSide[i, j] = (float)Math.Pow(leftSide[i], 3 - j);
                }
            }

                _rightSide = rightSide;
        }



        private float[,] _leftSide = new float[4, 4];
        private float[] _rightSide;

        private float[] _solution;
        public float[] Solution
        {
            get
            {
                return _solution = _solution ?? SolveEquation(_leftSide, _rightSide);
            }
        }

        private float[] SolveEquation(float[,] leftSide, float[] rightSide)
        {
            var leftSideMatrix = Matrix<float>.Build.DenseOfArray(leftSide);
            var rightSideMatrix = Vector<float>.Build.Dense(rightSide);
            return leftSideMatrix.Solve(rightSideMatrix).ToArray();
        }


    }
}
