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
    public abstract class ShortSampleTileCreator : TileCreator<short>
    {
        public ShortSampleTileCreator(TileOutputType type)
            : base(type)
        { }

        protected abstract override short[] GetSampleData(byte[] sourceBytes);

        protected override short ComputeNormalizationFactor(LocatorFile loc, int strDataLen, int strHeadLen, int frameHeight)
        {
            OnReportName("Вычисление коэффициента нормировки");
            byte[] bRliString = new byte[strDataLen];
            short normal = 0;

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

                    var rliString = GetSampleData(bRliString);
                    avg += (float)rliString.Average(x => x);

                    //fill histogram:
                    //count distinct float values, eg:
                    //numbers 1.4, 5, 6, 9, 24
                    //steps 1-10, 11-20
                    //1st step - 4 numbers, 2nd step 1 number
                    for (int i = 0; i < rliString.Length; i++)
                    {
                        int index = (int)(rliString[i] / histogramStep);
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

                normal = (short)((maxIndex + dst) * histogramStep);

                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Computed normalization value of {0}", normal));

                if ((int)normal == 0) normal = (short)histogramStep;
                return normal;
            }
        }



        protected override byte[] ProcessLinear(short[] samples)
        {
            return samples.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                            NormalizationHelpers.GetLinearValue(x, NormalizationFactor))).ToArray();
        }

        protected override byte[] ProcessLogarithmic(short[] samples)
        {
            return samples.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                            NormalizationHelpers.GetLogarithmicValue(x, MaxValue))).ToArray();
        }

        protected override byte[] ProcessLinLog(short[] samples)
        {
            return samples.AsParallel().Select(x => NormalizationHelpers.ToByteRange(
                        NormalizationHelpers.GetLinearLogarithmicValue(x, MaxValue, NormalizationFactor))).ToArray();
        }

    }
}
