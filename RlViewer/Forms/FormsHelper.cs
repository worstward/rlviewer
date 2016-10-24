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

        /// <summary>
        /// Adds textbox click event to provide user-friendly performance
        /// </summary>
        /// <param name="controls">Controls container</param>
        public static void AddTbClickEvent<T>(Control.ControlCollection controls) where T : TextBoxBase
        {
            AddClickEvent<T>(controls, (t) =>
                    {
                        if (t.SelectionStart > t.Text.Length)
                        {
                            t.Select(t.Text.Length, 0);
                        }
                        else t.Select(t.SelectionStart, t.SelectionLength);
                    });
        }

        /// <summary>
        /// Adds click event to each control in the container
        /// </summary>
        /// <typeparam name="T">Control type</typeparam>
        /// <param name="controls">Control container</param>
        /// <param name="callback">Delegate to call when event fires</param>
        public static void AddClickEvent<T>(Control.ControlCollection controls, Action<T> callback) where T : Control
        {
            foreach (var control in controls)
            {
                if (control.GetType() == typeof(MaskedTextBox))
                {
                    var genericControl = ((T)control);
                    genericControl.Click += (s, e) => callback(genericControl);
                }

                var controlAsContainer = ((Control)control).Controls;

                if (controlAsContainer.Count != 0)
                {
                    AddClickEvent<T>(controlAsContainer, callback);
                }
            }
            
        }

    }
}
