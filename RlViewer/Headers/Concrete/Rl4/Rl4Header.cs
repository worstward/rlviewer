using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Rl4;


namespace RlViewer.Headers.Concrete.Rl4
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
        
        private string _path;
        private HeaderInfoOutput[] _headerInfo;
        private RliFileHeader _headerStruct;

        private async Task<byte[]> ReadHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];

            using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }
       
            return header;
        }

        public override async Task<HeaderInfoOutput[]> GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                byte[] _header = await ReadHeaderAsync(_path);

                using (var ms = new System.IO.MemoryStream(_header))
                {
                    _headerStruct = await ReadStruct<RliFileHeader>(ms);
                }
                CheckInfo(_headerStruct);
                _headerInfo = ParseHeader(_headerStruct);
            }
            return _headerInfo;
        }


        private void CheckInfo(RliFileHeader headerStruct)
        {
            for (int i = 0; i < headerStruct.fileSign.Length; i++)
            {
                if (headerStruct.fileSign[i] != _signature[i])
                {
                    throw new ArgumentException("Header signature");
                }
            }
        }



        private HeaderInfoOutput[] ParseHeader(RliFileHeader headerStruct)
        {
            var rhgHeader = new List<Tuple<string, string>>();
            rhgHeader.Add(new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(Encoding.UTF8.GetString(headerStruct.rhgParams.fileName).Trim('\0'))));
            rhgHeader.Add(new Tuple<string, string>("Размер файла",                      headerStruct.rhgParams.fileLength.ToReadableFileSize()));
            rhgHeader.Add(new Tuple<string, string>("Дата и время создания",             headerStruct.rhgParams.fileTime.ToDateTime().ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по дальности",     headerStruct.rhgParams.cadrWidth.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по азимуту",       headerStruct.rhgParams.cadrHeight.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в РГГ по дальности",       headerStruct.rhgParams.width.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в РГГ по азимуту",         headerStruct.rhgParams.height.ToString()));
        
            var rliHeader = new List<Tuple<string, string>>();
            rliHeader.Add(new Tuple<string, string>("Размер файла",                      headerStruct.rlParams.fileLength.ToReadableFileSize()));
            rliHeader.Add(new Tuple<string, string>("Дата и время создания",             headerStruct.rlParams.fileTime.ToDateTime().ToString()));
            rliHeader.Add(new Tuple<string, string>("Ширина, отсчетов",                  headerStruct.rlParams.width.ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота, строк",                     headerStruct.rlParams.height.ToString())); 
            rliHeader.Add(new Tuple<string, string>("Ширина кадра, отсчетов",            headerStruct.rlParams.cadrWidth.ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота кадра, строк",               headerStruct.rlParams.cadrHeight.ToString()));
            rliHeader.Add(new Tuple<string, string>("Кадров",                           (headerStruct.rlParams.height / headerStruct.rlParams.cadrHeight).ToString()));
            rliHeader.Add(new Tuple<string, string>("Тип файла/упаковка",                headerStruct.rlParams.type.ToRliFileType()));
            rliHeader.Add(new Tuple<string, string>("Тип дальности",                     headerStruct.rlParams.rangeType == 0 ? "Наклонная" : "Не определено"));
            rliHeader.Add(new Tuple<string, string>("Шаг разложения по дальности, м",    headerStruct.rlParams.dx.ToString()));
            rliHeader.Add(new Tuple<string, string>("Шаг разложения по азимуту, м",      headerStruct.rlParams.dy.ToString()));

            var synthHeader = new List<Tuple<string, string>>();
            synthHeader.Add(new Tuple<string, string>("Частота повторения, Гц", headerStruct.synthParams.Fn.ToString()));           
            synthHeader.Add(new Tuple<string, string>("Начальная дальность, м", headerStruct.synthParams.D0.ToString()));
            synthHeader.Add(new Tuple<string, string>("Скорость, м/с", headerStruct.synthParams.VH.ToString()));
            synthHeader.Add(new Tuple<string, string>("Борт", headerStruct.synthParams.board == 0 ? "Левый" : "Правый"));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по наклонной дальности, м", headerStruct.synthParams.dD.ToString()));
            synthHeader.Add(new Tuple<string, string>("Длина волны", headerStruct.synthParams.lambda.ToString()));

            var comments = new List<Tuple<string, string>>();
            comments.Add(new Tuple<string, string>("Комментарии", System.IO.Path.GetFileName(Encoding.UTF8.GetString(headerStruct.synthParams.comments).Trim('\0'))));


            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("РГГ", rhgHeader),
                new HeaderInfoOutput("РЛИ", rliHeader),
                new HeaderInfoOutput("Синтез", synthHeader),
                new HeaderInfoOutput("Комментарии", comments)
            };
        }


    }


}
