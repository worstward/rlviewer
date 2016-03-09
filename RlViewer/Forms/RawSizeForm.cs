﻿using System;
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
    public partial class SizeForm : Form
    {
        public SizeForm()
        {
            InitializeComponent();
            maskedTextBox1.PromptChar = ' ';
        }

        private System.Drawing.Size _imgSize = new Size();

        public System.Drawing.Size ImgSize
        {
            get { return _imgSize; }
        }

        private int ImgWidth
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(maskedTextBox1.Text))
                    return Convert.ToInt32(maskedTextBox1.Text);

                return 0;
            }
        }
        private int ImgHeight
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(maskedTextBox2.Text))
                    return Convert.ToInt32(maskedTextBox2.Text);

                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ImgWidth == 0 || ImgHeight == 0)
            {
                MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _imgSize = new Size(ImgWidth, ImgHeight);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void SizeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ImgWidth == 0 || ImgHeight == 0)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}