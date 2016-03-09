﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Draw
{
    class ItemDrawer
    {
        public ItemDrawer(PointSelector.PointSelector pointSelector, AreaSelector.AreaSelector areaSelector)
        {
            _pointSelector = pointSelector;
            _areaSelector = areaSelector;
        }

        private PointSelector.PointSelector _pointSelector;
        private AreaSelector.AreaSelector _areaSelector;

        public Image DrawItems(Image canvas, Point leftTopPointOfView, Size screenSize, Color selectionColor)
        {
            GC.Collect();

            var screen = new RectangleF(leftTopPointOfView, screenSize);
            var img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {
                DrawPoints(g, screen);
                DrawArea(g, screen, selectionColor);
            }
            return img;
        }


        private void DrawPoints(Graphics g, RectangleF screen)
        {
            foreach (var point in _pointSelector)
            {
                if (screen.Contains(point.Location))
                {
                    using (var pen = new Pen(Color.Red, 3f))
                    {
                        g.DrawRectangle(pen, (int)(point.Location.X - screen.Location.X),
                            (int)(point.Location.Y - screen.Location.Y), 1, 1);
                    }
                }
            }
        }

        private void DrawArea(Graphics g, RectangleF screen, Color selectionColor)
        {
            using (var pen = new Pen(selectionColor) { DashPattern = new float[] { 5, 2, 15, 4 } })
            {
                g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X), (int)(_areaSelector.Area.Location.Y - screen.Y),
                    _areaSelector.Area.Width, _areaSelector.Area.Height);
            }
        }

       
    }
}