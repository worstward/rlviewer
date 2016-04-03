using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.PointSelector
{
    public class PointSelector : IEnumerable<SelectedPoint>
    {
        private List<SelectedPoint> selectedPoints = new List<SelectedPoint>();

        public SelectedPoint this[int index]
        {
            get
            {
                return selectedPoints[index];
            }
        }

        public IEnumerator<SelectedPoint> GetEnumerator()
        {
            return selectedPoints.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Orders selected point list as 4x4 matrix
        /// </summary>
        /// <param name="selectedPoints">Selected points list</param>
        /// <returns>Ordered in matrix order list</returns>
        public List<SelectedPoint> OrderAsMatrix(List<SelectedPoint> selectedPoints)
        {
            var sortedList = new List<SelectedPoint>();
            for (int i = 0; i < 4; i++)
            {
                //take each 4 topmost elements from original list, order them by X coordinate,
                //add to new list and remove from original
                sortedList.AddRange(selectedPoints.OrderBy(point => point.Location.Y).Take(4).OrderBy(point => point.Location.X));
                foreach(var item in sortedList)
                {
                    selectedPoints.Remove(item);
                }           
            }
            return sortedList;
        }


        public void Add(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            //16 points required for the algorythm to work properly
            if (selectedPoints.Count < 16)
            {
                if (location.X >= 0 && location.X < file.Width && location.Y >= 0 && location.Y < file.Height)
                {
                    using (Forms.EprInputForm epr = new Forms.EprInputForm())
                    {
                        if (epr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            selectedPoints.Add(new SelectedPoint(file, location, epr.EprValue));
                        }
                    }
                }
                if (selectedPoints.Count == 16)
                {
                    selectedPoints = OrderAsMatrix(selectedPoints);
                }
            }
        }

        public void Add(SelectedPoint selectedPoint)
        {
            selectedPoints.Add(selectedPoint);
        }

        public void RemoveLast()
        {
            if (selectedPoints.Count > 0)
            {
                selectedPoints.RemoveAt(selectedPoints.Count - 1);
            }
        }

    }
}
