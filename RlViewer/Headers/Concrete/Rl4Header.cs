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

        private byte[] _signature = new byte[] { 0x52, 0x4c, 0x49, 0x00 };
        private byte[] _header;
        private string _path;




        public async Task<byte[]> FillHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];

            using (var fs = System.IO.File.OpenRead(path))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }

            var a = new byte[] { header[78], header[77], header[79], header[80] };
            var b = BitConverter.ToInt32(a, 0);
            var c = BitConverter.ToUInt32(a, 0);

            var e = BitConverter.ToInt16(a, 0);
            var f = BitConverter.ToUInt16(a, 0);
            var g = BitConverter.ToSingle(a.Reverse().ToArray(), 0);
            var h = BitConverter.ToString(a, 0);
            var i = BitConverter.ToChar(a, 0);


            return header;
        }


        private int _headerLength = 16384;
        public override int HeaderLength
        {
            get { return _headerLength; }
        }

        public override async Task<List<Tuple<string, string>>> GetHeaderInfo()
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

        List<Tuple<string, string>> _headerInfo;
        private List<Tuple<string, string>> ParseHeader(byte[] header)
        {
            var parsedList = new List<Tuple<string, string>>();

            


            parsedList.Add(new Tuple<string, string>("Размер файла"                  , (BitConverter.ToInt64(header, 24) / 1024).ToString() + " kb"));                   //4
            parsedList.Add(new Tuple<string, string>("Дата и время создания"         , header.Skip(8).Take(16).ToArray().ToDateTime().ToString()));
            parsedList.Add(new Tuple<string, string>("Отсчетов в кадре по долготе"   , BitConverter.ToInt32(header, 65).ToString()));
            parsedList.Add(new Tuple<string, string>("Отсчетов в кадре по азимуту"   , BitConverter.ToInt32(header, 69).ToString())); 
            parsedList.Add(new Tuple<string, string>("Число отсчетов в строке"       , BitConverter.ToInt32(header, 73).ToString()));

            

            parsedList.Add(new Tuple<string, string>("Количество строк"              , BitConverter.ToInt64(header, 77).ToString())); 

            parsedList.Add(new Tuple<string, string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString())); 
            parsedList.Add(new Tuple<string, string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString())); 
            parsedList.Add(new Tuple<string, string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString())); 
            parsedList.Add(new Tuple<string, string>("Время создания РГГ" ,header.Skip(8).Take(16).ToArray().ToDateTime().ToString())); 
            

            return parsedList;
        }



    }
}
