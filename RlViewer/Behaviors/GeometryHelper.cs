using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors
{
    public static class GeometryHelper
    {

        /// <summary>
        /// Gets intersection point of 2 lines
        /// </summary>
        /// <param name="p1">First point of the 1st line</param>
        /// <param name="p2">Second point of the 1st line</param>
        /// <param name="p3">First point of the 2nd line</param>
        /// <param name="p4">Second point of the 2nd line</param>
        /// <returns>Intersection point</returns>
        public static Point Intersection(Point p1, Point p2, Point p3, Point p4)
        {
            var u1 = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) /
                ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));

            var u2 = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) /
               ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));

            return new Point(p1.X + u1 * (p2.X - p1.X), p1.Y + u2 * (p2.Y - p1.Y));
        }



        /// <summary>
        /// Tells if given point is to the left or to the right of the line
        /// </summary>
        /// <param name="p1">First point of the line</param>
        /// <param name="p2">Second point of the line</param>
        /// <param name="input">Point which relative position we want to know</param>
        /// <returns>True if point is to the right of the line, false if it is to the left</returns>
        public static bool MutualPosition(Point p1, Point p2, Point input)
        {
            var valueToAnalyze = (input.X - p1.X) * (p2.Y - p1.Y) - (input.Y - p1.Y) * (p2.X - p1.X);
            //valueToAnalyze > 0 : right
            //valueToAnalyze < 0 : left
            //valueToAnalyze == 0 : point is on the line

            //we don't care if the point lies on a line so this case is just included to 'right'
            return valueToAnalyze >= 0;
        }


        /// <summary>
        /// Gets Y coordinate of a point on the line
        /// </summary>
        /// <param name="point1">First point of the line</param>
        /// <param name="point2">Second point of the line</param>
        /// <param name="x">X coordinate of a needed point</param>
        /// <returns>Y coordinate</returns>
        public static int GetY(Point point1, Point point2, float x)
        {
            //y = kx+b
            float k = (point2.Y - point1.Y) / (float)(point2.X - point1.X);
            float b = point1.Y - (k * point1.X);

            return (int)(k * x + b);
        }

    }
}
