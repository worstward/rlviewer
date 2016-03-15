using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Navigation
{
    public abstract class NavigationContainer
    {
        public abstract NavigationString this[int stringNumber] { get; }

        protected virtual T[] GetNaviStrings<T>(string path, int headerLength, int dataLength) where T : struct
        {
            var header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            List<T> naviCollection = new List<T>();

            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(headerLength, SeekOrigin.Begin);
                while (fs.Position < fs.Length)
                {
                    naviCollection.Add(RlViewer.Files.LocatorFile.ReadStruct<T>(fs));
                    fs.Seek(dataLength,SeekOrigin.Current);
                }
            }
            return naviCollection.ToArray();
        }

    }
}
