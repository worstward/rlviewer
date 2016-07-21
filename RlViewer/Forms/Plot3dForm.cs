using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace RlViewer.Forms
{
    public partial class Plot3dForm : Form
    {

        public Plot3dForm(float xFrom, float xTo, float yFrom, float yTo, float[] zVal)
        {
            _xFrom = xFrom;
            _xTo = xTo;
            _yFrom = yFrom;
            _yTo = yTo;

            _zVal = zVal;

            InitializeComponent();
        }

        private float _xFrom;
        private float _xTo;
        private float _yFrom;
        private float _yTo;
        
        private float[] _zVal;

        private void ilPanel1_Load(object sender, EventArgs e)
        {
            ILArray<float> X = ILMath.vec<float>(_xFrom, 1, _xTo - 1);
            ILArray<float> Y = ILMath.vec<float>(_yFrom, 1, _yTo - 1);

            ILArray<float> YMat = 1;
            ILArray<float> XMat = ILMath.meshgrid(X, Y, YMat);
            ILArray<float> A = ILMath.zeros<float>(X.Length == 0 ? 1 : X.Length, Y.Length == 0 ? 1 : Y.Length, 3);

            A[":;:;0"] = _zVal;
            A[":;:;1"] = XMat;
            A[":;:;2"] = YMat;


            var surface = new ILSurface(A) {UseLighting = true,  Children = { new ILColorbar() }};
            var plot = new ILPlotCube(twoDMode: false) { surface };
            var scene = new ILScene { plot };
            ilPanel1.Scene.Add(scene);

        }
     
    }
}
