using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation
{
    public class Navigation
    {
        public Navigation(RlViewer.Files.FileProperties properties, float initialRange, float step, byte board, int headerLength, int dataLength)
        {
            _naviContainer = GetNavigationContainer(properties, initialRange, step, board, headerLength, dataLength);
            _computer = new Behaviors.Navigation.NavigationComputing(initialRange, step);
        }


        private NavigationContainer _naviContainer;
        private RlViewer.Behaviors.Navigation.NavigationComputing _computer;

        public Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0]
        {
            get
            {
                return _naviContainer[stringNumber].NaviInfo(sampleNumber, _computer);    //.NaviInfo();          
            }
        }


        private NavigationContainer GetNavigationContainer(RlViewer.Files.FileProperties properties, float initialRange, float step, byte board, int headerLength, int dataLength)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new RlViewer.Navigation.Concrete.Brl4NavigationContainer(properties.FilePath, initialRange, step, board, headerLength, dataLength);
                //case FileType.k:
                //    return new RlViewer.Navigation.Concrete.RhgKNavigationContainer(properties.FilePath, board);
                //case FileType.raw:
                //    return new RlViewer.Navigation.Concrete.RawNavigationContainer(properties.FilePath, board);
                case FileType.rl4:
                    return new RlViewer.Navigation.Concrete.Rl4NavigationContainer(properties.FilePath, initialRange, step, board, headerLength, dataLength);

                default: throw new ArgumentException();
            }
        }

    }
}
