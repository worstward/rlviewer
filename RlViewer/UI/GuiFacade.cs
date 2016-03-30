using System;
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

namespace RlViewer.UI
{
    public class GuiFacade
    {
        public GuiFacade(ISuitableForm form)
        {
            _form = form;
            _settings = new Settings.Settings();
            _filterFacade = new Behaviors.Filters.ImageFilterFacade();
            _scaler = new Behaviors.Scaling.Scaler();
            _analyzer = new Behaviors.Analyzing.PointAnalyzer();
            _drag = new Behaviors.DragController(_scaler);
            _keyboardFacade = new KeyboardFacade(() => Undo(), () => OpenFile());
            InitializeWindow();
            _form.Canvas.MouseWheel += Canvas_MouseWheel;
            _win = _form.Canvas;
        }


        private System.ComponentModel.BackgroundWorker _worker;
        private Settings.Settings _settings;
        private TileCreator _creator;
        private Behaviors.Filters.ImageFilterFacade _filterFacade;
        private KeyboardFacade _keyboardFacade;
        private Behaviors.Saving.Abstract.Saver _saver;
        private RlViewer.Behaviors.Scaling.Scaler _scaler;
        private RlViewer.Behaviors.Analyzing.PointAnalyzer _analyzer;

        private Files.FileProperties _properties;
        private Headers.Abstract.LocatorFileHeader _header;
        private Navigation.NavigationContainer _navi;

        private Files.LocatorFile _file;
        private RlViewer.Behaviors.TileCreator.Tile[] _tiles;
        private RlViewer.Behaviors.Draw.DrawerFacade _drawer;
        private RlViewer.Behaviors.PointSelector.PointSelector _pointSelector;
        private RlViewer.Behaviors.AreaSelector.AreaSelector _areaSelector;
        private RlViewer.Behaviors.DragController _drag;

        private ISuitableForm _form;
        private string _caption = string.Empty;
        private ToolTip _toolTip = new ToolTip();
        private IWin32Window _win;


        public void OpenWithDoubleClick()
        {
            foreach (var path in System.Environment.GetCommandLineArgs())
            {
                if (!path.EndsWith(".exe"))
                {
                    OpenFile(path);
                }
            }
        }

        public string OpenFile()
        {
            string caption;
            using (var openFileDlg = new OpenFileDialog() { Filter = Resources.OpenFilter })
            {
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



        public string OpenFile(string fileName)
        {
            
            string caption;

            if (_worker != null && _worker.IsBusy)
            {
                CancelLoading();
            }

            _file = null;
            _drawer = null;
            _form.Canvas.Image = null;
            _form.NavigationDgv.Rows.Clear();

            try
            {
                _properties = new Files.FileProperties(fileName);
                _header = Factories.Header.Abstract.HeaderFactory.GetFactory(_properties).Create(_properties.FilePath);       
            }
            catch (ArgumentException)
            {
                ErrorGuiMessage("Неподдерживаемый формат файла");
                Logging.Logger.Log(Logging.SeverityGrades.Error, "Неподдерживаемый формат файла");
                return string.Empty;
            }

            caption = _caption = _properties.FilePath;

            _form.StatusLabel.Text = "Чтение навигации";
            InitProgressBar();
            _worker = InitWorker(loaderWorker_InitFile, loaderWorker_InitFileCompleted);
            _worker.RunWorkerAsync();
         
            return caption;
        }

        public void ShowFileInfo()
        {
            if(_file != null)
            {
                using (var iFrm = new Forms.InfoForm(_file.Header.HeaderInfo))
                {                   
                    iFrm.ShowDialog();
                }
            }
        }

        private void InitProgressBar()
        {
            _form.ProgressBar.Value = 0;
            _form.ProgressBar.Visible = true;
            _form.ProgressLabel.Visible = true;
            _form.ProgressLabel.Text = "0 %";
            _form.StatusLabel.Visible = true;
            _form.CancelButton.Visible = true;
            _form.Horizontal.Visible = false;
            _form.Vertical.Visible = false;
        }

        public void GetImage()
        {
            if (_file != null)
            {
                _form.StatusLabel.Text = "Генерация тайлов";
                InitProgressBar();
                _worker = InitWorker(loaderWorker_CreateTiles, loaderWorker_CreateTilesCompleted);
                _worker.RunWorkerAsync();
            }
        }

        public void CancelLoading()
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
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
                disposer(); 
            }

            _worker.Dispose();          
        }




        private void ClearCancelledFileTiles()
        {
            try
            {
                var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(_file.Properties.FilePath),
                    Path.GetExtension(_file.Properties.FilePath));
                File.SetAttributes(path, FileAttributes.Normal);
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                ErrorGuiMessage(ex.Message);
            }

            InitializeWindow();   
        }

