using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Files
{
    /// <summary>
    /// Incapsulates common properties of supported files
    /// </summary>
    public class FileProperties
    {
        public FileProperties(string path)
        {
            FilePath = path;
            Type = GetType(path);
        }


        /// <summary>
        /// Path to file
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// <see cref="FileType"/>  of this file
        /// </summary>
        public FileType Type { get; private set; }


        /// <summary>
        /// Gets this file <see cref="FileType"/> from a provided filename
        /// </summary>
        /// <param name="path">Filename</param>
        /// <returns></returns>
        private FileType GetType(string path)
        {
           return System.IO.Path.GetExtension(path).Replace(".", "").ToEnum<FileType>();     
        }   


    }
}
