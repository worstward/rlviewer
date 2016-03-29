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
        protected float GetValue(RlViewer.Files.LocatorFile file, Point p)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                //TODO: provide complex samples support (2 float = 8 bytes)
                var offset = p.X * file.Header.BytesPerSample +
                    p.Y * (file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength) + file.Header.FileHeaderLength + file.Header.StrHeaderLength;
                fs.Seek(offset, SeekOrigin.Begin);
                var floatValue = new byte[file.Header.BytesPerSample];
                fs.Read(floatValue, 0, floatValue.Length);
                return BitConverter.ToSingle(floatValue, 0);
            }
            catch (FileNotFoundException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Analyzed file not found: {0}", file.Properties.FilePath));
                throw;
            }
            catch (IOException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Can't open file for analyzation: {0}", file.Properties.FilePath));
                throw;
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("General error occured while analyzing file: {0}", file.Properties.FilePath));
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
