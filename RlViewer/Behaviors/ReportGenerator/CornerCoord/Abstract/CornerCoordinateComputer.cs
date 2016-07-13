using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Behaviors.ReportGenerator.CornerCoord.Abstract
{
    public abstract class CornerCoordinates
    {
        public CornerCoordinates(RlViewer.Files.LocatorFile file)
        {
            _file = file;
            _container = GetContainer();
        }

        RlViewer.Files.LocatorFile _file;

        protected RlViewer.Files.LocatorFile File
        {
            get { return _file; }
        }


        RlViewer.Navigation.NavigationContainer _container;
        protected abstract RlViewer.Navigation.NavigationContainer GetContainer();


        protected virtual T[] GetNaviStrings<T>(string path, int fileHeaderLength, int strDataLength) where T : struct
        {
            var header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            List<T> naviCollection = new List<T>();

            using (var fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(fileHeaderLength, SeekOrigin.Begin);
                naviCollection.Add(RlViewer.Files.LocatorFile.ReadStruct<T>(fs));
                fs.Seek(File.Width * File.Header.BytesPerSample, SeekOrigin.Current);

                for (int i = 0; i < File.Height - 2; i++)
                {
                    fs.Seek(File.Width * File.Header.BytesPerSample + File.Header.StrHeaderLength, SeekOrigin.Current);

                }

                naviCollection.Add(RlViewer.Files.LocatorFile.ReadStruct<T>(fs));    
            }

            return naviCollection.ToArray();
        }


        public IEnumerable<Tuple<string, string>> GetCoornerCoordinates()
        {
            var leftTop = _container[0, 0];
            var rightTop = _container[0, _file.Width - 1];
            var leftBottom = _container[1, 0];
            var rightBottom = _container[1, _file.Width - 1];


            var naviData = new List<Tuple<string, string>>();
            naviData.AddRange(leftTop.Where(x => x.Item1.Contains("Широта") || x.Item1.Contains("Долгота"))
                .Select(x => new Tuple<string, string>("Левый верхний угол, " + x.Item1.ToLowerInvariant(), x.Item2)));
            naviData.AddRange(rightTop.Where(x => x.Item1.Contains("Широта") || x.Item1.Contains("Долгота"))
                .Select(x => new Tuple<string, string>("Правый верхний угол, " + x.Item1.ToLowerInvariant(), x.Item2)));
            naviData.AddRange(leftBottom.Where(x => x.Item1.Contains("Широта") || x.Item1.Contains("Долгота"))
                .Select(x => new Tuple<string, string>("Левый нижний угол, " + x.Item1.ToLowerInvariant(), x.Item2)));
            naviData.AddRange(rightBottom.Where(x => x.Item1.Contains("Широта") || x.Item1.Contains("Долгота"))
                .Select(x => new Tuple<string, string>("Правый нижний угол, " + x.Item1.ToLowerInvariant(), x.Item2)));

            return naviData;
        }

    }
}
