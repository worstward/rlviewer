using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.IO.MemoryMappedFiles;
using RlViewer.Behaviors.Converters;
using RlViewer.Behaviors.Synthesis.ServerSarEmbedding;

namespace RlViewer.Behaviors.Synthesis
{
    class ServerSarInterop : WorkerEventController, IDisposable
    {
        public ServerSarInterop(string serverSarPath, string serverSarParams, ServerSarTaskParams sstp,
            Settings.SynthesisSettings synthSettings, RliFileCreator rliCreator, ServerSarEmbeddingProcessor embedder, bool useEmbeddedServerSar, bool forcedSynthesis)
        {
            
            _serverSarPath = serverSarPath;
            _serverSarParams = serverSarParams;
            _sstp = sstp;
            _rliCreator = rliCreator;
            _forcedSynthesis = forcedSynthesis;
            _events = new SystemEvents.SystemEvents(synthSettings.WaitTimeOut, sstp.RGG_RLI_DSP_numbers);
            _memoryParts = sstp.RGG_RLI_DSP_numbers;
            _embedder = embedder;
            _useEmbeddedServerSar = useEmbeddedServerSar;

            InitSynthesisEnvironment(sstp.RGG_RLI_DSP_numbers, synthSettings.SstpSharedMemoryName,
                synthSettings.ErrorSharedMemoryNameTemplate, synthSettings.DspSharedMemoryNameTemplate,
                synthSettings.HologramSharedMemoryNameTemplate, synthSettings.RliSharedMemoryNameTemplate);


        }

        public event EventHandler<int> ServerSarExited = delegate { };
        public event EventHandler<int> FrameAdded = delegate { };
        public event EventHandler<long> LengthChanged = delegate { };
        public ServerSarTaskParams Sstp
        {
            get { return _sstp; }
        }

        private string _serverSarPath;
        private string _serverSarParams;
        private string _serverSarSlaveDirectory = @"C:/vega/temp/obzor";
        private ServerSarTaskParams _sstp;
        private RliFileCreator _rliCreator;
        private bool _forcedSynthesis;
        private int _memoryParts;
        private SystemEvents.SystemEvents _events;
        private ServerSarEmbeddingProcessor _embedder;
        private bool _useEmbeddedServerSar;

        private RhgProcessor<Headers.Concrete.K.KStrHeaderStruct>[] _rhgProcessors;

        private MemoryMappedFile _sstpSharedMemory;
        private MemoryMappedFile[] _errorSharedMemory;
        private MemoryMappedFile[] _dspSharedMemory;
        private MemoryMappedFile[] _holSharedMemory;
        private MemoryMappedFile[] _rliSharedMemory;

        private MemoryMappedViewStream _sstpSharedMemoryStream;
        private MemoryMappedViewStream[] _errorSharedMemoryStream;
        private MemoryMappedViewStream[] _holSharedMemoryStream;
        private MemoryMappedViewStream[] _rliSharedMemoryStream;

        private Headers.Concrete.K.KStrHeaderStruct[][] _kHeaders;


        /// <summary>
        /// Synthesizes radiolocation image based on provided hologram series
        /// </summary>
        /// <param name="rhgSeries">Source holograms</param>
        /// <param name="rli">Image to write synthesized data in</param>
        /// <param name="showServerSar">True to show server sar console, otherwise false</param>
        public void Synthesize(Files.Rhg.Abstract.RhgFile[] rhgSeries, Files.LocatorFile rli, bool showServerSar)
        {
            OnNameReport("Инициализация синтеза");

            var sstpBytes = RlViewer.Behaviors.Converters.StructIO.WriteStruct<RlViewer.Behaviors.Synthesis.ServerSarTaskParams>(_sstp);
            _sstpSharedMemoryStream.Write(sstpBytes, 0, sstpBytes.Length);

            StartServerSar(showServerSar);
            _events.SstpReady();

            //skip 1st frame
            _events.HologramReady(0);
            _events.WaitRliReady(0);

            for (int i = 0; i < _memoryParts; i++)
            {
                _rhgProcessors[i] = new RhgProcessor<Headers.Concrete.K.KStrHeaderStruct>(rhgSeries, _sstp.Nlength * sizeof(short) * 2, _sstp.Mlength, _memoryParts);
            }

            Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Using {0} memory parts", _memoryParts));

            ReadRhgSeriesAsync(rhgSeries);
            WriteSynthesizedRli(rhgSeries, rli);
        }



