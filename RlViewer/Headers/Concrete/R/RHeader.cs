﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Files;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Behaviors.Converters;



namespace RlViewer.Headers.Concrete.R
{
    class RHeader : RlViewer.Headers.Abstract.LocatorFileHeader
    {
        public RHeader(string path)
        {
            _headerStruct = ReadHeader<RFileHeaderStruct>(path);
            CheckSignature(_headerStruct.signatureHeader.fileSign);
        }

        public RHeader(Headers.Abstract.IHeaderStruct headerStruct)
        {
            _headerStruct = (Headers.Concrete.R.RFileHeaderStruct)headerStruct;
            CheckSignature(_headerStruct.signatureHeader.fileSign);
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
        private int _strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Headers.Concrete.R.RStrHeaderStruct));
        private int _headerLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Headers.Concrete.R.RFileHeaderStruct));
        private byte[] _signature = new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFE, 0x01, 0xFC, 0x01, 0xF8, 0x01, 0xF0, 0x01, 0xAA, 0x55, 0xAA, 0x56 };
        private RFileHeaderStruct _headerStruct;

        public RFileHeaderStruct HeaderStruct
        {
            get
            {
                return _headerStruct;
            }
        }


        protected override HeaderInfoOutput[] GetHeaderInfo()
        {
            return ParseHeader(_headerStruct);
        }


        private HeaderInfoOutput[] ParseHeader(RFileHeaderStruct headerStruct)
        {

            var adcHeader = new List<Tuple<string, string>>();
            adcHeader.Add(new Tuple<string, string>("Задержка АЦП, мс", headerStruct.adcHeader.adcDelay.ToString()));
            adcHeader.Add(new Tuple<string, string>("Частота АЦП, МГц", headerStruct.adcHeader.adcFreq.ToString()));
            adcHeader.Add(new Tuple<string, string>("Делитель частоты АЦП", headerStruct.adcHeader.freqDivisor.ToString()));
            adcHeader.Add(new Tuple<string, string>("Размер строки РГГ", headerStruct.adcHeader.rhgStrSize.ToString()));
            var firmwareString = Encoding.UTF8.GetString(headerStruct.adcHeader.firmWareName);
            firmwareString = firmwareString.Substring(0, firmwareString.IndexOf('\0'));
            adcHeader.Add(new Tuple<string, string>("Имя файла прошивки", firmwareString));
            adcHeader.Add(new Tuple<string, string>("Частота внешнего генератора, МГц", headerStruct.adcHeader.externalGeneratorFreq.ToString()));
            adcHeader.Add(new Tuple<string, string>("Коэффициент деления внешней частоты", headerStruct.adcHeader.externalFreqDivCoef.ToString()));
            adcHeader.Add(new Tuple<string, string>("Генератор частоты", headerStruct.adcHeader.isExternalGenerator == 0 ? "Внутренний" : "Внешний"));
            adcHeader.Add(new Tuple<string, string>("Формат данных", headerStruct.adcHeader.format == 0 ? "16 бит/отсчет" : "8 бит/отсчет"));
            
            var transmitterHeader = new List<Tuple<string, string>>();
            transmitterHeader.Add(new Tuple<string, string>("Несущая частота", headerStruct.transmitterHeader.frequency.ToString()));
            transmitterHeader.Add(new Tuple<string, string>("Ширина полосы, Гц", headerStruct.transmitterHeader.freqWidth.ToString()));
            transmitterHeader.Add(new Tuple<string, string>("Длительность импульса, мкс", headerStruct.transmitterHeader.impulseLength.ToString()));
            transmitterHeader.Add(new Tuple<string, string>("Допплеровский сдвиг", headerStruct.transmitterHeader.dShift.ToString()));       

            var receiverHeader = new List<Tuple<string, string>>();
            receiverHeader.Add(new Tuple<string, string>("Поляризация", headerStruct.receiverHeader.polarization == 0 ? "Вертикальная" : "Горизонтальная"));
            receiverHeader.Add(new Tuple<string, string>("Ширина полосы, Мгц", headerStruct.receiverHeader.freqWidth.ToString()));
            receiverHeader.Add(new Tuple<string, string>("Коэфф затухания УАТТ", headerStruct.receiverHeader.uattCoef.ToString()));
            receiverHeader.Add(new Tuple<string, string>("Функция ВАРУ", headerStruct.receiverHeader.varu.ToString()));       

            var synchronizerHeader = new List<Tuple<string, string>>();
            synchronizerHeader.Add(new Tuple<string, string>("Режим", headerStruct.synchronizerHeader.mode.ToOverviewMode()));
            synchronizerHeader.Add(new Tuple<string, string>("Борт", ((byte)headerStruct.synchronizerHeader.board).ToSynchronizerBoard()));
            synchronizerHeader.Add(new Tuple<string, string>("Поляризация", ((byte)headerStruct.synchronizerHeader.polar).ToPolarizationType()));
            synchronizerHeader.Add(new Tuple<string, string>("Начальная дальность, м", (headerStruct.synchronizerHeader.initialRange * 1000).ToString()));
            synchronizerHeader.Add(new Tuple<string, string>("Делитель частоты повторения", headerStruct.synchronizerHeader.freqDivisor.ToString()));
            synchronizerHeader.Add(new Tuple<string, string>("Знак ЛЧМ по дальности", headerStruct.synchronizerHeader.rangeSign == 1 ? "Плюс" : "Минус"));
            synchronizerHeader.Add(new Tuple<string, string>("Дискрет по азимуту", headerStruct.synchronizerHeader.azimuthDecompositionStep.ToString()));
            synchronizerHeader.Add(new Tuple<string, string>("Знак ЛЧМ по азимуту", headerStruct.synchronizerHeader.azimuthSign == 1 ? "Плюс" : "Минус"));
            synchronizerHeader.Add(new Tuple<string, string>("Внутр/внеш генератор", headerStruct.synchronizerHeader.isExternalGenerator == 0 ? "Внутренний" : "Внешний"));

            var locatorHeader = new List<Tuple<string, string>>();
            locatorHeader.Add(new Tuple<string, string>("Номер канала", headerStruct.locatorHeader.channelNumber.ToString()));
            locatorHeader.Add(new Tuple<string, string>("Режим оператора", headerStruct.locatorHeader.operatorMode.ToString()));
            locatorHeader.Add(new Tuple<string, string>("Версия", headerStruct.locatorHeader.version.ToString()));

            var flightParamHeader = new List<Tuple<string, string>>();
            flightParamHeader.Add(new Tuple<string, string>("Время АРМ", new DateTime().AddMilliseconds(headerStruct.flightHeader.timeArm).AddYears(1970).ToString()));
            flightParamHeader.Add(new Tuple<string, string>("Время UTC", new DateTime().AddMilliseconds(headerStruct.flightHeader.timeUtc).AddYears(1970).ToString()));
            flightParamHeader.Add(new Tuple<string, string>("Номер миссии", headerStruct.flightHeader.missionNum.ToString()));
            flightParamHeader.Add(new Tuple<string, string>("Номер полета", headerStruct.flightHeader.flightNum.ToString()));
            flightParamHeader.Add(new Tuple<string, string>("Номер периода", headerStruct.flightHeader.periodNum.ToString()));
            flightParamHeader.Add(new Tuple<string, string>("Номер файла", headerStruct.flightHeader.fileNum.ToString()));

            var generatorHeader = new List<Tuple<string, string>>();
            generatorHeader.Add(new Tuple<string, string>("ПРДУ", headerStruct.generatorHeader.prdu == 0 ? "Выкл" : "Вкл"));
            generatorHeader.Add(new Tuple<string, string>("ВН", headerStruct.generatorHeader.vn == 0 ? "Выкл" : "Вкл"));
            generatorHeader.Add(new Tuple<string, string>("Излучение", headerStruct.generatorHeader.emission == 0 ? "Разрешено" : "Запрещено"));
            generatorHeader.Add(new Tuple<string, string>("Эквивалент", headerStruct.generatorHeader.equivalent == 0 ? "Выкл" : "Вкл"));

            var gsoHeader = new List<Tuple<string, string>>();
            gsoHeader.Add(new Tuple<string, string>("Насос", headerStruct.sgoHeader.pump == 0 ? "Выкл" : "Вкл"));
            gsoHeader.Add(new Tuple<string, string>("Вентилятор 1", headerStruct.sgoHeader.fan1 == 0 ? "Выкл" : "Вкл"));
            gsoHeader.Add(new Tuple<string, string>("Вентилятор 2", headerStruct.sgoHeader.fan2 == 0 ? "Выкл" : "Вкл"));
            gsoHeader.Add(new Tuple<string, string>("ПТ", headerStruct.sgoHeader.pt == 0 ? "Выкл" : "Вкл"));

            var country = Encoding.UTF8.GetString(headerStruct.flightHeader.country);
            country = firmwareString.Substring(0, country.IndexOf('\0'));
            flightParamHeader.Add(new Tuple<string, string>("Страна полета", country));

            var territory = Encoding.UTF8.GetString(headerStruct.flightHeader.territory);
            territory = firmwareString.Substring(0, territory.IndexOf('\0'));
            flightParamHeader.Add(new Tuple<string, string>("Территория полета", territory));

            var antennaHeader = new List<Tuple<string, string>>();
            antennaHeader.Add(new Tuple<string, string>("Угол раскрыва антенны, град", headerStruct.antennaSystemHeader.antennaAngle.ToString()));

            var synthHeader = new List<Tuple<string, string>>();
            synthHeader.Add(new Tuple<string, string>("Алгоритм синтеза", headerStruct.synthesisHeader.processAlgorithm == 1 ? "ЕОК" : "Не определено"));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по дальности, м", headerStruct.synthesisHeader.dx.ToString()));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по азимуту, м", headerStruct.synthesisHeader.dy.ToString()));
            synthHeader.Add(new Tuple<string, string>("Начальная дальность, м", headerStruct.synthesisHeader.initialRange.ToString()));
            synthHeader.Add(new Tuple<string, string>("Борт", headerStruct.synthesisHeader.sideObservation == 0 ? "Левый" : "Правый"));
            synthHeader.Add(new Tuple<string, string>("Знак ЛЧМ по дальности", headerStruct.synthesisHeader.rangeChirpSign == 1 ? "Плюс" : "Минус"));
            synthHeader.Add(new Tuple<string, string>("Знак ЛЧМ по азимуту", headerStruct.synthesisHeader.azimuthChirpSign == 1 ? "Плюс" : "Минус"));

            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("АЦП", adcHeader),         
                new HeaderInfoOutput("Передатчик", transmitterHeader),
                new HeaderInfoOutput("Приемник", receiverHeader),
                new HeaderInfoOutput("Синхронизатор", synchronizerHeader),
                new HeaderInfoOutput("Синтез", synthHeader),
                new HeaderInfoOutput("Локатор", locatorHeader),
                new HeaderInfoOutput("Полет", flightParamHeader),
                new HeaderInfoOutput("Антенная система", antennaHeader),
                new HeaderInfoOutput("Генератор", generatorHeader),   
                new HeaderInfoOutput("ЖСО", gsoHeader)
            };
        }
    }
}
