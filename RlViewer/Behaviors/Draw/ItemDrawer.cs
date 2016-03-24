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
        public ItemDrawer(PointSelector.PointSelector pointSelector, AreaSelector.AreaSelector areaSelector, RlViewer.Behaviors.Scaling.Scaler scaler) : base(scaler)
        {
            _pointSelector = pointSelector;
            _areaSelector = areaSelector;
        }

        private PointSelector.PointSelector _pointSelector;
        private AreaSelector.AreaSelector _areaSelector;

        private object _locker = new object();


        public Image DrawItems(Image canvas, Point leftTopPointOfView, Size screenSize)
        {
            GC.Collect();
            Image img;
            var screen = new RectangleF(leftTopPointOfView, screenSize);
            lock (_locker)
            {
                img = (Image)canvas.Clone();
                using (var g = Graphics.FromImage(img))
                {
                    DrawPoints(g, screen);
                    DrawArea(g, screen);             
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
                    g.FillRectangle(Brushes.Red, (int)((point.Location.X - screen.X) * Scaler.ZoomFactor),
                        (int)((point.Location.Y - screen.Y) * Scaler.ZoomFactor), (int)Scaler.ZoomFactor * 3, (int)Scaler.ZoomFactor * 3);
                    
                }
            }
        }

        private void DrawArea(Graphics g, RectangleF screen)
        {
            using (var pen = new Pen(Palette.Entries[240]) { DashPattern = new float[] { 5, 2, 15, 4 } })
            {
                g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X) * Scaler.ZoomFactor,
                    (int)(_areaSelector.Area.Location.Y - screen.Y) * Scaler.ZoomFactor,
                    _areaSelector.Area.Width * Scaler.ZoomFactor, _areaSelector.Area.Height * Scaler.ZoomFactor);
            }
        }

       
    }
}
