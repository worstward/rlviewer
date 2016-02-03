using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Hierarchy.Rhg.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;

namespace RlViewer.Hierarchy.Rhg.Concrete
{
    public class RhgK : RhgFile
    {
        public RhgK(FileProperties properties) : base(properties)
        {
            _header = new RhgKHeader();
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
        //        await fs.ReadAsync(_header, 0, array.Length);
        //    }
        //}


        public override bool CheckFile()
        {
            return false; 
         //   bool sigValid = unchecked((uint)0xff00ff00) == ;
        }
    }
}
