using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.CornerCoord.Concrete
{
    class Rl8CornerCoord : CornerCoord.Abstract.CornerCoordinates
    {
        public Rl8CornerCoord(Files.LocatorFile file)
            : base(file)
        { }


        protected override RlViewer.Navigation.NavigationContainer GetContainer()
        {
            var container = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(File.Properties).Create(File.Properties, File.Header);

            var naviStrings = container.ConvertToCommonNavigation(GetNaviStrings<Headers.Concrete.Rl4.Rl4StrHeaderStruct>
                (File.Properties.FilePath, File.Header.FileHeaderLength, File.Width * File.Header.BytesPerSample).Cast<Headers.Abstract.IStrHeader>().ToArray());

            container.NaviStrings = naviStrings;

            return container;
        }
    }
}
