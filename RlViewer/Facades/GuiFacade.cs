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

namespace RlViewer.Facades
{
    public class GuiFacade
    {
        public GuiFacade(ISuitableForm form)
        {
            _form = form;
            _drag = new Behaviors.DragController();
            _loaderWorker = InitWorker();
            _filterFacade = new ImageFilterFacade();
            _keyboardFacade = new KeyboardFacade(() => Undo(), () => OpenFile());

            InitControls();
        }

        private System.ComponentModel.BackgroundWorker _loaderWorker;
        private Settings.Settings _settings = new Settings.Settings();
        private ImageFilterFacade _filterFacade;
        private KeyboardFacade _keyboardFacade;
        private Behaviors.Saving.Abstract.Saver _saver;


        private Files.LoadedFile _file;
        private HeaderInfoOutput[] _info;
        private RlViewer.Behaviors.TileCreator.Tile[] _tiles;
        private RlViewer.Facades.DrawerFacade _drawer;
        private RlViewer.Behaviors.PointSelector.PointSelector _pointSelector;
        private RlViewer.Behaviors.AreaSelector.AreaSelector _areaSelector;
        private RlViewer.Behaviors.DragController _drag;

        private ISuitableForm _form;

        public string OpenFile()
        {
            string caption;
            using (var openFileDlg = new OpenFileDialog() { Filter = Resources.OpenFilter })
            {
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {                
                    Files.FileProperties properties = null;

                    try
                    {
                        if (_loaderWorker != null && _loaderWorker.IsBusy)
                        {
                            CancelLoading();
                        }

                        _file = null;
                        _drawer = null;
                        _form.Canvas.Image = null;
                        _loaderWorker = InitWorker();    

                        properties = new Files.FileProperties(openFileDlg.FileName);
                    }
                    catch (ArgumentException aex)
                    {
                        Logging.Logger.Log(Logging.SeverityGrades.Blocking,
                            string.Format("Error opening file {0}", aex.Message));
                        ErrorGuiMessage("Невозможно открыть файл");
                        return string.Empty;
                    }
                    _file = FileFactory.GetFactory(properties).Create(properties);
                    caption = _file.Properties.FilePath;
                    LoadFile();
                }
                else
                {
                    caption = string.Empty;
                }

                
                return caption;
            }
        }


        public void LoadFile()
        {
            if (_file != null)
            {
                try
                {
                    _info = ((RlViewer.Files.LocatorFile)_file).Header.GetHeaderInfo();
                }
                catch(InvalidCastException icex)
                {
                    _info = null;
                    ErrorGuiMessage(icex.Message);
                }

                if (_info == null)
                {
                    ErrorGuiMessage("Файл поврежден");
                    _file = null;
                    InitControls();
                }
                else
                {

                    GetImage();
                }
            }
        }


        public void ShowFileInfo()
        {
            if(_file != null)
            {
                using (var iFrm = new Forms.InfoForm(_info))
                {                   
                    iFrm.ShowDialog();
                }
            }
        }


        public void GetImage()
        {
            if (_file != null)
            {
                _filterFacade.GetFilter("Brightness", 4);

                _form.ProgressBar.Value = 0;
                _form.ProgressBar.Visible = true;
                _form.ProgressLabel.Visible = true;
                _form.ProgressLabel.Text = "0 %";
                _form.CancelButton.Visible = true;
                _form.Horizontal.Visible = false;
                _form.Vertical.Visible = false;
                _loaderWorker.RunWorkerAsync();
                //Task.Run(() =>
                //{
                //    return TileCreatorFactory.GetFactory(_file.Properties).Create(_file as RlViewer.Files.LocatorFile).Tiles;
                //}).ContinueWith((tileCreationTask) =>
                //{
                //    _tiles = tileCreationTask.Result;
                //    _pointSelector = new Behaviors.PointSelector.PointSelector();
                //    InitDrawImage();
                //}, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void CancelLoading()
        {
            if (_file != null && !_loaderWorker.CancellationPending && _loaderWorker.IsBusy)
            {
                _loaderWorker.CancelAsync();
                DisposeWorker(_loaderWorker);
                ClearCancelledFileTiles();
                InitControls();
            }
        }


        private void _loaderWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_settings.AllowViewWhileLoading)
            {
                _tiles = TileCreatorFactory.GetFactory(_file.Properties)
                   .Create(_file as RlViewer.Files.LocatorFile).GetTiles(_file.Properties.FilePath);
            }
            else
            {
                _tiles = TileCreatorFactory.GetFactory(_file.Properties)
                    .Create(_file as RlViewer.Files.LocatorFile).GetTiles(_file.Properties.FilePath, _loaderWorker);
            }
        }


        private void _loaderWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            _form.ProgressBar.Value = e.ProgressPercentage;
            _form.ProgressLabel.Text = string.Format("{0} %", e.ProgressPercentage.ToString());
        }

