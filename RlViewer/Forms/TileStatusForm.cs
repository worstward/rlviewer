﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RlViewer.Forms
{
    public partial class TileStatusForm : Form
    {
        public TileStatusForm(string tileDirectory)
        {
            InitializeComponent();
            InitDataGrid();

            _tileDir = tileDirectory;

            FillDataGrid(_tileDir);
           
        }

        private DataGridViewRow _selectedRow;
        private string _tileDir;

        private void InitDataGrid()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            dataGridView1.SelectionChanged += (s, e) =>
                {
                    if(dataGridView1.SelectedRows.Count != 0)
                    { 
                        _selectedRow = dataGridView1.SelectedRows[0];
                    }
                };

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewTextBoxColumn column in dataGridView1.Columns)
            {
                column.Width = dataGridView1.Width / 3 - 1;
            }
        }

        private void FillDataGrid(string tileDir)
        {

            foreach (var fileNameDirectory in Directory.GetDirectories(tileDir))
            {
                foreach (var extensionDirectory in Directory.GetDirectories(fileNameDirectory))
                {
                    foreach (var imgDirectory in Directory.GetDirectories(extensionDirectory))
                    {
                        DateTime creationTime;
                        try
                        {
                            creationTime  = DateTime.FromFileTime(Convert.ToInt64(Path.GetFileName(imgDirectory)));
                        }
                        catch
                        {
                            creationTime = default(DateTime);
                        }

                        dataGridView1.Rows.Add(Path.Combine(Path.GetFileName(fileNameDirectory),
                            Path.GetFileName(extensionDirectory)),
                            creationTime,
                            Directory.GetFiles(imgDirectory).Length);
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(_selectedRow != null)
            { 
                var deletionPath = Path.Combine(_tileDir, _selectedRow.Cells[0].Value.ToString(),
                    ((DateTime)_selectedRow.Cells[1].Value).ToFileTime().ToString());

                try
                {
                    Directory.Delete(deletionPath, true);
                    dataGridView1.Rows.Remove(_selectedRow);
                    Logging.Logger.Log(Logging.SeverityGrades.Info,
                        string.Format("Successfully deleted file cache: {0}", deletionPath));
                }
                catch(Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Error, ex.Message);
                }
            }
        }




        
    }
}
