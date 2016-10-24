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
          
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;

            _keyProcessor = new UI.KeyPressProcessor(() => GuiFacade.Undo(), () => this.Text = GuiFacade.OpenFile(),
                 () => GuiFacade.Save(), () => GuiFacade.ShowFileInfo(), () => GuiFacade.ShowLog(),
                 () => GuiFacade.ReportDialog());
        }

        UI.KeyPressProcessor _keyProcessor;

        UI.GuiFacade _guiFacade;
        public UI.GuiFacade GuiFacade
        {
            get
            {
                return _guiFacade = _guiFacade ?? new UI.GuiFacade(this); 
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

        public HScrollBar Horizontal
        {
            get
            {
                return hScrollBar1;
            }
        }
        public VScrollBar Vertical
        {
            get
            {
                return vScrollBar1;
            }
        }
        public TrackBar FilterTrackBar
        {
            get
            {
                return trackBar1;
            }
        }
        public Label FilterValueLabel
        {
            get
            {
                return filterLbl;
            }
        }

        public ToolStripProgressBar ProgressBar
        {
            get
            {
                return toolStripProgressBar1;
            }
        }
        public ToolStripStatusLabel ProgressLabel 
        {
            get
            {
                return toolStripStatusLabel1;
            }
        }

        public ToolStripStatusLabel StatusLabel
        {
            get
            {
                return toolStripStatusLabel2;
            }
        }

        public Label ScaleLabel
        {
            get
            {
                return scaleLabel;
            }
        }

        public new ToolStripDropDownButton  CancelButton
        {
            get
            {
                return toolStripDropDownButton1;
            }
        }

        public Button AlignBtn
        {
            get
            {
                return alignBtn;
            }
        }

        public Button FindPointBtn
        {
            get
            {
                return findPointBtn;
            }
        }

        public RadioButton BrightnessRb
        {
            get
            {
                return brightnessRb;
            }
        }

        public RadioButton ContrastRb
        {
            get
            {
                return contrastRb;
            }
        }

        public RadioButton GammaRb
        {
            get
            {
                return gammaCorrRb;
            }
        }

        public Button ResetFilter
        {
            get
            {
                return resetFilterBtn;
            }
        }


        public RadioButton DragRb
        {
            get
            {
                return dragRb;
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

        public RadioButton AnalyzePointRb
        {
            get
            {
                return analyzeRb;
            }
        }
        public RadioButton VerticalSectionRb
        {
            get
            {
                return verSection;
            }
        }
        public RadioButton HorizontalSectionRb
        {
            get
            {
                return horSection;
            }
        }

        public RadioButton LinearSectionRb
        {
            get
            {
                return linSectionRb;
            }
        }

        public RadioButton RulerRb
        {
            get
            {
                return rulerRb;
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
                return naviPanelCb;
            }
        }

        public CheckBox FilterPanelCb
        {
            get
            {
                return filterPanelCb;
            }
        }

        public DataGridView NavigationDgv
        {
            get
            {
                return dataGridView1;
            }
        }

        public SplitContainer NaviSplitter
        {
            get
            {
                return splitContainer1;
            }
        }

        public SplitContainer FilterSplitter
        {
            get
            {
                return splitContainer2;
            }
        }

        public ToolStripStatusLabel CoordinatesLabel
        {
            get
            {
                return toolStripStatusLabel4;
            }
        }


        public ToolStripStatusLabel DistanceLabel
        {
            get
            {
                return toolStripStatusLabel5;
            }
        }

        public System.Windows.Forms.DataVisualization.Charting.Chart HistogramChart
        {
            get
            {
                return chart1;
            }
        }

        public Button ZoomInBtn
        {
            get
            {
                return zoomInBtn;
            }
        }

        public Button ZoomOutBtn
        {
            get
            {
                return zoomOutBtn;
            }
        }
        public Button StatisticsBtn
        {
            get
            {
                return statisticsBtn;
            }
        }

        public RadioButton SquareAreaRb 
        {
            get
            {
                return squareAreaRb;
            }
        }


        #endregion

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
            GuiFacade.InitDrawImage();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            GuiFacade.TraceMouseMovement(e);
            GuiFacade.ShowMousePosition(e);
            GuiFacade.ShowNavigation(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            GuiFacade.ClickStarted(e);          
            GuiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            GuiFacade.ClickFinished(e);
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
            GuiFacade.ChangeFilterValue(trackBar1.Value);
            filterLbl.Text = string.Format("Уровень фильтра: {0}", trackBar1.Value);
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
         {
            if(((RadioButton)sender).Checked)
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
            GuiFacade.ToggleNavigation();
        }

        private void filterPanelCb_CheckedChanged(object sender, EventArgs e)
        {
            GuiFacade.ToggleFilters();
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

    }
}
