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
            //_sstp = sstp;
            InitializeComponent();
            frameSizeAzimuthCb.DataSource = _frameAzimuthSizes;           
            frameAzimuthCoefCb.DataSource = _frameAzimuthCompressionCoefs;
            frameRangeCoefCb.DataSource = _frameRangeCompressionCoefs;

            blockSizeAzimuthCb.DataSource = _blockAzimuthSizes;          
            blockAzimuthCoefCb.DataSource = _blockAzimuthCompressionCoefs;
            blockRangeCoefCb.DataSource = _blockRangeCompressionCoefs;
            matrixExtensionCb.DataSource = _matrixExtensionCoefs;

            _fileName = fileName;
            
        }

        Behaviors.Synthesis.ServerSarTaskParams _sstp;
        private Behaviors.Synthesis.Hologram.Abstract.SynthesisSourceKRhg _synthesisSourceRhg;


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



        public Behaviors.Synthesis.ServerSarTaskParams GenerateSstp(int currentBlock, int totalRhgLines, int rangeShift, int azimuthShift)
        {
            var holFileProp = new RlViewer.Files.FileProperties(_fileName);
            var holHeader = Factories.Header.Abstract.HeaderFactory.GetFactory(holFileProp).Create(_fileName) as Headers.Concrete.K.KHeader;
            var holHeaderStruct = holHeader.HeaderStruct;
            var rhg = (Files.Rhg.Abstract.RhgFile)Factories.File.Abstract.FileFactory.GetFactory(holFileProp).Create(holFileProp, holHeader, null);

            //var sourceRhg = Factories.SynthesisSourceRhg.SynthesisSourceRhgFactory.Create((int)holHeaderStruct.locatorHeader.channelNumber, rhg);


            var sstp = new Behaviors.Synthesis.ServerSarTaskParams();

            sstp.struct_id = 1;
            sstp.input_dir = new byte[256];
            sstp.input_file = new byte[256];
            sstp.input_file_K1 = new byte[256];
            sstp.output = false;

            var outputDir = Encoding.UTF8.GetBytes(@"C:\Users\lenovo\Desktop\");//@"c:\vega\temp\obzor");
            var outputDirArr = new byte[256];
            for (int i = 0; i < outputDir.Length; i++)
            {
                outputDirArr[i] = outputDir[i];
            }
            sstp.output_dir = outputDirArr;
            sstp.output_dir_loc = new byte[256];


            //var output_file = Encoding.UTF8.GetBytes("rli.rl4");
            var output_fileArr = new byte[256];
            //for (int i = 0; i < output_file.Length; i++)
            //{
            //    output_fileArr[i] = output_file[i];
            //}
            sstp.output_file = output_fileArr;

            //var ip = Encoding.UTF8.GetBytes("127.0.0.1:9978");
            var ipArr = new byte[256];
            //for (int i = 0; i < ip.Length; i++)
            //{
            //    ipArr[i] = ip[i];
            //}
            sstp.Client_ip_address = ipArr;


            sstp.M = 1;
            sstp.ny_start = 0;
            sstp.ny_start_initial = 0;
            sstp.Mshift = Convert.ToInt32(frameSizeAzimuthCb.SelectedValue.ToString());
            sstp.Mlength = Convert.ToInt32(blockSizeAzimuthCb.SelectedValue.ToString());
            sstp.Mparts = 1;//totalRhgLines / (sstp.Mlength - sstp.Mshift) - 1;
            sstp.Mscale = Convert.ToSingle(frameAzimuthCoefCb.SelectedValue.ToString());
            sstp.Mscale_initial = 1;
            sstp.N = currentBlock;
            sstp.nx_start = rangeShift;
            sstp.nx_start_initial = rangeShift;
            sstp.Nshift = rhg.Width;
            sstp.Nlength = rhg.Width;
            sstp.Nstripes = 1;
            sstp.Nscale = Convert.ToSingle(frameRangeCoefCb.SelectedValue.ToString());
            sstp.Nscale_initial = 1;
            sstp.lambda = 299792458 / (holHeaderStruct.transmitterHeader.frequency * 1000000);

            sstp.f0 = holHeaderStruct.transmitterHeader.freqWidth / 2;
            sstp.Tp = holHeaderStruct.transmitterHeader.impulseLength / 1000000;
            sstp.dR = 299792458 / 100000000f;/// (2 * holHeaderStruct.adcHeader.adcFreq * 106 / holHeaderStruct.adcHeader.freqDivisor);
            sstp.Fimpulses = 1000;
            sstp.Vfly = holHeaderStruct.synchronizerHeader.azimuthDecompositionStep * sstp.Fimpulses;
            sstp.Rmin = holHeaderStruct.synchronizerHeader.initialRange * 1000;
            sstp.Angle = holHeaderStruct.antennaSystemHeader.antennaAngle;
            sstp.Do_range_compress = true;
            sstp.interpolate = true;
            sstp.nLobs = 5;
            sstp.LoopBlocking = true;
            sstp.pNstripes = 64;
            sstp.pNlength = 1024;
            sstp.pNshift = 1024;
            sstp.cutDoppler = true;
            sstp.Doppler_Min = 512;
            sstp.Doppler_Max = 512;
            sstp.simul_clear_input = true;
            sstp.AF_pX = 32768;
            sstp.AF2_ncycle = 1;
            sstp.F1d_Inmem = true;
            sstp.F2d_Inmem = true;
            sstp.Vs_Inmem = true;

            sstp.Azimut_chirp_sign = holHeaderStruct.synchronizerHeader.azimuthSign != 1 ? -1 : 1;
            sstp.Range_chirp_sign = holHeaderStruct.synchronizerHeader.rangeSign != 1 ? -1 : 1;
            sstp.Nav_LR_Side = (byte)(holHeaderStruct.synchronizerHeader.board) == 2 ? -1 : 1;
            sstp.Memory_parts = 4;
            sstp.AF_ncycle = 4096;
            sstp.AF_WinX = 128;
            sstp.AF_WinY = 1024;
            sstp.AF2_Y_N = false;
            sstp.AF2_percent = 10;

            var afNameArr = new byte[256];
            var afName = Encoding.UTF8.GetBytes("AFTrajDump.dat");
            for(int i = 0; i < afName.Length; i++)
            {
                afNameArr[i] = afName[i];
            }
            sstp.AF_FileName = afNameArr;
            sstp.AF_FromFileName = new byte[256];
            sstp.AF_FromFileNameList = new byte[256];
            sstp.input_dir = new byte[256];
            sstp.input_file = new byte[256];
            sstp.input_file_K1 = new byte[256];

            sstp.fbp_KoeffSumming = new byte[20];
            sstp.RadioSuppression = Convert.ToSingle(radioSuppressionTb.Text);
            sstp.ppx = new int[20];
            var ppy = new int[20];
            ppy[0] = 512;
            sstp.ppy = ppy;

            sstp.reserved = new byte[936];
            sstp.simul_file = new byte[100];
            sstp.Nav_File = new byte[256];
            sstp.XNSum1024YNSum = (int)(Convert.ToSingle(blockRangeCoefCb.Text) * 1024 + Convert.ToSingle(blockAzimuthCoefCb.Text));

            sstp.Times_mc = (float)Convert.ToDouble(matrixExtensionCb.SelectedValue.ToString());
            sstp.dsp_zone_width = 512;
            sstp.frame_width = rhg.Width;
            sstp.n_of_frames = 1;

            sstp.Hc = 131.92f;//holheaderStruct.flightHeader
            //var asd = _sstp;
            //var dsa = asd.Mlength;

            //var fName = Encoding.UTF8.GetString(_sstp.AF_FileName);
            //fName = fName.Substring(0, fName.IndexOf('\0'));


            return sstp;
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
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }
    }
}
