﻿using System;
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
    class Brl4TileCreator : TileCreator.Abstract.TileCreator, INormalizable
    {
        public Brl4TileCreator(LocatorFile rli)
        {
            _rli = rli;
        }

        private Dictionary<float, string> pathCollection;

        private LocatorFile _rli;
        private float _normalCoef;

        private Tile[] _tiles;
        private Dictionary<float, Tile[]> tileSets = new Dictionary<float, Tile[]>();

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

                            //var tileFilesNum = Directory.GetFiles(pathCollection[1]).Length;
                            //if (tileFilesNum < _tiles.Length)
                            //{
                            //    Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("{0} tiles missing", _tiles.Length - tileFilesNum));
                            //}
                        }
                    }
                }
                return _tiles;
            }
        }

        private object _normalLocker = new object();
        public float NormalizationCoef
        {
            get
            {
                //double lock checking
                if (_normalCoef == 0)
                {
                    lock (_normalLocker)
                    {
                        if (_normalCoef == 0)
                        {
                            _normalCoef = ComputeNormalizationCoef(_rli, _rli.Width * _rli.Header.BytesPerSample,
                            System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct()),
                            Math.Min(_rli.Height, (_rli.Header as RlViewer.Headers.Concrete.Brl4.Brl4Header).HeaderStruct.rlParams.cadrHeight));
                        }
                    }
                }
                return _normalCoef;

            }
        }


        /// <summary>
        /// Creates tile objects array from existing tile files
        /// </summary>
        /// <param name="directoryPath">Directory with tiles</param>
        /// <returns></returns>
        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < _rli.Width; i += TileSize.Width)
            {
                for (int j = 0; j < _rli.Height; j += TileSize.Height)
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



        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Brl4 file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string filePath, System.ComponentModel.BackgroundWorker worker)
        {
            pathCollection = InitTilePath(filePath);
            
            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_rli.Header.FileHeaderLength, SeekOrigin.Begin);
                int strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(
                    new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());
                int signalDataLength = _rli.Width * _rli.Header.BytesPerSample;

                var totalLines = Math.Ceiling((double)_rli.Height / (double)TileSize.Height);
                for (int i = 0; i < totalLines; i++)
                {
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height);
                    tiles.AddRange(SaveTiles(tileLine, _rli.Width, i, TileSize));
                    worker.ReportProgress((int)(i / totalLines * 100));
                    if (worker.CancellationPending)
                    {
                        return null;
                    }
                }
            }
            return tiles.ToArray();
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Brl4 file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string filePath)
        {
            pathCollection = InitTilePath(filePath);
            var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath), "x1");
           
            Task.Run(() =>
                {
                    List<Tile> tiles = new List<Tile>();
                    byte[] tileLine;
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(_rli.Header.FileHeaderLength, SeekOrigin.Begin);
                        int strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(
                            new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct());
                        int signalDataLength = _rli.Width * _rli.Header.BytesPerSample;

                        var totalLines = Math.Ceiling((double)_rli.Height / (double)TileSize.Height);
                        for (int i = 0; i < totalLines; i++)
                        {
                            tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height);
                            SaveTiles(tileLine, _rli.Width, i, TileSize);
                        }
                    }
                });
            return GetTilesFromTl(path);

            //return tiles.ToArray();
        }

        private byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, int tileHeight)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            float[] fLine = new float[line.Length / 4];
            int index = 0;

            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }
            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);

            //normalize and return
            return fLine.AsParallel<float>().Select(x => (byte)(x * NormalizationCoef)).ToArray<byte>();

        }

        private IEnumerable<Tile> SaveTiles(byte[] line, int linePixelWidth, int lineNumber, Size TileSize)
        {
            byte[] tileData = new byte[TileSize.Width * TileSize.Height];
            List<Tile> tiles = new List<Tile>();

            using (var ms = new MemoryStream(line))
            {
                for (int i = 0; i < Math.Ceiling((double)line.Length / tileData.Length); i++)
                {
                    ms.Seek(i * TileSize.Width, SeekOrigin.Begin);

                    for (int j = 0; j < TileSize.Height; j++)
                    {
                        ms.Read(tileData, j * TileSize.Width, Math.Min(linePixelWidth, TileSize.Width));
                        ms.Seek(Math.Max(linePixelWidth - TileSize.Width, 0), SeekOrigin.Current);
                    }

                    tiles.Add(new Tile(SaveTile(Path.Combine(pathCollection[1], i + "-" + lineNumber), tileData),
                                       new Point(i * TileSize.Width, lineNumber * TileSize.Height), TileSize));
                }
            }
            return tiles;
        }


        private string SaveTile(string path, byte[] tileData)
        {
            path += ".tl";
            File.WriteAllBytes(path, tileData);
            return path;
        }

        private string SaveTileImage(string path, Bitmap bmp)
        {
            path += ".bmp";
            bmp.Save(path);
            return path;
        }
    }
}
