﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Draw
{

    /// <summary>
    /// Incapsulates drawing of tools
    /// </summary>
    public class ItemDrawer : ImageDrawer
    {
        public ItemDrawer(PointSelector.PointSelector pointSelector,
            AreaSelector.AreaSelector areaSelector, RlViewer.Behaviors.Scaling.Scaler scaler) : base(scaler)
        {
            _pointSelector = pointSelector;
            _areaSelector = areaSelector;
        }

        private PointSelector.PointSelector _pointSelector;
        private AreaSelector.AreaSelector _areaSelector;

        private object _locker = new object();


        public void DrawItems(Graphics g, Point leftTopPointOfView, Size screenSize)
        {
            var screen = new RectangleF(leftTopPointOfView.X, leftTopPointOfView.Y,
                screenSize.Width / Scaler.ScaleFactor, screenSize.Height / Scaler.ScaleFactor);


            DrawPoints(g, screen);
            DrawArea(g, screen);
        }


        private Image DrawMarker(Image canvas, Point point)
        {
            Image img;
            lock (_locker)
            {
                img = (Image)canvas.Clone();
                using (var g = Graphics.FromImage(img))
                {
                    using (var pen = new Pen(Palette.Entries[240]))
                    {
                        g.FillRectangle(Brushes.Red, point.X, point.Y, 5, 5);
                    
                    }
                }
            }
            return img;
        }


        private void DrawPoints(Graphics g, RectangleF screen)
        {
            foreach (var point in _pointSelector)
            {
                if (screen.Contains(point.Location))
                {
                    g.FillRectangle(Brushes.Red, (int)((point.Location.X - screen.X) * Scaler.ScaleFactor),
                        (int)((point.Location.Y - screen.Y) * Scaler.ScaleFactor),
                        5 < Scaler.ScaleFactor ? Scaler.ScaleFactor : 5, 
                        5 < Scaler.ScaleFactor ? Scaler.ScaleFactor : 5);                    
                }
            }
        }

        private void DrawArea(Graphics g, RectangleF screen)
        {
            using (var pen = new Pen(Palette.Entries[240]) { DashPattern = new float[] { 5, 2, 15, 4 } })
            {
                g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X) * Scaler.ScaleFactor,
                    (int)(_areaSelector.Area.Location.Y - screen.Y) * Scaler.ScaleFactor,
                    _areaSelector.Area.Width * Scaler.ScaleFactor, _areaSelector.Area.Height * Scaler.ScaleFactor);
            }
        }

        public Image DrawSection(Image canvas, Point pt1, Point pt2)
        {
            Image img;
            lock (_locker)
            {
                img = (Image)canvas.Clone();
                using (var g = Graphics.FromImage(img))
                {
                   using (var pen = new Pen(Palette.Entries[240]))
                    {
                        g.DrawLine(pen, pt1, pt2);
                    }           
                }
            }
            return img;
        }


    }
}
