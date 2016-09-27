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
using RlViewer.Files.Rhg.Concrete;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;


namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class KTileCreator : TileCreator.Abstract.ShortSampleTileCreator
    {
        public KTileCreator(LocatorFile rhg, TileOutputType type)
            : base(type)
        {
            _rhg = rhg;
        }

        private LocatorFile _rhg;
        private short _normalFactor;


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
                            _normalFactor = ComputeNormalizationFactor(_rhg, _rhg.Width * _rhg.Header.BytesPerSample,
                                System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.K.KStrHeaderStruct)),
                                Math.Min(_rhg.Height, 4096));
                        }
                    }
                }
                return (float)_normalFactor;

            }
        }


        private short _maxValue;
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
                            _maxValue = GetMaxValue(_rhg, _rhg.Width * _rhg.Header.BytesPerSample,
                                System.Runtime.InteropServices.Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.K.KStrHeaderStruct)));
                        }
                    }
                }
                return (float)_maxValue;

            }
        }

        protected override short[] GetSampleData(byte[] sourceBytes)
        {
            short[] sampleData = new short[sourceBytes.Length / sizeof(short)];

            Buffer.BlockCopy(sourceBytes, 0, sampleData, 0, sourceBytes.Length);

            var amplitudeModulus = new short[sampleData.Length / 2];

            for (int i = 0; i < sampleData.Length; i += 2)
            {
                amplitudeModulus[i / 2] = (short)Math.Sqrt(sampleData[i] * sampleData[i] +
                    sampleData[i + 1] * sampleData[i + 1]);
            }

            return amplitudeModulus;
        }


        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rhg.Width, _rhg.Height);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string filePath)
        {
            return GetTilesFromFile(filePath, _rhg, new Headers.Concrete.K.KStrHeaderStruct(), OutputType);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(string filePath)
        {
            return GetTilesFromFileAsync(filePath, _rhg, new Headers.Concrete.K.KStrHeaderStruct(), OutputType);
        }

    }
}
