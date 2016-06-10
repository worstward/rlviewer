using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using RlViewer.Files;
using RlViewer.Files.Rhg.Concrete;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;


namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class KTileCreator : TileCreator.Abstract.TileCreator, INormalizable
    {
        public KTileCreator(LocatorFile rhg, TileOutputType type)
            : base(type)
        {
            _rhg = rhg;
        }

        private LocatorFile _rhg;
        private float _normalFactor;


        private object _normalLocker = new object();
        public override float NormalizationFactor
        {
            get
            {
                //double lock checking
                if (_normalFactor == 0)
                {
                    lock (_normalLocker)
                    {
                        if (_normalFactor == 0)
                        {
                            _normalFactor = ComputeNormalizationFactor(_rhg, _rhg.Width * _rhg.Header.BytesPerSample,
                               0, Math.Min(_rhg.Height, 4096));
                        }
                    }
                }
                return _normalFactor;

            }
        }

        public override Tile[] Tiles
        {
            get { throw new NotImplementedException(); }
        }



        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rhg);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string filePath)
        {
            return GetTilesFromFile(filePath, _rhg, null, OutputType);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(string filePath)
        {
            return GetTilesFromFileAsync(filePath, _rhg, null, OutputType);
        }

    }
}
