using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.CornerCoord.Concrete
{
    class RawCornerCoord : CornerCoord.Abstract.CornerCoordinates
    {
        public RawCornerCoord(Files.LocatorFile file, int firstLine, int lastLine, bool readToEnd)
            : base(file, firstLine, lastLine, readToEnd)
        { }


        protected override RlViewer.Navigation.NavigationContainer GetContainer(int firstLine, int lastLine, bool readToEnd)
        {
            throw new NotImplementedException();
        }
    }
}
