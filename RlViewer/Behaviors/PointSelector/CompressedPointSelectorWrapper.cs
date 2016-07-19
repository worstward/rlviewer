using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.PointSelector
{
    public class CompressedPointSelectorWrapper : IEnumerable<SelectedPoint>
    {
        public CompressedPointSelectorWrapper(Files.LocatorFile file, IEnumerable<SelectedPoint> selector, int rangeCompressionCoef, int azimuthCompressionCoef)
        {
            _file = file;
            _rangeCompressionCoef = rangeCompressionCoef;
            _azimuthCompressionCoef = azimuthCompressionCoef;
            SetCompressedSelector(selector);
        }

        public IEnumerator<SelectedPoint> GetEnumerator()
        {
            return CompessedSelector.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public SelectedPoint this[int index]
        {
            get
            {
                return CompessedSelector[index];
            }
        }

        private Files.LocatorFile _file;


        private PointSelector _compressedSelector;

        public PointSelector CompessedSelector
        {
            get 
            {
                return _compressedSelector;
            }
        }

        private int _rangeCompressionCoef;

        public int RangeCompressionCoef
        {
            get { return _rangeCompressionCoef; }
        }

        private int _azimuthCompressionCoef;

        public int AzimuthCompressionCoef
        {
            get { return _azimuthCompressionCoef; }
        }


        public void SetCompressedSelector(IEnumerable<SelectedPoint> selector)
        {
            var compressed = new List<SelectedPoint>();
            compressed.AddRange(selector.Select(x =>
                new SelectedPoint(new Point(x.Location.X / _rangeCompressionCoef, x.Location.Y / _azimuthCompressionCoef),
                    GetAverageValue(_file, _rangeCompressionCoef, _azimuthCompressionCoef, x.Location), x.Rcs)));

            _compressedSelector = new PointSelector(compressed);
        }

        public void SetSelector(IEnumerable<SelectedPoint> selector)
        {
            var compressed = new List<SelectedPoint>();
            compressed.AddRange(selector.Select(x =>
                new SelectedPoint(new Point(x.Location.X, x.Location.Y),
                    GetAverageValue(_file, _rangeCompressionCoef, _azimuthCompressionCoef, x.Location), x.Rcs)));

            _compressedSelector = new PointSelector(compressed);
        }


        private float GetAverageValue(RlViewer.Files.LocatorFile file, int rangeCompressionCoef, int azimuthCompressionCoef, Point centerPoint)
        {
            float avgValue = 0;
            if (rangeCompressionCoef % 2 != 0 && azimuthCompressionCoef % 2 != 0)
            {
                avgValue = GetOddStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, centerPoint);
            }
            else if(rangeCompressionCoef % 2 == 0 && azimuthCompressionCoef == 0)
            {
                avgValue = GetEvenStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, centerPoint);
            }
            else if (rangeCompressionCoef % 2 == 0 && azimuthCompressionCoef != 0)
            {
                avgValue = GetEvenOddStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, centerPoint);
            }
            else if (rangeCompressionCoef % 2 != 0 && azimuthCompressionCoef == 0)
            {
                avgValue = GetOddEvenStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, centerPoint);
            }
            return avgValue;
        }

        /// <summary>
        /// Compresses area to 1 point with both 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rangeCompressionCoef"></param>
        /// <param name="azimuthCompressionCoef"></param>
        /// <param name="centerPoint">Clicked point (center of area)</param>
        /// <returns></returns>
        private float GetEvenStepAverage(RlViewer.Files.LocatorFile file, int rangeCompressionCoef, int azimuthCompressionCoef, Point centerPoint)
        {
            var leftBorder = new Point(centerPoint.X - rangeCompressionCoef + 1, centerPoint.Y - azimuthCompressionCoef + 1);
            List<float> samples = new List<float>();

            for (int startingFrameX = leftBorder.X; startingFrameX < leftBorder.X + rangeCompressionCoef; startingFrameX++)
            {
                for (int startingFrameY = leftBorder.Y; startingFrameY < leftBorder.Y + azimuthCompressionCoef; startingFrameY++)
                {
                    samples.Add(GetOddStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, new Point(startingFrameX, startingFrameY)));
                }
            }
            return samples.Max();
        }


        /// <summary>
        /// Compresses area to 1 point with both odd range and azimuth compression coefs
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rangeCompressionCoef"></param>
        /// <param name="azimuthCompressionCoef"></param>
        /// <param name="centerPoint">Clicked point (center of area)</param>
        /// <returns></returns>
        private float GetOddStepAverage(RlViewer.Files.LocatorFile file, int rangeCompressionCoef, int azimuthCompressionCoef, Point centerPoint)
        {
           
            float compressedSample = 0;
            int checkedSamplesNum = 0;

            for (int j = centerPoint.Y - azimuthCompressionCoef / 2; j <= centerPoint.Y + azimuthCompressionCoef / 2; j++)
            {
                for (int i = centerPoint.X - rangeCompressionCoef / 2; i <= centerPoint.X + rangeCompressionCoef / 2; i++)
                {
                    if (i < 0 || j < 0 || i >= file.Width || j >= file.Height)
                    {
                        continue;
                    }

                    checkedSamplesNum++;
                    compressedSample += file.GetSample(new System.Drawing.Point(i, j))
                        .ToFileSample(file.Properties.Type, file.Header.BytesPerSample);
                }
            }
            compressedSample /= checkedSamplesNum;

            return compressedSample;
        }



        /// <summary>
        /// Compresses area to 1 point with even range coef and odd azimuth coef 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rangeCompressionCoef"></param>
        /// <param name="azimuthCompressionCoef"></param>
        /// <param name="centerPoint">Clicked point (center of area)</param>
        /// <returns></returns>
        private float GetEvenOddStepAverage(RlViewer.Files.LocatorFile file, int rangeCompressionCoef, int azimuthCompressionCoef, Point centerPoint)
        {
            var leftBorder = new Point(centerPoint.X - rangeCompressionCoef + 1, centerPoint.Y - azimuthCompressionCoef + 1);
            List<float> samples = new List<float>();

            for (int startingFrameX = leftBorder.X; startingFrameX < leftBorder.X + rangeCompressionCoef; startingFrameX++)
            {
               samples.Add(GetOddStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, new Point(startingFrameX, centerPoint.Y)));
            }
            return samples.Max();
        }

        /// <summary>
        /// Compresses area to 1 point with odd range coef and even azimuth coef 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rangeCompressionCoef"></param>
        /// <param name="azimuthCompressionCoef"></param>
        /// <param name="centerPoint">Clicked point (center of area)</param>
        /// <returns></returns>
        private float GetOddEvenStepAverage(RlViewer.Files.LocatorFile file, int rangeCompressionCoef, int azimuthCompressionCoef, Point centerPoint)
        {
            var leftBorder = new Point(centerPoint.X - rangeCompressionCoef + 1, centerPoint.Y - azimuthCompressionCoef + 1);
            List<float> samples = new List<float>();

            for (int startingFrameY = leftBorder.Y; startingFrameY < leftBorder.Y + azimuthCompressionCoef; startingFrameY++)
            {
                samples.Add(GetOddStepAverage(file, rangeCompressionCoef, azimuthCompressionCoef, new Point(centerPoint.X, startingFrameY)));
            }
            
            return samples.Max();
        }


    }
}
