using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using RlViewer.Files;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;

namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class Rl8TileCreator : TileCreator.Abstract.FloatSampleTileCreator
    {
        public Rl8TileCreator(LocatorFile rli, TileOutputType type)
            : base(type)
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
                if (_normalFactor == 0)
                {
                    lock (_normalLocker)
                    {
                        if (_normalFactor == 0)
                        {
                            _normalFactor = ComputeNormalizationFactor(_rli, _rli.Width * _rli.Header.BytesPerSample,
                            System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct)),
                            Math.Min(_rli.Height, (_rli.Header as RlViewer.Headers.Concrete.Rl8.Rl8Header).HeaderStruct.rlParams.cadrHeight));
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

            var amplitudeModulus = new float[sampleData.Length / 2];

            for (int i = 0; i < sampleData.Length; i += 2)
            {
                amplitudeModulus[i / 2] = (float)Math.Sqrt(sampleData[i] * sampleData[i] +
                    sampleData[i + 1] * sampleData[i + 1]);
            }

            return amplitudeModulus;
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
        /// Saves tiles to local folder and creates tile objects array from Rl8 file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(int startingLine = 0)
        {
            return GetTilesFromFile(_rli, OutputType, startingLine);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Rl8 file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(int startingLine = 0)
        {
            return GetTilesFromFileAsync(_rli, OutputType, startingLine);
        }


    }
}
