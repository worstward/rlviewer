using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace RlViewer.Behaviors.Ruler
{
    class Ruler
    {
        public Ruler(float dx = 1, float dy = 1)
        {
            _dx = dx;
            _dy = dy;
        }

        private float _dx;
        private float _dy;

        public double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) * _dx * _dx + (p1.Y - p2.Y) * (p1.Y - p2.Y) * _dy * _dy);
        }

    }
}
