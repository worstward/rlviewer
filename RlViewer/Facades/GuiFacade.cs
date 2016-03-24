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

namespace RlViewer.Facades
{
    public class GuiFacade
    {
        public GuiFacade(ISuitableForm form)
        {
            _form = form;
            _settings = new Settings.Settings();
            _filterFacade = new ImageFilterFacade();
            _scaler = new Behaviors.Scaling.Scaler(1);
            _drag = new Behaviors.DragController(_scaler);
            _keyboardFacade = new KeyboardFacade(() => Undo(), () => OpenFile());
            InitControls();

            _form.Canvas.MouseWheel += Canvas_MouseWheel;
        }



        private System.ComponentModel.BackgroundWorker _loaderWorker;
        private Settings.Settings _settings;
        private TileCreator _creator;
        private ImageFilterFacade _filterFacade;
        private KeyboardFacade _keyboardFacade;
        private Behaviors.Saving.Abstract.Saver _saver;
        private RlViewer.Behaviors.Scaling.Scaler _scaler;


        private Files.FileProperties _properties;
        private Headers.Abstract.LocatorFileHeader _header;
        private Navigation.NavigationContainer _navi;

        private Files.LocatorFile _file;
        private RlViewer.Behaviors.TileCreator.Tile[] _tiles;
        private RlViewer.Facades.DrawerFacade _drawer;
        private RlViewer.Behaviors.PointSelector.PointSelector _pointSelector;
        private RlViewer.Behaviors.AreaSelector.AreaSelector _areaSelector;
        private RlViewer.Behaviors.DragController _drag;

        private ISuitableForm _form;
        private string _caption = string.Empty;


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

