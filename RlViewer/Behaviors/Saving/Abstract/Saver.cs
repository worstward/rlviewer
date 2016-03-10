using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Saving.Abstract
{
    //make factory
    abstract class Saver
    {
        public Saver(Files.LocatorFile file)
        {

        }


        public abstract void Save(string path, FileType saveAsType, Point leftTop, Size areaSize);

    }
}
