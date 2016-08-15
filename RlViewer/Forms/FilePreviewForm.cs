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
    public partial class FilePreviewForm : Form
    {
        public FilePreviewForm()
        {
            //TODO: для рл4 вызывается время создания (синтеза), а не полета. поправить
            InitializeComponent();

            treeView1.AfterSelect += treeView1_AfterSelect;

            var drives = Environment.GetLogicalDrives();
            InitTreeView(drives, treeView1.Nodes);

            InitDataGrid();
            FillDataGrid();

        }

        private string _fileToOpen;

        public string FileToOpen
        {
            get { return _fileToOpen; }
        }

        private void InitTreeView(IEnumerable<string> roots, TreeNodeCollection childNodes)
        {
            foreach (var root in roots)
            {
                childNodes.Add(root).Nodes.Add("tmpNode");
            }
        }

        private void LoadChildNodes(string dirName, TreeNodeCollection childNodes)
        {
            try
            {
                foreach (DirectoryInfo subdir in new DirectoryInfo(dirName).GetDirectories())
                {
                    childNodes.Add(subdir.Name).Nodes.Add("tmpNode");
                }

            }
            catch (UnauthorizedAccessException uaex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Unauthorized access: {0}", uaex));
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Error in file preview: {0}", ex));
            }          
        }



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
                dataGridView1.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                dataGridView1.Columns[i].ReadOnly = true;
            }

            dataGridView1.CellDoubleClick += (s, e) =>
            {
                AcceptFile();
            };


            foreach (DataGridViewTextBoxColumn column in dataGridView1.Columns)
            {
                column.Width = dataGridView1.Width / dataGridView1.Columns.Count;
            }
        }


        private string GetFullName(TreeNode node)
        {
            return GetName(node)
                .Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries)
                .Reverse()
                .Aggregate((x, y) => x + @"\" + y)
                + @"\";
        }

        private string GetName(TreeNode node)
        {
            if (node.Parent == null)
            {
                return node.Text;
            }
            return node.Text + @"\" + GetName(node.Parent);

        }

        private IEnumerable<string> GetLocatorFiles(IEnumerable<string> allFiles)
        {
            var locatorExt = Enum.GetNames(typeof(FileType));
            return allFiles.Where(x => locatorExt.Any(x.EndsWith));
        }



        private void FillDataGrid()
        {
            List<HeaderInfoOutput> headerInfos = new List<HeaderInfoOutput>();
            var files = GetLocatorFiles(Directory.GetFiles(treeView1.SelectedNode == null ? GetFullName(treeView1.Nodes[0])
                : GetFullName(treeView1.SelectedNode)));

            foreach (var file in files)
            {
                try
                {
                    var props = new Files.FileProperties(file);
                    var header = props.Type == FileType.raw ? null :
                        Factories.Header.Abstract.HeaderFactory.GetFactory(props).Create(file);
                    headerInfos.Add(Factories.FilePreview.Abstract.FilePreviewFactory.GetFactory(props)
                        .Create(file, header).GetPreview());
                }
                catch (NotSupportedException)
                {
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Internal, string.Format("Error in file preview: {0}", ex));
                }
            }

            dataGridView1.Rows.Clear();

            for(int i = 0; i < headerInfos.Count; i++)
            {
                dataGridView1.Rows.Add();


                dataGridView1.Rows[i].Cells[0].Value = Path.GetFileNameWithoutExtension(headerInfos[i].HeaderName);
                dataGridView1.Rows[i].Cells[1].Value = Path.GetExtension(headerInfos[i].HeaderName);
                for (int j = 1; j < headerInfos[i].Params.Count(); j++)
                {
                    dataGridView1.Rows[i].Cells[j + 1].Value = headerInfos[i].Params.ToArray()[j].Item2;
                }
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                FillDataGrid();
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Internal, "Unable to fill preview data grid");
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AcceptFile()
        {
            if (dataGridView1.SelectedRows.Count != 0)
            { 
                _fileToOpen = Path.Combine(GetFullName(treeView1.SelectedNode),
                                   dataGridView1.CurrentRow.Cells[0].Value.ToString() + 
                                   dataGridView1.CurrentRow.Cells[1].Value.ToString());
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }



        private void okBtn_Click(object sender, EventArgs e)
        {
            AcceptFile();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //remove tmpNode
            e.Node.Nodes[0].Remove();

            LoadChildNodes(GetFullName(e.Node), e.Node.Nodes);
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.Nodes.Clear();
            e.Node.Nodes.Add("tmpNode");
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewTextBoxColumn column in dataGridView1.Columns)
            {
                column.Width = dataGridView1.Width / dataGridView1.Columns.Count;
            }
        }

        private TreeNode GetRoot(TreeNode node)
        {
            if (node.Parent == null)
            {
                return node;
            }
            return GetRoot(node.Parent);
        }

        private void FilePreviewForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                AcceptFile();
            }       
        }



    }
}
