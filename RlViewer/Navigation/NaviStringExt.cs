using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation
{
    public static class NaviStringConverters
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

        public static Tuple<string, string>[] NaviInfo(this NavigationString s, int sampleNum, RlViewer.Behaviors.Navigation.NavigationComputing computer)
        {
            return new Tuple<string, string>[] 
            {
                new Tuple<string, string>("Широта", ParseLatitude(computer.InterpolateLatitude(sampleNum, s.AircraftLatitude, s.AircraftHeight, s.Track, s.Board))),
                new Tuple<string, string>("Долгота",ParseLongitude(computer.InterpolateLongtitude(sampleNum, s.AircraftLongitude, s.AircraftLatitude, s.AircraftHeight, s.Track, s.Board))),
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

        private static string ParseToDegrees(double value, string suffix = "")
        {
            var val = value;
            //radians to degrees
            value = Math.Abs(value * ( 180 / Math.PI ));

            var degrees = Math.Truncate(value);

            
            value = (value - degrees) * 60;

            var minutes = Math.Truncate(value);
            var seconds = (value - minutes) * 60;

            var formatted =  string.Format("{0:000}° {1:00}' {2:00}'' {3}", degrees, minutes, seconds, suffix);

            return formatted;
        }


        public static double ParseToRadians(string degrees)
        {
            var splitted = degrees.Split(new char[] { '°', '\'' }, StringSplitOptions.RemoveEmptyEntries);

            //degrees + minutes / 60 + seconds / 3600;
            double parsedValue = Convert.ToDouble(splitted[0]) +
                          Convert.ToDouble(splitted[1]) / 60d + 
                          Convert.ToDouble(splitted[2]) / 3600d; 

            //specify sign
            var suffix = splitted.Last();
            parsedValue = (suffix == "S" || suffix == "W") ? -parsedValue : parsedValue;

            //degrees to radians
            parsedValue = parsedValue * Math.PI / 180d;

            return parsedValue;
           
        }

    }
}
