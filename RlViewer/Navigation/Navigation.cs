using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation
{
    public class Navigation
    {
        public Navigation(RlViewer.Files.FileProperties properties, byte board, int headerLength, int dataLength)
        {
            _naviContainer = GetNavigationContainer(properties, board, headerLength, dataLength);
        }

        private NavigationContainer _naviContainer;

        public NavigationString this[int stringNumber]
        {
            get
            {
                return _naviContainer[stringNumber];
            }
        }

        private NavigationContainer GetNavigationContainer(RlViewer.Files.FileProperties properties, byte board, int headerLength, int dataLength)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new RlViewer.Navigation.Concrete.Brl4NavigationContainer(properties.FilePath, board, headerLength, dataLength);
                case FileType.k:
                    return new RlViewer.Navigation.Concrete.RhgKNavigationContainer(properties.FilePath, board);
                case FileType.raw:
                    return new RlViewer.Navigation.Concrete.RawNavigationContainer(properties.FilePath, board);
                case FileType.rl4:
                    return new RlViewer.Navigation.Concrete.Rl4NavigationContainer(properties.FilePath, board, headerLength, dataLength);

                default: throw new ArgumentException();
            }
        }

    }
}
