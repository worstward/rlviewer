﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            using (var openFileDlg = new OpenFileDialog() { Filter = Resourses.Filter })
            {
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    Files.FileProperties properties = new Files.FileProperties(openFileDlg.FileName);
                    file = RlViewer.Factories.File.Abstract.FileFactory.GetFactory(properties).Create(properties);
                }
                else return;
            }

            using (var iFrm = new InfoFrm(await Task.Run(() => ((RlViewer.Files.LocatorFile)file).Header.GetHeaderInfo())))
            {
                iFrm.ShowDialog();
            }
        }
    }
}
