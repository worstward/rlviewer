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
                new Tuple<string, string>("Широта",  ParseLatitude(s.AircraftLatitude)),
                new Tuple<string, string>("Долгота", ParseLongitude(s.AircraftLongitude)),
                new Tuple<string, string>("Курс",    ParseToDegrees(s.Track))
            };
        }

        public static Tuple<string, string>[] NaviInfo(this NavigationString s, int sampleNum, RlViewer.Behaviors.Navigation.NavigationInterpolator interpolator)
        {
            return new Tuple<string, string>[] 
            {
                new Tuple<string, string>("Широта", ParseLatitude(interpolator.InterpolateLatitude(sampleNum, s.AircraftLatitude, s.AircraftHeight, s.Track, s.Board))),
                new Tuple<string, string>("Долгота",ParseLongitude(interpolator.InterpolateLongtitude(sampleNum, s.AircraftLongitude, s.AircraftLatitude, s.AircraftHeight, s.Track, s.Board))),
                new Tuple<string, string>("Курс", ParseToDegrees(s.Track))
            };
        }


        public static string ParseLatitude(double value)
        {
            var direction = value < 0 ? "S" : "N";
            return ParseToDegrees(value, direction);
        }

        public static string ParseLongitude(double value)
        {
            var direction = value < 0 ? "W" : "E";
            return ParseToDegrees(value, direction);
        }

        public static string ParseToDegrees(double value, string suffix = "")
        {
            //radians to degrees
            value = Math.Abs(value * ( 180 / Math.PI ));

            var degrees = (int)value;

            value = (value - degrees) * 60;

            var minutes = (int)(value);
            var seconds = (value - minutes) * 60;

            return string.Format("{0:000}° {1:00}' {2:00}'' {3}", degrees, minutes, seconds, suffix);
        }



    }
}
