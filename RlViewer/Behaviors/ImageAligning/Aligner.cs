using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    class Aligning : WorkerEventController
    {
        public Aligning(Files.LocatorFile file, PointSelector.PointSelector selector)
        {
            _file = file;     
            _selector = selector;
            _surface = Surfaces.Abstract.SurfaceFactory.CreateSurface(_selector);
        }

        public override event ReportProgress Report
        {
            add 
            {
                _surface.Report += value;
            }
            remove
            {
                _surface.Report -= value;
            }
        }

        public override event EventHandler<CancelEventArgs> CancelJob
        {
            add
            {
                _surface.CancelJob += value;
            }
            remove
            {
                _surface.CancelJob -= value;
            }
        }


        private Surfaces.Abstract.Surface _surface;
        private PointSelector.PointSelector _selector;


        public void Resample(string fileName)
        {
            System.IO.File.WriteAllBytes(fileName, _surface.ResampleImage(_file, GetArea(_selector)));
        }
     

        private Files.LocatorFile _file;

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








    }
}