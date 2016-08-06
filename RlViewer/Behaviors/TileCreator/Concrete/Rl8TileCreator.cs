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
                            _tiles = GetTiles(_rli.Properties.FilePath);
                        }
                    }
                }
                return _tiles;
            }
        }

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
                            System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct()),
                            Math.Min(_rli.Height, (_rli.Header as RlViewer.Headers.Concrete.Rl8.Rl8Header).HeaderStruct.rlParams.cadrHeight));
                        }
                    }
                }
                return _normalFactor;

            }
        }

        protected override float GetMaxValue(LocatorFile loc, int strDataLen, int strHeadLen)
        {
            byte[] bRliString = new byte[strDataLen + strHeadLen];

            float[] fRliString = new float[strDataLen / sizeof(float)];

            float maxSampleValue = 0;

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                while (s.Position != s.Length)
                {
                    s.Read(bRliString, 0, bRliString.Length);
                    Buffer.BlockCopy(bRliString, strHeadLen, fRliString, 0, bRliString.Length - strHeadLen);

                    var amplitudeModulus = new float[fRliString.Length / 2];

                    for (int i = 0; i < fRliString.Length; i += 2)
                    {
                        amplitudeModulus[i / 2] = (float)Math.Sqrt(fRliString[i] * fRliString[i] +
                            fRliString[i + 1] * fRliString[i + 1]); 
                    }

                    var localMax = amplitudeModulus.Max();

                    Array.Clear(fRliString, 0, fRliString.Length);
                    maxSampleValue = maxSampleValue > localMax ? maxSampleValue : localMax;// EqualityComparer<T>.Default.Equals(maxSampleValue, localMax) ? maxSampleValue : localMax;
                }
            }

            if (maxSampleValue == 0) throw new ArgumentException("Corrupted file");
            return maxSampleValue;
        }


        protected override float ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] bRliString = new byte[strDataLen + strHeadLen];
            float[] fRliString = new float[strDataLen / sizeof(float)];
            float normal = 0;

            frameHeight = frameHeight > 1024 ? 1024 : frameHeight;

            long frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;

            MaxValue = GetMaxValue(loc, strDataLen, strHeadLen);


            float histogramStep = MaxValue / 1000f;
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
                    Buffer.BlockCopy(bRliString, strHeadLen, fRliString, 0, bRliString.Length - strHeadLen);

                    var amplitudeModulus = new float[fRliString.Length / 2];

                    for (int i = 0; i < fRliString.Length; i += 2)
                    {
                        amplitudeModulus[i / 2] = (float)Math.Sqrt(fRliString[i] * fRliString[i] +
                            fRliString[i + 1] * fRliString[i + 1]);
                    }

                    avg += amplitudeModulus.Average();

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < amplitudeModulus.Length; i++)
                    {
                        int index = (int)(amplitudeModulus[i] / histogramStep);
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

                normal = (maxIndex + dst) * histogramStep;

                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Computed normalization value of {0}", normal));

                if ((int)normal == 0) normal = histogramStep;
                return normal;
            }
        }


        /// <summary>
        /// Creates tile objects array from existing tile files
        /// </summary>
        /// <param name="directoryPath">Directory with tiles</param>
        /// <returns></returns>
        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rli);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Rl8 file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string path)
        {
            return GetTilesFromFile(path, _rli, new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct(), OutputType);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Rl8 file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(string path)
        {
            return GetTilesFromFileAsync(path, _rli, new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct(), OutputType);
        }



        protected override byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength,
            int tileHeight, TileOutputType outputType)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            float[] fLine = new float[line.Length / sizeof(float)];
            byte[] normalizedLine = new byte[fLine.Length];

            int index = 0;

            float border = NormalizationFactor / 9f * 7;// *3;


            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }

            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);

            var amplitudeModulus = new float[fLine.Length / 2];

            for (int i = 0; i < fLine.Length; i += 2)
            {
                amplitudeModulus[i / 2] = (float)Math.Sqrt(fLine[i] * fLine[i] +
                    fLine[i + 1] * fLine[i + 1]);
            }



            switch (outputType)
            {
                case TileOutputType.Linear:
                    normalizedLine = amplitudeModulus.AsParallel().Select(x => NormalizationHelpers.ToByteRange(x / NormalizationFactor * 255)).ToArray();
                    break;
                case TileOutputType.Logarithmic:
                    normalizedLine = amplitudeModulus.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLogarithmicValue(x, MaxValue))).ToArray();
                    break;
                case TileOutputType.LinearLogarithmic:
                    normalizedLine = amplitudeModulus.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearLogarithmicValue(x, border, MaxValue, NormalizationFactor))).ToArray();
                    break;
                default:
                    break;

            }

            return normalizedLine;
        }





    }
}
