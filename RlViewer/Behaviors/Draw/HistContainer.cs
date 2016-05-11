using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Draw
{
    public class HistContainer
    {
        private long[] histogram = new long[256];

        public async Task<long[]> GetHistogramAsync(System.Drawing.Image img, int visibleWidth, int visibleHeight)
        {
            return await Task.Run(() =>
                {
                    
                    byte[] imgBytes = GetBytesFromBmp(img, visibleWidth, visibleHeight);

                    //var hist =  imgBytes.GroupBy(x => x).Select(x => new { Key = x.Key, Value = x.Count() });
                    Array.Clear(histogram, 0, histogram.Length);

                    for (int i = 0; i < imgBytes.Length; i++)
                    {
                        histogram[imgBytes[i]]++;
                    }

                    return histogram;
                });
        }

        public async Task<long[]> GetHistogramAsync(Files.LocatorFile file, TileCreator.Tile[] tiles)
        {
            return await Task.Run(() =>
                {
                    Array.Clear(histogram, 0, histogram.Length);
                    foreach (var tile in tiles)
                    {
                        var tileBytes = tile.ReadData();

                        for (int i = 0; i < tileBytes.Length; i++)
                        {
                            histogram[tileBytes[i]]++;
                        }
                        
                    }
                    return histogram;
                });
        }


        private byte[] GetBytesFromBmp(System.Drawing.Image img, int visibleWidth, int visibleHeight)
        {
            byte[] imgBytes = new byte[visibleWidth * 3 * visibleHeight];

            //since bmp format requests image width of (multiples of 4) we have to take padding into account
            var padding = img.Width - (int)(Math.Floor((double)(img.Width / 4f))) * 4;
            var bitmapHeaderOffset = 54;

            using (var ms = new System.IO.MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(bitmapHeaderOffset, System.IO.SeekOrigin.Begin);

                if (img.Height - visibleHeight > 0)
                {
                    //3 for rgb image (1 byte for each color channel)
                    ms.Seek((3 * img.Width + padding) * (img.Height - visibleHeight), System.IO.SeekOrigin.Current);
                }

                for (int i = 0; i < visibleHeight; i++)
                {
                    ms.Read(imgBytes, i * visibleWidth * 3, visibleWidth * 3);
                    ms.Seek(padding, System.IO.SeekOrigin.Current);
                    ms.Seek((img.Width - visibleWidth) * 3, System.IO.SeekOrigin.Current);
                }
            }
            return imgBytes;
        }

       
    }
}
