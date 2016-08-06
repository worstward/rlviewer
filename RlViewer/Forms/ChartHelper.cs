using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace RlViewer.Forms
{
    class ChartHelper
    {

        public ChartHelper(Behaviors.Draw.HistContainer histogram)
        {
            _histogram = histogram;
        }

        private Behaviors.Draw.HistContainer _histogram;

        public void InitChart(Chart c)
        {
            c.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            c.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            c.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            c.ChartAreas[0].AxisX.Maximum = 265;//max pixel val + 10
            c.ChartAreas[0].AxisX.Minimum = -10;//min pixel val - 10
            c.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            c.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            c.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            c.ChartAreas[0].BackColor = Color.Transparent;
            c.ChartAreas[0].AxisY.Minimum = 0;
            c.Series[0]["PixelPointWidth"] = "3";
            c.ChartAreas[0].AxisY.LabelStyle.Format = "#";
            c.Series[0].IsVisibleInLegend = false;
        }

        public async void RedrawChart(Chart c, Image img, int fileWidth, int fileHeight)
        {

            var width = img.Width > fileWidth ? fileWidth : img.Width;
            var height = img.Height > fileHeight ? fileHeight : img.Height;

            if (width > 0 && height > 0)
            {
                c.ChartAreas[0].AxisY.Maximum = width * height / 2;
                c.Series[0].Points.DataBindXY(new List<int>(Enumerable.Range(0, 256)), "bits",
                    await _histogram.GetHistogramAsync(img, width, height), "values");
            }

        }

        //private async void RedrawChart(Chart c, int fileWidth, int fileHeight)
        //{
        //    c.ChartAreas[0].AxisY.Maximum = fileWidth * fileHeight / 4;
        //    c.Series[0].Points.DataBindXY(new List<int>(Enumerable.Range(0, 256)), "bits",
        //      await _histogram.GetHistogramAsync(_tiles), "values");
        //}



    }
}
