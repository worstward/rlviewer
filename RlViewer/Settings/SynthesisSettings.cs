using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace RlViewer.Settings
{

    [DataContract]
    public class SynthesisSettings : XmlSerializable
    {
        protected override string SavingPath
        {
            get
            {
                var settingsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "synthesisSettings");
                var fileName = Path.ChangeExtension(settingsPath, SavingExtension);
                return fileName;
            }
        }

        private int _speedOfLight = 299792458;

        [DataMember(IsRequired = true)]
        public int SpeedOfLight
        {
            get { return _speedOfLight; }
            set { _speedOfLight = value; }
        }

        private string _sstpSharedMemoryName = "SSTP_inSharedMem";

        [DataMember(IsRequired = true)]
        public string SstpSharedMemoryName
        {
            get { return _sstpSharedMemoryName; }
            set { _sstpSharedMemoryName = value; }
        }

        private string _errorSharedMemoryNameTemplate = "ErrMes_inSharedMem_";

        [DataMember(IsRequired = true)]
        public string ErrorSharedMemoryNameTemplate
        {
            get { return _errorSharedMemoryNameTemplate; }
            set { _errorSharedMemoryNameTemplate = value; }
        }


        private string _dspSharedMemoryNameTemplate = "DSP_inSharedMem_";

        [DataMember(IsRequired = true)]
        public string DspSharedMemoryNameTemplate
        {
            get { return _dspSharedMemoryNameTemplate; }
            set { _dspSharedMemoryNameTemplate = value; }
        }


        private string _holSharedMemoryNameTemplate = "Hol_inSharedMem_";

        [DataMember(IsRequired = true)]
        public string HologramSharedMemoryNameTemplate
        {
            get { return _holSharedMemoryNameTemplate; }
            set { _holSharedMemoryNameTemplate = value; }
        }


        private string _rliSharedMemoryNameTemplate = "Rli_inSharedMem_";

        [DataMember(IsRequired = true)]
        public string RliSharedMemoryNameTemplate
        {
            get { return _rliSharedMemoryNameTemplate; }
            set { _rliSharedMemoryNameTemplate = value; }
        }

        private int _memoryChunksCount = 1;

        [DataMember(IsRequired = true)]
        public int MemoryChunksCount
        {
            get { return _memoryChunksCount; }
            set { _memoryChunksCount = value; }
        }


        private int _waitTimeOut = 1000000;

        [DataMember(IsRequired = true)]
        public int WaitTimeOut
        {
            get { return _waitTimeOut; }
            set { _waitTimeOut = value; }
        }


        #region Signal

      

        private int _blockAzimuthSize = 8192;

        [DataMember(IsRequired = true)]
        public int BlockAzimuthSize
        {
            get { return _blockAzimuthSize; }
            set { _blockAzimuthSize = value; }
        }

        private int _frameAzimuthSize = 4096;

        [DataMember(IsRequired = true)]
        public int FrameAzimuthSize
        {
            get { return _frameAzimuthSize; }
            set { _frameAzimuthSize = value; }
        }

        private int _blockAzimuthCompressionCoef = 1;

        [DataMember(IsRequired = true)]
        public int BlockAzimuthCompressionCoef
        {
            get { return _blockAzimuthCompressionCoef; }
            set { _blockAzimuthCompressionCoef = value; }
        }


        private int _blockRangeCompressionCoef = 1;

        [DataMember(IsRequired = true)]
        public int BlockRangeCompressionCoef
        {
            get { return _blockRangeCompressionCoef; }
            set { _blockRangeCompressionCoef = value; }
        }

        private int _frameAzimuthCompressionCoef = 4;

        [DataMember(IsRequired = true)]
        public int FrameAzimuthCompressionCoef
        {
            get { return _frameAzimuthCompressionCoef; }
            set { _frameAzimuthCompressionCoef = value; }
        }

        private int _frameRangeCompressionCoef = 1;

        [DataMember(IsRequired = true)]
        public int FrameRangeCompressionCoef
        {
            get { return _frameRangeCompressionCoef; }
            set { _frameRangeCompressionCoef = value; }
        }


        private float _matrixExtensionCoef = 1f;

        [DataMember(IsRequired = true)]
        public float MatrixExtensionCoef
        {
            get { return _matrixExtensionCoef; }
            set { _matrixExtensionCoef = value; }
        }

        private float _radioSuppressionCoef = 0.3f;

        [DataMember(IsRequired = true)]
        public float RadioSuppressionCoef
        {
            get { return _radioSuppressionCoef; }
            set { _radioSuppressionCoef = value; }
        }

        private float _rhgNormalizingCoef = 1f;

        [DataMember(IsRequired = true)]
        public float RhgNormalizingCoef
        {
            get { return _rhgNormalizingCoef; }
            set { _rhgNormalizingCoef = value; }
        }

        private float _rliNormalizingCoef = 1f;

        [DataMember(IsRequired = true)]
        public float RliNormalizingCoef
        {
            get { return _rliNormalizingCoef; }
            set { _rliNormalizingCoef = value; }
        }


        #endregion


        #region Eok

        private bool _useDopplerFiltering = true;

        [DataMember(IsRequired = true)]
        public bool UseDopplerFiltering
        {
            get { return _useDopplerFiltering; }
            set { _useDopplerFiltering = value; }
        }

        private int _minDoppler = 1024;

        [DataMember(IsRequired = true)]
        public int MinDoppler
        {
            get { return _minDoppler; }
            set { _minDoppler = value; }
        }

        private int _maxDoppler = 1024;

        [DataMember(IsRequired = true)]
        public int MaxDoppler
        {
            get { return _maxDoppler; }
            set { _maxDoppler = value; }
        }

        private int _pNLength = 4096;

        [DataMember(IsRequired = true)]
        public int PNLength
        {
            get { return _pNLength; }
            set { _pNLength = value; }
        }

        private int _pNShift = 4096;

        [DataMember(IsRequired = true)]
        public int PNShift
        {
            get { return _pNShift; }
            set { _pNShift = value; }
        }

        #endregion

    }
}
