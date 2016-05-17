using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Abstract
{
    /// <summary>
    /// Surface based on provided points
    /// </summary>
    public abstract class Surface : WorkerEventController
    {
        public Surface(PointSelector.PointSelector selector)
        {
            Selector = selector;
        }

        protected PointSelector.PointSelector Selector
        {
            get;
            private set;
        }

        protected abstract IRcsDependenceProvider RcsProvider { get; }

        /// <summary>
        /// Changes image amplitudes with prebuilt surface
        /// </summary>
        /// <param name="file">Image file</param>
        /// <param name="area">Working area</param>
        /// <returns>Resampled area</returns>
        public abstract byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area);

    }
}
