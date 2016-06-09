using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RlViewer.Headers.Concrete.R;

namespace RlViewer.Headers.Concrete.K
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KFileHeaderStruct
    {

        public SignatureHeader signatureHeader;		// сигнатура заголовка
        public FlightHeader flightHeader;			// параметры полета
        public LocatorHeader locatorHeader;			// режим локатора
        public ReceiverHeader receiverHeader;			// параметры приемника
        public TransmitterHeader transmitterHeader;		// параметры передатчика
        public SynchronizerHeader synchronizerHeader;		// параметры синхронизатора
        public GeneratorHeader generatorHeader;		// параметры генератора
        public SgoHeader sgoHeader;				// параметры СЖО
        public AntennaSystemHeader antennaSystemHeader;	// параметры антенной системы
        public AdcHeader adcHeader;				// параметры АЦП
        //public SynthesisHeader synthesisHeader;		// параметры синтеза;
        public LineInfoHeader lineInfoHeader;			// формат строки
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KStrHeaderStruct : Abstract.IStrHeader
    {
        public SignatureHeader signatureHeader; 				// сигнатура заголовка
        public NavigationHeader navigationHeader;				// параметры навигации
        public ControlReceiverHeader controlReceiverHeader;			// контрольные параметры приемника
        public ControlTransmitterHeader controlTransmitterHeader;		// контрольные параметры передатчика
        public ControlSynchronizerHeader controlSynchronizerHeader;		// контрольные параметры синхронизатора
        public ControlGeneratorHeader controlGeneratorHeader;			// контрольные параметры генератора
        public ControlSgoHeader controlSgoHeader;				// контрольные параметры СЖО
        public ControlAntennaSystemHeader controlAntennaSystemHeader;		// контрольные параметры антенной системы
        public ControlAdcHeader controlAdcHeader;				// контрольные параметры АЦП
    };


}
