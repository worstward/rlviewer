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





        protected override void SaveAndReport(SaverParams saverParams, float normalization, float maxValue)
        {
            switch (saverParams.DestinationType)
            {
                case FileType.raw:
                    {
                        SaveAsRaw(saverParams.Path, saverParams.SavingArea);
                        break;
                    }
                case FileType.bmp:
                    {
                        DataProcessor.Abstract.DataStringProcessor processor = null;

                        if (_file.Header.BytesPerSample == 4)
                        {
                            processor = Processor;
                        }
                        else if (_file.Header.BytesPerSample == 8)
                        {
                            processor = new DataProcessor.Concrete.DataStringModulusProcessor();
                        }
                        else
                        {
                            throw new ArgumentException("Bytes per sample");
                        }

                        SaveAsBmp(saverParams.Path, saverParams.SavingArea, normalization, maxValue, processor, saverParams.OutputType, saverParams.Palette, saverParams.ImageFilter);
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
