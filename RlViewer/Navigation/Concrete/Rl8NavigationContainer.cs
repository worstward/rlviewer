using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Navigation.Concrete
{
    public class Rl8NavigationContainer : Rl4NavigationContainer
    {

        public Rl8NavigationContainer(string path, float initialRange, byte flipType, int imageWidth, float step, byte board,
            int headerLength, int dataLength, int sx, int sy)
            : base(path, initialRange, flipType, imageWidth, step, board, headerLength, dataLength, sx, sy)
        {

        }
    }
}
