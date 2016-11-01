using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Navigation.Concrete
{
    class KNavigationContainer : NavigationContainer
    {
        public KNavigationContainer(string path, float initialRange, byte flipType, int imageWidth, float rangeStep, byte board, int fileHeaderLength, int strDataLength)
            : base(initialRange, rangeStep, flipType, imageWidth)
        {
            _path = path;
            _board = board;
            _fileHeaderLength = fileHeaderLength;
            _strDataLength = strDataLength;
        }

        private string _path;
        private byte _board;
        private int _fileHeaderLength;
        private int _strDataLength;

        public override NavigationString[] ConvertToCommonNavigation(RlViewer.Headers.Abstract.IStrHeader[] strCollection)
        {
            RlViewer.Headers.Concrete.K.KStrHeaderStruct[] rStrColl = strCollection.Select(x => (RlViewer.Headers.Concrete.K.KStrHeaderStruct)x).ToArray();
            IEnumerable<NavigationString> naviStrings;

            try
            {  
                naviStrings = rStrColl.Select
                    (x => new NavigationString((float)(x.navigationHeader.longtitudeInsSns / 180f * Math.PI),
                        (float)(x.navigationHeader.latitudeInsSns / 180f * Math.PI), (float)x.navigationHeader.heightInsSns,
                        (float)(x.navigationHeader.realTraceInsSns / 180f * Math.PI), _board, new DateTime().AddYears(1970).AddMilliseconds(x.navigationHeader.timeArm)));
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
                    ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.K.KStrHeaderStruct>(
                    _path, _fileHeaderLength, _strDataLength).Cast<Headers.Abstract.IStrHeader>().ToArray());
            }
            catch (ArgumentNullException)
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
                    return new NavigationString(1, 1, 1, 1, 1, default(DateTime));
                }
            }
        }

        public override Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0]
        {
            get
            {
                return NaviStrings[stringNumber].NaviInfo(sampleNumber, Computer);    //.NaviInfo();          
            }
        }  

    }
}
