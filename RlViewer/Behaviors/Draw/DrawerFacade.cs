﻿using System;
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
            _canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);
        }

        private ItemDrawer _iDrawer;
        private TileDrawer _tDrawer;
        private Size _screenSize;
        private Image _canvas;



        public void GetPalette(int R, int G, int B, bool reversed)
        {
            _tDrawer.GetPalette(R, G, B, reversed);
        }

        public Image Draw(Tile[] tiles, Point pointOfView)
        {
            return _iDrawer.DrawItems(_tDrawer.DrawImage(_canvas, tiles, pointOfView, _screenSize), pointOfView, _screenSize);
        }

        public Image Draw(Point pointOfView)
        {
            return _iDrawer.DrawItems(_canvas, pointOfView, _screenSize);
        }


        public Image DrawHorizontalSection(Point current, int size)
        {
            return _iDrawer.DrawHorizontalSection(_canvas, current, size); 
        }

        public Image DrawVerticalSection(Point current, int size)
        {
            return _iDrawer.DrawVerticalSection(_canvas, current, size);
        }


       
    }
}
