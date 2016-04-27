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
        public Brl4Saver(Files.LoadedFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Brl4;
            _head = _file.Header as RlViewer.Headers.Concrete.Brl4.Brl4Header;
        }

        private RlViewer.Files.Rli.Concrete.Brl4 _file;
        private RlViewer.Headers.Concrete.Brl4.Brl4Header _head;

        public override void Save(string path, RlViewer.FileType destinationType, Rectangle area, float normalization)
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
                    SaveAsBmp(path, area, normalization);
                    break;
                default:
                    throw new ArgumentException();
            }
        }


        public override void SaveAsAligned(string fileName, System.Drawing.Rectangle area, byte[] image)
        {
            fileName = Path.ChangeExtension(fileName + "_aligned", "brl4");

            Headers.Concrete.Brl4.Brl4RliFileHeader brlHeadStruct;
            byte[] strHeader;
            byte[] strData = new byte[area.Width * _file.Header.BytesPerSample];
            
            var brlHead = _file.Header as Headers.Concrete.Brl4.Brl4Header;
            strHeader = new byte[System.Runtime.InteropServices.Marshal.SizeOf(new Headers.Concrete.Brl4.Brl4StrHeaderStruct())];
            var rlParams = brlHead.HeaderStruct.rlParams
                .ChangeFragmentShift(area.X, area.Y).ChangeImgDimensions(area.Width, area.Height);
            brlHeadStruct = new Headers.Concrete.Brl4.Brl4RliFileHeader(brlHead.HeaderStruct.fileSign, brlHead.HeaderStruct.fileVersion,
                brlHead.HeaderStruct.rhgParams, rlParams, brlHead.HeaderStruct.synthParams, brlHead.HeaderStruct.reserved);

            using (var ms = new MemoryStream(image))
            {
                using (var fr = File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var fw = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader()), SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.rlParams
                        .ChangeImgDimensions(area.Width, area.Height)
                        .ChangeFragmentShift(area.X, area.Y);

                    RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader brl4Header =
                        new Headers.Concrete.Brl4.Brl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    var rl4Header = brl4Header.ToRl4(); 

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Header),
                    0, Marshal.SizeOf(rl4Header));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[area.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
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
                        fr.Read(strHeader, 0, strHeaderSize);
                        var rl4StrHead = Converters.FileHeaderConverters.ToRl4StrHeader(strHeader, 100, 100, 100);
                        fw.Write(rl4StrHead, 0, strHeaderSize);

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
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.rlParams
                        .ChangeImgDimensions(area.Width, area.Height)
                        .ChangeFragmentShift(area.X, area.Y);

                    RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader rl4Header =
                        new Headers.Concrete.Brl4.Brl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader>(rl4Header),
                    0, Marshal.SizeOf(rl4Header));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[area.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = area.X * _file.Header.BytesPerSample;


                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {
                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }


                        fr.Read(strHeader, 0, strHeaderSize);
                        fw.Write(strHeader, 0, strHeaderSize);

                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameData, 0, frameData.Length);
                        fw.Write(frameData, 0, frameData.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }
                }
            }
        }



        private void SaveAsRaw(string path, Rectangle area)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".raw");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[area.Width * _file.Header.BytesPerSample];

                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());


                    var lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = area.X * _file.Header.BytesPerSample;

                    fr.Seek(fileHeaderSize, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {

                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }

                        //fr.Seek(leftTop.X * )
                        //read-write string data
                        fr.Seek(strHeaderSize, SeekOrigin.Current);

                        //fr.Seek(leftTop.X * )
                        //read-write string data
                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameStrData, 0, frameStrData.Length);
                        fw.Write(frameStrData, 0, frameStrData.Length);
                        fr.Seek(strDataLength - frameStrData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }

        private void SaveAsBmp(string path, Rectangle area, float normalization = 0)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".bmp");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[area.Width * _file.Header.BytesPerSample];
                    float[] floatFrameStrData = new float[area.Width];

                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());


                    var lineToStartSaving = area.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = area.X * _file.Header.BytesPerSample;

                    fr.Seek(fileHeaderSize, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);



                    var rgbSize = Marshal.SizeOf(new RGBQUAD());
                    var headerSize = Marshal.SizeOf(new BITMAPINFOHEADER()) + Marshal.SizeOf(new BITMAPFILEHEADER()) + rgbSize * 256;

                    var bmpFileheader = new BITMAPFILEHEADER((uint)(area.Width * area.Height + headerSize),
                        (uint)headerSize);


                    List<byte> palette = new List<byte>();

                    for (int i = 0; i < 256; i++)
                    {
                        palette.AddRange(RlViewer.Files.LocatorFile.WriteStruct<RGBQUAD>(new RGBQUAD((byte)i, (byte)i, (byte)i)));
                    }


                    var bmpInfoHeader = new BITMAPINFOHEADER(area.Width, area.Height, (uint)(area.Height * area.Width));



                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<BITMAPFILEHEADER>(bmpFileheader),
               0, Marshal.SizeOf(bmpFileheader));
                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<BITMAPINFOHEADER>(bmpInfoHeader),
                0, Marshal.SizeOf(bmpInfoHeader));
                    fw.Write(palette.ToArray(), 0, palette.Count);

                    var padBytes = new byte[(int)(Math.Ceiling((double)(area.Width / 4f))) * 4 - area.Width];


                    for (int i = 0; i < area.Height; i++)
                    {

                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }

                        fr.Seek(strHeaderSize, SeekOrigin.Current);
                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameStrData, 0, frameStrData.Length);
                        Buffer.BlockCopy(frameStrData, 0, floatFrameStrData, 0, frameStrData.Length);
                        var a = floatFrameStrData.Select(x => (byte)(x * normalization)).ToArray();
                        fw.Write(a, 0, floatFrameStrData.Length);

                        fw.Write(padBytes, 0, padBytes.Length);

                        fr.Seek(strDataLength - frameStrData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }

    }
}
