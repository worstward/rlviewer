using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Hierarchy.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;


namespace RlViewer.Hierarchy.Rli.Concrete
{
    public class Rl4 : RliFile
    {
        public Rl4(FileProperties properties) : base(properties)
        {
            _header = new Rl4Header();
        }

        private FileHeader _header;

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
