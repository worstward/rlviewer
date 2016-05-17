using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using RlViewer.Behaviors.Draw;
using RlViewer.Files;


namespace RlViewer.Behaviors.TileCreator.Abstract
{
    public abstract class TileCreator : WorkerEventController
    {
        public TileCreator(TileOutputType type)
        {
            OutputType = type;
        }

        private System.Drawing.Size tileSize = new System.Drawing.Size(1024, 1024);
        protected System.Drawing.Size TileSize
        {
            get
            {
                return tileSize;
            }
        }


        protected TileOutputType OutputType
        {
            get;
            set;
        }

        public abstract Tile[] Tiles { get; }

        private string tileExtension = ".tl";
        protected virtual string TileFileExtension
        {
            get
            {
                return tileExtension;
            }
        }

        public abstract float NormalizationFactor { get; }

        public float MaxValue 
        {
            get
            {
                return _maxValue;
            }
        }

        private float _maxValue;

        public virtual Tile[] GetTiles(string filePath, bool forceTileGeneration = false, bool allowScrolling = false)
        {
            var path = GetDirectoryName(filePath);
            Tile[] tiles;

            if (!forceTileGeneration && Directory.Exists(path))
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to get existing tiles");
                tiles = GetTilesFromTl(path);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to create tiles from file");
                if (allowScrolling)
                {
                    tiles = GetTilesFromFileAsync(filePath);
                }
                else
                {
                    tiles = GetTilesFromFile(filePath);
                }
            }

            return tiles;
        }


        protected abstract Tile[] GetTilesFromTl(string path);
        protected abstract Tile[] GetTilesFromFileAsync(string path);
        protected abstract Tile[] GetTilesFromFile(string path);


