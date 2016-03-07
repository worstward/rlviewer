using System;
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
    public partial class SettingsForm : Form
    {
        public SettingsForm(Settings.Settings settings, GuiFacade.GuiFacade guiFacade)
        {
            _settings = settings;
            _guiFacade = guiFacade;
            InitializeComponent();
            comboBox1.SelectedItem = _settings.Palette.Select(x => x.ToString())
                .Aggregate((x, y) => x.ToString() + " " + y.ToString());
            checkBox1.Checked = _settings.IsPaletteReversed;
            allowViewCheckBox.Checked = _settings.AllowViewWhileLoading;
        }

        private Settings.Settings _settings;
        private GuiFacade.GuiFacade _guiFacade;

        private void allowViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allowViewCheckBox.Checked)
            {
                _settings.AllowViewWhileLoading = true;
            }
            else _settings.AllowViewWhileLoading = false;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _settings.Palette = comboBox1.GetItemText(comboBox1.SelectedItem).Split(' ')
                    .Select(x => Convert.ToInt32(x)).ToArray();
            }
            catch (Exception ex)
            {
                _settings.Palette = new int[] { 1, 1, 1 };
            }
            _guiFacade.ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _settings.IsPaletteReversed = checkBox1.Checked;
            _guiFacade.ChangePalette(_settings.Palette, _settings.IsPaletteReversed);
        }
    }
}
