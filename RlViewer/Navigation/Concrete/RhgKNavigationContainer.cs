﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    class RhgKNavigationContainer : NavigationContainer
    {
        public RhgKNavigationContainer(string path, byte board)
        {
            //_naviStrings =
              //  ConvertToCommonNavigation(GetNaviStrings<RlViewer.Headers.Concrete.r>(path, 16384, 1));
        }

        private NavigationString[] _naviStrings;

        private NavigationString[] ConvertToCommonNavigation(RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct[] strCollection)
        {
            var naviStrings = strCollection.Select
                (x => new NavigationString((float)x.longtitude, (float)x.latitude, (float)x.H, 1, 1));

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
