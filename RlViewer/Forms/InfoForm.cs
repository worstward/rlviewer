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
    public partial class InfoForm : Form
    {
        public InfoForm(params HeaderInfoOutput[] headers)
        {
            InitializeComponent();
            InitializeTabs(headers);             
        }


        /// <summary>
        /// Adds tabs to TabControl according to number of input headers
        /// </summary>
        private void InitializeTabs(HeaderInfoOutput[] headers)
        {
            if (headers != null)
            {
                for (int i = 0; i < headers.Length; i++)
                {
                    var dgv = GetDataGrid();

                    infoTabsControl.TabPages.Add(headers[i].HeaderName);
                    infoTabsControl.TabPages[i].Controls.Add(dgv);
                    ShowInfo(headers[i].Params, dgv);
                }
            }
        }

        private DataGridView GetDataGrid()
        {
            var dgv = new DataGridView()
            {
                Size = infoTabsControl.Size,
                Location = this.Location,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                
            };

            dgv.Columns.AddRange(new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn() { HeaderText = "Параметр", Name = "paramColumn", ReadOnly = true },
                    new DataGridViewTextBoxColumn() { HeaderText = "Значение", Name = "valueColumn", ReadOnly = true }
                });

            dgv.SelectionChanged += (s, e) => dgv.ClearSelection();

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv.Columns[i].Width = dgv.Width / dgv.Columns.Count;
            }
            return dgv;
        }

        private void ShowInfo(IEnumerable<Tuple<string, string>> hInfo, DataGridView dgv)
        {
            foreach (var pair in hInfo)
            {
                dgv.Rows.Add(pair.Item1, pair.Item2);  
            }  
        }

        private void InfoForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
