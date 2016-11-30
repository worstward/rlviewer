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
            _width = tileSize.Width;
            _height = tileSize.Height;

            if (_emptyTile == null)
            {
               _emptyTile = new Lazy<byte[]>(() => { return new byte[tileSize.Width * tileSize.Height]; });
            }
        }

        private static Lazy<byte[]> _emptyTile;

        private string _filePath;
        public string FilePath
        {
            get
            { 
                return _filePath;
            }
        }

        private int _width;

        private int _height;

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
            if ((leftTopPointOfView.X + screenWidth > _leftTopCoord.X)  && (leftTopPointOfView.X < _leftTopCoord.X + _width) &&
                (leftTopPointOfView.Y + screenHeight > _leftTopCoord.Y) && (leftTopPointOfView.Y < _leftTopCoord.Y + _height))
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
            if (TileExists(_filePath))
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



        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private extern static bool PathFileExists(StringBuilder filePath);

        private bool TileExists(string filePath)
        {
            var stringBuilder = new StringBuilder(filePath);
            return PathFileExists(stringBuilder);
        }

    }
}
