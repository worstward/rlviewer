using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RlViewer.Behaviors.TileCreator.Abstract;
using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Factories.File.Abstract;

namespace RlViewer.GuiFacade
{
    class GuiFacade
    {
        public GuiFacade(ISuitableForm form)
        {
            _pictureBox = form.Canvas;
            _horizontal = form.Horizontal;
            _vertical   = form.Vertical;
            _filterTrackbar = form.TrackBar;
            _progressBar = form.ProgressBar;
            _reverseCheckBox = form.ReverseCheckBox;
            _paletteComboBox = form.PaletteComboBox;

            _drag = new Behaviors.DragController();
            InitControls();
            InitWorker();
        }
        
        private System.ComponentModel.BackgroundWorker _worker = new System.ComponentModel.BackgroundWorker();
                
        private Files.LoadedFile _file;
        private HeaderInfoOutput[] _info;
        private RlViewer.Behaviors.TileCreator.Tile[] _tiles;
        private RlViewer.Behaviors.Draw.Drawing _drawer;
        private RlViewer.Behaviors.PointSelector.PointSelector _selector;
        private RlViewer.Behaviors.DragController _drag;
        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;
        private int _filterDelta;


        private PictureBox _pictureBox;
        private HScrollBar _horizontal;
        private VScrollBar _vertical;
        private TrackBar _filterTrackbar;
        private ProgressBar _progressBar;
        private CheckBox _reverseCheckBox;
        private ComboBox _paletteComboBox;


        public string OpenFile(string fileName)
        {
            Files.FileProperties properties = null;
            _file = null;
            _drawer = null;
            _pictureBox.Image = null;
            try
            {
                properties = new Files.FileProperties(fileName);
            }
            catch (ArgumentException aex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, string.Format("Error opening file {0}", aex.Message));
                ErrorGuiMessage("Невозможно открыть файл");
                return string.Empty;
            }
            _file = FileFactory.GetFactory(properties).Create(properties);
            return _file.Properties.FilePath;
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
                _worker.RunWorkerAsync();
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



        private void _worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
           
            _tiles = TileCreatorFactory.GetFactory(_file.Properties)
                .Create(_file as RlViewer.Files.LocatorFile).GetTilesReportProgress(_file.Properties.FilePath, _worker);
        }

        private void _worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            _progressBar.Value = e.ProgressPercentage;
        }

        private void _worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _selector = new Behaviors.PointSelector.PointSelector();
            _progressBar.Visible = false;
            InitDrawImage();
        }

        public void InitDrawImage()
        {
            if (_pictureBox.Size.Width != 0 && _pictureBox.Size.Height != 0 && _tiles != null)
            {               
                _drawer = new Behaviors.Draw.Drawing(_pictureBox.Size, _filter, _selector);
                InitScrollBars();
                DrawImage();
            }
        }

        public void GetFilter(string filterType, int filterDelta)
        {
            _filterDelta = filterDelta;
            _filter = RlViewer.Factories.Filter.Abstract.FilterFactory.GetFactory(filterType).GetFilter();
            _filterTrackbar.Value = _filter.FilterValue >> _filterDelta;
        }
        
        public void ChangeFilterValue()
        {
            _filter.FilterValue = _filterTrackbar.Value << _filterDelta;
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

            if (_file != null && _drawer != null)
            {
                _pictureBox.Image = _drawer.DrawImage(_tiles, new System.Drawing.Point(_horizontal.Value, _vertical.Value));
            }
        }

        public void ChangePalette()
        {
            if (_file != null && _drawer != null)
            {
                int[] rgb;
                try
                {
                    rgb = _paletteComboBox.GetItemText(_paletteComboBox.SelectedItem).Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                }
                catch (Exception ex)
                {
                    rgb = new int[] { 1, 1, 1 };
                }
                _drawer.GetPalette(rgb[0], rgb[1], rgb[2], _reverseCheckBox.Checked);
                _pictureBox.Image = _drawer.DrawImage(_tiles, new System.Drawing.Point(_horizontal.Value, _vertical.Value));
            }
        }
        

        private void InitControls()
        {
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
            _horizontal.Maximum = f.Width - _pictureBox.Size.Width;
            _vertical.Maximum = f.Height - _pictureBox.Size.Height;
            _horizontal.Visible = true;
            _vertical.Visible = true;
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


        private void InitWorker()
        {
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += _worker_DoWork;
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }

    }
}
