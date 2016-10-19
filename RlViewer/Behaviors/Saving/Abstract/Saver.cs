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
        protected abstract void SaveAndReport(string path, RlViewer.FileType destinationType, Rectangle area,
            float normalization, float maxValue, System.Drawing.Imaging.ColorPalette palette, Filters.ImageFilterProxy filter);

        public void Save(string path, RlViewer.FileType destinationType, Rectangle area,
            float normalization, float maxValue, System.Drawing.Imaging.ColorPalette palette, Filters.ImageFilterProxy filter)
        {
            OnReportName("Сохранение изображения");
            SaveAndReport(path, destinationType, area, normalization, maxValue, palette, filter);
        }


        public virtual void SaveAsAligned(string fileName, System.Drawing.Rectangle area, byte[] image)
        {
            SaveAsAligned(fileName, area, image, 0, 0, 0);
        }

        public abstract void SaveAsAligned(string fileName, System.Drawing.Rectangle area, byte[] image,
            int aligningPointsCount, int rangeCompressionCoef, int azimuthCompressionCoef);


        protected virtual void SaveAsRaw(string path, Rectangle area)
        {
            var fname = Path.ChangeExtension(path, ".raw");
            long sampleToStartSaving = (long)area.Y * (long)(SourceFile.Width * SourceFile.Header.BytesPerSample + SourceFile.Header.StrHeaderLength) + 
                area.X * SourceFile.Header.BytesPerSample;

            int strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;
            byte[] frameStrData = new byte[area.Width * SourceFile.Header.BytesPerSample];


            using (var fr = System.IO.File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var fw = System.IO.File.Open(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {

                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                    fr.Seek(sampleToStartSaving, SeekOrigin.Current);

                    for (int i = 0; i < area.Height; i++)
                    {

                        OnProgressReport((int)((double)i / (double)area.Height * 100));
                        if (OnCancelWorker())
                        {
                            return;
                        }

                        fr.Seek(SourceFile.Header.StrHeaderLength, SeekOrigin.Current);

                        fr.Read(frameStrData, 0, frameStrData.Length);
                        fw.Write(frameStrData, 0, frameStrData.Length);
                        fr.Seek(strDataLength - frameStrData.Length, SeekOrigin.Current);
                    }

                }

            }
        }


        protected void SaveAsBmp(string path, Rectangle area, float normalization, float maxValue,
            DataProcessor.Abstract.DataStringProcessor processor, System.Drawing.Imaging.ColorPalette palette = null, 
            Filters.ImageFilterProxy filter = null)
        {
            var destinationFileName = Path.ChangeExtension(path, ".bmp");
            var padding = (area.Width % 4);
            padding = padding == 0 ? 0 : 4 - padding;

            //since bitmap width has to be multiple of 4 at all times, we have to add padding bytes
            var padBytes = new byte[padding];
            int strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;
            byte[] frameStrData = new byte[area.Width * SourceFile.Header.BytesPerSample];
            float[] floatFrameStrData = new float[area.Width];

            var bmpHeader = GetBitmapFileHeader(area.Width, area.Height, palette);

            using (var fr = System.IO.File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var fw = System.IO.File.Open(destinationFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    long lineToStartSaving = (long)area.Y * (long)(SourceFile.Width * SourceFile.Header.BytesPerSample + SourceFile.Header.StrHeaderLength);
                    long sampleToStartSaving = area.X * SourceFile.Header.BytesPerSample;
                    long offset = lineToStartSaving + sampleToStartSaving;

                    fr.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                    fr.Seek(offset, SeekOrigin.Current);


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
                        fr.Read(frameStrData, 0, frameStrData.Length);

                        var processedData  = processor.ProcessDataString(frameStrData);

                        var bytes = processedData.Select(x => TileCreator.NormalizationHelpers.ToByteRange(
                            TileCreator.NormalizationHelpers.GetLinearValue(x, normalization))).ToArray();

                        if (filter != null)
                        {
                            bytes = filter.Filter.ApplyFilters(bytes);
                        }

                        fw.Write(bytes, 0, floatFrameStrData.Length);
                        fw.Write(padBytes, 0, padBytes.Length);
                        fr.Seek(strDataLength - frameStrData.Length, SeekOrigin.Current);
                    }
                }
            }
        }



        private byte[] GetBitmapFileHeader(int imgWidth, int imgHeight, System.Drawing.Imaging.ColorPalette colorPalette = null)
        {
            var rgbSize = Marshal.SizeOf(typeof(RGBQUAD));

            //height is < 0 since image is flipped
            var bmpInfoHeader = new BITMAPINFOHEADER(imgWidth, -imgHeight, (uint)(imgHeight * imgWidth));
            var headerSize = Marshal.SizeOf(bmpInfoHeader) + Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + rgbSize * 256;
            var bmpFileHeader = new BITMAPFILEHEADER((uint)(imgWidth * imgHeight + headerSize), (uint)headerSize);

            List<byte> palette = new List<byte>(256);

            //if palete is not provided
            if (colorPalette == null)
            {
                //fill palette with shades of gray
                for (int i = 0; i < 256; i++)
                {
                    palette.AddRange(RlViewer.Behaviors.Converters.StructIO.WriteStruct<RGBQUAD>(new RGBQUAD((byte)i, (byte)i, (byte)i)));
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    var color = colorPalette.Entries[i];
                    palette.AddRange(RlViewer.Behaviors.Converters.StructIO.WriteStruct<RGBQUAD>(new RGBQUAD((byte)color.R, (byte)color.G, (byte)color.B)));
                }
            }

            List<byte> bmpHeader = new List<byte>();
            bmpHeader.AddRange(RlViewer.Behaviors.Converters.StructIO.WriteStruct<BITMAPFILEHEADER>(bmpFileHeader));
            bmpHeader.AddRange(RlViewer.Behaviors.Converters.StructIO.WriteStruct<BITMAPINFOHEADER>(bmpInfoHeader));
            bmpHeader.AddRange(palette);

            return bmpHeader.ToArray();
        }

    }
}
