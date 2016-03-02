using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RlViewer
{
    public partial class MainForm : Form, RlViewer.GuiFacade.ISuitableForm
    {

        public MainForm()
        {
            InitializeComponent();
            guiFacade = new GuiFacade.GuiFacade(this);
            brightnessRb.Checked = true;
            comboBox1.SelectedIndex = 0;
            this.Text = string.Empty;
        }

        GuiFacade.GuiFacade guiFacade;

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
        public TrackBar TrackBar
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

        public ComboBox PaletteComboBox
        {
            get
            {
                return comboBox1;
            }
        }

        public CheckBox ReverseCheckBox
        {
            get
            {
                return checkBox1;
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDlg = new OpenFileDialog() { Filter = Resources.Filter })
            {
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    this.Text = guiFacade.OpenFile(openFileDlg.FileName);
                }
                else
                {
                    this.Text = string.Empty;
                    return;
                }
            }
            guiFacade.LoadFile();
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
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            guiFacade.ClickStarted(e, markRb.Checked);
            guiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            guiFacade.ClickFinished();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            guiFacade.ChangeFilterValue();
            guiFacade.DrawImage();
            filterLbl.Text = string.Format("Filter value: {0}", trackBar1.Value);
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
         {
            if (contrastRb.Checked)
            {
                guiFacade.GetFilter("Contrast", 4);
                filterLbl.Text = string.Format("Filter value: {0}", trackBar1.Value);
            }
        }

        private void gammaCorrRb_CheckedChanged(object sender, EventArgs e)
        {
            if (gammaCorrRb.Checked)
            {
                guiFacade.GetFilter("Gamma Correction", 0);
                filterLbl.Text = string.Format("Filter value: {0}", trackBar1.Value);
            }
        }

        private void brightnessRb_CheckedChanged(object sender, EventArgs e)
        {
            if (brightnessRb.Checked)
            {
                guiFacade.GetFilter("Brightness", 4);
                filterLbl.Text = string.Format("Filter value: {0}", trackBar1.Value);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            guiFacade.ChangePalette();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            guiFacade.ChangePalette();
        }

        private void infoBtn_Click(object sender, EventArgs e)
        {
            guiFacade.ShowFileInfo();
        }



    }
}
