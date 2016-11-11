using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.Forms
{
    public partial class SynthesisParamsForm : Form
    {
        public SynthesisParamsForm(string fileName)
        {
            InitializeComponent();

            frameSizeAzimuthCb.DataSource = _frameAzimuthSizes;           
            frameAzimuthCoefCb.DataSource = _frameAzimuthCompressionCoefs;
            frameRangeCoefCb.DataSource = _frameRangeCompressionCoefs;

            blockSizeAzimuthCb.DataSource = _blockAzimuthSizes;          
            blockAzimuthCoefCb.DataSource = _blockAzimuthCompressionCoefs;
            blockRangeCoefCb.DataSource = _blockRangeCompressionCoefs;
            radioSuppressionCoefCb.DataSource = _matrixExtensionCoefs;

            _fileName = fileName;
        }

        private string _fileName;

        private List<int> _blockAzimuthSizes = new List<int>()
        {
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536,
            131072
        };

        private List<int> _frameAzimuthSizes = new List<int>()
        {
            512,
            1024,
            2048,
            4096,
            8192,
            16384,
            32768,
            65536,
            131072
        };

        private List<int> _blockRangeCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16};
        private List<int> _blockAzimuthCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };
        private List<int> _frameRangeCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };
        private List<int> _frameAzimuthCompressionCoefs = new List<int>() { 1, 2, 4, 8, 16 };
        private List<float> _matrixExtensionCoefs = new List<float>() { 1, 1.5f, 2, 4 };



        private void GenerateSstp()
        {

            var holFileProp = new RlViewer.Files.FileProperties(_fileName);
            var holHeader = Factories.Header.Abstract.HeaderFactory.GetFactory(holFileProp).Create(_fileName) as Headers.Concrete.K.KHeader;
            var holheaderStruct = holHeader.HeaderStruct;


            var sstp = new Behaviors.Synthesis.ServerSarTaskParams();

            sstp.struct_id = 0;
            sstp.input_dir = new byte[256];
            sstp.input_file = new byte[256];
            sstp.input_file_K1 = new byte[256];
            sstp.output = false;
            var outputDir = Encoding.UTF8.GetBytes(@"c:\vega\temp\obzor");
            sstp.output_dir = outputDir;
            sstp.output_dir_loc = outputDir;
            sstp.output_file = Encoding.UTF8.GetBytes("rli.rl4");
            var ip = Encoding.UTF8.GetBytes("127.0.0.1:9978");
            sstp.Client_ip_address = ip;
            sstp.M = 0;
            sstp.ny_start = 0;
            sstp.ny_start_initial = 0;
            //            sstp.Mshift = holheaderStruct.



        }





        private void Cancel()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void SynthesisParams_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Cancel();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
