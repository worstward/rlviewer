using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Brl4;


namespace RlViewer.Files.Rli.Concrete
{
    public class Brl4 : RliFile
    {
        public Brl4(FileProperties properties) : base(properties)
        {
            _header = new Brl4Header(properties.FilePath);
            Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Brl4 file opened: {0}", properties.FilePath));
        }
        private FileHeader _header;

        public override FileHeader Header
        {
            get { return _header; }
        }


        public override int Height
        {
            get { throw new NotImplementedException(); }
        }

        public override int Width
        {
            get { throw new NotImplementedException(); }
        }
    }
}
