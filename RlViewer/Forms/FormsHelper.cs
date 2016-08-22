using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RlViewer.Forms
{
    public static class FormsHelper
    {

        /// <summary>
        /// Initializes 2 column datagrid
        /// </summary>
        /// <param name="f">Container form</param>
        /// <returns></returns>
        public static DataGridView GetDataGrid(Form f, string firstColumnCaption, string secondColumnCaption)
        {
            var dgv = new DataGridView()
            {
                Size = new Size(f.Width - 10, f.Height - 10),
                Location = f.Location,
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
                    new DataGridViewTextBoxColumn() { HeaderText = firstColumnCaption, Name = "paramColumn", ReadOnly = true },
                    new DataGridViewTextBoxColumn() { HeaderText = secondColumnCaption, Name = "valueColumn", ReadOnly = true }
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

        /// <summary>
        /// Adds tooltip to control
        /// </summary>
        /// <param name="c">Target control</param>
        /// <param name="caption">Caption text</param>
        public static void AddToolTip(Control c, string caption)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(c, caption);
        }

        /// <summary>
        /// Shows messagebox with provided error text
        /// </summary>
        /// <param name="message"></param>
        public static void ShowErrorMsg(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
