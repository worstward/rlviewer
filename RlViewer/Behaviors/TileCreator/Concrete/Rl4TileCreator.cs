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
    class Rl4TileCreator : ITileCreator
    {
        public Rl4TileCreator(LocatorFile rli)
        {
            _rli = rli as Rl4;
            //prop = new ParallelProperties(0, (int)(new System.IO.FileInfo(rli.Properties.FilePath).Length));
            //_normalCoef = ComputeNormalizationCoef(rli.Width * bytesPerSample, 256);
            tileSize = new System.Drawing.Size(128, 128);
            _tiles = GetTiles();
        }

        private Dictionary<float, string> pathCollection;

        private Rl4 _rli;
        private float _normalCoef;
        private int bytesPerSample = 4;//float - 4 байта на отсчет

        //private RlViewer.ParallelProperties prop;
        private Size tileSize;

        private Tile[] _tiles;
        private Dictionary<float, Tile[]> tileSets = new Dictionary<float, Tile[]>();

        public Tile[] Tiles
        {
            get
            {
                return _tiles ?? GetTiles();
            }
        }

        private Dictionary<float, string> InitTilePath()
        {

#if DEBUG
         //  if (Directory.Exists("tiles")) Directory.Delete("tiles", true);
#endif

            Dictionary<float, string> paths = new Dictionary<float, string>();

            paths = new Dictionary<float, string>();
            paths.Add(0.25f, Path.Combine("tiles", Path.GetFileNameWithoutExtension(_rli.Properties.FilePath), "x0.0625"));
            paths.Add(0.5f,  Path.Combine("tiles", Path.GetFileNameWithoutExtension(_rli.Properties.FilePath), "x0.25"));
            paths.Add(1,     Path.Combine("tiles", Path.GetFileNameWithoutExtension(_rli.Properties.FilePath), "x1"));
            paths.Add(2,     Path.Combine("tiles", Path.GetFileNameWithoutExtension(_rli.Properties.FilePath), "x4"));
            paths.Add(4,     Path.Combine("tiles", Path.GetFileNameWithoutExtension(_rli.Properties.FilePath), "x16"));

            foreach (var path in paths)
            {      
                Directory.CreateDirectory(path.Value);
            }
            return paths;
        }

       
        private Tile[] GetTiles()
        {
            var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(_rli.Properties.FilePath));
            if (Directory.Exists(path))
            {
                return GetTilesFromTl(Path.Combine(path, "x1"));
            }
            return GetTilesFromRl();
        }

        private Tile[] GetTilesFromTl(string directoryPath)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < _rli.Width; i += tileSize.Width)
            {
                for (int j = 0; j < _rli.Height; j += tileSize.Height)
                {
                    tiles.Add(new Tile(
                                Path.Combine(
                                directoryPath, (Math.Ceiling(i / (double)tileSize.Width)).ToString() +
                                "-" + Math.Ceiling(j / (double)tileSize.Height).ToString() + ".tl"),
                            new Point(i, j),
                            tileSize));   
                }
            }


                //foreach (var file in Directory.GetFiles(directoryPath))
                //{
                //    var coords = Path.GetFileNameWithoutExtension(file).Split('-');
                //    tiles.Add(new Tile(file, new Point(Convert.ToInt32(coords[0]) * tileSize.Width,
                //        Convert.ToInt32(coords[1]) * tileSize.Height), tileSize));
                //}
            return tiles.ToArray();
        }

        private bool CheckTilesConsistency()
        {
            return Math.Ceiling((double)(_rli.Width * _rli.Height) / (double)(tileSize.Width * tileSize.Height)) == Tiles.Length;
        }

   

        private Tile[] GetTilesFromRl()
        {
            pathCollection = InitTilePath();

            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);
                int strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());

                int signalDataLength = _rli.Width * bytesPerSample;

                //var lineHeight = (int)(tileSize.Height * paths.Keys.Max());
                for (int i = 0; i < Math.Ceiling((double)_rli.Height / (double)tileSize.Height); i++)
                {
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, tileSize.Height);
                    tiles.AddRange(SaveTiles(tileLine, _rli.Width, i, tileSize));
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
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);
                    localMax = floatArr.Max();
                    max = max > localMax ? max : localMax;          
                }
            }   

            normal = 255f / max;
            return normal;
        }


        private IEnumerable<Tile> SaveTiles(byte[] line, int linePixelWidth, int lineNumber, Size tileSize)
        {
            byte[] tileData = new byte[tileSize.Width * tileSize.Height];
            List<Tile> tiles = new List<Tile>();

            using (var ms = new MemoryStream(line))
            {
                for (int i = 0; i < Math.Ceiling((double)line.Length / tileData.Length); i++)
                {
                    ms.Seek(i * tileSize.Width, SeekOrigin.Begin);

                    for (int j = 0; j < tileSize.Height; j++)
                    {
                        ms.Read(tileData, j * tileSize.Width, tileSize.Width);
                        ms.Seek(linePixelWidth - tileSize.Width, SeekOrigin.Current);
                    }

                    tiles.Add(new Tile(SaveTile(Path.Combine(pathCollection[1], i + "-" + lineNumber), tileData),
                                       new Point(i * tileSize.Width, lineNumber * tileSize.Height),
                                       new Size(tileSize.Width, tileSize.Height)));
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

        //        Bitmap littleTile = new Bitmap(tileSize.Width, tileSize.Height);
        //        var img = Image.FromFile(tile.FilePath);
        //        var resized = Resizer.ResizeImage(img, (int)(tile.Size.Width / zoomValue), (int)(tile.Size.Width / zoomValue));

        //        for (int i = 0; i < resized.Width; i += littleTile.Width)
        //        {
        //            for (int j = 0; j < resized.Height; j += littleTile.Height)
        //            {
        //                var newPath = Path.Combine(paths[zoomValue], (tile.LeftTopCoord.X / zoomValue + i) / tileSize.Width + "-" +
        //                     (tile.LeftTopCoord.Y * zoomValue + j) / tileSize.Height);

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

        //    Bitmap largeTile = new Bitmap(tileSize.Width * (int)zoomValue, tileSize.Height * (int)zoomValue);


        //    //for (int i = 0; i < resized.Width; i += littleTile.Width)
        //    //{
        //    //    for (int j = 0; j < resized.Height; j += littleTile.Height)
        //    //    {
        //    //        var newPath = Path.Combine(paths[zoomValue], (tile.LeftTopCoord.X / zoomValue + i) / tileSize.Width + "-" +
        //    //             (tile.LeftTopCoord.Y * zoomValue + j) / tileSize.Height);

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
