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
        public Tile(string pathToFile, PointF leftTopCoord, Size size)
        {
            _pathToFile = pathToFile;
            _leftTopCoord = leftTopCoord;
            _size = size;
        }

        private string _pathToFile;
        public string PathToFile
        {
            get { return _pathToFile; }
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
            //(X2' >= X1 && X1' <= X2) && (Y2' >= Y1 && Y1' <= Y2)
            if ((leftTopPointOfView.X + screenWidth >= _leftTopCoord.X) && (leftTopPointOfView.X <= _leftTopCoord.X + _size.Width) &&
                (leftTopPointOfView.Y + screenHeight >= _leftTopCoord.Y) && (leftTopPointOfView.Y <= _leftTopCoord.Y + _size.Height))
            {

                return true;
            }
            return false;
            
        }



    }
}
