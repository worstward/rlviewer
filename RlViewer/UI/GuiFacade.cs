using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using RlViewer.Behaviors.TileCreator.Abstract;
using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Factories.File.Abstract;
using RlViewer.Factories.Saver.Abstract;
using RlViewer.Navigation;
using RlViewer.Settings;
using RlViewer.Behaviors;
using RlViewer.Files;

namespace RlViewer.UI
{
    public class GuiFacade : IDisposable, INotifyPropertyChanged
    {
        public GuiFacade(Size canvasSize, Action<Action> synchronizer)
        {
            //var a = new Behaviors.Synthesis.SharedMemory.MappedFile();

            LoadSettings();
            TryRunAsAdmin(_settings.ForceAdminMode);

            CanvasSize = canvasSize;
            _synchronizer = synchronizer;
            _filterProxy = new Behaviors.Filters.ImageFilterProxy();
            _scaler = new Behaviors.Scaling.Scaler(_settings.MinScale, _settings.MaxScale, _settings.InitialScale);
            _drag = new Behaviors.DragController();
            //_chart = new Forms.ChartHelper(new Behaviors.Draw.HistContainer());

            Deinitialize();
        }

        private Settings.Settings _settings;
        private ITileCreator _creator;

        private Behaviors.Filters.ImageFilterProxy _filterProxy;
        private Behaviors.Saving.Abstract.Saver _saver;
        private Behaviors.Scaling.Scaler _scaler;
        private Behaviors.Analyzing.SampleAnalyzer _analyzer;
        private Behaviors.Ruler.RulerProxy _ruler;
        private Behaviors.ImageAligning.Aligning _aligner;
        private Behaviors.Sections.Abstract.Section _section;
        private Behaviors.Navigation.NavigationSearcher.Abstract.GeodesicPointFinder _searcher;
        private Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer _pointSharer;
        private Behaviors.ReportGenerator.Abstract.Reporter _reporter;
        private Behaviors.FilesAggregator.LocatorFilesAggregator _aggregator = new Behaviors.FilesAggregator.LocatorFilesAggregator();
        private Behaviors.ImageMirroring.Abstract.ImageMirrorer _mirrorer;
        private WorkerEventController _cancellableAction;

        private Files.FileProperties _properties;
        private Headers.Abstract.LocatorFileHeader _header;
        private Navigation.NavigationContainer _navi;
        //private Forms.ChartHelper _chart;

        private Files.LocatorFile _file;
        private Behaviors.TileCreator.Tile[] _tiles;
        private Behaviors.Draw.DrawerFacade _drawer;
        private Behaviors.PointSelector.PointSelector _pointSelector;
        private Behaviors.AreaSelector.AreaSelectorDecorator _selectedPointArea;
        private Behaviors.AreaSelector.AreaSelectorsAlignerContainer _areaAligningWrapper;


        private Behaviors.AreaSelector.AreaSelector _areaSelector;
        private Behaviors.DragController _drag;


        private System.ComponentModel.BackgroundWorker _worker;
        private string _caption = string.Empty;
        private ToolTip _toolTip = new ToolTip();
        private Action<Action> _synchronizer;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event EventHandler<Image> OnImageDrawn = delegate { };
        public event EventHandler OnPointOfViewMaxChanged = delegate { };
        public event EventHandler<Events.ProgressControlsVisibilityEventArgs> OnProgressVisibilityChanged = delegate { };
        public event EventHandler<Events.AlignPossibilityEventArgs> OnAlignPossibilityChanged = delegate { };
        public event EventHandler<Events.ProgressChangedEventArgs> OnProgressChanged = delegate { };
        public event EventHandler<Events.ErrorOccuredEventArgs> OnErrorOccured = delegate { };
        private object _animationLock = new object();


        private List<NavigationItem> _navigationList = new List<NavigationItem>();

        private bool _allowRemoteDataReceiving;

        public bool AllowRemoteDataReceiving
        {
            get
            {
                return _allowRemoteDataReceiving; 
            }
            set 
            {
                _allowRemoteDataReceiving = value;
            }
        }


        public Size CanvasSize
        {
            get;
            set;
        }



        public float ScaleFactor
        {
            get
            {
                return _scaler.ScaleFactor;
            }
            set
            {
                SetField(ref _scaler, new Behaviors.Scaling.Scaler(_scaler.MinZoom, _scaler.MaxZoom, value));
            }
        }

        private string _rulerDistance;

        public string RulerDistance
        {
            get
            {
                return _rulerDistance;
            }
            set
            {
                SetField(ref _rulerDistance, value);
            }
        }


        private string _currentTaskName;

        public string CurrentTaskName
        {
            get
            {
                return _currentTaskName;
            }
            set
            {
                SetField(ref _currentTaskName, value);
            }
        }

        private int _progressBarValue;

        public int ProgressBarValue
        {
            get
            {
                return _progressBarValue; 
            }
            set
            {
                SetField(ref _progressBarValue, value);    
            }
        }


        private int _xPointOfViewMax;

        public int XPointOfViewMax
        {
            get
            {
                return _xPointOfViewMax;
            }
            set
            {
                if (SetField(ref _xPointOfViewMax, value))
                {
                    OnPointOfViewMaxChanged(null, null);
                }
            }
        }


        private int _yPointOfViewMax;

        public int YPointOfViewMax
        {
            get
            {
                return _yPointOfViewMax;
            }
            set
            {
                if (SetField(ref _yPointOfViewMax, value))
                {
                    OnPointOfViewMaxChanged(null, null);
                }
            }
        }




        private int _xPointOfView;
        public int XPointOfView
        {
            get
            {
                return _xPointOfView;
            }
            set
            {
                SetField(ref _xPointOfView, value);
            }
        }

        private int _yPointOfView;
        public int YPointOfView
        {
            get
            {
                return _yPointOfView;
            }
            set
            {
                SetField(ref _yPointOfView, value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                _synchronizer.Invoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
        protected bool SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }



        #region openFile
        public string OpenWithDoubleClick()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length > 10)
            {
                return string.Empty;
            }

            var fileExts = Enum.GetNames(typeof(FileType)).Except(new List<string>() { "bmp" });

            //get first filepath that has supported extension
            var fName = args.Where(x => fileExts.Any(Path.GetExtension(x).Contains)).FirstOrDefault();
            return fName == null ? string.Empty : OpenFile(fName);
        }

        public string OpenFileCustomDialog()
        {
            using (var previewForm = new Forms.FilePreviewForm())
            {
                if (previewForm.ShowDialog() == DialogResult.OK)
                {
                    return OpenFile(previewForm.FileToOpen);
                }

                return _caption;
            }
        }

        public string OpenFileDragDrop(DragEventArgs e)
        {
            if (MoveFileDragDrop(e))
            {
                var fileName = ((string[])e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
                return OpenFile(fileName);
            }
            return string.Empty;
        }

        public bool MoveFileDragDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var ext = System.IO.Path.GetExtension(((string[])e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault());
                try
                {
                    ext.Replace(".", "").ToEnum<FileType>();
                    e.Effect = DragDropEffects.Copy;
                    return true;
                }
                catch
                {
                    e.Effect = DragDropEffects.None;
                    return false;
                }
            }
            return false;
        }


        public string OpenFile()
        {
            string caption;

            if (!_settings.UseCustomFileOpenDlg)
            {
                using (var openFileDlg = new OpenFileDialog() { Filter = Resources.OpenFilter })
                {
                    openFileDlg.Title = "Радиолокационный файл";
                    if (openFileDlg.ShowDialog() == DialogResult.OK)
                    {
                        return OpenFile(openFileDlg.FileName);
                    }
                    else
                    {
                        caption = _caption;
                    }

                    return caption;
                }
            }
            else
            {
                return OpenFileCustomDialog();
            }
        }

