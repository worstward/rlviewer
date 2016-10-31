using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;

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

        //public void Test(Bitmap bmp)
        //{
        //    String wktProj = null;
        //    String tmpPath = @"C:\tmp.bmp";
        //    Bitmap tmpBitmap = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), pixFormat);
        //    tmpBitmap.Save(tmpPath, ImageFormat.Bmp);

        //    String[] options = null;
        //    Gdal.AllRegister();
        //    OSGeo.GDAL.Driver srcDrv = Gdal.GetDriverByName("GTiff");
        //    Dataset srcDs = Gdal.Open(tmpPath, Access.GA_ReadOnly);
        //    Dataset dstDs = srcDrv.CreateCopy(path, srcDs, 0, options, null, null);

        //    //Set the map projection
        //    Osr.GetWellKnownGeogCSAsWKT("WGS84", out wktProj);
        //    dstDs.SetProjection(wktProj);

        //    //Set the map georeferencing
        //    double mapWidth = Math.Abs(latLongMap.listBounds.topRight.x - latLongMap.listBounds.bottomLeft.x);
        //    double mapHeight = Math.Abs(latLongMap.listBounds.topRight.y - latLongMap.listBounds.bottomLeft.y);
        //    double[] geoTransfo = new double[] { -5.14, mapWidth / bmp.Width, 0, 48.75, 0, mapHeight / bmp.Height };
        //    dstDs.SetGeoTransform(geoTransfo);

        //    dstDs.FlushCache();
        //    dstDs.Dispose();
        //    srcDs.Dispose();
        //    srcDrv.Dispose();
        //    tmpBitmap.Dispose();

        //    System.IO.File.Delete(tmpPath);
        //}



        string[] _sourceFiles;

        public string[] SourceFiles
        {
            get { return _sourceFiles; }
        }

        private void Confirm()
        {
            _sourceFiles = sourceFilesListBox.Items.Cast<string>().ToArray();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


        private void okBtn_Click(object sender, EventArgs e)
        {
            Confirm();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void upBtn_Click(object sender, EventArgs e)
        {
            MoveUp(sourceFilesListBox);
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            MoveDown(sourceFilesListBox);
        }

        void MoveUp(ListBox myListBox)
        {
            int selectedIndex = myListBox.SelectedIndex;
            if (selectedIndex > 0)
            {
                myListBox.Items.Insert(selectedIndex - 1, myListBox.Items[selectedIndex]);
                myListBox.Items.RemoveAt(selectedIndex + 1);
                myListBox.SelectedIndex = selectedIndex - 1;
            }
        }

        void MoveDown(ListBox myListBox)
        {
            int selectedIndex = myListBox.SelectedIndex;
            if (selectedIndex < myListBox.Items.Count - 1 & selectedIndex != -1)
            {
                myListBox.Items.Insert(selectedIndex + 2, myListBox.Items[selectedIndex]);
                myListBox.Items.RemoveAt(selectedIndex);
                myListBox.SelectedIndex = selectedIndex + 1;

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
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Confirm();
            }
        }
    }
}
