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

        public PointSelector(IList<SelectedPoint> points)
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

                            var selectedPoint = new SelectedPoint(file, file.GetMaxSampleLocation(area), epr.EprValue);

                            if (!SelectedPoints.Any(p => p.Equals(selectedPoint.Location)))
                            {
                                SelectedPoints.Add(selectedPoint);
                            }        
                        }
                    }
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