        private string OpenFile(string fileName)
        {
            if (ConfirmTaskStart(loaderWorker_InitFile, loaderWorker_InitFileCompleted, fileName))
            {
                Deinitialize(true);
                _caption = fileName;
                return _caption;
            }


            if (_file != null)
            {
                return _file.Properties.FilePath;
            }

            return string.Empty;
        }
        #endregion

        #region tasks


        private bool ConfirmTaskStart(System.ComponentModel.DoWorkEventHandler workerCallBack,
            System.ComponentModel.RunWorkerCompletedEventHandler afterTaskCallback, object arg = null)
        {
            if (_worker != null && _worker.IsBusy)
            {
                var confirmation = MessageBox.Show("Вы уверены, что хотите отменить выполняемую операцию?",
                                "Подтвердите отмену",
                                MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    StartTask(workerCallBack, afterTaskCallback, arg);
                    return true;
                }
            }
            else
            {
                StartTask(workerCallBack, afterTaskCallback, arg);
                return true;
            }

            return false;
        }



        /// <summary>
        /// Starts backgroundWorker with provided callbacks
        /// </summary>
        /// <param name="workerCallBack"></param>
        /// <param name="afterTaskCallback"></param>
        /// <param name="arg"></param>
        /// <returns>True if task started, false otherwise</returns>
        private void StartTask(System.ComponentModel.DoWorkEventHandler workerCallBack,
            System.ComponentModel.RunWorkerCompletedEventHandler afterTaskCallback, object arg = null)
        {
            CancelLoading();
            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(true));
            _worker = ThreadHelper.InitWorker(workerCallBack, afterTaskCallback);
            _worker.RunWorkerAsync(arg);
        }

        /// <summary>
        /// Cancels current backgroundworker task
        /// </summary>
        public void CancelLoading()
        {
            if (_worker != null && _cancellableAction != null)
            {
                _cancellableAction.Cancelled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposer">Action that clears all redundant data after task cancelling</param>
        private void ClearWorkerData(Action disposer)
        {
            if (_worker != null)
            {
                try
                {
                    disposer();
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                    OnErrorOccured(this, new Events.ErrorOccuredEventArgs(ex.Message));
                }
            }

            _worker.Dispose();
        }


        #region findPointWorkerMethods
        private void loaderWorker_FindPoint(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            _searcher.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;// OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _searcher.CancelJob += (s, ce) => ce.Cancel = _searcher.Cancelled;
            _searcher.ReportName += (s, tne) => CurrentTaskName = tne.Name;

            _cancellableAction = _searcher;

            var searcherParams = (Behaviors.Navigation.NavigationSearcher.SearcherParams)e.Argument;

            try
            {
                if (searcherParams.IsNavigationSearcher)
                {
                    e.Result = _searcher.GetCoordinates(searcherParams.Lat, searcherParams.Lon, searcherParams.Error);
                }
                else
                {
                    e.Result = new Point(searcherParams.X, searcherParams.Y);
                }
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
        }

        private void loaderWorker_FindPointCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _searcher.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _searcher.CancelJob -= (s, ce) => ce.Cancel = _searcher.Cancelled;
            _searcher.ReportName -= (s, tne) => CurrentTaskName = tne.Name;

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Searching cancelled"));

            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Error searching point: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно найти точку"));
            }
            else
            {
                CenterImageAtPoint((Point)e.Result, true);
            }

            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }
        #endregion

        #region NormalizeFileWorkerMethods
        private void loaderWorker_NormalizeFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _saver = SaverFactory.GetFactory(_file.Properties).Create(_file);

            _saver.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _saver.CancelJob += (s, ce) => ce.Cancel = _saver.Cancelled;
            _saver.ReportName += (s, tne) => CurrentTaskName = tne.Name;
            _cancellableAction = _saver;

            var fileName = (string)e.Argument;

            try
            {
                var targetPoint = _pointSelector.First();
                var normalizationCoef = targetPoint.Value / targetPoint.Rcs;

                _saver.SaveAsNormalized(fileName, normalizationCoef);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }

            if (_saver.Cancelled)
            {
                ClearWorkerData(() => File.Delete(fileName));
            }

            e.Result = fileName;
        }

