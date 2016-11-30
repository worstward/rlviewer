using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RlViewer.Files;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;

namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class Brl4TileCreator : TileCreator.Abstract.FloatSampleTileCreator
    {
        public Brl4TileCreator(LocatorFile rli, TileOutputType type, System.Drawing.Size tileSize)
            : base(type, tileSize)
        {
            _rli = rli;
        }

        private LocatorFile _rli;
        private float _normalFactor;
       
        private object _normalLocker = new object();
        public override  float NormalizationFactor
        {
            get
            {
                //double lock checking
                if (_normalFactor == 0)
                {
                    lock (_normalLocker)
                    {
                        if (_normalFactor == 0)
                        {
                            _normalFactor = ComputeNormalizationFactor(_rli, _rli.Width * _rli.Header.BytesPerSample,
                            System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct)),
                            Math.Min(_rli.Height, (_rli.Header as RlViewer.Headers.Concrete.Brl4.Brl4Header).HeaderStruct.rlParams.cadrHeight));
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
                            _maxValue = GetMaxValue(_rli, _rli.Width * _rli.Header.BytesPerSample,
                                System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct)));
                        }
                    }
                }
                return (float)_maxValue;

            }
        }

        protected override float[] GetSampleData(byte[] sourceBytes)
        {
            float[] sampleData = new float[sourceBytes.Length / sizeof(float)];
            Buffer.BlockCopy(sourceBytes, 0, sampleData, 0, sourceBytes.Length);
            return sampleData;
        }

        /// <summary>
        /// Creates tile objects array from existing tile files
        /// </summary>
        /// <param name="directoryPath">Directory with tiles</param>
        /// <returns></returns>
        protected override Tile[] GetExistingTiles(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rli.Width, _rli.Height);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Brl4 file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(int startingLine = 0)
        {
            return GetTilesFromFile(_rli, OutputType, startingLine);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Brl4 file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(int startingLine = 0)
        {
            return GetTilesFromFileAsync(_rli, OutputType, startingLine);
        }


    }
}

