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

namespace RlViewer.GuiFacade
{
    public class GuiFacade
    {
        public GuiFacade(ISuitableForm form)
        {
            _pictureBox = form.Canvas;
            _horizontal = form.Horizontal;
            _vertical   = form.Vertical;
            _filterTrackbar = form.TrackBar;
            _progressBar = form.ProgressBar;
            _progressLabel = form.ProgressLabel;
            _cancelButton = form.CancelButton;

            _drag = new Behaviors.DragController();
            _loaderWorker = InitWorker();
            _filterFacade = new ImageFilterFacade(_filterTrackbar);
            _keyboardFacade = new KeyboardFacade(
                () =>
                {
                    if (_file != null)
                    {
                        _selector.RemoveLast();
                        DrawImage();
                    }
                },
                () => OpenFile());

            InitControls();
        }

        private System.ComponentModel.BackgroundWorker _loaderWorker;
        private Settings.Settings _settings = new Settings.Settings();
        private ImageFilterFacade _filterFacade;
        private KeyboardFacade _keyboardFacade;
        
        internal ImageFilterFacade FilterFacade
        {
            get { return _filterFacade; }
        }

        private Files.LoadedFile _file;
        private HeaderInfoOutput[] _info;
        private RlViewer.Behaviors.TileCreator.Tile[] _tiles;
        private RlViewer.Behaviors.Draw.Drawing _drawer;
        private RlViewer.Behaviors.PointSelector.PointSelector _selector;
        private RlViewer.Behaviors.DragController _drag;

        private PictureBox _pictureBox;
        private HScrollBar _horizontal;
        private VScrollBar _vertical;
        private TrackBar _filterTrackbar;
        private ProgressBar _progressBar;
        private Label _progressLabel;
        private Button _cancelButton;

        public string OpenFile()
        {
            string caption;
            using (var openFileDlg = new OpenFileDialog() { Filter = Resources.Filter })
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
                        _pictureBox.Image = null;
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
                }
                else
                {
                    caption = string.Empty;
                }
                LoadFile();
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
                using (var iFrm = new InfoForm(_info))
                {                   
                    iFrm.ShowDialog();
                }
            }
        }


        public void GetImage()
        {
            if (_file != null)
            {
                _progressBar.Value = 0;
                _progressBar.Visible = true;
                _progressLabel.Visible = true;
                _progressLabel.Text = "0 %";
                _cancelButton.Visible = true;
                _horizontal.Visible = false;
                _vertical.Visible = false;
                _loaderWorker.RunWorkerAsync();
                //Task.Run(() =>
                //{
                //    return TileCreatorFactory.GetFactory(_file.Properties).Create(_file as RlViewer.Files.LocatorFile).Tiles;
                //}).ContinueWith((tileCreationTask) =>
                //{
                //    _tiles = tileCreationTask.Result;
                //    _selector = new Behaviors.PointSelector.PointSelector();
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
            _progressBar.Value = e.ProgressPercentage;
            _progressLabel.Text = string.Format("{0} %", e.ProgressPercentage.ToString());
        }

        private void _loaderWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_tiles != null)
            {
                _selector = new Behaviors.PointSelector.PointSelector();
                _progressBar.Visible = false;
                _progressLabel.Visible = false;
                _cancelButton.Visible = false;
                InitDrawImage();
            }
        }

        public void InitDrawImage()
        {
            if (_pictureBox.Size.Width != 0 && _pictureBox.Size.Height != 0 && _tiles != null)
            {               
                _drawer = new Behaviors.Draw.Drawing(_pictureBox.Size, _filterFacade.Filter, _selector);
                ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
                InitScrollBars();
                DrawImage();
            }
        }

        public void DrawImage()
        {
            //System.Threading.ThreadPool.QueueUserWorkItem((e) =>
            //    {
            //        _pictureBox.Image = _drawer.DrawImage(_tiles, new System.Drawing.Point(_horizontal.Value, _vertical.Value));
            //    });

            //if (_drawer != null)
            //{
            //    Task.Run(() =>
            //        {
            //            return _drawer.DrawImage(_tiles, new System.Drawing.Point(_horizontal.Value, _vertical.Value));
            //        })
            //        .ContinueWith((t) =>
            //        {
            //            _pictureBox.Image = t.Result;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //}

            if (_file != null && _drawer != null && _tiles != null)
            {
                _pictureBox.Image = _drawer.DrawImage(_tiles,
                    new System.Drawing.Point(_horizontal.Value, _vertical.Value));
            }
        }

        public void ChangePalette(int[] rgb, bool isReversed)
        {
            if (_drawer != null)
            {
                _drawer.GetPalette(rgb[0], rgb[1], rgb[2], isReversed);
                if (_file != null && _tiles != null)
                {
                    _pictureBox.Image = _drawer.DrawImage(_tiles,
                        new System.Drawing.Point(_horizontal.Value, _vertical.Value));
                }
            }
        }
        

        private void InitControls()
        {
            _pictureBox.Image = null;
            _horizontal.Visible = false;
            _vertical.Visible = false;

            _filterTrackbar.SmallChange = 1;
            _filterTrackbar.LargeChange = 1;
            _filterTrackbar.Minimum = -16;
            _filterTrackbar.Maximum = 16;
            _filterTrackbar.Value = 0;

            _progressBar.Minimum = 0;
            _progressBar.Maximum = 100;
            _progressBar.Visible = false;
            _progressLabel.Visible = false;
            _cancelButton.Visible = false;
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
            var horMax = f.Width - _pictureBox.Size.Width;
            var verMax = f.Height - _pictureBox.Size.Height;
            _horizontal.Maximum = horMax > 0 ? horMax : 0;
            _vertical.Maximum = verMax > 0 ? verMax : 0;
            _horizontal.Visible = _horizontal.Maximum > 0 ? true : false;
            _vertical.Visible = _vertical.Maximum > 0 ? true : false;
        }

        public void MarkPoint(System.Drawing.Point location)
        {
            if (_file != null && _selector != null)
            {
                _selector.Add((RlViewer.Files.LocatorFile)_file,
                    new System.Drawing.Point(location.X + _horizontal.Value, location.Y + _vertical.Value));
            }
        }

        public void ClickStarted(MouseEventArgs e, bool canMark)
        {
            if (_file != null && _drawer != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!canMark)
                    {
                        _pictureBox.Cursor = Cursors.SizeAll;
                        _drag.StartTracing(e.Location, !canMark);
                        return;
                    }
                    MarkPoint(e.Location);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    _pictureBox.Cursor = Cursors.SizeAll;
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
                    if ((_vertical.Value - _drag.Delta.Y) >= 0 && (_vertical.Value - _drag.Delta.Y) < _vertical.Maximum)
                    {
                        _vertical.Value -= _drag.Delta.Y;
                    }
                    if ((_horizontal.Value - _drag.Delta.X) >= 0 && (_horizontal.Value - _drag.Delta.X) < _horizontal.Maximum)
                    {
                        _horizontal.Value -= _drag.Delta.X;
                    }
                    DrawImage();
                }
            }         
        }


        public void ClickFinished()
        {
            if (_file != null &&  _drag != null)
            {
                _pictureBox.Cursor = Cursors.Arrow;
                _drag.StopTracing();
            }
        }


        public void ShowSettings()
        {
            using (var settgingsForm = new SettingsForm(_settings))
            {
                if (settgingsForm.ShowDialog() == DialogResult.OK)
                {
                    ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
                }

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
                _selector = null;
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
