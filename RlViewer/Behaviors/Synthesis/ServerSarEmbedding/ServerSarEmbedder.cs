using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.IO.Compression;

namespace RlViewer.Behaviors.Synthesis.ServerSarEmbedding
{
    public class ServerSarEmbeddingProcessor : IDisposable
    {
        public ServerSarEmbeddingProcessor(string serverSarFileName, string localFolderPath)
        {
            _serverSarFileName = serverSarFileName;
            _localFolder = localFolderPath;
        }

        private string _serverSarFileName;
        private string _localFolder;


        public string RebuildFromResources()
        {
            if (!Directory.Exists(_localFolder))
            {
                Directory.CreateDirectory(_localFolder);
                var serverSarArchive = Resources.server_sar;
                var serverSarArchivePath = Path.Combine(_localFolder, Path.ChangeExtension(GetNameOf(() => Resources.server_sar), "zip"));
                File.WriteAllBytes(serverSarArchivePath, serverSarArchive);

                ZipFile.ExtractToDirectory(serverSarArchivePath, Path.GetDirectoryName(serverSarArchivePath));
                File.Delete(serverSarArchivePath);
            }

            var serverSarName = Path.GetFileName(_serverSarFileName);
            var serverSarPath = Path.Combine(_localFolder, serverSarName);

            return serverSarPath;
        }

        private string GetNameOf<T>(Expression<Func<T>> property)
        {
            return (property.Body as MemberExpression).Member.Name;
        }


        public void Dispose()
        {
            if (Directory.Exists(_localFolder))
            {
                foreach (var file in Directory.GetFiles(_localFolder))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                Directory.Delete(_localFolder);
            }


        }
    }
}
