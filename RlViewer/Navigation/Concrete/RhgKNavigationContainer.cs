using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    class RhgKNavigationContainer : NavigationContainer
    {
        public RhgKNavigationContainer(string path)
        {
            //_naviStrings =
              //  ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.r>(path, 16384, 1));
        }

        private NavigationString[] naviStrings;

        private NavigationString[] ConvertToCommonNavigation(RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct[] strCollection)
        {
            var naviStrings = strCollection.Select
                (x => new NavigationString((float)x.longtitude, (float)x.latitude, (float)x.H, 1, 1));

            return naviStrings.ToArray();
        }

        protected override Behaviors.Navigation.NavigationComputing Computer
        {
            get { throw new NotImplementedException(); }
        }

        public override void GetNavigation()
        {
            
        }

        public override NavigationString this[int stringNumber]
        {
            get
            {
                return naviStrings[stringNumber];
            }
        }

        public override Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0]
        {
            get
            {
                throw new NotImplementedException();   
            }
        }  

    }
}
