using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    class BaNavigationContainer : NavigationContainer
    {
        public BaNavigationContainer(string path)
            : base(0, 0, 0, 0)
        {
        }

        public override void GetNavigation()
        {
            
        }

        public override NavigationString[] ConvertToCommonNavigation(Headers.Abstract.IStrHeader[] strCollection)
        {
            throw new NotImplementedException();
        }

        public override NavigationString this[int stringNumber]
        {
            get 
            {
                throw new NotSupportedException("Ba navigation");
            }
        }

        public override NavigationItem[] this[int stringNumber, int sampleNumber = 0]
        {
            get
            {
                throw new NotImplementedException();
            }
        }  


    }
}
