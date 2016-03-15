using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RlViewer.Navigation.Concrete
{
    class Brl4NavigationContainer : NavigationContainer
    {
        public Brl4NavigationContainer(string path, float initialRange, float step, byte board, int headerLength, int dataLength)
        {
            _naviStrings =
                ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct>(
                path, headerLength, dataLength), board);
        }

        private NavigationString[] _naviStrings;

        private NavigationString[] ConvertToCommonNavigation(
            RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct[] strCollection, byte board)
        {
            var naviStrings = strCollection.Select
                (x => new NavigationString((float)x.longtitude, (float)x.latitude, (float)x.H, (float)x.a, board));

            return naviStrings.ToArray();
        }

        public override NavigationString this[int stringNumber]
        {
            get
            {
                return _naviStrings[stringNumber];
            }
        }
    }
}
