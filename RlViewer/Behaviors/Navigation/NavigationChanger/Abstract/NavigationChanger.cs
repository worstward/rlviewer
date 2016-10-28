using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.NavigationChanger.Abstract
{
    public abstract class NavigationChanger
    {
        public NavigationChanger(Files.LocatorFile fileToChange, Files.LocatorFile sourceFile)
        {
            SourceFile = sourceFile;
        }

        public Files.LocatorFile SourceFile
        {
            get;
            private set;
        }

        public abstract void ChangeNavigation();

        protected Headers.Abstract.IStrHeader GetSourceNavigationHeader(System.IO.Stream s)
        {
            switch (SourceFile.Properties.Type)
            {
                case FileType.k:
                    return Behaviors.Converters.StructIO.ReadStruct<Headers.Concrete.K.KStrHeaderStruct>(s);
                case FileType.ba:
                    return Behaviors.Converters.StructIO.ReadStruct<Headers.Concrete.Ba.BaStrHeader>(s);
                default: 
                    throw new ArgumentException("SourceFile type");
            }             
        }


    }
}
