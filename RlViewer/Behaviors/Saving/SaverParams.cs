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
        public SaverParams(string path, Point leftTop, int width, int height, Behaviors.TileCreator.TileOutputType outputType,
            Behaviors.Filters.ImageFilterProxy imageFilter, System.Drawing.Imaging.ColorPalette palette)
        {
            _path = path;
            _leftTop = leftTop;
            _width = width;
            _height = height;
            _outputType = outputType;
            _imageFilter = imageFilter;
            _palette = palette;
        }


        private string _path;
        public string Path
        {
            get { return _path; }
        }

        public FileType DestinationType
        {
            get
            {
                return (FileType)Enum.Parse(typeof(FileType), System.IO.Path.GetExtension(_path).ToLowerInvariant().TrimStart('.'));
            }
        }
        
        public System.Drawing.Rectangle SavingArea
        {
        
            get
            {
                
                return new Rectangle(_leftTop.X, _leftTop.Y, _width, _height);
            }
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


        private Behaviors.TileCreator.TileOutputType _outputType;

        public Behaviors.TileCreator.TileOutputType OutputType
        {
            get { return _outputType; }
        }


        private Behaviors.Filters.ImageFilterProxy _imageFilter;
        public Behaviors.Filters.ImageFilterProxy ImageFilter
        {
            get
            {
                return _imageFilter;
            }
            set
            {
                _imageFilter = value;
            }
        }


        private System.Drawing.Imaging.ColorPalette _palette;
        public System.Drawing.Imaging.ColorPalette Palette
        {
            get
            {
                return _palette;
            }
            set
            {
                _palette = value;
            }
        }


    }
}
