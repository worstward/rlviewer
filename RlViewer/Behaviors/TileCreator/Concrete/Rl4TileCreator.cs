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
    class Rl4TileCreator : TileCreator.Abstract.FloatSampleTileCreator
    {
        public Rl4TileCreator(LocatorFile rli, TileOutputType type) : base(type)
        {
            _rli = rli;
        }

        private LocatorFile _rli;
        private float _normalFactor;


        private object _normalLocker = new object();
        public override float NormalizationFactor
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
                            System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct)),
                            Math.Min(_rli.Height, (_rli.Header as RlViewer.Headers.Concrete.Rl4.Rl4Header).HeaderStruct.rlParams.cadrHeight));
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
                                System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct)));
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

        /// <summary>
        /// Creates tile objects array from existing tile files
        /// </summary>
        /// <param name="directoryPath">Directory with tiles</param>
        /// <returns></returns>
        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rli.Width, _rli.Height);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Rl4 file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string path)
        {
            return GetTilesFromFile(path, _rli, new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct(), OutputType);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Rl4 file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(string path)
        {
            return GetTilesFromFileAsync(path, _rli, new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct(), OutputType);
        }
    }

}
