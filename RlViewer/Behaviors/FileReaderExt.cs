using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RlViewer.Behaviors
{
    public static class FileReaderExt
    {
        public static byte[] GetArea(this RlViewer.Files.LocatorFile file, Rectangle areaBorders)
        {

            FileStream fs = null;
            try
            {
                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                byte[] area = new byte[file.Header.BytesPerSample * areaBorders.Width * areaBorders.Height];
                int width = file.Width < areaBorders.Width ? file.Width : areaBorders.Width;
                int height = file.Height < areaBorders.Height ? file.Height : areaBorders.Height;

                int areaLineLength = file.Header.BytesPerSample * width;

                int headersOffset = file.Header.FileHeaderLength;
                int xOffset = ((file.Width - areaBorders.X) < 0 ? 0 : areaBorders.X) * file.Header.BytesPerSample;
                int yOffset = ((file.Height - areaBorders.Y) < 0 ? 0 : areaBorders.Y) * 
                    (file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength);

                fs.Seek(headersOffset, SeekOrigin.Current);
                fs.Seek(yOffset, SeekOrigin.Current);
                for (int i = 0; i < height; i++)
                {
                    fs.Seek(xOffset + file.Header.StrHeaderLength, SeekOrigin.Current);
                    fs.Read(area, i * areaBorders.Width * file.Header.BytesPerSample, areaLineLength);
                    fs.Seek(file.Width * file.Header.BytesPerSample - xOffset - areaLineLength,
                        SeekOrigin.Current);
                }

                return area;
            }
            catch (FileNotFoundException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("Analyzed file not found: {0}", file.Properties.FilePath));
                throw;
            }
            catch (IOException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("Can't open file for analyzation: {0}", file.Properties.FilePath));
                throw;
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("General error occured while analyzing file: {0}", file.Properties.FilePath));
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
        }


        public static short ToShortSample(this byte[] sampleBytes, int sampleSize)
        {
            return BitConverter.ToInt16(sampleBytes, 0);
        }

        public static short[] ToShortArea(this byte[] areaBytes, int sampleSize)
        {
            var sampleShorts = new short[areaBytes.Length / sampleSize];
            Buffer.BlockCopy(areaBytes, 0, sampleShorts, 0, areaBytes.Length);
            return sampleShorts;
        }


        public static float ToFloatSample(this byte[] sampleBytes, int sampleSize)
        {
            switch (sampleSize)
            {
                case 4:
                    {
                        return BitConverter.ToSingle(sampleBytes, 0);
                    }
                case 8:
                    {
                        var re = BitConverter.ToSingle(sampleBytes, 0);
                        var im = BitConverter.ToSingle(sampleBytes, sizeof(float));

                        return (float)(Math.Sqrt(re * re + im * im));
                    }
                default:
                    throw new NotSupportedException("Unsupported sample size");
            }
        }
        

        public static float[] ToFloatArea(this byte[] areaBytes, int sampleSize)
        {
            switch (sampleSize)
            {
                case 4:
                    {
                        var sampleFloats = new float[areaBytes.Length / sampleSize];
                        Buffer.BlockCopy(areaBytes, 0, sampleFloats, 0, areaBytes.Length);
                        return sampleFloats;
                    }
                case 8:
                    {
                        var sampleFloats = new float[areaBytes.Length / sizeof(float)];
                        var complexFloats = new float[areaBytes.Length / sampleSize];

                        Buffer.BlockCopy(areaBytes, 0, sampleFloats, 0, areaBytes.Length);

                        for (int i = 0; i < sampleFloats.Length; i += 2)
                        {
                            var re = sampleFloats[i];
                            var im = sampleFloats[i + 1];
                            complexFloats[i / 2] = (float)(Math.Sqrt(re * re + im * im));
                        }

                        return complexFloats;
                    }
                default:
                    throw new NotSupportedException("Unsupported sample size");
            }
        }

        public static Point GetMaxSampleLocation(this RlViewer.Files.LocatorFile file, Rectangle area)
        {
            Point p = area.Location;

            if (file.Header.BytesPerSample == 2)
            {
                short[] sample = file.GetArea(area).ToShortArea(file.Header.BytesPerSample);
                short max = sample.Max();
                var maxIndex = sample.Select((v, i) => new { Index = i, Value = v})
                                        .Where(v => v.Value == max)
                                        .First()
                                        .Index;
                var y = maxIndex / area.Width;
                var x = maxIndex - y * area.Width;

                return new Point(area.Location.X + x, area.Location.Y + y);
            }
            else if (file.Header.BytesPerSample == 4 || file.Header.BytesPerSample == 8)
            {
                float[] sample = file.GetArea(area).ToFloatArea(file.Header.BytesPerSample);
                float max = sample.Max();
                var maxIndex = sample.Select((v, i) => new { Index = i, Value = v })
                                        .Where(v => v.Value == max)
                                        .First()
                                        .Index;
                var y = maxIndex / area.Width;
                var x = maxIndex - y * area.Width;


                return new Point(area.Location.X + x, area.Location.Y + y);
            }

            throw new NotSupportedException("Unsupported sample size");   
        }

        public static byte[] GetSample(this RlViewer.Files.LocatorFile file, Point p)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] sampleBytes = new byte[file.Header.BytesPerSample];

                int headersOffset = file.Header.FileHeaderLength;
                int xOffset = p.X * file.Header.BytesPerSample;
                int yOffset = p.Y * (file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength) + file.Header.StrHeaderLength;

                int offset = headersOffset + xOffset + yOffset;
                
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(sampleBytes, 0, sampleBytes.Length);

                return sampleBytes;
            }
            catch (FileNotFoundException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("Analyzed file not found: {0}", file.Properties.FilePath));
                throw;
            }
            catch (IOException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("Can't open file for analyzation: {0}", file.Properties.FilePath));
                throw;
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("General error occured while analyzing file: {0}", file.Properties.FilePath));
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
        }
    }
}
