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
    public partial class SaveSizeForm : Form
    {
        public SaveSizeForm(int fileWidth, int fileHeight, AreaSelector selector)
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
            radioButton1.Checked = true;

            InitControls(selector);
        }

        AreaSelector _selector;
        private int _fileWidth;
        private int _fileHeight;


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
                InitControls(_selector);
                _leftTop = new Point(0, 0);
                _width = _fileWidth;
                _heigth = _fileHeight;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                InitControls(_selector);
               
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                InitControls(_selector);

            }
        }

        private void InitControls(AreaSelector selector)
        {
            if (radioButton1.Checked)
            {
                ControlEnabler(panel2, false);
                ControlEnabler(panel3, false);
            }
            else if (radioButton2.Checked)
            {
                ControlEnabler(panel2, true);
                ControlEnabler(panel3, false);
            }
            else if(radioButton3.Checked)
            {
                ControlEnabler(panel2, false);
                ControlEnabler(panel3, true);
            }

            x1CoordTextBox.Text = selector.Area.Location.X.ToString();
            y1CoordTextBox.Text = selector.Area.Location.Y.ToString();

            var x2 = selector.Area.Location.X + selector.Area.Width;
            x2 = x2 < _fileWidth ? x2 : _fileWidth;
            var y2 = selector.Area.Location.Y + selector.Area.Height;
            y2 = y2 < _fileHeight ? y2 : _fileHeight;

            x2CoordTextBox.Text = x2.ToString();
            y2CoordTextBox.Text = y2.ToString();

            xSizeCoordTextBox.Text = selector.Area.Location.X.ToString();
            ySizeCoordTextBox.Text = selector.Area.Location.Y.ToString();
            widthTextBox.Text = selector.Area.Width.ToString();
            heightTextBox.Text = selector.Area.Height.ToString();


        }

        private void ControlEnabler(Panel p, bool enable)
        {
            foreach (Control c in p.Controls)
            {
                c.Enabled = enable;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                _width = Convert.ToInt32(x2CoordTextBox.Text) - Convert.ToInt32(x1CoordTextBox.Text);
                _heigth = Convert.ToInt32(y2CoordTextBox.Text) - Convert.ToInt32(y1CoordTextBox.Text);
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


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}