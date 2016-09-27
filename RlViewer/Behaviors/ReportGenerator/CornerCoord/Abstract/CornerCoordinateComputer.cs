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
        public CornerCoordinates(RlViewer.Files.LocatorFile file, int firstLine, int lastLine, bool readToEnd)
        {
            _file = file;
            _firstLine = firstLine;
            _lastLine = lastLine;
            _readToEnd = readToEnd;
        }

        private RlViewer.Files.LocatorFile _file;

        protected RlViewer.Files.LocatorFile File
        {
            get { return _file; }
        }

        private int _firstLine;
        private int _lastLine;
        private bool _readToEnd;


        protected abstract RlViewer.Navigation.NavigationContainer GetContainer(int firstLine, int lastLine, bool readToEnd);


        /// <summary>
        /// Gets first and last navigation strings from a selected file
        /// </summary>
        /// <typeparam name="T">String header type</typeparam>
        /// <param name="path">Path to locator file to read</param>
        /// <param name="fileHeaderLength">File header size in bytes</param>
        /// <param name="strDataLength">Data</param>
        /// <returns></returns>
        protected virtual T[] GetFirstAndLastNaviStrings<T>(string path, int fileHeaderLength, 
            int strDataLength,int firstLine, int lastLine, bool readToEnd) where T : Headers.Abstract.IStrHeader
        {
            if (readToEnd)
            {
                lastLine = _file.Height - 1;
            }

            if (firstLine >= _file.Height)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, "First line number is larger than file total lines count, reverting to 0");
                firstLine = 0;
            }

            if (lastLine >= _file.Height)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, "Last line number is larger than file total lines count, reverting to total line number");
                lastLine = _file.Height - 1;
            }


            var header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            List<T> naviCollection = new List<T>();

            using (var fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(fileHeaderLength, SeekOrigin.Begin);
                for (int i = 0; i < firstLine; i++)
                {
                    fs.Seek(File.Width * File.Header.BytesPerSample + File.Header.StrHeaderLength, SeekOrigin.Current);
                }

                naviCollection.Add(RlViewer.Behaviors.Converters.StructIO.ReadStruct<T>(fs));
                
                fs.Position = 0;
                fs.Seek(fileHeaderLength, SeekOrigin.Begin);

                for (int i = 0; i < lastLine; i++)
                {
                    fs.Seek(File.Width * File.Header.BytesPerSample + File.Header.StrHeaderLength, SeekOrigin.Current);
                }

                naviCollection.Add(RlViewer.Behaviors.Converters.StructIO.ReadStruct<T>(fs));    
            }

            return naviCollection.ToArray();
        }


        public IEnumerable<Tuple<string, string>> GetZoneStartAndEndTimes()
        {

            var container = GetContainer(_firstLine, _lastLine, _readToEnd);
            var Top = container[0];
            var Bottom = container[container.Count() - 1];

            var zoneStart = Top.TimeArm.ToLongTimeString();
            var zoneEnd = Bottom.TimeArm.ToLongTimeString();

            var lst = new List<Tuple<string, string>>();
            lst.Add(new Tuple<string, string>("Время начала зоны", zoneStart));
            lst.Add(new Tuple<string, string>("Время конца зоны", zoneEnd));

            return lst;
        }


        public IEnumerable<Tuple<string, string>> GetCenterCoordinates()
        {
            var container = GetContainer(_file.Height / 2, _file.Height - 1, _readToEnd);

            var center = container[0, _file.Width / 2];

            var centerTuple = (center.Where(x => x.Item1.Contains("Широта") || x.Item1.Contains("Долгота"))
                .Select(x => new Tuple<string, string>("Координаты центра изображения: " + x.Item1.ToLowerInvariant(), x.Item2)));

            return centerTuple;
        }



        public IEnumerable<Tuple<string, string>> GetCoornerCoordinates()
        {

            var container = GetContainer(_firstLine, _lastLine, _readToEnd);
            var leftTop = container[0, 0];
            var rightTop = container[0, _file.Width - 1];
            var leftBottom = container[container.Count() - 1, 0];
            var rightBottom = container[container.Count() - 1, _file.Width - 1];


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