            if (_loaderWorker != null && _loaderWorker.IsBusy)
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
            _loaderWorker = InitWorker(loaderWorker_InitFile, loaderWorker_InitFileCompleted);
            _loaderWorker.RunWorkerAsync();
         
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
                _loaderWorker = InitWorker(loaderWorker_CreateTiles, loaderWorker_CreateTilesCompleted);
                _loaderWorker.RunWorkerAsync();
            }
        }

        public void CancelLoading()
        {
            if (_loaderWorker != null && !_loaderWorker.CancellationPending && _loaderWorker.IsBusy)
            {
                _loaderWorker.CancelAsync();
                _loaderWorker.Dispose();
                InitControls();

                if (_file != null)
                {
                    _file = null;
                    _drawer = null;
                    _pointSelector = null;
                    _areaSelector = null;

                    ClearCancelledFileTiles();
                }
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
           

            try
            {
                _saver = SaverFactory.GetFactory(_properties).Create(_file);   

                _saver.Report += ProgressReporter;
                _saver.CancelJob += (s, cEvent) => cEvent.Cancel = _loaderWorker.CancellationPending;

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
            _saver.CancelJob -= (s, cEvent) => cEvent.Cancel = _loaderWorker.CancellationPending;
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
                _navi.CancelJob += (s, cEvent) => cEvent.Cancel = _loaderWorker.CancellationPending;
                _navi.GetNavigation();

                _file = FileFactory.GetFactory(_properties).Create(_properties, _header, _navi);
            }
            catch (ArgumentNullException)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Navigation reading cancelled"));
                InitControls();
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error opening file {0}", ex.Message));
                ErrorGuiMessage("Невозможно открыть файл");
                InitControls();
            }
        }


        private void loaderWorker_InitFileCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _navi.Report -= ProgressReporter;
            _navi.CancelJob -= (s, cEvent) => cEvent.Cancel = _loaderWorker.CancellationPending;
            GetImage();
        }


        private void loaderWorker_CreateTiles(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _creator = TileCreatorFactory.GetFactory(_file.Properties).Create(_file as RlViewer.Files.LocatorFile);

            _creator.Report += ProgressReporter;
            _creator.CancelJob += (s, cEvent) => cEvent.Cancel = _loaderWorker.CancellationPending;
            _tiles = _creator.GetTiles(_file.Properties.FilePath, _settings.ForceTileGeneration, _settings.AllowViewWhileLoading);          
        }


        private void loaderWorker_CreateTilesCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _creator.Report -= ProgressReporter;
            _creator.CancelJob -= (s, cEvent) => cEvent.Cancel = _loaderWorker.CancellationPending;

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
        }

        private void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ChangeScaleFactor((int)Math.Log(_scaler.ZoomFactor, 2) + 1);
            }
            else
            {
                ChangeScaleFactor((int)Math.Log(_scaler.ZoomFactor, 2) - 1);
            }
        }


        private void ProgressReporter(int progress)
        {
            _form.ProgressBar.Invoke((Action)delegate
            {
                _form.ProgressBar.Value = progress;
                _form.ProgressLabel.Text = string.Format("{0} %", progress.ToString());
            });
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
                _drawer = new RlViewer.Facades.DrawerFacade(_form.Canvas.Size, iDrawer, tDrawer);

                ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
                InitScrollBars(_scaler.ZoomFactor);
                DrawImage();
            }
        }

        public void DrawImage()
        {
            if (_file != null && _drawer != null && _tiles != null)
            {
                Task.Run(() => _form.Canvas.Image = _drawer.Draw(_tiles,
                        new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value))).Wait();
                //_form.Canvas.Image = _drawer.Draw(_tiles,
                //        new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
            }
        }



        private void DrawItems()
        {
            if (_file != null && _drawer != null)
            {
                Task.Run(()=> _form.Canvas.Image = _drawer.Draw(
                    new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value))).Wait();
                //_form.Canvas.Image = _drawer.Draw(
                //    new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
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
                                _loaderWorker = InitWorker(loaderWorker_SaveFile, loaderWorker_SaveFileCompleted);
                                _loaderWorker.RunWorkerAsync(new object[] { sfd.FileName, sSize.LeftTop, sSize.ImageWidth, sSize.ImageHeight });
                            }
                        }
                    }
                }
            }

        }


        public void ChangeScaleFactor(float value)
        {
            if (value > 7 || value < 0) return;
            float scaleFactor = (float)Math.Pow(2, value);
            InitScrollBars(scaleFactor);

            float a = _form.Canvas.Width / scaleFactor / 2;
            a = _scaler.ZoomFactor > scaleFactor ? -a / 2 : a;

            float b = _form.Canvas.Height / scaleFactor / 2;
            b = _scaler.ZoomFactor > scaleFactor ? -b / 2 : b;

            float newHorizontalValue = _form.Horizontal.Value + a < 0 ? 0 : _form.Horizontal.Value + a;
            newHorizontalValue = newHorizontalValue + _form.Canvas.Width > _form.Horizontal.Maximum ?
                _form.Horizontal.Maximum  : newHorizontalValue;

            float newVerticalValue = _form.Vertical.Value + b < 0 ? 0 : _form.Vertical.Value + b;
            newVerticalValue = newVerticalValue + _form.Canvas.Height > _form.Vertical.Maximum ?
                _form.Vertical.Maximum : newVerticalValue;


            _form.Horizontal.Value = (int)newHorizontalValue;
            _form.Vertical.Value = (int)newVerticalValue;

            _scaler = new Behaviors.Scaling.Scaler((float)Math.Pow(2, value));
            _drag = new Behaviors.DragController(_scaler);
            InitDrawImage();
        }



        private void InitControls()
        {
            _form.Canvas.Image = null;
            _form.Horizontal.Visible = false;
            _form.Vertical.Visible = false;

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

            var horMax = (int)((f.Width  - _form.Canvas.Size.Width  / scaleFactor));
            var verMax = (int)((f.Height - _form.Canvas.Size.Height / scaleFactor));
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

        #region MouseHandlers

        public string ShowMousePosition(MouseEventArgs e)
        {
            //return string.Format("X:{0} Y:{1}",
            //   e.X + _form.Horizontal.Value, e.Y + _form.Vertical.Value);
            if (_file != null && !_loaderWorker.IsBusy)
            {
                if (e.Y / _scaler.ZoomFactor + _form.Vertical.Value > 0 &&
                    e.Y / _scaler.ZoomFactor + _form.Vertical.Value < _file.Height
                    && e.X > 0 && e.X / _scaler.ZoomFactor < _file.Width)
                {
                    return string.Format("X:{0} Y:{1}",
                        (int)(e.X / _scaler.ZoomFactor) + _form.Horizontal.Value, (int)(e.Y / _scaler.ZoomFactor) + _form.Vertical.Value);
                }
            }
            return string.Empty;
        }



        public void ShowNavigation(MouseEventArgs e)
        {
            if (_file != null && !_loaderWorker.IsBusy)
            {
                if (e.Y / _scaler.ZoomFactor + _form.Vertical.Value > 0 &&
                    e.Y / _scaler.ZoomFactor + _form.Vertical.Value < _file.Height && e.X > 0 && e.X / _scaler.ZoomFactor < _file.Width)
                {

                    if (_form.NavigationCb.Checked && _file.Navigation != null)
                    {
                        _form.NavigationDgv.Rows.Clear();


                        foreach (var i in _file.Navigation[(int)(e.Y / _scaler.ZoomFactor) + _form.Vertical.Value,
                            (int)(e.X / _scaler.ZoomFactor) + _form.Horizontal.Value])
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
                    if (!_form.MarkPointRb.Checked && !_form.MarkAreaRb.Checked)
                    {
                        _form.Canvas.Cursor = Cursors.SizeAll;
                        _drag.StartTracing(e.Location, !_form.MarkPointRb.Checked);
                        return;
                    }
                    else if (_form.MarkAreaRb.Checked)
                    {
                        _areaSelector.ResetArea();
                        _areaSelector.StartArea(new Point((int)Math.Ceiling(e.X / _scaler.ZoomFactor) + _form.Horizontal.Value,
                            (int)Math.Ceiling(e.Y / _scaler.ZoomFactor) + _form.Vertical.Value),
                            new Point((int)(_form.Horizontal.Value), (int)(_form.Vertical.Value)));
                    }
                    else if (_form.MarkPointRb.Checked)
                    {
                        _pointSelector.Add((RlViewer.Files.LocatorFile)_file,
                            new System.Drawing.Point((int)(e.X / _scaler.ZoomFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ZoomFactor) + _form.Vertical.Value));
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
                    if ((_form.Vertical.Value - _drag.Delta.Y) >= 0 &&
                        (_form.Vertical.Value - _drag.Delta.Y) < _form.Vertical.Maximum)
                    {
                        _form.Vertical.Value -= _drag.Delta.Y;
                    }
                    if ((_form.Horizontal.Value - _drag.Delta.X) >= 0 &&
                        (_form.Horizontal.Value - _drag.Delta.X) < _form.Horizontal.Maximum)
                    {
                        _form.Horizontal.Value -= _drag.Delta.X;
                    }
                    DrawImage();
                }
                else if (_areaSelector != null && _form.MarkAreaRb.Checked)
                {
                    if (_areaSelector.ResizeArea(new System.Drawing.Point((int)(e.X / _scaler.ZoomFactor) + _form.Horizontal.Value,
                                (int)(e.Y / _scaler.ZoomFactor) + _form.Vertical.Value), new Point(_form.Horizontal.Value, _form.Vertical.Value)))
                    {
                        DrawItems();
                    }
                }
                
            }         
        }


        public void ClickFinished(MouseEventArgs e)
        {
            if (_file != null && _drag != null)
            {
                _form.Canvas.Cursor = Cursors.Arrow;
                _drag.StopTracing();
                if (e.Button == MouseButtons.Left && _areaSelector != null)
                {
                    _areaSelector.StopResizing();
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


        private void ClearCancelledFileTiles()
        {
            try
            {          
                var path = Path.Combine("tiles", Path.GetFileNameWithoutExtension(_file.Properties.FilePath), 
                    Path.GetExtension(_file.Properties.FilePath));
                File.SetAttributes(path, FileAttributes.Normal);
                System.Threading.Thread.Sleep(3000);
                Directory.Delete(path, true);
            }
            catch(Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                ErrorGuiMessage(ex.Message);
            }
        }

        private System.ComponentModel.BackgroundWorker InitWorker(System.ComponentModel.DoWorkEventHandler doWork,
            System.ComponentModel.RunWorkerCompletedEventHandler completed)
        {
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += doWork;
            //worker.ProgressChanged += _loaderWorker_ProgressChanged;
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
