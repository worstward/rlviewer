using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;


namespace RlViewer.Forms
{
    public partial class SectionGraphForm : Form
    {
        public SectionGraphForm(IEnumerable<PointF> points, float initialPointMark, string caption = "")
        {
            _points = points;
            _initialPointMark = initialPointMark;
            _zedGraph = new ZedGraphControl();
            _zedGraph.Location = this.Location;
            
            _zedGraph.IsEnableVZoom = false;
            _zedGraph.IsEnableSelection = false;
            _zedGraph.IsShowHScrollBar = true;           
            _zedGraph.IsAutoScrollRange = true;
            this.Controls.Add(_zedGraph);

            
            InitializeComponent();

            //uncomment for logarithm graph choser
            groupBox1.Height = 0;

            _zedGraph.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            _zedGraph.Size = new System.Drawing.Size(this.Width, this.Height - (this.Height - groupBox1.Location.Y));
            this.Text = caption;
            amplitudeRb.Checked = true;
        }

        private IEnumerable<PointF> _points;
        private ZedGraphControl _zedGraph;
        private float _initialPointMark;

        private void BuildGraph(IEnumerable<PointF> points, string yAxisName)
        {
            GraphPane pane = _zedGraph.GraphPane;

            pane.YAxis.Scale.MagAuto = false;
            pane.XAxis.Title.Text = "Отсчеты";
            pane.YAxis.Title.Text = yAxisName;

            pane.Title.Text = string.Empty;
            pane.CurveList.Clear();

            PointPairList list = new PointPairList();

            foreach(var point in points)
            {
                list.Add(point.X, point.Y);
            }

            //add vertical line to mark clicked point
            int i = pane.AddYAxis("");
            pane.YAxisList[i].Scale.MagAuto = false;
            pane.YAxisList[i].Scale.IsVisible = false;
            pane.YAxisList[i].Color = Color.Red;
            pane.YAxisList[i].Scale.IsVisible = false;
            pane.YAxisList[i].MajorTic.IsAllTics = false;
            pane.YAxisList[i].MinorTic.IsAllTics = false;
            pane.YAxisList[i].Cross = _initialPointMark;


            LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.Diamond);
            myCurve.Symbol.Size = 4f;
            myCurve.Symbol.Fill.Type = FillType.Solid;
            _zedGraph.AxisChange();

            _zedGraph.Invalidate();
        }

        private void SectionGraphForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void amplitudeRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                BuildGraph(_points, "Амплитуды");
            }
        }

        private void LogRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                BuildGraph(_points.Select(x => new PointF(x.X, 10 * (float)Math.Log10(x.Y))), "Дб");
            }
        }




    }
}
