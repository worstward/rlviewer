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

        public void Add(System.Drawing.Point location, float value)
        {
            _selectedPoints.Add(new SelectedPoint(location, value));
        }

        public void Add(SelectedPoint selectedPoint)
        {
            _selectedPoints.Add(selectedPoint);
        }


        public void Add(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            _selectedPoints.Add(new SelectedPoint(file, location));
        }


        public void RemoveLast()
        {
            _selectedPoints.RemoveAt(_selectedPoints.Count - 1);
        }



    }
}
