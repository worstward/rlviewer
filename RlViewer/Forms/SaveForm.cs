﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RlViewer.Behaviors.AreaSelector;

namespace RlViewer.Forms
{
    public partial class SaveForm : Form
    {

        ///TODO: add class to keep save parameters

        public SaveForm(int fileWidth, int fileHeight, AreaSelector selector)
        {

            InitializeComponent();
            x1CoordTextBox.PromptChar = ' ';
            x2CoordTextBox.PromptChar = ' ';
            y1CoordTextBox.PromptChar = ' ';
            y2CoordTextBox.PromptChar = ' ';
            xSizeCoordTextBox.PromptChar = ' ';
            ySizeCoordTextBox.PromptChar = ' ';
            widthTextBox.PromptChar = ' ';
            heightTextBox.PromptChar = ' ';

            _fileWidth = fileWidth;
            _fileHeight = fileHeight;

            _selector = selector;

            if (selector.Area.Height != 0)
            {
                radioButton3.Checked = true;
            }

            InitControls(selector.Area.Location.X, selector.Area.Location.Y, selector.Area.Width, selector.Area.Height);
        }

        private AreaSelector _selector;


        private int _fileWidth;
        private int _fileHeight;

        private bool _keepFiltering;

        public bool KeepFiltering
        {
            get { return _keepFiltering; }
        }

       
        private Point _leftTop;
        public Point LeftTop
        {
            get { return _leftTop; }
        }

        private int _width;
        public int ImageWidth
        {
            get { return _width; }
        }

        private int _heigth;
        public int ImageHeight
        {
            get { return _heigth; }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                InitControls(_selector.Area.Location.X, _selector.Area.Location.Y, _selector.Area.Width, _selector.Area.Height);
                _leftTop = new Point(0, 0);
                _width = _fileWidth;
                _heigth = _fileHeight;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                InitControls(_selector.Area.Location.X, _selector.Area.Location.Y, _selector.Area.Width, _selector.Area.Height);
               
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                InitControls(_selector.Area.Location.X, _selector.Area.Location.Y, _selector.Area.Width, _selector.Area.Height);

            }
        }

        private void InitControls(int x, int y, int width, int height)
        {
            if (radioButton1.Checked)
            {
                ControlSwitch(panel2, false);
                ControlSwitch(panel3, false);
            }
            else if (radioButton2.Checked)
            {
                ControlSwitch(panel2, true);
                ControlSwitch(panel3, false);
            }
            else if(radioButton3.Checked)
            {
                ControlSwitch(panel2, false);
                ControlSwitch(panel3, true);
            }

            width = x < 0 ? x + width : width;
            height = y < 0 ? y + height : height;

            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;
            x = x >_fileWidth ? 0 : x;
            y = y > _fileHeight ? 0 : y;


            x1CoordTextBox.Text = x.ToString();
            y1CoordTextBox.Text = y.ToString();

            var x2 = x + width;
            x2 = x2 == 0 || x2 > _fileWidth  ? _fileWidth : x2;
            var y2 = y + height;
            y2 = y2 == 0 || y2 > _fileHeight ? _fileHeight : y2;

            x2CoordTextBox.Text = (x2 - 1).ToString();
            y2CoordTextBox.Text = (y2 - 1).ToString();

            xSizeCoordTextBox.Text = x.ToString();
            ySizeCoordTextBox.Text = y.ToString();

            var computedWidth = x2 - x;
            var computedHeight = y2 - y;
            widthTextBox.Text = (x2 - x).ToString();
            heightTextBox.Text = (y2 - y).ToString();

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
            if (radioButton2.Checked)
            {
                _width = Convert.ToInt32(x2CoordTextBox.Text) - Convert.ToInt32(x1CoordTextBox.Text) + 1;
                _heigth = Convert.ToInt32(y2CoordTextBox.Text) - Convert.ToInt32(y1CoordTextBox.Text) + 1;

                int x = Convert.ToInt32(x1CoordTextBox.Text);
                int y = Convert.ToInt32(y1CoordTextBox.Text);

               
                if (_width < 0)
                {
                    _width = -_width;
                    x = Convert.ToInt32(x2CoordTextBox.Text);
                }
                if (_heigth < 0)
                {
                    _heigth = -_heigth;
                    y = Convert.ToInt32(y2CoordTextBox.Text);
                }

                _leftTop = new Point(x, y);
                
            }
            else if (radioButton3.Checked)
            {

                var x = Convert.ToInt32(xSizeCoordTextBox.Text);
                var y = Convert.ToInt32(ySizeCoordTextBox.Text);

                _width = Convert.ToInt32(widthTextBox.Text);
                _heigth = Convert.ToInt32(heightTextBox.Text);
                if (_width < 0)
                {
                    _width = -_width;
                    x -= _width;
                }
                if (_heigth < 0)
                {
                    _heigth = -_heigth;
                    y -= _heigth;
                }

                _leftTop = new Point(x, y);
            }


            if (Convert.ToInt32(x1CoordTextBox.Text) < _fileWidth && Convert.ToInt32(x2CoordTextBox.Text) < _fileWidth &&
                   Convert.ToInt32(y1CoordTextBox.Text) < _fileHeight && Convert.ToInt32(y2CoordTextBox.Text) < _fileHeight)
            {
                _keepFiltering = keepFilteringCb.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Selected area is out of bounds", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveSizeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}