        private void ReadRhgSeriesAsync(Files.Rhg.Abstract.RhgFile[] rhgSeries)
        {

            long rhgSeriesLength = rhgSeries.Sum(x => x.Properties.Length);
            var readerShift = (long)(_sstp.Mlength - _sstp.Mshift) * (rhgSeries[0].Header.StrHeaderLength + _sstp.Nlength * sizeof(short) * 2);
            bool lastFrame = true;

            Task.Run(() =>
            {
                int counter = 0;

                long rhgReaderPosition = 0;
                var index = counter % _memoryParts;

                var skippedLines = _sstp.AF_Do_Y_N ? 0 : (_sstp.Mlength - _sstp.Mshift) / 2;

                while (rhgReaderPosition < rhgSeriesLength)
                {
                    index = counter % _memoryParts;


                    Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Waiting for {0} rhg memory", index));

                    _events.WaitFreeMemoryPart();

                    Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("{0} rhg memory is free", index));

                    Array.Clear(_kHeaders[index], 0, _kHeaders[index].Length);

                    _kHeaders[index] = _rhgProcessors[index].ReadRhgToStream(rhgReaderPosition, _holSharedMemoryStream[index], skippedLines);
                    _events.HologramReady(index);

                    rhgReaderPosition += (_sstp.Mlength - skippedLines) * (_sstp.Nlength * (long)sizeof(short) * 2 + rhgSeries[0].Header.StrHeaderLength);

                    if (skippedLines > 0)
                    {
                        skippedLines -= _sstp.Mshift;
                        skippedLines = skippedLines < 0 ? 0 : skippedLines;
                    }

                    counter++;
                    if (rhgReaderPosition < rhgSeriesLength)
                    {
                        rhgReaderPosition -= readerShift;
                        rhgReaderPosition = rhgReaderPosition < 0 ? 0 : rhgReaderPosition;
                    }

                    if (rhgReaderPosition > rhgSeriesLength && lastFrame)
                    {
                        lastFrame = false;
                        rhgReaderPosition -= readerShift;
                    }
                }
            });

        }

        private void WriteSynthesizedRli(Files.Rhg.Abstract.RhgFile[] rhgSeries, Files.LocatorFile rli)
        {
            var blockSize = (_sstp.Nlength * (long)sizeof(short) * 2 + rhgSeries[0].Header.StrHeaderLength) * _sstp.Mlength;
            long rhgSeriesLength = rhgSeries.Sum(x => x.Properties.Length);
            var readerShift = (long)(_sstp.Mlength - _sstp.Mshift) * (rhgSeries[0].Header.StrHeaderLength + _sstp.Nlength * sizeof(short) * 2);
            var totalRliLines = (int)(rhgSeries.Sum(x => x.Height) / _sstp.Mscale);
            var totalFrames = (int)Math.Ceiling(rhgSeriesLength / (float)(blockSize - readerShift));

            var errorProcessor = new ErrorProcessor();
            var rliProcessor = new RliProcessor<Headers.Concrete.Rl4.Rl4StrHeaderStruct>(rli.Properties.FilePath, totalRliLines);

            for (int j = 0; j < totalFrames; j++)
            {
                int usedSharedMemoryIndex = j % _memoryParts;

                _rliSharedMemoryStream[usedSharedMemoryIndex].Position = 0;
                _errorSharedMemoryStream[usedSharedMemoryIndex].Position = 0;

                _events.WaitRliReady(usedSharedMemoryIndex);
                _events.ReleaseMemoryPart();

                errorProcessor.LogErrors(_errorSharedMemoryStream[usedSharedMemoryIndex]);

                if (_sstp.AF_Do_Y_N)
                {
                    OnNameReport("Автофокусировка изображения");
                }
                else
                {
                    OnNameReport("Синтез изображения");
                }

                var rliNavigation = _kHeaders[usedSharedMemoryIndex]
                    .Skip(_sstp.Mlength - _sstp.Mshift)
                    .Where((x, index) => index % _sstp.Mscale == 0)
                    .Select(x => x.ToRl4StrHeader()).ToArray();

                var frameHeight = rliProcessor.ProcessRli(_rliSharedMemoryStream[usedSharedMemoryIndex],
                    (int)(_sstp.Nshift / _sstp.Nscale), (int)(_sstp.Mshift / _sstp.Mscale), rliNavigation);

                FrameAdded(this, frameHeight);
                LengthChanged(this, frameHeight * (long)((_sstp.Mlength) * sizeof(float) + Marshal.SizeOf(rliNavigation.First())));
                OnCancelWorker();
                OnProgressReport((int)(j / (float)totalFrames * 100));
            }

        }

