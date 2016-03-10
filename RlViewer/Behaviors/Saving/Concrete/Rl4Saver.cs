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
        public Rl4Saver(Files.LocatorFile loc)
            : base(loc)
        {
            _file = loc as RlViewer.Files.Rli.Concrete.Rl4;
            _head = loc.Header as RlViewer.Headers.Concrete.Rl4.Rl4Header;
        }

        private RlViewer.Files.Rli.Concrete.Rl4 _file;
        private RlViewer.Headers.Concrete.Rl4.Rl4Header _head;

        public override void Save(string path, FileType saveAsType, Point leftTop, Size areaSize)
        {
            switch (saveAsType)
            {
                case FileType.brl4:
                    SaveAsBrl4(path, leftTop, areaSize);
                    break;
                case FileType.raw:
                    break;
                case FileType.rl4:
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void SaveAsBrl4(string path, Point leftTop, Size areaSize)
        {
            using (var fr = System.IO.File.Open(_file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var fw = System.IO.File.OpenWrite(path))
                {
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);
                    var brl4Head = _head.HeaderStruct.ToBrl4(255, 255, 1);
                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader>(brl4Head),
                    0, Marshal.SizeOf(brl4Head));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    byte[] strData = new byte[_file.Width * _file.Header.BytesPerSample];

                    //var startOfAreaToSave = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                        //+ leftTop.X * _file.Header.BytesPerSample;

                    //fr.Seek(startOfAreaToSave, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {
                        //read-write string header
                        //fr.Read(strHeader, 0, strHeaderSize);
                        var sh = RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct>(fr);
                        var brl4StrHead = Converters.Converters.ToBrl4StrHeader(strHeader, 2, 2, 2);


                        fw.Write(brl4StrHead, 0, strHeaderSize);

                        //fr.Seek(leftTop.X * )
                        //read-write string data
                        fr.Read(strData, 0, strData.Length);
                        fw.Write(strData, 0, strData.Length);
                    }

                }

            }
        }



    }
}
