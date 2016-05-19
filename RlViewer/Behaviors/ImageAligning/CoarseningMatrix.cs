using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    public class CoarseningMatrix
    {
        /// <summary>
        /// Matrix, which cells dimensions are 1 meter for both range and azimuth
        /// </summary>
        /// <param name="dx">Range decomposition step in meters</param>
        /// <param name="dy">Azimuth decomposition step in meters</param>
        public CoarseningMatrix(Files.LocatorFile file, float dx, float dy)
        {
            _file = file;
            _matrixCellWidthInPx  = GetSidePixelCount(dx);
            _matrixCellHeightInPx = GetSidePixelCount(dy);
        }

        private int _matrixCellWidthInPx;
        private int _matrixCellHeightInPx;
        private Files.LocatorFile _file;

        /// <summary>
        /// Gets amount of pixels needed to fill 1 meter
        /// </summary>
        /// <returns></returns>
        private int GetSidePixelCount(float step)
        {
            return (int)Math.Ceiling(1 / step);
        }


        /// <summary>
        /// Gets image with lowered resolution according to matrix cell size
        /// </summary>
        /// <param name="image">Array containing image</param>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <returns></returns>
        public byte[] ApplyMatrix(byte[] image, int width, int height)
        {
            var coarseMatrixCellArea = _matrixCellHeightInPx * _matrixCellWidthInPx;

            byte[] coarsedImage = new byte[_file.Width / _matrixCellWidthInPx * _file.Height / _matrixCellHeightInPx];
            int index = 0;

            for (int i = 0; i < height * width - width; i += _matrixCellHeightInPx * width)
            {
                for (int j = i; j < i + width; j += _matrixCellWidthInPx)
                {
                    float cumulative = 0;
                    for (int n = j; n < j + width * _matrixCellHeightInPx; n += width)
                    {
                        for (int k = n; k < n + _matrixCellWidthInPx; k++)
                        {
                            cumulative += image[k];
                        }
                    }
                    coarsedImage[index++] = (byte)(cumulative / coarseMatrixCellArea);
                }
            }

            return coarsedImage;
        }


    }
}
