using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Converters;
using System.IO;

namespace RlViewer.Behaviors.ImageAligning
{
    class Aligning : WorkerEventController
    {
        public Aligning(Files.LocatorFile file, PointSelector.CompressedPointSelectorWrapper selector, 
            Behaviors.ImageAligning.IInterpolationProvider rcsProvider,
            Behaviors.ImageAligning.Surfaces.SurfaceType surfaceType, int baseRadius, int layers, double lambda)
        {
            _file = file;     
            _selector = selector;
            _surface = Factories.Surface.SurfaceFactory.CreateSurface(_selector, rcsProvider, surfaceType, baseRadius, layers, lambda);
        }

        public override bool Cancelled
        {
            get
            {
                return _surface.Cancelled;
            }
            set
            {
                _surface.Cancelled = value;
            }
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

        public override event CancelEventHandler CancelJob
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
        private PointSelector.CompressedPointSelectorWrapper _selector;


        public byte[] Resample(string fileName, System.Drawing.Rectangle area)
        {
            OnReportName("Выравнивание изображения");
            return _surface.ResampleImage(_file, area);
        }

        private Files.LocatorFile _file;
    }
}