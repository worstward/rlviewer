using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilePreview.Concrete
{
    class Rl4Preview : Abstract.LocatorFilePreview
    {
        public Rl4Preview(string fileName, Headers.Abstract.LocatorFileHeader header)
            : base(header)
        {
            _fileName = fileName;
            _header = header as Headers.Concrete.Rl4.Rl4Header;
        }

        private Headers.Concrete.Rl4.Rl4Header _header;
        private string _fileName;

        public override HeaderInfoOutput GetPreview()
        {
            var preview = new List<Tuple<string, string>>();

            var dateTime = _header.HeaderStruct.rlParams.fileTime.ToDateTime();

            preview.Add(new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(_fileName)));
            preview.Add(new Tuple<string, string>("Дата", dateTime.ToShortDateString()));
            preview.Add(new Tuple<string, string>("Время", dateTime.ToShortTimeString()));

            preview.Add(new Tuple<string, string>("Борт", _header.HeaderStruct.synthParams.board == 0 ? "Левый" : "Правый"));
            preview.Add(new Tuple<string, string>("Начальная дальность, м",          _header.HeaderStruct.synthParams.D0.ToString()));
            

            return new HeaderInfoOutput(_fileName, preview);
        }
    }
}
