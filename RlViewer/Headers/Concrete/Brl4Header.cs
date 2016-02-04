using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using System.IO;

namespace RlViewer.Headers.Concrete
{
    class Brl4Header : FileHeader
    {
        public Brl4Header(string path)
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

        private byte[] _signature = new byte[] { 0x00, 0xFF, 0x00, 0xFF };
        private byte[] _header;
        private string _path;


        private async Task<byte[]> FillHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];
            using (var fs = System.IO.File.OpenRead(path))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }

            return header;
        }


        public override async Task<List<Tuple<string, string>>> GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                _header = await FillHeaderAsync(_path);
                CheckInfo(_header);
                return ParseHeader(_header);    
            }
            return _headerInfo;
        }


        private List<Tuple<string, string>> ParseHeader(byte[] header)
        {
            var parsedList = new List<Tuple<string, string>>()
            {

                new Tuple<string,string>("Размер файла"   ,BitConverter.ToInt64(header, 24).ToString()),                        //4
                new Tuple<string,string>("Дата создания"  ,header.Skip(8).Take(16).ToArray().ToDateTime().ToShortDateString()),
                new Tuple<string,string>("Время создания" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToShortTimeString()), //24
                new Tuple<string,string>("Число отсчетов в строке" ,BitConverter.ToInt32(header, 69).ToString()),   
                new Tuple<string,string>("Количество строк" ,BitConverter.ToInt32(header, 73).ToString()),
   

                new Tuple<string,string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString()),
                new Tuple<string,string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString()),
                new Tuple<string,string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString()),
                new Tuple<string,string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString()),
            };

            return parsedList;
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


        List<Tuple<string, string>> _headerInfo;


        private int _headerLength = 16384;
        public override int HeaderLength
        {
            get { return _headerLength; }
        }
    }
}
