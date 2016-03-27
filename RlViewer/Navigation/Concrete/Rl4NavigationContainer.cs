﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    class Rl4NavigationContainer : NavigationContainer
    {
        public Rl4NavigationContainer(string path, float initialRange, float step, byte board, int headerLength, int dataLength)
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
        protected override RlViewer.Behaviors.Navigation.NavigationComputing Computer
        {
            get { return _computer; }
        }


        private NavigationString[] _naviStrings;

        private NavigationString[] ConvertToCommonNavigation(RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct[] strCollection, byte board)
        {
            IEnumerable<NavigationString> naviStrings;
            try
            {
                naviStrings = strCollection.Select
                    (x => new NavigationString((float)x.longtitude, (float)x.latitude, (float)x.H, (float)x.a, board));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return naviStrings.ToArray();
        }

        public override void GetNavigation()
        {
            _naviStrings =
                ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct>(
                _path, _headerLength, _dataLength), _board);
            _computer = new Behaviors.Navigation.NavigationComputing(_initialRange, _step);
        }

        public override NavigationString this[int stringNumber]
        {
            get
            {
                try
                {
                    return _naviStrings[stringNumber];
                }
                catch (IndexOutOfRangeException)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Error, "Wrong navigation data");
                    _naviStrings = null;
                    return new NavigationString(1, 1, 1, 1, 1);
                }
            }
        }

        public override Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0]
        {
            get
            {
                return _naviStrings[stringNumber].NaviInfo(sampleNumber, _computer);    //.NaviInfo();          
            }
        }  

    }
}
