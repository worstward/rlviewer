using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;
using RlViewer.Behaviors.TileCreator;


namespace RlViewer.Behaviors.Draw
{

    /// <summary>
    /// Incapsulates functions of drawing image tiles and tools
    /// </summary>
    class DrawerFacade
    {
        public DrawerFacade(Size screenSize, ItemDrawer iDrawer, TileDrawer tDrawer)
        {
            _screenSize = screenSize;
            _iDrawer = iDrawer;
            _tDrawer = tDrawer;
        }

        private ItemDrawer _iDrawer;
        private TileDrawer _tDrawer;
        private Size _screenSize;
        
        private Image _canvas;

        public ColorPalette Palette
        {
            get
            {
                return _tDrawer.Palette;
            }
        }

        public void GetPalette(float R, float G, float B, bool reversed, bool isGroupped, bool useTemperaturePalette)
        {
            _tDrawer.GetPalette(R, G, B, reversed, isGroupped, useTemperaturePalette);
        }

        /// <summary>
        /// Draws visible tiles
        /// </summary>
        /// <param name="tiles">tile array</param>
        /// <param name="pointOfView">left top coordinate of visible area</param>
        /// <param name="highRes">Determines downscale resampling algorithm</param>
        /// <returns></returns>
        public Image Draw(Tile[] tiles, Point pointOfView, bool highRes = false)
        {
            _canvas = _tDrawer.DrawImage(_screenSize.Width, _screenSize.Height, tiles, pointOfView, _screenSize, highRes);
            return _canvas;
        }

        /// <summary>
        /// Draws artifacts (tools, points etc) on top of the current image
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointOfView"></param>
        public void Draw(Graphics g, Point pointOfView)
        {
            _iDrawer.DrawItems(g, pointOfView, _screenSize);
        }

        /// <summary>
        /// Redraws last fetched tiles
        /// </summary>
        /// <returns></returns>
        public Image RedrawImage()
        {
            return _tDrawer.RedrawImage(_screenSize);
        }


        public Image DrawHorizontalSection(Point current, int size)
        {
            if (_canvas != null)
            {
                return _iDrawer.DrawSection(_canvas, new Point(current.X - size / 2, current.Y),
                                           new Point(current.X + size / 2, current.Y)); 
            }
            return null;
        }

        public Image DrawVerticalSection(Point current, int size)
        {
            if (_canvas != null)
            {
                return _iDrawer.DrawSection(_canvas, new Point(current.X, current.Y - size / 2),
                                new Point(current.X, current.Y + size / 2));
            }
            return null;
        }

        public Image DrawRuler(Point from, Point to)
        {
            if (_canvas != null)
            {
                return _iDrawer.DrawSection(_canvas, from, to);
            }
            return null;
        }

        public Image DrawLinearSection(Point from, Point to)
        {
            if (_canvas != null)
            {
                return _iDrawer.DrawSection(_canvas, from, to);
            }
            return null;
        }

        public Image DrawSquareArea(Point leftTop, int borderSize)
        {
            if (_canvas != null)
            {
                return _iDrawer.DrawSquareArea(_canvas, leftTop, borderSize);
            }
            return null;
        }

        public Image DrawSharedPoint(Point shared, Point leftTopPointOfView, Size screenSize)
        {
            if (_canvas != null)
            {
                Image img = (Image)_canvas.Clone();
                return _iDrawer.DrawSharedPoint(img, shared, leftTopPointOfView, screenSize);
            }
            return null;
        }

        public Image DrawSharedPoint(Image canvas, Point shared, Point leftTopPointOfView, Size screenSize)
        {
            if (canvas != null)
            {
                Image img = (Image)canvas.Clone();
                return _iDrawer.DrawSharedPoint(img, shared, leftTopPointOfView, screenSize);
            }
            return null;
        }


    }
}
