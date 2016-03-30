using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RlViewer.Behaviors
{
    public class PointReader
    {

        //TODO: provide complex samples support (2 float = 8 bytes)
        protected float GetValue(RlViewer.Files.LocatorFile file, Point p)
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