        protected Tile[] GetTilesFromFile(string filePath, LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType)
        {
            if (file.Width == 0 || file.Height == 0)
            {
                return new Tile[0];
            }

            var tileFolder = GetDirectoryName(filePath, true);

            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(file.Header.FileHeaderLength, SeekOrigin.Begin);
                int signalDataLength = file.Width * file.Header.BytesPerSample;

                int strHeaderLength = 0;
                if (strHeader != null)
                {
                    strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(strHeader);
                }

                var totalLines = Math.Ceiling((double)file.Height / (double)TileSize.Height);
                for (int i = 0; i < totalLines; i++)
                {
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height, NormalizationFactor, outputType);
                    tiles.AddRange(SaveTiles(tileFolder, tileLine, file.Width, i, TileSize));
                    OnProgressReport((int)(i / totalLines * 100));
                    if (OnCancelWorker())
                    {
                        return null;
                    }
                }
            }
            return tiles.ToArray();
        }

        /// <summary>
        /// Creates tile objects array from existing tile files
        /// </summary>
        /// <param name="directoryPath">Directory with tiles</param>
        /// <returns></returns>
        protected virtual Tile[] GetTilesFromTl(string directoryPath, LocatorFile file)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < file.Width; i += TileSize.Width)
            {
                for (int j = 0; j < file.Height; j += TileSize.Height)
                {

                    tiles.Add(new Tile(
                                Path.Combine(
                                directoryPath, (Math.Ceiling(i / (double)TileSize.Width)).ToString() +
                                "-" + Math.Ceiling(j / (double)TileSize.Height).ToString() + TileFileExtension),
                            new Point(i, j), TileSize));
                }
            }

            return tiles.ToArray();
        }


        protected virtual Tile[] GetTilesFromFileAsync(string filePath, LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType)
        {
            if (file.Width == 0 || file.Height == 0)
            {
                return new Tile[0];
            }

            var tileFolder = GetDirectoryName(filePath, true);

            Task.Run(() =>
            {
                List<Tile> tiles = new List<Tile>();
                byte[] tileLine;
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(file.Header.FileHeaderLength, SeekOrigin.Begin);

                    int strHeaderLength = 0;
                    if (strHeader != null)
                    {
                        strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(strHeader);
                    }

                    int signalDataLength = file.Width * file.Header.BytesPerSample;

                    var totalLines = Math.Ceiling((double)file.Height / (double)TileSize.Height);
                    for (int i = 0; i < totalLines; i++)
                    {
                        tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height, NormalizationFactor, outputType);
                        SaveTiles(tileFolder, tileLine, file.Width, i, TileSize);
                    }
                }
            });
            return GetTilesFromTl(tileFolder);
        }



        protected virtual float GetMaxValue(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] arr = new byte[strDataLen + strHeadLen];
            float[] floatArr = new float[strDataLen / 4];

            int frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;
            float maxSampleValue = 0;

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                while (s.Position != frameLength && s.Position != s.Length)
                {
                    s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);
                    var localMax = floatArr.Max();

                    Array.Clear(floatArr, 0, floatArr.Length);

                    if (float.IsNaN(localMax))
                    {
                        continue;
                    }
                    maxSampleValue = maxSampleValue > localMax ? maxSampleValue : localMax;
                }
            }
            return maxSampleValue;
        }

        protected virtual float ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] arr = new byte[strDataLen + strHeadLen];
            float[] floatArr = new float[strDataLen / 4];
            float normal = 0;

            int frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;
            float maxSampleValue = 0;

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                while (s.Position != frameLength && s.Position != s.Length)
                {
                    s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);
                    var localMax = floatArr.Max();

                    Array.Clear(floatArr, 0, floatArr.Length);

                    if(float.IsNaN(localMax))
                    {
                        continue;
                    }
                    maxSampleValue = maxSampleValue > localMax ? maxSampleValue : localMax;
                }
            }

            _maxValue = maxSampleValue;

            float histogramStep = maxSampleValue / 1000f;
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

                    s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);

                    var nonNans = floatArr.Where(x => !float.IsNaN(x)).Where(x => !float.IsInfinity(x));
                    if (nonNans.Count() == 0)
                    {
                        continue;
                    }

                    avg += nonNans.Average();

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < floatArr.Length; i++)
                    {
                        int index = (int)(floatArr[i] / histogramStep);
                        if(index < 0) continue;

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

        protected virtual IEnumerable<Tile> SaveTiles(string folderPath, byte[] line, int linePixelWidth,
            int lineNumber, System.Drawing.Size tileSize)
        {
            byte[] tileData = new byte[tileSize.Width * tileSize.Height];
            List<Tile> tiles = new List<Tile>();

            int bytesToRead = 0;

            using (var ms = new MemoryStream(line))
            {
                for (int i = 0; i < Math.Ceiling((double)line.Length / tileData.Length); i++)
                {
                    Array.Clear(tileData, 0, tileData.Length);
                    ms.Seek(i * TileSize.Width, SeekOrigin.Begin);

                    bytesToRead = Math.Min(tileSize.Width, linePixelWidth - i * tileSize.Width);

                    for (int j = 0; j < tileSize.Height; j++)
                    {
                        ms.Read(tileData, j * tileSize.Width, bytesToRead);
                        ms.Seek(Math.Max(linePixelWidth - bytesToRead, 0), SeekOrigin.Current);
                    }
                  
                    tiles.Add(new Tile(SaveTile(Path.Combine(folderPath, i + "-" + lineNumber), tileData),
                                       new System.Drawing.Point(i * tileSize.Width, lineNumber * tileSize.Height), tileSize));
                    
                }
            }
            return tiles;
        }

        protected virtual string SaveTile(string path, byte[] tileData)
        {
            path += ".tl";
            File.WriteAllBytes(path, tileData);
            return path;
        }

        protected virtual byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, int tileHeight, float normalizationFactor, TileOutputType logarithmicOutput)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            float[] fLine = new float[line.Length / 4];
            byte[] normalizedLine = new byte[fLine.Length];

            int index = 0;

            //if (normalizationFactor > _maxValue)
            //{
            //    _maxValue = normalizationFactor;
            //}

            float border = normalizationFactor / 9f * 7;// *3;

           
            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }

            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);

            switch(logarithmicOutput)
            {
                case TileOutputType.Linear:
                    normalizedLine = fLine.AsParallel<float>().Select(x => NormalizationHelpers.ToByteRange(x / normalizationFactor * 255)).ToArray();
                    break;
                case TileOutputType.Logarithmic:
                    normalizedLine = fLine.AsParallel<float>().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLogarithmicValue(x, _maxValue))).ToArray();
                    break;
                case TileOutputType.LinearLogarithmic:
                    normalizedLine = fLine.AsParallel<float>().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearLogarithmicValue(x, border, _maxValue, normalizationFactor))).ToArray();
                    break;
                default:
                    break;

            }

            return normalizedLine;
        }

       


        /// <summary>
        /// Gets unique name for tile directory
        /// </summary>
        /// <param name="fileName">Initial input file name</param>
        /// <param name="initialize">Should we try to create folder for this path</param>
        /// <returns></returns>
        public static string GetDirectoryName(string filePath, bool initialize = false)
        {
            string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "tiles",
                Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath),
                File.GetCreationTime(filePath).ToFileTime().ToString());


            if (initialize && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
