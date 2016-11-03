using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.Forms
{

    public partial class MainForm : Form, RlViewer.UI.ISuitableForm
    {
        public MainForm()
        {
            InitializeComponent();

            GuiFacade.OnPointOfViewMaxChanged += (s, e) => CheckScrollBarVisibility();
            GuiFacade.OnTaskNameChanged += (s, e) => ChangeStatusText(e.TaskName);
            GuiFacade.OnScaleFactorChanged += (s, e) => scaleLabel.Text = string.Format("Масштаб: {0}%", e.ScaleFactor * 100);
            GuiFacade.OnProgressVisibilityChanged += (s, e) => InitProgressControls(e.IsVisible);

            GuiFacade.OnImageDrawn += (s, image) => pictureBox1.Image = image;
            GuiFacade.OnDistanceChanged += (s, e) => distanceLabel.Text = e.DistanceString;
            GuiFacade.OnAlignPossibilityChanged += (s, e) => alignBtn.Enabled = e.IsPossible;


            _keyProcessor = new UI.KeyPressProcessor(() => GuiFacade.Undo(markPointRb.Checked, markAreaRb.Checked), () => this.Text = GuiFacade.OpenFile(),
                 () => GuiFacade.Save(), () => GuiFacade.ShowFileInfo(), () => GuiFacade.ShowLog(),
                 () => GuiFacade.ReportDialog(), () => GuiFacade.AggregateFiles(), () => GuiFacade.EmbedNavigation());

            InitForm();
            InitDataBindings();
        }

        private void InitForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

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
            verticalScrollBar.DataBindings.Add("Value", GuiFacade, "YPointOfView", false, DataSourceUpdateMode.OnPropertyChanged);
            verticalScrollBar.DataBindings.Add("Maximum", GuiFacade, "YPointOfViewMax", false, DataSourceUpdateMode.OnPropertyChanged);
            horizontalScrollBar.DataBindings.Add("Value", GuiFacade, "XPointOfView", false, DataSourceUpdateMode.OnPropertyChanged);
            horizontalScrollBar.DataBindings.Add("Maximum", GuiFacade, "XPointOfViewMax", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ChangeStatusText(string statusText)
        {
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripLabel>(statusLabel, lbl => { lbl.Text = statusText; });
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


        UI.KeyPressProcessor _keyProcessor;

        UI.GuiFacade _guiFacade;
        public UI.GuiFacade GuiFacade
        {
            get
            {
                return _guiFacade = _guiFacade ?? new UI.GuiFacade(this, pictureBox1.Size);
            }
        }


        #region ISuitableForm controls
        public PictureBox Canvas
        {
            get
            {
                return pictureBox1;
            }
        }

        public TrackBar FilterTrackBar
        {
            get
            {
                return filterTrackBar;
            }
        }


        public ToolStripProgressBar ProgressBar
        {
            get
            {
                return progressBar;
            }
        }
        public ToolStripStatusLabel ProgressLabel
        {
            get
            {
                return progressLabel;
            }
        }


        public Button AlignBtn
        {
            get
            {
                return alignBtn;
            }
        }


        public RadioButton MarkPointRb
        {
            get
            {
                return markPointRb;
            }
        }
        public RadioButton MarkAreaRb
        {
            get
            {
                return markAreaRb;
            }
        }

      
        public RadioButton SharerRb
        {
            get
            {
                return sharerRb;
            }
        }

        public CheckBox NavigationPanelCb
        {
            get
            {
                return navigationPanelCb;
            }
        }

       
        public DataGridView NavigationDgv
        {
            get
            {
                return navigationDgv;
            }
        }

        public ToolStripStatusLabel DistanceLabel
        {
            get
            {
                return distanceLabel;
            }
        }


        #endregion


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
            GuiFacade.InitDrawImage();
        }

        private void MouseClickStarted(MouseEventArgs e)
        {
            if (dragRb.Checked)
            {
                Cursor = Cursors.SizeAll;
                GuiFacade.DragStart(e.Location);
                return;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Cursor = Cursors.SizeAll;
                GuiFacade.DragStart(e.Location);
                return;
            }

            if (analyzeRb.Checked)
            {
                Cursor = Cursors.Cross;
                GuiFacade.GetPointAmplitudeStart(e.Location);
            }
            else
            {
                Cursor = Cursors.Arrow;
                if (markAreaRb.Checked)
                {
                    GuiFacade.SelectAreaStart(e.Location);
                }
                else if (sharerRb.Checked)
                {
                    GuiFacade.ShareCoords(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    GuiFacade.SelectPointStart(e.Location);
                }

                else if (verticalSectionRb.Checked)
                {
                    GuiFacade.VerticalSectionStart(e.Location);
                }
                else if (horizontalSectionRb.Checked)
                {
                    GuiFacade.HorizontalSectionStart(e.Location);
                }
                else if (linearSectionRb.Checked)
                {
                    GuiFacade.LinearSectionStart(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    GuiFacade.RulerStart(e.Location);
                }
            }
        }

        private void MouseMoving(MouseEventArgs e)
        {
            GuiFacade.Drag(e.Location);
            
            if (markAreaRb.Checked)
            {
                GuiFacade.SelectArea(e.Location);
            }
            else if (markPointRb.Checked)
            {
                GuiFacade.SelectPoint(e.Location);
            }
            else if (analyzeRb.Checked)
            {
                GuiFacade.GetPointAmplitude(e.Location);
            }
            else if (verticalSectionRb.Checked)
            {
                GuiFacade.VerticalSection(e.Location);
            }
            else if (horizontalSectionRb.Checked)
            {
                GuiFacade.HorizontalSection(e.Location);
            }
            else if (linearSectionRb.Checked)
            {
                GuiFacade.LinearSection(e.Location);
            }
            else if (rulerRb.Checked)
            {
                GuiFacade.GetDistance(e.Location);
            }

        }

        private void MouseClickFinished(MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            GuiFacade.DragFinish();

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (markAreaRb.Checked)
                {
                    GuiFacade.SelectAreaFinish(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    GuiFacade.SelectPointFinish(e.Location);
                }
                else if (analyzeRb.Checked)
                {
                    GuiFacade.StopPointAnalyzer();
                }
                else if (verticalSectionRb.Checked || horizontalSectionRb.Checked || linearSectionRb.Checked)
                {
                    GuiFacade.StopSection(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    GuiFacade.GetDistance(e.Location);
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
            Text = GuiFacade.OpenFile();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            GuiFacade.DrawImage();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            GuiFacade.DrawImage();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            GuiFacade.InitDrawImage(pictureBox1.Size);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMoving(e);
            coordinatesLabel.Text = GuiFacade.ShowMousePosition(e.Location);
            GuiFacade.ShowNavigation(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseClickStarted(e);
            GuiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MouseClickFinished(e);
            GuiFacade.DrawImage();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            GuiFacade.ScaleImage(e.Delta, e.Location);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Focus();
        }


        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            GuiFacade.ChangeFilterValue(filterTrackBar.Value);
            filterLbl.Text = string.Format("Уровень фильтра: {0}", filterTrackBar.Value);
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                GuiFacade.GetFilter(Behaviors.Filters.FilterType.Contrast, 4);
            }
        }

        private void gammaCorrRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                GuiFacade.GetFilter(Behaviors.Filters.FilterType.GammaCorrection, 0);
            }
        }

        private void brightnessRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                GuiFacade.GetFilter(Behaviors.Filters.FilterType.Brightness, 4);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.ShowSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GuiFacade.CancelLoading();
            GuiFacade.Dispose();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _keyProcessor.ProcessKeyPress(e);
        }

        private void оФайлеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.ShowFileInfo();
        }

        private void логToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GuiFacade.ShowLog();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.Save();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            GuiFacade.InitDrawImage();
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
            Text = GuiFacade.OpenWithDoubleClick();
        }


        private void alignBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.ResampleImage();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            GuiFacade.CancelLoading();
        }

        private void resetFilterBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.ResetFilter();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            GuiFacade.MoveFileDragDrop(e);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            Text = GuiFacade.OpenFileDragDrop(e);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.ShowAbout();
        }

        private void статусКешаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.ShowCache();
        }

        private void findPointBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.ShowFindPoint();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            GuiFacade.InitDrawImage();
        }

        private void rulerRb_CheckedChanged(object sender, EventArgs e)
        {
            GuiFacade.ResetRuler();
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.ScaleImage(-1);
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.ScaleImage(1);
        }

        private void создатьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.ReportDialog();
        }

        private void statisticsBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.GetAreaStatistics();
        }


        private void вшитьНавигациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.EmbedNavigation();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            GuiFacade.DrawItems(e.Graphics);
        }

        private void совместитьФайлыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiFacade.AggregateFiles();
        }

        private void mirrorImageBtn_Click(object sender, EventArgs e)
        {
            GuiFacade.MirrorImage();
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

    }
}
