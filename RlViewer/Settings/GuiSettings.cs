using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace RlViewer.Settings
{
    public class GuiSettings : XmlSerializable
    {
        protected override string SavingPath
        {
            get
            {
                var settingsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "guiSettings");
                var fileName = Path.ChangeExtension(settingsPath, SavingExtension);
                return fileName;
            }
        }

        private bool _useCustomFileOpenDlg = false;

        [DataMember(IsRequired = true)]
        public bool UseCustomFileOpenDlg
        {
            get { return _useCustomFileOpenDlg; }
            set { _useCustomFileOpenDlg = value; }
        }

        private bool _showRhgSynthesisHeaderParamsTab = false;

        [DataMember(IsRequired = true)]
        public bool ShowRhgSynthesisHeaderParamsTab
        {
            get { return _showRhgSynthesisHeaderParamsTab; }
            set { _showRhgSynthesisHeaderParamsTab = value; }
        }


        private bool _showServerSar = false;

        [DataMember(IsRequired = true)]
        public bool ShowServerSar
        {
            get { return _showServerSar; }
            set { _showServerSar = value; }
        }

          private List<int> _blockAzimuthSizes = new List<int>()
        {
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536,
            131072
        };


        [DataMember(IsRequired = true)]
        public List<int> BlockAzimuthSizes
        {
            get { return _blockAzimuthSizes; }
            set { _blockAzimuthSizes = value; }
        }



        private List<int> _frameAzimuthSizes = new List<int>()
        {
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536,
            131072
        };

        [DataMember(IsRequired = true)]
        public List<int> FrameAzimuthSizes
        {
            get { return _frameAzimuthSizes; }
            set { _frameAzimuthSizes = value; }
        }


        private List<int> _blockRangeCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };

        [DataMember(IsRequired = true)]
        public List<int> BlockRangeCompressionCoefs
        {
            get { return _blockRangeCompressionCoefs; }
            set { _blockRangeCompressionCoefs = value; }
        }


        private List<int> _blockAzimuthCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };

        [DataMember(IsRequired = true)]
        public List<int> BlockAzimuthCompressionCoefs
        {
            get { return _blockAzimuthCompressionCoefs; }
            set { _blockAzimuthCompressionCoefs = value; }
        }



        private List<int> _frameRangeCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };

        [DataMember(IsRequired = true)]
        public List<int> FrameRangeCompressionCoefs
        {
            get { return _frameRangeCompressionCoefs; }
            set { _frameRangeCompressionCoefs = value; }


        }

        private List<int> _frameAzimuthCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };
        [DataMember(IsRequired = true)]
        public List<int> FrameAzimuthCompressionCoefs
        {
            get { return _frameAzimuthCompressionCoefs; }
            set { _frameAzimuthCompressionCoefs = value; }
        }


        private List<float> _matrixExtensionCoefs = new List<float>() { 1, 1.5f, 2 };

        [DataMember(IsRequired = true)]
        public List<float> MatrixExtensionCoefs
        {
            get { return _matrixExtensionCoefs; }
            set { _matrixExtensionCoefs = value; }
        }

        

        private List<int> _pNLengthValues = new List<int>()
        {
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536
        };


        [DataMember(IsRequired = true)]
        public List<int> PNLengthValues
        {
            get { return _pNLengthValues; }
            set { _pNLengthValues = value; }
        }

        private List<int> _minDopplerFilterValues = new List<int>()
        {
            0,
            128,
            256,
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536
        };


        [DataMember(IsRequired = true)]
        public List<int> MinDopplerFilterValues
        {
            get { return _minDopplerFilterValues; }
            set { _minDopplerFilterValues = value; }
        }


        private List<int> _maxDopplerFilterValues = new List<int>()
        {
            0,
            128,
            256,
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536
        };


        [DataMember(IsRequired = true)]
        public List<int> MaxDopplerFilterValues
        {
            get { return _maxDopplerFilterValues; }
            set { _maxDopplerFilterValues = value; }
        }


        private int _recentFilesToDisplay = 5;

        [DataMember(IsRequired = true)]
        public int RecentFilesToDisplay
        {
            get { return _recentFilesToDisplay; }
            set
            {
                _recentFilesToDisplay = value > 20 || value < 0 ? 5 : value; 
            }
        }

    }
}
