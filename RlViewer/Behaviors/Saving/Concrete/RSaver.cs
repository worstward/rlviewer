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
    class RSaver : RlViewer.Behaviors.Saving.Abstract.Saver
    {
        public RSaver(Files.LocatorFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.R;
            _head = _file.Header as RlViewer.Headers.Concrete.R.RHeader;
        }

        public override Files.LocatorFile SourceFile
        {
            get
            {
                return _file;
            }
        }


        private RlViewer.Files.Rli.Concrete.R _file;
        private RlViewer.Headers.Concrete.R.RHeader _head;

        public override void Save(string path, RlViewer.FileType destinationType, Rectangle area, 
            float normalization, float maxValue, System.Drawing.Imaging.ColorPalette palette, Filters.ImageFilterProxy filter)
        {
            switch (destinationType)
            {
                case FileType.raw:
                    SaveAsRaw(path, area);
                    break;
                case FileType.bmp:
                    SaveAsBmp(path, area, normalization, maxValue, new DataProcessor.Concrete.DataStringSampleProcessor(),
                        palette, filter);
                    break;
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }


        public override void SaveAsAligned(string alignedFileName, System.Drawing.Rectangle area, byte[] image,
            int aligningPointsCount, int rangeCompressionCoef, int azimuthCompressionCoef)
        {
            alignedFileName = Path.ChangeExtension(alignedFileName, "raw");
            File.WriteAllBytes(alignedFileName, image);
        }


    }
}
