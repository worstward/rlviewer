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
        public RawSaver(Files.LoadedFile file)
            : base(file)
        {
            _file = file as RlViewer.Files.Rli.Concrete.Raw;
            _head = _file.Header as RlViewer.Headers.Concrete.Raw.RawHeader;
        }

        private RlViewer.Files.Rli.Concrete.Raw _file;
        private RlViewer.Headers.Concrete.Raw.RawHeader _head;

        public override void Save(string path, RlViewer.FileType destinationType, Point leftTop, Size areaSize)
        {           
            switch (destinationType)
            {
                case FileType.raw:
                    SaveAsRaw(path, leftTop, areaSize);
                    break;
                case FileType.bmp:
                    SaveAsBmp(path, leftTop, areaSize);
                    break;
                default:
                    throw new ArgumentException();
            }
        }


     

        private void SaveAsRaw(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".raw";
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                { 
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[areaSize.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;

                    for (int i = 0; i < areaSize.Height; i++)
                    {
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

        private void SaveAsBmp(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".bmp";
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[areaSize.Width * _file.Header.BytesPerSample];
                    float[] floatFrameStrData = new float[areaSize.Width];

                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;


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

                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameStrData, 0, frameStrData.Length);
                        Buffer.BlockCopy(frameStrData, 0, floatFrameStrData, 0, frameStrData.Length);
                        fw.Write(floatFrameStrData.Select(x => (byte)(x * 255f / 87000f)).ToArray(), 0, floatFrameStrData.Length);
                        //fw.Write(padBytes, 0, padBytes.Length);
                        fw.Write(padBytes, 0, padBytes.Length);

                        fr.Seek(strDataLength - frameStrData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }

    }
}
