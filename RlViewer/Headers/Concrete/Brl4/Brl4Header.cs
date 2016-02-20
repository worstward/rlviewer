using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using System.IO;

namespace RlViewer.Headers.Concrete.Brl4
{
    class Brl4Header : FileHeader
    {
        public Brl4Header(string path)
        {
            _path = path;
        }

        protected override byte[] Signature
        {
            get
            {
                return _signature;
            }
        }

        public override int HeaderLength
        {
            get { return _headerLength; }
        }

        private int _headerLength = 16384;
        private byte[] _signature = new byte[] { 0x00, 0xFF, 0x00, 0xFF };
        private byte[] _header;
        private string _path;
        private HeaderInfoOutput[] _headerInfo;

        private byte[] FillHeader(string path)
        {
            byte[] header = new byte[HeaderLength];
            using (var fs = System.IO.File.OpenRead(path))
            {
                fs.Read(header, 0, header.Length);
            }

            return header;
        }


        public override HeaderInfoOutput[] GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                _header = FillHeader(_path);
                CheckInfo(_header);
                _headerInfo = ParseHeader(_header);
            }
            return _headerInfo;
        }

        private void CheckInfo(byte[] header)
        {
            for (int i = 0; i < _signature.Length; i++)
            {
                if (_header[i] != _signature[i])
                {
                    throw new ArgumentException("Header signature");
                }
            }
        }

        private HeaderInfoOutput[] ParseHeader(byte[] header)
        {
            var rhgHeader = new List<Tuple<string, string>>();

            rhgHeader.Add(new Tuple<string, string>("Размер файла", (BitConverter.ToInt64(header, 24) / 1024).ToString() + " kb"));                   //4
            rhgHeader.Add(new Tuple<string, string>("Дата и время создания", header.Skip(8).Take(16).ToArray().ToDateTime().ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по долготе", BitConverter.ToInt32(header, 65).ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по азимуту", BitConverter.ToInt32(header, 69).ToString()));
            rhgHeader.Add(new Tuple<string, string>("Число отсчетов в строке", BitConverter.ToInt32(header, 73).ToString()));
            rhgHeader.Add(new Tuple<string, string>("Количество строк", BitConverter.ToInt64(header, 77).ToString()));

            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("РГГ", rhgHeader)
            };
        }

    }


    struct Brl4HeaderStruct
    {
 
    }



}
