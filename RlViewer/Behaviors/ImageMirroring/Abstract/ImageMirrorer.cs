using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Behaviors.ImageMirroring.Abstract
{
    public abstract class ImageMirrorer : WorkerEventController
    {
        public ImageMirrorer(Files.LocatorFile sourceFile)
        {

        }

        protected abstract Files.LocatorFile SourceFile
        {
            get;
        }

        protected abstract byte[] InvertFlipType();


        public void MirrorImage(string destinationImagePath)
        {
            OnNameReport("Отражение изображения");
            Flip(destinationImagePath);
        }


        private void Flip(string destinationImagePath)
        {
            destinationImagePath = Path.ChangeExtension(destinationImagePath, Path.GetExtension(SourceFile.Properties.FilePath));

            using (var sourceStream = File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var destinationStream = File.Open(destinationImagePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    var headerBytes = new byte[SourceFile.Header.FileHeaderLength];
                    sourceStream.Read(headerBytes, 0, headerBytes.Length);
                    destinationStream.Write(InvertFlipType(), 0, headerBytes.Length);


                    var strHeader = new byte[SourceFile.Header.StrHeaderLength];
                    var strData = new byte[SourceFile.Width * SourceFile.Header.BytesPerSample];
                    for (int i = 0; i < SourceFile.Height; i++)
                    {
                        sourceStream.Read(strHeader, 0, strHeader.Length);
                        destinationStream.Write(strHeader, 0, strHeader.Length);

                        sourceStream.Read(strData, 0, strData.Length);
                        var reversedStrData = new byte[strData.Length];

                        for (int j = 0; j < strData.Length; j++)
                        {
                            var index = strData.Length - j + (2 * (j % SourceFile.Header.BytesPerSample) - (SourceFile.Header.BytesPerSample - 1)) - 1;

                            reversedStrData[index] = strData[j];
                        }

                        destinationStream.Write(reversedStrData, 0, reversedStrData.Length);


                        OnProgressReport((int)(i / (float)SourceFile.Height * 100));
                        if (OnCancelWorker())
                        {
                            break;
                        }

                    }
                }
            }
        }


    }
}
