using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    class KNavigationContainer : NavigationContainer
    {
        public KNavigationContainer(string path, float initialRange, float rangeStep, byte board, int fileHeaderLength, int strDataLength)
        {
            _path = path;
            _initialRange = initialRange;
            _rangeStep = rangeStep;
            _board = board;
            _fileHeaderLength = fileHeaderLength;
            _strDataLength = strDataLength;
        }

        private string _path;
        private float _initialRange;
        private float _rangeStep;
        private byte _board;
        private int _fileHeaderLength;
        private int _strDataLength;



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
                    _path, _fileHeaderLength, _strDataLength));
                _computer = new Behaviors.Navigation.NavigationComputing(_initialRange, _rangeStep);
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
                return NaviStrings[stringNumber].NaviInfo(sampleNumber, _computer);    //.NaviInfo();          
            }
        }  

    }
}
