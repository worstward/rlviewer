using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using Microsoft.VisualBasic.Devices;

namespace RlViewer.Behaviors.Synthesis.SharedMemory
{
    public class MappedFile
    {
        public MappedFile(string fileShareName)
        {
            _fileShareName = fileShareName;
        }

        private string _fileShareName;

        public void OpenFileMapping()
        {
            var memSize = ((long)new ComputerInfo().TotalPhysicalMemory / 2);
            using (var memoryMappedFile = MemoryMappedFile.CreateNew(_fileShareName, memSize))
            {
                using (var mappedFileStream = memoryMappedFile.CreateViewStream())
                {
                    
                }
            }
        }
    }
}