        private void _loaderWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_tiles != null)
            {
                _pointSelector = new Behaviors.PointSelector.PointSelector();
                _areaSelector = new Behaviors.AreaSelector.AreaSelector();
                _form.ProgressBar.Visible = false;
                _form.ProgressLabel.Visible = false;
                _form.CancelButton.Visible = false;
                InitDrawImage();
            }
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
                RlViewer.Behaviors.Draw.TileDrawer tDrawer = new Behaviors.Draw.TileDrawer(_filterFacade.Filter);
                RlViewer.Behaviors.Draw.ItemDrawer iDrawer = new Behaviors.Draw.ItemDrawer(_pointSelector, _areaSelector);
                _drawer = new RlViewer.Facades.DrawerFacade(_form.Canvas.Size, iDrawer, tDrawer);

                ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
                InitScrollBars();
                DrawImage();
            }
        }

        public void DrawImage()
        {
            if (_file != null && _drawer != null && _tiles != null)
            {
                _form.Canvas.Image = _drawer.Draw(_tiles,
                        new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
            }
        }

        private void DrawItems()
        {
            if (_file != null && _drawer != null)
            {
                _form.Canvas.Image = _drawer.Draw(new System.Drawing.Point(_form.Horizontal.Value, _form.Vertical.Value));
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
                    sfd.Filter = Resources.SaveFilter;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                       
                        var loc = _file as RlViewer.Files.LocatorFile;
                        _saver = new Behaviors.Saving.Concrete.Rl4Saver(loc);
                        _saver.Save(sfd.FileName, FileType.brl4, new Point(0, 0), new Size(loc.Width, loc.Height));
                    }
                }
            }

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
            _form.CancelButton.Visible = false;
        }

        private void ErrorGuiMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InitScrollBars()
        {
            //Task.Run(() =>
            //{
            //    return _file as RlViewer.Files.LocatorFile;
            //})
            //.ContinueWith((t) =>
            //{
            //    _horizontal.Maximum = t.Result.Width - _pictureBox.Size.Width;
            //    _vertical.Maximum = t.Result.Height - _pictureBox.Size.Height;
            //    _horizontal.Visible = true;
            //    _vertical.Visible = true;
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            var f = _file as RlViewer.Files.LocatorFile;
            var horMax = f.Width - _form.Canvas.Size.Width;
            var verMax = f.Height - _form.Canvas.Size.Height;
            _form.Horizontal.Maximum = horMax > 0 ? horMax : 0;
            _form.Vertical.Maximum = verMax > 0 ? verMax : 0;
            _form.Horizontal.Visible = _form.Horizontal.Maximum > 0 ? true : false;
            _form.Vertical.Visible = _form.Vertical.Maximum > 0 ? true : false;
        }



        public void ChangeFilterValue()
        {
            _filterFacade.ChangeFilterValue(_form.FilterTrackBar.Value);
        }

        public void GetFilter(string filterType, int filterDelta)
        {
            _filterFacade.GetFilter(filterType, filterDelta);
            _form.FilterTrackBar.Value = _filterFacade.Filter.FilterValue >> filterDelta;
        }

        #region pictureBoxMouseHandlers
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
                        _areaSelector.StartArea(e.Location, new Point(_form.Horizontal.Value, _form.Vertical.Value));
                    }
                    else if (_form.MarkPointRb.Checked)
                    {
                        _pointSelector.AddManualVal((RlViewer.Files.LocatorFile)_file,
                            new System.Drawing.Point(e.X + _form.Horizontal.Value, e.Y + _form.Vertical.Value));
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
                    if ((_form.Vertical.Value - _drag.Delta.Y) >= 0 && (_form.Vertical.Value - _drag.Delta.Y) < _form.Vertical.Maximum)
                    {
                        _form.Vertical.Value -= _drag.Delta.Y;
                    }
                    if ((_form.Horizontal.Value - _drag.Delta.X) >= 0 && (_form.Horizontal.Value - _drag.Delta.X) < _form.Horizontal.Maximum)
                    {
                        _form.Horizontal.Value -= _drag.Delta.X;
                    }
                    DrawImage();
                }
                else if (_areaSelector != null && _form.MarkAreaRb.Checked)
                {
                    _areaSelector.ResizeArea(e);
                    DrawItems();
                }
                
            }         
        }


        public void ClickFinished()
        {
            if (_file != null && _drag != null)
            {
                _form.Canvas.Cursor = Cursors.Arrow;
                _drag.StopTracing();
                if (_areaSelector != null)
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
                _file = null;
                _drawer = null;
                _pointSelector = null;
                _areaSelector = null;
                System.Threading.Thread.Sleep(3000);
                Directory.Delete(path, true);
            }
            catch(Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                ErrorGuiMessage(ex.Message);
            }
        }

        private System.ComponentModel.BackgroundWorker InitWorker()
        {
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += _loaderWorker_DoWork;
            worker.ProgressChanged += _loaderWorker_ProgressChanged;
            worker.RunWorkerCompleted += _loaderWorker_RunWorkerCompleted;

            return worker;
        }

        private void DisposeWorker(System.ComponentModel.BackgroundWorker worker)
        {
            worker.DoWork -= _loaderWorker_DoWork;
            worker.ProgressChanged -= _loaderWorker_ProgressChanged;
            worker.RunWorkerCompleted -= _loaderWorker_RunWorkerCompleted;
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
