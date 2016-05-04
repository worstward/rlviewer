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

            if (_emptyTile == null)
            {
               _emptyTile = new Lazy<byte[]>(() => { return new byte[tileSize.Width * tileSize.Height]; });
            }
        }

        private static Lazy<byte[]> _emptyTile;

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

        public bool CheckVisibility(PointF leftTopPointOfView, int screenWidth, int screenHeight)
        {
            return CheckIntersection(leftTopPointOfView, screenWidth, screenHeight);
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
        public byte[] ReadData()
        {
            byte[] tile;
            if (System.IO.File.Exists(_filePath))
            {
                tile = System.IO.File.ReadAllBytes(_filePath);
            }
            else
            {
                tile = _emptyTile.Value;
            }

            //if tile doesn't exist, return empty tile
            return tile;
        }
    }
}
