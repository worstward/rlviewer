using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace RlViewer.Behaviors.Saving.Concrete
{
    class RawSaver : RlViewer.Behaviors.Saving.Abstract.Saver
    {
        public RawSaver(Files.LocatorFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Raw;
            _head = _file.Header as RlViewer.Headers.Concrete.Raw.RawHeader;
        }

        public override Files.LocatorFile SourceFile
        {
            get
            {
                return _file;
            }
        }

        private RlViewer.Files.Rli.Concrete.Raw _file;
        private RlViewer.Headers.Concrete.Raw.RawHeader _head;


        protected override void SaveAndReport(string path, RlViewer.FileType destinationType, Rectangle area,
            float normalization, float maxValue, System.Drawing.Imaging.ColorPalette palette, Filters.ImageFilterProxy filter)
        {           
            switch (destinationType)
            {
                case FileType.raw:
                    SaveAsRaw(path, area);
                    break;
                case FileType.bmp:
                    {
                        DataProcessor.Abstract.DataStringProcessor processor = null;

                        if (_file.Header.BytesPerSample == 4)
                        {
                            processor = new DataProcessor.Concrete.DataStringSampleProcessor();
                        }
                        else if (_file.Header.BytesPerSample == 8)
                        {
                            processor = new DataProcessor.Concrete.DataStringModulusProcessor();
                        }
                        else
                        {
                            throw new ArgumentException("Bytes per sample");
                        }

                        SaveAsBmp(path, area, normalization, maxValue, processor, palette, filter);
                        break;
                    }
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
