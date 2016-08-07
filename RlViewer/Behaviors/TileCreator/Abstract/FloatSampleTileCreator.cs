﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using RlViewer.Behaviors.Draw;
using RlViewer.Files;


namespace RlViewer.Behaviors.TileCreator.Abstract
{
    public abstract class FloatSampleTileCreator : TileCreator<float>
    {
        public FloatSampleTileCreator(TileOutputType type) : base(type)
        { }


        protected override Tile[] GetTilesFromFile(string filePath, LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType)
        {
            if (file.Width == 0 || file.Height == 0)
            {
                throw new ArgumentException("File size");
            }

            var tileFolder = GetDirectoryName(filePath);
            CreateTileFolder(tileFolder);

            List<Tile> tiles = new List<Tile>();
            byte[] tileLine;
            using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(file.Header.FileHeaderLength, SeekOrigin.Begin);
                int signalDataLength = file.Width * file.Header.BytesPerSample;

                int strHeaderLength = 0;
                if (strHeader != null)
                {
                    strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(strHeader);
                }

                var totalLines = Math.Ceiling((double)file.Height / (double)TileSize.Height);
                for (int i = 0; i < totalLines; i++)
                {
                    tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height, outputType);
                    tiles.AddRange(SaveTiles(tileFolder, tileLine, file.Width, i, TileSize));
                    OnProgressReport((int)(i / totalLines * 100));
                    if (OnCancelWorker())
                    {
                        return null;
                    }
                }
            }
            return tiles.ToArray();
        }


        protected override Tile[] GetTilesFromFileAsync(string filePath, LocatorFile file,
            RlViewer.Headers.Abstract.IStrHeader strHeader, TileOutputType outputType)
        {
            if (file.Width == 0 || file.Height == 0)
            {
                return new Tile[0];
            }

            var tileFolder = GetDirectoryName(filePath);
            CreateTileFolder(tileFolder);


            Task.Factory.StartNew(() =>
            {
                List<Tile> tiles = new List<Tile>();
                byte[] tileLine;
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(file.Header.FileHeaderLength, SeekOrigin.Begin);

                    int strHeaderLength = 0;
                    if (strHeader != null)
                    {
                        strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(strHeader);
                    }

                    int signalDataLength = file.Width * file.Header.BytesPerSample;

                    var totalLines = Math.Ceiling((double)file.Height / (double)TileSize.Height);
                    for (int i = 0; i < totalLines; i++)
                    {
                        tileLine = GetTileLine(fs, strHeaderLength, signalDataLength, TileSize.Height, outputType);
                        SaveTiles(tileFolder, tileLine, file.Width, i, TileSize);
                    }
                }
            });
            return GetTilesFromTl(tileFolder);
        }

      

        protected override float ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            byte[] bRliString = new byte[strDataLen + strHeadLen];
            float[] fRliString = new float[strDataLen / sizeof(float)];
            float normal = 0;

            frameHeight = frameHeight > 1024 ? 1024 : frameHeight;

            long frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;

            MaxValue = GetMaxValue<float>(loc, strDataLen, strHeadLen, (arr) => { return arr.Max(); });

            float histogramStep = MaxValue / 1000f;
            var histogram = new List<int>();

            for (float i = 0; i < 1000; i += histogramStep)
            {
                histogram.Add(0);
            }

            using (var s = File.Open(loc.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                s.Seek(loc.Header.FileHeaderLength, SeekOrigin.Begin);

                float avg = 0;
                int parts = 0;

                while (s.Position != frameLength && s.Position != s.Length)
                {
                    parts++;

                    s.Read(bRliString, 0, bRliString.Length);
                    Buffer.BlockCopy(bRliString, strHeadLen, fRliString, 0, bRliString.Length - strHeadLen);

                    avg += fRliString.Average();

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < fRliString.Length; i++)
                    {
                        int index = (int)(fRliString[i] / histogramStep);
                        if(index < 0) continue;

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

                if ((int)normal == 0) normal = histogramStep;
                return normal;
            }
        }

       
        protected override byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, int tileHeight, TileOutputType outputType)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            float[] fLine = new float[line.Length / sizeof(float)];
            byte[] normalizedLine = new byte[fLine.Length];

            int index = 0;

            //if (normalizationFactor > MaxValue)
            //{
            //    MaxValue = normalizationFactor;
            //}

            float border = NormalizationFactor / 9f * 7;// *3;

           
            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }

            Buffer.BlockCopy(line, 0, fLine, 0, line.Length);

            switch (outputType)
            {
                case TileOutputType.Linear:
                    normalizedLine = fLine.AsParallel().Select(x => NormalizationHelpers.ToByteRange(x / NormalizationFactor * 255)).ToArray();
                    break;
                case TileOutputType.Logarithmic:
                    normalizedLine = fLine.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLogarithmicValue(x, MaxValue))).ToArray();
                    break;
                case TileOutputType.LinearLogarithmic:
                    normalizedLine = fLine.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearLogarithmicValue(x, border, MaxValue, NormalizationFactor))).ToArray();
                    break;
                default:
                    break;

            }

            return normalizedLine;
        }
        
    }
}
