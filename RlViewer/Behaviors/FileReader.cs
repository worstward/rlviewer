using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RlViewer.Behaviors
{
    public static class FileReader
    {

        public static float[] GetArea(RlViewer.Files.LocatorFile file, Rectangle areaBorders)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                byte[] area = new byte[file.Header.BytesPerSample * areaBorders.Width * areaBorders.Height];
                float[] floatArea = new float[areaBorders.Width * areaBorders.Height];
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

                Buffer.BlockCopy(area, 0, floatArea, 0, area.Length);

                return floatArea;
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



        //TODO: provide complex samples support (2 float = 8 bytes)
        public static float GetSample(RlViewer.Files.LocatorFile file, Point p)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] amplitudeBytes = new byte[file.Header.BytesPerSample];

                int headersOffset = file.Header.FileHeaderLength;
                int xOffset = p.X * file.Header.BytesPerSample;
                int yOffset = p.Y * (file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength) + file.Header.StrHeaderLength;

                int offset = headersOffset + xOffset + yOffset;
                
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(amplitudeBytes, 0, amplitudeBytes.Length);
                float pointAmplitude = BitConverter.ToSingle(amplitudeBytes, 0);
                
                return pointAmplitude;
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
