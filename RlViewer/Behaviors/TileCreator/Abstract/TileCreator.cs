using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Behaviors.Draw;
using RlViewer.Files;


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


        public virtual Tile[] GetTilesReportProgress(string filePath, System.ComponentModel.BackgroundWorker worker)
        {
            var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath));
            Tile[] tiles;
            if (Directory.Exists(path))
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to get existing tiles");
                tiles = GetTilesFromTl(Path.Combine(path, "x1"));
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to create tiles from file");
                tiles = GetTilesFromFile(filePath, worker);
            }

            Logging.Logger.Log(Logging.SeverityGrades.Info,
                string.Format("Tile creation process succeed. {0} tiles generated", tiles.Length));
            return tiles;
        }

        protected virtual Tile[] GetTiles(string filePath)
        {
            var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath));
            Tile[] tiles;
            if (Directory.Exists(path))
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to get existing tiles");
                tiles = GetTilesFromTl(Path.Combine(path, "x1"));
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Attempting to create tiles from file");
                tiles = GetTilesFromFile(filePath);
            }

            Logging.Logger.Log(Logging.SeverityGrades.Info, 
                string.Format("Tile creation process succeed. {0} tiles generated", tiles.Length));
            return tiles;
        }




        protected abstract Tile[] GetTilesFromTl(string path);

        protected abstract Tile[] GetTilesFromFile(string path, System.ComponentModel.BackgroundWorker worker);
        protected abstract Tile[] GetTilesFromFile(string path);

        protected virtual float ComputeNormalizationCoef(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] arr = new byte[strDataLen + strHeadLen];
            float[] floatArr = new float[strDataLen / 4];

            int histogramStep = 1000;

            var histogram = new List<int>();


            for (int i = 0; i < 1000; i++)
            {
                histogram.Add(0);
            }

            float normal;

            int frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                float avg = 0;
                int parts = 0;

                while (s.Position != frameLength && s.Position != s.Length)
                {
                    parts++;

                    s.Read(arr, 0, arr.Length);
                    Buffer.BlockCopy(arr, strHeadLen, floatArr, 0, arr.Length - strHeadLen);
                    avg += floatArr.Average();

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < floatArr.Length; i++)
                    {
                        int index = (int)(floatArr[i] / histogramStep);
                        if (index >= histogram.Count)
                            histogram[histogram.Count - 1]++;
                        else histogram[index]++;
                    }
                }

                //find average value of samples array
                avg /= parts;

                //select max histogram value (most often occuring element)
                var max = histogram.Max();

                //get index of max histogram value
                var maxIndex = histogram.Where(x => x == max).Select((x, i) => i).FirstOrDefault();

                //find histogram index of average value sample and shift it
                var avgIndex = avg / histogramStep * 5;


                //get abs distance from max to avg values of histogram
                var dst = Math.Abs(maxIndex - avgIndex);

                normal = (maxIndex + dst) * histogramStep;

                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Computed normalization value of {0}", normal));

                if (normal == 0) normal = histogramStep;
                return 255f / normal;
            }
        }


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
