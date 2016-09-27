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
            FormsHelper.AddTbClickEvent(panel1.Controls);
            FormsHelper.AddTbClickEvent(panel2.Controls);
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            accuracyTb.Text = "00° 00' 10''";
        }

        private bool _hasNavigation;

        internal Behaviors.Navigation.NavigationSearcher.SearcherParams Params
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
                string x = string.IsNullOrEmpty(xCoordTb.Text) ? "0" : xCoordTb.Text;
                string y = string.IsNullOrEmpty(yCoordTb.Text) ? "0" : yCoordTb.Text;
                Params = new Behaviors.Navigation.NavigationSearcher.SearcherParams(Convert.ToInt32(x), Convert.ToInt32(y), 0, 0, 0, false);
            }
            else if (radioButton2.Checked && _hasNavigation)
            {
                string lat = string.IsNullOrEmpty(latitudeTb.Text) ? "00° 00' 00''" + comboBox1.Text : "0" + latitudeTb.Text + comboBox1.Text;
                string lon = string.IsNullOrEmpty(longtitudeTb.Text) ? "000° 00' 00''" + comboBox2.Text : longtitudeTb.Text + comboBox2.Text;
                var error = string.IsNullOrEmpty(accuracyTb.Text) ? "00° 00' 00''" : accuracyTb.Text;

                Params = new Behaviors.Navigation.NavigationSearcher.SearcherParams(0, 0,
                Navigation.NaviStringConverters.ParseToRadians(lat), Navigation.NaviStringConverters.ParseToRadians(lon),
                Navigation.NaviStringConverters.ParseToRadians(accuracyTb.Text), true);
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
                xCoordTb.Focus();
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ControlSwitch(panel2, true);
                ControlSwitch(panel1, false);
                latitudeTb.Focus();
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
            xCoordTb.Focus();
        }

    }
}
