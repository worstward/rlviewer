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
    class Rl4Saver : RlViewer.Behaviors.Saving.Abstract.Saver
    {
        public Rl4Saver(Files.LocatorFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Rl4;
            _head = _file.Header as RlViewer.Headers.Concrete.Rl4.Rl4Header;
        }

        public override Files.LocatorFile SourceFile
        {
            get
            {
                return _file;
            }
        }


        private RlViewer.Files.Rli.Concrete.Rl4 _file;
        private RlViewer.Headers.Concrete.Rl4.Rl4Header _head;

        private DataProcessor.Abstract.DataStringProcessor _processor;
        protected override DataProcessor.Abstract.DataStringProcessor Processor
        {
            get
            {
                return _processor = _processor ?? new DataProcessor.Concrete.DataStringSampleProcessor();
            }
        }

        protected override void SaveAndReport(SaverParams saverParams, float normalization, float maxValue)
        {
            switch (saverParams.DestinationType)
            {
                case FileType.brl4:
                    {
                        SaveAsBrl4(saverParams.Path, saverParams.SavingArea, ".brl4", Processor);
                        break;
                    }
                case FileType.raw:
                    {
                        SaveAsRaw(saverParams.Path, saverParams.SavingArea);
                        break;
                    }
                case FileType.rl4:
                    {
                        SaveAsRl4(saverParams.Path, saverParams.SavingArea, ".rl4", RlViewer.Headers.Concrete.Rl4.SampleType.Float, Processor);
                        break;
                    }
                case FileType.bmp:
                    {
                        SaveAsBmp(saverParams.Path, saverParams.SavingArea, normalization, maxValue, Processor, saverParams.OutputType,
                            saverParams.Palette, saverParams.ImageFilter);
                        break;
                    }
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }

        public override void SaveAsAligned(string alignedFileName, System.Drawing.Rectangle area, byte[] image,
            int aligningPointsCount, int rangeCompressionCoef, int azimuthCompressionCoef)
        {
            alignedFileName = Path.ChangeExtension(alignedFileName, "brl4");

            Headers.Concrete.Brl4.Brl4RliFileHeader brlHeadStruct;
            byte[] strHeader = new byte[SourceFile.Header.StrHeaderLength];
            byte[] strData = new byte[area.Width * _file.Header.BytesPerSample];

            var rlHead = _file.Header as Headers.Concrete.Rl4.Rl4Header;

            brlHeadStruct = rlHead.HeaderStruct.ToBrl4(1, 1, 1);

            //angle_zond = arccos(height) * initialRange
            var rlParams = brlHeadStruct.rlParams
                .ChangeFragmentShift(area.X, area.Y).ChangeImgDimensions(area.Width, area.Height);
            brlHeadStruct = new Headers.Concrete.Brl4.Brl4RliFileHeader(brlHeadStruct.fileSign,
                brlHeadStruct.fileVersion, brlHeadStruct.rhgParams, rlParams, brlHeadStruct.synthParams,
                aligningPointsCount, rangeCompressionCoef, azimuthCompressionCoef, brlHeadStruct.reserved);


            using (var ms = new MemoryStream(image))
            {
                using (var fr = File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var fw = File.Open(alignedFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var headBytes = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Headers.Concrete.Brl4.Brl4RliFileHeader>(brlHeadStruct);
                        long offset = (strHeader.Length + _file.Width * _file.Header.BytesPerSample) * area.Y;
                        fw.Write(headBytes, 0, headBytes.Length);
                        fr.Seek(_file.Header.FileHeaderLength, SeekOrigin.Current);
                        fr.Seek(offset, SeekOrigin.Current);

                        for (int i = 0; i < area.Height; i++)
                        {
                            fr.Read(strHeader, 0, strHeader.Length);
                            fr.Seek(_file.Width * _file.Header.BytesPerSample, SeekOrigin.Current);
                            ms.Read(strData, 0, strData.Length);
                            fw.Write(strHeader, 0, strHeader.Length);

                            var processedStr = Processor.ProcessRawDataString(strData);

                            fw.Write(processedStr, 0, processedStr.Length);
                        }
                    }
                }
            }

        }


        protected virtual void SaveAsRl4(string path, Rectangle area, string newExt,
            RlViewer.Headers.Concrete.Rl4.SampleType sampleType, DataProcessor.Abstract.DataStringProcessor processor)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, newExt);
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.rlParams
                        .ChangeImgDimensions(area.Width, area.Height)
                        .ChangeFragmentShift(area.X, area.Y);

                    RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader rl4Header =
                        new Headers.Concrete.Rl4.Rl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    rl4Header.rlParams.type = sampleType;

                    fw.Write(RlViewer.Behaviors.Converters.StructIO.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Header),
                    0, Marshal.SizeOf(rl4Header));

                    byte[] strHeader = new byte[SourceFile.Header.StrHeaderLength];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[area.Width * _file.Header.BytesPerSample];

                    long lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    long sampleToStartSaving = area.X * _file.Header.BytesPerSample;


                    fr.Seek(lineToStartSaving, SeekOrigin.Current);


                    for (int i = 0; i < area.Height; i++)
                    {
                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        OnCancelWorker();

                        fr.Read(strHeader, 0, SourceFile.Header.StrHeaderLength);
                        fw.Write(strHeader, 0, SourceFile.Header.StrHeaderLength);

                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameData, 0, frameData.Length);

                        var processedFrame = processor.ProcessRawDataString(frameData);

                        fw.Write(processedFrame, 0, processedFrame.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }


        protected void SaveAsBrl4(string path, Rectangle area, string newExt, DataProcessor.Abstract.DataStringProcessor processor)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, newExt);
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader)), SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.rlParams
                        .ChangeImgDimensions(area.Width, area.Height)
                        .ChangeFragmentShift(area.X, area.Y);

                    RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader rl4Header =
                        new Headers.Concrete.Rl4.Rl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    var brl4Head = rl4Header.ToBrl4(0, 1, 0);

                    fw.Write(RlViewer.Behaviors.Converters.StructIO.WriteStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader>(brl4Head),
                    0, Marshal.SizeOf(brl4Head));

                    byte[] strHeader = new byte[SourceFile.Header.StrHeaderLength];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[area.Width * _file.Header.BytesPerSample];

                    long lineToStartSaving = (long)area.Y * (long)(_file.Width * _file.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    long sampleToStartSaving = area.X * _file.Header.BytesPerSample;


                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {
                        //read-write string header
                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }


                        fr.Read(strHeader, 0, SourceFile.Header.StrHeaderLength);
                        var brl4StrHead = Converters.FileHeaderConverters.ToBrl4StrHeader(strHeader);
                        fw.Write(brl4StrHead, 0, SourceFile.Header.StrHeaderLength);

                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameData, 0, frameData.Length);

                        var processedFrame = processor.ProcessRawDataString(frameData);

                        fw.Write(processedFrame, 0, processedFrame.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }

    }
}
