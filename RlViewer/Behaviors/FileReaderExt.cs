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
                var areaLocX = areaBorders.X < 0 ? 0 : areaBorders.X;
                var areaLocY = areaBorders.Y < 0 ? 0 : areaBorders.Y;
                areaLocX = areaLocX > file.Width ? 0 : areaLocX;
                areaLocY = areaLocY > file.Height ? 0 : areaLocY;
                var areaWidth = areaBorders.X + areaBorders.Width > file.Width ? file.Width - areaBorders.X : areaBorders.Width;
                var areaHeight = areaBorders.Y + areaBorders.Height > file.Height ? file.Height - areaBorders.Y : areaBorders.Height;

                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                byte[] area = new byte[file.Header.BytesPerSample * areaWidth * areaHeight];
                int width = file.Width < areaWidth ? file.Width : areaWidth;
                int height = file.Height < areaHeight ? file.Height : areaHeight;

                int areaLineLength = file.Header.BytesPerSample * width;

                int headersOffset = file.Header.FileHeaderLength;
                long xOffset = ((file.Width - areaLocX) < 0 ? 0 : areaLocX) * file.Header.BytesPerSample;
                long yOffset = (long)((file.Height - areaLocY) < 0 ? 0 : areaLocY) * 
                    (long)(file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength);

                fs.Seek(headersOffset, SeekOrigin.Current);
                fs.Seek(yOffset, SeekOrigin.Current);
                for (int i = 0; i < height; i++)
                {
                    fs.Seek(xOffset + file.Header.StrHeaderLength, SeekOrigin.Current);
                    fs.Read(area, i * areaWidth * file.Header.BytesPerSample, areaLineLength);
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

        public static float ToFloatSample(this byte[] sampleBytes, int sampleSize)
        {
            switch (sampleSize)
            {
                case 4:
                    return sampleBytes.ToFloatSample();
                case 8:
                    return sampleBytes.ToFloatSampleModulus();

                default:
                    throw new ArgumentException("sampleSize");
            }
        }


        public static float ToFileSample(this byte[] sample, FileType type, int sampleSize)
        {
            switch (type)
            {
                case FileType.brl4:
                case FileType.rl4:
                case FileType.r:
                    return ToFloatSample(sample, sampleSize);               
                case FileType.rl8:
                    return ToFloatSampleModulus(sample);
                case FileType.raw:
                    if (sampleSize == 4)
                    {
                        return ToFloatSample(sample, sampleSize);
                    }
                    else if(sampleSize == 8)
                    {
                        return ToFloatSampleModulus(sample);
                    }
                    break;
                case FileType.k:
                    return ToShortSampleModulus(sample);
            }


            throw new NotSupportedException("file type");
        }



        private static float ToFloatSampleModulus(this byte[] sampleBytes)
        {
            var re = BitConverter.ToSingle(sampleBytes, 0);
            var im = BitConverter.ToSingle(sampleBytes, sizeof(float));
            return (float)(Math.Sqrt(re * re + im * im));
        }
        private static float ToFloatSample(this byte[] sampleBytes)
        {
            return BitConverter.ToSingle(sampleBytes, 0);
        }



        public static short ToShortSample(this byte[] sampleBytes, int sampleSize)
        {
            switch (sampleSize)
            {
                case 2:
                    return sampleBytes.ToShortSample();
                case 4:
                    return sampleBytes.ToShortSampleModulus();

                default:
                    throw new ArgumentException("sampleSize");
            }
        }

        private static short ToShortSampleModulus(this byte[] sampleBytes)
        {
            var re = BitConverter.ToInt16(sampleBytes, 0);
            var im = BitConverter.ToInt16(sampleBytes, sizeof(short));
            return (short)(Math.Sqrt(re * re + im * im));
        }
        private static short ToShortSample(this byte[] sampleBytes)
        {
            return BitConverter.ToInt16(sampleBytes, 0);
        }

        public static T[] ToArea<T>(this byte[] areaBytes, int sampleSize)
        {
            if (sampleSize == 8)
            {
                var complexFloats = new float[areaBytes.Length / sampleSize];
                var sampleFloats = new float[areaBytes.Length / sizeof(float)];
                        
                Buffer.BlockCopy(areaBytes, 0, sampleFloats, 0, areaBytes.Length);

                for (int i = 0; i < sampleFloats.Length; i += 2)
                {
                    var re = sampleFloats[i];
                    var im = sampleFloats[i + 1];
                    complexFloats[i / 2] = (float)(Math.Sqrt(re * re + im * im));
                }
                return (T[])(object)(complexFloats);
            }
            else if (typeof(T) == typeof(short))
            {
                var complexShorts = new short[areaBytes.Length / sampleSize];
                var sampleShorts = new short[areaBytes.Length / sizeof(short)];

                Buffer.BlockCopy(areaBytes, 0, sampleShorts, 0, areaBytes.Length);

                for (int i = 0; i < sampleShorts.Length; i += 2)
                {
                    var re = sampleShorts[i];
                    var im = sampleShorts[i + 1];
                    complexShorts[i / 2] = (short)(Math.Sqrt(re * re + im * im));
                }
                return (T[])(object)(complexShorts);
            }

            var samples = new T[areaBytes.Length / sampleSize];
            Buffer.BlockCopy(areaBytes, 0, samples, 0, areaBytes.Length);
            return samples;
        }



        public static float GetMaxSample(this RlViewer.Files.LocatorFile file, Rectangle area)
        {
            Point p = area.Location;

            if (file.Header.BytesPerSample == 2)
            {
                short[] sample = file.GetArea(area).ToArea<short>(file.Header.BytesPerSample);
                return (float)sample.Max();
            }
            else if (file.Header.BytesPerSample == 4 || file.Header.BytesPerSample == 8)
            {
                float[] sample = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);
                return sample.Max();
            }

            throw new NotSupportedException("Unsupported sample size");
        }

        public static float GetMinSample(this RlViewer.Files.LocatorFile file, Rectangle area)
        {
            Point p = area.Location;

            if (file.Header.BytesPerSample == 2)
            {
                short[] sample = file.GetArea(area).ToArea<short>(file.Header.BytesPerSample);
                return (float)sample.Min();
            }
            else if (file.Header.BytesPerSample == 4 || file.Header.BytesPerSample == 8)
            {
                float[] sample = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);
                return sample.Min();
            }

            throw new NotSupportedException("Unsupported sample size");
        }

        public static float GetAvgSample(this RlViewer.Files.LocatorFile file, Rectangle area)
        {
            Point p = area.Location;

            if (file.Header.BytesPerSample == 2)
            {
                short[] sample = file.GetArea(area).ToArea<short>(file.Header.BytesPerSample);
                return (float)sample.Average(x => x);
            }
            else if (file.Header.BytesPerSample == 4 || file.Header.BytesPerSample == 8)
            {
                float[] sample = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);
                return sample.Average(x => x);
            }

            throw new NotSupportedException("Unsupported sample size");
        }



        public static Point GetMinSampleLocation(this RlViewer.Files.LocatorFile file, Rectangle area)
        {
            Point p = area.Location;

            if (file.Header.BytesPerSample == 2)
            {
                short[] sample = file.GetArea(area).ToArea<short>(file.Header.BytesPerSample);
                short min = sample.Min();
                var minIndex = sample.Select((v, i) => new { Index = i, Value = v })
                                        .Where(v => v.Value == min)
                                        .First()
                                        .Index;
                var y = minIndex / area.Width;
                var x = minIndex - y * area.Width;

                return new Point(area.Location.X + x, area.Location.Y + y);
            }
            else if (file.Header.BytesPerSample == 4 || file.Header.BytesPerSample == 8)
            {
                float[] sample = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);
                float min = sample.Min();
                var minIndex = sample.Select((v, i) => new { Index = i, Value = v })
                                        .Where(v => v.Value == min)
                                        .First()
                                        .Index;
                var y = minIndex / area.Width;
                var x = minIndex - y * area.Width;


                return new Point(area.Location.X + x, area.Location.Y + y);
            }

            throw new NotSupportedException("Unsupported sample size");
        }
       

        public static Point GetMaxSampleLocation(this RlViewer.Files.LocatorFile file, Rectangle area)
        {
            Point p = area.Location;

            if (file.Header.BytesPerSample == 2)
            {
                short[] sample = file.GetArea(area).ToArea<short>(file.Header.BytesPerSample);
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
                float[] sample = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);
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
            if (p.X >= file.Width || p.Y >= file.Height)
            {
                return new byte[file.Header.BytesPerSample];
            }

            FileStream fs = null;
            try
            {
                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] sampleBytes = new byte[file.Header.BytesPerSample];

                int headersOffset = file.Header.FileHeaderLength;
                long xOffset = p.X * file.Header.BytesPerSample;
                long yOffset = ((long)p.Y * (long)(file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength)) + file.Header.StrHeaderLength;

                long offset = headersOffset + xOffset + yOffset;
                
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(sampleBytes, 0, sampleBytes.Length);

                return sampleBytes;
            }
            catch (FileNotFoundException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                    string.Format("Analyzing file not found: {0}", file.Properties.FilePath));
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
