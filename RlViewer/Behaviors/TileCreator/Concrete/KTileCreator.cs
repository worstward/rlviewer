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
                                System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.K.KStrHeaderStruct())
                                , Math.Min(_rhg.Height, 4096));
                        }
                    }
                }
                return (float)_normalFactor;

            }
        }

        private Tile[] _tiles;

        private object _tileLocker = new object();
        public override Tile[] Tiles
        {
            get
            {
                if (_tiles == null)
                {
                    lock (_tileLocker)
                    {
                        if (_tiles == null)
                        {
                            _tiles = GetTiles(_rhg.Properties.FilePath);
                        }
                    }
                }
                return _tiles;
            }
        }


        protected override short GetMaxValue(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] bRliString = new byte[strDataLen + strHeadLen];

            short[] rliString = new short[strDataLen / sizeof(short)];

            short maxSampleValue = 0;

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                while (s.Position != s.Length)
                {
                    s.Read(bRliString, 0, bRliString.Length);

                    var amplitudeModulus = new short[rliString.Length / 2];

                    Buffer.BlockCopy(bRliString, strHeadLen, rliString, 0, bRliString.Length - strHeadLen);

                    for (int i = 0; i < rliString.Length; i += 2)
                    {
                        amplitudeModulus[i / 2] = (short)Math.Sqrt(rliString[i] * rliString[i] +
                           rliString[i + 1] * rliString[i + 1]);
                    }

                    var localMax = amplitudeModulus.Max();


                    Array.Clear(rliString, 0, rliString.Length);

                    maxSampleValue = maxSampleValue > localMax ? maxSampleValue : localMax;
                }
            }

            if (maxSampleValue == 0) throw new ArgumentException("Corrupted file");
            return maxSampleValue;
        }

        protected override short ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] bRliString = new byte[strDataLen + strHeadLen];
            short[] rliString = new short[strDataLen / sizeof(short)];
            short normal = 0;

            frameHeight = frameHeight > 1024 ? 1024 : frameHeight;

            long frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;

            _maxValue = GetMaxValue(loc, strDataLen, strHeadLen, frameHeight);


            float histogramStep = _maxValue / 1000f;
            var histogram = new List<int>();

            for (float i = 0; i < 1000; i += histogramStep)
            {
                histogram.Add(0);
            }

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                float avg = 0;
                int parts = 0;

                while (s.Position != frameLength && s.Position != s.Length)
                {
                    parts++;

                    s.Read(bRliString, 0, bRliString.Length);
                    Buffer.BlockCopy(bRliString, strHeadLen, rliString, 0, bRliString.Length - strHeadLen);

                    var amplitudeModulus = new short[rliString.Length / 2];

                    for (int i = 0; i < rliString.Length; i += 2)
                    {
                        amplitudeModulus[i / 2] = (short)Math.Sqrt(rliString[i] * rliString[i] +
                            rliString[i + 1] * rliString[i + 1]);
                    }

                    avg += (float)rliString.Average(x => x);

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < rliString.Length; i++)
                    {
                        int index = (int)(rliString[i] / histogramStep);
                        if (index < 0) continue;

                        if (index >= histogram.Count)
                            histogram[histogram.Count - 1]++;
                        else histogram[index]++;
                    }
                }

                //find average value of samples array
                avg /= parts;

                //select max histogram value (most often occuring element)
                var max = histogram.Max();

                //get index of max histogram value
                var maxIndex = histogram.Where(x => x == max).Select((x, i) => i).FirstOrDefault();

                //find histogram index of average value sample and shift it
                var avgIndex = avg / histogramStep * 5;


                //get abs distance from max to avg values of histogram
                var dst = Math.Abs(maxIndex - avgIndex);

                normal = (short)((maxIndex + dst) * histogramStep);

                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Computed normalization value of {0}", normal));

                if ((int)normal == 0) normal = (short)histogramStep;
                return normal;
            }
        }


        protected override byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength,
            int tileHeight, TileOutputType outputType)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            short[] sLine = new short[line.Length / sizeof(short)];
            byte[] normalizedLine = new byte[sLine.Length];

            int index = 0;

            float border = NormalizationFactor / 9f * 7;// *3;


            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }

            Buffer.BlockCopy(line, 0, sLine, 0, line.Length);

            var amplitudeModulus = new short[sLine.Length / 2];

            for (int i = 0; i < sLine.Length; i += 2)
            {
                amplitudeModulus[i / 2] = (short)Math.Sqrt(sLine[i] * sLine[i] +
                    sLine[i + 1] * sLine[i + 1]);
            }



            switch (outputType)
            {
                case TileOutputType.Linear:
                    normalizedLine = amplitudeModulus.AsParallel().Select(x => NormalizationHelpers.ToByteRange(x / NormalizationFactor * 255)).ToArray();
                    break;
                case TileOutputType.Logarithmic:
                    normalizedLine = amplitudeModulus.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLogarithmicValue(x, _maxValue))).ToArray();
                    break;
                case TileOutputType.LinearLogarithmic:
                    normalizedLine = amplitudeModulus.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearLogarithmicValue(x, border, _maxValue, NormalizationFactor))).ToArray();
                    break;
                default:
                    break;

            }

            return normalizedLine;
        }


        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rhg);
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
