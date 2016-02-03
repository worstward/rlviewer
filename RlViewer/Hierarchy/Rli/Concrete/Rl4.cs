using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Hierarchy.Rli.Abstract;

namespace RlViewer.Hierarchy.Rli.Concrete
{
    public class Rl4 : RliFile
    {
        public Rl4(FileProperties properties) : base(properties)
        { }

        private readonly int FILE_HEADER_LENGTH = 16384;
        private FileHeader _header;

        public override byte[] ReadFileHeader()
        {
            throw new NotImplementedException();
        }

        public override FileHeader Header
        {
            get { return _header; }
        }


        //private async Task ReadFileHeaderAsync()
        //{

        //    using (var fs = System.IO.File.OpenRead(Properties.FilePath))
        //    {
        //        byte[] array = new byte[HeaderLength];
        //        await fs.ReadAsync(array, 0, array.Length);
        //    }
        //}

        public override bool CheckFile()
        {
            return false;
        }

    }
}
