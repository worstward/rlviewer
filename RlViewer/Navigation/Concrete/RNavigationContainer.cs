using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    public class RNavigationContainer : NavigationContainer
    {
        public RNavigationContainer(string path, float initialRange, float step, byte board, int headerLength, int dataLength)
            : base(initialRange, step)
        {
            _path = path;
            _board = board;
            _headerLength = headerLength;
            _dataLength = dataLength;
        }

        private string _path;
        private byte _board;
        private int _headerLength;
        private int _dataLength;


        public override NavigationString[] ConvertToCommonNavigation(Headers.Abstract.IStrHeader[] strCollection)
        {
            RlViewer.Headers.Concrete.R.RStrHeaderStruct[] rStrColl = strCollection.Select(x => (RlViewer.Headers.Concrete.R.RStrHeaderStruct)x).ToArray();

            IEnumerable<NavigationString> naviStrings;
            try
            {
                naviStrings = rStrColl.Select
                    (x => new NavigationString((float)(x.navigationHeader.longtitudeInsSns / 180f * Math.PI),
                        (float)(x.navigationHeader.latitudeInsSns / 180f * Math.PI), (float)x.navigationHeader.heightInsSns,
                        (float)(x.navigationHeader.realTraceInsSns / 180f * Math.PI), _board));
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
                    ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.R.RStrHeaderStruct>(
                    _path, _headerLength, _dataLength).Cast<Headers.Abstract.IStrHeader>().ToArray());
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
                    return new NavigationString(1, 1, 1, 1, 1);
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
