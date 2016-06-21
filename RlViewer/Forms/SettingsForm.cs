using System;
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
            FillComboBox();

            comboBoxPics1.SelectedItem = comboBoxPics1.Items.OfType<CboItem>()
                .Where(item => item.Text == _settings.Palette.Select(x => x.ToString())
                .Aggregate((x, y) => x.ToString() + " " + y.ToString())).FirstOrDefault();
            inverseCheckBox.Checked = _settings.IsPaletteReversed;
            allowViewCheckBox.Checked = _settings.AllowViewWhileLoading;
            forceTileGenCheckBox.Checked = _settings.ForceTileGeneration;
            tileOutputCb.SelectedIndex = (int)_settings.TileOutputAlgorithm;

            logPaletteCb.Checked = _settings.IsPaletteGroupped;
            sectionSizeTextBox.Text = _settings.SectionSize.ToString();
            sectionSizeTextBox.PromptChar = ' ';
            highResCb.Checked = _settings.HighResForDownScaled;
            compressCoefTb.Text = _settings.CompressionCoef.ToString();
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


        private void FillComboBox()
        {
            comboBoxPics1.Items.Add(new CboItem("1 1 1", Properties.Resources.Grayscale));
            comboBoxPics1.Items.Add(new CboItem("-1 -1 -1", Properties.Resources.Rainbow));
            comboBoxPics1.Items.Add(new CboItem("1 1 0", Properties.Resources.Yellows));
            comboBoxPics1.Items.Add(new CboItem("1 0 1", Properties.Resources.Pinks));
            comboBoxPics1.Items.Add(new CboItem("0 1 1", Properties.Resources.LightBlues));
        }

        private void allowViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _allowViewWhileLoading = ((CheckBox)sender).Checked;
        }

        private void forceTileGenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _forceTileGen = ((CheckBox)sender).Checked;
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

            float compressCoef;
            if (Single.TryParse(compressCoefTb.Text, out compressCoef))
            {
                _settings.CompressionCoef = compressCoef == 0 ? 1 : compressCoef;
            }
            else return;


            if ((_palette[0] == _palette[1]) && (_palette[1] == _palette[2]) && (_palette[0] == -1))
            {
                _settings.UseTemperaturePalette = true;
            }
            else
            {
                _settings.UseTemperaturePalette = false;
            }
            
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

        private void comboBoxPics1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _palette = ((ComboBox)sender).GetItemText(((CboItem)comboBoxPics1.SelectedItem).Text).Split(' ')
                    .Select(x => Convert.ToSingle(x)).ToArray();
            }
            catch (Exception ex)
            {
                _palette = new float[] { 1, 1, 1 };
                Logging.Logger.Log(Logging.SeverityGrades.Warning,
                    string.Format("Attempt to get palette from settings failed with message {0}", ex.Message));
            }
        }

    }

}
