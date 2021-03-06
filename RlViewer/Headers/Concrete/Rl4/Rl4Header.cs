﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Files;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Behaviors.Converters;


namespace RlViewer.Headers.Concrete.Rl4
{
    class Rl4Header : RlViewer.Headers.Abstract.LocatorFileHeader
    {
        public Rl4Header(string path)
        {
            _headerStruct = ReadHeader<Rl4RliFileHeader>(path);
            CheckSignature(_headerStruct.fileSign);
        }

        public Rl4Header(Headers.Abstract.IHeaderStruct headerStruct)
        {
            _headerStruct = (Headers.Concrete.Rl4.Rl4RliFileHeader)headerStruct;
            CheckSignature(_headerStruct.fileSign);
        }


        protected override byte[] Signature
        {
            get 
            {
                return _signature;
            }
        }

        public override int FileHeaderLength
        {
            get
            {
                return _headerLength;
            }
        }

        public override int StrHeaderLength
        {
            get 
            {
                return _strHeaderLength;
            }
        }
        public override int BytesPerSample
        {
            get
            {
                return _bytesPerSample;
            }
        }


        private int _bytesPerSample = 4;
        private int _strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Rl4StrHeaderStruct));
        private int _headerLength = System.Runtime.InteropServices.Marshal.SizeOf(new Rl4RliFileHeader());
        private byte[] _signature = new byte[] { 0x52, 0x4c, 0x49, 0x00 };
        private Rl4RliFileHeader _headerStruct;

        public Rl4RliFileHeader HeaderStruct
        {
            get
            {
                return _headerStruct;
            }
            set
            {
                _headerStruct = value;
            }
        }

        protected override HeaderInfoOutput[] GetHeaderInfo()
        {
            return ParseHeader(_headerStruct);
        }


        private HeaderInfoOutput[] ParseHeader(Rl4RliFileHeader headerStruct)
        {
            var rhgHeader = new List<Tuple<string, string>>();

            var fname = string.IsNullOrEmpty(Path.GetFileName(Encoding.UTF8.GetString(headerStruct.rhgParams.fileName).Trim('\0'))) ?
                                             Path.GetFileName(Encoding.UTF8.GetString(headerStruct.synthParams.rhgName).Trim('\0').Trim(' ')) : 
                                             Path.GetFileName(Encoding.UTF8.GetString(headerStruct.rhgParams.fileName).Trim('\0'));
            
            rhgHeader.Add(new Tuple<string, string>("Имя файла",                         fname));
            rhgHeader.Add(new Tuple<string, string>("Размер файла", headerStruct.rhgParams.fileLength == 0 ? "Не определено" : headerStruct.rhgParams.fileLength.ToReadableFileSize()));
            rhgHeader.Add(new Tuple<string, string>("Дата и время полета",             headerStruct.rhgParams.fileTime.ToDateTime().ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по дальности",     headerStruct.rhgParams.cadrWidth.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по азимуту",       headerStruct.rhgParams.cadrHeight.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в РГГ по дальности",       headerStruct.rhgParams.width.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в РГГ по азимуту",         headerStruct.rhgParams.height.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Шаг разложения по дальности",       headerStruct.rhgParams.dx.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Шаг разложения по азимуту",         (headerStruct.rhgParams.dy == 0 ? headerStruct.synthParams.VH : headerStruct.rhgParams.dy) .ToString()));


            var rliHeader = new List<Tuple<string, string>>();
            //rliHeader.Add(new Tuple<string, string>("Размер файла",                      new FileInfo(path).Length.ToReadableFileSize()));
            rliHeader.Add(new Tuple<string, string>("Дата и время создания",             headerStruct.rlParams.fileTime.ToDateTime().ToString()));
            //rliHeader.Add(new Tuple<string, string>("Дата и время синтеза",              headerStruct.rlParams.synthTime.ToDateTime().ToString()));
            rliHeader.Add(new Tuple<string, string>("Ширина, отсчетов",                  headerStruct.rlParams.width.ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота, строк",                     headerStruct.rlParams.height.ToString())); 
            rliHeader.Add(new Tuple<string, string>("Ширина кадра, отсчетов",            headerStruct.rlParams.cadrWidth.ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота кадра, строк",               headerStruct.rlParams.cadrHeight.ToString()));
            rliHeader.Add(new Tuple<string, string>("Кадров",                Math.Ceiling(((double)headerStruct.rlParams.height / (double)headerStruct.rlParams.cadrHeight)).ToString()));
            rliHeader.Add(new Tuple<string, string>("Тип файла/упаковка",                headerStruct.rlParams.type.ToRliFileType()));
            rliHeader.Add(new Tuple<string, string>("Тип дальности",                     headerStruct.rlParams.rangeType == 0 ? "Наклонная" : "Не определено"));
            rliHeader.Add(new Tuple<string, string>("Шаг разложения по дальности, м",    headerStruct.rlParams.dx.ToString()));
            rliHeader.Add(new Tuple<string, string>("Шаг разложения по азимуту, м",      headerStruct.rlParams.dy.ToString()));
            rliHeader.Add(new Tuple<string, string>("Ближний край зоны",                 headerStruct.rlParams.flipType == 0 ? "Слева" : "Справа"));
            rliHeader.Add(new Tuple<string, string>("Отступ от оригинала по X, пикс",    headerStruct.rlParams.sx.ToString()));
            rliHeader.Add(new Tuple<string, string>("Отступ от оригинала по Y, пикс",    headerStruct.rlParams.sy.ToString()));



            var synthHeader = new List<Tuple<string, string>>();
            synthHeader.Add(new Tuple<string, string>("Алгоритм синтеза",                headerStruct.synthParams.processAlgorithm == 18 ? "Омега-К" : "Не определено"));           
            synthHeader.Add(new Tuple<string, string>("Частота повторения, Гц",          headerStruct.synthParams.Fn.ToString()));
            //synthHeader.Add(new Tuple<string, string>("Время",                           headerStruct.synthParams.time.ToDateTime().ToString()));
            synthHeader.Add(new Tuple<string, string>("Начальная дальность, м",          headerStruct.synthParams.D0.ToString()));
            synthHeader.Add(new Tuple<string, string>("Скорость, м/с",                   headerStruct.synthParams.VH.ToString()));
            synthHeader.Add(new Tuple<string, string>("Борт",                            headerStruct.synthParams.board == 0 ? "Левый" : "Правый"));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по наклонной дальности, м", headerStruct.synthParams.dD.ToString()));
            synthHeader.Add(new Tuple<string, string>("Длина волны, м",                  headerStruct.synthParams.lambda.ToString()));
            synthHeader.Add(new Tuple<string, string>("Поляризация",                     headerStruct.synthParams.polarization.ToPolarizationType()));
            
            var comments = new List<Tuple<string, string>>();
            comments.Add(new Tuple<string, string>("Комментарий", Encoding.UTF8.GetString(headerStruct.synthParams.comments).Trim('\0')));

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
