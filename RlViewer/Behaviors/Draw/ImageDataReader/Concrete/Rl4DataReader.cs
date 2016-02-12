using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.Draw.Abstract;
using RlViewer.Behaviors.Draw.ImageDataReader.Abstract;

namespace RlViewer.Behaviors.Draw.ImageDataReader.Concrete
{
    class Rl4DataReader : DataReader
    {
        public Rl4DataReader(RliFile rli)
        {
            _rli = rli as Rl4;
            prop = new ParallelProperties(0, (int)(new System.IO.FileInfo(rli.Properties.FilePath).Length));
            //_normalCoef = ComputeNormalizationCoef(rli.Width * bytesPerSample, 256);
            
            InitTilePath();
            tileSize = new System.Drawing.Size(256, 256);
#if DEBUG
           // if (Directory.Exists(path)) Directory.Delete(path);
#endif

            _tiles = GetTiles();
        }

        private Dictionary<float, string> paths;
        


        private Rl4 _rli;
        private float _normalCoef;
        private int bytesPerSample = 4;//float - 4 байта на отсчет

        private RlViewer.ParallelProperties prop;
        private Size tileSize;

        private Tile[] _tiles;


        public override Tile[] Tiles
        {
            get
            {
                return _tiles ?? GetTiles();
            }

        }


        private void InitTilePath()
        {
            paths = new Dictionary<float, string>();
     
            paths.Add(0.25f, Path.Combine("tiles", "x0.0625"));
            paths.Add(0.5f,  Path.Combine("tiles", "x0.25"));
            paths.Add(1,     Path.Combine("tiles", "x1"));
            paths.Add(2,     Path.Combine("tiles", "x4"));
            paths.Add(4,     Path.Combine("tiles", "x16"));

            foreach (var i in paths)
            {
                Directory.CreateDirectory(i.Value);
            }
        }

        

        private Tile[] GetTiles()
        {

            List<Tile> tiles = new List<Tile>();
            using (var fs = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);
                int strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());

                int signalDataLength = _rli.Width * bytesPerSample;

                for (int i = 0; i < Math.Ceiling((double)_rli.Height / (double)tileSize.Height); i++)
                {
                    tiles.AddRange(SaveTiles(GetTileLine(fs, strHeaderLength, signalDataLength , tileSize.Height),
                        _rli.Width, i, tileSize.Width, tileSize.Height));                   
                }

            }
            GetResizedTiles(tiles.ToArray());
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

