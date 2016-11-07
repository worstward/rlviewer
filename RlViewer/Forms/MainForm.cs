using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;


namespace RlViewer.Forms
{

    public partial class MainForm : Form
    {

        public MainForm()
        {
            //var mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("SSTP_inSharedMem", (long)(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 20));
            System.Threading.EventWaitHandle handle = new System.Threading.EventWaitHandle(false, EventResetMode.AutoReset, "SSTP_Ready");
            handle.WaitOne();

            var mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting("SSTP_inSharedMem");
            var fStream = mmf.CreateViewStream();

           
            var buf = new byte[Marshal.SizeOf(typeof(Behaviors.Synthesis.ServerSarTaskParams))];

            fStream.Read(buf, 0, buf.Length);

            var sstp = Behaviors.Converters.StructIO.ReadStruct<Behaviors.Synthesis.ServerSarTaskParams>(buf);
            var asd = sstp;

       
            InitializeComponent();
            _guiFacade = new UI.GuiFacade(pictureBox1.Size, action => Invoke(action));
            _guiFacade.OnPointOfViewMaxChanged += (s, e) => CheckScrollBarVisibility();
            _guiFacade.OnProgressVisibilityChanged += (s, e) => InitProgressControls(e.IsVisible);
            _guiFacade.OnProgressChanged += (s, e) => ReportProgress(e.Progress);
            _guiFacade.OnImageDrawn += (s, image) => pictureBox1.Image = image;
            _guiFacade.OnAlignPossibilityChanged += (s, e) => alignBtn.Enabled = e.IsPossible;
            _guiFacade.OnErrorOccured += (s, e) => Forms.FormsHelper.ShowErrorMsg(e.ErrorText);

            _keyProcessor = new UI.KeyPressProcessor(() => _guiFacade.Undo(markPointRb.Checked, markAreaRb.Checked),
                () =>
                {
                    navigationDgv.Rows.Clear();
                    this.Text = _guiFacade.OpenFile();
                },
                 () => _guiFacade.Save(), () => _guiFacade.ShowFileInfo(), () => _guiFacade.ShowLog(),
                 () => _guiFacade.ReportDialog(), () => _guiFacade.AggregateFiles(), () => _guiFacade.EmbedNavigation(),
                 () => _guiFacade.ShowCache());

            InitForm();
            InitDataBindings();
        }

        private void InitForm()
        {
            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;

            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;

            AddToolTips();
            navigationDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            alignBtn.Enabled = false;
            filterTrackBar.SmallChange = 1;
            filterTrackBar.LargeChange = 1;
            filterTrackBar.Minimum = -16;
            filterTrackBar.Maximum = 16;
            filterTrackBar.Value = 0;

            horizontalScrollBar.Visible = false;
            verticalScrollBar.Visible = false;

            InitProgressControls(false);
        }

        private void InitDataBindings()
        {
            var scaleBinding = new Binding("Text", _guiFacade, "ScaleFactor", true, DataSourceUpdateMode.OnPropertyChanged);
            scaleBinding.Format += delegate(object sender, ConvertEventArgs convertedArgs)
            {
                convertedArgs.Value = string.Format("Масштаб: {0}%", (Convert.ToSingle(convertedArgs.Value)) * 100);
            };


            scaleLabel.DataBindings.Add(scaleBinding);
            distanceLabel.DataBindings.Add("Text", _guiFacade, "RulerDistance", false, DataSourceUpdateMode.OnPropertyChanged);
            statusLabel.DataBindings.Add("Text", _guiFacade, "CurrentTaskName", false, DataSourceUpdateMode.OnPropertyChanged);
            verticalScrollBar.DataBindings.Add("Value", _guiFacade, "YPointOfView", false, DataSourceUpdateMode.OnPropertyChanged);
            verticalScrollBar.DataBindings.Add("Maximum", _guiFacade, "YPointOfViewMax", false, DataSourceUpdateMode.OnPropertyChanged);
            horizontalScrollBar.DataBindings.Add("Value", _guiFacade, "XPointOfView", false, DataSourceUpdateMode.OnPropertyChanged);
            horizontalScrollBar.DataBindings.Add("Maximum", _guiFacade, "XPointOfViewMax", false, DataSourceUpdateMode.OnPropertyChanged);
        }


