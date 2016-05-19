using System;
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
        public TileStatusForm(string tileDirectory, string currFile)
        {
            InitializeComponent();
            InitDataGrid();

            _tileDir = tileDirectory;
            _currFile = currFile;
            FillDataGrid(_tileDir);
           
        }

        private string _tileDir;
        private string _currFile;

        private void InitDataGrid()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].ReadOnly = true;
            }

            dataGridView1.CellDoubleClick += (s, e) =>
                {
                    OpenInExplorer();
                };


            foreach (DataGridViewTextBoxColumn column in dataGridView1.Columns)
            {
                column.Width = dataGridView1.Width / 3 - 1;
            }
        }



        private void FillDataGrid(string tileDir)
        {
            var currFileTilePath = _currFile == string.Empty ? string.Empty : Behaviors.TileCreator.Abstract.TileCreator.GetDirectoryName(_currFile);

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


                        var tilePath = Path.Combine(Path.GetFileName(fileNameDirectory), Path.GetFileName(extensionDirectory));

                        dataGridView1.Rows.Add(tilePath, creationTime,
                            Directory.GetFiles(imgDirectory).Where(x => Path.GetExtension(x).ToLowerInvariant() == ".tl").Count());

                        var b = Path.Combine(tilePath, creationTime.ToFileTime().ToString());
                        if (currFileTilePath == imgDirectory)
                        {
                            var style = new DataGridViewCellStyle();
                            style.BackColor = Color.Aquamarine;

                            foreach (var cell in dataGridView1.Rows.Cast<DataGridViewRow>().LastOrDefault().Cells.Cast<DataGridViewCell>())
                            {
                                cell.Style = style;
                            }
                        }
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TileStatusForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null && dataGridView1.SelectedRows.Count != 0)
            {

                var confirmation = MessageBox.Show("Вы уверены, что хотите удалить кеш выбранных файлов?",
                                     "Подтвердите удаление",
                                     MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    foreach (var r in dataGridView1.SelectedRows)
                    {
                        var row = r as DataGridViewRow;
                        
                        try
                        {
                            var deletionPath = Path.Combine(_tileDir, row.Cells[0].Value.ToString(),
                            ((DateTime)row.Cells[1].Value).ToFileTime().ToString());
                            Directory.Delete(deletionPath, true);


                            //if (Directory.GetDirectories(Directory.GetParent(deletionPath)).Select(x => Directory.GetFiles(x)).SelectMany(x => x).Count() == 0)
                            //{
 
                            //}
                            dataGridView1.Rows.Remove(row);
                            Logging.Logger.Log(Logging.SeverityGrades.Info,
                                string.Format("Successfully deleted file cache: {0}", deletionPath));
                        }
                        catch (Exception ex)
                        {
                            Logging.Logger.Log(Logging.SeverityGrades.Internal, ex.Message);
                        }
                    }
                }
            }
        }

        private void OpenInExplorer()
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                var selectedRow = (dataGridView1.SelectedRows.Cast<DataGridViewRow>()).FirstOrDefault();
                var path = Path.Combine(_tileDir, selectedRow.Cells[0].Value.ToString(),
                    ((DateTime)selectedRow.Cells[1].Value).ToFileTime().ToString());

                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }



        
    }
}
