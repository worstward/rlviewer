using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    class RNavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            var rHeader = header as RlViewer.Headers.Concrete.R.RHeader;
            if (rHeader == null) throw new ArgumentException("rHeader");
            return new RNavigationContainer(properties.FilePath, rHeader.HeaderStruct.synthesisHeader.initialRange,
                rHeader.HeaderStruct.synthesisHeader.dx, rHeader.HeaderStruct.synthesisHeader.sideObservation,
                rHeader.FileHeaderLength, (int)rHeader.HeaderStruct.lineInfoHeader.lineLength * rHeader.BytesPerSample);
        }
    }
}
