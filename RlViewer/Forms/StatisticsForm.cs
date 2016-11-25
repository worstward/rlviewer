using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RlViewer.Behaviors;

namespace RlViewer.Forms
{
    public partial class StatisticsForm : Form
    {
        public StatisticsForm(IEnumerable<Tuple<string, string>> statistics)
        {
            InitializeComponent();
         
            var dgv = FormsHelper.GetDataGrid(this, "Параметр", "Значение");
            this.Controls.Add(dgv);

            ShowInfo(statistics, dgv);
        }



        private void ShowInfo(IEnumerable<Tuple<string, string>> statisticsInfo, DataGridView dgv)
        {
            foreach (var pair in statisticsInfo)
            {
                dgv.Rows.Add(pair.Item1, pair.Item2);
            }
        }

        private void StatisticsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
