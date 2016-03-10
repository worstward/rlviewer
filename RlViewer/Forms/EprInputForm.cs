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

        private int eprValue;

        public int EprValue
        {
            get
            {
                //if (!string.IsNullOrWhiteSpace(maskedTextBox1.Text))
                //    return Convert.ToInt32(maskedTextBox1.Text);

                //return 0;
                return eprValue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Int32.TryParse(maskedTextBox1.Text, out eprValue))
            {
                MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

    }
}
