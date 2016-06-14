using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Navigation
{
    public abstract class NavigationContainer : WorkerEventController, IEnumerable<NavigationString>
    {

        public abstract NavigationString this[int stringNumber] { get; }
        public abstract Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0] { get; }
        public abstract void GetNavigation();

        protected NavigationString[] NaviStrings;

        public IEnumerator<NavigationString> GetEnumerator()
        {
            return NaviStrings.AsEnumerable<NavigationString>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract RlViewer.Behaviors.Navigation.NavigationComputing Computer { get; }

        protected virtual T[] GetNaviStrings<T>(string path, int fileHeaderLength, int strDataLength) where T : struct
        {
            var header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            List<T> naviCollection = new List<T>();

            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(fileHeaderLength, SeekOrigin.Begin);

                while (fs.Position < fs.Length)
                {
                    naviCollection.Add(RlViewer.Files.LocatorFile.ReadStruct<T>(fs));
                    fs.Seek(strDataLength, SeekOrigin.Current);

                    OnProgressReport((int)((float)fs.Position / (float)fs.Length * 100));
                    if (OnCancelWorker())
                    {
                        return null;
                    }
                }
            }
            return naviCollection.ToArray();
        }

    }
}