        private void InitSharedMemoryParts(int memoryParts, string sstpShMemName, string errShMemName, string dspShMemName, string holShMemName, string rliShMemName)
        {
            _errorSharedMemory = new MemoryMappedFile[memoryParts];
            _dspSharedMemory = new MemoryMappedFile[memoryParts];
            _holSharedMemory = new MemoryMappedFile[memoryParts];
            _rliSharedMemory = new MemoryMappedFile[memoryParts];

            _errorSharedMemoryStream = new MemoryMappedViewStream[memoryParts];
            _holSharedMemoryStream = new MemoryMappedViewStream[memoryParts];
            _rliSharedMemoryStream = new MemoryMappedViewStream[memoryParts];

            _rhgProcessors = new RhgProcessor<Headers.Concrete.K.KStrHeaderStruct>[memoryParts];
            _kHeaders = new Headers.Concrete.K.KStrHeaderStruct[_memoryParts][];


            _sstpSharedMemory = MemoryMappedFile.CreateNew(sstpShMemName, Marshal.SizeOf(typeof(Behaviors.Synthesis.ServerSarTaskParams)));

            for (int i = 0; i < memoryParts; i++)
            {
                _errorSharedMemory[i] = MemoryMappedFile.CreateNew(errShMemName + i, Marshal.SizeOf(typeof(ServerSarErrorMessage)));
                _dspSharedMemory[i] = MemoryMappedFile.CreateNew(dspShMemName + i, _sstp.dsp_zone_width * _sstp.Mlength);
                _holSharedMemory[i] = MemoryMappedFile.CreateNew(holShMemName + i, (long)_sstp.Mlength * _sstp.Nlength * sizeof(float) * 2);
                _rliSharedMemory[i] = MemoryMappedFile.CreateNew(rliShMemName + i, (long)(_sstp.Nshift / _sstp.Nscale) * (long)(_sstp.Mshift / _sstp.Mscale) * sizeof(float));

                _sstpSharedMemoryStream = _sstpSharedMemory.CreateViewStream();
                _errorSharedMemoryStream[i] = _errorSharedMemory[i].CreateViewStream();
                _holSharedMemoryStream[i] = _holSharedMemory[i].CreateViewStream();
                _rliSharedMemoryStream[i] = _rliSharedMemory[i].CreateViewStream();

                _kHeaders[i] = new Headers.Concrete.K.KStrHeaderStruct[_sstp.Mlength];
            }
        }

        private void InitSynthesisEnvironment(int memoryParts, string sstpShMemName, string errMesShMemName, string dspShMemName, string holShMemName, string rliShMemName)
        {
            var serverSarProc = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(_serverSarPath));

            if (_forcedSynthesis)
            {
                if (serverSarProc != null && serverSarProc.Length != 0)
                {
                    foreach (var proc in serverSarProc)
                    {
                        proc.Kill();
                    }
                }
            }

            if (!Directory.Exists(_serverSarSlaveDirectory))
            {
                Directory.CreateDirectory(_serverSarSlaveDirectory);
            }


            InitSharedMemoryParts(memoryParts, sstpShMemName, errMesShMemName, dspShMemName, holShMemName, rliShMemName);
        }


        private void StartServerSar(bool shellExecute)
        {
            if (_useEmbeddedServerSar)
            {
                _serverSarPath = _embedder.RebuildFromResources();
            }
            else
            {
                if (!File.Exists(_serverSarPath))
                {
                    throw new FileNotFoundException("Invalid server sar path");
                }
            }

            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    Arguments = _serverSarParams,
                    CreateNoWindow = !shellExecute,
                    FileName = _serverSarPath,
                    UseShellExecute = shellExecute,
                    RedirectStandardOutput = !shellExecute,
                    RedirectStandardError = !shellExecute
                },
                EnableRaisingEvents = true
            };

            process.ErrorDataReceived += (sender, args) => Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Server sar error: {0}", args.Data), type: Logging.LogType.Synthesis);
            process.OutputDataReceived += (sender, args) => Logging.Logger.Log(Logging.SeverityGrades.Synthesis, args.Data, type: Logging.LogType.Synthesis);
            process.Exited += (sender, args) =>
                {
                    ServerSarExited(this, process.ExitCode);
                };

            process.Start();

            if (!shellExecute)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

        }





        #region dispose
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                var serverSarProc = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(_serverSarPath)).FirstOrDefault();

                if (serverSarProc != null)
                {
                    serverSarProc.Kill();
                }

                if (_sstpSharedMemory != null)
                {
                    _sstpSharedMemory.Dispose();
                }

                if (_errorSharedMemory != null)
                {
                    foreach (var shMem in _errorSharedMemory)
                    {
                        shMem.Dispose();
                    }
                }

                if (_dspSharedMemory != null)
                {
                    foreach (var shMem in _dspSharedMemory)
                    {
                        shMem.Dispose();
                    }
                }

                if (_holSharedMemory != null)
                {
                    foreach (var shMem in _holSharedMemory)
                    {
                        shMem.Dispose();
                    }
                }

                if (_rliSharedMemory != null)
                {
                    foreach (var shMem in _rliSharedMemory)
                    {
                        shMem.Dispose();
                    }
                }

                if (_sstpSharedMemoryStream != null)
                {
                    _sstpSharedMemoryStream.Dispose();
                }

                if (_errorSharedMemoryStream != null)
                {
                    foreach (var stream in _errorSharedMemoryStream)
                    {
                        stream.Dispose();
                    }
                }
                if (_holSharedMemoryStream != null)
                {
                    foreach (var stream in _holSharedMemoryStream)
                    {
                        stream.Dispose();
                    }
                }
                if (_rliSharedMemoryStream != null)
                {
                    foreach (var stream in _rliSharedMemoryStream)
                    {
                        stream.Dispose();
                    }
                }

                if (_embedder != null)
                {
                    _embedder.Dispose();
                }
            }

            _disposed = true;

        }
        #endregion


    }
}