        private void loaderWorker_SaveFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] args;
            string path;
            Point leftTop;
            int width;
            int height;

            try
            {
                args    = (object[])e.Argument;
                path    = (string)args[0];
                leftTop = (Point)args[1];
                width   = (int)args[2];
                height  = (int)args[3];
            }
            catch (InvalidCastException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Internal, "Invalid saver params");
                throw;
            }
           

            try
            {
                _saver = SaverFactory.GetFactory(_properties).Create(_file);   

                _saver.Report += ProgressReporter;
                _saver.CancelJob += (s, cEvent) => cEvent.Cancel = _worker.CancellationPending;

                _saver.Save(path, Path.GetExtension(path).Replace(".", "")
                    .ToEnum<RlViewer.FileType>(), leftTop, new Size(width, height), _creator.NormalizationFactor);
            }
            catch (ArgumentException aex)
            {
                ErrorGuiMessage(string.Format("Unable to save file: {0}", path));
                Logging.Logger.Log(Logging.SeverityGrades.Error, aex.Message);
            }  
        }

        private void loaderWorker_SaveFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _saver.Report -= ProgressReporter;
            _saver.CancelJob -= (s, cEvent) => cEvent.Cancel = _worker.CancellationPending;
            _form.ProgressBar.Visible = false;
            _form.ProgressLabel.Visible = false;
            _form.StatusLabel.Visible = false;
            _form.CancelButton.Visible = false;

        }





        private void loaderWorker_InitFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                _navi = Factories.NavigationContainer.Abstract.NavigationContainerFactory.GetFactory(_properties).Create(_properties, _header);

                _navi.Report += ProgressReporter;
                _navi.CancelJob += (s, cEvent) => cEvent.Cancel = _worker.CancellationPending;
                _navi.GetNavigation();

                _file = FileFactory.GetFactory(_properties).Create(_properties, _header, _navi);
            }
            catch (ArgumentNullException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Navigation reading cancelled"));
                InitializeWindow();
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error opening file {0}", ex.Message));
                ErrorGuiMessage("Невозможно открыть файл");
                InitializeWindow();
            }
        }


        private void loaderWorker_InitFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _navi.Report -= ProgressReporter;
            _navi.CancelJob -= (s, cEvent) => cEvent.Cancel = _worker.CancellationPending;

            if (e.Cancelled || e.Error != null)
            {
                InitializeWindow();
            }
            else
            {
                GetImage();
            }             
        }


        private void loaderWorker_CreateTiles(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _creator = TileCreatorFactory.GetFactory(_file.Properties).Create(_file as RlViewer.Files.LocatorFile);

            _creator.Report += ProgressReporter;
            _creator.CancelJob += (s, cEvent) => cEvent.Cancel = _worker.CancellationPending;
            _tiles = _creator.GetTiles(_file.Properties.FilePath, _settings.ForceTileGeneration, _settings.AllowViewWhileLoading);          
        }


        private void loaderWorker_CreateTilesCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _creator.Report -= ProgressReporter;
            _creator.CancelJob -= (s, cEvent) => cEvent.Cancel = _worker.CancellationPending;

            if (_tiles != null)
            {
                _pointSelector = new Behaviors.PointSelector.PointSelector();
                _areaSelector = new Behaviors.AreaSelector.AreaSelector();
                _form.ProgressBar.Visible = false;
                _form.ProgressLabel.Visible = false;
                _form.StatusLabel.Visible = false;
                _form.CancelButton.Visible = false;
                InitDrawImage();
            }
            else           
            {
                ClearWorkerData(() => ClearCancelledFileTiles());
            }

        }

        private void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_file != null)
            {
                if (e.Delta > 0)
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
            _form.ProgressBar.Invoke(new Action(() =>
            {
                _form.ProgressBar.Value = progress;
                _form.ProgressLabel.Text = string.Format("{0} %", progress.ToString());
            }));
        }

        private void Undo()
        {
            if (_file != null)
            {
                if (_form.MarkPointRb.Checked)
                {
                    _pointSelector.RemoveLast();
                }
                else if (_form.MarkAreaRb.Checked)
                {
                    _areaSelector.ResetArea();
                }
                DrawItems();
            }
        }

        public void InitDrawImage()
        {
            if (_form.Canvas.Size.Width != 0 && _form.Canvas.Size.Height != 0 && _tiles != null)
            {
                RlViewer.Behaviors.Draw.TileDrawer tDrawer = new Behaviors.Draw.TileDrawer(_filterFacade.Filter, _scaler);
                RlViewer.Behaviors.Draw.ItemDrawer iDrawer = new Behaviors.Draw.ItemDrawer(_pointSelector, _areaSelector, _scaler);
                _drawer = new RlViewer.Behaviors.Draw.DrawerFacade(_form.Canvas.Size, iDrawer, tDrawer);

                ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
                InitScrollBars(_scaler.ScaleFactor);
                DrawImage();
            }
        }

        public void DrawImage()
        {
            if (_file != null && _drawer != null && _tiles != null)
            {
                Task.Run(() => _form.Canvas.Image = _drawer.Draw(_tiles,
                        new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value))).Wait();
            }
        }



        private void DrawItems()
        {
            if (_file != null && _drawer != null)
            {
                Task.Run(()=> _form.Canvas.Image = _drawer.Draw(
                    new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value))).Wait();
            }
        }

        public void ChangePalette(int[] rgb, bool isReversed)
        {
            if (_drawer != null)
            {
                _drawer.GetPalette(rgb[0], rgb[1], rgb[2], isReversed);
                if (_file != null && _tiles != null)
                {
                    _form.Canvas.Image = _drawer.Draw(_tiles,
                        new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
                }
            }
        }
     

        public void ToggleNavigation()
        {
            if (_form.NavigationCb.Checked)
            {
                _form.WorkingAreaSplitter.Panel2Collapsed = false;
                _form.WorkingAreaSplitter.Panel2.Show();
            }
            else
            {
                _form.WorkingAreaSplitter.Panel2Collapsed = true;
                _form.WorkingAreaSplitter.Panel2.Hide();
            }
            InitDrawImage();
        }


        public void Save()
        {
            if (_file != null)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.FileName = Path.GetFileNameWithoutExtension(_file.Properties.FilePath).ToString();
                    sfd.Filter = _file.GetType() == typeof(RlViewer.Files.Rli.Concrete.Raw) ? Resources.RawSaveFilter : Resources.SaveFilter;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var sSize = new Forms.SaveSizeForm(_file.Width, _file.Height, _areaSelector))
                        {
                            if (sSize.ShowDialog() == DialogResult.OK)
                            {
                                _form.StatusLabel.Text = "Сохранение файла";
                                InitProgressBar();
                                _worker = InitWorker(loaderWorker_SaveFile, loaderWorker_SaveFileCompleted);
                                _worker.RunWorkerAsync(new object[] { sfd.FileName, sSize.LeftTop, sSize.ImageWidth, sSize.ImageHeight });
                            }
                        }
                    }
                }
            }

        }


        public void ChangeScaleFactor(float value)
        {
            if (value > Math.Log(_scaler.MaxZoom, 2) || value < Math.Log(_scaler.MinZoom, 2)) return;
            
            
            
            float scaleFactor = (float)Math.Pow(2, value);

            _form.ScaleLabel.Text = string.Format("Масштаб: {0}%", (scaleFactor * 100).ToString());
            
            #region centerPointOfView
            InitScrollBars(scaleFactor);

            float a = Math.Min(_form.Canvas.Width, _file.Width) / scaleFactor / 2;
            a = _scaler.ScaleFactor > scaleFactor ? -a / 2 : a;

            float b = Math.Min(_form.Canvas.Height, _file.Height) / scaleFactor / 2;
            b = _scaler.ScaleFactor > scaleFactor ? -b / 2 : b;

            float newHorizontalValue = _form.Horizontal.Value + a < 0 ? 0 : _form.Horizontal.Value + a;
            newHorizontalValue = newHorizontalValue + _form.Canvas.Width /
                (_scaler.ScaleFactor > scaleFactor ? _scaler.ScaleFactor : scaleFactor) > _form.Horizontal.Maximum ?
                _form.Horizontal.Maximum : newHorizontalValue;

            float newVerticalValue = _form.Vertical.Value + b < 0 ? 0 : _form.Vertical.Value + b;
            newVerticalValue = newVerticalValue + _form.Canvas.Height /
                (_scaler.ScaleFactor > scaleFactor ? _scaler.ScaleFactor : scaleFactor) > _form.Vertical.Maximum ?
                _form.Vertical.Maximum : newVerticalValue;


            _form.Horizontal.Value = (int)newHorizontalValue;
            _form.Vertical.Value = (int)newVerticalValue;
            #endregion

            _scaler = new Behaviors.Scaling.Scaler(scaleFactor);
            _drag = new Behaviors.DragController(_scaler);
            InitDrawImage();
        }



        private void InitializeWindow()
        {

            _file = null;
            _drawer = null;
            _pointSelector = null;
            _areaSelector = null;

            _form.Canvas.Image = null;
            _form.Horizontal.Visible = false;
            _form.Vertical.Visible = false;
            _form.Vertical.SmallChange = 1;
            _form.Vertical.LargeChange = 1;
            _form.Horizontal.SmallChange = 1;
            _form.Horizontal.LargeChange = 1;

            _form.FilterTrackBar.SmallChange = 1;
            _form.FilterTrackBar.LargeChange = 1;
            _form.FilterTrackBar.Minimum = -16;
            _form.FilterTrackBar.Maximum = 16;
            _form.FilterTrackBar.Value = 0;

            _form.ProgressBar.Minimum = 0;
            _form.ProgressBar.Maximum = 100;
            _form.ProgressBar.Visible = false;
            _form.ProgressLabel.Visible = false;
            _form.StatusLabel.Visible = false;
            _form.CancelButton.Visible = false;
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
            DrawImage();
        }

        public void GetFilter(string filterType, int filterDelta)
        {
            _filterFacade.GetFilter(filterType, filterDelta);
            _form.FilterTrackBar.Value = _filterFacade.Filter.FilterValue >> filterDelta;
        }



        private void ShowSection(Behaviors.Sections.Section section, Point p)
        {
            
            var points = section.GetValues(_file, p);
            var mark = section.InitialPointMark;
            var caption = section.GetType() == typeof(Behaviors.Sections.HorizontalSection)
                ? "Горизонтальное сечение" : "Вертикальное сечение";


            using (var sectionForm = new Forms.SectionGraphForm(points, mark, caption))
            { 
                sectionForm.ShowDialog();
            }
        }




        #region MouseHandlers

        public string ShowMousePosition(MouseEventArgs e)
        {
            //return string.Format("X:{0} Y:{1}",
            //   e.X + _form.Horizontal.Value, e.Y + _form.Vertical.Value);
            if (_file != null && !_worker.IsBusy)
            {
                if (e.Y / _scaler.ScaleFactor + _form.Vertical.Value > 0 &&
                    e.Y / _scaler.ScaleFactor + _form.Vertical.Value < _file.Height
                    && e.X > 0 && e.X / _scaler.ScaleFactor < _file.Width)
                {
                    return string.Format("X:{0} Y:{1}",
                        (int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value, (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);
                }
            }
            return string.Empty;
        }



        public void ShowNavigation(MouseEventArgs e)
        {
            if (_file != null && !_worker.IsBusy)
            {

                if (_form.NavigationCb.Checked && _file.Navigation != null)
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
                    if (!_form.MarkPointRb.Checked && !_form.MarkAreaRb.Checked && !_form.AnalyzePointRb.Checked &&
                        !_form.VerticalSectionRb.Checked && !_form.HorizontalSectionRb.Checked)
                    {
                        _form.Canvas.Cursor = Cursors.SizeAll;
                        _drag.StartTracing(e.Location, !_form.MarkPointRb.Checked);
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
                        _pointSelector.Add((RlViewer.Files.LocatorFile)_file,
                            new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                    else if (_form.AnalyzePointRb.Checked)
                    {
                        _analyzer.StartTracing();
                    }
                    else if (_form.VerticalSectionRb.Checked)
                    {
                        ShowSection(new Behaviors.Sections.VerticalSection(_settings.SectionSize), new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                    else if (_form.HorizontalSectionRb.Checked)
                    {
                        ShowSection(new Behaviors.Sections.HorizontalSection(_settings.SectionSize), new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value));
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    _form.Canvas.Cursor = Cursors.SizeAll;
                    _drag.StartTracing(e.Location, true);
                }
            }
        }

        public void TraceMouseMovement(MouseEventArgs e)
        {
            if (_file != null)
            {
                if (_drag.Trace(e.Location))
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
                        DrawItems();
                    }
                }
                else if (_form.AnalyzePointRb.Checked && _analyzer != null)
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
                else if (_form.VerticalSectionRb.Checked && _drawer != null)
                {
                    _form.Canvas.Image = _drawer.DrawVerticalSection(e.Location, (int)(_settings.SectionSize * _scaler.ScaleFactor));
                }
                else if (_form.HorizontalSectionRb.Checked && _drawer != null)
                {
                    _form.Canvas.Image = _drawer.DrawHorizontalSection(e.Location, (int)(_settings.SectionSize * _scaler.ScaleFactor));
                }
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
                }               
            }
        }

#endregion

        public void ShowSettings()
        {
            using (var settgingsForm = new Forms.SettingsForm(_settings))
            {
                if (settgingsForm.ShowDialog() == DialogResult.OK)
                {
                    ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
                }

            }
        }

        public void ShowLog()
        {
            using (var logForm = new Forms.LogForm())
            {
                logForm.ShowDialog();
            }
        }


        private System.ComponentModel.BackgroundWorker InitWorker(System.ComponentModel.DoWorkEventHandler doWork,
            System.ComponentModel.RunWorkerCompletedEventHandler completed)
        {
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += doWork;
            //worker.ProgressChanged += _worker_ProgressChanged;
            worker.RunWorkerCompleted += completed;

            return worker;
        }

        public void ProceedKeyPress(System.Windows.Forms.KeyEventArgs e)
        {
            if(_keyboardFacade != null)
            { 
                _keyboardFacade.ProceedKeyPress(e);
            }
        }

    }
}
