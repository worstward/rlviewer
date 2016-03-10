using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.PointSelector
{
    public class PointSelector
    {
        private List<SelectedPoint> _selectedPoints = new List<SelectedPoint>();

        public IEnumerator<SelectedPoint> GetEnumerator()
        {
            return _selectedPoints.GetEnumerator();
        }

        public void AddManualVal(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            if (location.X > 0 && location.X < file.Width && location.Y > 0 && location.Y < file.Height)
            {
                using (Forms.EprInputForm epr = new Forms.EprInputForm())
                {
                    if (epr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _selectedPoints.Add(new SelectedPoint(file, location, epr.EprValue));
                    }
                }                
            }
        }

        public void Add(SelectedPoint selectedPoint)
        {
            _selectedPoints.Add(selectedPoint);
        }

        //public void AddFileVal(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        //{
        //    //if we hit the image
        //    if (location.X > 0 && location.X < file.Width && location.Y > 0 && location.Y < file.Height)
        //    {
        //        _selectedPoints.Add(new SelectedPoint(file, location));
        //    }
        //}

        public void RemoveLast()
        {
            if (_selectedPoints.Count > 0)
            {
                _selectedPoints.RemoveAt(_selectedPoints.Count - 1);
            }
        }

    }
}
