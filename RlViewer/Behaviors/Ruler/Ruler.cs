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
        /// <summary>
        /// Gets distance between 2 points in pixels
        /// </summary>
        /// <param name="p1">Point to start measuring distance from</param>
        /// <param name="p2">Point to finish measuring distance at</param>
        /// <returns>Distance in pixels</returns>
        public double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        /// <summary>
        /// Gets distance between 2 points in meters
        /// </summary>
        /// <param name="p1">Point to start measuring distance from</param>
        /// <param name="p2">Point to finish measuring distance at</param>
        /// <param name="dx">Range decomposition step (in meters)</param>
        /// <param name="dy">Azimuth decomposition step (in meters)</param>
        /// <returns>Distance in meters</returns>
        public double GetDistance(Point p1, Point p2, float dx, float dy)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) * dx * dx + (p1.Y - p2.Y) * (p1.Y - p2.Y) * dy * dy);
        }

    }
}
