using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
        }

        Rl4 _rli;
        
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

                int strHeaderSize = System.Runtime.InteropServices.Marshal.SizeOf(new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct());
                byte[] imgData = new byte[_rli.Width * 4 * tileSize.Height];

                int signalDataLength = _rli.Width * 4;
                int i = 0;

                while (fs.Position != fs.Length)
                {
                    fs.Seek(strHeaderSize, SeekOrigin.Begin);
                    i += fs.Read(imgData, i, signalDataLength);
                }


            }
            return null;
        }

        ///// <summary>
        ///// Reads as many rlStrings from rl file as a "tile width" number is
        ///// </summary>
        ///// <returns>byte array containing line</returns>
        //private byte[] GetTileLine(Stream s)
        //{
        //    throw new NotImplementedException();
        //}





        Tile[] _tiles;


    }
}
