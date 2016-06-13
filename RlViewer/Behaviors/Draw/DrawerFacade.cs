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



        public void GetPalette(float R, float G, float B, bool reversed, bool logarithmic)
        {
            _tDrawer.GetPalette(R, G, B, reversed, logarithmic);
        }

        public Image Draw(Tile[] tiles, Point pointOfView, bool highRes = false)
        {
            _canvas = _tDrawer.DrawImage(_screenSize.Width, _screenSize.Height, tiles, pointOfView, _screenSize, highRes);
            return _iDrawer.DrawItems(_canvas, pointOfView, _screenSize);
        }

        public Image Draw(Point pointOfView)
        {
            return _iDrawer.DrawItems(_canvas, pointOfView, _screenSize);
        }


        public Image DrawHorizontalSection(Point current, int size)
        {
            return _iDrawer.DrawSection(_canvas, new Point(current.X - size / 2, current.Y),
                            new Point(current.X + size / 2, current.Y)); 
        }

        public Image DrawVerticalSection(Point current, int size)
        {
            return _iDrawer.DrawSection(_canvas, new Point(current.X, current.Y - size / 2),
                            new Point(current.X, current.Y + size / 2));
        }

        public Image DrawRuler(Point from, Point to)
        {
            return _iDrawer.DrawSection(_canvas, from, to);
        }

        public Image DrawLinearSection(Point from, Point to)
        {
            return _iDrawer.DrawSection(_canvas, from, to);
        }


       
    }
}
