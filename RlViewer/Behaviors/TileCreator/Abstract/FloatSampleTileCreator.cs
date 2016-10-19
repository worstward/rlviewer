using System;
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

        protected abstract override float[] GetSampleData(byte[] sourceBytes);


        protected override float ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            OnReportName("Вычисление коэффициента нормировки");
            byte[] bRliString = new byte[strDataLen];
            float normal = 0;

            frameHeight = frameHeight > 1024 ? 1024 : frameHeight;

            long frameLength = loc.Header.FileHeaderLength + (strDataLen + strHeadLen) * frameHeight;

            var maxFrame = GetMaxValue(loc, strDataLen, strHeadLen, 0, frameHeight);

            float histogramStep = maxFrame / 1000f;         
            
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
                    s.Seek(strHeadLen, SeekOrigin.Current);
                    s.Read(bRliString, 0, bRliString.Length);

                    var fRliString = GetSampleData(bRliString);
                    avg += fRliString.Average(x => x);

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < fRliString.Length; i++)
                    {
                        int index = (int)(fRliString[i] / histogramStep);
                        if (index < 0) continue;

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

       
        protected override byte[] GetTileLine(Stream s, int strHeaderLength, int signalDataLength, 
            float normalizationFactor, int tileHeight, TileOutputType outputType)
        {
            byte[] line = new byte[signalDataLength * tileHeight];
            
            int index = 0;

   
            while (index != line.Length && s.Position != s.Length)
            {
                s.Seek(strHeaderLength, SeekOrigin.Current);
                index += s.Read(line, index, signalDataLength);
            }

            var fLine = GetSampleData(line);
            byte[] normalizedLine = new byte[fLine.Length];

            switch (outputType)
            {
                case TileOutputType.Linear:
                    normalizedLine = fLine.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearValue(x, normalizationFactor))).ToArray();
                    break;
                case TileOutputType.Logarithmic:
                    {
                        normalizedLine = fLine.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                            NormalizationHelpers.GetLogarithmicValue(x, MaxValue))).ToArray();
                        break;
                    }
                case TileOutputType.LinearLogarithmic:
                    {
                        normalizedLine = fLine.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearLogarithmicValue(x, MaxValue, normalizationFactor))).ToArray();
                        break;
                    }
                default:
                    break;

            }

            return normalizedLine;
        }
        
    }
}
