﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilePreview.Concrete
{
    class Brl4Preview : Abstract.LocatorFilePreview
    {
        public Brl4Preview(string fileName, Headers.Abstract.LocatorFileHeader header)
            : base(header)
        {
            _fileName = fileName;
            _header = header as Headers.Concrete.Brl4.Brl4Header;
        }

        private Headers.Concrete.Brl4.Brl4Header _header;
        private string _fileName;

        public override HeaderInfoOutput GetPreview()
        {
            var preview = new List<Tuple<string, string>>();
            var dateTime = _header.HeaderStruct.rlParams.fileTime.ToDateTime();

            preview.Add(new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(_fileName)));
            preview.Add(new Tuple<string, string>("Дата", dateTime.ToShortDateString()));
            preview.Add(new Tuple<string, string>("Время", dateTime.ToShortTimeString()));

            preview.Add(new Tuple<string, string>("Борт",   _header.HeaderStruct.synthParams.board == 0 ? "Левый" : "Правый"));
            preview.Add(new Tuple<string, string>("Начальная дальность, м", _header.HeaderStruct.synthParams.D0.ToString()));
            preview.Add(new Tuple<string, string>("Размер файла", new System.IO.FileInfo(_fileName).Length.ToReadableFileSize()));



            return new HeaderInfoOutput(_fileName, preview);
        }
    }
}
