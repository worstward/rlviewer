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
            _drag = new Behaviors.DragController();
            _keyboardFacade = new KeyboardFacade(() => Undo(), () => OpenFile(),
                () => Save(), () => ShowFileInfo(), () => ShowLog());
            InitializeWindow();
            _form.Canvas.MouseWheel += Canvas_MouseWheel;
            _form.RulerRb.CheckedChanged += RulerRb_CheckedChanged;
            _win = _form.Canvas;
            AddToolTips(form);
        }


        private Settings.Settings _settings;
        private TileCreator _creator;
        private Behaviors.Filters.ImageFilterFacade _filterFacade;
        private KeyboardFacade _keyboardFacade;
        private Behaviors.Saving.Abstract.Saver _saver;
        private RlViewer.Behaviors.Scaling.Scaler _scaler;
        private RlViewer.Behaviors.Analyzing.PointAnalyzer _analyzer;
        private RlViewer.Behaviors.Ruler.RulerFacade _ruler;
        private RlViewer.Behaviors.ImageAligning.Aligning _aligner;


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
        private System.ComponentModel.BackgroundWorker _worker;
        private string _caption = string.Empty;
        private ToolTip _toolTip = new ToolTip();
        private IWin32Window _win;


        #region OpenFile
        public string OpenWithDoubleClick()
        {
            var fName = Environment.GetCommandLineArgs().Where(x => Path.GetExtension(x) != ".exe").FirstOrDefault();
            return  fName == null ? string.Empty : OpenFile(fName);
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

            _file = null;
            _tiles = null;
            _drawer = null;

            _form.NavigationDgv.Rows.Clear();
            InitializeWindow();
       
            caption = _caption = fileName;

            InitProgressControls(true, "Чтение навигации");
            _worker = ThreadHelper.InitWorker(loaderWorker_InitFile, loaderWorker_InitFileCompleted);
            _worker.RunWorkerAsync(fileName);
         
            return caption;
        }
        #endregion

    
        public void GetImage()
        {
            if (_file != null)
            {
                InitProgressControls(true, "Генерация тайлов");
                _worker = ThreadHelper.InitWorker(loaderWorker_CreateTiles, loaderWorker_CreateTilesCompleted);
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
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "tiles", Path.GetFileNameWithoutExtension(_file.Properties.FilePath),
                    Path.GetExtension(_file.Properties.FilePath),
                    File.GetCreationTime(_file.Properties.FilePath).GetHashCode().ToString());

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

            _saver.Report += (s, pe) => ProgressReporter(pe.Percent);
            _saver.CancelJob += (s, ce) => ce.Cancel = _worker.CancellationPending;

            _saver.Save(path, Path.GetExtension(path).Replace(".", "")
                .ToEnum<RlViewer.FileType>(), new Rectangle(leftTop.X, leftTop.Y, width, height), _creator.NormalizationFactor);

           
            if (_saver.Cancelled)
            {
                ClearWorkerData(() => ClearCancelledSaving(path));
            }

            e.Cancel = _saver.Cancelled;
            e.Result = path;
        }

        private void loaderWorker_SaveFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _saver.Report -= (s, pe) => ProgressReporter(pe.Percent); ;
            _saver.CancelJob -= (s, ce) => ce.Cancel = _worker.CancellationPending;

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
            _aligner.Report += (s, pe) => ProgressReporter(pe.Percent);
            _aligner.CancelJob += (s, ce) => ce.Cancel = _worker.CancellationPending;
            string fileName = Path.GetFileName(_file.Properties.FilePath);
            _aligner.Resample(fileName);

            e.Cancel = _aligner.Cancelled;
            e.Result = Path.GetFileNameWithoutExtension(fileName) + "_aligned" + Path.GetExtension(fileName);           
        }

        private void loaderWorker_AlignImageCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _aligner.Report -= (s, pe) => ProgressReporter(pe.Percent);
            _aligner.CancelJob -= (s, ce) => ce.Cancel = _worker.CancellationPending;

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
            _navi.CancelJob += (s, ce) => ce.Cancel = _worker.CancellationPending;
            _navi.GetNavigation();
                e.Cancel = _navi.Cancelled;

            _file = FileFactory.GetFactory(_properties).Create(_properties, _header, _navi);
            _saver = SaverFactory.GetFactory(_properties).Create(_file);
            _ruler = new Behaviors.Ruler.RulerFacade(_file);                
        }


        private void loaderWorker_InitFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_navi != null)
            {
                _navi.Report -= (s, pe) => ProgressReporter(pe.Percent);
                _navi.CancelJob -= (s, ce) => ce.Cancel = _worker.CancellationPending;
            }


            if (e.Cancelled)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Navigation reading cancelled"));
                InitializeWindow();
            }
            else if (e.Error != null)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error opening file: {0}", e.Error.Message));
                ErrorGuiMessage("Unable to open file");
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

            _creator.Report += (s, pe) => ProgressReporter(pe.Percent);
            _creator.CancelJob += (s, ce) => ce.Cancel = _worker.CancellationPending;
            _tiles = _creator.GetTiles(_file.Properties.FilePath, _settings.ForceTileGeneration, _settings.AllowViewWhileLoading);

            e.Cancel = _creator.Cancelled;
            
        }


        private void loaderWorker_CreateTilesCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_creator != null)
            {
                _creator.Report -= (s, pe) => ProgressReporter(pe.Percent);
                _creator.CancelJob -= (s, ce) => ce.Cancel = _worker.CancellationPending;
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
                
                _pointSelector = new Behaviors.PointSelector.PointSelector();
                _areaSelector = new Behaviors.AreaSelector.AreaSelector();
                InitProgressControls(false);
                InitDrawImage();              
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
            ThreadHelper.ThreadSafeUpdate<ToolStripProgressBar>(_form.ProgressBar, pb => { pb.Value = progress; });
            ThreadHelper.ThreadSafeUpdate<ToolStripStatusLabel>(_form.ProgressLabel, pl =>
                        { pl.Text = string.Format("{0} %", progress.ToString()); });
        }

        private void Undo()
        {
            if (_file != null)
            {
                if (_form.MarkPointRb.Checked)
                {
                    _pointSelector.RemoveLast();

                    if (_pointSelector.Count() == 3 || _pointSelector.Count() == 4 || _pointSelector.Count() == 16)
                    {
                        _form.AlignBtn.Enabled = true;
                    }
                    else
                    {
                        _form.AlignBtn.Enabled = false;
                    }
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
                var tDrawer = new Behaviors.Draw.TileDrawer(_filterFacade.Filter, _scaler);
                var iDrawer = new Behaviors.Draw.ItemDrawer(_pointSelector, _areaSelector, _scaler);
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
                    sfd.Filter = _file.GetType() == typeof(RlViewer.Files.Rli.Concrete.Raw) || _file.GetType() == typeof(RlViewer.Files.Rli.Concrete.R) 
                        ? Resources.RawSaveFilter : Resources.SaveFilter;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var sSize = new Forms.SaveSizeForm(_file.Width, _file.Height, _areaSelector))
                        {
                            if (sSize.ShowDialog() == DialogResult.OK)
                            {
                                InitProgressControls(true, "Сохранение");
                                _worker = ThreadHelper.InitWorker(loaderWorker_SaveFile, loaderWorker_SaveFileCompleted);
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


        private void InitializeWindow()
        {
            _file = null;
            _drawer = null;
            _pointSelector = null;
            _areaSelector = null;

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

            InitProgressControls(false);
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
                        _pointSelector.Add((RlViewer.Files.LocatorFile)_file,
                            new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value), new Size(_settings.SelectorAreaSize, _settings.SelectorAreaSize));

                        if (_pointSelector.Count() == 3 || _pointSelector.Count() == 4 || _pointSelector.Count() == 16)
                        {
                            _form.AlignBtn.Enabled = true;
                        }
                        else
                        {
                            _form.AlignBtn.Enabled = false;
                        }
                    }
                    else if (_form.AnalyzePointRb.Checked)
                    {
                        _analyzer.StartTracing();
                        AnalyzePoint(e);
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
                    else if (_form.RulerRb.Checked)
                    {
                        if (!_ruler.Pt1Fixed)
                        {
                            _ruler.Pt1 = new Point((int)(e.X / _scaler.ScaleFactor) + _form.Horizontal.Value,
                                    (int)(e.Y / _scaler.ScaleFactor) + _form.Vertical.Value);
                        }
                        else if(!_ruler.Pt2Fixed)
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
                        DrawItems();
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
                else if(_form.RulerRb.Checked && _drawer != null)
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
                }               
            }
        }

#endregion


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
            using (var settgingsForm = new Forms.SettingsForm(_settings))
            {
                if (settgingsForm.ShowDialog() == DialogResult.OK)
                {
                    ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
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

        public void ShowAbout()
        {
            using (var about = new Forms.About())
            {
                about.ShowDialog();
            }
        }
        

        public void ProceedKeyPress(System.Windows.Forms.KeyEventArgs e)
        {
            if(_keyboardFacade != null)
            { 
                _keyboardFacade.ProceedKeyPress(e);
            }
        }

        public void AlignImage()
        {
            _form.StatusLabel.Text = "Чтение навигации";

            _aligner = new Behaviors.ImageAligning.Aligning(_file, _pointSelector, _saver);

            InitProgressControls(true, "Выравнивание изображения");
            _worker = ThreadHelper.InitWorker(loaderWorker_AlignImage, loaderWorker_AlignImageCompleted);

            _worker.RunWorkerAsync();
        }

        private void AddToolTips(ISuitableForm frm)
        {
            AddToolTip(frm.AlignBtn, "Выровнять");
            AddToolTip(frm.AnalyzePointRb, "Анализ амплитуды");
            AddToolTip(frm.DragRb, "Перемещение по изображению");
            AddToolTip(frm.HorizontalSectionRb, "Горизонтальное сечение");
            AddToolTip(frm.MarkAreaRb, "Область");
            AddToolTip(frm.MarkPointRb, "Точка");
            AddToolTip(frm.NavigationCb, "Навигация");
            AddToolTip(frm.RulerRb, "Линейка");
            AddToolTip(frm.VerticalSectionRb, "Вертикальное сечение");

            AddToolTip(frm.BrightnessRb, "Яркость");
            AddToolTip(frm.ContrastRb, "Контрастность");
            AddToolTip(frm.GammaRb, "Гамма");
            AddToolTip(frm.ResetFilter, "Сброс фильтров");
        }

        private void AddToolTip(Control c, string caption)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(c, caption);
        }

        private void RulerRb_CheckedChanged(object sender, EventArgs e)
        {
            if (_ruler != null)
            {
                _ruler.ResetRuler();
                _form.DistanceLabel.Text = string.Empty;
            }
        }
    }
}