        private void ReportProgress(int progress)
        {
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripProgressBar>(progressBar, pb => { pb.Value = progress; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(progressLabel, pl =>
            { pl.Text = string.Format("{0} %", progress.ToString()); });
        }




        private void InitProgressControls(bool isVisible)
        {
            progressBar.Visible = isVisible;
            progressBar.Value = 0;
            statusLabel.Visible = isVisible;
            progressLabel.Visible = isVisible;
            progressLabel.Text = "0%";
            cancelBtn.Visible = isVisible;

        }


        private UI.KeyPressProcessor _keyProcessor;

        private UI.GuiFacade _guiFacade;

        public void ToggleNavigation()
        {
            TogglePanel(navigationPanelCb.Checked, naviSplitter);
        }

        public void ToggleFilters()
        {
            TogglePanel(filterPanelCb.Checked, filterSplitter);
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
            _guiFacade.InitDrawImage();
        }

        private void MouseClickStarted(MouseEventArgs e)
        {
            if (dragRb.Checked)
            {
                Cursor = Cursors.SizeAll;
                _guiFacade.DragStart(e.Location);
                return;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Cursor = Cursors.SizeAll;
                _guiFacade.DragStart(e.Location);
                return;
            }

            if (analyzeRb.Checked)
            {
                Cursor = Cursors.Cross;
                _guiFacade.GetPointAmplitudeStart(e.Location, pictureBox1);
            }
            else
            {
                Cursor = Cursors.Arrow;
                if (markAreaRb.Checked)
                {
                    _guiFacade.SelectAreaStart(e.Location);
                }
                else if (sharerRb.Checked)
                {
                    _guiFacade.ShareCoords(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    _guiFacade.SelectPointStart(e.Location);
                }

                else if (verticalSectionRb.Checked)
                {
                    _guiFacade.VerticalSectionStart(e.Location);
                }
                else if (horizontalSectionRb.Checked)
                {
                    _guiFacade.HorizontalSectionStart(e.Location);
                }
                else if (linearSectionRb.Checked)
                {
                    _guiFacade.LinearSectionStart(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    _guiFacade.RulerStart(e.Location);
                }
            }
        }

        private void MouseMoving(MouseEventArgs e)
        {
            if (dragRb.Checked || e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _guiFacade.Drag(e.Location);
            }
            else if (markAreaRb.Checked)
            {
                _guiFacade.SelectArea(e.Location);
            }
            else if (markPointRb.Checked)
            {
                _guiFacade.SelectPoint(e.Location);
            }
            else if (analyzeRb.Checked)
            {
                _guiFacade.GetPointAmplitude(e.Location, pictureBox1);
            }
            else if (verticalSectionRb.Checked)
            {
                _guiFacade.VerticalSection(e.Location);
            }
            else if (horizontalSectionRb.Checked)
            {
                _guiFacade.HorizontalSection(e.Location);
            }
            else if (linearSectionRb.Checked)
            {
                _guiFacade.LinearSection(e.Location);
            }
            else if (rulerRb.Checked)
            {
                _guiFacade.GetDistance(e.Location);
            }

        }

        private void MouseClickFinished(MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            _guiFacade.DragFinish();

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (markAreaRb.Checked)
                {
                    _guiFacade.SelectAreaFinish(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    _guiFacade.SelectPointFinish(e.Location);
                }
                else if (analyzeRb.Checked)
                {
                    _guiFacade.StopPointAnalyzer(pictureBox1);
                }
                else if (verticalSectionRb.Checked || horizontalSectionRb.Checked || linearSectionRb.Checked)
                {
                    _guiFacade.StopSection(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    _guiFacade.GetDistance(e.Location);
                }
            }


        }


        private void CheckScrollBarVisibility()
        {
            horizontalScrollBar.Visible = horizontalScrollBar.Maximum > 0 ? true : false;
            verticalScrollBar.Visible = verticalScrollBar.Maximum > 0 ? true : false;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            navigationDgv.Rows.Clear();
            Text = _guiFacade.OpenFile();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            _guiFacade.DrawImage();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            _guiFacade.DrawImage();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _guiFacade.InitDrawImage(pictureBox1.Size);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMoving(e);
            coordinatesLabel.Text = _guiFacade.ShowMousePosition(e.Location);

            if (navigationPanelCb.Checked)
            {
                var currentNavigation = _guiFacade.ShowNavigation(e);
                if (currentNavigation != null)
                {
                    navigationDgv.Rows.Clear();
                    foreach (var item in currentNavigation)
                    {
                        navigationDgv.Rows.Add(item.ParameterName, item.ParameterValue);
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseClickStarted(e);
            _guiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MouseClickFinished(e);
            _guiFacade.DrawImage();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            _guiFacade.ScaleImage(e.Delta, e.Location);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Focus();
        }


        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _guiFacade.ChangeFilterValue(filterTrackBar.Value);
            filterLbl.Text = string.Format("Уровень фильтра: {0}", filterTrackBar.Value);
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                filterTrackBar.Value = _guiFacade.GetFilter(Behaviors.Filters.FilterType.Contrast, 4);
            }
        }

        private void gammaCorrRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                filterTrackBar.Value = _guiFacade.GetFilter(Behaviors.Filters.FilterType.GammaCorrection, 0);
            }
        }

        private void brightnessRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                filterTrackBar.Value = _guiFacade.GetFilter(Behaviors.Filters.FilterType.Brightness, 4);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _guiFacade.CancelLoading();
            _guiFacade.Dispose();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _keyProcessor.ProcessKeyPress(e);
        }

        private void оФайлеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowFileInfo();
        }

        private void логToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowLog();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.Save();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _guiFacade.InitDrawImage();
        }

        private void naviPanelCb_CheckedChanged(object sender, EventArgs e)
        {
            ToggleNavigation();
        }

        private void filterPanelCb_CheckedChanged(object sender, EventArgs e)
        {
            ToggleFilters();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = _guiFacade.OpenWithDoubleClick();
        }


        private void alignBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ResampleImage();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            _guiFacade.CancelLoading();
        }

