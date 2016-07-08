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
        public StatisticsForm(Files.LocatorFile file, Behaviors.AreaSelector.AreaSelector areaSelector)
        {
            InitializeComponent();
         
            var dgv = FormsHelper.GetDataGrid(this, "Параметр", "Значение");
            this.Controls.Add(dgv);

            var statsInfo = GetStatistics(file, areaSelector);
            ShowInfo(statsInfo, dgv);
        }


        private IEnumerable<Tuple<string, string>> GetStatistics(Files.LocatorFile file, Behaviors.AreaSelector.AreaSelector areaSelector)
        {
            var maxSample = file.GetMaxSample(
                            new Rectangle(areaSelector.Area.Location.X, areaSelector.Area.Location.Y,
                            areaSelector.Area.Width, areaSelector.Area.Height));

            var minSample = file.GetMinSample(
                    new Rectangle(areaSelector.Area.Location.X, areaSelector.Area.Location.Y,
                    areaSelector.Area.Width, areaSelector.Area.Height));

            var maxSampleLoc = file.GetMaxSampleLocation(
                new Rectangle(areaSelector.Area.Location.X, areaSelector.Area.Location.Y,
                    areaSelector.Area.Width, areaSelector.Area.Height));

            var minSampleLoc = file.GetMinSampleLocation(
                new Rectangle(areaSelector.Area.Location.X, areaSelector.Area.Location.Y,
                    areaSelector.Area.Width, areaSelector.Area.Height));

            var avgSample = file.GetAvgSample(
                new Rectangle(areaSelector.Area.Location.X, areaSelector.Area.Location.Y,
                    areaSelector.Area.Width, areaSelector.Area.Height));


            List<Tuple<string, string>> statistics = new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(file.Properties.FilePath)),
                    new Tuple<string, string>("X1", areaSelector.Area.Location.X.ToString()),
                    new Tuple<string, string>("Y1", areaSelector.Area.Location.Y.ToString()),
                    new Tuple<string, string>("X2", (areaSelector.Area.Location.X + areaSelector.Area.Width).ToString()),
                    new Tuple<string, string>("Y2", (areaSelector.Area.Location.Y + areaSelector.Area.Height).ToString()),
                    new Tuple<string, string>("Ширина фрагмента", areaSelector.Area.Width.ToString()),
                    new Tuple<string, string>("Высота фрагмента", areaSelector.Area.Height.ToString()),
                    new Tuple<string, string>("Максимальная амплитуда", maxSample.ToString()),
                    new Tuple<string, string>("Xmax", maxSampleLoc.X.ToString()),
                    new Tuple<string, string>("Ymax", maxSampleLoc.Y.ToString()),
                    new Tuple<string, string>("Минимальная амплитуда", minSample.ToString()),
                    new Tuple<string, string>("Xmin", minSampleLoc.X.ToString()),
                    new Tuple<string, string>("Ymin", minSampleLoc.Y.ToString()),
                    new Tuple<string, string>("Средняя амплитуда", avgSample.ToString())
                };

            return statistics;
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
