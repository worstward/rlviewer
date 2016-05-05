﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    public class RNavigationContainer : NavigationContainer
    {
        public RNavigationContainer(string path, float initialRange, float step, byte board, int headerLength, int dataLength)
        {
            _path = path;
            _initialRange = initialRange;
            _step = step;
            _board = board;
            _headerLength = headerLength;
            _dataLength = dataLength;
        }

        private string _path;
        private float _initialRange;
        private float _step;
        private byte _board;
        private int _headerLength;
        private int _dataLength;



        private RlViewer.Behaviors.Navigation.NavigationComputing _computer;
        public override RlViewer.Behaviors.Navigation.NavigationComputing Computer
        {
            get { return _computer; }
        }

        private NavigationString[] ConvertToCommonNavigation(RlViewer.Headers.Concrete.R.RStrHeaderStruct[] strCollection)
        {
            IEnumerable<NavigationString> naviStrings;
            try
            {
                naviStrings = strCollection.Select
                    (x => new NavigationString((float)x.navigationHeader.longtitudeInsSns,
                        (float)x.navigationHeader.latitudeInsSns, (float)x.navigationHeader.heightInsSns,
                        (float)x.navigationHeader.realTraceInsSns, _board));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return naviStrings.ToArray();
        }

        public override void GetNavigation()
        {
            NaviStrings =
                ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.R.RStrHeaderStruct>(
                _path, _headerLength, _dataLength));
            _computer = new Behaviors.Navigation.NavigationComputing(_initialRange, _step);
        }

        public override NavigationString this[int stringNumber]
        {
            get
            {
                try
                {
                    return NaviStrings[stringNumber];
                }
                catch (IndexOutOfRangeException)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Warning, "Wrong navigation data");
                    NaviStrings = null;
                    return new NavigationString(1, 1, 1, 1, 1);
                }
            }
        }

        public override Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0]
        {
            get
            {
                return NaviStrings[stringNumber].NaviInfo(sampleNumber, _computer);    //.NaviInfo();          
            }
        }  

    }
}