        private void loaderWorker_NormalizeFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _saver.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _saver.CancelJob -= (s, ce) => ce.Cancel = _saver.Cancelled;
            _saver.ReportName -= (s, tne) => CurrentTaskName = tne.Name;

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Normalization cancelled"));
                string errMess = string.Format("Normalization cancelled");
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while normalizing image: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно выполнить нормировку"));
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Normalization completed: {0}", (string)e.Result));
            }

            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }
        #endregion

        #region saveFileWorkerMethods

        private void loaderWorker_SaveFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Behaviors.Saving.SaverParams parameters;

            try
            {
                parameters = (Behaviors.Saving.SaverParams)e.Argument;
            }
            catch (InvalidCastException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Internal, "Invalid saver params");
                throw;
            }

            _saver = SaverFactory.GetFactory(_file.Properties).Create(_file);

            _saver.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _saver.CancelJob += (s, ce) => ce.Cancel = _saver.Cancelled;
            _saver.ReportName += (s, tne) => CurrentTaskName = tne.Name;

            float maxSampleValue = 0;



            try
            {
                if (parameters.OutputType != Behaviors.TileCreator.TileOutputType.Linear)
                {
                    _cancellableAction = ((WorkerEventController)_creator);
                    maxSampleValue = _creator.MaxValue;
                }
                _cancellableAction = _saver;

                _saver.Save(parameters, _creator.NormalizationFactor, maxSampleValue);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }

            if (_saver.Cancelled)
            {
                ClearWorkerData(() => File.Delete(parameters.Path));
            }

            e.Result = parameters.Path;
        }

        private void loaderWorker_SaveFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _saver.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _saver.CancelJob -= (s, ce) => ce.Cancel = _saver.Cancelled;
            _saver.ReportName -= (s, tne) => CurrentTaskName = tne.Name;

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Saving cancelled"));
                string errMess = string.Format("Saving cancelled");
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while saving image: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно выполнить сохранение"));
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Saving completed: {0}", (string)e.Result));
            }

            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }
        #endregion

        #region alignImageWorkerMethods

        private void loaderWorker_AlignImage(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var fileName = (string)e.Argument;

            _aligner.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _aligner.CancelJob += (s, ce) => ce.Cancel = _aligner.Cancelled;
            _aligner.ReportName += (s, tne) => CurrentTaskName = tne.Name;

            _cancellableAction = _aligner;

            var resamplingArea = GeometryHelper.GetArea(_pointSelector, _settings.AligningAreaBorderSize);

            byte[] resampledImage = null;

            try
            {
                resampledImage = _aligner.Resample(fileName, resamplingArea);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }

            if (resampledImage != null)
            {
                _saver.SaveAsAligned(fileName, resamplingArea, resampledImage, _pointSelector.Count(),
                      (int)_settings.RangeCompressionCoef, (int)_settings.AzimuthCompressionCoef);
            }

            e.Result = fileName;
        }

        private void loaderWorker_AlignImageCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _aligner.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            _aligner.CancelJob -= (s, ce) => ce.Cancel = _aligner.Cancelled;


            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Image aligning cancelled"));
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while aligning image: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно выполнить калибровку"));
            }
            else
            {
                var fileName = Path.GetFullPath((string)e.Result);
                fileName = Path.ChangeExtension(fileName, "brl4");

                var type = new Files.FileProperties(fileName).Type;
                if (type != FileType.raw && type != FileType.r)
                {
                    EmbedNavigation(fileName, false);
                }

                Logging.Logger.Log(Logging.SeverityGrades.Info,
                    string.Format("Image aligning completed, new file saved at: {0}", fileName));
            }
            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }
        #endregion

        #region initFileWorkerMethods

        private void loaderWorker_InitFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                _properties = new Files.FileProperties((string)e.Argument);
                _header = Factories.Header.Abstract.HeaderFactory.GetFactory(_properties).Create(_properties.FilePath);

                _navi = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(_properties).Create(_properties, _header);

                _navi.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _navi.CancelJob += (s, ce) => ce.Cancel = _navi.Cancelled;
                _navi.ReportName += (s, tne) => CurrentTaskName = tne.Name;

                _cancellableAction = _navi;
                _navi.GetNavigation();
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
                return;
            }

            _file = FileFactory.GetFactory(_properties).Create(_properties, _header, _navi);

            _ruler = new Behaviors.Ruler.RulerProxy(_file);
            _saver = SaverFactory.GetFactory(_properties).Create(_file);
            _searcher = Factories.NavigationSearcher.Abstract.PointFinderFactory.GetFactory(_file.Properties).Create(_file);
            _analyzer = new Behaviors.Analyzing.SampleAnalyzer();
            _pointSelector = new Behaviors.PointSelector.PointSelector();
            _areaSelector = new Behaviors.AreaSelector.AreaSelector(_file);
            _areaAligningWrapper = new Behaviors.AreaSelector.AreaSelectorsAlignerContainer();

            if (_pointSharer != null)
            {
                _pointSharer.DataReceived -= (s, gde) => CrossAppDataReceivedCallback(s, gde);
                _pointSharer.Dispose();
            }

            _pointSharer = Factories.PointSharer.Abstract.PointSharerFactory.GetFactory(_file.Properties).Create(_file,
                 new Behaviors.CrossAppCommunication.UdpExchange(_settings.MulticastEp, Behaviors.CrossAppCommunication.CommunicationType.Multicast),
                 System.Diagnostics.Process.GetCurrentProcess().Id);

            _pointSharer.DataReceived += (s, gde) => CrossAppDataReceivedCallback(s, gde);
        }

        private void loaderWorker_InitFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_navi != null)
            {
                _navi.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _navi.CancelJob -= (s, ce) => ce.Cancel = _navi.Cancelled;
                _navi.ReportName -= (s, tne) => CurrentTaskName = tne.Name;
            }


            if (_settings.ForceImageHeightAdjusting && _file != null && _file.Navigation != null)
            {
                if (_file.Navigation.Count() != _file.Height)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Warning, "Incorrect height detected, reverting to navigation strings count");
                    _file.SetHeight(_file.Navigation.Count());
                }
            }


            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("File opening cancelled"));
                Deinitialize();
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while opening file: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно открыть файл"));
                Deinitialize();
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("{0} file opened: {1}",
                    _file.Properties.Type, _file.Properties.FilePath));

                ConfirmTaskStart(loaderWorker_CreateTiles, loaderWorker_CreateTilesCompleted);
            }
        }
        #endregion

        #region createTilesWorkerMethods

        private void loaderWorker_CreateTiles(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _creator = TileCreatorFactory.GetFactory(_file).Create(_file as RlViewer.Files.LocatorFile, _settings.TileOutputAlgorithm);

            ((WorkerEventController)_creator).Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
            ((WorkerEventController)_creator).CancelJob += (s, ce) => ce.Cancel = ((WorkerEventController)_creator).Cancelled;
            ((WorkerEventController)_creator).ReportName += (s, tne) =>
                CurrentTaskName = tne.Name;

            _cancellableAction = ((WorkerEventController)_creator);

            try
            {
                e.Result = _creator.GetTiles(_file.Properties.FilePath, _settings.ForceTileGeneration, _settings.AllowViewWhileLoading);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
            catch (AggregateException aggrex)
            {
                if (aggrex.InnerException.GetType() == typeof(OperationCanceledException))
                {
                    e.Cancel = true;
                }
            }
        }


        private void loaderWorker_CreateTilesCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_creator != null)
            {
                ((WorkerEventController)_creator).Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                ((WorkerEventController)_creator).CancelJob -= (s, ce) => ce.Cancel = ((WorkerEventController)_creator).Cancelled;
                ((WorkerEventController)_creator).ReportName -= (s, tne) =>
                    CurrentTaskName = tne.Name;

            }

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Tile creation cancelled"));
                ClearWorkerData(() => _creator.ClearCancelledFileTiles(_file.Properties.FilePath));
                Deinitialize();
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error creating tiles: {0}",
                e.Error.InnerException == null ? e.Error.Message : e.Error.InnerException.Message));
                ClearWorkerData(() => _creator.ClearCancelledFileTiles(_file.Properties.FilePath));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Ошибка генерации изображения из файла"));
                Deinitialize();

            }
            else
            {
                _tiles = (Behaviors.TileCreator.Tile[])e.Result;

                Logging.Logger.Log(Logging.SeverityGrades.Info,
                    string.Format("Tile creation process succeed. {0} {1} generated", _tiles.Length, _tiles.Length == 1 ? "tile" : "tiles"));

                if (!_creator.CheckTileConsistency(_file.Properties.FilePath, _tiles.Length))
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("Missing tiles for {0} detected", _file.Properties.FilePath));
                }

                OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
                InitDrawImage();
            }

        }
        #endregion

        #region generateReportWorkerMethods

        private void loaderWorker_GenerateReport(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_reporter != null)
            {
                _reporter.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _reporter.CancelJob += (s, ce) => ce.Cancel = _reporter.Cancelled;
                _reporter.ReportName += (s, tne) =>
                    CurrentTaskName = tne.Name;
            }

            _cancellableAction = _reporter;

            var reportArgs = (object[])e.Argument;
            var reportFileName = (string)reportArgs[0];
            var reporterSettings = (Behaviors.ReportGenerator.ReporterSettings)reportArgs[1];


            try
            {
                _reporter.GenerateReport(reportFileName, reporterSettings);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
            e.Result = reportFileName;
        }


        private void loaderWorker_GenerateReportCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_reporter != null)
            {
                _reporter.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _reporter.CancelJob -= (s, ce) => ce.Cancel = _reporter.Cancelled;
                _reporter.ReportName -= (s, tne) =>
                    CurrentTaskName = tne.Name;
            }

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Report generation cancelled"));
                OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Unable to make report: {0}", e.Error.Message));
                Deinitialize();
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info,
                        string.Format("Report file generated: {0}", (string)e.Result));
            }

            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }

        #endregion

        #region aggregateFilesWorkerMethods

        private void loaderWorker_AggregateFiles(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_aggregator != null)
            {
                _aggregator.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _aggregator.CancelJob += (s, ce) => ce.Cancel = _aggregator.Cancelled;
                _aggregator.ReportName += (s, tne) =>
                    CurrentTaskName = tne.Name;
            }

            _cancellableAction = _aggregator;

            var aggregatorParams = (Behaviors.FilesAggregator.AggregatorParams)e.Argument;

            try
            {
                _aggregator.Aggregate(aggregatorParams.AggregateFileName, aggregatorParams.SourceFilesNames);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
                ClearWorkerData(
                    () =>
                    {
                        if (File.Exists(aggregatorParams.AggregateFileName))
                        {
                            File.Delete(aggregatorParams.AggregateFileName);
                        }
                    });
            }
            e.Result = aggregatorParams.AggregateFileName;
        }

        private void loaderWorker_AggregateFilesCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_aggregator != null)
            {
                _aggregator.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _aggregator.CancelJob -= (s, ce) => ce.Cancel = _aggregator.Cancelled;
                _aggregator.ReportName -= (s, tne) =>
                    CurrentTaskName = tne.Name;
            }

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Files aggregation cancelled"));
                OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Unable to aggregate files: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно совместить файлы"));
                Deinitialize();
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info,
                        string.Format("Aggregation completed: {0}", (string)e.Result));
            }

            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }

        #endregion

        #region mirrorImageWorkerMethods

        private void loaderWorker_MirrorImage(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_mirrorer != null)
            {
                _mirrorer.Report += (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _mirrorer.CancelJob += (s, ce) => ce.Cancel = _mirrorer.Cancelled;
                _mirrorer.ReportName += (s, tne) =>
                    CurrentTaskName = tne.Name;
            }

            _cancellableAction = _mirrorer;


            var destinationPath = (string)e.Argument;

            try
            {
                _mirrorer.MirrorImage(destinationPath);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
                ClearWorkerData(
                    () =>
                    {
                        if (File.Exists(destinationPath))
                        {
                            File.Delete(destinationPath);
                        }
                    });
            }

            e.Result = destinationPath;
        }

        private void loaderWorker_MirrorImageCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_mirrorer != null)
            {
                _mirrorer.Report -= (s, pe) => OnProgressChanged(this, new Events.ProgressChangedEventArgs(pe.Percent));;
                _mirrorer.CancelJob -= (s, ce) => ce.Cancel = _mirrorer.Cancelled;
                _mirrorer.ReportName -= (s, tne) =>
                    CurrentTaskName = tne.Name;
            }

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("File mirroring cancelled"));
                OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Unable to mirror file: {0}", e.Error.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно отразить файл"));
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info,
                        string.Format("Mirroring completed: {0}", (string)e.Result));
            }

            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(false));
        }

        #endregion



        #endregion

        public void Undo(bool allowPointRemoval, bool allowAreaRemoval)
        {
            if (_file != null)
            {
                if (allowPointRemoval)
                {
                    _areaAligningWrapper.RemoveArea();
                    _pointSelector.RemoveLast();
                    OnAlignPossibilityChanged(this, new Events.AlignPossibilityEventArgs(AllowAligning()));
                }
                else if (allowAreaRemoval)
                {
                    _areaSelector.ResetArea();
                }

                DrawImage();
            }
        }

        #region drawing

        public void InitDrawImage(Size canvasSize = default(Size))
        {
            if (canvasSize != default(Size))
            {
                CanvasSize = canvasSize;
            }

            if (CanvasSize.Width != 0 && CanvasSize.Height != 0 && _tiles != null && _file != null)
            {
                var tDrawer = new Behaviors.Draw.TileDrawer(_filterProxy.Filter, _scaler);
                var iDrawer = new Behaviors.Draw.ItemDrawer(_pointSelector, _areaSelector, _scaler, _areaAligningWrapper);
                _drawer = new RlViewer.Behaviors.Draw.DrawerFacade(CanvasSize, iDrawer, tDrawer);

                ChangePalette(_settings.Palette, _settings.IsPaletteReversed,
                    _settings.IsPaletteGroupped, _settings.UseTemperaturePalette);
                InitScrollBars(ScaleFactor);

                DrawImage();
            }
        }


        public void DrawImage(Func<Image, Image> RedrawWithItems = null)
        {
            //if (_tiles != null && _drawer != null)
            //{
            Task.Run(() =>
            {
                Image img = null;
                // lock (_animationLock)
                // {
                if (_tiles != null && _drawer != null)
                {
                    try
                    {
                        img = _drawer.Draw(_tiles,
                                new System.Drawing.Point(XPointOfView, YPointOfView), _settings.HighResForDownScaled);
                    }
                    catch (Exception ex)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Generic type drawing error: {0}", ex.Message));
                    }
                }
                // }

                if (RedrawWithItems != null)
                {
                    OnImageDrawn(null, RedrawWithItems(img));
                }
                else
                {
                    OnImageDrawn(null, img);
                }
            }).Wait();

            //}
        }


        public void ChangePalette(float[] rgb, bool isReversed, bool isGrouped, bool useTemperaturePalette)
        {
            if (_drawer != null)
            {
                _drawer.GetPalette(rgb[0], rgb[1], rgb[2], isReversed, isGrouped, useTemperaturePalette);
            }
        }

        public void ScaleImage(int delta, Point mousePos = default(Point))
        {
            if (delta > 0)
            {
                ChangeScaleFactor((int)Math.Log(ScaleFactor, 2) + 1, mousePos);
            }
            else
            {
                ChangeScaleFactor((int)Math.Log(ScaleFactor, 2) - 1, mousePos);
            }
        }


        private void ChangeScaleFactor(float value, Point mousePos)
        {
            if (value > Math.Log(_scaler.MaxZoom, 2) || value < Math.Log(_scaler.MinZoom, 2)) return;

            float newScaleFactor = (float)Math.Pow(2, value);
            CenterScaledImage(newScaleFactor, mousePos);
            ScaleFactor = newScaleFactor;
            InitDrawImage();
        }

        public void DrawItems(Graphics g)
        {
            if (_tiles != null && _drawer != null)
            {
                _drawer.Draw(g, new System.Drawing.Point(XPointOfView, YPointOfView));
            }
        }
        #endregion

        #region guiProcessing


        private void CenterImageAtPoint(Point center, bool showWarning)
        {
            if (_file != null)
            {
                if (center != default(Point) &&
                    center.X > 0 && center.X < _file.Width && center.Y > 0 && center.Y < _file.Height)
                {

                    var horValue = (center.X - (int)(CanvasSize.Width / 2 / ScaleFactor));
                    horValue = horValue < 0 ? 0 : horValue;
                    horValue = horValue > XPointOfViewMax ? XPointOfViewMax : horValue;
                    XPointOfView = horValue;

                    var vertValue = (center.Y - (int)(CanvasSize.Height / 2 / ScaleFactor));
                    vertValue = vertValue < 0 ? 0 : vertValue;
                    vertValue = vertValue > YPointOfViewMax ? YPointOfViewMax : vertValue;
                    YPointOfView = vertValue;

                    DrawImage((image) => _drawer.DrawSharedPoint(image, center,
                        new Point(XPointOfView, YPointOfView), CanvasSize));

                }
                else if (showWarning)
                {
                    OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно найти точку"));
                }
            }
        }

        private void CenterScaledImage(float newScaleFactor, Point currentMousePos = default(Point))
        {
            if (_file == null)
            {
                return;
            }

            InitScrollBars(newScaleFactor);

            float mouseDividedFactorX = 2;
            float mouseDividedFactorY = 2;

            if (currentMousePos != default(Point))
            {
                mouseDividedFactorX = CanvasSize.Width / (float)currentMousePos.X;
                mouseDividedFactorY = CanvasSize.Height / (float)currentMousePos.Y;
            }

            float dividedImageWidth = Math.Min(CanvasSize.Width, _file.Width) / newScaleFactor / mouseDividedFactorX;
            dividedImageWidth = ScaleFactor > newScaleFactor ? -dividedImageWidth / 2 : dividedImageWidth;

            float dividedImageHeight = Math.Min(CanvasSize.Height, _file.Height) / newScaleFactor / mouseDividedFactorY;
            dividedImageHeight = ScaleFactor > newScaleFactor ? -dividedImageHeight / 2 : dividedImageHeight;

            float newHorizontalValue = XPointOfView + dividedImageWidth < 0 ? 0 :
                XPointOfView + dividedImageWidth;
            newHorizontalValue = newHorizontalValue + CanvasSize.Width /
                (ScaleFactor > newScaleFactor ? ScaleFactor : newScaleFactor) > _file.Width ?
                XPointOfViewMax : newHorizontalValue;

            float newVerticalValue = YPointOfView + dividedImageHeight < 0 ? 0
                : YPointOfView + dividedImageHeight;
            newVerticalValue = newVerticalValue + CanvasSize.Height /
                (ScaleFactor > newScaleFactor ? ScaleFactor : newScaleFactor) > _file.Height ?
                YPointOfViewMax : newVerticalValue;

            XPointOfView = (int)Math.Round(newHorizontalValue);
            YPointOfView = (int)Math.Round(newVerticalValue);
        }



        private bool AllowAligning()
        {
            bool isEnabled = false;


            if (_pointSelector != null)
            {
                var selectedPointsCount = _pointSelector.Union(_areaAligningWrapper.Select(x => x.SelectedPoint)).Count();

                if (_settings.SurfaceType == Behaviors.ImageAligning.Surfaces.SurfaceType.Custom)
                {
                    if (selectedPointsCount == 1 || selectedPointsCount == 3 || selectedPointsCount == 4 || selectedPointsCount == 5 || selectedPointsCount == 16)
                    {
                        isEnabled = true;
                    }
                    else
                    {
                        isEnabled = false;
                    }
                }
                else if (selectedPointsCount == 1 || selectedPointsCount >= 3 && selectedPointsCount <= 16)
                {
                    isEnabled = true;
                }
                else
                {
                    isEnabled = false;
                }
            }
            return isEnabled;
        }


        private void Deinitialize(bool showProgress = false)
        {
            _file = null;
            _drawer = null;
            _pointSelector = null;
            _areaSelector = null;
            _tiles = null;

            OnImageDrawn(this, null);

            XPointOfViewMax = 0;
            YPointOfViewMax = 0;
            OnAlignPossibilityChanged(this, new Events.AlignPossibilityEventArgs(false));
            OnProgressVisibilityChanged(this, new Events.ProgressControlsVisibilityEventArgs(showProgress));
        }


        private void InitScrollBars(float scaleFactor)
        {
            var f = _file as RlViewer.Files.LocatorFile;

            var horMax = (int)(f.Width - Math.Ceiling(CanvasSize.Width / scaleFactor));
            var verMax = (int)(f.Height - Math.Ceiling(CanvasSize.Height / scaleFactor));
            XPointOfViewMax = horMax > 0 ? horMax : 0;
            YPointOfViewMax = verMax > 0 ? verMax : 0;
        }


        #endregion

        #region saving

        private string GetSaveDialogFilter(Files.LocatorFile file)
        {
            string filter = Resources.SaveFilter;

            if (file.Properties.Type == FileType.raw || file.Properties.Type == FileType.r)
            {
                filter = Resources.RawSaveFilter;
            }
            else if (file.Properties.Type == FileType.rl8)
            {
                filter = Resources.Rl8SaveFilter;
            }

            return filter;
        }

        public void Save()
        {
            if (_file != null && _file.Properties.Type != FileType.k)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Title = "Имя сохраняемого файла";
                    sfd.FileName = Path.GetFileNameWithoutExtension(_file.Properties.FilePath).ToString();

                    sfd.Filter = GetSaveDialogFilter(_file);

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var sSize = new Forms.SaveForm(sfd.FileName, _file.Width, _file.Height, _areaSelector,
                            _settings.TileOutputAlgorithm, _filterProxy, _drawer.Palette))
                        {
                            if (sSize.ShowDialog() == DialogResult.OK)
                            {
                                ConfirmTaskStart(loaderWorker_SaveFile, loaderWorker_SaveFileCompleted, sSize.SaverParams);
                            }
                        }
                    }
                }
            }

        }
        #endregion

        #region filters

        /// <summary>
        /// Changes filter value and redraws image
        /// </summary>
        /// <param name="newValue">trackBar position</param>
        public void ChangeFilterValue(int newValue)
        {
            _filterProxy.ChangeFilterValue(newValue);
            if (_drawer != null)
            {
                OnImageDrawn(null, _drawer.RedrawImage());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterType">Type of retrieved filter</param>
        /// <param name="filterDelta">filter change step</param>
        /// <returns>Filter current value</returns>
        public int GetFilter(Behaviors.Filters.FilterType filterType, int filterDelta)
        {
            _filterProxy.GetFilter(filterType, filterDelta);
            return _filterProxy.Filter.FilterValue >> filterDelta;
        }

        public void ResetFilter()
        {
            _filterProxy.ResetFilters();
        }

        #endregion

        #region mouseHandlers

        public string ShowMousePosition(Point mouseLocation)
        {
            string mouseCoordinates = string.Empty;
            if (_file != null && !_worker.IsBusy)
            {
                if (mouseLocation.Y / ScaleFactor + YPointOfView > 0 &&
                    mouseLocation.Y / ScaleFactor + YPointOfView < _file.Height
                    && mouseLocation.X > 0 && mouseLocation.X / ScaleFactor < _file.Width)
                {
                    mouseCoordinates = string.Format("X:{0} Y:{1}",
                        (int)(mouseLocation.X / ScaleFactor) + XPointOfView, (int)(mouseLocation.Y / ScaleFactor) + YPointOfView);
                }
            }

            return mouseCoordinates;
        }

        public List<NavigationItem> ShowNavigation(MouseEventArgs e)
        {
            if (_file != null && _file.Navigation != null && !_worker.IsBusy)
            {
                if (e.Y / ScaleFactor + YPointOfView > 0
                    && e.Y / ScaleFactor + YPointOfView < _file.Height
                    && e.X > 0 && e.X / ScaleFactor < _file.Width)
                {

                    _navigationList.Clear();
                    foreach (var i in _file.Navigation[(int)(e.Y / ScaleFactor) + YPointOfView,
                        (int)(e.X / ScaleFactor) + XPointOfView])
                    {
                        _navigationList.Add(i);
                    }
                    return _navigationList;
                }
            }
            return null;
        }

        private void AnalyzePoint(Point mouseCoords, IWin32Window toolTipWindow)
        {
            if (_analyzer == null)
            {
                return;
            }

            try
            {
                if (_analyzer.Analyze(_file, new Point((int)(mouseCoords.X / ScaleFactor)
                     + XPointOfView, (int)(mouseCoords.Y / ScaleFactor) + YPointOfView)))
                {
                    _toolTip.Show(string.Format("Амплитуда: {0}", _analyzer.Amplitude.ToString()),
                           toolTipWindow, new Point(mouseCoords.X, mouseCoords.Y - 20));
                }

            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Point analyzing failed with message: {0}", ex.Message));
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно проанализировать точку"));
            }
        }

        #region mouseDown
        public void DragStart(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            _drag.StartTracing(new Point((int)(mouseCoords.X / ScaleFactor), (int)(mouseCoords.Y / ScaleFactor)));
        }

        public void SelectAreaStart(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            _areaSelector.ResetArea();
            _areaSelector.StartArea(new Point((int)Math.Ceiling(mouseCoords.X / ScaleFactor), (int)Math.Ceiling(mouseCoords.Y / ScaleFactor)),
                new Point((int)(XPointOfView), (int)(YPointOfView)));

        }

        public void ShareCoords(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            var clickedPoint = new Point(XPointOfView + ((int)(mouseCoords.X / ScaleFactor)),
                       YPointOfView + ((int)(mouseCoords.Y / ScaleFactor)));
            try
            {
                _pointSharer.SendPoint(clickedPoint);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error,
                    string.Format("Can't send point to remote host. Reason: {0}", ex.Message));
            }


        }

        public void SelectPointStart(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            if (_settings.UseAreasForAligning)
            {
                try
                {
                    _selectedPointArea = new Behaviors.AreaSelector.AreaSelectorDecorator(_file, _settings.MaxAlignerAreaSize);
                    _selectedPointArea.ResetArea();
                    _selectedPointArea.StartArea(new Point((int)Math.Ceiling(mouseCoords.X / ScaleFactor), (int)Math.Ceiling(mouseCoords.Y / ScaleFactor)),
                        new Point((int)(XPointOfView), (int)(YPointOfView)));

                }
                catch (Exception)
                {
                    OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно отметить точку"));
                }
            }
            else
            {
                using (Forms.EprInputForm eprForm = new Forms.EprInputForm())
                {
                    if (eprForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _pointSelector.Add((RlViewer.Files.LocatorFile)_file, new Point((int)(mouseCoords.X / ScaleFactor) + XPointOfView,
                        (int)(mouseCoords.Y / ScaleFactor) + YPointOfView), new Size(_settings.SelectorAreaSize, _settings.SelectorAreaSize), eprForm.EprValue);
                    }
                }

            }

            OnAlignPossibilityChanged(this, new Events.AlignPossibilityEventArgs(AllowAligning()));
        }

        public void GetPointAmplitudeStart(Point mouseCoords, IWin32Window toolTipWindow)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            _analyzer.StartTracing();
            AnalyzePoint(mouseCoords, toolTipWindow);
        }

        public void VerticalSectionStart(Point mouseCoord)
        {

            if (_file == null || _drawer == null)
            {
                return;
            }

            _section = new Behaviors.Sections.Concrete.VerticalSection(_settings.SectionSize, new Point((int)(mouseCoord.X / ScaleFactor) + XPointOfView,
                                (int)(mouseCoord.Y / ScaleFactor) + YPointOfView));
        }


        public void HorizontalSectionStart(Point mouseCoord)
        {

            if (_file == null || _drawer == null)
            {
                return;
            }

            _section = new Behaviors.Sections.Concrete.HorizontalSection(_settings.SectionSize, new Point((int)(mouseCoord.X / ScaleFactor) + XPointOfView,
                                (int)(mouseCoord.Y / ScaleFactor) + YPointOfView));
        }

        public void LinearSectionStart(Point mouseCoord)
        {

            if (_file == null || _drawer == null)
            {
                return;
            }

            _section = new Behaviors.Sections.Concrete.LinearSection(_settings.SectionSize, new Point((int)(mouseCoord.X / ScaleFactor) + XPointOfView,
                                (int)(mouseCoord.Y / ScaleFactor) + YPointOfView));
        }


        public void RulerStart(Point mouseCoord)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }


            if (!_ruler.Pt1Fixed)
            {
                _ruler.Pt1 = new Point((int)(mouseCoord.X / ScaleFactor) + XPointOfView,
                        (int)(mouseCoord.Y / ScaleFactor) + YPointOfView);
            }
            else if (!_ruler.Pt2Fixed)
            {
                _ruler.Pt2 = new Point((int)(mouseCoord.X / ScaleFactor) + XPointOfView,
                                                   (int)(mouseCoord.Y / ScaleFactor) + YPointOfView);
            }
        }

        #endregion

        #region mouseMove

        /// <summary>
        /// Sets point of view to the selected point
        /// </summary>
        /// <param name="mouseCoords">Current mouse local coords</param>
        /// <returns>True if dragged succeeded, false otherwise</returns>
        public bool Drag(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return false;
            }

            var dragged = _drag.Trace(new Point((int)(mouseCoords.X / ScaleFactor), (int)(mouseCoords.Y / ScaleFactor)));

            if (dragged)
            {
                var newHor = XPointOfView - _drag.Delta.X * _settings.DragAccelerator;
                var newVert = YPointOfView - _drag.Delta.Y * _settings.DragAccelerator;

                newVert = newVert < 0 ? 0 : newVert;
                newVert = newVert > YPointOfViewMax ? YPointOfViewMax : newVert;
                YPointOfView = newVert;

                newHor = newHor < 0 ? 0 : newHor;
                newHor = newHor > XPointOfViewMax ? XPointOfViewMax : newHor;
                XPointOfView = newHor;

                DrawImage();
            }

            return dragged;
        }

        public void SelectArea(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            if (_areaSelector != null)
            {
                if (_areaSelector.ResizeArea(new Point((int)(mouseCoords.X / ScaleFactor),
                                               (int)(mouseCoords.Y / ScaleFactor)), new Point(XPointOfView, YPointOfView)))
                {
                    OnImageDrawn(this, _drawer.DrawSelectorArea(new Point(XPointOfView, YPointOfView)));
                }
            }
        }

        public void SelectPoint(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            if (_settings.UseAreasForAligning)
            {
                if (_selectedPointArea != null)
                {
                    if (_selectedPointArea.ResizeArea(new Point((int)(mouseCoords.X / ScaleFactor),
                                 (int)(mouseCoords.Y / ScaleFactor)), new Point(XPointOfView, YPointOfView)))
                    {
                        OnImageDrawn(this, _drawer.DrawAlignerAreas(new Point(XPointOfView, YPointOfView), _selectedPointArea));
                    }
                }
            }
            else
            {
                var leftTop = new Point(mouseCoords.X - (int)(_settings.SelectorAreaSize * ScaleFactor / 2),
                mouseCoords.Y - (int)(_settings.SelectorAreaSize * ScaleFactor / 2));
                var img = _drawer.DrawSquareArea(leftTop, _settings.SelectorAreaSize);
                OnImageDrawn(this, img);
            }
        }

        public void GetPointAmplitude(Point mouseCoords, IWin32Window toolTipWindow)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            AnalyzePoint(mouseCoords, toolTipWindow);
        }

        public void VerticalSection(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            var img = _drawer.DrawVerticalSection(mouseCoords, (int)(_settings.SectionSize * ScaleFactor));
            OnImageDrawn(this, img);
        }

        public void HorizontalSection(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            var img = _drawer.DrawHorizontalSection(mouseCoords, (int)(_settings.SectionSize * ScaleFactor));
            OnImageDrawn(this, img);
        }

        public void LinearSection(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }


            if (_section != null && _section.InitialPoint != default(Point))
            {
                var endPoint = new Point((int)(mouseCoords.X / ScaleFactor) + XPointOfView,
                                                       (int)(mouseCoords.Y / ScaleFactor) + YPointOfView);

                var img = _drawer.DrawLinearSection(
                     new Point((int)((_section.InitialPoint.X - XPointOfView) * ScaleFactor),
                      (int)((_section.InitialPoint.Y - YPointOfView) * ScaleFactor)),
                     new Point((int)((endPoint.X - XPointOfView) * ScaleFactor),
                      (int)((endPoint.Y - YPointOfView) * ScaleFactor)));

                OnImageDrawn(this, img);
            }
        }

        public void GetDistance(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            if (_ruler.Pt1Fixed || _ruler.Pt2Fixed)
            {
                var endPoint = _ruler.Pt2Fixed ? _ruler.Pt2 : new Point((int)(mouseCoords.X / ScaleFactor) + XPointOfView,
                                                       (int)(mouseCoords.Y / ScaleFactor) + YPointOfView);

                var img = _drawer.DrawRuler(new Point((int)((_ruler.Pt1.X - XPointOfView) * ScaleFactor),
                    (int)((_ruler.Pt1.Y - YPointOfView) * ScaleFactor)),
                    new Point((int)((endPoint.X - XPointOfView) * ScaleFactor),
                     (int)((endPoint.Y - YPointOfView) * ScaleFactor)));

                OnImageDrawn(this, img);

                RulerDistance = _ruler.GetDistance(endPoint);
            }
        }

        #endregion

        #region mouseUp
        public void DragFinish()
        {
            if (_file == null || _drawer == null || _drag == null)
            {
                return;
            }

            _drag.StopTracing();
        }


        public void SelectAreaFinish(Point mouseCoords)
        {
            if (_file == null || _drawer == null || _areaSelector == null)
            {
                return;
            }

            if (_areaSelector.IsActive)
            {
                _areaSelector.StopResizing();
            }
        }


        public void SelectPointFinish(Point mouseCoords)
        {
            if (_file == null || _drawer == null)
            {
                return;
            }

            if (_selectedPointArea != null && _selectedPointArea.SelectedPoint == null)
            {
                using (var eprForm = new Forms.EprInputForm())
                {
                    if (eprForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _selectedPointArea.StopResizing(eprForm.EprValue);
                        _areaAligningWrapper.AddArea(_selectedPointArea);
                    }
                    else
                    {
                        _selectedPointArea.ResetArea();
                        _selectedPointArea.StopResizing();
                    }
                }

            }
        }

        public void StopPointAnalyzer(IWin32Window toolTipWindow)
        {
            _analyzer.StopTracing();
            _toolTip.Hide(toolTipWindow);
        }

        public void StopSection(Point mouseCoords)
        {
            ShowSection(_section, new Point((int)(mouseCoords.X / ScaleFactor) + XPointOfView,
                        (int)(mouseCoords.Y / ScaleFactor) + YPointOfView));
        }

        #endregion

        #endregion

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

            if (disposing && _pointSharer != null)
            {
                _pointSharer.DataReceived -= (s, gde) => CrossAppDataReceivedCallback(s, gde);
                _pointSharer.Dispose();
            }

            _toolTip.Dispose();
            _disposed = true;

        }
        #endregion

        #region newFormTools

        public void ShowFileInfo()
        {
            if (_file != null)
            {
                try
                {
                    var headerInfo = _file.Header.HeaderInfo;
                }
                catch
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Error,
                        string.Format("Unable to parse file header {0}", _file.Properties.FilePath));
                    return;
                }


                using (var iFrm = new Forms.InfoForm(_file.Header.HeaderInfo))
                {
                    iFrm.ShowDialog();
                }
            }
        }

        public void ShowSettings()
        {
            try
            {
                using (var settgingsForm = new Forms.SettingsForm(_settings))
                {
                    if (settgingsForm.ShowDialog() == DialogResult.OK)
                    {
                        InitDrawImage();
                        OnAlignPossibilityChanged(this, new Events.AlignPossibilityEventArgs(AllowAligning()));
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("Error while applying new settings: {0}", ex.Message));
            }
        }


        public void ShowCache()
        {
            var tileDir = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "tiles");
            using (var cacheForm = new Forms.TileStatusForm(tileDir, _file == null ? string.Empty : _file.Properties.FilePath))
            {
                cacheForm.ShowDialog();
            }
        }



        public void ShowLog()
        {
            using (var logForm = new Forms.LogForm())
            {
                logForm.ShowDialog();
            }
        }


        public void ResampleImage()
        {
            if (_pointSelector != null)
            {
                if (_pointSelector.Count() == 1)
                {
                    NormalizeImage();
                }
                else AlignImage();
            }
        }


        private void NormalizeImage()
        {
            using (var normalizeSaveDlg = new SaveFileDialog())
            {
                normalizeSaveDlg.Filter = Resources.SaveFilter;
                if (normalizeSaveDlg.ShowDialog() == DialogResult.OK)
                {
                    ConfirmTaskStart(loaderWorker_NormalizeFile, loaderWorker_NormalizeFileCompleted, normalizeSaveDlg.FileName);
                }
            }
        }


        private void AlignImage()
        {
            using (var alignedSaveDlg = new SaveFileDialog())
            {
                alignedSaveDlg.Filter = Resources.AlignFilter;
                if (alignedSaveDlg.ShowDialog() == DialogResult.OK)
                {
                    var selectedPoints = _pointSelector.Union(_areaAligningWrapper.Select(x => x.SelectedPoint));

                    var compressedSelector = new Behaviors.PointSelector.CompressedPointSelectorWrapper(_file,
                       selectedPoints, (int)_settings.RangeCompressionCoef, (int)_settings.AzimuthCompressionCoef);

                    _aligner = new Behaviors.ImageAligning.Aligning(_file, compressedSelector,
                        new Behaviors.Interpolators.LeastSquares.Concrete.LinearLeastSquares(compressedSelector),
                        _settings.SurfaceType, _settings.RbfMlBaseRaduis, _settings.RbfMlLayersNumber, _settings.RbfMlRegularizationCoef);

                    ConfirmTaskStart(loaderWorker_AlignImage, loaderWorker_AlignImageCompleted,
                        Path.ChangeExtension(alignedSaveDlg.FileName, Path.GetExtension(_file.Properties.FilePath)));
                }
            }
        }

        public void ShowFindPoint()
        {
            if (_file != null && _searcher != null)
            {
                try
                {
                    using (var ff = new Forms.FindPointForm(_file.Navigation != null))
                    {
#if DEBUG
                        MessageBox.Show("Функция работает в тестовом режиме!");
#endif
                        if (ff.ShowDialog() == DialogResult.OK)
                        {
                            ConfirmTaskStart(loaderWorker_FindPoint, loaderWorker_FindPointCompleted, ff.Params);
                        };
                    }
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Error,
                        string.Format("Error occured while parsing input: {0}", ex.Message));
                }
            }
        }

        public void ResetRuler()
        {
            if (_ruler != null)
            {
                _ruler.ResetRuler();
            }
        }

        public void ShowAbout()
        {
            using (var about = new Forms.About())
            {
                about.ShowDialog();
            }
        }



        public void GetAreaStatistics()
        {
            if (_file == null)
            {
                return;
            }

            if (_areaSelector == null || _areaSelector.Area == null || _areaSelector.Area.Width == 0 || _areaSelector.Area.Height == 0)
            {
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно построить статистику области"));
                return;
            }
            else if (_areaSelector.Area.Width >= _settings.AligningAreaBorderSize || _areaSelector.Area.Height >= _settings.AligningAreaBorderSize)
            {

                OnErrorOccured(this, new Events.ErrorOccuredEventArgs(
                    string.Format("Выделен слишком большой участок (максимум: {0})", _settings.AligningAreaBorderSize)));
                return;
            }

            Form statFrm = null;

            try
            {
                statFrm = new Forms.StatisticsForm(_file, _areaSelector);
                statFrm.ShowDialog();
            }
            catch (Exception)
            {
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно построить статистику области"));
            }
            finally
            {
                if (statFrm != null)
                {
                    statFrm.Dispose();
                }
            }
        }


        public void EmbedNavigation()
        {
            using (var ofd = new OpenFileDialog() { Title = "Выберите исходный файл Банк-РЛ", Filter = Resources.NaviEmbeddingFilterDest })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    EmbedNavigation(ofd.FileName, true);
                }
            }
        }

        private void EmbedNavigation(string rliFileName, bool forced = true)
        {
            var fileToChangeProp = new Files.FileProperties(rliFileName);
            var fileToChangeHeader = Factories.Header.Abstract.HeaderFactory.GetFactory(fileToChangeProp).Create(rliFileName);
            var fileToChange = Factories.File.Abstract.FileFactory.GetFactory(fileToChangeProp).Create(fileToChangeProp, fileToChangeHeader, null);

            if (!forced)
            {
                if (!Behaviors.Navigation.NavigationChanger.Abstract.NavigationChanger.HasBaRhgSource(fileToChange))
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Info, @"Aligned image doesn't have .ba file as its source, navigation embedding process stopped.
                    Consider using manual embedding");
                    return;
                }
            }

            Behaviors.Navigation.NavigationChanger.Abstract.NavigationChanger naviChanger = null;

            using (var ofd = new OpenFileDialog() { Title = "Выберите исходный файл РГГ", Filter = Resources.NaviEmbeddingFilterSource })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sourceFileProp = new Files.FileProperties(ofd.FileName);
                        var sourceFileHeader = Factories.Header.Abstract.HeaderFactory.GetFactory(sourceFileProp).Create(ofd.FileName);
                        var sourceFile = Factories.File.Abstract.FileFactory.GetFactory(sourceFileProp).Create(sourceFileProp, sourceFileHeader, null);

                        naviChanger = Factories.NavigationChanger.Abstract.NavigationChangerFactory.GetFactory(fileToChangeProp).Create(fileToChange, sourceFile);

                        naviChanger.ChangeFlightTime();
                        naviChanger.ChangeNavigation();
                        Logging.Logger.Log(Logging.SeverityGrades.Info,
                            string.Format("Successfully applied navigation to {0}", rliFileName));
                    }
                    catch (Exception ex)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Error,
                            string.Format("Unable to embed new navigation: {0}", ex.Message));
                        OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Не удалось встроить навигацию"));
                    }
                }
            }
        }



        private Behaviors.ReportGenerator.ReporterSettings _reporterSettings;

        public void ReportDialog()
        {
            using (var reportSettings = new Forms.ReportSettingsForm(_reporterSettings))
            {
                if (reportSettings.ShowDialog() == DialogResult.OK)
                {
                    MakeReport(reportSettings.ReporterType, reportSettings.ReporterSettings);
                    _reporterSettings = reportSettings.ReporterSettings;
                }
            }
        }


        private void MakeReport(Behaviors.ReportGenerator.Abstract.ReporterTypes reportType,
            Behaviors.ReportGenerator.ReporterSettings reporterSettings)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Файлы для формирования отчета";
                ofd.Multiselect = true;
                ofd.Filter = Resources.OpenFilter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    FileType type;
                    var validFiles = ofd.FileNames.Where(
                        x => Enum.TryParse<FileType>(System.IO.Path.GetExtension(x).Replace(".", ""), out type) && type != FileType.raw);

                    if (validFiles.Count() == 0)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Error, "Unsuitable files selected");
                        OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Выбраны неподходящие файлы"));
                        return;
                    }

                    using (var fsd = new SaveFileDialog())
                    {
                        fsd.Title = "Имя для файла отчета";
                        fsd.Filter = "Документ MS Word|*.docx";
                        if (fsd.ShowDialog() == DialogResult.OK)
                        {
                            _reporter = Factories.Reporter.Abstract.ReporterFactory
                                                .GetFactory(reportType)
                                                .Create(validFiles.ToArray());

                            ConfirmTaskStart(loaderWorker_GenerateReport,
                                loaderWorker_GenerateReportCompleted, new object[] { fsd.FileName, reporterSettings });
                        }
                    }
                }
            }
        }


        //private void Show3dPlot(Rectangle area)
        //{
        //    float[] areaData = new float[area.Width * area.Height];
        //    float[] borderedArea = _file.GetArea(area).ToArea<float>(_file.Header.BytesPerSample);

        //    var borderedAreaWidth = area.X + area.Width - _file.Width > 0 ? _file.Width - area.X : area.Width;

        //    for (int i = 0; i < borderedArea.Length; i++)
        //    {
        //        var heightIndex = i / borderedAreaWidth * borderedAreaWidth;
        //        var widthIndex = i % borderedAreaWidth;
        //        areaData[widthIndex + heightIndex] = borderedArea[i];
        //    }

        //    using (var plotFrm = new Forms.Plot3dForm(area.Location.X,
        //        area.Location.X + area.Width, area.Location.Y, area.Location.Y + area.Height, areaData))
        //    {
        //        plotFrm.ShowDialog();
        //    }
        //}


        private string GetSectionFormCaption(Behaviors.Sections.Abstract.Section section)
        {
            var type = section.GetType();


            if (type == typeof(Behaviors.Sections.Concrete.HorizontalSection))
            {
                return "Горизонтальное сечение";
            }
            else if (type == typeof(Behaviors.Sections.Concrete.VerticalSection))
            {
                return "Вертикальное сечение";
            }
            else if (type == typeof(Behaviors.Sections.Concrete.LinearSection))
            {
                return "Произвольное сечение";
            }

            throw new NotSupportedException("unsupported section type");
        }


        private void ShowSection(Behaviors.Sections.Abstract.Section section, Point p)
        {
            List<PointF> points = null;

            try
            {
                points = section.GetValues(_file, p).ToList();
            }
            catch
            {
                OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Невозможно построить сечение"));
                return;
            }

            var mark = section.InitialPointMark;
            var caption = GetSectionFormCaption(section);

            using (var sectionForm = new Forms.SectionGraphForm(points, mark, caption))
            {
                sectionForm.ShowDialog();
            }
            _section.ResetSection();
        }


        #endregion

        /// <summary>
        /// Checks if started in admin mode. Allows restarting in admin mode if process is in user mode
        /// </summary>
        /// <param name="tryForceAdmin">True to show admin restart dialog, false to continue in current mode</param>
        private void TryRunAsAdmin(bool tryForceAdmin)
        {
            Adminizer adm = new Adminizer();

            if (!adm.CheckIfAdmin())
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Application started in user mode");

                if (!tryForceAdmin)
                {
                    return;
                }

                var confirmation = MessageBox.Show(string.Format(@"Система обнаружила запуск без привилегий администратора.{0}Их отсутствие может повлиять на работу программы.{1}Перезапустить в режиме администратора?", Environment.NewLine, Environment.NewLine),
                                "Повышение уровня доступа",
                                MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    try
                    {
                        adm.EvaluateToAdminProcess();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    catch (OperationCanceledException ocex)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Info, ocex.Message);
                    }
                    catch (Exception ex)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Warning,
                            string.Format("Unable to evaluate process status: {0}", ex.Message));
                        OnErrorOccured(this, new Events.ErrorOccuredEventArgs("Запуск в режиме администратора невозможен"));
                    }
                }
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Application started in admin mode");
            }

        }


        public void AggregateFiles()
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = @"Выберите путь для сохранения";
                sfd.Filter = @"Файлы РГГ Банк-РЛ (*.ba)|*.ba";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var ofd = new OpenFileDialog())
                    {
                        ofd.Title = @"Выберите файлы для совмещения";
                        ofd.Multiselect = true;
                        ofd.Filter = @"Файлы РГГ Банк-РЛ (*.ba)|*.ba";

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            var aggregatorOrder = new Forms.AggregatorOrderForm(ofd.FileNames);

                            if (aggregatorOrder.ShowDialog() == DialogResult.OK)
                            {
                                var aggregatorParams = new Behaviors.FilesAggregator.AggregatorParams(sfd.FileName, aggregatorOrder.SourceFiles);
                                ConfirmTaskStart(loaderWorker_AggregateFiles, loaderWorker_AggregateFilesCompleted, aggregatorParams);
                            }
                        }
                    }
                }
            }
        }


        public void MirrorImage()
        {
            if (_file != null && _file is Files.Rli.Abstract.RliFile)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Title = @"Выберите путь для сохранения";
                    sfd.Filter = @"Файлы изображений (*.brl4; *.rl4; *.rl8)|*.brl4;*.rl4;*.rl8";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        _mirrorer = Factories.ImageMirrorer.ImageMirrorerFactory.Create(_file);
                        ConfirmTaskStart(loaderWorker_MirrorImage, loaderWorker_MirrorImageCompleted, sfd.FileName);
                    }
                }
            }
        }


        /// <summary>
        /// Gets settings from xml file
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                _settings = _settings.FromXml();
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, "Settings file is corrupted, loading default values");
                _settings = new Settings.Settings();
                _settings.ToXml();
            }
        }

        private void CrossAppDataReceivedCallback(object sender, Behaviors.CrossAppCommunication.GotDataEventArgs gde)
        {
            if (_pointSharer != null)
            {
                _pointSharer.ProcessMessage(gde.Data, (point) =>
                {
                    if (_allowRemoteDataReceiving)
                    {
                        CenterImageAtPoint(point, false);
                    }
                });
            }
        }

    }
}
