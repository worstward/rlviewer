using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.TileCreator
{
    public class Tile
    {
        public Tile(string filePath, Point leftTopCoord, Size tileSize)
        {
            _filePath = filePath;
            _leftTopCoord = leftTopCoord;
            _size = tileSize;
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

        private Point _leftTopCoord;
        public Point LeftTopCoord
        {
            get { return _leftTopCoord; }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
        }

        public bool CheckVisibility(PointF leftTopPointOfView, int screenWidth, int screenHeight)
        {
            _isVisible = CheckIntersection(leftTopPointOfView, screenWidth, screenHeight);
            return _isVisible;
        }
     
        private bool CheckIntersection(PointF leftTopPointOfView, int screenWidth, int screenHeight)
        {
            //(b.x2 >= a.x1 && b.x1 <= a.x2) && (b.y2 >= a.y1 && b.y1 <= a.y2)
            if ((leftTopPointOfView.X + screenWidth > _leftTopCoord.X)  && (leftTopPointOfView.X < _leftTopCoord.X + _size.Width) &&
                (leftTopPointOfView.Y + screenHeight > _leftTopCoord.Y) && (leftTopPointOfView.Y < _leftTopCoord.Y + _size.Height))
            {
                return true;
            }
            return false;   
        }

        /// <summary>
        /// Reads tile data from disk based on given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadData(string path)
        {
            byte[] tile;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    tile = System.IO.File.ReadAllBytes(path);
                }
                catch (System.IO.IOException)
                {
                    tile = Resources.EmptyTile;
                }
            }
            else
            {
                tile = Resources.EmptyTile;
            }

            //if tile doesn't exist, return empty tile
            return tile;
        }
    }
}
