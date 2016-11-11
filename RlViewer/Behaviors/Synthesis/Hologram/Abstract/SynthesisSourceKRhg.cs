using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis.Hologram.Abstract
{
    public abstract class SynthesisSourceKRhg
    {
        public SynthesisSourceKRhg(RlViewer.Files.Rhg.Abstract.RhgFile file)
        {
            _rhgFile = file as RlViewer.Files.Rhg.Concrete.K;

            if (_rhgFile == null)
            {
                throw new ArgumentException("file is not of type .K");
            }

            var headerStruct = (_rhgFile.Header as Headers.Concrete.K.KHeader).HeaderStruct;


            _frequency = headerStruct.transmitterHeader.frequency;
            _freqDeviation = headerStruct.adcHeader.adcDelay;
            _impulseLength = headerStruct.transmitterHeader.impulseLength;
            _sampleFrequency = headerStruct.adcHeader.adcFreq;
            _adcFreqDivisionCoef = headerStruct.adcHeader.freqDivisor;
            _azimuthDiscrete = headerStruct.synchronizerHeader.azimuthDecompositionStep;
            _initialRange = headerStruct.synchronizerHeader.initialRange;
            _antennaAngle = headerStruct.antennaSystemHeader.antennaAngle;
            _board = (byte)headerStruct.synchronizerHeader.board;
            
        }

        private RlViewer.Files.Rhg.Concrete.K _rhgFile;

        private float _frequency;
        public float Frequency
        {
            get
            { 
                return _frequency;
            }
        }

        private float _freqDeviation;

        public float FreqDeviation
        {
            get
            {
                return _freqDeviation; 
            }
        }

        private float _impulseLength;
        public float ImpulseLength
        {
            get
            { 
                return _impulseLength; 
            }
        }

        private float _sampleFrequency;
        public float SampleFrequency
        {
            get 
            {
                return _sampleFrequency; 
            }
        }

        private float _adcFreqDivisionCoef;
        public float AdcFreqDivisionCoef
        {
            get
            { 
                return _adcFreqDivisionCoef;
            }
        }


        private float _azimuthDiscrete;

        public float AzimuthDiscrete
        {
            get
            {
                return _azimuthDiscrete; 
            }
        }


        private float _initialRange;
        public float InitialRange
        {
            get 
            { 
                return _initialRange;
            }
        }

        private float _antennaAngle;

        public float AntennaAngle
        {
            get 
            { 
                return _antennaAngle; 
            }
        }


        private byte _board;
        public byte Board
        {
            get
            { 
                return _board;
            }
        }

        private bool _lchmRangeCompression = false;

        public bool LchmRangeCompression
        {
            get
            {
                return _lchmRangeCompression;
            }
        }
        

        public abstract bool RangeSign
        {
            get;
        }

        public abstract bool AzimuthSign
        {
            get;
        }
    }
}
