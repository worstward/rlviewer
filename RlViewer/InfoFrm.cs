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
    public partial class InfoFrm : Form
    {
        public InfoFrm(params List<Tuple<string, string>>[] headerInfo)
        {
            _headerInfo = headerInfo[0];
            

            InitializeComponent();
            ShowInfo();
        }

        List<Tuple<string, string>> _headerInfo;

        private void ShowInfo()
        {
            for(int i = 0; i < _headerInfo.Count; i++)
            {
                dataGridView1.Rows.Add(_headerInfo[i].Item1, _headerInfo[i].Item2);  
            }

            
        }



    }
}
