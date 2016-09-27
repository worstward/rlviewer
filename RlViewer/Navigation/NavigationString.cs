using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation
{
    public class NavigationString
    {
        public NavigationString(double longitude, double latitude, double height, double track,
            byte board, DateTime timeArm)//, DateTime timeUtc)
        {
            AircraftLongitude = longitude;
            AircraftLatitude = latitude;
            AircraftHeight = height;
            Track = track;
            Board = board;
            TimeArm = timeArm;
            //TimeUtc = timeUtc;
        }

        public DateTime TimeArm
        {
            get;
            private set;
        }


        public double AircraftLongitude
        {
            get;
            private set;
        }

        public double AircraftLatitude
        {
            get;
            private set;
        }

        public double AircraftHeight
        {
            get;
            private set;
        }

        public double Track
        {
            get;
            private set;
        }

        public byte Board
        {
            get;
            private set;
        }

    }
}
