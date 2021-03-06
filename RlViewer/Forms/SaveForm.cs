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
        public SaveForm(string savePath, int fileWidth, int fileHeight, AreaSelector selector, Behaviors.TileCreator.TileOutputType outputType,
            Behaviors.Filters.ImageFilterProxy imageFilter = null, System.Drawing.Imaging.ColorPalette palette = null)
        {

            InitializeComponent();

            _savePath = savePath;
            _fileWidth = fileWidth;
            _fileHeight = fileHeight;
            _imageFilter = imageFilter;
            _palette = palette;

            _selector = selector;

            if (selector.Area.Height != 0)
            {
                radioButton3.Checked = true;
            }

            _outputType = outputType;
            outputTypeCb.SelectedIndex = (int)outputType;

            FormsHelper.AddTbClickEvent<MaskedTextBox>(this.Controls);
            InitControls(selector.Area.Location.X, selector.Area.Location.Y, selector.Area.Width, selector.Area.Height);
        }

        private string _savePath;
        private AreaSelector _selector;
        private int _fileWidth;
        private int _fileHeight;
        private bool _keepFiltering;
        private bool _keepPalette;   
        private Point _leftTop;
        private int _width;       
        private int _height;
        private Behaviors.TileCreator.TileOutputType _outputType;
        private Behaviors.Filters.ImageFilterProxy _imageFilter;
        private System.Drawing.Imaging.ColorPalette _palette;


        private Behaviors.Saving.SaverParams _saverParams;

        public Behaviors.Saving.SaverParams SaverParams
        {
            get { return _saverParams; }
        }




        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                InitControls(_selector.Area.Location.X, _selector.Area.Location.Y, _selector.Area.Width, _selector.Area.Height);
                _leftTop = new Point(0, 0);
                _width = _fileWidth;
                _height = _fileHeight;
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
                ControlSwitch(coordPanel, false);
                ControlSwitch(sizePanel, false);
            }
            else if (radioButton2.Checked)
            {
                ControlSwitch(coordPanel, true);
                ControlSwitch(sizePanel, false);
            }
            else if(radioButton3.Checked)
            {
                ControlSwitch(coordPanel, false);
                ControlSwitch(sizePanel, true);
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
                _height = Convert.ToInt32(y2CoordTextBox.Text) - Convert.ToInt32(y1CoordTextBox.Text) + 1;

                int x = Convert.ToInt32(x1CoordTextBox.Text);
                int y = Convert.ToInt32(y1CoordTextBox.Text);

               
                if (_width < 0)
                {
                    _width = -_width;
                    x = Convert.ToInt32(x2CoordTextBox.Text);
                }
                if (_height < 0)
                {
                    _height = -_height;
                    y = Convert.ToInt32(y2CoordTextBox.Text);
                }

                _leftTop = new Point(x, y);
                
            }
            else if (radioButton3.Checked)
            {

                var x = Convert.ToInt32(xSizeCoordTextBox.Text);
                var y = Convert.ToInt32(ySizeCoordTextBox.Text);

                _width = Convert.ToInt32(widthTextBox.Text);
                _height = Convert.ToInt32(heightTextBox.Text);
                if (_width < 0)
                {
                    _width = -_width;
                    x -= _width;
                }
                if (_height < 0)
                {
                    _height = -_height;
                    y -= _height;
                }

                _leftTop = new Point(x, y);

                if (_leftTop.X + _width > _fileWidth)
                {
                    _width = _fileWidth - _leftTop.X;
                }

                if (_leftTop.Y + _height > _fileHeight)
                {
                    _height = _fileHeight - _leftTop.Y;
                }
            }


            if (Convert.ToInt32(x1CoordTextBox.Text) < _fileWidth && Convert.ToInt32(x2CoordTextBox.Text) < _fileWidth &&
                   Convert.ToInt32(y1CoordTextBox.Text) < _fileHeight && Convert.ToInt32(y2CoordTextBox.Text) < _fileHeight)
            {
                _keepFiltering = keepFilteringCb.Checked;
                _keepPalette = keepPaletteCb.Checked;
                var imageFilter = _keepFiltering ? _imageFilter : null;
                var palette = _keepPalette ? _palette : null;

                _saverParams = new Behaviors.Saving.SaverParams(_savePath, _leftTop, _width, _height, _outputType, imageFilter, palette);
                 
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

        private void outputTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).Text)
            {
                case "Линейный":
                    _outputType = Behaviors.TileCreator.TileOutputType.Linear;
                    break;
                case "Логарифмический":
                    _outputType = Behaviors.TileCreator.TileOutputType.Logarithmic;
                    break;
                case "Линейно-Логарифмический":
                    _outputType = Behaviors.TileCreator.TileOutputType.LinearLogarithmic;
                    break;
                default:
                    throw new NotSupportedException("Tile Output settings");
            }
        }

        private void SaveForm_Shown(object sender, EventArgs e)
        {
            var type = (FileType)Enum.Parse(typeof(FileType), System.IO.Path.GetExtension(_savePath).ToLowerInvariant().TrimStart('.'));

            if (type != FileType.bmp)
            {
                this.Height -= groupBox2.Height;
                button1.Top -= groupBox2.Height;
                button2.Top -= groupBox2.Height;
                groupBox2.Visible = false;
            }
        }

    }
}
