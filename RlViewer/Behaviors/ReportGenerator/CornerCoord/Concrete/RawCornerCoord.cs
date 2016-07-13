using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.CornerCoord.Concrete
{
    class RawCornerCoord : CornerCoord.Abstract.CornerCoordinates
    {
        public RawCornerCoord(Files.LocatorFile file)
            : base(file)
        { }


        protected override RlViewer.Navigation.NavigationContainer GetContainer()
        {
            throw new NotImplementedException();
        }
    }
}
