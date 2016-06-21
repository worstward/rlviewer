using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.PointSelector
{
    class CompressedPointSelectorWrapper : PointSelector
    {
        public CompressedPointSelectorWrapper(int decompositionStepCoef)
        {
            _decompositionStepCoef = decompositionStepCoef;
        }

        private int _decompositionStepCoef;

        public override void Add(RlViewer.Files.LocatorFile file, System.Drawing.Point location, System.Drawing.Size selectorSize)
        {
            if (SelectedPoints.Count < 16)
            {
                if (location.X >= 0 && location.X < file.Width && location.Y >= 0 && location.Y < file.Height)
                {
                    using (Forms.EprInputForm epr = new Forms.EprInputForm())
                    {
                        if (epr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {

                            int width = selectorSize.Width;
                            int height = selectorSize.Height;


                            int x = (location.X - (selectorSize.Width / 2));

                            if (x < 0)
                            {
                                width = width + x;
                                x = 0;
                            }

                            if (x + width > file.Width)
                            {
                                width = file.Width - x;
                            }

                            int y = (location.Y - (selectorSize.Height / 2));

                            if (y < 0)
                            {
                                height = height + y;
                                y = 0;
                            }

                            if (y + height > file.Height)
                            {
                                height = file.Height - y;
                            }

                            System.Drawing.Rectangle area = new System.Drawing.Rectangle(x, y, width, height);
                            var maxSampleLoc = file.GetMaxSampleLocation(area);
                            var xCompressionGroup = maxSampleLoc.X / _decompositionStepCoef * _decompositionStepCoef;
                            float compressedSample = 0;

                            for (int i = xCompressionGroup; i < xCompressionGroup + _decompositionStepCoef; i++)
                            {
                                compressedSample += file.GetSample(new System.Drawing.Point(i, maxSampleLoc.Y))
                                    .ToFileSample(file.Properties.Type, file.Header.BytesPerSample);
                            }
                            compressedSample /= _decompositionStepCoef;

                            SelectedPoints.Add(new SelectedPoint(maxSampleLoc, compressedSample, epr.EprValue));
                        }
                    }
                }
                if (SelectedPoints.Count == 4 || SelectedPoints.Count == 16)
                {
                    SelectedPoints = OrderAsMatrix(SelectedPoints);
                }
            }
        }




    }
}
