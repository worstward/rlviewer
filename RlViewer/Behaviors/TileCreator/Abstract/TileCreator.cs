using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using RlViewer.Behaviors.Draw;
using RlViewer.Files;


namespace RlViewer.Behaviors.TileCreator.Abstract
{
    public abstract class TileCreator<T> : WorkerEventController, ITileCreator
    {
        public TileCreator(TileOutputType type)
        {
            OutputType = type;
        }

        private System.Drawing.Size _tileSize = new System.Drawing.Size(1024, 1024);
        protected System.Drawing.Size TileSize
        {
            get
            {
                return _tileSize;
            }
        }


        protected TileOutputType OutputType
        {
            get;
            set;
        }

        private string _tileExtension = ".tl";
        protected virtual string TileFileExtension
        {
            get
            {
                return _tileExtension;
            }
        }

        public abstract float NormalizationFactor { get; }

        public virtual float MaxValue
        {
            get;
            protected set;
        }

        public virtual void ClearCancelledFileTiles(string sourceFilePath)
        {
            var path = this.GetDirectoryName(sourceFilePath);

            if (Directory.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                Directory.Delete(path, true);
            }
        }


        protected abstract Tile[] GetExistingTiles(string path);
        protected abstract Tile[] GetTilesFromFileAsync();
        protected abstract Tile[] GetTilesFromFile();
        protected abstract T ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight);
        protected abstract byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, float normalizationFactor,
            int tileHeight, TileOutputType outputType);
        protected virtual Tile[] GetTilesFromFile(LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType)
        {
            if (file.Width == 0 || file.Height == 0)
            {
                throw new ArgumentException(
                    string.Format("Image dimensions can't be equal to zero. Detected width: {0}, height: {1}", file.Width, file.Height));
            }

            var tileFolder = GetDirectoryName(file.Properties.FilePath);
            CreateTileFolder(tileFolder);

            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                    OnReportName("Генерация тайлов");
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, NormalizationFactor,
                        TileSize.Height, outputType);

                    OnProgressReport((int)(i / totalLines * 100));
                    if (OnCancelWorker())
                    {
                        return null;
                    }

                    tiles.AddRange(SaveTiles(tileFolder, tileLine, file.Width, i, TileSize));
                }
            }
            return tiles.ToArray();
        }


        protected virtual Tile[] GetTilesFromFileAsync(LocatorFile file,
           RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType)
        {
            if (file.Width == 0 || file.Height == 0)
            {
                throw new ArgumentException(
                    string.Format("Image dimensions can't be equal to zero. Detected width: {0}, height: {1}", file.Width, file.Height));
            }

            var tileFolder = GetDirectoryName(file.Properties.FilePath);
            CreateTileFolder(tileFolder);


            Task.Factory.StartNew(() =>
            {
                List<Tile> tiles = new List<Tile>();
                byte[] tileLine;
                using (var fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                        tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, NormalizationFactor,
                            TileSize.Height, outputType);
                        SaveTiles(tileFolder, tileLine, file.Width, i, TileSize);
                    }
                }
            });
            return GetExistingTiles(tileFolder);
        }


        protected abstract T[] GetSampleData(byte[] sourceBytes);
        
        /// <summary>
        /// Determines if there are no missing tiles for current file
        /// </summary>
        /// <param name="filePath">Current file path</param>
        /// <param name="tileCount">Expected tiles count</param>
        /// <returns>True if all tiles are present, false otherwise</returns>
        public bool CheckTileConsistency(string filePath, int tileCount)
        {
            var tileDir = GetDirectoryName(filePath);
            var createdTilesCount = Directory.GetFiles(tileDir).Select(
                x => Path.GetExtension(x)).Where(x => x.ToLowerInvariant() == TileFileExtension).Count();
            return createdTilesCount == tileCount;
        }

        /// <summary>
        /// Gets unique name for tile directory
        /// </summary>
        /// <param name="fileName">Initial input file name</param>
        /// <returns></returns>
        public string GetDirectoryName(string filePath)
        {
            string dirPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "tiles",
                Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath),
                File.GetCreationTime(filePath).ToFileTime().ToString());
            return dirPath;
        }

        public virtual Tile[] GetTiles(string filePath, bool forceTileGeneration = false, bool allowScrolling = false)
        {
            var path = GetDirectoryName(filePath);
            Tile[] tiles;

            if (!forceTileGeneration && Directory.Exists(path))
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to get existing tiles");
                tiles = GetExistingTiles(path);
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
                    tiles = GetTilesFromFileAsync();
                }
                else
                {
                    tiles = GetTilesFromFile();
                }
            }

            return tiles;
        }



        protected T GetMaxValue(LocatorFile loc, int strDataLen, int strHeadLen, int fromStr, int toStr)
        {
            byte[] bRliString = new byte[strDataLen];
            T maxSampleValue = default(T);
            var comparer = Comparer<T>.Default;

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                long estimatedFileSize = ((long)(loc.Width * loc.Header.BytesPerSample + loc.Header.StrHeaderLength)) * (long)loc.Height + loc.Header.FileHeaderLength;

                if (s.Length != estimatedFileSize)
                {
                    var msg = string.Format(@"Wrong image dimensions provided, calculated size: {0} bytes, real size: {1} bytes", estimatedFileSize, s.Length);
                    Logging.Logger.Log(Logging.SeverityGrades.Warning, msg);
                    //throw new ArgumentException(msg);
                }

                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);
                var preReadingOffset = (long)(strDataLen + strHeadLen) * (long)fromStr;
                var postReadingOffset = (long)loc.Header.FileHeaderLength + ((long)(strDataLen + strHeadLen) * (long)toStr);

                s.Seek(preReadingOffset, SeekOrigin.Current);

                while (s.Position != postReadingOffset)
                {
                    s.Seek(strHeadLen, SeekOrigin.Current);
                    s.Read(bRliString, 0, bRliString.Length);
                    var rliString = GetSampleData(bRliString);
                    var localMax = rliString.Max();

                    Array.Clear(rliString, 0, rliString.Length);

                    if (comparer.Compare(maxSampleValue, localMax) <= 0)
                    {
                        maxSampleValue = localMax;
                    }

                    OnProgressReport((int)(s.Position / (float)s.Length * 100));
                    OnCancelWorker();
                }
            }

            if (comparer.Compare(maxSampleValue, default(T)) == 0)
            {
                throw new ArgumentException("File is corrupted (zero sample values)");
            }

            return maxSampleValue;
        }

        protected T GetMaxValue(LocatorFile loc, int strDataLen, int strHeadLen)
        {
            OnReportName("Поиск максимальной амплитуды");
            return GetMaxValue(loc, strDataLen, strHeadLen, 0, loc.Height - 1);
        }





        /// <summary>
        /// Creates tile objects array from existing tile files
        /// </summary>
        /// <param name="directoryPath">Directory with tiles</param>
        /// <returns></returns>
        protected virtual Tile[] GetTilesFromTl(string directoryPath, int fileWidth, int fileHeight)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < fileWidth; i += TileSize.Width)
            {
                for (int j = 0; j < fileHeight; j += TileSize.Height)
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
            path = Path.ChangeExtension(path, TileFileExtension);
            File.WriteAllBytes(path, tileData);
            return path;
        }

        protected void CreateTileFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }
}
