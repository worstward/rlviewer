using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using RlViewer.Behaviors.TileCreator.Abstract;
using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Factories.File.Abstract;
using RlViewer.Factories.Saver.Abstract;
using RlViewer.Navigation;
using RlViewer.Settings;
using RlViewer.Behaviors;

namespace RlViewer.UI
{
    public class GuiFacade
    {
        public GuiFacade(ISuitableForm form)
        {
            _form = form;

            _settings = new Settings.Settings();

            try
            {
                _settings = _settings.FromXml();
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, "Settings file is corrupted, loading default values");
                _settings.ToXml();
            }

            _filterFacade = new Behaviors.Filters.ImageFilterFacade();
            _scaler = new Behaviors.Scaling.Scaler();
            _drag = new Behaviors.DragController();
            _histogram = new Behaviors.Draw.HistContainer();

            _form.DragRb.Checked = true;

            _win = _form.Canvas;

            _form.Canvas.Paint += (s, e) =>
                {
                    DrawItems(e.Graphics);
                };

            //SetDoubleBuffered(_form.Canvas);

            InitializeWindow();
        }



        private Settings.Settings _settings;
        private ITileCreator _creator;

        private Behaviors.Filters.ImageFilterFacade _filterFacade;
        private Behaviors.Saving.Abstract.Saver _saver;
        private Behaviors.Scaling.Scaler _scaler;
        private Behaviors.Analyzing.Abstract.SampleAnalyzer _analyzer;
        private Behaviors.Ruler.RulerFacade _ruler;
        private Behaviors.ImageAligning.Aligning _aligner;
        private Behaviors.Sections.Abstract.Section _section;
        private Behaviors.Navigation.Abstract.GeodesicPointFinder _searcher;
        private WorkerEventController _cancellableAction;

        private Files.FileProperties _properties;
        private Headers.Abstract.LocatorFileHeader _header;
        private Navigation.NavigationContainer _navi;
        private Behaviors.Draw.HistContainer _histogram;

        private Files.LocatorFile _file;
        private Behaviors.TileCreator.Tile[] _tiles;
        private Behaviors.Draw.DrawerFacade _drawer;
        private Behaviors.PointSelector.PointSelector _pointSelector;
        private Behaviors.AreaSelector.AreaSelectorWrapper _selectedPointArea;
        private Behaviors.AreaSelector.AreaSelectorsAlignerContainer _areaAligningWrapper;

        private Behaviors.AreaSelector.AreaSelector _areaSelector;
        private Behaviors.DragController _drag;

        private ISuitableForm _form;
        private System.ComponentModel.BackgroundWorker _worker;
        private string _caption = string.Empty;
        private ToolTip _toolTip = new ToolTip();
        private IWin32Window _win;


        #region OpenFile
        public string OpenWithDoubleClick()
        {
            var fName = Environment.GetCommandLineArgs().Where(x => Path.GetExtension(x) != ".exe").FirstOrDefault();
            return fName == null ? string.Empty : OpenFile(fName);
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

        private string OpenFile(string fileName)
        {
            string caption;

            if (_worker != null && _worker.IsBusy)
            {
                CancelLoading();
            }


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
                    return _file.Properties.FilePath;
                }
            }


