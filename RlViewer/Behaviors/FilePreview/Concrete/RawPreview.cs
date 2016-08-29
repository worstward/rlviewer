using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilePreview.Concrete
{
    class RawPreview : Abstract.LocatorFilePreview
    {
        public RawPreview(string fileName, Headers.Abstract.LocatorFileHeader header)
            : base(header)
        {
            _fileName = fileName;
        }

        private string _fileName;


        public override HeaderInfoOutput GetPreview()
        {
            var preview = new List<Tuple<string, string>>();
            preview.Add(new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(_fileName)));
            preview.Add(new Tuple<string, string>("Имя файла", ""));
            preview.Add(new Tuple<string, string>("Имя файла", ""));
            preview.Add(new Tuple<string, string>("Имя файла", ""));
            preview.Add(new Tuple<string, string>("Имя файла", ""));
            preview.Add(new Tuple<string, string>("Размер файла", new System.IO.FileInfo(_fileName).Length.ToReadableFileSize()));

            return new HeaderInfoOutput(_fileName, preview);

            
        }
    }
}
