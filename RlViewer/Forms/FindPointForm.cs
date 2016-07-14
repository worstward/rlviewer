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
    public partial class FindPointForm : Form
    {
        public FindPointForm(bool hasNavigation)
        {
            InitializeComponent();
            InitControls();
            _hasNavigation = hasNavigation;
        }

        private void InitControls()
        {
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            maskedTextBox1.PromptChar = ' ';
            maskedTextBox2.PromptChar = ' ';
            maskedTextBox3.PromptChar = ' ';
            maskedTextBox4.PromptChar = ' ';
        }

        private bool _hasNavigation;

        public string XLat
        {
            get;
            private set;
        }

        public string YLon
        {
            get;
            private set;
        }
        
        private void ControlSwitch(Control controlContainer, bool enable)
        {
            foreach (Control control in controlContainer.Controls)
            {
                control.Enabled = enable;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                XLat = string.IsNullOrEmpty(maskedTextBox3.Text) ? "0" : maskedTextBox3.Text;
                YLon = string.IsNullOrEmpty(maskedTextBox4.Text) ? "0" : maskedTextBox4.Text;
            }
            else if (radioButton2.Checked && _hasNavigation)
            {
                XLat = string.IsNullOrEmpty(maskedTextBox1.Text) ? "00° 00' 00''" + comboBox1.Text : "0" + maskedTextBox1.Text + comboBox1.Text;
                YLon = string.IsNullOrEmpty(maskedTextBox2.Text) ? "000° 00' 00''" + comboBox2.Text : maskedTextBox2.Text + comboBox2.Text;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ControlSwitch(panel1, true);
                ControlSwitch(panel2, false);
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ControlSwitch(panel2, true);
                ControlSwitch(panel1, false);
            }
        }

        private void FindPointForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void FindPointForm_Shown(object sender, EventArgs e)
        {
            maskedTextBox3.Focus();
        }

    }
}
