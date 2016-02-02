using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Hierarchy
{
    public class FileProperties
    {
        public FileProperties(string path)
        {
            FilePath = path;
            Type = GetType(path);
        }

        public string FilePath { get; private set; }

        public FileType Type { get; private set; }

        private FileType GetType(string path)
        {
            return System.IO.Path.GetExtension(path).ToEnum<FileType>();
        }

    }
}
