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
    public partial class SettingsForm : Form
    {
        public SettingsForm(Settings.AppSettings settings, Settings.GuiSettings guiSettings)
        {
            _settings = settings;
            _guiSettings = guiSettings;

            InitializeComponent();
            FillComboBox();
            Forms.FormsHelper.AddTbClickEvent<MaskedTextBox>(tabControl1);


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
            areasOrPointsForAligningCb.Checked = settings.UseAreasForAligning;
            adminReminderCb.Checked = _settings.ForceAdminMode;
            surfaceTypeCb.SelectedIndex = (int)_settings.SurfaceType;
            baseRadiusTb.Text = _settings.RbfMlBaseRaduis.ToString();
            layersNumTb.Text = _settings.RbfMlLayersNumber.ToString();
            regularizationCoefTb.Text = _settings.RbfMlRegularizationCoef.ToString();
            forceImageHeightAdjustingCb.Checked = _settings.ForceImageHeightAdjusting;
            deleteOnCancelCb.Checked = _settings.DeleteSynthesizedFileOnCalcel;
            serverSarPathTb.Text = _settings.ServerSarPath;
            forceSynthesisCb.Checked = _settings.ForceSynthesis;
            useEmbeddedServerSarCb.Checked = _settings.UseEmbeddedServerSar;

            useCustomFileOpenDlgCb.Checked = _guiSettings.UseCustomFileOpenDlg;
            showSynthesisCommonTabCb.Checked = _guiSettings.ShowRhgSynthesisHeaderParamsTab;
            showServerSarCb.Checked = _guiSettings.ShowServerSar;
        }


        private Settings.AppSettings _settings;
        private Settings.GuiSettings _guiSettings;

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
        private bool _forceImageHeightAdjusting;
        private bool _showSynthesisCommonTab;
        private bool _showServerSar;
        private bool _deleteSynthOnCancel;
        private bool _forceSynthesis;
        private bool _useEmbeddedServerSar;

        private void FillComboBox()
        {
            comboBoxPics1.Items.Add(new CboItem("1 1 1", Properties.Resources.Grayscale));
            comboBoxPics1.Items.Add(new CboItem("-1 -1 -1", Properties.Resources.Rainbow));
            comboBoxPics1.Items.Add(new CboItem("1 1 0", Properties.Resources.Yellows));
            comboBoxPics1.Items.Add(new CboItem("5 1 2", Properties.Resources.Pinks));
            comboBoxPics1.Items.Add(new CboItem("1 2 5", Properties.Resources.LightBlues));
            comboBoxPics1.Items.Add(new CboItem("2 5 1", Properties.Resources.AcidGreen));
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

            int rbfMlBaseRadius;
            if (Int32.TryParse(baseRadiusTb.Text, out rbfMlBaseRadius))
            {
                _settings.RbfMlBaseRaduis = rbfMlBaseRadius == 0 ? 100 : rbfMlBaseRadius;
            }
            else return;

            int rbfMlLayersNumber;
            if (Int32.TryParse(layersNumTb.Text, out rbfMlLayersNumber))
            {
                _settings.RbfMlLayersNumber = rbfMlLayersNumber == 0 ? 3 : rbfMlLayersNumber;
            }
            else return;

            double rbfMlRegularizationCoef;
            if (Double.TryParse(regularizationCoefTb.Text, out rbfMlRegularizationCoef))
            {
                _settings.RbfMlRegularizationCoef = rbfMlRegularizationCoef == 0 ? 0.01 : rbfMlRegularizationCoef;
            }
            else return;

            _settings.AllowViewWhileLoading = _allowViewWhileLoading;
            _settings.Palette = _palette;
            _settings.IsPaletteReversed = _isReversed;
            _settings.ForceTileGeneration = _forceTileGen;
            _settings.IsPaletteGroupped = _isGrouped;
            _settings.TileOutputAlgorithm = _outputType;
            _settings.HighResForDownScaled = _highRes;
            _settings.UseAreasForAligning = _areasOrPointsForAligning;
            _settings.ForceAdminMode = _forceAdmin;
            _settings.SurfaceType = _surfaceType;
            _settings.ForceImageHeightAdjusting = _forceImageHeightAdjusting;
            _settings.DeleteSynthesizedFileOnCalcel = _deleteSynthOnCancel;
            _settings.ServerSarPath = serverSarPathTb.Text;
            _settings.ForceSynthesis = _forceSynthesis;
            _settings.UseEmbeddedServerSar = _useEmbeddedServerSar;

            _guiSettings.UseCustomFileOpenDlg = _customFileOpenDlg;
            _guiSettings.ShowRhgSynthesisHeaderParamsTab = _showSynthesisCommonTab;
            _guiSettings.ShowServerSar = _showServerSar;

            _settings.ToXml<Settings.AppSettings>();
            _guiSettings.ToXml<Settings.GuiSettings>();


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
                case @"РБФ NN коэф":
                    _surfaceType = Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionQnnCoef;
                    break;
                case @"РБФ многослойная коэф":
                    _surfaceType = Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionMultiLayeredCoef;
                    break;
                default:
                    throw new NotSupportedException("SurfaceType settings");
            }


            if (_surfaceType == Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionMultiLayered ||
                _surfaceType == Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunctionMultiLayeredCoef)
            {
                rbfInterpolationcSettingsGb.Enabled = true;
            }
            else
            {
                rbfInterpolationcSettingsGb.Enabled = false;
            }

        }

        private void forceImageHeightAdjustingCb_CheckedChanged(object sender, EventArgs e)
        {
            _forceImageHeightAdjusting = ((CheckBox)sender).Checked;
        }

        private void showSynthesisCommonTabCb_CheckedChanged(object sender, EventArgs e)
        {
            _showSynthesisCommonTab = ((CheckBox)sender).Checked;
        }

        private void showServerSarCb_CheckedChanged(object sender, EventArgs e)
        {
            _showServerSar = ((CheckBox)sender).Checked;
        }

        private void deleteOnCancelCb_CheckedChanged(object sender, EventArgs e)
        {
            _deleteSynthOnCancel = ((CheckBox)sender).Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Исполняемые файлы(.exe)|*.exe";
                if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    serverSarPathTb.Text = ofd.FileName;
                }
            }
        }

        private void forceSynthesisCb_CheckedChanged(object sender, EventArgs e)
        {
            _forceSynthesis = ((CheckBox)sender).Checked;
        }

        private void useEmbeddedServerSarCb_CheckedChanged(object sender, EventArgs e)
        {
            var cbChecked = ((CheckBox)sender).Checked;
            _useEmbeddedServerSar = cbChecked;
            serverSarPathAreaGb.Visible = !cbChecked;
        }
       

    }

}
