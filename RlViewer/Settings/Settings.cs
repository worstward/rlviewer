using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RlViewer.Settings
{
    [DataContract]
    public class Settings
    {

        private bool _allowViewWhileLoading;

        [DataMember(IsRequired=true)]
        public bool AllowViewWhileLoading
        {
            get { return _allowViewWhileLoading; }
            set { _allowViewWhileLoading = value; }
        }


        private bool _forceTileGen;
        [DataMember(IsRequired = true)]
        public bool ForceTileGeneration
        {
            get { return _forceTileGen; }
            set { _forceTileGen = value; }
        }


        private float[] _palette = new float[3] { 1, 1, 1 };

        [DataMember(IsRequired = true)]
        public float[] Palette
        {
            get { return _palette; }
            set { _palette = value; }
        }

        private bool _isPaletteReversed;

        [DataMember(IsRequired = true)]
        public bool IsPaletteReversed
        {
            get { return _isPaletteReversed; }
            set { _isPaletteReversed = value; }
        }

        private bool isGrouped;

        [DataMember(IsRequired = true)]
        public bool IsPaletteGroupped
        {
            get { return isGrouped; }
            set { isGrouped = value; }
        }

        private bool _useTemperaturePalette;

        [DataMember(IsRequired = true)]
        public bool UseTemperaturePalette
        {
            get { return _useTemperaturePalette; }
            set { _useTemperaturePalette = value; }
        }

        private int _sectionSize = 50;

        /// <summary>
        /// Amount of points for section graph
        /// </summary>
        [DataMember(IsRequired = true)]
        public int SectionSize
        {
            get { return _sectionSize; }
            set { _sectionSize = value; }
        }

        private int _areaSize = 3;
        /// <summary>
        /// Rectangle area side length
        /// </summary>
        [DataMember(IsRequired = true)]
        public int SelectorAreaSize
        {
            get { return _areaSize; }
            set { _areaSize = value; }
        }

        private bool _highResForDownScaled = true;

        [DataMember(IsRequired = true)]
        public bool HighResForDownScaled
        {
            get { return _highResForDownScaled; }
            set { _highResForDownScaled = value; }
        }

        /// <summary>
        /// False to use areas, true to use points
        /// </summary>
        private bool _areasOrPointsForAligning = false;

        [DataMember(IsRequired = true)]
        public bool UsePointsForAligning
        {
            get { return _areasOrPointsForAligning; }
            set { _areasOrPointsForAligning = value; }
        }


        private int _maxAlignerAreaSize = 500;
        /// <summary>
        /// Maximum pixels covered by aligner area (if area mode selected)
        /// </summary>
        [DataMember(IsRequired = true)]
        public int MaxAlignerAreaSize
        {
            get { return _maxAlignerAreaSize; }
            set { _maxAlignerAreaSize = value; }
        }
        


        private float _rangeCompressionCoef = 1f;
        [DataMember(IsRequired = true)]
        public float RangeCompressionCoef
        {
            get { return _rangeCompressionCoef; }
            set { _rangeCompressionCoef = value; }
        }


        private float _azimuthCompressionCoef = 1f;
        [DataMember(IsRequired = true)]
        public float AzimuthCompressionCoef
        {
            get { return _azimuthCompressionCoef; }
            set { _azimuthCompressionCoef = value; }
        }

        private Behaviors.TileCreator.TileOutputType _tileOutputAlgorithm = Behaviors.TileCreator.TileOutputType.LinearLogarithmic;
        [DataMember(IsRequired = true)]
        public Behaviors.TileCreator.TileOutputType TileOutputAlgorithm
        {
            get
            {
                return _tileOutputAlgorithm;
            }
            set
            {
                _tileOutputAlgorithm = value;
            }
        }

        private int _aligningAreaBorderSize = 4000;

        [DataMember(IsRequired = true)]
        public int AligningAreaBorderSize
        {
            get 
            {
                return _aligningAreaBorderSize;
            }
            set 
            {
                _aligningAreaBorderSize = value > 9999 ? 9999 : value;
            }
        }

        private int _plot3dAreaBorderSize = 100;

        [DataMember(IsRequired = true)]
        public int Plot3dAreaBorderSize
        {
            get
            {
                return _plot3dAreaBorderSize;
            }
            set
            {
                _plot3dAreaBorderSize = value > 999 ? 999 : value;
            }
        }


        private bool _forceAdminMode = true;

        [DataMember(IsRequired = true)]
        public bool ForceAdminMode
        {
            get { return _forceAdminMode; }
            set { _forceAdminMode = value; }
        }


        private bool _useCustomFileOpenDlg = false;

        [DataMember(IsRequired = true)]      
        public bool UseCustomFileOpenDlg
        {
            get { return _useCustomFileOpenDlg; }
            set { _useCustomFileOpenDlg = value; }
        }

        private System.Net.IPEndPoint _multicastEp = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("234.0.1.1"), 1000);

        [DataMember(IsRequired = true)]
        public System.Net.IPEndPoint MulticastEp
        {
            get { return _multicastEp; }
            set { _multicastEp = value; }
        }

        private float _minScale = 0.125f;

        [DataMember(IsRequired = true)]
        public float MinScale
        {
            get { return _minScale; }
            set { _minScale = value; }
        }

        private float _maxScale = 128f;

        [DataMember(IsRequired = true)]
        public float MaxScale
        {
            get { return _maxScale; }
            set { _maxScale = value; }
        }

        private bool _useKriging = false;

        public bool UseKriging
        {
            get { return _useKriging; }
            set { _useKriging = value; }
        }

    }
}
