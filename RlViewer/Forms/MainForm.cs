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
            _guiFacade = new UI.GuiFacade(this);
            Text = string.Empty;
            checkBox1.Checked = false;
        }


        UI.GuiFacade _guiFacade;

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

        public RadioButton RulerRb
        {
            get
            {
                return rulerRb;
            }
        }
        
        public CheckBox NavigationCb
        {
            get
            {
                return checkBox1;
            }
        }

        public DataGridView NavigationDgv
        {
            get
            {
                return dataGridView1;
            }
        }

        public SplitContainer WorkingAreaSplitter
        {
            get
            {
                return splitContainer1;
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


        #endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            _guiFacade.InitDrawImage();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _guiFacade.TraceMouseMovement(e);
            _guiFacade.ShowMousePosition(e);
            _guiFacade.ShowNavigation(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _guiFacade.ClickStarted(e);
            
            _guiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _guiFacade.ClickFinished(e);
        }


        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _guiFacade.ChangeFilterValue();
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
         {
            if(((RadioButton)sender).Checked)
            {
                _guiFacade.GetFilter("Contrast", 4);
            }
        }

        private void gammaCorrRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _guiFacade.GetFilter("Gamma Correction", 0);
            }
        }

        private void brightnessRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _guiFacade.GetFilter("Brightness", 4);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _guiFacade.CancelLoading();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _guiFacade.ProceedKeyPress(e);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _guiFacade.ToggleNavigation();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = _guiFacade.OpenWithDoubleClick();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Focus();
        }

        private void alignBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.AlignImage();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            Text = string.Empty;
            _guiFacade.CancelLoading();
        }

        private void resetFilterBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ResetFilter();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            _guiFacade.MoveFileDragDrop(e);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
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
            _guiFacade.FindPoint();
        }

    }
}
