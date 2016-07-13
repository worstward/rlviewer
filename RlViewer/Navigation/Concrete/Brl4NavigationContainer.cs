﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RlViewer.Navigation.Concrete
{
    class Brl4NavigationContainer : NavigationContainer
    {

        public Brl4NavigationContainer(string path, float initialRange, float step, byte board,
            int headerLength, int dataLength, int sx, int sy) : base(initialRange, step)
        {
            _path = path;
            _board = board;
            _headerLength = headerLength;
            _dataLength = dataLength;
            _sx = sx;
        }

        private string _path;

        private byte _board;
        private int _headerLength;
        private int _dataLength;
        private int _sx;


        public override NavigationString[] ConvertToCommonNavigation(Headers.Abstract.IStrHeader[] strCollection)
        {
            RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct[] rStrColl = strCollection.Select(x => (RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct)x).ToArray();
            IEnumerable<NavigationString> naviStrings;

            try
            {
                naviStrings = rStrColl.Select
                    (x => new NavigationString((float)x.longtitude, (float)x.latitude, (float)x.H, (float)x.a, _board));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return naviStrings.ToArray();
        }

        public override void GetNavigation()
        {
            try
            {
                NaviStrings =
                    ConvertToCommonNavigation(
                    GetNaviStrings<RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct>(_path, _headerLength, _dataLength)
                    .Cast<Headers.Abstract.IStrHeader>().ToArray());
               
            }
            catch (ArgumentException)
            {
                return;
            }
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
                return NaviStrings[stringNumber].NaviInfo((sampleNumber + _sx), Computer);    //.NaviInfo();          
            }
        }       



    }
}
