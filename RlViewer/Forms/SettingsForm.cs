using System;
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
    public partial class SettingsForm : Form
    {
        public SettingsForm(Settings.Settings settings)
        {
            _settings = settings;
            InitializeComponent();
            comboBox1.SelectedItem = _settings.Palette.Select(x => x.ToString())
                .Aggregate((x, y) => x.ToString() + " " + y.ToString());
            checkBox1.Checked = _settings.IsPaletteReversed;
            allowViewCheckBox.Checked = _settings.AllowViewWhileLoading;
        }

        private Settings.Settings _settings;

        private int[] _palette;
        private bool _isReversed;
        private bool _allowViewWhileLoading;


        private void allowViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allowViewCheckBox.Checked)
            {
                _allowViewWhileLoading = true;
            }
            else _allowViewWhileLoading = false;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _palette = comboBox1.GetItemText(comboBox1.SelectedItem).Split(' ')
                    .Select(x => Convert.ToInt32(x)).ToArray();
            }
            catch (Exception ex)
            {
                _palette = new int[] { 1, 1, 1 };
                Logging.Logger.Log(Logging.SeverityGrades.Warning, 
                    string.Format("Attempt to get palette from settings failed with message {0}", ex.Message));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isReversed = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _settings.AllowViewWhileLoading = _allowViewWhileLoading;
            _settings.Palette = _palette;
            _settings.IsPaletteReversed = _isReversed;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _settings.AllowViewWhileLoading = _allowViewWhileLoading;
            _settings.Palette = _palette;
            _settings.IsPaletteReversed = _isReversed;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
