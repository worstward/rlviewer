using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Draw
{
    public class HistContainer
    {
        public async Task<int[]> GetHistogram(System.Drawing.Image img, int width, int height)
        {
            return await Task.Run(() =>
                {
                    byte[] imgBytes = GetBytesFromBmp(img);

                    //var hist =  imgBytes.GroupBy(x => x).Select(x => new { Key = x.Key, Value = x.Count() });
                    Array.Clear(histogram, 0, histogram.Length);

                    for (int i = 0; i < imgBytes.Length; i++)
                    {
                        histogram[imgBytes[i]]++;
                    }

                    return histogram;
                });
        }

        public byte[] GetBytesFromBmp(System.Drawing.Image img)
        {
            byte[] imgBytes;

            using (var ms = new System.IO.MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                imgBytes = ms.ToArray().Skip(54).ToArray();
            }
            return imgBytes;
        }



        private int[] histogram = new int[256];

       
    }
}
