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

        private DataProcessor.Concrete.DataStringModulusProcessor _processor;
        protected override DataProcessor.Abstract.DataStringProcessor Processor
        {
            get
            {
                return _processor = _processor ?? new DataProcessor.Concrete.DataStringModulusProcessor();
            }
        }
       


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
                        SaveAsBmp(saverParams.Path, saverParams.SavingArea, normalization, maxValue, Processor, saverParams.OutputType,
                            saverParams.Palette, saverParams.ImageFilter);
                        break;
                    }
                case FileType.rl4:
                    { 
                    SaveAsRl4(saverParams.Path, saverParams.SavingArea, ".rl4", RlViewer.Headers.Concrete.Rl4.SampleType.Float, Processor);
                    break;
                    }
                case FileType.rl8:
                    {
                        SaveAsRl4(saverParams.Path, saverParams.SavingArea, ".rl8", RlViewer.Headers.Concrete.Rl4.SampleType.Complex,
                        new DataProcessor.Concrete.DataStringSampleProcessor());
                        break;
                    }
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }



    }
}
