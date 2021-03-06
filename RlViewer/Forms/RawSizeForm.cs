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
    public partial class SizeForm : Form
    {
        public SizeForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            FormsHelper.AddTbClickEvent<MaskedTextBox>(this.Controls);
        }

        private int _bytesPerSample;

        public int BytesPerSample
        {
            get { return _bytesPerSample; }
        }

        private System.Drawing.Size _imgSize = new Size();

        public System.Drawing.Size ImgSize
        {
            get 
            {
                return _imgSize;
            }
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
            SubmitFileSize();
        }


        private void SubmitFileSize()
        {
            if (ImgWidth == 0 || ImgHeight == 0)
            {
                MessageBox.Show("Неверный параметр", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        _bytesPerSample = 4;
                        break;
                    case 1:
                        _bytesPerSample = 8;
                        break;
                }

                _imgSize = new Size(ImgWidth, ImgHeight);
                DialogResult = DialogResult.OK;
            }
        }


        private void SizeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                SubmitFileSize();
            }
        }

        private void SizeForm_Shown(object sender, EventArgs e)
        {
            maskedTextBox1.Focus();
        }


    }
}
