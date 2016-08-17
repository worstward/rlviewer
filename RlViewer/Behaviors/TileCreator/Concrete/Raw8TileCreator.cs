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
using RlViewer.Files.Rli.Concrete;
using RlViewer.Behaviors.Draw;
using RlViewer.Behaviors.TileCreator.Abstract;


namespace RlViewer.Behaviors.TileCreator.Concrete
{
    class Raw8TileCreator : Rl8TileCreator
    {
        public Raw8TileCreator(LocatorFile rli, TileOutputType type)
            : base(rli, type)
        {
            _rli = rli;
        }

        private LocatorFile _rli;
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
            return GetTilesFromTl(directoryPath, _rli.Width, _rli.Height);
        }


        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.  Reports progress to backgroundworker object.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFile(string filePath)
        {
            return GetTilesFromFile(filePath, _rli, null, OutputType);
        }

        /// <summary>
        /// Saves tiles to local folder and creates tile objects array from Raw file.
        /// </summary>
        /// <returns></returns>
        protected override Tile[] GetTilesFromFileAsync(string filePath)
        {
            return GetTilesFromFileAsync(filePath, _rli, null, OutputType);
        }

    }
}
