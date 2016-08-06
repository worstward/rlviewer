using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RlViewer.Behaviors.AreaSelector
{
    public class AreaSelectorDecorator : AreaSelector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">Locator file that's opened</param>
        /// <param name="maxAreaSize">Max permitted area size</param>
        public AreaSelectorDecorator(Files.LocatorFile file, int maxAreaSize) : base(file)
        {
            _file = file;
            _maxAreaSize = maxAreaSize;
        }

        private Files.LocatorFile _file;
        private PointSelector.SelectedPoint _selectedPoint;
        private int _maxAreaSize;


        /// <summary>
        /// Aligner selected point based on highest area sample
        /// </summary>
        public PointSelector.SelectedPoint SelectedPoint
        {
            get { return _selectedPoint; }
        }



        /// <summary>
        /// Stops resizing area size and creates aligner selected point from max sample
        /// </summary>
        /// <param name="rcsValue"></param>
        public void StopResizing(float rcsValue)
        {
            IsActive = false;

            if (Area.Width * Area.Height > _maxAreaSize)
            {
                return;
            }

            if (Area.Location.X > _file.Width || Area.Location.Y > _file.Height || Area.Width <= 0 || Area.Height <= 0)
            {
                return;
            }

            
            var area = new System.Drawing.Rectangle(
                Area.Location.X, Area.Location.Y, Area.Width, Area.Height);
            _selectedPoint = new PointSelector.SelectedPoint(_file.GetMaxSampleLocation(area),
                _file.GetMaxSample(area), GetSplashedRcs(rcsValue));
            
        }

        /// <summary>
        /// Gets portion of objects rcs that suits maximum sample
        /// </summary>
        /// <param name="rcs">Full object rcs</param>
        /// <returns></returns>
        private float GetSplashedRcs(float rcs)
        {
            var maxSample = _file.GetMaxSample(new System.Drawing.Rectangle(
                Area.Location.X, Area.Location.Y, Area.Width, Area.Height));

            var area = _file.GetArea(new System.Drawing.Rectangle(
                Area.Location.X, Area.Location.Y, Area.Width, Area.Height));
            var sampleArea = area.ToArea<float>(_file.Header.BytesPerSample);
            var cumulativeSamples = sampleArea.Sum();

            return maxSample / cumulativeSamples * rcs;
        }



    }
}
