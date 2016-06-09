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
        /// <returns>True if point is to the right of the line, false if it is to the left or on the line</returns>
        public static bool MutualPosition(Point p1, Point p2, Point input)
        {
            var valueToAnalyze = (input.X - p1.X) * (p2.Y - p1.Y) - (input.Y - p1.Y) * (p2.X - p1.X);
            //valueToAnalyze > 0 : right
            //valueToAnalyze < 0 : left
            //valueToAnalyze == 0 : point is on the line

            return valueToAnalyze > 0;
        }


        /// <summary>
        /// Gets line angle
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Angle in radians</returns>
        public static float GetAngle(PointF p1, PointF p2)
        {
            return (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }



        private static float GetK(Point p1, Point p2) //y = kx+b
        {
            return (p2.Y - p1.Y) / (float)(p2.X - p1.X);
        }

        private static float GetB(Point p1, Point p2) //y = kx+b
        {
            return (p2.X * p1.Y - p2.Y * p1.X) / (float)(p2.X - p1.X);
        }


        /// <summary>
        /// Gets Y coordinate of a point on the line
        /// </summary>
        /// <param name="point1">First point of the line</param>
        /// <param name="point2">Second point of the line</param>
        /// <param name="x">X coordinate of a needed point</param>
        /// <returns>Y coordinate</returns>
        public static int GetY(Point p1, Point p2, int x)
        {
            if (p1.Y == p2.Y)
                return p1.Y;
            return (int)(GetK(p1, p2) * x + GetB(p1, p2));
        }


        /// <summary>
        /// Gets X coordinate of a point on the line
        /// </summary>
        /// <param name="point1">First point of the line</param>
        /// <param name="point2">Second point of the line</param>
        /// <param name="y">Y coordinate of a needed point</param>
        /// <returns>X coordinate</returns>
        public static int GetX(Point p1, Point p2, int y)
        {
            if (p1.X == p2.X)
                return p1.X;
            return (int)((y - GetB(p1, p2)) / GetK(p1, p2));
        }



    }
}
