using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation
{
    public static class NaviStringExt
    {
        public static Tuple<string, string>[] NaviInfo(this NavigationString s)
        {
            return new Tuple<string, string>[] 
            {
                new Tuple<string, string>("Широта", ParseLatitude(s.AircraftLatitude)),
                new Tuple<string, string>("Долгота", ParseLongitude(s.AircraftLongitude))
            };
        }


        private enum Direction
        {
            N, E, S, W
        }
        private static string ParseLatitude(double value)
        {
            var direction = value < 0 ? Direction.S : Direction.N;
            return ParseLatitudeOrLongitude(value, direction);
        }

        private static string ParseLongitude(double value)
        {
            var direction = value < 0 ? Direction.W : Direction.E;
            return ParseLatitudeOrLongitude(value, direction);
        }


        private static string ParseLatitudeOrLongitude(double value, Direction direction)
        {
            //radians to degrees
            value = Math.Abs(value * ( 180 / Math.PI ));

            var degrees = (int)value;

            value = (value - degrees) * 60;

            var minutes = (int)(value);
            var seconds = (value - minutes) * 60;

            return string.Format("{0:000}º {1:00}' {2:00}'' {3}", degrees, minutes, seconds, direction);
        }



    }
}
