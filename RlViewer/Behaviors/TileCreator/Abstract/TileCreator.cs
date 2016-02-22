using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Draw;
using System.IO;

namespace RlViewer.Behaviors.TileCreator.Abstract
{
    public abstract class TileCreator
    {
        private System.Drawing.Size _tileSize = new System.Drawing.Size(512, 512);
        protected System.Drawing.Size TileSize
        {
            get { return _tileSize; }
        }

        public abstract Tile[] Tiles { get; }

        protected virtual string TileFileExtension
        {
            get
            {
                return ".tl";
            }
        }


        protected virtual Tile[] GetTiles(string filePath)
        {
            var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath));
            if (Directory.Exists(path))
            {
                return GetTilesFromTl(Path.Combine(path, "x1"));
            }
            return GetTilesFromFile(filePath);
        }

        protected abstract Tile[] GetTilesFromTl(string path);

        protected abstract Tile[] GetTilesFromFile(string path);

        


        protected virtual Dictionary<float, string> InitTilePath(string filePath)
        {
            Dictionary<float, string> paths = new Dictionary<float, string>();

            paths = new Dictionary<float, string>();
            paths.Add(0.25f, Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath), "x0.0625"));
            paths.Add(0.5f, Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath), "x0.25"));
            paths.Add(1, Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath), "x1"));
            paths.Add(2, Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath), "x4"));
            paths.Add(4, Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath), "x16"));

            foreach (var path in paths)
            {
                Directory.CreateDirectory(path.Value);
            }
            return paths;
        }

    }
}
