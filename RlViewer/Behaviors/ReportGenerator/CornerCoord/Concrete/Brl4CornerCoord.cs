using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.CornerCoord.Concrete
{
    class Brl4CornerCoord : CornerCoord.Abstract.CornerCoordinates
    {
        public Brl4CornerCoord(Files.LocatorFile file)
            : base(file)
        { }


        protected override RlViewer.Navigation.NavigationContainer GetContainer()
        {
            var container = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(File.Properties).Create(File.Properties, File.Header);

            var naviStringsBrl = GetNaviStrings<Headers.Concrete.Brl4.Brl4StrHeaderStruct>
                (File.Properties.FilePath, File.Header.FileHeaderLength, File.Width * File.Header.BytesPerSample);

            var naviStrings = container.ConvertToCommonNavigation(naviStringsBrl.Cast<Headers.Abstract.IStrHeader>().ToArray());
            container.NaviStrings = naviStrings;

            return container;
        }
    }
}
