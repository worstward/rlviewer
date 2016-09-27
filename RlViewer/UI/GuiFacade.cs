﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
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
    public class GuiFacade : IDisposable
    {
        public GuiFacade(ISuitableForm form)
        {
            LoadSettings();
            TryRunAsAdmin(_settings.ForceAdminMode);

            _form = form;
            _filterProxy = new Behaviors.Filters.ImageFilterProxy();
            _scaler = new Behaviors.Scaling.Scaler(_settings.MinScale, _settings.MaxScale, _settings.InitialScale);
            _drag = new Behaviors.DragController();
            _chart = new Forms.ChartHelper(new Behaviors.Draw.HistContainer());

            _win = _form.Canvas;
            OnImageDrawn += (s, img) => _form.Canvas.Image = img;

            InitializeWindow();
        }

        private Settings.Settings _settings;
        private ITileCreator _creator;

        private Behaviors.Filters.ImageFilterProxy _filterProxy;
        private Behaviors.Saving.Abstract.Saver _saver;
        private Behaviors.Scaling.Scaler _scaler;
        private Behaviors.Analyzing.Abstract.SampleAnalyzer _analyzer;
        private Behaviors.Ruler.RulerProxy _ruler;
        private Behaviors.ImageAligning.Aligning _aligner;
        private Behaviors.Sections.Abstract.Section _section;
        private Behaviors.Navigation.NavigationSearcher.Abstract.GeodesicPointFinder _searcher;
        private Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer _pointSharer;
        private Behaviors.ReportGenerator.Abstract.Reporter _reporter;
        private WorkerEventController _cancellableAction;


        private Files.FileProperties _properties;
        private Headers.Abstract.LocatorFileHeader _header;
        private Navigation.NavigationContainer _navi;
        private Forms.ChartHelper _chart;

        private Files.LocatorFile _file;
        private Behaviors.TileCreator.Tile[] _tiles;
        private Behaviors.Draw.DrawerFacade _drawer;
        private Behaviors.PointSelector.PointSelector _pointSelector;
        private Behaviors.AreaSelector.AreaSelectorDecorator _selectedPointArea;
        private Behaviors.AreaSelector.AreaSelectorsAlignerContainer _areaAligningWrapper;


        private Behaviors.AreaSelector.AreaSelector _areaSelector;
        private Behaviors.DragController _drag;

        private ISuitableForm _form;
        private System.ComponentModel.BackgroundWorker _worker;
        private string _caption = string.Empty;
        private ToolTip _toolTip = new ToolTip();
        private IWin32Window _win;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event EventHandler<Image> OnImageDrawn = delegate { };

        private object _animationLock = new object();


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
            string caption;


            if (_worker != null && _worker.IsBusy)
            {
                var confirmation = MessageBox.Show("Вы уверены, что хотите отменить выполняемую операцию?",
                                "Подтвердите отмену",
                                MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    CancelLoading();
                }
                else
                {
                    DrawImage();
                    if (_file != null)
                    {
                        return _file.Properties.FilePath;
                    }

                    return string.Empty;
                }
            }


            InitializeWindow();
            StartTask("Чтение навигации", loaderWorker_InitFile, loaderWorker_InitFileCompleted, fileName);
            _form.NavigationDgv.Rows.Clear();
            caption = _caption = fileName;
            return caption;

        }
        #endregion

        #region tasks

        private void StartTask(string caption, System.ComponentModel.DoWorkEventHandler d,
            System.ComponentModel.RunWorkerCompletedEventHandler c, object arg = null)
        {

            InitProgressControls(true, caption);
            _worker = ThreadHelper.InitWorker(d, c);
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
            if (_worker != null && _file != null)
            {
                try
                {
                    disposer();
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                    Forms.FormsHelper.ShowErrorMsg(ex.Message);
                }
            }

            _worker.Dispose();
        }


        #region findPointWorkerMethods
        private void loaderWorker_FindPoint(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            _searcher.Report += (s, pe) => ProgressReporter(pe.Percent);
            _searcher.CancelJob += (s, ce) => ce.Cancel = _searcher.Cancelled;
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
            _searcher.Report -= (s, pe) => ProgressReporter(pe.Percent);
            _searcher.CancelJob -= (s, ce) => ce.Cancel = _searcher.Cancelled;

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Searching cancelled"));
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Error searching point: {0}", e.Error.Message));
                Forms.FormsHelper.ShowErrorMsg("Unable to find point");
            }
            else
            {
                CenterImageAtPoint((Point)e.Result, true);
            }


            InitProgressControls(false);
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

            _saver.Report += (s, pe) => ProgressReporter(pe.Percent);
            _saver.CancelJob += (s, ce) => ce.Cancel = _saver.Cancelled;
            _cancellableAction = _saver;

            var filter = parameters.KeepFiltering ? _filterProxy : null;
            var palette = parameters.KeepPalette ? _drawer.Palette : null;

            try
            {
                _saver.Save(parameters.Path, Path.GetExtension(parameters.Path).Replace(".", "")
                    .ToEnum<RlViewer.FileType>(), new Rectangle(parameters.LeftTop.X, parameters.LeftTop.Y, parameters.Width, parameters.Height),
                    _creator.NormalizationFactor, _creator.MaxValue, palette, filter);
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
            _saver.Report -= (s, pe) => ProgressReporter(pe.Percent);
            _saver.CancelJob -= (s, ce) => ce.Cancel = _saver.Cancelled;

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Saving cancelled"));
                string errMess = string.Format("Saving cancelled");
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while saving image: {0}", e.Error.Message));
                Forms.FormsHelper.ShowErrorMsg("Unable to save image");
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Saving completed: {0}", (string)e.Result));
            }

            InitProgressControls(false);
        }
        #endregion

        #region alignImageWorkerMethods

        private void loaderWorker_AlignImage(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var fileName = (string)e.Argument;

            _aligner.Report += (s, pe) => ProgressReporter(pe.Percent);
            _aligner.CancelJob += (s, ce) => ce.Cancel = _aligner.Cancelled;
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
            _aligner.Report -= (s, pe) => ProgressReporter(pe.Percent);
            _aligner.CancelJob -= (s, ce) => ce.Cancel = _aligner.Cancelled;


            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Image aligning cancelled"));
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while aligning image: {0}", e.Error.Message));
                Forms.FormsHelper.ShowErrorMsg("Unable to align image");
            }
            else
            {
                var fileName = Path.GetFullPath((string)e.Result);

                var type = new Files.FileProperties(fileName).Type;

                if (type != FileType.raw && type != FileType.r)
                {
                    fileName = Path.ChangeExtension(fileName, "brl4");
                    EmbedNavigation(fileName, false);
                }

                Logging.Logger.Log(Logging.SeverityGrades.Info,
                    string.Format("Image aligning completed, new file saved at: {0}", fileName));
            }
            InitProgressControls(false);
        }
        #endregion

        #region initFileWorkerMethods

        private void loaderWorker_InitFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            _properties = new Files.FileProperties((string)e.Argument);
            _header = Factories.Header.Abstract.HeaderFactory.GetFactory(_properties).Create(_properties.FilePath);

            _navi = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(_properties).Create(_properties, _header);

            _navi.Report += (s, pe) => ProgressReporter(pe.Percent);
            _navi.CancelJob += (s, ce) => ce.Cancel = _navi.Cancelled;
            _cancellableAction = _navi;

            try
            {
                _navi.GetNavigation();
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }

            _file = FileFactory.GetFactory(_properties).Create(_properties, _header, _navi);

            _ruler = new Behaviors.Ruler.RulerProxy(_file);
            _saver = SaverFactory.GetFactory(_properties).Create(_file);
            _searcher = Factories.NavigationSearcher.Abstract.PointFinderFactory.GetFactory(_file.Properties).Create(_file);
            _analyzer = Factories.Analyzer.AnalyzerFactory.Create(_file);
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
                _navi.Report -= (s, pe) => ProgressReporter(pe.Percent);
                _navi.CancelJob -= (s, ce) => ce.Cancel = _navi.Cancelled;
            }


            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("File opening cancelled"));
                InitializeWindow();
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error occured while opening file: {0}", e.Error.Message));
                Forms.FormsHelper.ShowErrorMsg("Unable to open file");
                InitializeWindow();
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("{0} file opened: {1}",
                    _file.Properties.Type, _file.Properties.FilePath));

                StartTask("Генерация тайлов", loaderWorker_CreateTiles, loaderWorker_CreateTilesCompleted);
            }
        }
        #endregion

        #region createTilesWorkerMethods

        private void loaderWorker_CreateTiles(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _creator = TileCreatorFactory.GetFactory(_file).Create(_file as RlViewer.Files.LocatorFile, _settings.TileOutputAlgorithm);

            ((WorkerEventController)_creator).Report += (s, pe) => ProgressReporter(pe.Percent);
            ((WorkerEventController)_creator).CancelJob += (s, ce) => ce.Cancel = ((WorkerEventController)_creator).Cancelled;
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
                ((WorkerEventController)_creator).Report -= (s, pe) => ProgressReporter(pe.Percent);
                ((WorkerEventController)_creator).CancelJob -= (s, ce) => ce.Cancel = ((WorkerEventController)_creator).Cancelled;
            }

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Tile creation cancelled"));
                ClearWorkerData(() => _creator.ClearCancelledFileTiles(_file.Properties.FilePath));
                InitializeWindow();
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error creating tiles: {0}",
                e.Error.InnerException == null ? e.Error.Message : e.Error.InnerException.Message));
                ClearWorkerData(() => _creator.ClearCancelledFileTiles(_file.Properties.FilePath));
                Forms.FormsHelper.ShowErrorMsg("Ошибка генерации изображения из файла");
                InitializeWindow();

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

                InitProgressControls(false);
                InitDrawImage();
            }

        }
        #endregion

        #region generateReportWorkerMethods

        private void loaderWorker_GenerateReport(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_reporter != null)
            {
                ((WorkerEventController)_reporter).Report += (s, pe) => ProgressReporter(pe.Percent);
                ((WorkerEventController)_reporter).CancelJob += (s, ce) => ce.Cancel = ((WorkerEventController)_reporter).Cancelled;
            }

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
                ((WorkerEventController)_reporter).Report -= (s, pe) => ProgressReporter(pe.Percent);
                ((WorkerEventController)_reporter).CancelJob -= (s, ce) => ce.Cancel = ((WorkerEventController)_reporter).Cancelled;
            }

            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Report generation cancelled"));
                InitializeWindow();
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Unable to make report: {0}", e.Error.Message));
                InitializeWindow();
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info,
                        string.Format("Report file generated: {0}", (string)e.Result));
            }

            InitProgressControls(false);
        }

        #endregion


        #endregion

        public void Undo()
        {
            if (_file != null)
            {
                if (_form.MarkPointRb.Checked)
                {
                    _areaAligningWrapper.RemoveArea();
                    _pointSelector.RemoveLast();
                    BlockAlignButton();
                }
                else if (_form.MarkAreaRb.Checked)
                {
                    _areaSelector.ResetArea();
                }


                _form.Canvas.Invalidate();
            }
        }

        #region drawing

        public void InitDrawImage()
        {
            if (_form.Canvas.Size.Width != 0 && _form.Canvas.Size.Height != 0 && _tiles != null && _file != null)
            {
                var tDrawer = new Behaviors.Draw.TileDrawer(_filterProxy.Filter, _scaler);
                var iDrawer = new Behaviors.Draw.ItemDrawer(_pointSelector, _areaSelector,
                    _scaler, _areaAligningWrapper);
                _drawer = new RlViewer.Behaviors.Draw.DrawerFacade(_form.Canvas.Size, iDrawer, tDrawer);

                ChangePalette(_settings.Palette, _settings.IsPaletteReversed,
                    _settings.IsPaletteGroupped, _settings.UseTemperaturePalette);
                InitScrollBars(_scaler.ScaleFactor);

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
                                new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value), _settings.HighResForDownScaled);
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

        public void ScaleImage(int delta)
        {
            if (delta > 0)
            {
                ChangeScaleFactor((int)Math.Log(_scaler.ScaleFactor, 2) + 1);
            }
            else
            {
                ChangeScaleFactor((int)Math.Log(_scaler.ScaleFactor, 2) - 1);
            }
        }


        private void ChangeScaleFactor(float value)
        {
            if (value > Math.Log(_scaler.MaxZoom, 2) || value < Math.Log(_scaler.MinZoom, 2)) return;

            float scaleFactor = (float)Math.Pow(2, value);
            _form.ScaleLabel.Text = string.Format("Масштаб: {0}%", (scaleFactor * 100).ToString());

            if (_file != null)
            {
                CenterScaledImage(scaleFactor);
            }

            _scaler = new Behaviors.Scaling.Scaler(_settings.MinScale, _settings.MaxScale, scaleFactor);
            InitDrawImage();
        }

        public void DrawItems(Graphics g)
        {
            if (_tiles != null && _drawer != null)
            {
                _drawer.Draw(g, new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
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

                    var horValue = (center.X - (int)(_form.Canvas.Width / 2 / _scaler.ScaleFactor));
                    horValue = horValue < 0 ? 0 : horValue;
                    horValue = horValue > _form.Horizontal.Maximum ? _form.Horizontal.Maximum : horValue;

                    ThreadHelper.ThreadSafeUpdate<HScrollBar>(_form.Horizontal, () => _form.Horizontal.Value = horValue);

                    var vertValue = (center.Y - (int)(_form.Canvas.Height / 2 / _scaler.ScaleFactor));
                    vertValue = vertValue < 0 ? 0 : vertValue;
                    vertValue = vertValue > _form.Vertical.Maximum ? _form.Vertical.Maximum : vertValue;

                    ThreadHelper.ThreadSafeUpdate<VScrollBar>(_form.Vertical, () => _form.Vertical.Value = vertValue);

                    DrawImage((image) => _drawer.DrawSharedPoint(image, center,
                        new Point(_form.Horizontal.Value, _form.Vertical.Value), _form.Canvas.Size));

                }
                else if (showWarning)
                {
                    Forms.FormsHelper.ShowErrorMsg("Невозможно найти точку");
                }
            }
        }

        private void CenterScaledImage(float scaleFactor)
        {
            InitScrollBars(scaleFactor);

            float visibleImageWidthHalf = Math.Min(_form.Canvas.Width, _file.Width) / scaleFactor / 2;
            visibleImageWidthHalf = _scaler.ScaleFactor > scaleFactor ? -visibleImageWidthHalf / 2 : visibleImageWidthHalf;

            float visibleImageHeightHalf = Math.Min(_form.Canvas.Height, _file.Height) / scaleFactor / 2;
            visibleImageHeightHalf = _scaler.ScaleFactor > scaleFactor ? -visibleImageHeightHalf / 2 : visibleImageHeightHalf;

            float newHorizontalValue = _form.Horizontal.Value + visibleImageWidthHalf < 0 ? 0 :
                _form.Horizontal.Value + visibleImageWidthHalf;
            newHorizontalValue = newHorizontalValue + _form.Canvas.Width /
                (_scaler.ScaleFactor > scaleFactor ? _scaler.ScaleFactor : scaleFactor) > _file.Width ?
                _form.Horizontal.Maximum : newHorizontalValue;

            float newVerticalValue = _form.Vertical.Value + visibleImageHeightHalf < 0 ? 0
                : _form.Vertical.Value + visibleImageHeightHalf;
            newVerticalValue = newVerticalValue + _form.Canvas.Height /
                (_scaler.ScaleFactor > scaleFactor ? _scaler.ScaleFactor : scaleFactor) > _file.Height ?
                _form.Vertical.Maximum : newVerticalValue;

            _form.Horizontal.Value = (int)Math.Round(newHorizontalValue);
            _form.Vertical.Value = (int)Math.Round(newVerticalValue);
        }

        private void ProgressReporter(int progress)
        {
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Value = progress; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(_form.ProgressLabel, pl =>
            { pl.Text = string.Format("{0} %", progress.ToString()); });
        }

        private void BlockAlignButton()
        {
            if (_pointSelector != null)
            {
                var selectedPointsCount = _pointSelector.Union(_areaAligningWrapper.Select(x => x.SelectedPoint)).Count();

                if (_settings.SurfaceType == Behaviors.ImageAligning.Surfaces.SurfaceType.Custom)
                {
                    if (selectedPointsCount == 3 || selectedPointsCount == 4 || selectedPointsCount == 5 || selectedPointsCount == 16)
                    {
                        _form.AlignBtn.Enabled = true;
                    }
                    else
                    {
                        _form.AlignBtn.Enabled = false;
                    }
                }
                else if (selectedPointsCount >= 3 && selectedPointsCount <= 16)
                {
                    _form.AlignBtn.Enabled = true;
                }
                else
                {
                    _form.AlignBtn.Enabled = false;
                }
            }
        }

        public void ToggleNavigation()
        {
            TogglePanel(_form.NavigationPanelCb.Checked, _form.NaviSplitter);
        }

        public void ToggleFilters()
        {
            TogglePanel(_form.FilterPanelCb.Checked, _form.FilterSplitter);
        }

        private void TogglePanel(bool isPanelOpen, SplitContainer sp)
        {
            if (isPanelOpen)
            {
                sp.Panel2Collapsed = false;
            }
            else
            {
                sp.Panel2Collapsed = true;
            }
            InitDrawImage();
        }

        private void InitializeWindow()
        {
            _file = null;
            _drawer = null;
            _pointSelector = null;
            _areaSelector = null;
            _tiles = null;

            ThreadHelper.ThreadSafeUpdate<PictureBox>(_form.Canvas).Image = null;
            ThreadHelper.ThreadSafeUpdate<HScrollBar>(_form.Horizontal).Visible = false;
            ThreadHelper.ThreadSafeUpdate<HScrollBar>(_form.Horizontal).SmallChange = 1;
            ThreadHelper.ThreadSafeUpdate<HScrollBar>(_form.Horizontal).LargeChange = 1;
            ThreadHelper.ThreadSafeUpdate<VScrollBar>(_form.Vertical).Visible = false;
            ThreadHelper.ThreadSafeUpdate<VScrollBar>(_form.Vertical).SmallChange = 1;
            ThreadHelper.ThreadSafeUpdate<VScrollBar>(_form.Vertical).LargeChange = 1;
            ThreadHelper.ThreadSafeUpdate<TrackBar>(_form.FilterTrackBar).SmallChange = 1;
            ThreadHelper.ThreadSafeUpdate<TrackBar>(_form.FilterTrackBar).LargeChange = 1;
            ThreadHelper.ThreadSafeUpdate<TrackBar>(_form.FilterTrackBar).Minimum = -16;
            ThreadHelper.ThreadSafeUpdate<TrackBar>(_form.FilterTrackBar).Maximum = 16;
            ThreadHelper.ThreadSafeUpdate<TrackBar>(_form.FilterTrackBar).Value = 0;
            ThreadHelper.ThreadSafeUpdate<DataGridView>(_form.NavigationDgv).AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            ThreadHelper.ThreadSafeUpdate<Label>(_form.ScaleLabel).Text = string.Format("Масштаб: {0}%", _scaler.ScaleFactor * 100);
            ThreadHelper.ThreadSafeUpdate<Button>(_form.AlignBtn).Enabled = false;
            ThreadHelper.ThreadSafeUpdate<System.Windows.Forms.DataVisualization.Charting.Chart>(_form.HistogramChart)
                .Series[0].Points.Clear();

            InitProgressControls(false);
            AddToolTips(_form);
            _chart.InitChart(_form.HistogramChart);
        }

        private void InitProgressControls(bool isVisible, string caption = "")
        {
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Visible = isVisible; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Value = 0; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(_form.StatusLabel, lbl => { lbl.Text = caption; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(_form.StatusLabel, lbl => { lbl.Visible = isVisible; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(_form.ProgressLabel, pl => { pl.Visible = isVisible; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(_form.ProgressLabel, pl => { pl.Text = "0%"; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripDropDownButton>(_form.CancelButton, cb => { cb.Visible = isVisible; });
        }

        private void AddToolTips(ISuitableForm frm)
        {
            Forms.FormsHelper.AddToolTip(frm.AlignBtn, "Выровнять");
            Forms.FormsHelper.AddToolTip(frm.AnalyzePointRb, "Анализ амплитуды");
            Forms.FormsHelper.AddToolTip(frm.DragRb, "Перемещение по изображению");
            Forms.FormsHelper.AddToolTip(frm.HorizontalSectionRb, "Горизонтальное сечение");
            Forms.FormsHelper.AddToolTip(frm.LinearSectionRb, "Произвольное сечение");
            Forms.FormsHelper.AddToolTip(frm.MarkAreaRb, "Область");
            Forms.FormsHelper.AddToolTip(frm.MarkPointRb, "Отметка");
            Forms.FormsHelper.AddToolTip(frm.NavigationPanelCb, "Навигация");
            Forms.FormsHelper.AddToolTip(frm.RulerRb, "Линейка");
            Forms.FormsHelper.AddToolTip(frm.FindPointBtn, "Поиск точки");
            Forms.FormsHelper.AddToolTip(frm.VerticalSectionRb, "Вертикальное сечение");
            Forms.FormsHelper.AddToolTip(frm.BrightnessRb, "Яркость");
            Forms.FormsHelper.AddToolTip(frm.ContrastRb, "Контрастность");
            Forms.FormsHelper.AddToolTip(frm.GammaRb, "Гамма");
            Forms.FormsHelper.AddToolTip(frm.ResetFilter, "Сброс фильтров");
            Forms.FormsHelper.AddToolTip(frm.FilterPanelCb, "Фильтры");
            Forms.FormsHelper.AddToolTip(frm.ZoomInBtn, "Увеличить масштаб");
            Forms.FormsHelper.AddToolTip(frm.ZoomOutBtn, "Уменьшить масштаб");
            Forms.FormsHelper.AddToolTip(frm.StatisticsBtn, "Статистика");
            Forms.FormsHelper.AddToolTip(frm.SquareAreaRb, "Трехмерный график");
            Forms.FormsHelper.AddToolTip(frm.SharerRb, "Сравнить точки");
        }


        private void InitScrollBars(float scaleFactor)
        {
            var f = _file as RlViewer.Files.LocatorFile;

            var horMax = (int)(f.Width - Math.Ceiling(_form.Canvas.Size.Width / scaleFactor));
            var verMax = (int)(f.Height - Math.Ceiling(_form.Canvas.Size.Height / scaleFactor));
            _form.Horizontal.Maximum = horMax > 0 ? horMax : 0;
            _form.Vertical.Maximum = verMax > 0 ? verMax : 0;
            _form.Horizontal.Visible = _form.Horizontal.Maximum > 0 ? true : false;
            _form.Vertical.Visible = _form.Vertical.Maximum > 0 ? true : false;

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
                        using (var sSize = new Forms.SaveForm(_file.Width, _file.Height, _areaSelector))
                        {
                            if (sSize.ShowDialog() == DialogResult.OK)
                            {
                                StartTask("Сохранение", loaderWorker_SaveFile, loaderWorker_SaveFileCompleted,
                                    new Behaviors.Saving.SaverParams(sfd.FileName, sSize.LeftTop,
                                        sSize.ImageWidth, sSize.ImageHeight, sSize.KeepFiltering, sSize.KeepPalette));
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
        /// <param name="filterType"></param>
        /// <param name="filterDelta"></param>
        public void GetFilter(Behaviors.Filters.FilterType filterType, int filterDelta)
        {
            _filterProxy.GetFilter(filterType, filterDelta);
            _form.FilterTrackBar.Value = _filterProxy.Filter.FilterValue >> filterDelta;
        }

        public void ResetFilter()
        {
            _filterProxy.ResetFilters();
            _form.FilterTrackBar.Value = 0;
        }
        #endregion

        #region mouseHandlers

        public void ShowMousePosition(MouseEventArgs e)
        {
            string pos = string.Empty;
            if (_file != null && !_worker.IsBusy)
            {
                if (e.Y / _scaler.ScaleFactor + _form.Vertical.Value > 0 &&
                    e.Y / _scaler.ScaleFactor + _form.Vertical.Value < _file.Height
                    && e.X > 0 && e.X / _scaler.ScaleFactor < _file.Width)
                {
                    pos = string.Format("X:{0} Y:{1}",
                        (int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value, (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);
                }
            }
            _form.CoordinatesLabel.Text = pos;
        }



        public void ShowNavigation(MouseEventArgs e)
        {
            if (_file != null && !_worker.IsBusy)
            {

                if (_form.NavigationPanelCb.Checked && _file.Navigation != null)
                {
                    if (e.Y / _scaler.ScaleFactor + _form.Vertical.Value > 0
                        && e.Y / _scaler.ScaleFactor + _form.Vertical.Value < _file.Height
                        && e.X > 0 && e.X / _scaler.ScaleFactor < _file.Width)
                    {

                        _form.NavigationDgv.Rows.Clear();

                        foreach (var i in _file.Navigation[(int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value,
                            (int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value])
                        {
                            _form.NavigationDgv.Rows.Add(i.Item1, i.Item2);
                        }
                    }
                }

                else
                {
                    _form.NavigationDgv.Rows.Clear();
                }

            }
        }



        public void ClickStarted(MouseEventArgs e)
        {
            if (_file != null && _drawer != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (_form.DragRb.Checked)
                    {
                        _form.Canvas.Cursor = Cursors.SizeAll;
                        _drag.StartTracing(new Point((int)(e.X / _scaler.ScaleFactor), (int)(e.Y / _scaler.ScaleFactor)), !_form.MarkPointRb.Checked);
                        return;
                    }
                    else if (_form.MarkAreaRb.Checked)
                    {
                        _areaSelector.ResetArea();
                        _areaSelector.StartArea(new Point((int)Math.Ceiling(e.X / _scaler.ScaleFactor), (int)Math.Ceiling(e.Y / _scaler.ScaleFactor)),
                            new Point((int)(_form.Horizontal.Value), (int)(_form.Vertical.Value)));
                    }
                    else if (_form.SharerRb.Checked)
                    {
                        var clickedPoint = new Point(_form.Horizontal.Value + ((int)(e.X / _scaler.ScaleFactor)),
                        _form.Vertical.Value + ((int)(e.Y / _scaler.ScaleFactor)));
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
                    else if (_form.MarkPointRb.Checked)
                    {
                        if (!_settings.UsePointsForAligning)
                        {

                            try
                            {
                                _pointSelector.Add((RlViewer.Files.LocatorFile)_file,
                                    new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                        (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value), new Size(_settings.SelectorAreaSize, _settings.SelectorAreaSize));
                            }
                            catch (Exception)
                            {
                                Forms.FormsHelper.ShowErrorMsg("Unable to set point");
                            }
                        }
                        else
                        {
                            _selectedPointArea = new Behaviors.AreaSelector.AreaSelectorDecorator(_file, _settings.MaxAlignerAreaSize);
                            _areaAligningWrapper.AddArea(_selectedPointArea);
                            _selectedPointArea.ResetArea();
                            _selectedPointArea.StartArea(new Point((int)Math.Ceiling(e.X / _scaler.ScaleFactor), (int)Math.Ceiling(e.Y / _scaler.ScaleFactor)),
                                new Point((int)(_form.Horizontal.Value), (int)(_form.Vertical.Value)));
                        }

                        BlockAlignButton();
                    }
                    else if (_form.AnalyzePointRb.Checked)
                    {
                        _analyzer.StartTracing();
                        AnalyzePoint(e);
                    }
                    else if (_form.VerticalSectionRb.Checked)
                    {
                        _section = new Behaviors.Sections.Concrete.VerticalSection(_settings.SectionSize, new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                    else if (_form.HorizontalSectionRb.Checked)
                    {
                        _section = new Behaviors.Sections.Concrete.HorizontalSection(_settings.SectionSize, new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                    else if (_form.LinearSectionRb.Checked)
                    {
                        _section = new Behaviors.Sections.Concrete.LinearSection(_settings.SectionSize, new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                    else if (_form.RulerRb.Checked)
                    {
                        if (!_ruler.Pt1Fixed)
                        {
                            _ruler.Pt1 = new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                    (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);
                        }
                        else if (!_ruler.Pt2Fixed)
                        {
                            _ruler.Pt2 = new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                                               (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);
                        }
                    }
                    //else if (_form.SquareAreaRb.Checked)
                    //{
                    //    _squareArea = new Rectangle(new Point((int)(e.X / _scaler.ScaleFactor - (int)(_settings.Plot3dAreaBorderSize / 2)) + _form.Horizontal.Value,
                    //                                           (int)(e.Y / _scaler.ScaleFactor - (int)(_settings.Plot3dAreaBorderSize / 2)) + _form.Vertical.Value),
                    //                                           new Size(_settings.Plot3dAreaBorderSize, _settings.Plot3dAreaBorderSize));

                    //    try
                    //    {
                    //        Show3dPlot(_squareArea);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Unable to build 3d plot: {0}", ex.Message));
                    //        Forms.FormsHelper.ShowErrorMsg("Невозможно построить график");
                    //    }
                    //}

                }
                else if (e.Button == MouseButtons.Right)
                {
                    _form.Canvas.Cursor = Cursors.SizeAll;
                    _drag.StartTracing(new Point((int)(e.X / _scaler.ScaleFactor), (int)(e.Y / _scaler.ScaleFactor)), true);
                }
            }
        }

        public void TraceMouseMovement(MouseEventArgs e)
        {
            if (_file != null && _drawer != null)
            {
                if (_drag.Trace(new Point((int)(e.X / _scaler.ScaleFactor), (int)(e.Y / _scaler.ScaleFactor))))
                {
                    var newHor = _form.Horizontal.Value - _drag.Delta.X * _settings.DragAccelerator;
                    var newVert = _form.Vertical.Value - _drag.Delta.Y * _settings.DragAccelerator;

                    newVert = newVert < 0 ? 0 : newVert;
                    newVert = newVert > _form.Vertical.Maximum ? _form.Vertical.Maximum : newVert;
                    _form.Vertical.Value = newVert;

                    newHor = newHor < 0 ? 0 : newHor;
                    newHor = newHor > _form.Horizontal.Maximum ? _form.Horizontal.Maximum : newHor;
                    _form.Horizontal.Value = newHor;

                    DrawImage();
                }
                else if (_form.MarkAreaRb.Checked && _areaSelector != null)
                {

                    if (_areaSelector.ResizeArea(new Point((int)(e.X / _scaler.ScaleFactor),
                                (int)(e.Y / _scaler.ScaleFactor)), new Point(_form.Horizontal.Value, _form.Vertical.Value)))
                    {
                        _form.Canvas.Invalidate();
                    }
                }
                else if (_form.MarkPointRb.Checked)
                {
                    if (_settings.UsePointsForAligning && _selectedPointArea != null)
                    {
                        if (_selectedPointArea.ResizeArea(new Point((int)(e.X / _scaler.ScaleFactor),
                                     (int)(e.Y / _scaler.ScaleFactor)), new Point(_form.Horizontal.Value, _form.Vertical.Value)))
                        {
                            _form.Canvas.Invalidate();
                        }
                    }
                    else
                    {
                        var leftTop = new Point(e.X - (int)(_settings.SelectorAreaSize * _scaler.ScaleFactor / 2),
                        e.Y - (int)(_settings.SelectorAreaSize * _scaler.ScaleFactor / 2));
                        _form.Canvas.Image = _drawer.DrawSquareArea(leftTop, _settings.SelectorAreaSize);
                    }
                }
                //else if (_form.SquareAreaRb.Checked && _drawer != null)
                //{
                //    var leftTop = new Point(e.X - (int)(_settings.Plot3dAreaBorderSize * _scaler.ScaleFactor / 2),
                //        e.Y - (int)(_settings.Plot3dAreaBorderSize * _scaler.ScaleFactor / 2));
                //    _form.Canvas.Image = _drawer.DrawSquareArea(leftTop, _settings.Plot3dAreaBorderSize);
                //}
                else if (_form.AnalyzePointRb.Checked && _analyzer != null)
                {
                    AnalyzePoint(e);
                }
                else if (_form.VerticalSectionRb.Checked && _drawer != null)
                {
                    _form.Canvas.Image = _drawer.DrawVerticalSection(e.Location, (int)(_settings.SectionSize * _scaler.ScaleFactor));
                }
                else if (_form.HorizontalSectionRb.Checked && _drawer != null)
                {
                    _form.Canvas.Image = _drawer.DrawHorizontalSection(e.Location, (int)(_settings.SectionSize * _scaler.ScaleFactor));
                }
                else if (_form.RulerRb.Checked && _drawer != null)
                {
                    if (_ruler.Pt1Fixed || _ruler.Pt2Fixed)
                    {
                        var endPoint = _ruler.Pt2Fixed ? _ruler.Pt2 : new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                                               (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);

                        _form.Canvas.Image = _drawer.DrawRuler(new Point((int)((_ruler.Pt1.X - _form.Horizontal.Value) * _scaler.ScaleFactor),
                            (int)((_ruler.Pt1.Y - _form.Vertical.Value) * _scaler.ScaleFactor)),
                            new Point((int)((endPoint.X - _form.Horizontal.Value) * _scaler.ScaleFactor),
                             (int)((endPoint.Y - _form.Vertical.Value) * _scaler.ScaleFactor)));

                        _form.DistanceLabel.Text = _ruler.GetDistance(endPoint);
                    }
                }
                else if (_form.LinearSectionRb.Checked && _drawer != null)
                {
                    if (_section != null && _section.InitialPoint != default(Point))
                    {
                        var endPoint = new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                                               (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);

                        _form.Canvas.Image = _drawer.DrawLinearSection(
                            new Point((int)((_section.InitialPoint.X - _form.Horizontal.Value) * _scaler.ScaleFactor),
                             (int)((_section.InitialPoint.Y - _form.Vertical.Value) * _scaler.ScaleFactor)),
                            new Point((int)((endPoint.X - _form.Horizontal.Value) * _scaler.ScaleFactor),
                             (int)((endPoint.Y - _form.Vertical.Value) * _scaler.ScaleFactor)));
                    }
                }
            }
        }


        private void AnalyzePoint(MouseEventArgs e)
        {
            try
            {
                if (_analyzer.Analyze(_file, new Point((int)(e.X / _scaler.ScaleFactor)
                     + _form.Horizontal.Value, (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value)))
                {
                    _toolTip.Show(string.Format("Амплитуда: {0}", _analyzer.Amplitude.ToString()),
                           _win, new Point(e.Location.X, e.Location.Y - 20));
                }

            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Point analyzing failed with message: {0}", ex.Message));
                Forms.FormsHelper.ShowErrorMsg("Невозможно проанализировать точку");
            }
        }



        public void ClickFinished(MouseEventArgs e)
        {
            if (_file != null && _drag != null)
            {
                _form.Canvas.Cursor = Cursors.Arrow;
                _drag.StopTracing();
                if (e.Button == MouseButtons.Left && _areaSelector != null && _analyzer != null)
                {
                    if (_areaSelector.IsActive)
                    {
                        _areaSelector.StopResizing();
                    }

                    _analyzer.StopTracing();
                    _toolTip.Hide(_win);

                    if (_settings.UsePointsForAligning)
                    {
                        if (_form.MarkPointRb.Checked)
                        {
                            using (Forms.EprInputForm epr = new Forms.EprInputForm())
                            {
                                if (epr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    _selectedPointArea.StopResizing(epr.EprValue);
                                }
                            }
                            if (_selectedPointArea.SelectedPoint == null)
                            {
                                _areaAligningWrapper.RemoveArea();
                            }
                        }
                    }

                    if (_form.LinearSectionRb.Checked || _form.VerticalSectionRb.Checked || _form.HorizontalSectionRb.Checked)
                    {
                        ShowSection(_section, new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }

                }

                _form.Canvas.Invalidate();
            }

        }

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

            OnImageDrawn -= (s, img) => _form.Canvas.Image = img;
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
                        BlockAlignButton();
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

        public void AlignImage()
        {
            //new Behaviors.ImageAligning.LeastSquares.Concrete.PolynomialLeastSquares(_pointSelector)
            using (var alignedSaveDlg = new SaveFileDialog())
            {
                alignedSaveDlg.Filter = "Обработанные файлы|*.brl4;*.raw";
                if (alignedSaveDlg.ShowDialog() == DialogResult.OK)
                {
                    var selectedPoints = _pointSelector.Union(_areaAligningWrapper.Select(x => x.SelectedPoint));

                    var compressedSelector = new Behaviors.PointSelector.CompressedPointSelectorWrapper(_file,
                       selectedPoints, (int)_settings.RangeCompressionCoef, (int)_settings.AzimuthCompressionCoef);

                    _aligner = new Behaviors.ImageAligning.Aligning(_file, compressedSelector,
                        new Behaviors.Interpolators.LeastSquares.Concrete.LinearLeastSquares(compressedSelector),
                        _settings.SurfaceType, _settings.RbfMlBaseRaduis, _settings.RbfMlLayersNumber, _settings.RbfMlRegularizationCoef);

                    StartTask("Выравнивание изображения", loaderWorker_AlignImage, loaderWorker_AlignImageCompleted,
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
                            StartTask("Поиск точки", loaderWorker_FindPoint, loaderWorker_FindPointCompleted, ff.Params);
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
                _form.DistanceLabel.Text = string.Empty;
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
                Forms.FormsHelper.ShowErrorMsg("Невозможно построить статистику области");
                return;
            }
            else if (_areaSelector.Area.Width >= _settings.AligningAreaBorderSize || _areaSelector.Area.Height >= _settings.AligningAreaBorderSize)
            {
                Forms.FormsHelper.ShowErrorMsg(string.Format("Выделен слишком большой участок (максимум: {0})", _settings.AligningAreaBorderSize));
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
                Forms.FormsHelper.ShowErrorMsg("Unable to determine area statistics");
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
            using (var ofd = new OpenFileDialog() { Title = "Выберите исходный файл Банк-РЛ", Filter = "РЛИ Банк-РЛ (*.brl4)|*.brl4;" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    EmbedNavigation(ofd.FileName, true);
                }
            }
        }

        private void EmbedNavigation(string brl4FileName, bool forced = true)
        {
            Behaviors.Navigation.NavigationChanger.Brl4NavigationChanger naviChanger = null;


            try
            {
                naviChanger = new Behaviors.Navigation.NavigationChanger.Brl4NavigationChanger(brl4FileName);
            }
            catch (InvalidCastException icex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error,
                    string.Format("Provided header is not of brl4 type: {0}", icex.Message));
                return;
            }
            catch (ArgumentException aex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error,
                    string.Format("Unable to parse blr4 header: {0}", aex.Message));
                return;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error,
                    string.Format("Unknown brl4 opening exception: {0}", ex.Message));
                return;
            }


            if (!forced)
            {
                if (!naviChanger.CheckIsBaRhg())
                {
                    return;
                }
            }

            using (var ofd = new OpenFileDialog() { Title = "Выберите исходный файл РГГ", Filter = "РГГ Ba (*.ba)|*.ba;" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        naviChanger.ChangeNavigation(ofd.FileName);
                        Logging.Logger.Log(Logging.SeverityGrades.Info,
                            string.Format("Successfully applied navigation to {0}", brl4FileName));
                    }
                    catch (Exception ex)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Error,
                            string.Format("Unable to embed new navigation: {0}", ex.Message));
                        Forms.FormsHelper.ShowErrorMsg("Не удалось вшить навигацию");
                    }
                }
            }
        }



        public void ReportDialog()
        {
            using (var reportSettings = new Forms.ReportSettingsForm())
            {
                if (reportSettings.ShowDialog() == DialogResult.OK)
                {
                    MakeReport(reportSettings.ReporterType, reportSettings.ReporterSettings);
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
                        Forms.FormsHelper.ShowErrorMsg("Unsuitable files selected");
                        return;
                    }

                    using (var fsd = new SaveFileDialog())
                    {
                        fsd.Title = "Имя для файла отчета";
                        fsd.Filter = "Документ MS Word|.docx";
                        if (fsd.ShowDialog() == DialogResult.OK)
                        {
                            _reporter = Factories.Reporter.Abstract.ReporterFactory
                                                .GetFactory(reportType)
                                                .Create(validFiles.ToArray());

                            StartTask("Генерация отчета", loaderWorker_GenerateReport,
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
                Forms.FormsHelper.ShowErrorMsg("Невозможно построить сечение");
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
                        Forms.FormsHelper.ShowErrorMsg("Запуск в режиме администратора невозможен");
                    }
                }
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Application started in admin mode");
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
                _pointSharer.ProcessMessage(gde.Data, (point) => CenterImageAtPoint(point, false));
            }
        }

    }
}
