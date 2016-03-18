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
            maskedTextBox1.PromptChar = ' ';
        }

        private int _eprValue;

        public int EprValue
        {
            get
            {
                //if (!string.IsNullOrWhiteSpace(maskedTextBox1.Text))
                //    return Convert.ToInt32(maskedTextBox1.Text);

                //return 0;
                return _eprValue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Int32.TryParse(maskedTextBox1.Text, out _eprValue))
            {
                MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DialogResult = DialogResult.Cancel;
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void EprInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

    }
}
