using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Files;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;

namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class Raw4TileCreator : RlViewer.Behaviors.TileCreator.Abstract.FloatSampleTileCreator
    {
        public Raw4TileCreator(LocatorFile rli, TileOutputType type, int tileBorderLength)
            : base(type, tileBorderLength)
        {
            _rli = rli;
        }

        private LocatorFile _rli;
        private float _normalFactor;


        private object _locker = new object();
        public override float NormalizationFactor
        {
            get
            {
                if (_normalFactor == 0)
                {
                    lock (_locker)
                    {
                        if (_normalFactor == 0)
                        {
                            _normalFactor = ComputeNormalizationFactor(_rli, _rli.Width * _rli.Header.BytesPerSample,
                                0, Math.Min(_rli.Height, 4096));
                        }
                    }
                }
                return _normalFactor;
            }
        }


        private float _maxValue;
        private object _maxLocker = new object();
        public override float MaxValue
        {
            get
            {
                //double lock checking
                if (_maxValue == 0)
                {
                    lock (_maxLocker)
                    {
                        if (_maxValue == 0)
                        {
                            _maxValue = GetMaxValue(_rli, _rli.Width * _rli.Header.BytesPerSample, 0);
                        }
                    }
                }
                return _maxValue;
            }
        }
        protected override float[] GetSampleData(byte[] sourceBytes)
        {
            float[] sampleData = new float[sourceBytes.Length / sizeof(float)];
            Buffer.BlockCopy(sourceBytes, 0, sampleData, 0, sourceBytes.Length);
            return sampleData;
        }


        protected override Tile[] GetExistingTiles(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rli.Width, _rli.Height);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(int startingLine = 0)
        {
            return GetTilesFromFile(_rli, OutputType, startingLine);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(int startingLine = 0)
        {
            return GetTilesFromFileAsync(_rli, OutputType, startingLine);
        }
      
    }
}