            normalizedLine = fLine.AsParallel<float>().Select(x => (byte)(x / 200f)).ToArray<byte>();
#if DEBUG
             GetGrayscaleBmp(normalizedLine, signalDataLength / 4, tileHeight).Save(DateTime.Now.Ticks + ".bmp");
#endif
            return normalizedLine;    
        }

        private float ComputeNormalizationCoef(int strDataLen, int strHeadLen)
        {

            byte[] arr = new byte[strDataLen + strHeadLen];
            float[] floatArr = new float[arr.Length / 4];
            float max = 0;

            float normal;
            using (var s = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);

                while (s.Position < s.Length)
                {
                    s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);
                    max = max < floatArr.Max() ? floatArr.Max() : max;                   
                }
            }
            normal = 255f / max;
            
            return normal;
        }


        private IEnumerable<Tile> SaveTiles(byte[] line, int linePixelWidth, int lineNumber, int tileWidth, int tileHeight)
        {
            byte[] tileData = new byte[tileWidth * tileHeight];
            List<Tile> tiles = new List<Tile>();

            using (var ms = new MemoryStream(line))
            {
                for (int i = 0; i < Math.Ceiling((double)line.Length / (double)(tileWidth * tileHeight)); i++)
                {
                    ms.Seek(i * tileWidth, SeekOrigin.Begin);
                    
                    for (int j = 0; j < tileHeight; j++)
                    {
                        ms.Read(tileData, j * tileWidth, tileWidth);
                        ms.Seek(linePixelWidth - tileWidth, SeekOrigin.Current);
                    }
 
                    tiles.Add(new Tile(SaveTileImage(Path.Combine(paths[1], i + "-" + lineNumber),
                        GetGrayscaleBmp(tileData, tileWidth, tileHeight)), 
                                          
                                       new PointF(i * tileWidth, lineNumber * tileHeight),
                                       new Size(tileWidth, tileHeight)));
                }
            }
            return tiles;
        }

        private Bitmap GetGrayscaleBmp(byte[] imgData, int tileWidth, int tileHeight)
        {
            Bitmap bmp = new Bitmap(tileWidth, tileHeight, PixelFormat.Format8bppIndexed);
            ColorPalette ncp = bmp.Palette;
            for (int i = 0; i < 256; i++)
                ncp.Entries[i] = Color.FromArgb(255, i, i, i);
            bmp.Palette = ncp;

            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            System.Runtime.InteropServices.Marshal.Copy(imgData, 0, ptr, imgData.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private Tile[] GetResizedTiles(Tile[] tiles)
        {
            foreach (var zooms in paths)
            {
                //we skip zoom value of 1 (normal picture) since it's already processed by GetTiles()
                if (zooms.Key < 1)
                {
                    foreach (var tile in tiles)
                    {
                        ResizeTileIn(tile, zooms.Key);
                    }
                }
                else if(zooms.Key > 1)
                {
                    foreach (var tile in tiles)
                    {
                        ResizeTileIn(tile, zooms.Key);
                    }
                }


            }
            return null;
        }


        Tile[] ResizeTileIn(Tile tile, float zoomValue)
        {
            var tiles = new List<Tile>();

                Bitmap littleTile = new Bitmap(tileSize.Width, tileSize.Height);
                var img = Image.FromFile(tile.FilePath);
                var resized = Resizer.ResizeImage(img, (int)(tile.Size.Width / zoomValue), (int)(tile.Size.Width / zoomValue));

                for (int i = 0; i < resized.Width; i += littleTile.Width)
                {
                    for (int j = 0; j < resized.Height; j += littleTile.Height)
                    {
                        var newPath = Path.Combine(paths[zoomValue], (tile.LeftTopCoord.X / zoomValue + i) / tileSize.Width + "-" +
                             (tile.LeftTopCoord.Y * zoomValue + j) / tileSize.Height);

                        var newBmp = resized.Clone(new Rectangle(new Point(i, j), littleTile.Size), PixelFormat.Format8bppIndexed);

                        tiles.Add(new Tile(SaveTileImage(newPath, newBmp),
                            new PointF(tile.LeftTopCoord.X + i * zoomValue, tile.LeftTopCoord.Y + j * zoomValue),
                            littleTile.Size));
                    }                  
                }
            
            return tiles.ToArray();
        }

        Tile[] ResizeTileOut(Tile[] tile, float zoomValue)
        {
            var tiles = new List<Tile>();

            Bitmap largeTile = new Bitmap(tileSize.Width * (int)zoomValue, tileSize.Height * (int)zoomValue);


            //for (int i = 0; i < resized.Width; i += littleTile.Width)
            //{
            //    for (int j = 0; j < resized.Height; j += littleTile.Height)
            //    {
            //        var newPath = Path.Combine(paths[zoomValue], (tile.LeftTopCoord.X / zoomValue + i) / tileSize.Width + "-" +
            //             (tile.LeftTopCoord.Y * zoomValue + j) / tileSize.Height);

            //        var newBmp = resized.Clone(new Rectangle(new Point(i, j), littleTile.Size), PixelFormat.Format8bppIndexed);

            //        tiles.Add(new Tile(SaveTileImage(newPath, newBmp),
            //            new PointF(tile.LeftTopCoord.X + i * zoomValue, tile.LeftTopCoord.Y + j * zoomValue),
            //            littleTile.Size));
            //    }
            //}




            var resized = Resizer.ResizeImage(img, (int)(tile.Size.Width / zoomValue), (int)(tile.Size.Width / zoomValue));

        }




        private string SaveTileImage(string path, Bitmap bmp)
        {
            path += ".bmp";
            bmp.Save(path);
            return path;
        }





    }
}
