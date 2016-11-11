using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using Microsoft.VisualBasic.Devices;

namespace RlViewer.Behaviors.Synthesis.SharedMemory
{
    public class MappedFile : IDisposable
    {
        public MappedFile(string fileShareName, long fileSize)
        {
            //var memSize = ((long)new ComputerInfo().TotalPhysicalMemory / 2);

            _fileShareName = fileShareName;

            OpenFileMapping(fileShareName, fileSize);
        }

        private string _fileShareName;
        private MemoryMappedFile _mmf;
        private MemoryMappedViewStream _mmfStream;

        private void OpenFileMapping(string name, long size)
        {
            _mmf = MemoryMappedFile.CreateNew(name, size);
            _mmfStream = _mmf.CreateViewStream();
        }

        public void WriteData(byte[] dataToWrite)
        {
            _mmfStream.Write(dataToWrite, 0, dataToWrite.Length);
        }

        public async Task<byte[]> ReadData(int bytesToRead)
        {
            var data = new byte[bytesToRead];
            await _mmfStream.ReadAsync(data, 0, bytesToRead);
            return data;
        }


        public void Dispose()
        {
            if (_mmf != null)
            {
                _mmf.Dispose();
            }
        }

    }
}
