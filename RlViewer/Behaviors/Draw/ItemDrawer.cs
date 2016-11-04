using System;
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
            AreaSelector.AreaSelector areaSelector, RlViewer.Behaviors.Scaling.Scaler scaler,
            AreaSelector.AreaSelectorsAlignerContainer areaAlignerWrapper)
            : base(scaler)
        {
            _pointSelector = pointSelector;
            _areaSelector = areaSelector;
            _areaAlignerWrapper = areaAlignerWrapper;
        }

        private PointSelector.PointSelector _pointSelector;
        private AreaSelector.AreaSelector _areaSelector;
        private AreaSelector.AreaSelectorsAlignerContainer _areaAlignerWrapper;


        public void DrawItems(Graphics g, Point leftTopPointOfView, Size screenSize)
        {
            var screen = new RectangleF(leftTopPointOfView.X, leftTopPointOfView.Y,
                screenSize.Width / Scaler.ScaleFactor, screenSize.Height / Scaler.ScaleFactor);

            DrawAlignerAreas(g, screen);
            DrawPoints(g, screen, _pointSelector.Select(x => x.Location));
            DrawSelectorArea(g, screen);
        }

        public Image DrawSection(Image canvas, Point pt1, Point pt2)
        {
            Image img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {
                using (var pen = new Pen(Palette.Entries[240]))
                {
                    g.DrawLine(pen, pt1, pt2);
                }
            }

            return img;
        }
        public Image DrawSharedPoint(Image canvas, Point shared, Point leftTopPointOfView, Size screenSize)
        {
            var screen = new RectangleF(leftTopPointOfView.X, leftTopPointOfView.Y,
                screenSize.Width / Scaler.ScaleFactor, screenSize.Height / Scaler.ScaleFactor);

            using (var g = Graphics.FromImage(canvas))
            {
                DrawPoint(g, screen, shared, Brushes.Orange);
            }

            return canvas;
        }

        public Image DrawSquareArea(Image canvas, Point leftTop, int borderSize)
        {
            Image img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {
                using (var pen = new Pen(Palette.Entries[240]))
                {
                    g.DrawRectangle(pen, new Rectangle(leftTop,
                        new Size((int)(borderSize * Scaler.ScaleFactor), (int)(borderSize * Scaler.ScaleFactor))));
                }
            }

            return img;
        }


        public Image DrawAlignerAreas(Image canvas, AreaSelector.AreaSelectorDecorator areaSelector, Point pointOfView, Size screenSize)
        {
            Image img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {

                var screen = new RectangleF(pointOfView.X, pointOfView.Y, screenSize.Width / Scaler.ScaleFactor, screenSize.Height / Scaler.ScaleFactor);



                var areaRect = new RectangleF(areaSelector.Area.Location.X, areaSelector.Area.Location.Y, areaSelector.Area.Width, areaSelector.Area.Height);

                if (areaRect.IntersectsWith(screen))
                {
                    using (Pen p = new Pen(Color.Red))
                    {
                        g.DrawRectangle(p, (int)(areaRect.X - screen.X) * Scaler.ScaleFactor,
                           (int)(areaRect.Y - screen.Y) * Scaler.ScaleFactor,
                           areaRect.Width * Scaler.ScaleFactor, areaRect.Height * Scaler.ScaleFactor);
                    }
                }

                DrawPoints(g, screen, _areaAlignerWrapper.Where(x => x.SelectedPoint != null).Select(x => x.SelectedPoint.Location));
            }
            return img;
        }



        private void DrawAlignerAreas(Graphics g, RectangleF screen)
        {
            foreach (var area in _areaAlignerWrapper)
            {
                var areaRect = new RectangleF(area.Area.Location.X, area.Area.Location.Y, area.Area.Width, area.Area.Height);

                if (areaRect.IntersectsWith(screen))
                {
                    using (Pen p = new Pen(Color.Red))
                    {
                        g.DrawRectangle(p, (int)(areaRect.X - screen.X) * Scaler.ScaleFactor,
                           (int)(areaRect.Y - screen.Y) * Scaler.ScaleFactor,
                           areaRect.Width * Scaler.ScaleFactor, areaRect.Height * Scaler.ScaleFactor);
                    }
                }
            }
            DrawPoints(g, screen, _areaAlignerWrapper.Where(x => x.SelectedPoint != null).Select(x => x.SelectedPoint.Location));
        }

        private void DrawPoints(Graphics g, RectangleF screen, IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                DrawPoint(g, screen, point, Brushes.Red);
            }

        }

        private void DrawPoint(Graphics g, RectangleF screen, Point point, Brush pointBrush)
        {
            if (screen.Contains(point))
            {
                g.FillRectangle(pointBrush, (int)((point.X - screen.X) * Scaler.ScaleFactor),
                    (int)((point.Y - screen.Y) * Scaler.ScaleFactor),
                    5 < Scaler.ScaleFactor ? Scaler.ScaleFactor : 5,
                    5 < Scaler.ScaleFactor ? Scaler.ScaleFactor : 5);
            }
        }

        public Image DrawSelectorArea(Image canvas, Point leftTopPointOfView, Size screenSize)
        {
            var screen = new RectangleF(leftTopPointOfView.X, leftTopPointOfView.Y,
                screenSize.Width / Scaler.ScaleFactor, screenSize.Height / Scaler.ScaleFactor);

            Image img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {
                using (var pen = new Pen(Palette.Entries[240]) { DashPattern = new float[] { 5, 2, 15, 4 } })
                {
                    g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X) * Scaler.ScaleFactor,
                        (int)(_areaSelector.Area.Location.Y - screen.Y) * Scaler.ScaleFactor,
                        _areaSelector.Area.Width * Scaler.ScaleFactor, _areaSelector.Area.Height * Scaler.ScaleFactor);
                }
            }
            return img;
        }




        private void DrawSelectorArea(Graphics g, RectangleF screen)
        {
            using (var pen = new Pen(Palette.Entries[240]) { DashPattern = new float[] { 5, 2, 15, 4 } })
            {
                g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X) * Scaler.ScaleFactor,
                    (int)(_areaSelector.Area.Location.Y - screen.Y) * Scaler.ScaleFactor,
                    _areaSelector.Area.Width * Scaler.ScaleFactor, _areaSelector.Area.Height * Scaler.ScaleFactor);
            }
        }


    }
}
