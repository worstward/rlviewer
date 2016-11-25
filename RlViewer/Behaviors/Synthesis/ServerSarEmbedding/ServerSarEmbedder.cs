using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

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

        private string _binToRliName;
        private string _serverSarDllName;
        private string _serverSarExeName;



        public string RebuildFromResources()
        {
            var binToRli = Resources.bintorli_dll_base_x64;
            var serverSarDll = Resources.server_sar_base_dll_x64;
            var serverSarExe = Resources.server_sar_base_tcp_x64;

            _binToRliName = Path.Combine(_localFolder, Path.ChangeExtension(GetNameOf(() => Resources.bintorli_dll_base_x64), "dll"));
            _serverSarDllName = Path.Combine(_localFolder, Path.ChangeExtension(GetNameOf(() => Resources.server_sar_base_dll_x64), "dll"));
            _serverSarExeName = Path.Combine(_localFolder, Path.GetFileName(_serverSarFileName));

            if(!Directory.Exists(_localFolder))
            {
                Directory.CreateDirectory(_localFolder);
            }

            if (!File.Exists(_binToRliName))
            {
                File.WriteAllBytes(_binToRliName, binToRli);
            }

            if (!File.Exists(_serverSarDllName))
            {
                File.WriteAllBytes(_serverSarDllName, serverSarDll);
            }

            if (!File.Exists(_serverSarExeName))
            {
                File.WriteAllBytes(_serverSarExeName, serverSarExe);
            }

            return _serverSarExeName;
        }

        private string GetNameOf<T>(Expression<Func<T>> property)
        {
            return (property.Body as MemberExpression).Member.Name;
        }


        public void Dispose()
        {
            if (File.Exists(_binToRliName))
            {
                File.Delete(_binToRliName);
            }

            if (File.Exists(_serverSarDllName))
            {
                File.Delete(_serverSarDllName);
            }

            if (File.Exists(_serverSarExeName))
            {
                File.Delete(_serverSarExeName);
            }
        }
    }
}
