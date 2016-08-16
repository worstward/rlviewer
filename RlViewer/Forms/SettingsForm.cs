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
            AddTbClickEvent(tabControl1);


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
            rangeCompressCoefTb.Text = _settings.RangeCompressionCoef.ToString();
            azimuthCompressCoefTb.Text = _settings.AzimuthCompressionCoef.ToString();

            areaSizeTextBox.Text = _settings.SelectorAreaSize.ToString();
            areasOrPointsForAligningCb.Checked = settings.UsePointsForAligning;
            plot3dSizeTb.Text = settings.Plot3dAreaBorderSize.ToString();
            adminReminderCb.Checked = _settings.ForceAdminMode;
            useCustomFileOpenDlgCb.Checked = _settings.UseCustomFileOpenDlg;
            surfaceTypeCb.SelectedIndex = (int)_settings.SurfaceType;

            baseRadiusTb.Text = _settings.RbfMlBaseRaduis.ToString();
            layersNumTb.Text = _settings.RbfMlLayersNumber.ToString();
            regularizationCoefTb.Text = _settings.RbfMlRegularizationCoef.ToString();
        }


        private Settings.Settings _settings;


        private float[] _palette;
        private bool _isReversed;
        private bool _isGrouped;
        private bool _allowViewWhileLoading;
        private bool _forceTileGen;
        private Behaviors.TileCreator.TileOutputType _outputType;
        private bool _highRes;
        private bool _areasOrPointsForAligning;
        private bool _forceAdmin;
        private bool _customFileOpenDlg;
        private Behaviors.ImageAligning.Surfaces.SurfaceType _surfaceType;

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
            ConfirmSettings();
        }

        private void ConfirmSettings()
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

            float rangeCompressCoef;
            if (Single.TryParse(rangeCompressCoefTb.Text, out rangeCompressCoef))
            {
                _settings.RangeCompressionCoef = rangeCompressCoef == 0 ? 1 : rangeCompressCoef;
            }
            else return;

            float azimuthCompressCoef;
            if (Single.TryParse(azimuthCompressCoefTb.Text, out azimuthCompressCoef))
            {
                _settings.AzimuthCompressionCoef = azimuthCompressCoef == 0 ? 1 : azimuthCompressCoef;
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


            int plot3dSize;
            if (Int32.TryParse(plot3dSizeTb.Text, out plot3dSize))
            {
                _settings.Plot3dAreaBorderSize = plot3dSize == 0 ? 10 : plot3dSize;
            }
            else return;


            _settings.AllowViewWhileLoading = _allowViewWhileLoading;
            _settings.Palette = _palette;
            _settings.IsPaletteReversed = _isReversed;
            _settings.ForceTileGeneration = _forceTileGen;
            _settings.IsPaletteGroupped = _isGrouped;
            _settings.TileOutputAlgorithm = _outputType;
            _settings.HighResForDownScaled = _highRes;
            _settings.UsePointsForAligning = _areasOrPointsForAligning;
            _settings.ForceAdminMode = _forceAdmin;
            _settings.UseCustomFileOpenDlg = _customFileOpenDlg;
            _settings.SurfaceType = _surfaceType;
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
            else if (e.KeyCode == Keys.Enter)
            {
                ConfirmSettings();
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
                    throw new NotSupportedException("Tile Output settings");
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
                throw new ArgumentException(string.Format("Palette settings: {0}", ex.Message));
            }
        }

        private void pointsOrAreasForAligningCb_CheckedChanged(object sender, EventArgs e)
        {
            _areasOrPointsForAligning = ((CheckBox)sender).Checked;
        }

        private void AddTbClickEvent(TabControl container)
        {

            foreach (TabPage page in container.TabPages)
            {
                foreach(var control in page.Controls)
                { 
                    if (control.GetType() == typeof(MaskedTextBox))
                    {
                        var tb = ((MaskedTextBox)control);
                        tb.Click += (s, e) => tb.Select(tb.Text.Length, 0);
                    }
                }
            }
           
        }

        private void adminReminderCb_CheckedChanged(object sender, EventArgs e)
        {
            _forceAdmin = ((CheckBox)sender).Checked;
        }

        private void useCustomFileOpenDlgCb_CheckedChanged(object sender, EventArgs e)
        {
            _customFileOpenDlg = ((CheckBox)sender).Checked;
        }


        private void surfaceTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = ((ComboBox)sender);
            switch((string)cb.SelectedItem)
            {
                case @"РБФ NN":
                    _surfaceType = Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionQnn;
                    break;
                case @"РБФ многослойная":
                    _surfaceType = Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionMultiLayered;
                    break;
                case @"Кастомная":
                    _surfaceType = Behaviors.ImageAligning.Surfaces.SurfaceType.Custom;
                    break;
                default:
                    throw new NotSupportedException("SurfaceType settings");
            }


            if (_surfaceType == Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionMultiLayered)
            {
                rbfInterpolationcSettingsGb.Enabled = true;
            }
            else
            {
                rbfInterpolationcSettingsGb.Enabled = false;
            }
            
        }

       

    }

}
