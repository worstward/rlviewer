using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Files
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
            FileType type;
            try
            {
                //get file extension without dot
                type = System.IO.Path.GetExtension(path).Substring(1).ToEnum<FileType>();
            }
            catch (ArgumentException aex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error,
                    string.Format("Attempt to open unsupported file type {0}  {1}", Environment.NewLine, aex.Message));
                throw aex;
            }

            return type;        
        }   


    }
}
