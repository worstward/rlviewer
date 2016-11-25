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
    public partial class AggregatorOrderForm : Form
    {
        public AggregatorOrderForm(string[] sourceFiles)
        {
            InitializeComponent();
            _sourceFiles = sourceFiles;

            sourceFilesListBox.Items.AddRange(sourceFiles);
            sourceFilesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            sourceFilesListBox.MeasureItem += sourceFilesListBox_MeasureItem;
            sourceFilesListBox.DrawItem += sourceFilesListBox_DrawItem;

            if (sourceFilesListBox.Items.Count != 0)
            {
                sourceFilesListBox.SelectedIndex = 0;
            }
        }


        private string[] _sourceFiles;

        public string[] SourceFiles
        {
            get 
            {
                return _sourceFiles;
            }
        }

        private void Confirm()
        {
            _sourceFiles = sourceFilesListBox.Items.Cast<string>().ToArray();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Reject()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Confirm();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Reject();
        }

        private void upBtn_Click(object sender, EventArgs e)
        {
            MoveUp(sourceFilesListBox);
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            MoveDown(sourceFilesListBox);
        }

        private void MoveUp(ListBox myListBox)
        {
            var selectedIndex = myListBox.SelectedIndex;
            if (selectedIndex > 0)
            {
                myListBox.Items.Insert(selectedIndex - 1, myListBox.Items[selectedIndex]);
                myListBox.Items.RemoveAt(selectedIndex + 1);
                myListBox.SelectedIndex = selectedIndex - 1;
            }
        }

        private void MoveDown(ListBox myListBox)
        {
            var selectedIndex = myListBox.SelectedIndex;
            if (selectedIndex < myListBox.Items.Count - 1 & selectedIndex != -1)
            {
                myListBox.Items.Insert(selectedIndex + 2, myListBox.Items[selectedIndex]);
                myListBox.Items.RemoveAt(selectedIndex);
                myListBox.SelectedIndex = selectedIndex + 1;

            }
        }


        private void removeFileBtn_Click(object sender, EventArgs e)
        {

            var selectedIndex = sourceFilesListBox.SelectedIndex;
            sourceFilesListBox.Items.RemoveAt(sourceFilesListBox.SelectedIndex);

            if (sourceFilesListBox.Items.Count == 0)
            {
                Reject();
            }
            else
            {
                selectedIndex = selectedIndex == 0 ? 0 : selectedIndex - 1;
                sourceFilesListBox.SelectedIndex = selectedIndex;
            }
        }

        #region linesWrap


        private void sourceFilesListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(sourceFilesListBox.Items[e.Index].ToString(), sourceFilesListBox.Font, sourceFilesListBox.Width).Height;
        }

        private void sourceFilesListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(sourceFilesListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }
        #endregion

        private void AggregatorOrderForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Reject();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Confirm();
            }
        }

    }

}
