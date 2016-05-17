using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Behaviors.Saving.Concrete
{
    class Brl4Saver : RlViewer.Behaviors.Saving.Abstract.Saver
    {
        public Brl4Saver(Files.LocatorFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Brl4;
            _head = _file.Header as RlViewer.Headers.Concrete.Brl4.Brl4Header;
        }

        public override Files.LocatorFile SourceFile
        {
            get 
            {
                return _file;    
            }
        }

      

        private RlViewer.Files.Rli.Concrete.Brl4 _file;
        private RlViewer.Headers.Concrete.Brl4.Brl4Header _head;

        public override void Save(string path, RlViewer.FileType destinationType, Rectangle area, float normalization, float maxValue)
        {
            switch (destinationType)
            {
                case FileType.brl4:
                    SaveAsBrl4(path, area);
                    break;
                case FileType.raw:
                    SaveAsRaw(path, area);
                    break;
                case FileType.rl4:
                    SaveAsRl4(path, area);
                    break;
                case FileType.bmp:
                    SaveAsBmp(path, area, normalization, maxValue);
                    break;
                default:
                    throw new NotSupportedException("Unsupported destination type");
            }
        }


        public override void SaveAsAligned(string fileName, System.Drawing.Rectangle area, byte[] image)
        {
            var alignedFileName = Path.GetFileNameWithoutExtension(fileName) + "_aligned";

            alignedFileName = Path.ChangeExtension(alignedFileName, "brl4");
            
            Headers.Concrete.Brl4.Brl4RliFileHeader brlHeadStruct;

            byte[] strHeader = new byte[SourceFile.Header.StrHeaderLength];
            byte[] strData = new byte[area.Width * _file.Header.BytesPerSample];
            
            var brlHead = _file.Header as Headers.Concrete.Brl4.Brl4Header;

            var rlParams = brlHead.HeaderStruct.rlParams
                .ChangeFragmentShift(area.X, area.Y).ChangeImgDimensions(area.Width, area.Height);
            brlHeadStruct = new Headers.Concrete.Brl4.Brl4RliFileHeader(brlHead.HeaderStruct.fileSign, brlHead.HeaderStruct.fileVersion,
                brlHead.HeaderStruct.rhgParams, rlParams, brlHead.HeaderStruct.synthParams, brlHead.HeaderStruct.reserved);

            using (var ms = new MemoryStream(image))
            {
                
                using (var fr = File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var fw = File.Open(alignedFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var headBytes = RlViewer.Files.LocatorFile.WriteStruct<Headers.Concrete.Brl4.Brl4RliFileHeader>(brlHeadStruct);
                        fw.Write(headBytes, 0, headBytes.Length);
                        fr.Seek(_file.Header.FileHeaderLength, SeekOrigin.Current);
                        fr.Seek((strHeader.Length + _file.Width * _file.Header.BytesPerSample) * area.Y, SeekOrigin.Current);

                        for (int i = 0; i < area.Height; i++)
                        {
                            fr.Read(strHeader, 0, strHeader.Length);
                            fr.Seek(_file.Width * _file.Header.BytesPerSample, SeekOrigin.Current);
                            ms.Read(strData, 0, strData.Length);
                            fw.Write(strHeader, 0, strHeader.Length);
                            fw.Write(strData, 0, strData.Length);
                        }
                    }
                }
            }

        }


        private void SaveAsRl4(string path, Rectangle area)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".rl4");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.rlParams
                        .ChangeImgDimensions(area.Width, area.Height)
                        .ChangeFragmentShift(area.X, area.Y);

                    RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader brl4Header =
                        new Headers.Concrete.Brl4.Brl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    var rl4Header = brl4Header.ToRl4(); 

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Header),
                    0, Marshal.SizeOf(rl4Header));


                    byte[] strHeader = new byte[SourceFile.Header.StrHeaderLength];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[area.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    var sampleToStartSaving = area.X * _file.Header.BytesPerSample;
                        //+ leftTop.X * _file.Header.BytesPerSample;

                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {
                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }


                        //read-write string header
                        fr.Read(strHeader, 0, SourceFile.Header.StrHeaderLength);
                        var rl4StrHead = Converters.FileHeaderConverters.ToRl4StrHeader(strHeader, 100, 100, 100);
                        fw.Write(rl4StrHead, 0, SourceFile.Header.StrHeaderLength);

                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameData, 0, frameData.Length);
                        fw.Write(frameData, 0, frameData.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }
            }
        }


        private void SaveAsBrl4(string path, Rectangle area)
        {

            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".brl4");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.rlParams
                        .ChangeImgDimensions(area.Width, area.Height)
                        .ChangeFragmentShift(area.X, area.Y);

                    RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader rl4Header =
                        new Headers.Concrete.Brl4.Brl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader>(rl4Header),
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
                        fw.Write(frameData, 0, frameData.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }
                }
            }
        }

    }
}
