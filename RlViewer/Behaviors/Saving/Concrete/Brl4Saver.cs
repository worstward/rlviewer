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

        public override void Save(string path, RlViewer.FileType destinationType, Point leftTop, Size areaSize)
        {
            switch (destinationType)
            {
                case FileType.brl4:
                    break;
                case FileType.raw:
                    SaveAsRaw(path, leftTop, areaSize);
                    break;
                case FileType.rl4:
                    SaveAsRl4(path, leftTop, areaSize);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void SaveAsRl4(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".rl4";
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);
                    var rl4Head = _head.HeaderStruct.ToRl4();

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Head),
                    0, Marshal.SizeOf(rl4Head));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameData = new byte[areaSize.Width * _file.Header.BytesPerSample];

                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;
                        //+ leftTop.X * _file.Header.BytesPerSample;

                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {
                        //read-write string header
                        fr.Read(strHeader, 0, strHeaderSize);
                        var rl4StrHead = Converters.Converters.ToRl4StrHeader(strHeader, 100, 100, 100);
                        fw.Write(rl4StrHead, 0, strHeaderSize);


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
                var fname = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".brl4";
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);

                    var rlSubHeader = _head.HeaderStruct.ChangeImgDimensions(areaSize.Width, areaSize.Height);

                    RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader rl4Header =
                        new Headers.Concrete.Brl4.Brl4RliFileHeader(_head.HeaderStruct.fileSign, _head.HeaderStruct.fileVersion,
                            _head.HeaderStruct.rhgParams, rlSubHeader, _head.HeaderStruct.synthParams, _head.HeaderStruct.reserved);

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader>(rl4Header),
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



        private void SaveAsRaw(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".raw";
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = _file.Width * _file.Header.BytesPerSample;
                    byte[] frameStrData = new byte[areaSize.Width * _file.Header.BytesPerSample];

                    var fileHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader());
                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());


                    var lineToStartSaving = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                    var sampleToStartSaving = leftTop.X * _file.Header.BytesPerSample;

                    fr.Seek(fileHeaderSize, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {

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






    }
}
