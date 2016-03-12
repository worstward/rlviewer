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
            this.MaximumSize = this.Size;
            dgv = GetDataGrid();
            LoadData();
            dgv.Size = SetDgvSize();
            panel1.Controls.Add(dgv);
        }

        DataGridView dgv;

        private DataGridView GetDataGrid()
        {
            var dgv = new DataGridView()
            {
                BackgroundColor = Color.White,
                Location = new Point(this.Location.X + 5, this.Location.Y),
                RowHeadersVisible = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                Anchor = AnchorStyles.Top,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
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
            //some magic numbers to set proper offsets and make each row appear correctly
            foreach (DataGridViewRow row in dgv.Rows) height += (row.Height + 7);
            height += 30;
            return new Size(this.Width - 10, height);
        }



        private void LoadData()
        {
            foreach (var logEntry in Logging.Logger.Logs)
            {
                dgv.Rows.Add(logEntry.EventTime, logEntry.Severity, logEntry.Message);
            }  
        }


    }
}
