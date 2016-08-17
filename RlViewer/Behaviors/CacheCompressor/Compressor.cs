using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;

namespace RlViewer.Behaviors.CacheCompressor
{
    public static class Compressor
    {
        public static void Compress(string directoryName)
        {
            if (Directory.Exists(directoryName))
            {
                try
                {
                    using (var zip = new ZipFile())
                    {
                        zip.AddDirectory(directoryName);
                        zip.SaveProgress += (s, e) =>
                            {
                                var filePercentage = e.EntriesSaved / e.EntriesTotal * 100;
                            };
                        zip.Save(Path.ChangeExtension(directoryName, "zip"));
                    }
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("Archive creation failed: {0}", ex.Message));
                }
            }
        }

        public static void Extract(string directoryName)
        {
            if (Directory.Exists(directoryName))
            {
                try
                {
                    var archiveNameNoExtension = Path.Combine(directoryName, new DirectoryInfo(directoryName).Name);
                    var archiveName = Path.Combine(archiveNameNoExtension, "zip");

                    using (var zip = new ZipFile(archiveName))
                    {
                        zip.ExtractProgress += (s, e) =>
                            {
                                var filePercentage = e.EntriesExtracted / e.EntriesTotal * 100;
                            };

                        zip.ExtractAll(directoryName);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("Archive extraction failed: {0}", ex.Message));
                }
            }
        }


    }
}
