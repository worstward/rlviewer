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

        [DataMember]
        public bool AllowViewWhileLoading
        {
            get { return _allowViewWhileLoading; }
            set { _allowViewWhileLoading = value; }
        }


        private bool _forceTileGen;
        [DataMember]
        public bool ForceTileGeneration
        {
            get { return _forceTileGen; }
            set { _forceTileGen = value; }
        }


        private float[] _palette = new float[3] { 1, 1, 1 };

        [DataMember]
        public float[] Palette
        {
            get { return _palette; }
            set { _palette = value; }
        }

        private bool _isPaletteReversed;

        [DataMember]
        public bool IsPaletteReversed
        {
            get { return _isPaletteReversed; }
            set { _isPaletteReversed = value; }
        }

        private bool isGrouped;

        [DataMember]
        public bool IsPaletteGroupped
        {
            get { return isGrouped; }
            set { isGrouped = value; }
        }

        private bool _useTemperaturePalette;

        [DataMember]
        public bool UseTemperaturePalette
        {
            get { return _useTemperaturePalette; }
            set { _useTemperaturePalette = value; }
        }

        private int _sectionSize = 50;

        /// <summary>
        /// Amount of points for section graph
        /// </summary>
        [DataMember]
        public int SectionSize
        {
            get { return _sectionSize; }
            set { _sectionSize = value; }
        }

        private int _areaSize = 3;
        /// <summary>
        /// Rectangle area side length
        /// </summary>
        [DataMember]
        public int SelectorAreaSize
        {
            get { return _areaSize; }
            set { _areaSize = value; }
        }

        private bool _highResForDownScaled = true;
        
        [DataMember]
        public bool HighResForDownScaled
        {
            get { return _highResForDownScaled; }
            set { _highResForDownScaled = value; }
        }

        /// <summary>
        /// False to use areas, true to use points
        /// </summary>
        private bool _areasOrPointsForAligning = false;

        [DataMember]
        public bool AreasOrPointsForAligning
        {
            get { return _areasOrPointsForAligning; }
            set { _areasOrPointsForAligning = value; }
        }

        private int _maxAlignerAreaSize = 500;
        [DataMember]
        public int MaxAlignerAreaSize
        {
            get { return _maxAlignerAreaSize; }
            set { _maxAlignerAreaSize = value; }
        }
        


        private float _rangeCompressionCoef = 1f;
        //[DataMember]
        public float RangeCompressionCoef
        {
            get { return _rangeCompressionCoef; }
            set { _rangeCompressionCoef = value; }
        }


        private float _azimuthCompressionCoef = 1f;
        //[DataMember]
        public float AzimuthCompressionCoef
        {
            get { return _azimuthCompressionCoef; }
            set { _azimuthCompressionCoef = value; }
        }

        private Behaviors.TileCreator.TileOutputType _tileOutputAlgorithm = Behaviors.TileCreator.TileOutputType.LinearLogarithmic;
        [DataMember]
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

    }
}
