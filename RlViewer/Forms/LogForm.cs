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
            
            MaximumSize = Size;
            _dgv = GetDataGrid();
            panel1.Controls.Add(_dgv);
        }

        DataGridView _dgv;

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
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

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
                }
            }

            //dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            return dgv;
        }


        private Size SetDgvSize()
        {
            int height = 0;
            
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                height += row.Height;
            }
            height += _dgv.ColumnHeadersHeight;
            
            return new Size(Width - 10, height);
        }



        private void LoadData(Logging.SeverityGrades grade)
        {
            _dgv.Rows.Clear();

            foreach (var logEntry in Logging.Logger.Logs.Where(x => x.Severity == grade))
            {
                _dgv.Rows.Add(logEntry.EventTime, logEntry.Severity, logEntry.Message);
            }  
        }

        private void LoadData()
        {
            _dgv.Rows.Clear();

            foreach (var logEntry in Logging.Logger.Logs)
            {
                _dgv.Rows.Add(logEntry.EventTime, logEntry.Severity, logEntry.Message);
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
            Logging.SeverityGrades grade;

            if (Enum.TryParse<Logging.SeverityGrades>(comboBox1.SelectedItem.ToString(), out grade))
            {
                LoadData(grade);
            }
            else
            {
                LoadData();
            }
            _dgv.Size = SetDgvSize();
            
        }

        private void LogForm_Shown(object sender, EventArgs e)
        {
            InitComboBox(comboBox1);
            _dgv.Size = SetDgvSize();
        }


    }
}
