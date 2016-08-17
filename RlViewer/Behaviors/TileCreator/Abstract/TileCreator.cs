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


        protected abstract Tile[] GetTilesFromTl(string path);
        protected abstract Tile[] GetTilesFromFileAsync(string path);
        protected abstract Tile[] GetTilesFromFile(string path);
        protected abstract T ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight);
        protected abstract byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, int tileHeight, TileOutputType outputType);
        protected abstract Tile[] GetTilesFromFile(string filePath, LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType);
        protected abstract Tile[] GetTilesFromFileAsync(string filePath, LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType);


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

        

        protected T GetMaxValue<T>(LocatorFile loc, int strDataLen, int strHeadLen, Func<T[], T> GetMax) where T : new()
        {
            byte[] bRliString = new byte[strDataLen + strHeadLen];
            T[] rliString = new T[strDataLen / Marshal.SizeOf(new T())];
            T maxSampleValue = default(T);
            var comparer = Comparer<T>.Default;


            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                while (s.Position != s.Length)
                {
                    s.Read(bRliString, 0, bRliString.Length);

                    Buffer.BlockCopy(bRliString, strHeadLen, rliString, 0, bRliString.Length - strHeadLen);

                    var localMax = GetMax(rliString);

                    Array.Clear(rliString, 0, rliString.Length);

                    if (comparer.Compare(maxSampleValue, localMax) <= 0)
                    {
                        maxSampleValue = localMax;
                    }

                    OnProgressReport((int)(s.Position / (float)s.Length * 100));
                    if (OnCancelWorker())
                    {
                        return default(T);
                    }
                }
            }

            return maxSampleValue;
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
