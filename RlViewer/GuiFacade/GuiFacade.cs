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
            _pictureBox = form.PictureBox;
            _horizontal = form.Horizontal;
            _vertical   = form.Vertical;
            _filterTrackbar = form.TrackBar;
            _progressBar = form.ProgressBar;

            _drag = new Behaviors.DragController();
            InitControls();
            InitWorker();
        }
        
        private System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                
        private Files.LoadedFile _file;
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
                MessageBox.Show("Невозможно открыть файл", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            _file = FileFactory.GetFactory(properties).Create(properties);
            return _file.Properties.FilePath;
        }


        public void LoadFile()
        {
            if (_file != null)
            {
                HeaderInfoOutput[] info = ((RlViewer.Files.LocatorFile)_file).Header.GetHeaderInfo();

                if (info == null)
                {
                    MessageBox.Show("Файл поврежден", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _file = null;
                }
                else
                {
                    using (var iFrm = new InfoForm(info))
                    {
                        GetImage();
                        iFrm.ShowDialog();
                    }
                }
            }
        }


        public void GetImage()
        {
            if (_file != null)
            {
                _progressBar.Value = 0;
                _progressBar.Visible = true;
                worker.RunWorkerAsync();
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



        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
           
            _tiles = TileCreatorFactory.GetFactory(_file.Properties)
                .Create(_file as RlViewer.Files.LocatorFile).GetTilesReportProgress(_file.Properties.FilePath, worker);
        }

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            _progressBar.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _selector = new Behaviors.PointSelector.PointSelector();
            _progressBar.Visible = false;
            InitDrawImage();
        }

        public void InitDrawImage()
        {
            if (_pictureBox.Size.Width != 0 && _pictureBox.Size.Height != 0 && _tiles != null)
            {
                InitScrollBars();
                _drawer = new Behaviors.Draw.Drawing(_pictureBox.Size, _filter, _selector);
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



        private void InitScrollBars()
        {

            Task.Run(() =>
            {
                return _file as RlViewer.Files.LocatorFile;
            })
            .ContinueWith((t) =>
            {
                _horizontal.Maximum = t.Result.Width - _pictureBox.Size.Width;
                _vertical.Maximum = t.Result.Height - _pictureBox.Size.Height;
                _horizontal.Visible = true;
                _vertical.Visible = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
           
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


        public void TraceMouseMovement(MouseEventArgs e, HScrollBar horizontal, VScrollBar vertical)
        {
            if (_file != null)
            {
                if (_drag.Trace(e.Location))
                {
                    if ((vertical.Value - _drag.Delta.Y) >= 0 && (vertical.Value - _drag.Delta.Y) < vertical.Maximum)
                    {
                        vertical.Value -= _drag.Delta.Y;
                    }
                    if ((horizontal.Value - _drag.Delta.X) >= 0 && (horizontal.Value - _drag.Delta.X) < horizontal.Maximum)
                    {
                        horizontal.Value -= _drag.Delta.X;
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
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

    }
}
