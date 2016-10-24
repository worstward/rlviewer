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
            FormsHelper.AddTbClickEvent<TextBox>(this.Controls);
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
            if (Single.TryParse(textBox1.Text, out _eprValue))
            {
                DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBox1.Focus();
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
            textBox1.Focus();
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }


    }
}
