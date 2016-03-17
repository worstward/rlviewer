using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;


namespace RlViewer.Behaviors.Saving.Abstract
{
    public abstract class Saver
    {
        public Saver(Files.LoadedFile file)
        {

        }

        public abstract void Save(string path, RlViewer.FileType destinationType, Point leftTop, Size areaSize, float normalization);

    }
}
