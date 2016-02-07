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
        public InfoFrm(params HeaderInfoOutput[] headers)
        {
            InitializeComponent();
            InitializeTabs(headers);             
        }


        /// <summary>
        /// Adds tabs to TabControl according to number of input headers
        /// </summary>
        private void InitializeTabs(HeaderInfoOutput[] headers)
        {
            for(int i = 0; i < headers.Length; i++)
            {
                var dgv = new DataGridView() { Size = infoTabsControl.Size, Location = this.Location };

                dgv.Columns.AddRange(new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn() { HeaderText = "Параметр", Name = "paramColumn", ReadOnly = true },
                    new DataGridViewTextBoxColumn() { HeaderText = "Значение", Name = "valueColumn", ReadOnly = true }
                });

                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    dgv.Columns[j].Width = dgv.Width / dgv.Columns.Count;
                }

                //((System.Collections.Generic.IEnumerable<DataGridViewColumn>)dgv.Columns).Select(x => x.Width = dgv.Width / dgv.Columns.Count);


                infoTabsControl.TabPages.Add(headers[i].HeaderName);
                infoTabsControl.TabPages[i].Controls.Add(dgv);
                ShowInfo(headers[i].Params, dgv);
            }
        }


        private void ShowInfo(List<Tuple<string, string>> hInfo, DataGridView dgv)
        {
            for (int i = 0; i < hInfo.Count; i++)
            {
                dgv.Rows.Add(hInfo[i].Item1, hInfo[i].Item2);  
            }      
        }

    }
}
