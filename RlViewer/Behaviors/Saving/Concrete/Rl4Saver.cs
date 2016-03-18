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
        public Rl4Saver(Files.LoadedFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Rl4;
            _head = _file.Header as RlViewer.Headers.Concrete.Rl4.Rl4Header;
        }

        private RlViewer.Files.Rli.Concrete.Rl4 _file;
        private RlViewer.Headers.Concrete.Rl4.Rl4Header _head;

        public override void Save(string path, RlViewer.FileType destinationType, Point leftTop, Size areaSize, float normalization = 0)
        {           
            switch (destinationType)
            {
                case FileType.brl4:
                    SaveAsBrl4(path, leftTop, areaSize);
                    break;
                case FileType.raw:
                    SaveAsRaw(path, leftTop, areaSize);
                    break;
                case FileType.rl4:
                    SaveAsRl4(path, leftTop, areaSize);
                    break;
                case FileType.bmp:
                    SaveAsBmp(path, leftTop, areaSize, normalization);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void SaveAsRl4(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".rl4");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.ChangeImgDimensions(areaSize.Width, areaSize.Height);

                    RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader rl4Header =
                        new Headers.Concrete.Rl4.Rl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Header),
                    0, Marshal.SizeOf(rl4Header));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[areaSize.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;


                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {
                        OnProgressReport((int)((double)i / (double)areaSize.Height * 100));
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


        private void SaveAsBrl4(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".brl4");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.ChangeImgDimensions(areaSize.Width, areaSize.Height);

                    RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader rl4Header =
                        new Headers.Concrete.Rl4.Rl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);
                    
                    var brl4Head = rl4Header.ToBrl4(0, 1, 30);

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader>(brl4Head),
                    0, Marshal.SizeOf(brl4Head));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[areaSize.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;


                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {
                        //read-write string header
                        OnProgressReport((int)((double)i / (double)areaSize.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }



                        fr.Read(strHeader, 0, strHeaderSize);
                        var brl4StrHead = Converters.FileHeaderConverters.ToBrl4StrHeader(strHeader);
                        fw.Write(brl4StrHead, 0, strHeaderSize);

                        //fr.Seek(leftTop.X * )
                        //read-write string data
                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameData, 0, frameData.Length);
                        fw.Write(frameData, 0, frameData.Length);
                        fr.Seek(strDataLength - frameData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }

        private void SaveAsRaw(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".raw");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[areaSize.Width * _file.Header.BytesPerSample];

                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());


                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;

                    fr.Seek(fileHeaderSize, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {


                        OnProgressReport((int)((double)i / (double)areaSize.Height * 100));
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


        private void SaveAsBmp(string path, Point leftTop, Size areaSize, float normalization)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".bmp");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[areaSize.Width * _file.Header.BytesPerSample];
                    float[] floatFrameStrData = new float[areaSize.Width];

                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());


                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;

                    fr.Seek(fileHeaderSize, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);



                    var rgbSize = Marshal.SizeOf(new RGBQUAD());
                    var headerSize = Marshal.SizeOf(new BITMAPINFOHEADER()) + Marshal.SizeOf(new BITMAPFILEHEADER()) + rgbSize * 256;

                    var bmpFileheader = new BITMAPFILEHEADER((uint)(areaSize.Width * areaSize.Height + headerSize),
                        (uint)headerSize);


                    List<byte> palette = new List<byte>();

                    for (int i = 0; i < 256; i++)
                    {
                        palette.AddRange(RlViewer.Files.LocatorFile.WriteStruct<RGBQUAD>(new RGBQUAD((byte)i, (byte)i, (byte)i)));
                    }


                    var bmpInfoHeader = new BITMAPINFOHEADER(areaSize.Width, areaSize.Height, (uint)(areaSize.Height * areaSize.Width));



                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<BITMAPFILEHEADER>(bmpFileheader),
               0, Marshal.SizeOf(bmpFileheader));
                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<BITMAPINFOHEADER>(bmpInfoHeader),
                0, Marshal.SizeOf(bmpInfoHeader));
                    fw.Write(palette.ToArray(), 0, palette.Count);

                    var padBytes = new byte[(int)(Math.Ceiling((double)(areaSize.Width / 4f))) * 4 - areaSize.Width];


                    for (int i = 0; i < areaSize.Height; i++)
                    {

                        OnProgressReport((int)((double)i / (double)areaSize.Height * 100));
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
