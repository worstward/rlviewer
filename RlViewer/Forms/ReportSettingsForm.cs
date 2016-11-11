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
    public partial class ReportSettingsForm : Form
    {
        public ReportSettingsForm(Settings.ReporterSettings reporterSettings)
        {
            InitializeComponent();
            _reporterSettings = reporterSettings;


            reportTypeComboBox.Items.AddRange(Enum.GetNames(typeof(Behaviors.ReportGenerator.Abstract.ReporterTypes)));
            reportTypeComboBox.SelectedIndex = 0;

            if (reporterSettings == null)
            {
                finishAtLastCb.Checked = true;
                areaCb.Checked = true;
                centerCb.Checked = false;
                cornersCb.Checked = true;
                headerInfoCb.Checked = false;
                timeCb.Checked = true;
            }
            else
            {
                firstLineTb.Text = reporterSettings.FirstLineOffset.ToString();
                lastLineTb.Text = reporterSettings.LastLineOffset.ToString();

                finishAtLastCb.Checked = reporterSettings.ReadToEnd;
                areaCb.Checked = reporterSettings.AddArea;
                centerCb.Checked = reporterSettings.AddCenter;
                cornersCb.Checked = reporterSettings.AddCorners;
                headerInfoCb.Checked = reporterSettings.AddParametersTable;
                timeCb.Checked = reporterSettings.AddTimes;
            }
            FormsHelper.AddTbClickEvent<MaskedTextBox>(this.Controls);
        }



        private Settings.ReporterSettings _reporterSettings;

        private void ConfirmSettings()
        {
            int firstLine = 0;
            if (!Int32.TryParse(firstLineTb.Text, out firstLine))
            {
                return;
            }

            int lastLine = 0;
            bool readToEnd = false;

            if (finishAtLastCb.Checked)
            {
                readToEnd = true;
            }
            else if (!Int32.TryParse(lastLineTb.Text, out lastLine))
            {
                return;
            }


            //if (!readToEnd && firstLine >= lastLine)
            //{
            //    Forms.FormsHelper.ShowErrorMsg("Номер последней строки меньше или равен номеру первой");
            //    return;
            //}
            lastLine = lastLine == 0 ? 1 : lastLine;

            _reporterSettings.FirstLineOffset = firstLine;
            _reporterSettings.LastLineOffset = lastLine;
            _reporterSettings.ReadToEnd = readToEnd;
            _reporterSettings.AddArea = areaCb.Checked;
            _reporterSettings.AddCenter = centerCb.Checked;
            _reporterSettings.AddCorners = cornersCb.Checked;
            _reporterSettings.AddParametersTable = headerInfoCb.Checked;
            _reporterSettings.AddTimes = timeCb.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


        private void createReportBtn_Click(object sender, EventArgs e)
        {
            ConfirmSettings();
        }

        private void cancelReportBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }


        private void finishAtLastCb_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (CheckBox)sender;
            lastLineTb.Visible = !(cb.Checked);
            label3.Visible = !(cb.Checked);
        }

        private void ReportSettingsForm_KeyDown(object sender, KeyEventArgs e)
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
    }
}
