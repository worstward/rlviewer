using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Files;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;



namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class RawTileCreator : RlViewer.Behaviors.TileCreator.Abstract.TileCreator
    {
        public RawTileCreator(LocatorFile rli)
        {
            _rli = rli;
            //prop = new ParallelProperties(0, (int)(new System.IO.FileInfo(rli.Properties.FilePath).Length));
            //_normalCoef = ComputeNormalizationCoef(rli.Width * bytesPerSample, 256);

            //_tiles = GetTiles(_rli.Properties.FilePath);
        }

        private Dictionary<float, string> pathCollection;

        private LocatorFile _rli;
        private float _normalCoef;
        private int bytesPerSample = 4;//float - 4 байта на отсчет

        //private RlViewer.ParallelProperties prop;

        private Tile[] _tiles;
        private Dictionary<float, Tile[]> tileSets = new Dictionary<float, Tile[]>();

        public override Tile[] Tiles
        {
            get
            {
                return _tiles ?? GetTiles(_rli.Properties.FilePath);
            }
        }
       

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

        //private bool CheckTilesConsistency()
        //{
        //    return Math.Ceiling((double)(_rli.Width * _rli.Height) / (double)(TileSize.Width * TileSize.Height)) == Tiles.Length;
        //}



        protected override Tile[] GetTilesFromFile(string filePath)
        {
            pathCollection = InitTilePath(filePath);

            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);
                int strHeaderLength = 0;

                int signalDataLength = _rli.Width * bytesPerSample;

                //var lineHeight = (int)(TileSize.Height * paths.Keys.Max());
                for (int i = 0; i < Math.Ceiling((double)_rli.Height / (double)TileSize.Height); i++)
                {
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height);
                    tiles.AddRange(SaveTiles(tileLine, _rli.Width, i, TileSize));
                }
            }
            //GetResizedTiles(tiles.ToArray());
            return tiles.ToArray();
        }

        private byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, int tileHeight)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            float[] fLine = new float[line.Length / 4];
            byte[] normalizedLine = new byte[fLine.Length];

            int index = 0;

            while(index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }
            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);
            //rework normalization coef
            normalizedLine = fLine.AsParallel<float>().Select(x => (byte)(x / 255f)).ToArray<byte>();

            return normalizedLine;    
        }

        private float ComputeNormalizationCoef(int strDataLen, int strHeadLen)
        {
            byte[] arr = new byte[strDataLen + strHeadLen];
            float[] floatArr = new float[arr.Length / 4];
            float max = 0;
            float localMax = 0;
            float normal;
            using (var s = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);

                while (s.Read(arr, 0, arr.Length) != 0)
                {
                   // s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);
                    localMax = floatArr.Max();
                    max = max > localMax ? max : localMax;          
                }
            }   

            normal = 255f / max;
            return normal;
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
        //    //    }-
        //    //}


        //    //var resized = Resizer.ResizeImage(img, (int)(tile.Size.Width / zoomValue), (int)(tile.Size.Width / zoomValue));
        //    return null;
        //}

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

