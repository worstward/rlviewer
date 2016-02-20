using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RlViewer.Behaviors.TileCreator.Abstract;
using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Factories.File.Abstract;

namespace RlViewer
{
    public partial class MainForm : Form
    {

        Files.LoadedFile file;
        public MainForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            using (var openFileDlg = new OpenFileDialog() { Filter = Resources.Filter })
            {
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    Files.FileProperties properties = new Files.FileProperties(openFileDlg.FileName);
                    file = FileFactory.GetFactory(properties).Create(properties);
                }
                else return;
            }

            using (var iFrm = new InfoFrm(await Task.Run(() => ((RlViewer.Files.LocatorFile)file).Header.GetHeaderInfo())))
            {
                iFrm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            Task.Run(() =>
            {
                ITileCreator tc = TileCreatorFactory.GetFactory(file.Properties).Create(file as RlViewer.Files.LocatorFile);
                return tc.Tiles;
            }).ContinueWith((t) =>
            {
                tiles = t.Result;
                drawer = new Behaviors.Draw.Drawing(tiles, pictureBox1.Size);
                this.Text = file.Properties.FilePath;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }

        private RlViewer.Behaviors.TileCreator.Tile[] tiles;

        private RlViewer.Behaviors.Draw.Drawing drawer;

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox1.Image = drawer.Draw(pictureBox1.Size, tiles,
                new Point(3500 + hScrollBar1.Value * 50, 7000));
        }
       
    }
}
