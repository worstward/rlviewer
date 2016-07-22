using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Saving
{
    public class SaverParams
    {
        public SaverParams(string path, Point leftTop, int width, int height, bool keepFiltering)
        {
            _path = path;
            _leftTop = leftTop;
            _width = width;
            _height = height;
            _keepFiltering = keepFiltering;
        }


        private string _path;
        public string Path
        {
            get { return _path; }
        }

        private Point _leftTop;
        public Point LeftTop
        {
            get { return _leftTop; }
        }

        private int _width;
        public int Width
        {
            get { return _width; }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
        }

        private bool _keepFiltering;
        public bool KeepFiltering
        {
            get { return _keepFiltering; }
        }

    }
}
