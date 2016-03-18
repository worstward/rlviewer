using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Draw
{
    public class ItemDrawer : ImageDrawer
    {
        public ItemDrawer(PointSelector.PointSelector pointSelector, AreaSelector.AreaSelector areaSelector, RlViewer.Behaviors.Scaling.Scaler scaler)
        {
            _pointSelector = pointSelector;
            _areaSelector = areaSelector;
            _scaler = scaler;
        }

        private PointSelector.PointSelector _pointSelector;
        private AreaSelector.AreaSelector _areaSelector;
        private RlViewer.Behaviors.Scaling.Scaler _scaler;


        public Image DrawItems(Image canvas, Point leftTopPointOfView, Size screenSize)
        {
            GC.Collect();

            var screen = new RectangleF(leftTopPointOfView, screenSize);
            var img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {

                DrawPoints(g, screen);
                DrawArea(g, screen);
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

        private void DrawArea(Graphics g, RectangleF screen)
        {
            using (var pen = new Pen(Palette.Entries[240]) { DashPattern = new float[] { 5, 2, 15, 4 } })
            {
                g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X), (int)(_areaSelector.Area.Location.Y - screen.Y),
                    _areaSelector.Area.Width, _areaSelector.Area.Height);
            }
        }

       
    }
}
