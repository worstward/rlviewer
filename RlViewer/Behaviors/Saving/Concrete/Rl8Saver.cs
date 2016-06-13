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
                case FileType.rl8:
                    base.SaveAsRl4(path, area, ".rl8");
                    break;
                case FileType.rl4:
                    SaveAsRl4(path, area, ".rl4");
                    break;
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }

        protected override void SaveAsRl4(string path, Rectangle area, string newExt)
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

                    rl4Header.rlParams.type = 2;//represents floating point sample

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Header),
                    0, Marshal.SizeOf(rl4Header));


                    byte[] strHeader = new byte[SourceFile.Header.StrHeaderLength];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[area.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    var sampleToStartSaving = area.X * _file.Header.BytesPerSample;


                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {
                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }

                        fr.Read(strHeader, 0, SourceFile.Header.StrHeaderLength);
                        fw.Write(strHeader, 0, SourceFile.Header.StrHeaderLength);

                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameData, 0, frameData.Length);

                        var floatSamples = new float[frameData.Length / sizeof(float)];
                        var amplitudes = new float[floatSamples.Length / 2];

                        var amplitudeBytes = new byte[amplitudes.Length * sizeof(float)];
                        int index = 0;
                        Buffer.BlockCopy(frameData, 0, floatSamples, 0, frameData.Length);

                        for (int j = 0; j < floatSamples.Length; j += 2)
                        {
                            var re = floatSamples[j];
                            var im = floatSamples[j + 1];
                            amplitudes[index++] = (float)(Math.Sqrt(re * re + im * im));
                        }
                        Buffer.BlockCopy(amplitudes, 0, amplitudeBytes, 0, amplitudeBytes.Length);

                        
                        fw.Write(amplitudeBytes, 0, amplitudeBytes.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }

        public override void SaveAsAligned(string fileName, Rectangle area, byte[] image)
        {
            throw new NotImplementedException();
        }


    }
}
