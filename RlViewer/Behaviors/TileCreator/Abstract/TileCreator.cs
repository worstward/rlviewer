﻿using System;
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
        public TileCreator(TileOutputType type, System.Drawing.Size tileSize)
        {
            OutputType = type;
            TileSize = tileSize;
        }


        protected System.Drawing.Size TileSize
        {
            get;
            private set;
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
        protected abstract Tile[] GetTilesFromFileAsync(int startingLine = 0);
        protected abstract Tile[] GetTilesFromFile(int startingLine = 0);
        protected abstract T ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight);


        protected virtual Tile[] GetTilesFromFile(LocatorFile file, TileOutputType outputType, int startingLine = 0)
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
                int signalDataLength = file.Width * file.Header.BytesPerSample;
                var offset = (file.Header.StrHeaderLength + signalDataLength) * (long)startingLine;

                fs.Seek(file.Header.FileHeaderLength, SeekOrigin.Begin);
                fs.Seek(offset, SeekOrigin.Current);

                var totalLines = Math.Ceiling((double)(file.Height - startingLine) / (double)TileSize.Height);

                for (int i = 0; i < totalLines; i++)
                {
                    OnNameReport("Генерация тайлов");
                    tileLine = GetTileLine(fs, file.Header.StrHeaderLength, signalDataLength, TileSize.Height, outputType);

                    OnProgressReport((int)(i / totalLines * 100));
                    if (OnCancelWorker())
                    {
                        return null;
                    }

                    tiles.AddRange(SaveTiles(tileFolder, tileLine, file.Width, i + startingLine / TileSize.Height, TileSize));
                }
            }
            return tiles.ToArray();
        }


        protected virtual Tile[] GetTilesFromFileAsync(LocatorFile file, TileOutputType outputType, int startingLine = 0)
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
                    int signalDataLength = file.Width * file.Header.BytesPerSample;

                    fs.Seek(file.Header.FileHeaderLength, SeekOrigin.Begin);
                    var offset = (file.Header.StrHeaderLength + signalDataLength) * (long)startingLine;
                    fs.Seek(offset, SeekOrigin.Current);

                    var totalLines = Math.Ceiling((double)(file.Height - startingLine) / (double)TileSize.Height);
                    for (int i = 0; i < totalLines; i++)
                    {
                        tileLine = GetTileLine(fs, file.Header.StrHeaderLength, signalDataLength, TileSize.Height, outputType);
                        SaveTiles(tileFolder, tileLine, file.Width, i + startingLine / TileSize.Height, TileSize);
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

        public virtual Tile[] GetTiles(string filePath, bool forceTileGeneration = false, bool allowScrolling = false, bool synthesis = false, int startingLine = 0)
        {
            var path = GetDirectoryName(filePath);
            Tile[] tiles;

            if (!forceTileGeneration && !synthesis && Directory.Exists(path))
            {
                Logging.Logger.Log(Logging.SeverityGrades.Internal, "Attempting to get existing tiles");
                tiles = GetExistingTiles(path);
            }
            else
            {
                if (!synthesis && Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Logging.Logger.Log(Logging.SeverityGrades.Internal, "Attempting to create tiles from file");

                if (allowScrolling)
                {
                    tiles = GetTilesFromFileAsync(startingLine);
                }
                else
                {
                    tiles = GetTilesFromFile(startingLine);
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

                for (int i = fromStr; i < toStr; i++)
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

                    OnProgressReport((int)(i / (float)toStr * 100));
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
            OnNameReport("Поиск максимальной амплитуды");
            return GetMaxValue(loc, strDataLen, strHeadLen, 0, loc.Height - 1);
        }


        protected abstract byte[] ProcessLinear(T[] samples);
        protected abstract byte[] ProcessLogarithmic(T[] samples);
        protected abstract byte[] ProcessLinLog(T[] samples);



        protected byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength,
          int tileHeight, TileOutputType outputType)
        {
            byte[] line = new byte[signalDataLength * tileHeight];

            int index = 0;

            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }

            var sampleLine = GetSampleData(line);
            byte[] normalizedLine = new byte[sampleLine.Length];

            switch (outputType)
            {
                case TileOutputType.Linear:
                    {
                        normalizedLine = ProcessLinear(sampleLine);
                        break;
                    }
                case TileOutputType.Logarithmic:
                    {
                        normalizedLine = ProcessLogarithmic(sampleLine);
                        break;
                    }
                case TileOutputType.LinearLogarithmic:
                    {
                        normalizedLine = ProcessLinLog(sampleLine);  
                        break;
                    }
                default:
                    break;
            }

            return normalizedLine;
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
