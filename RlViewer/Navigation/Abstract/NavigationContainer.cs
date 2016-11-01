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

        public NavigationContainer(float initialRange, float step, byte flipType, int imageWidth)
        {
            _computer = new Behaviors.Navigation.NavigationComputing(initialRange, step, flipType, imageWidth);
        }

        public abstract NavigationString this[int stringNumber] { get; }
        public abstract Tuple<string, string>[] this[int stringNumber, int sampleNumber = 0] { get; }
        public abstract void GetNavigation();

        private NavigationString[] _naviStrings;

        public NavigationString[] NaviStrings
        {
            get { return _naviStrings; }
            set { _naviStrings = value; }
        }

        public IEnumerator<NavigationString> GetEnumerator()
        {
            return NaviStrings.AsEnumerable<NavigationString>().GetEnumerator();
        }

        public abstract NavigationString[] ConvertToCommonNavigation(Headers.Abstract.IStrHeader[] strCollection);


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private RlViewer.Behaviors.Navigation.NavigationComputing _computer;
        public RlViewer.Behaviors.Navigation.NavigationComputing Computer
        {
            get
            {
                return _computer;
            }
        }

        protected virtual T[] GetNaviStrings<T>(string path, int fileHeaderLength, int strDataLength) where T : struct
        {
            OnNameReport("Чтение навигации");
            var header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            List<T> naviCollection = new List<T>();

            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(fileHeaderLength, SeekOrigin.Begin);

                while (fs.Position < fs.Length)
                {
                    naviCollection.Add(RlViewer.Behaviors.Converters.StructIO.ReadStruct<T>(fs));
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
