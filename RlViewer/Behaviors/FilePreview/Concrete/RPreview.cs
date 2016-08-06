using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilePreview.Concrete
{
    class RPreview : Abstract.LocatorFilePreview
    {
        public RPreview(string fileName, Headers.Abstract.LocatorFileHeader header)
            : base(header)
        {
            _fileName = fileName;
            _header = header as Headers.Concrete.R.RHeader;
        }

        private Headers.Concrete.R.RHeader _header;
        private string _fileName;

        public override HeaderInfoOutput GetPreview()
        {
            var preview = new List<Tuple<string, string>>();

            var dateTime = new DateTime().AddMilliseconds(_header.HeaderStruct.flightHeader.timeArm).AddYears(1970);
            preview.Add(new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(_fileName)));
            preview.Add(new Tuple<string, string>("Дата", dateTime.ToShortDateString()));
            preview.Add(new Tuple<string, string>("Время", dateTime.ToShortTimeString()));
            
            preview.Add(new Tuple<string, string>("Борт", ((byte)_header.HeaderStruct.synchronizerHeader.board).ToSynchronizerBoard()));
            preview.Add(new Tuple<string, string>("Начальная дальность, м", (_header.HeaderStruct.synchronizerHeader.initialRange * 1000).ToString()));

            return new HeaderInfoOutput(_fileName, preview);
        }
    }
}
