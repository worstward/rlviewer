using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Draw
{
    public class Tile
    {
        public Tile(string filePath, PointF leftTopCoord, Size size)
        {
            _filePath = filePath;
            _leftTopCoord = leftTopCoord;
            _size = size;
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
        }

        private Size _size;
        public Size Size
        {
            get { return _size; }
        }

        private PointF _leftTopCoord;
        public PointF LeftTopCoord
        {
            get { return _leftTopCoord; }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
        }

        public void CheckVisibility(PointF leftTopPointOfView, int screenWidth, int screenHeight)
        {
            _isVisible = CheckIntersection(leftTopPointOfView, screenWidth, screenHeight);
        }
     
        private bool CheckIntersection(PointF leftTopPointOfView, int screenWidth, int screenHeight)
        {
            //(b.x2 >= a.x1 && b.x1 <= a.x2) && (b.y2 >= a.y1 && b.y1 <= a.y2)
            if ((leftTopPointOfView.X + screenWidth >= _leftTopCoord.X) && (leftTopPointOfView.X <= _leftTopCoord.X + _size.Width) &&
                (leftTopPointOfView.Y + screenHeight >= _leftTopCoord.Y) && (leftTopPointOfView.Y <= _leftTopCoord.Y + _size.Height))
            {

                return true;
            }
            return false;
            
        }



    }
}
