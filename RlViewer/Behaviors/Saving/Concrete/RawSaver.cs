using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

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



    }
}
