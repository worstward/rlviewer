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

    class Rl4TileCreator : TileCreator.Abstract.TileCreator, INormalizable
    {
        public Rl4TileCreator(LocatorFile rli)
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
                            System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct()),
                            Math.Min(_rli.Height, (_rli.Header as RlViewer.Headers.Concrete.Rl4.Rl4Header).HeaderStruct.rlParams.cadrHeight));
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
        /// Saves tiles to local folder and creates tile objects array from Rl4 file.  Reports progress to backgroundworker object.
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
                    new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
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
        /// Saves tiles to local folder and creates tile objects array from Rl4 file.
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
                            new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
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
            byte[] normalizedLine = new byte[fLine.Length];

            int index = 0;

            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }
            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);

            normalizedLine = fLine.AsParallel<float>().Select(x => (byte)(x * NormalizationCoef)).ToArray<byte>();

            return normalizedLine;
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
                        ms.Read(tileData, j * TileSize.Width, TileSize.Width);
                        ms.Seek(linePixelWidth - TileSize.Width, SeekOrigin.Current);
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


        ///// <summary>
        ///// Saves tiles to local folder and creates tile objects array from Rl4 file
        ///// </summary>
        ///// <returns></returns>
        //protected override Tile[] GetTilesFromFile(string filePath)
        //{
        //    pathCollection = InitTilePath(filePath);
        //    prop = new RlViewer.ParallelProperties(0, (int)Math.Ceiling((double)_rli.Height / (double)TileSize.Height));

        //    List<Tile> tiles = new List<Tile>();
        //    byte[] tileLine;
        //    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //    {
        //        fs.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);
        //        int strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
        //        int signalDataLength = _rli.Width *  _rli.Header.BytesPerSample;

        //        Parallel.ForEach(prop.Chunks, prop.Options, range =>
        //        {
        //            for (int i = range.Item1; i < range.Item2; i++)
        //            {
        //                tileLine = GetTileLine(filePath, i, strHeaderLength, signalDataLength, TileSize.Height);
        //                tiles.AddRange(SaveTiles(tileLine, _rli.Width, i, TileSize));
        //            }
        //        });
        //    }
        //    //GetResizedTiles(tiles.ToArray());
        //    return tiles.ToArray();
        //}

        //private byte[] GetTileLine(string filePath, int currentLine, int strHeaderLength, int signalDataLength, int tileHeight)
        //{
        //    byte[] line = new byte[signalDataLength * tileHeight];
        //    float[] fLine = new float[line.Length / 4];
        //    byte[] normalizedLine = new byte[fLine.Length];
        //    int index = 0;

        //    var offset = _rli.Header.HeaderLength + currentLine * tileHeight * (strHeaderLength + signalDataLength);
        //    using (var s = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //    {
        //        s.Seek(offset, SeekOrigin.Begin);
        //        while (index != line.Length && s.Position != s.Length)
        //        {
        //            s.Seek(strHeaderLength, SeekOrigin.Current);
        //            index += s.Read(line, index, signalDataLength);
        //        }
        //    }
        //    Buffer.BlockCopy(line, 0, fLine, 0, line.Length);
        //    //rework normalization coef
        //    normalizedLine = fLine.AsParallel<float>().Select(x => (byte)(x * NormalizationCoef)).ToArray<byte>();

        //    return normalizedLine;
        //}

        //private IEnumerable<Tile> SaveTiles(byte[] line, int linePixelWidth, int lineNumber, Size TileSize)
        //{
        //    byte[] tileData = new byte[TileSize.Width * TileSize.Height];
        //    List<Tile> tiles = new List<Tile>();

        //    using (var ms = new MemoryStream(line))
        //    {
        //        for (int i = 0; i < Math.Ceiling((double)line.Length / tileData.Length); i++)
        //        {
        //            ms.Seek(i * TileSize.Width, SeekOrigin.Begin);

        //            for (int j = 0; j < TileSize.Height; j++)
        //            {
        //                ms.Read(tileData, j * TileSize.Width, TileSize.Width);
        //                ms.Seek(linePixelWidth - TileSize.Width, SeekOrigin.Current);
        //            }
        //            Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("tile generated: {0} - {1}", i, lineNumber));
        //            tiles.Add(new Tile(SaveTile(Path.Combine(pathCollection[1], i + "-" + lineNumber), tileData),
        //                               new Point(i * TileSize.Width, lineNumber * TileSize.Height), TileSize));
        //        }
        //    }
        //    return tiles;
        //}
        


        //private Tile[] GetResizedTiles(Tile[] tiles)
        //{
        //    foreach (var zooms in paths)
        //    {
        //        //we skip zoom value of 1 (normal picture) since it's already processed by GetTiles()
        //        if (zooms.Key < 1)
        //        {
        //            foreach (var tile in tiles)
        //            {
        //                ResizeTileIn(tile, zooms.Key);
        //            }
        //        }
        //        else if(zooms.Key > 1)
        //        {
        //            foreach (var tile in tiles)
        //            {
        //                ResizeTileIn(tile, zooms.Key);
        //            }
        //        }


        //    }
        //    return null;
        //}


        //Tile[] ResizeTileIn(Tile tile, float zoomValue)
        //{
        //    var tiles = new List<Tile>();

        //        Bitmap littleTile = new Bitmap(TileSize.Width, TileSize.Height);
        //        var img = Image.FromFile(tile.FilePath);
        //        var resized = Resizer.ResizeImage(img, (int)(tile.Size.Width / zoomValue), (int)(tile.Size.Width / zoomValue));

        //        for (int i = 0; i < resized.Width; i += littleTile.Width)
        //        {
        //            for (int j = 0; j < resized.Height; j += littleTile.Height)
        //            {
        //                var newPath = Path.Combine(paths[zoomValue], (tile.LeftTopCoord.X / zoomValue + i) / TileSize.Width + "-" +
        //                     (tile.LeftTopCoord.Y * zoomValue + j) / TileSize.Height);

        //                var newBmp = resized.Clone(new Rectangle(new Point(i, j), littleTile.Size), PixelFormat.Format8bppIndexed);

        //                tiles.Add(new Tile(SaveTileImage(newPath, newBmp),
        //                    new PointF(tile.LeftTopCoord.X + i * zoomValue, tile.LeftTopCoord.Y + j * zoomValue),
        //                    littleTile.Size));
        //            }                  
        //        }

        //    return tiles.ToArray();
        //}

        //Tile[] ResizeTileOut(Tile[] tile, float zoomValue)
        //{
        //    var tiles = new List<Tile>();

        //    Bitmap largeTile = new Bitmap(TileSize.Width * (int)zoomValue, TileSize.Height * (int)zoomValue);


        //    //for (int i = 0; i < resized.Width; i += littleTile.Width)
        //    //{
        //    //    for (int j = 0; j < resized.Height; j += littleTile.Height)
        //    //    {
        //    //        var newPath = Path.Combine(paths[zoomValue], (tile.LeftTopCoord.X / zoomValue + i) / TileSize.Width + "-" +
        //    //             (tile.LeftTopCoord.Y * zoomValue + j) / TileSize.Height);

        //    //        var newBmp = resized.Clone(new Rectangle(new Point(i, j), littleTile.Size), PixelFormat.Format8bppIndexed);

        //    //        tiles.Add(new Tile(SaveTileImage(newPath, newBmp),
        //    //            new PointF(tile.LeftTopCoord.X + i * zoomValue, tile.LeftTopCoord.Y + j * zoomValue),
        //    //            littleTile.Size));
        //    //    }
        //    //}


        //    //var resized = Resizer.ResizeImage(img, (int)(tile.Size.Width / zoomValue), (int)(tile.Size.Width / zoomValue));
        //    return null;
        //}

}
