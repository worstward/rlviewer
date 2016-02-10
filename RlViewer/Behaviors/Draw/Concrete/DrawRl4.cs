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
using RlViewer.Behaviors.Draw.Abstract;

namespace RlViewer.Behaviors.Draw.Concrete
{
    class DrawRl4 : Drawing
    {
        public DrawRl4(RliFile rli)
            : base(rli)
        {
            _rli = rli as Rl4;
            _normalCoef = ComputeNormalizationCoef();
            path = Path.Combine("tiles", "x1");
            Directory.CreateDirectory(path);
            GetTiles();
        }

        Rl4 _rli;
        private float _normalCoef;
        private int bytesPerSample = 4;//float - 4 байта на отсчет
        private string path;


        public override Tile[] Tiles
        {
            get
            {
                return _tiles ?? GetTiles();
            }

        }

        private Tile[] GetTiles()
        {
            System.Drawing.Size tileSize = new System.Drawing.Size(256, 256);

            List<Tile> tiles = new List<Tile>();
            using (var fs = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_rli.Header.HeaderLength, SeekOrigin.Begin);

                int strHeaderLength = 256;//System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                byte[] imgData = new byte[_rli.Width * 4 * tileSize.Height];

                int signalDataLength = _rli.Width * 4;

                for (int i = 0; i < Math.Ceiling((double)(_rli.Height / tileSize.Height)) * tileSize.Height; i += tileSize.Height)
                {
                    tiles.AddRange(SaveTiles(GetTileLine(fs, strHeaderLength, signalDataLength, tileSize.Height),
                        signalDataLength, i, tileSize.Width, tileSize.Height));                   
                }

            }
            return null;
        }

        private byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, int tileHeight)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            float[] fLine = new float[line.Length / 4];
            byte[] normalizedLine = new byte[fLine.Length];

            int index = 0;
            while(index != line.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }
            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);
             normalizedLine = fLine.AsParallel<float>().Select(x => (byte)(x  * _normalCoef)).ToArray<byte>();

             GetGrayscaleBmp(normalizedLine, signalDataLength / 4, tileHeight).Save("a.bmp");

                return normalizedLine;    
        }

        private float ComputeNormalizationCoef()
        {

            byte[] arr = new byte[4096];
            float[] floatArr = new float[1024];
            float max = 0;

            float normal;
            using (var s = File.Open(_rli.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                while (s.Position < s.Length)
                {
                    s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, 0, floatArr, 0, arr.Length);
                    max = floatArr.Max();
                }
            }
            normal = 255f / max;
            
            return normal;
        }


        private IEnumerable<Tile> SaveTiles(byte[] line, int signalDataLength, int lineNumber, int tileWidth, int tileHeight)
        {
            byte[] tileData = new byte[tileWidth * tileHeight];
            List<Tile> tiles = new List<Tile>();

            using (var ms = new MemoryStream(line))
            {
                for (int i = 0; i < Math.Ceiling((double)(line.Length / (tileWidth * tileHeight))); i++)
                {
                    ms.Seek(i * tileWidth, SeekOrigin.Begin);
                    
                    for (int j = 0; j < tileHeight; j++)
                    {
                        ms.Read(tileData, j * tileWidth, tileWidth);
                        ms.Seek(signalDataLength / bytesPerSample, SeekOrigin.Current);
                    }
                    tiles.Add(new Tile(SaveTileImage(Path.Combine(path, i + "-" + lineNumber), 
                                           GetGrayscaleBmp(tileData, tileWidth, tileHeight)),
                                       new PointF(i, lineNumber),
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

        private string SaveTileImage(string path, Bitmap bmp)
        {
            bmp.Save(path + ".bmp");
            return path;
        }

        Tile[] _tiles;
        
    }
}
