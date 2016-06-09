using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Behaviors.Saving.Concrete
{
    class Rl8Saver : RlViewer.Behaviors.Saving.Abstract.Saver
    {
        public Rl8Saver(Files.LocatorFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Rl8;
            _head = _file.Header as RlViewer.Headers.Concrete.Rl8.Rl8Header;
        }

        public override Files.LocatorFile SourceFile
        {
            get
            {
                return _file;
            }
        }


        private RlViewer.Files.Rli.Concrete.Rl8 _file;
        private RlViewer.Headers.Concrete.Rl8.Rl8Header _head;


        public override void Save(string path, RlViewer.FileType destinationType, Rectangle area, Filters.ImageFilterFacade filter, float normalization, float maxValue)
        {
            switch (destinationType)
            {
                case FileType.raw:
                    SaveAsRaw(path, area);
                    break;
                case FileType.bmp:
                    SaveAsBmp(path, area, normalization, maxValue, filter);
                    break;
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }

        public override void SaveAsAligned(string fileName, Rectangle area, byte[] image)
        {
            throw new NotImplementedException();
        }


    }
}
