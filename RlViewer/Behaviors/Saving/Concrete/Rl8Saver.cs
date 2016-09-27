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
    class Rl8Saver : RlViewer.Behaviors.Saving.Concrete.Rl4Saver
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
        private DataProcessor.Abstract.DataStringProcessor _processor;
        protected override DataProcessor.Abstract.DataStringProcessor Processor
        {
            get
            {
                return _processor = _processor ?? new DataProcessor.Concrete.DataStringModulusProcessor();
            }
        }

        public override void Save(string path, RlViewer.FileType destinationType, Rectangle area,
            float normalization, float maxValue, System.Drawing.Imaging.ColorPalette palette, Filters.ImageFilterProxy filter)
        {
            switch (destinationType)
            {
                case FileType.raw:
                    SaveAsRaw(path, area);
                    break;
                case FileType.bmp:
                    SaveAsBmp(path, area, normalization, maxValue, Processor,
                        palette, filter);
                    break;
                case FileType.rl4:
                    SaveAsRl4(path, area, ".rl4", RlViewer.Headers.Concrete.Rl4.SampleType.Float, Processor);
                    break;
                case FileType.rl8:
                    SaveAsRl4(path, area, ".rl8", RlViewer.Headers.Concrete.Rl4.SampleType.Complex,
                        new DataProcessor.Concrete.DataStringSampleProcessor());
                    break;
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }



    }
}
