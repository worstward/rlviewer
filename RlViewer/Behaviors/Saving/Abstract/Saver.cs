using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace RlViewer.Behaviors.Saving.Abstract
{
    public abstract class Saver : WorkerEventController
    {
        public Saver(Files.LocatorFile file)
        {
            
        }
        public abstract Files.LocatorFile SourceFile { get; }

        public abstract void Save(string path, RlViewer.FileType destinationType, Rectangle area, Filters.ImageFilterFacade filter, float normalization, float maxValue);

        public abstract void SaveAsAligned(string fileName, System.Drawing.Rectangle area, byte[] image);

        protected virtual void SaveAsRaw(string path, Rectangle area)
        {
            using (var fr = System.IO.File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".raw");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;
                    byte[] frameStrData = new byte[area.Width * SourceFile.Header.BytesPerSample];


                    var lineToStartSaving = area.Y * (SourceFile.Width * SourceFile.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    var sampleToStartSaving = area.X * SourceFile.Header.BytesPerSample;

                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {

                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }

                        fr.Seek(SourceFile.Header.StrHeaderLength, SeekOrigin.Current);
                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameStrData, 0, frameStrData.Length);
                        fw.Write(frameStrData, 0, frameStrData.Length);
                        fr.Seek(strDataLength - frameStrData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }

                }

            }
        }


        private byte[] GetBitmapFileHeader(int imgWidth, int imgHeight)
        {
            var rgbSize = Marshal.SizeOf(new RGBQUAD());

            //height is < 0 since image is flipped
            var bmpInfoHeader = new BITMAPINFOHEADER(imgWidth, -imgHeight, (uint)(imgHeight * imgWidth));
            var headerSize = Marshal.SizeOf(bmpInfoHeader) + Marshal.SizeOf(new BITMAPFILEHEADER()) + rgbSize * 256;
            var bmpFileHeader = new BITMAPFILEHEADER((uint)(imgWidth * imgHeight + headerSize), (uint)headerSize);

            List<byte> palette = new List<byte>();
            //fill palette with shades of gray
            for (int i = 0; i < 256; i++)
            {
                palette.AddRange(RlViewer.Files.LocatorFile.WriteStruct<RGBQUAD>(new RGBQUAD((byte)i, (byte)i, (byte)i)));
            }

            List<byte> bmpHeader = new List<byte>();
            bmpHeader.AddRange(RlViewer.Files.LocatorFile.WriteStruct<BITMAPFILEHEADER>(bmpFileHeader));
            bmpHeader.AddRange(RlViewer.Files.LocatorFile.WriteStruct<BITMAPINFOHEADER>(bmpInfoHeader));
            bmpHeader.AddRange(palette);

            return bmpHeader.ToArray();
        }

        protected virtual void SaveAsBmp(string path, Rectangle area, float normalization, float maxValue, Filters.ImageFilterFacade filter = null)
        {
            using (var fr = System.IO.File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fname = Path.ChangeExtension(path, ".bmp");
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    int strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;
                    byte[] frameStrData = new byte[area.Width * SourceFile.Header.BytesPerSample];
                    float[] floatFrameStrData = new float[area.Width];


                    var lineToStartSaving = area.Y * (SourceFile.Width * SourceFile.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    var sampleToStartSaving = area.X * SourceFile.Header.BytesPerSample;

                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                    fr.Seek(lineToStartSaving, SeekOrigin.Current);

                    //since bitmap width has to be multiple of 4 at all times, we have to add padding bytes
                    var padBytes = new byte[(int)(Math.Ceiling((double)(area.Width / 4f))) * 4 - area.Width];

                    var bmpHeader = GetBitmapFileHeader(area.Width, area.Height);

                    fw.Write(bmpHeader, 0, bmpHeader.Length);

                    for (int i = 0; i < area.Height; i++)
                    {

                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            fw.Flush();
                            return;
                        }

                        fr.Seek(SourceFile.Header.StrHeaderLength, SeekOrigin.Current);
                        fr.Seek(sampleToStartSaving, SeekOrigin.Current);
                        fr.Read(frameStrData, 0, frameStrData.Length);
                        
                        Buffer.BlockCopy(frameStrData, 0, floatFrameStrData, 0, frameStrData.Length);

                        var bytes = floatFrameStrData.Select(x => TileCreator.NormalizationHelpers.ToByteRange(
                            TileCreator.NormalizationHelpers.GetLinearLogarithmicValue(x, normalization / 9f * 7, maxValue, normalization))).ToArray();

                        if (filter != null)
                        {
                            bytes = filter.Filter.ApplyFilters(bytes);
                        }

                        fw.Write(bytes, 0, floatFrameStrData.Length);
                        fw.Write(padBytes, 0, padBytes.Length);
                        fr.Seek(strDataLength - frameStrData.Length - sampleToStartSaving, SeekOrigin.Current);
                    }
                }
            }
        }

    }
}
