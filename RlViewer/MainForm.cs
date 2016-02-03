using System;
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
        public MainForm()
        {
            using (var openFileDlg = new OpenFileDialog())// { Filter = Resourses.Filter })
            {
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {


                    Hierarchy.FileProperties properties = new Hierarchy.FileProperties(openFileDlg.FileName);

                    var file = Factories.FileFactory.GetFactory(properties).Create(properties);
                }
            }


            InitializeComponent();
        }
    }
}
