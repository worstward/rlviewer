using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Behaviors.FilesAggregator
{
    public class LocatorFilesAggregator : WorkerEventController
    {
        public void Aggregate(string aggregatedFile, params string[] sourceFiles)
        {
            OnReportName("Совмещение файлов");
            CreateAggregatedFile(aggregatedFile, sourceFiles);
        }


        //private void CreateAggregatedFile(string aggregatedFile, params string[] sourceFilesNames)
        //{
        //    using (var aggregateStream = File.Open(aggregatedFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
        //    {

        //        List<Files.LocatorFile> sourceFiles = new List<Files.LocatorFile>(sourceFilesNames.Length);

        //        foreach (var filePath in sourceFilesNames.OrderBy(x => x))
        //        {
        //            var fileProp = new Files.FileProperties(filePath);
        //            var header = Factories.Header.Abstract.HeaderFactory.GetFactory(fileProp).Create(filePath);
        //            var file = Factories.File.Abstract.FileFactory.GetFactory(fileProp).Create(fileProp, header, null);

        //            sourceFiles.Add(file);
        //        }

        //        int index = 0;

        //        foreach (var file in sourceFiles)
        //        {
        //            using (var sourceStream = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //            {

        //                byte[] dataString = new byte[file.Width * file.Header.BytesPerSample + file.Header.FileHeaderLength];

        //                while (sourceStream.Position != sourceStream.Length)
        //                {
        //                    sourceStream.CopyTo(aggregateStream);
        //                    OnProgressReport((int)(++index / (float)sourceFiles.Count * 100));
        //                    if (OnCancelWorker())
        //                    {
        //                        return;
        //                    }
        //                }

        //            }
        //        }

        //    }

        //}

        private void CreateAggregatedFile(string aggregatedFile, params string[] sourceFilesNames)
        {
            using (var aggregateStream = File.Open(aggregatedFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {

                List<Files.LocatorFile> sourceFiles = new List<Files.LocatorFile>(sourceFilesNames.Length);

                foreach (var filePath in sourceFilesNames)
                {
                    var fileProp = new Files.FileProperties(filePath);
                    var header = Factories.Header.Abstract.HeaderFactory.GetFactory(fileProp).Create(filePath);
                    var file = Factories.File.Abstract.FileFactory.GetFactory(fileProp).Create(fileProp, header, null);

                    sourceFiles.Add(file);
                }

                float totalLines = sourceFiles.Select(x => x.Height).Sum();
                int index = 0;


                foreach (var file in sourceFiles)
                {
                    using (var sourceStream = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {

                        byte[] dataString = new byte[file.Width * file.Header.BytesPerSample + file.Header.FileHeaderLength];

                        while (sourceStream.Position != sourceStream.Length)
                        {
                            sourceStream.Read(dataString, 0, dataString.Length);
                            aggregateStream.Write(dataString, 0, dataString.Length);


                            OnProgressReport((int)(index / totalLines * 100));
                            if (OnCancelWorker())
                            {
                                return;
                            }
                            index++;
                        }

                    }
                }

            }

        }


    }
}