            InitializeWindow();
            StartTask("Чтение навигации", loaderWorker_InitFile, loaderWorker_InitFileCompleted, fileName);
            _form.NavigationDgv.Rows.Clear();
            caption = _caption = fileName;
            return caption; 

        }
        #endregion


        public void GetImage()
        {
            if (_file != null)
            {
                StartTask("Генерация тайлов", loaderWorker_CreateTiles, loaderWorker_CreateTilesCompleted);
            }
        }

        public void CancelLoading()
        {
            if (_worker != null && _cancellableAction != null)
            {
                _cancellableAction.Cancelled = true;
            }
        }

        private void StartTask(string caption, System.ComponentModel.DoWorkEventHandler d,
            System.ComponentModel.RunWorkerCompletedEventHandler c, object arg = null)
        {

            InitProgressControls(true, caption);
            _worker = ThreadHelper.InitWorker(d, c);
            _worker.RunWorkerAsync(arg);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposer">Action that clears all redundant data after task cancelling</param>
        private void ClearWorkerData(Action disposer)
        {
            if (_worker != null && _file != null)
            {
                disposer();
            }

            _worker.Dispose();
        }

        private void ClearCancelledFileTiles()
        {
            try
            {
                var path = _creator.GetDirectoryName(_file.Properties.FilePath);

                if (Directory.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                ErrorGuiMessage(ex.Message);
            }

        }


        private void ClearCancelledSaving(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, "Unable to delete saved file");
                ErrorGuiMessage(ex.Message);
            }
        }


        private void loaderWorker_FindPoint(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            _searcher.Report += (s, pe) => ProgressReporter(pe.Percent);
            _searcher.CancelJob += (s, ce) => ce.Cancel = _searcher.Cancelled;
            _cancellableAction = _searcher;
            var xy = (Tuple<string, string>)(e.Argument);
            int x;
            if (Int32.TryParse(xy.Item1, out x))
            {
                e.Result = new Point(Convert.ToInt32(xy.Item1), Convert.ToInt32(xy.Item2));
            }
            else
            {
                e.Result = _searcher.GetCoordinates(NaviStringConverters.ParseToRadians(xy.Item1),
                    NaviStringConverters.ParseToRadians(xy.Item2));
            }

        }

        private void loaderWorker_FindPointCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            _searcher.Report -= (s, pe) => ProgressReporter(pe.Percent);
            _searcher.CancelJob -= (s, ce) => ce.Cancel = _searcher.Cancelled;

            if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Error searching point: {0}", e.Error.Message));
                ErrorGuiMessage("Unable to find point");
            }
            else
            {
                var foundPoint = (Point)e.Result;

                if (foundPoint != default(Point) &&
                    foundPoint.X > 0 && foundPoint.X < _file.Width && foundPoint.Y > 0 && foundPoint.Y < _file.Height)
                {

                    var horValue = (((Point)e.Result).X - (int)(_form.Canvas.Width / 2 / _scaler.ScaleFactor));
                    horValue = horValue < 0 ? 0 : horValue;
                    horValue = horValue > _form.Horizontal.Maximum ? _form.Horizontal.Maximum : horValue;

                    _form.Horizontal.Value = horValue;

                    var vertValue = (((Point)e.Result).Y - (int)(_form.Canvas.Height / 2 / _scaler.ScaleFactor));
                    vertValue = vertValue < 0 ? 0 : vertValue;
                    vertValue = vertValue > _form.Vertical.Maximum ? _form.Vertical.Maximum : vertValue;

                    _form.Vertical.Value = vertValue;
                    _form.Canvas.Invalidate();
                }
                else
                {
                    ErrorGuiMessage("Невозможно найти точку");
                }
            }


            InitProgressControls(false);
        }



        private void loaderWorker_SaveFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] args;
            string path;
            Point leftTop;
            int width;
            int height;
            bool keepFiltering;


            try
            {
                args = (object[])e.Argument;
                path = (string)args[0];
                leftTop = (Point)args[1];
                width = (int)args[2];
                height = (int)args[3];
                keepFiltering = (bool)args[4];
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

            var filter = keepFiltering ? _filterFacade : null;

            _saver.Save(path, Path.GetExtension(path).Replace(".", "")
                .ToEnum<RlViewer.FileType>(), new Rectangle(leftTop.X, leftTop.Y, width, height),
                filter, _creator.NormalizationFactor, _creator.MaxValue);


            if (_saver.Cancelled)
            {
                ClearWorkerData(() => ClearCancelledSaving(path));
            }

            e.Cancel = _saver.Cancelled;
            e.Result = path;
        }

        private void loaderWorker_SaveFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _saver.Report -= (s, pe) => ProgressReporter(pe.Percent);
            _saver.CancelJob -= (s, ce) => ce.Cancel = _saver.Cancelled;


            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Saving cancelled");
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Error saving image: {0}", e.Error.Message));
                ErrorGuiMessage("Unable to save image");
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Saving completed: {0}", (string)e.Result));
            }

            InitProgressControls(false);
        }

        private void loaderWorker_AlignImage(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var args = (object[])e.Argument;
            var fileName = (string)args[0];

            _aligner.Report += (s, pe) => ProgressReporter(pe.Percent);
            _aligner.CancelJob += (s, ce) => ce.Cancel = _aligner.Cancelled;
            _cancellableAction = _aligner;
            _aligner.Resample(fileName);
            e.Cancel = _aligner.Cancelled;
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
                Logging.Logger.Log(Logging.SeverityGrades.Error, string.Format("Error aligning image: {0}", e.Error.Message));
                ErrorGuiMessage("Unable to align image");
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info,
                    string.Format("Image aligning completed, new file saved at: {0}", Path.GetFullPath((string)e.Result)));
            }
            InitProgressControls(false);
        }

        private void loaderWorker_InitFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            _properties = new Files.FileProperties((string)e.Argument);
            _header = Factories.Header.Abstract.HeaderFactory.GetFactory(_properties).Create(_properties.FilePath);

            _navi = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(_properties).Create(_properties, _header);

            _navi.Report += (s, pe) => ProgressReporter(pe.Percent);
            _navi.CancelJob += (s, ce) => ce.Cancel = _navi.Cancelled;
            _cancellableAction = _navi;
            _navi.GetNavigation();
            e.Cancel = _navi.Cancelled;

            _file = FileFactory.GetFactory(_properties).Create(_properties, _header, _navi);
            _ruler = new Behaviors.Ruler.RulerFacade(_file);
            _saver = SaverFactory.GetFactory(_properties).Create(_file);
            _searcher = Factories.NavigationSearcher.Abstract.PointFinderFactory.GetFactory(_file.Properties).Create(_file);
            _analyzer = Factories.Analyzer.AnalyzerFactory.Create(_file);
            _pointSelector = new Behaviors.PointSelector.PointSelector();
            _areaSelector = new Behaviors.AreaSelector.AreaSelector(_file);
            _areaAligningWrapper = new Behaviors.AreaSelector.AreaSelectorsAlignerContainer();
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
                string errMess = string.Format("File opening cancelled");
                Logging.SeverityGrades grade = Logging.SeverityGrades.Info;

                if (e.Error.GetType() != typeof(OperationCanceledException))
                {
                    grade = Logging.SeverityGrades.Blocking;
                    errMess = string.Format("Error opening file: {0}", e.Error.Message);
                    ErrorGuiMessage("Unable to open file");
                }

                Logging.Logger.Log(grade, errMess);
                InitializeWindow();
            }
            else
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("{0} file opened: {1}",
                    _file.Properties.Type, _file.Properties.FilePath));
                GetImage();
            }
        }


        private void loaderWorker_CreateTiles(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _creator = TileCreatorFactory.GetFactory(_file).Create(_file as RlViewer.Files.LocatorFile, _settings.TileOutputAlgorithm);

            ((WorkerEventController)_creator).Report += (s, pe) => ProgressReporter(pe.Percent);
            ((WorkerEventController)_creator).CancelJob += (s, ce) => ce.Cancel = ((WorkerEventController)_creator).Cancelled;
            _cancellableAction = ((WorkerEventController)_creator);
            _tiles = _creator.GetTiles(_file.Properties.FilePath, _settings.ForceTileGeneration, _settings.AllowViewWhileLoading);
            e.Cancel = ((WorkerEventController)_creator).Cancelled;
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
                ClearWorkerData(() => ClearCancelledFileTiles());
                Logging.Logger.Log(Logging.SeverityGrades.Info, "Tile creation operation cancelled");
                InitializeWindow();
            }
            else if (e.Error != null)
            {
                ClearWorkerData(() => ClearCancelledFileTiles());
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error creating tiles: {0}",
                    e.Error.InnerException == null ? e.Error.Message : e.Error.InnerException.Message));
                ErrorGuiMessage("Unable to create tiles");
                InitializeWindow();
            }
            else
            {
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

        public void ScaleImage(int delta)
        {
            if (_file != null && _drawer != null)
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
        }



        private void ProgressReporter(int progress)
        {
            ThreadHelper.ThreadSafeUpdate<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Value = progress; });
            ThreadHelper.ThreadSafeUpdate<ToolStripStatusLabel>(_form.ProgressLabel, pl =>
            { pl.Text = string.Format("{0} %", progress.ToString()); });
        }

        private void BlockAlignButton()
        {
            var selectedPointsCount = _pointSelector.Union(_areaAligningWrapper.Select(x => x.SelectedPoint)).Count();

            if (selectedPointsCount == 3 || selectedPointsCount == 4 || selectedPointsCount == 16)
            {
                _form.AlignBtn.Enabled = true;
            }
            else
            {
                _form.AlignBtn.Enabled = false;
            }
        }


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

        public void InitDrawImage()
        {
            if (_form.Canvas.Size.Width != 0 && _form.Canvas.Size.Height != 0 && _tiles != null && _file != null)
            {
                var tDrawer = new Behaviors.Draw.TileDrawer(_filterFacade.Filter, _scaler);
                var iDrawer = new Behaviors.Draw.ItemDrawer(_pointSelector, _areaSelector, _scaler, _areaAligningWrapper);
                _drawer = new RlViewer.Behaviors.Draw.DrawerFacade(_form.Canvas.Size, iDrawer, tDrawer);

                ChangePalette(_settings.Palette, _settings.IsPaletteReversed,
                    _settings.IsPaletteGroupped, _settings.UseTemperaturePalette);
                InitScrollBars(_scaler.ScaleFactor);
                
                DrawImage();
            }
        }

        //private void InvalidateCanvas()
        //{
        //    Task.Run(() =>
        //    {
        //        while(true)
        //        {
        //            if (_messageList.Count != 0)
        //            {
        //                _form.Canvas.Image = _messageList.Take()();
        //                while (_messageList.Count > 0)
        //                {
        //                    var obj = _messageList.Take();
        //                }
        //            }
        //        }
        //    });
        //}

        //private System.Collections.Concurrent.BlockingCollection<Func<Image>> _messageList = new System.Collections.Concurrent.BlockingCollection<Func<Image>>();

        protected static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        public void DrawImage()
        {
            if (_tiles != null && _drawer != null)
            {
                Task.Run(() =>
                    {
                        _form.Canvas.Image = _drawer.Draw(_tiles,
                                new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value), _settings.HighResForDownScaled);
                    }).Wait();

                //Task.Run(() =>
                //    { 
                //        _messageList.Add(() => _drawer.Draw(_tiles,
                //               new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value), _settings.HighResForDownScaled));
                //    });
                RedrawChart(_form.HistogramChart, (Image)_form.Canvas.Image.Clone());
            }
        }

        private void DrawItems(Graphics g)
        {
            if (_tiles != null && _drawer != null)
            {
               _drawer.Draw(g, new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
            }
        }

        public void ChangePalette(float[] rgb, bool isReversed, bool isGrouped, bool useTemperaturePalette)
        {
            if (_drawer != null)
            {
                _drawer.GetPalette(rgb[0], rgb[1], rgb[2], isReversed, isGrouped, useTemperaturePalette);
            }
        }




        private void TogglePanel(bool isPanelOpen, SplitContainer sp)
        {
            if (isPanelOpen)
            {
                sp.Panel2Collapsed = false;
                //sp.Panel2.Show();
            }
            else
            {
                sp.Panel2Collapsed = true;
                //sp.Panel2.Hide();
            }
            InitDrawImage();
        }

        public void ToggleNavigation()
        {
            TogglePanel(_form.NavigationPanelCb.Checked, _form.NaviSplitter);
        }

        public void ToggleFilters()
        {
            TogglePanel(_form.FilterPanelCb.Checked, _form.FilterSplitter);
        }



        private string GetSaveDialogFilter(Files.LocatorFile file)
        {
            string filter = Resources.SaveFilter;
            
            if(file.Properties.Type == FileType.raw || file.Properties.Type == FileType.r)
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
                                    new object[] { sfd.FileName, sSize.LeftTop, sSize.ImageWidth, sSize.ImageHeight, sSize.KeepFiltering });
                            }
                        }
                    }
                }
            }

        }


        private void ChangeScaleFactor(float value)
        {
            if (value > Math.Log(_scaler.MaxZoom, 2) || value < Math.Log(_scaler.MinZoom, 2)) return;

            float scaleFactor = (float)Math.Pow(2, value);
            _form.ScaleLabel.Text = string.Format("Масштаб: {0}%", (scaleFactor * 100).ToString());

            CenterPointOfView(scaleFactor);

            _scaler = new Behaviors.Scaling.Scaler(scaleFactor);
            InitDrawImage();
        }


        private void CenterPointOfView(float scaleFactor)
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

            _form.Horizontal.Value = (int)newHorizontalValue;
            _form.Vertical.Value = (int)newVerticalValue;

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
            ThreadHelper.ThreadSafeUpdate<DataGridView>(_form.NavigationDgv).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ThreadHelper.ThreadSafeUpdate<Button>(_form.AlignBtn).Enabled = false;
            ThreadHelper.ThreadSafeUpdate<Chart>(_form.HistogramChart).Series[0].Points.Clear();

            InitProgressControls(false);
            AddToolTips(_form);
            InitChart(_form.HistogramChart);
        }

        private void InitProgressControls(bool isVisible, string caption = "")
        {
            ThreadHelper.ThreadSafeUpdate<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Visible = isVisible; });
            ThreadHelper.ThreadSafeUpdate<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Value = 0; });
            ThreadHelper.ThreadSafeUpdate<ToolStripStatusLabel>(_form.StatusLabel, lbl => { lbl.Text = caption; });
            ThreadHelper.ThreadSafeUpdate<ToolStripStatusLabel>(_form.StatusLabel, lbl => { lbl.Visible = isVisible; });
            ThreadHelper.ThreadSafeUpdate<ToolStripStatusLabel>(_form.ProgressLabel, pl => { pl.Visible = isVisible; });
            ThreadHelper.ThreadSafeUpdate<ToolStripStatusLabel>(_form.ProgressLabel, pl => { pl.Text = "0%"; });
            ThreadHelper.ThreadSafeUpdate<ToolStripDropDownButton>(_form.CancelButton, cb => { cb.Visible = isVisible; });
        }

        private void AddToolTips(ISuitableForm frm)
        {
            AddToolTip(frm.AlignBtn, "Выровнять");
            AddToolTip(frm.AnalyzePointRb, "Анализ амплитуды");
            AddToolTip(frm.DragRb, "Перемещение по изображению");
            AddToolTip(frm.HorizontalSectionRb, "Горизонтальное сечение");
            AddToolTip(frm.LinearSectionRb, "Произвольное сечение");
            AddToolTip(frm.MarkAreaRb, "Область");
            AddToolTip(frm.MarkPointRb, "Точка");
            AddToolTip(frm.NavigationPanelCb, "Навигация");
            AddToolTip(frm.RulerRb, "Линейка");
            AddToolTip(frm.FindPointBtn, "Поиск точки");
            AddToolTip(frm.VerticalSectionRb, "Вертикальное сечение");
            AddToolTip(frm.BrightnessRb, "Яркость");
            AddToolTip(frm.ContrastRb, "Контрастность");
            AddToolTip(frm.GammaRb, "Гамма");
            AddToolTip(frm.ResetFilter, "Сброс фильтров");
            AddToolTip(frm.FilterPanelCb, "Панель фильтров");
            AddToolTip(frm.ZoomInBtn, "Увеличить масштаб");
            AddToolTip(frm.ZoomOutBtn, "Уменьшить масштаб");
            AddToolTip(frm.StatisticsBtn, "Статистика");
        }

        private void AddToolTip(Control c, string caption)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(c, caption);
        }


        private void InitChart(Chart c)
        {
            c.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            c.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            c.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            c.ChartAreas[0].AxisX.Maximum = 265;
            c.ChartAreas[0].AxisX.Minimum = -10;
            c.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            c.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            c.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            c.ChartAreas[0].BackColor = Color.Transparent;
            c.ChartAreas[0].AxisY.Minimum = 0;
            c.Series[0]["PixelPointWidth"] = "3";
            c.ChartAreas[0].AxisY.LabelStyle.Format = "#";
            c.Series[0].IsVisibleInLegend = false;
        }

        private async void RedrawChart(Chart c, Image img)
        {
            if (_form.FilterPanelCb.Checked)
            {
                var width = (int)((_file.Width - _form.Horizontal.Value) * _scaler.ScaleFactor);
                width = width < img.Width ? width : img.Width;
                var height = (int)((_file.Height - _form.Vertical.Value) * _scaler.ScaleFactor);
                height = height < img.Height ? height : img.Height;

                if (width > 0 && height > 0)
                {
                    c.ChartAreas[0].AxisY.Maximum = width * height / 2;
                    c.Series[0].Points.DataBindXY(new List<int>(Enumerable.Range(0, 256)), "bits",
                        await _histogram.GetHistogramAsync(img, width, height), "values");
                }
            }
        }

        private async void RedrawChart(Chart c)
        {
            c.ChartAreas[0].AxisY.Maximum = _file.Width * _file.Height / 4;
            c.Series[0].Points.DataBindXY(new List<int>(Enumerable.Range(0, 256)), "bits",
              await _histogram.GetHistogramAsync(_tiles), "values");
        }

        private void ErrorGuiMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        public void ChangeFilterValue()
        {
            _filterFacade.ChangeFilterValue(_form.FilterTrackBar.Value);
            _form.FilterValueLabel.Text = string.Format("Уровень фильтра: {0}", _form.FilterTrackBar.Value);
            DrawImage();
        }

        public void GetFilter(string filterType, int filterDelta)
        {
            _filterFacade.GetFilter(filterType, filterDelta);
            _form.FilterTrackBar.Value = _filterFacade.Filter.FilterValue >> filterDelta;
        }

        public void ResetFilter()
        {
            _filterFacade.ResetFilters();
            _form.FilterTrackBar.Value = 0;
        }



        #region MouseHandlers

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
                    else if (_form.MarkPointRb.Checked)
                    {
                        if (!_settings.AreasOrPointsForAligning)
                        {

                            _pointSelector.Add((RlViewer.Files.LocatorFile)_file,
                                new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                    (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value), new Size(_settings.SelectorAreaSize, _settings.SelectorAreaSize));

                        }
                        else
                        {
                            _selectedPointArea = new Behaviors.AreaSelector.AreaSelectorWrapper(_file, _settings.MaxAlignerAreaSize);
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

                        ShowSection(_section,
                            new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                    else if (_form.HorizontalSectionRb.Checked)
                    {
                        _section = new Behaviors.Sections.Concrete.HorizontalSection(_settings.SectionSize, new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));

                        ShowSection(_section,
                                new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
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
            if (_file != null)
            {
                if (_drag.Trace(new Point((int)(e.X / _scaler.ScaleFactor), (int)(e.Y / _scaler.ScaleFactor))))
                {
                    var newHor = _form.Horizontal.Value - _drag.Delta.X;
                    var newVert = _form.Vertical.Value - _drag.Delta.Y;

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
                else if (_form.MarkPointRb.Checked && _selectedPointArea != null)
                {
                    if (_settings.AreasOrPointsForAligning)
                    {
                        if (_selectedPointArea.ResizeArea(new Point((int)(e.X / _scaler.ScaleFactor),
                                     (int)(e.Y / _scaler.ScaleFactor)), new Point(_form.Horizontal.Value, _form.Vertical.Value)))
                        {
                            _form.Canvas.Invalidate();
                        }
                    }
                }
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
                            (int)((_ruler.Pt1.Y - _form.Vertical.Value) * _scaler.ScaleFactor))
                            , new Point((int)((endPoint.X - _form.Horizontal.Value) * _scaler.ScaleFactor),
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

                        _form.Canvas.Image = _drawer.DrawRuler(
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
            catch (Exception)
            {
                ErrorGuiMessage("Невозможно проанализировать точку");
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
                    _areaSelector.StopResizing();
                    _analyzer.StopTracing();
                    _toolTip.Hide(_win);

                    if (_settings.AreasOrPointsForAligning)
                    {
                        if (_form.MarkPointRb.Checked)
                        {
                            _selectedPointArea.StopResizing();

                            if (_selectedPointArea.SelectedPoint == null)
                            {
                                _areaAligningWrapper.RemoveArea();
                            }
                        }
                    }
                    if (_form.LinearSectionRb.Checked)
                    {
                        ShowSection(_section, new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                }
                
                _form.Canvas.Invalidate();
            }

        }

        #endregion


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
                ErrorGuiMessage("Невозможно построить сечение");
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


        public void ShowFileInfo()
        {
            if (_file != null)
            {
                using (var iFrm = new Forms.InfoForm(_file.Header.HeaderInfo))
                {
                    iFrm.ShowDialog();
                }
            }
        }

        public void ShowSettings()
        {
            int pointsCount = 0;

            if (_areaAligningWrapper != null && _pointSelector != null)
            {
                pointsCount = _areaAligningWrapper.Select(x => x.SelectedPoint).Union(_pointSelector).Count();
            }

            using (var settgingsForm = new Forms.SettingsForm(_settings, pointsCount))
            {
                if (settgingsForm.ShowDialog() == DialogResult.OK)
                {
                    InitDrawImage();
                }
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
                        new Behaviors.Interpolators.LeastSquares.Concrete.LinearLeastSquares(compressedSelector), _saver);
                    StartTask("Выравнивание изображения", loaderWorker_AlignImage, loaderWorker_AlignImageCompleted,
                        new object[] { alignedSaveDlg.FileName });
                }
            }
        }

        public void ShowFindPoint()
        {
            if (_file != null && _searcher != null)
            {
                using (var ff = new Forms.FindPointForm(_file.Navigation != null))
                {
                    MessageBox.Show("Функция работает в тестовом режиме!");
                    if (ff.ShowDialog() == DialogResult.OK)
                    {
                        StartTask("Поиск точки", loaderWorker_FindPoint, loaderWorker_FindPointCompleted,
                            new Tuple<string, string>(ff.XLat, ff.YLon));
                    };
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
            if (_file != null)
            {
                if (_areaSelector.Area != null && _areaSelector.Area.Width != 0
                    && _areaSelector.Area.Width < 3000 && _areaSelector.Area.Height < 3000)
                {           
                    using (var statFrm = new Forms.StatisticsForm(_file, _areaSelector))
                    {
                        statFrm.ShowDialog();
                    }
                }
                else
                {
                    ErrorGuiMessage("Невозможно построить статистику области");
                }

            }
        }



        public void MakeReport()
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
                        ErrorGuiMessage("Unsuitable files selected");
                        return;
                    }

                    var reporter = Factories.Reporter.Abstract.ReporterFactory
                        .GetFactory(Behaviors.ReportGenerator.Abstract.ReporterTypes.Docx)
                        .Create(validFiles.ToArray());
                    using (var fsd = new SaveFileDialog())
                    {
                        fsd.Title = "Имя для файла отчета";
                        fsd.Filter = "Документ MS Word|.docx";
                        if (fsd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                reporter.GenerateReport(fsd.FileName);
                                Logging.Logger.Log(Logging.SeverityGrades.Info, 
                                    string.Format("Report file generated: {0}", fsd.FileName));
                            }
                            catch (Exception ex)
                            {
                                ErrorGuiMessage("Ошибка при создании отчета");
                                Logging.Logger.Log(Logging.SeverityGrades.Error, 
                                    string.Format("Unable to make report: {0}", ex.Message));
                            }
                        }
                    }
                }
            }
        }

    }
}
