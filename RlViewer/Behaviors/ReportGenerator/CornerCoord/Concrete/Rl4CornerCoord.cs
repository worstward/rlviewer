﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.CornerCoord.Concrete
{
    class Rl4CornerCoord : CornerCoord.Abstract.CornerCoordinates
    {
        public Rl4CornerCoord(Files.LocatorFile file, int firstLine, int lastLine, bool readToEnd)
            : base(file, firstLine, lastLine, readToEnd)
        { }


        protected override RlViewer.Navigation.NavigationContainer GetContainer(int firstLine, int lastLine, bool readToEnd)
        {
            var container = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(File.Properties).Create(File.Properties, File.Header);

            var naviStrings = container.ConvertToCommonNavigation(GetFirstAndLastNaviStrings<Headers.Concrete.Rl4.Rl4StrHeaderStruct>
                (File.Properties.FilePath, File.Header.FileHeaderLength, File.Width * File.Header.BytesPerSample, firstLine, lastLine, readToEnd)
                .Cast<Headers.Abstract.IStrHeader>().ToArray());

            container.NaviStrings = naviStrings;

            return container;
        }
    }
}
