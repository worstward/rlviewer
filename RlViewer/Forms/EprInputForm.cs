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
    public partial class EprInputForm : Form
    {
        public EprInputForm()
        {
            InitializeComponent();
            FormsHelper.AddTbClickEvent(this.Controls);
        }

        private float _eprValue;

        public float EprValue
        {
            get
            {
                return _eprValue;
            }
        }

        private void SubmitEprValue()
        {

            if (!Single.TryParse(maskedTextBox1.Text, out _eprValue))
            {
                MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                maskedTextBox1.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            this.Close();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            SubmitEprValue();
        }

        private void EprInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                SubmitEprValue();
            }
        }

        private void EprInputForm_Shown(object sender, EventArgs e)
        {
            maskedTextBox1.Focus();
        }


    }
}
