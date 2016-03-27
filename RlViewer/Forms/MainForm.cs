﻿using System;
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
            this.DoubleBuffered = true;
            InitializeComponent();
            guiFacade = new UI.GuiFacade(this);
            Text = string.Empty;
            checkBox1.Checked = false;
        }


        UI.GuiFacade guiFacade;

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
        public ProgressBar ProgressBar
        {
            get
            {
                return progressBar1;
            }
        }
        public Label ProgressLabel 
        {
            get
            {
                return percentageLabel;
            }
        }

        public Label StatusLabel
        {
            get
            {
                return statusLabel;
            }
        }

        public new Button CancelButton
        {
            get
            {
                return loadCancelBtn;
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

        #endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Text = guiFacade.OpenFile();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            guiFacade.DrawImage();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            guiFacade.DrawImage();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            guiFacade.InitDrawImage();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            guiFacade.TraceMouseMovement(e);
            mouseCoordLabel.Text = guiFacade.ShowMousePosition(e);
            guiFacade.ShowNavigation(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            guiFacade.ClickStarted(e);
            
            guiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            guiFacade.ClickFinished(e);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            guiFacade.ChangeFilterValue();
            filterLbl.Text = string.Format("Уровень фильтра: {0}", trackBar1.Value);
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
         {
            if(((RadioButton)sender).Checked)
            {
                guiFacade.GetFilter("Contrast", 4);
                filterLbl.Text = string.Format("Уровень фильтра: {0}", trackBar1.Value);
            }
        }

        private void gammaCorrRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                guiFacade.GetFilter("Gamma Correction", 0);
                filterLbl.Text = string.Format("Уровень фильтра: {0}", trackBar1.Value);
            }
        }

        private void brightnessRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                guiFacade.GetFilter("Brightness", 4);
                filterLbl.Text = string.Format("Уровень фильтра: {0}", trackBar1.Value);
            }
        }

        private void loadCancelBtn_Click(object sender, EventArgs e)
        {
            Text = string.Empty;
            guiFacade.CancelLoading();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guiFacade.ShowSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            guiFacade.CancelLoading();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            guiFacade.ProceedKeyPress(e);
        }

        private void оФайлеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guiFacade.ShowFileInfo();
        }

        private void логToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guiFacade.ShowLog();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guiFacade.Save();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            guiFacade.InitDrawImage();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            guiFacade.ToggleNavigation();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            guiFacade.OpenWithDoubleClick();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Focus();
        }


    }
}
