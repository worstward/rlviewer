using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.PointSelector
{
    public class PointSelector : IEnumerable<SelectedPoint>
    {

        public PointSelector()
        {
 
        }

        private PointSelector(IList<SelectedPoint> points)
        {
            _selectedPoints = points;
        }


        private IList<SelectedPoint> _selectedPoints = new List<SelectedPoint>();

        protected IList<SelectedPoint> SelectedPoints
        {
            get { return _selectedPoints; }
            set { _selectedPoints = value; }
        }


        public SelectedPoint this[int index]
        {
            get
            {
                return SelectedPoints[index];
            }
        }

        public IEnumerator<SelectedPoint> GetEnumerator()
        {
            return SelectedPoints.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        /// <summary>
        /// Orders selected point list as a square matrix
        /// </summary>
        /// <param name="selectedPoints">Selected points list</param>
        /// <returns>Ordered in matrix order list</returns>
        protected IList<SelectedPoint> OrderAsMatrix(IList<SelectedPoint> selectedPoints)
        {
            var sortedList = new List<SelectedPoint>();
            var matrixDimension = (int)Math.Sqrt(selectedPoints.Count);
            if (matrixDimension * matrixDimension != selectedPoints.Count)
            {
                throw new ArgumentOutOfRangeException("selectedPoints.Count");
            }


            for (int i = 0; i < matrixDimension; i++)
            {
                //take each sqrt(elementCount) topmost elements from original list, order them by X coordinate,
                //add to new list and remove from original
                sortedList.AddRange(
                    selectedPoints
                    .OrderBy(point => point.Location.Y)
                    .Take(matrixDimension)
                    .OrderBy(point => point.Location.X));
                foreach(var item in sortedList)
                {
                    selectedPoints.Remove(item);
                }           
            }
            return sortedList;
        }

        //public PointSelector AverageAmplitudes()
        //{
        //    List<SelectedPoint> lst = new List<SelectedPoint>();

        //    foreach (var grp in _selectedPoints.GroupBy(s => s.Rcs))
        //    {
        //        foreach (var item in grp)
        //        {
        //            lst.Add(new SelectedPoint(item.Location, grp.Average(x => (float)x.Value), item.Rcs));
        //        }
        //    }
        //    return new PointSelector(lst);
        //}



        public virtual void Add(RlViewer.Files.LocatorFile file, System.Drawing.Point location, System.Drawing.Size selectorSize)
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

                            SelectedPoints.Add(new SelectedPoint(file,
                                file.GetMaxSampleLocation(area), epr.EprValue));
                        }
                    }
                }
                if (SelectedPoints.Count == 4 || SelectedPoints.Count == 16)
                {
                    SelectedPoints = OrderAsMatrix(SelectedPoints);
                }
            }
        }

        public void Add(SelectedPoint selectedPoint)
        {
            SelectedPoints.Add(selectedPoint);
        }

        public void RemoveLast()
        {
            if (SelectedPoints.Count > 0)
            {
                SelectedPoints.RemoveAt(SelectedPoints.Count - 1);
            }
        }

    }
}
