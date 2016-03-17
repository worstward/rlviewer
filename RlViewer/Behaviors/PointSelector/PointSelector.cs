using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.PointSelector
{
    public class PointSelector
    {
        private List<SelectedPoint> selectedPoints = new List<SelectedPoint>();

        public IEnumerator<SelectedPoint> GetEnumerator()
        {
            return selectedPoints.GetEnumerator();
        }

        public void Sort()
        {
            selectedPoints.OrderBy(x => x.Location.X).ThenBy(x => x.Location.Y);
        }


        public void Add(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            //16 points required for the algorythm to work properly
            if (selectedPoints.Count <= 16)
            {
                if (location.X > 0 && location.X < file.Width && location.Y > 0 && location.Y < file.Height)
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
                    Sort();
                }
            }
        }

        public void Add(SelectedPoint selectedPoint)
        {
            selectedPoints.Add(selectedPoint);
        }

        //public void AddFileVal(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        //{
        //    //if we hit the image
        //    if (location.X > 0 && location.X < file.Width && location.Y > 0 && location.Y < file.Height)
        //    {
        //        selectedPoints.Add(new SelectedPoint(file, location));
        //    }
        //}

        public void RemoveLast()
        {
            if (selectedPoints.Count > 0)
            {
                selectedPoints.RemoveAt(selectedPoints.Count - 1);
            }
        }

    }
}
