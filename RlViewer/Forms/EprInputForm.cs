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



        /// <summary>
        /// Pressed keys (0-9). 1st char after empty input are disappearing for some reason,
        /// so now all pressed digit keys are stored while maskedTextbox is focused.
        /// Then maskedTextBox.Text property is set to all pressed keys value
        /// REFACTOR ASAP
        /// </summary>
        string _keys = string.Empty;

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
            
            if (!Single.TryParse(_keys, out _eprValue))
            {
                MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                maskedTextBox1.Focus();
                return;
            }
                        
            DialogResult = DialogResult.OK;
            Close();
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SubmitEprValue();
        }

        private void EprInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && _keys.Length > 0)
            {

                _keys = _keys.Substring(0, _keys.Length - 1);
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                SubmitEprValue();
            }
        }

        private void EprInputForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                maskedTextBox1.Text = _keys;
                _keys += e.KeyChar;
            }
            
        }

    }
}
