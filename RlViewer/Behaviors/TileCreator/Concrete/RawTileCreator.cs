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
        protected override Tile[] GetTilesFromFile(string filePath)
        {
            tileFolder = InitTilePath(filePath);

            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_rli.Header.FileHeaderLength, SeekOrigin.Begin);
                int strHeaderLength = 0;
                int signalDataLength = _rli.Width * _rli.Header.BytesPerSample;

                var totalLines = Math.Ceiling((double)_rli.Height / (double)TileSize.Height);
                for (int i = 0; i < totalLines; i++)
                {
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height, NormalizationFactor);
                    tiles.AddRange(SaveTiles(tileFolder, tileLine, _rli.Width, i, TileSize));

                    OnProgressReport((int)(i / totalLines * 100));
                    if (OnCancelWorker())
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
        protected override Tile[] GetTilesFromFileAsync(string filePath)
        {
            tileFolder = InitTilePath(filePath);

            Task.Run(() =>
            {
                List<Tile> tiles = new List<Tile>();
                byte[] tileLine;
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(_rli.Header.FileHeaderLength, SeekOrigin.Begin);
                    int strHeaderLength = 0;
                    int signalDataLength = _rli.Width * _rli.Header.BytesPerSample;

                    var totalLines = Math.Ceiling((double)_rli.Height / (double)TileSize.Height);
                    for (int i = 0; i < totalLines; i++)
                    {
                        tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height, NormalizationFactor);
                        SaveTiles(tileFolder, tileLine, _rli.Width, i, TileSize);
                    }
                }
            });
            return GetTilesFromTl(tileFolder);

        }
      
    }
}

