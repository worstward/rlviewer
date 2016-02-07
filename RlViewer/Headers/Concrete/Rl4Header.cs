using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;


namespace RlViewer.Headers.Concrete
{
    class Rl4Header : FileHeader
    {
        public Rl4Header(string path)
        {
            _path = path;
        }

        public override byte[] Signature
        {
            get 
            {
                return _signature;
            }
        }
  
        public override int HeaderLength
        {
            get
            {
                return _headerLength;
            }
        }

        private int _headerLength = 16384;
        private byte[] _signature = new byte[] { 0x52, 0x4c, 0x49, 0x00 };
        private byte[] _header;
        private string _path;
        private HeaderInfoOutput[] _headerInfo;

        private async Task<byte[]> FillHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];

            using (var fs = System.IO.File.OpenRead(path))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }
       
            return header;
        }

        public override async Task<HeaderInfoOutput[]> GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                _header = await FillHeaderAsync(_path);
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


            var pathToRhg = Encoding.UTF8.GetString(header.Skip(3839).Take(256).ToArray()).TrimEnd('\0');
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по долготе", System.IO.Path.GetFileName(pathToRhg)));
            rhgHeader.Add(new Tuple<string, string>("Размер файла",                BitConverter.ToInt64(header, 24).ToReadableFileSize()));
            rhgHeader.Add(new Tuple<string, string>("Дата и время создания",       header.Skip(8).Take(16).ToArray().ToDateTime().ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по долготе", BitConverter.ToInt32(header, 65).ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по азимуту", BitConverter.ToInt32(header, 69).ToString()));
            rhgHeader.Add(new Tuple<string, string>("Число отсчетов в строке",     BitConverter.ToInt32(header, 73).ToString()));
            rhgHeader.Add(new Tuple<string, string>("Количество строк",            BitConverter.ToInt32(header, 77).ToString()));


            var rliHeader = new List<Tuple<string, string>>();
            rliHeader.Add(new Tuple<string, string>("Дата и время создания", header.Skip(4104).Take(16).ToArray().ToDateTime().ToString()));
            rliHeader.Add(new Tuple<string, string>("Размер файла",                BitConverter.ToInt64(header, 4120).ToReadableFileSize()));
            rliHeader.Add(new Tuple<string, string>("Тип файла", header[4137].ToRliFileType()));
            
            rliHeader.Add(new Tuple<string, string>("Ширина кадра", BitConverter.ToInt32(header, 4161).ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота кадра", BitConverter.ToInt32(header, 4165).ToString()));

            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("РГГ", rhgHeader),
                new HeaderInfoOutput("РЛИ", rliHeader)
            };
        }


    }
}