        private void resetFilterBtn_Click(object sender, EventArgs e)
        {
            filterTrackBar.Value = 0;
            _guiFacade.ResetFilter();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            _guiFacade.MoveFileDragDrop(e);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            navigationDgv.Rows.Clear();
            Text = _guiFacade.OpenFileDragDrop(e);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowAbout();
        }

        private void статусКешаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowCache();
        }

        private void findPointBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowFindPoint();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _guiFacade.InitDrawImage();
        }

        private void rulerRb_CheckedChanged(object sender, EventArgs e)
        {
            _guiFacade.ResetRuler();
            distanceLabel.Text = string.Empty;
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ScaleImage(-1);
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ScaleImage(1);
        }

        private void создатьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ReportDialog();
        }

        private void statisticsBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.GetAreaStatistics();
        }


        private void вшитьНавигациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.EmbedNavigation();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            _guiFacade.DrawItems(e.Graphics);
        }

        private void совместитьФайлыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.AggregateFiles();
        }

        private void mirrorImageBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.MirrorImage();
        }


        private void AddToolTips()
        {
            Forms.FormsHelper.AddToolTip(alignBtn, "Выровнять");
            Forms.FormsHelper.AddToolTip(analyzeRb, "Анализ амплитуды");
            Forms.FormsHelper.AddToolTip(dragRb, "Перемещение по изображению");
            Forms.FormsHelper.AddToolTip(horizontalSectionRb, "Горизонтальное сечение");
            Forms.FormsHelper.AddToolTip(linearSectionRb, "Произвольное сечение");
            Forms.FormsHelper.AddToolTip(markAreaRb, "Область");
            Forms.FormsHelper.AddToolTip(markPointRb, "Отметка");
            Forms.FormsHelper.AddToolTip(navigationPanelCb, "Навигация");
            Forms.FormsHelper.AddToolTip(rulerRb, "Линейка");
            Forms.FormsHelper.AddToolTip(findPointBtn, "Поиск точки");
            Forms.FormsHelper.AddToolTip(verticalSectionRb, "Вертикальное сечение");
            Forms.FormsHelper.AddToolTip(brightnessRb, "Яркость");
            Forms.FormsHelper.AddToolTip(contrastRb, "Контрастность");
            Forms.FormsHelper.AddToolTip(gammaCorrRb, "Гамма");
            Forms.FormsHelper.AddToolTip(resetFilterBtn, "Сброс фильтров");
            Forms.FormsHelper.AddToolTip(filterPanelCb, "Фильтры");
            Forms.FormsHelper.AddToolTip(zoomInBtn, "Увеличить масштаб");
            Forms.FormsHelper.AddToolTip(zoomOutBtn, "Уменьшить масштаб");
            Forms.FormsHelper.AddToolTip(statisticsBtn, "Статистика");
            Forms.FormsHelper.AddToolTip(squareAreaRb, "Трехмерный график");
            Forms.FormsHelper.AddToolTip(sharerRb, "Сравнить точки");
            Forms.FormsHelper.AddToolTip(mirrorImageBtn, "Отразить изображение");
        }

        private void sharerRb_CheckedChanged(object sender, EventArgs e)
        {
            _guiFacade.AllowRemoteDataReceiving = ((RadioButton)sender).Checked;
        }

    }
}
