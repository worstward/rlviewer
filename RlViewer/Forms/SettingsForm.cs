﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RlViewer.Settings;

namespace RlViewer.Forms
{
    public partial class  SettingsForm : Form
    {
        public SettingsForm(Settings.Settings settings)
        {
            _settings = settings;

            InitializeComponent();
            comboBox1.SelectedItem = _settings.Palette.Select(x => x.ToString())
                .Aggregate((x, y) => x.ToString() + " " + y.ToString());
            inverseCheckBox.Checked = _settings.IsPaletteReversed;
            allowViewCheckBox.Checked = _settings.AllowViewWhileLoading;
            forceTileGenCheckBox.Checked = _settings.ForceTileGeneration;
            tileOutputCb.SelectedIndex = (int)_settings.TileOutputAlgorithm;

            logPaletteCb.Checked = _settings.IsPaletteGroupped;
            sectionSizeTextBox.Text = _settings.SectionSize.ToString();
            sectionSizeTextBox.PromptChar = ' ';
            highResCb.Checked = _settings.HighResForDownScaled;

            areaSizeTextBox.Text = _settings.SelectorAreaSize.ToString();
            areaSizeTextBox.PromptChar = ' ';
        }

        private Settings.Settings _settings;


        private float[] _palette;
        private bool _isReversed;
        private bool _isGrouped;
        private bool _allowViewWhileLoading;
        private bool _forceTileGen;
        private Behaviors.TileCreator.TileOutputType _outputType;
        private bool _highRes;

        private void allowViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _allowViewWhileLoading = ((CheckBox)sender).Checked;
        }

        private void forceTileGenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _forceTileGen = ((CheckBox)sender).Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _palette = ((ComboBox)sender).GetItemText(comboBox1.SelectedItem).Split(' ')
                    .Select(x => Convert.ToSingle(x)).ToArray();
            }
            catch (Exception ex)
            {
                _palette = new float[] { 1, 1, 1 };
                Logging.Logger.Log(Logging.SeverityGrades.Warning, 
                    string.Format("Attempt to get palette from settings failed with message {0}", ex.Message));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isReversed = ((CheckBox)sender).Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int sectionSize;
            if (Int32.TryParse(sectionSizeTextBox.Text, out sectionSize))
            {
                _settings.SectionSize = sectionSize;
            }
            else return;

            int areaSize;
            if (Int32.TryParse(areaSizeTextBox.Text, out areaSize))
            {
                _settings.SelectorAreaSize = areaSize == 0 ? 1 : areaSize;
            }
            else return;

            
            _settings.AllowViewWhileLoading = _allowViewWhileLoading;
            _settings.Palette = _palette;
            _settings.IsPaletteReversed = _isReversed;
            _settings.ForceTileGeneration = _forceTileGen;
            _settings.IsPaletteGroupped = _isGrouped;
            _settings.TileOutputAlgorithm = _outputType;
            _settings.HighResForDownScaled = _highRes;

            _settings.ToXml();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void logPaletteCb_CheckedChanged(object sender, EventArgs e)
        {
            _isGrouped = ((CheckBox)sender).Checked;
        }

        private void tileOutputCb_SelectedIndexChanged(object sender, EventArgs e)
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
                    break;
            }
        }

        private void highResCb_CheckedChanged(object sender, EventArgs e)
        {
            _highRes = ((CheckBox)sender).Checked;
        }

    }
}
