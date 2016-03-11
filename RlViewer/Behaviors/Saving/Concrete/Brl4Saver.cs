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

        public override void Save(string path, FileType saveAsType, Point leftTop, Size areaSize)
        {
            switch (saveAsType)
            {
                case FileType.brl4:
                    break;
                case FileType.raw:
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
                using (var fw = System.IO.File.OpenWrite(path))
                {
                    fr.Seek(Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader()), SeekOrigin.Begin);
                    var rl4Head = _head.HeaderStruct.ToRl4();

                    fw.Write(RlViewer.Files.LocatorFile.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Head),
                    0, Marshal.SizeOf(rl4Head));

                    var strHeaderSize = Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                    byte[] strHeader = new byte[strHeaderSize];

                    byte[] strData = new byte[_file.Width * _file.Header.BytesPerSample];

                    var startOfAreaToSave = leftTop.Y * (_file.Width * _file.Header.BytesPerSample + strHeaderSize);
                        //+ leftTop.X * _file.Header.BytesPerSample;

                    fr.Seek(startOfAreaToSave, SeekOrigin.Current);

                    for (int i = 0; i < areaSize.Height; i++)
                    {
                        //read-write string header
                        fr.Read(strHeader, 0, strHeaderSize);
                        var rl4StrHead = Converters.Converters.ToRl4StrHeader(strHeader, 100, 100, 100);
                        fw.Write(rl4StrHead, 0, strHeaderSize);

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
