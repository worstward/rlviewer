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
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
            _dgv = GetDataGrid();
            panel1.Controls.Add(_dgv);

            vScrollBar1.SmallChange = _dgv.RowTemplate.Height;
            vScrollBar1.LargeChange = _dgv.Height - _dgv.ColumnHeadersHeight - _dgv.RowTemplate.Height * 4;

            InitComboBox(comboBox1);
        }


        private DataGridView _dgv;
        private bool _autoScroll;

        private int PotentiallyVisibleRows
        {
            get
            {
                return (int)Math.Ceiling((_dgv.Height - _dgv.ColumnHeadersHeight) / (double)_dgv.RowTemplate.Height) - 4;
            }
        }



        private DataGridView GetDataGrid()
        {
            var dgv = new DataGridView()
            {
                BackgroundColor = Color.White,
                Location = new Point(Location.X + 5, Location.Y),
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top,
                ScrollBars = ScrollBars.None,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                Height = panel1.Height,
                Width = panel1.Width
            };

            dgv.Columns.AddRange(new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn() { HeaderText = "Время", Name = "timeColumn", ReadOnly = true },
                    new DataGridViewTextBoxColumn() { HeaderText = "Тип", Name = "severityColumn", ReadOnly = true },
                    new DataGridViewTextBoxColumn() { HeaderText = "Сообщение", Name = "messageColumn", ReadOnly = true }
                });

            dgv.SelectionChanged += (s, e) => dgv.ClearSelection();

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                if (i == dgv.Columns.Count - 1)
                {
                    dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[i].Width = 250;
                }
            }

            return dgv;
        }

        /// <summary>
        /// Splits message string to fit into dgv cell
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private string[] GetFormattedEntry(string entry, int acceptedMsgSize)
        {
            List<string> entryParts = new List<string>();

            if (string.IsNullOrEmpty(entry))
            {
                return new string[1];
            }

            if (entry.Length < acceptedMsgSize)
            {
                return new string[] { entry };
            }
            else
            {
                int lastSlashIndex = entry.Substring(0, acceptedMsgSize).LastIndexOf('\\');
                if (lastSlashIndex <= 0)
                {
                    return new string[] { entry };
                }

                entryParts.Add(entry.Substring(0, lastSlashIndex));
                entryParts.Add(Environment.NewLine);
                entryParts.AddRange(GetFormattedEntry(entry.Substring(lastSlashIndex, entry.Length - lastSlashIndex), acceptedMsgSize));
            }

            return entryParts.ToArray();
        }


        private void LoadData(IEnumerable<Logging.LogEntry> entries)
        {
            _dgv.Rows.Clear();

            foreach (var entry in entries)
            {
                var msg = GetFormattedEntry(entry.Message, _dgv.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).Width / 5);
                _dgv.Rows.Add(entry.EventTime, entry.Severity, msg.Aggregate((x, y) => x + y));
            }
        }

        private void LogForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void InitComboBox(ComboBox cb)
        {
            var enumVals = Enum.GetValues(typeof(Logging.SeverityGrades));
            cb.Items.Add("All");
            foreach (var item in enumVals)
            {
                cb.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVisibleEntries(comboBox1.SelectedItem.ToString());
        }

        private int GetMaxDgvHeight(int entries)
        {
            var maximum = entries * _dgv.RowTemplate.Height;
            return maximum;
        }

        private void LogForm_Shown(object sender, EventArgs e)
        {
            vScrollBar1.Maximum = GetMaxDgvHeight(Logging.Logger.Logs.Count);
        }

        private void UpdateVisibleEntries(string severityGradeString)
        {
            Logging.SeverityGrades grade;
            IEnumerable<Logging.LogEntry> dataEntries = null;

            if (_autoScroll)
            {
                var newValue = vScrollBar1.Maximum - vScrollBar1.LargeChange;
                newValue = newValue < 0 ? 0 : newValue;
                vScrollBar1.Value = newValue;
            }


            if (Enum.TryParse<Logging.SeverityGrades>(severityGradeString, out grade))
            {
                dataEntries = Logging.Logger.Logs.Where(x => x.Severity == grade);
            }
            else
            {
                dataEntries = Logging.Logger.Logs.Where(x => x.Severity != Logging.SeverityGrades.Internal && x.Severity != Logging.SeverityGrades.Synthesis);
            }

            var entriesCount = dataEntries.Count();
            var visibleData = GetVisibleData(dataEntries, entriesCount);
            LoadData(visibleData);

            vScrollBar1.Maximum = GetMaxDgvHeight(entriesCount);
        }


        private IEnumerable<Logging.LogEntry> GetVisibleData(IEnumerable<Logging.LogEntry> dataEntries, int dataEntriesCount)
        {

            var position = vScrollBar1.Value / (float)vScrollBar1.Maximum * dataEntriesCount;
            position = position > dataEntriesCount - PotentiallyVisibleRows ? dataEntriesCount - PotentiallyVisibleRows : position;

            var visibleData = dataEntries.Skip((int)position)
               .Take(PotentiallyVisibleRows);

            return visibleData;
        }


        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateVisibleEntries(comboBox1.SelectedItem.ToString());
        }


        private void AutoScrollUpdateUi(object sender, EventArgs args)
        {
            BeginInvoke((Action)(() => UpdateVisibleEntries(comboBox1.SelectedItem.ToString())));
        }

        private void enableAutoScrollCb_CheckedChanged(object sender, EventArgs e)
        {
            _autoScroll = ((CheckBox)sender).Checked;

            if (_autoScroll)
            {
                Logging.Logger.LogEntryAdded += AutoScrollUpdateUi;
            }
            else
            {
                Logging.Logger.LogEntryAdded -= AutoScrollUpdateUi;
            }
        }


        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logging.Logger.LogEntryAdded -= AutoScrollUpdateUi;
        }


    }
}
