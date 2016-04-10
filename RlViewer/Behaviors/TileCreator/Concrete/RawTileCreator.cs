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
    class RawTileCreator : RlViewer.Behaviors.TileCreator.Abstract.TileCreator, INormalizable
    {
        public RawTileCreator(LocatorFile rli)
        {
            _rli = rli;
        }

        private string tileFolder;

        private LocatorFile _rli;
        private float _normalFactor;

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
                        _tiles = _tiles ?? GetTiles(_rli.Properties.FilePath);
                    }
                }
                return _tiles;
            }
        }


        private object _locker = new object();
        public override float NormalizationFactor
        {
            get
            {
                if (_normalFactor == 0)
                {
                    lock (_locker)
                    {
                        if (_normalFactor == 0)
                        {
                            _normalFactor = ComputeNormalizationFactor(_rli, _rli.Width * _rli.Header.BytesPerSample,
                                0, Math.Min(_rli.Height, 4096));
                        }
                    }
                }
                return _normalFactor;
            }
        }


        protected override Tile[] GetTilesFromTl(string directoryPath)
        {
            return GetTilesFromTl(directoryPath, _rli);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string filePath)
        {
            return GetTilesFromFile(filePath, _rli, null);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(string filePath)
        {
            return GetTilesFromFileAsync(filePath, _rli, null);
        }
      
    }
}

