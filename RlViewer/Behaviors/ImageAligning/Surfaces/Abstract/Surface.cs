using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.PointSelector;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Abstract
{
    /// <summary>
    /// Surface based on provided points
    /// </summary>
    public abstract class Surface : WorkerEventController
    {
        public Surface(PointSelector.CompressedPointSelectorWrapper selector)
        {
            Selector = selector;
            Selector.SetSelector(new Behaviors.PointSelector.PointSelector(OrderAsMatrix(selector.CompessedSelector.ToList())));
        }

        protected PointSelector.CompressedPointSelectorWrapper Selector
        {
            get;
            private set;
        }

        protected abstract IInterpolationProvider RcsProvider { get; }

        /// <summary>
        /// Changes image amplitudes with prebuilt surface
        /// </summary>
        /// <param name="file">Image file</param>
        /// <param name="area">Working area</param>
        /// <returns>Resampled area</returns>
        public abstract byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area);



        /// <summary>
        /// Orders selected point list as a square matrix
        /// </summary>
        /// <param name="selectedPoints">Selected points list</param>
        /// <returns>Ordered in matrix order list</returns>
        protected virtual IList<SelectedPoint> OrderAsMatrix(IList<SelectedPoint> selectedPoints)
        {
            var sortedList = new List<SelectedPoint>();
            var selected = selectedPoints.Select(x => x).ToList();

            var matrixDimension = (int)Math.Sqrt(selectedPoints.Count());

            if (matrixDimension * matrixDimension != selected.Count)
            {
                return selectedPoints;
            }

            for (int i = 0; i < matrixDimension; i++)
            {
                //take each sqrt(elementCount) topmost elements from original list, order them by X coordinate,
                //add to new list and remove from original
                sortedList.AddRange(
                    selected
                    .OrderBy(point => point.Location.Y)
                    .Take(matrixDimension)
                    .OrderBy(point => point.Location.X));

                foreach (var item in sortedList)
                {
                    selected.Remove(item);
                }
            }

            return sortedList;
        }





    }
}
