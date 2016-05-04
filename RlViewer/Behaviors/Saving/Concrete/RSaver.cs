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
        public RSaver(Files.LoadedFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.R;
            _head = _file.Header as RlViewer.Headers.Concrete.R.RHeader;
        }

        private RlViewer.Files.Rli.Concrete.R _file;
        private RlViewer.Headers.Concrete.R.RHeader _head;

        public override void Save(string path, RlViewer.FileType destinationType, Rectangle area, float normalization = 0)
        {
            switch (destinationType)
            {
                case FileType.raw:
                    SaveAsRaw(path, area);
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
            var alignedFileName = Path.GetFileNameWithoutExtension(fileName) + "_aligned";

            alignedFileName = Path.ChangeExtension(alignedFileName, "raw");


            File.WriteAllBytes(alignedFileName, image);
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


                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.R.RFileHeaderStruct());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.R.RStrHeaderStruct());


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

                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.R.RFileHeaderStruct());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.R.RStrHeaderStruct());


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
