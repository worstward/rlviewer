using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace RlViewer.Forms
{
    public partial class SynthesisParamsForm : Form
    {
        public SynthesisParamsForm(RlViewer.Settings.SynthesisSettings synthesisSettings, RlViewer.Settings.GuiSettings guiSettings, params string[] fileName)
        {
            InitializeComponent();
            InitControls(synthesisSettings, guiSettings);
            _fileName = fileName.FirstOrDefault();
            _synthesisSettings = synthesisSettings;
            ToggleRhgFileParamsTab(guiSettings.ShowRhgSynthesisHeaderParamsTab);
            Forms.FormsHelper.AddTbClickEvent<MaskedTextBox>(tabControl1);

            var holFileProp = new RlViewer.Files.FileProperties(_fileName);
            var holHeader = (Headers.Concrete.K.KHeader)Factories.Header.Abstract.HeaderFactory.GetFactory(holFileProp).Create(_fileName);
            _headerStruct = holHeader.HeaderStruct;
            _rhg = (Files.Rhg.Abstract.RhgFile)Factories.File.Abstract.FileFactory.GetFactory(holFileProp).Create(holFileProp, holHeader, null);

        }

        private RlViewer.Settings.SynthesisSettings _synthesisSettings;
        private string _fileName;
        private Headers.Concrete.K.KFileHeaderStruct _headerStruct;
        private Files.Rhg.Abstract.RhgFile _rhg;

        private void ToggleRhgFileParamsTab(bool extendedParameters)
        {
            if (!extendedParameters)
            {
                var commonPage = (TabPage)tabControl1.Controls.Find("commonPage", true).First();
                var tabHandle = tabControl1.Handle;//known ms bug, need to read tab container handle to insert tab in it
                tabControl1.Controls.Remove(commonPage);
            }

            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(tabControl1.Width / tabControl1.TabCount, 0);
        }


        private void InitControls(RlViewer.Settings.SynthesisSettings synthesisSettings, RlViewer.Settings.GuiSettings guiSettings)
        {
            frameSizeAzimuthCb.DataSource = guiSettings.FrameAzimuthSizes;
            frameAzimuthCoefCb.DataSource = guiSettings.FrameAzimuthCompressionCoefs;
            frameRangeCoefCb.DataSource = guiSettings.FrameRangeCompressionCoefs;
            blockSizeAzimuthCb.DataSource = guiSettings.BlockAzimuthSizes;
            blockAzimuthCoefCb.DataSource = guiSettings.BlockAzimuthCompressionCoefs;
            blockRangeCoefCb.DataSource = guiSettings.BlockRangeCompressionCoefs;
            matrixExtensionCb.DataSource = guiSettings.MatrixExtensionCoefs;
            pNLengthCb.DataSource = guiSettings.PNLengthValues;
            minDopplerCb.DataSource = guiSettings.MinDopplerFilterValues;
            maxDopplerCb.DataSource = guiSettings.MaxDopplerFilterValues;
            memoryChunksCountCb.DataSource = guiSettings.MemoryChunksCountValues;

            frameAzimuthCoefCb.SelectedItem = synthesisSettings.FrameAzimuthCompressionCoef;
            frameRangeCoefCb.SelectedItem = synthesisSettings.FrameRangeCompressionCoef;
            frameSizeAzimuthCb.SelectedItem = synthesisSettings.FrameAzimuthSize;

            blockAzimuthCoefCb.SelectedItem = synthesisSettings.BlockAzimuthCompressionCoef;
            blockRangeCoefCb.SelectedItem = synthesisSettings.BlockRangeCompressionCoef;
            blockSizeAzimuthCb.SelectedItem = synthesisSettings.BlockAzimuthSize;
            matrixExtensionCb.SelectedItem = synthesisSettings.MatrixExtensionCoef;
            memoryChunksCountCb.SelectedItem = synthesisSettings.MemoryChunksCount;


            rliNormalizationCoefTb.Text = synthesisSettings.RliNormalizingCoef.ToString();
            rhgNormalizationCoefTb.Text = synthesisSettings.RhgNormalizingCoef.ToString();
            radioSuppressionTb.Text = synthesisSettings.RadioSuppressionCoef.ToString();

            useDopplerFilteringCb.Checked = synthesisSettings.UseDopplerFiltering;

            ToggleDopplerControls(synthesisSettings.UseDopplerFiltering);

            pNLengthCb.SelectedItem = synthesisSettings.PNLength;
            pNShiftTb.Text = synthesisSettings.PNShift.ToString();
            minDopplerCb.SelectedItem = synthesisSettings.MinDoppler;
            maxDopplerCb.SelectedItem = synthesisSettings.MaxDoppler;

        }

        private void ConfirmParams()
        {
            _synthesisSettings.FrameAzimuthCompressionCoef = (int)frameAzimuthCoefCb.SelectedItem;
            _synthesisSettings.FrameRangeCompressionCoef = (int)frameRangeCoefCb.SelectedItem;
            _synthesisSettings.FrameAzimuthSize = (int)frameSizeAzimuthCb.SelectedItem;

            _synthesisSettings.BlockAzimuthCompressionCoef = (int)blockAzimuthCoefCb.SelectedItem;
            _synthesisSettings.BlockRangeCompressionCoef = (int)blockRangeCoefCb.SelectedItem;
            _synthesisSettings.BlockAzimuthSize = (int)blockSizeAzimuthCb.SelectedItem;

            _synthesisSettings.MatrixExtensionCoef = (float)matrixExtensionCb.SelectedItem;
            _synthesisSettings.MinDoppler = (int)minDopplerCb.SelectedItem;
            _synthesisSettings.MaxDoppler = (int)maxDopplerCb.SelectedItem;
            _synthesisSettings.UseDopplerFiltering = useDopplerFilteringCb.Checked;
            _synthesisSettings.MemoryChunksCount = (int)memoryChunksCountCb.SelectedItem;

            var pNLength = (int)pNLengthCb.SelectedItem;

            int pNShift;
            Int32.TryParse(pNShiftTb.Text, out pNShift);
            if (pNShift == 0)
            {
                FormsHelper.ShowErrorMsg(string.Format("Неверный параметр: {0} ({1})", pNShiftLbl.Text, eokPage.Text));
                return;
            }
            _synthesisSettings.PNShift = pNShift;


            if (pNLength > _rhg.Width)
            {
                FormsHelper.ShowErrorMsg(string.Format("Размер смещения по дальности не должен превышать ширины полосы по дальности ({0} < {1}) ({2})",
                    pNLength.ToString(), pNShift.ToString(), eokPage.Text));
                return;
            }

            if (pNShift > pNLength)
            {
                FormsHelper.ShowErrorMsg(string.Format("Размер смещения по дальности не должен превышать ширины полосы по дальности ({0} < {1}) ({2})",
                    pNLength.ToString(), pNShift.ToString(), eokPage.Text));
                return;
            }

            _synthesisSettings.PNLength = pNLength;

            float radioSuppressionCoef;
            Single.TryParse(radioSuppressionTb.Text, out radioSuppressionCoef);

            if (radioSuppressionCoef > 0)
            {
                _synthesisSettings.RadioSuppressionCoef = radioSuppressionCoef;
            }
            else
            {
                FormsHelper.ShowErrorMsg(string.Format("Неверный параметр: {0} ({1})", radioSuppressionLbl.Text, signalPage.Text));
                return;
            }


            float rhgNormalizationCoef;
            if (Single.TryParse(rhgNormalizationCoefTb.Text, out rhgNormalizationCoef))
            {
                _synthesisSettings.RhgNormalizingCoef = rhgNormalizationCoef;
            }
            else
            {
                FormsHelper.ShowErrorMsg(string.Format("Неверный параметр: {0} ({1})", rhgNormalizationCoefLbl.Text, signalPage.Text));
                return;
            }

            float rliNormalizationCoef;
            if (Single.TryParse(rliNormalizationCoefTb.Text, out rliNormalizationCoef))
            {
                _synthesisSettings.RliNormalizingCoef = rliNormalizationCoef;
            }
            else
            {
                FormsHelper.ShowErrorMsg(string.Format("Неверный параметр: {0} ({1})", rliNormalizationCoefLbl.Text, signalPage.Text));
                return;
            }

            if (CanAllocateEnoughMemory(_synthesisSettings, _rhg.Width))
            {
                if (MessageBox.Show("Слишком большой объем выделяемой памяти. Возможны перебои в работе аппаратуры. Продолжить?",
                    "Предупреждение", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }              
            }


            _synthesisSettings.ToXml<RlViewer.Settings.SynthesisSettings>();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private bool CanAllocateEnoughMemory(RlViewer.Settings.SynthesisSettings synthesisSettings, int rangeSamples)
        {
            var hologramSharedMemorySize = (long)synthesisSettings.BlockAzimuthSize * rangeSamples * sizeof(float) * 2;
            var rliSharedMemorySize = (long)(synthesisSettings.FrameAzimuthSize / synthesisSettings.FrameAzimuthCompressionCoef)
                * (long)(rangeSamples / synthesisSettings.FrameRangeCompressionCoef) * sizeof(float);

            var requiredMemory = (ulong)((hologramSharedMemorySize + rliSharedMemorySize) * synthesisSettings.MemoryChunksCount);

            var info = new Microsoft.VisualBasic.Devices.ComputerInfo();

            if (requiredMemory > info.AvailablePhysicalMemory * 0.75f)
            {
                return true;
            }

            return false;
        }



        public Behaviors.Synthesis.ServerSarTaskParams GenerateSstp(int currentBlock, int rangeShift, int azimuthShift)
        {
            var sstp = new Behaviors.Synthesis.ServerSarTaskParams();

            sstp.struct_id = 1;
            sstp.input_dir = new byte[256];
            sstp.input_file = new byte[256];
            sstp.input_file_K1 = new byte[256];
            sstp.output = false;

            var outputDir = Encoding.UTF8.GetBytes(@"c:\vega\temp\obzor");
            var outputDirArr = new byte[256];
            for (int i = 0; i < outputDir.Length; i++)
            {
                outputDirArr[i] = outputDir[i];
            }
            sstp.output_dir = outputDirArr;
            sstp.output_dir_loc = new byte[256];
            var output_fileArr = new byte[256];
            sstp.output_file = output_fileArr;

            var ipArr = new byte[256];

            sstp.Client_ip_address = ipArr;

            sstp.M = 1;
            sstp.ny_start = 0;
            sstp.ny_start_initial = 0;
            sstp.Mshift = _synthesisSettings.FrameAzimuthSize;
            sstp.Mlength = _synthesisSettings.BlockAzimuthSize;
            sstp.Mparts = 1;
            sstp.Mscale = _synthesisSettings.FrameAzimuthCompressionCoef;
            sstp.Mscale_initial = 1;
            sstp.N = currentBlock;
            sstp.nx_start = rangeShift;
            sstp.nx_start_initial = rangeShift;
            sstp.Nshift = _rhg.Width;
            sstp.Nlength = _rhg.Width;
            sstp.Nstripes = 1;
            sstp.Nscale = _synthesisSettings.FrameRangeCompressionCoef;
            sstp.Nscale_initial = 1;
            sstp.lambda = _synthesisSettings.SpeedOfLight / (_headerStruct.transmitterHeader.frequency * 1000000);

            sstp.f0 = _headerStruct.transmitterHeader.freqWidth / 2;
            sstp.Tp = _headerStruct.transmitterHeader.impulseLength / 1000000;
            sstp.dR = _synthesisSettings.SpeedOfLight / (2 * _headerStruct.adcHeader.adcFreq * 1000000 / _headerStruct.adcHeader.freqDivisor);
            sstp.Fimpulses = 1000;
            sstp.Vfly = 300;//holHeaderStruct.synchronizerHeader.azimuthDecompositionStep * sstp.Fimpulses;
            sstp.Rmin = _headerStruct.synchronizerHeader.initialRange * 1000;
            sstp.Angle = _headerStruct.antennaSystemHeader.antennaAngle;
            sstp.Do_range_compress = true;
            sstp.interpolate = true;
            sstp.nLobs = 5;
            sstp.LoopBlocking = true;
            sstp.pNstripes = _rhg.Width / _synthesisSettings.PNLength;
            sstp.pNlength = _synthesisSettings.PNLength;
            sstp.pNshift = _synthesisSettings.PNShift;
            sstp.cutDoppler = _synthesisSettings.UseDopplerFiltering;
            sstp.Doppler_Min = _synthesisSettings.MinDoppler;
            sstp.Doppler_Max = _synthesisSettings.MaxDoppler;
            sstp.simul_clear_input = true;
            sstp.AF_pX = 16384;
            sstp.AF2_ncycle = 1;
            sstp.F1d_Inmem = true;
            sstp.F2d_Inmem = true;
            sstp.Vs_Inmem = true;

            sstp.Azimut_chirp_sign = _headerStruct.synchronizerHeader.azimuthSign != 1 ? -1 : 1;
            sstp.Range_chirp_sign = _headerStruct.synchronizerHeader.rangeSign != 1 ? -1 : 1;
            sstp.Nav_LR_Side = (byte)(_headerStruct.synchronizerHeader.board) == 2 ? -1 : 1;
            sstp.Memory_parts = _synthesisSettings.MemoryChunksCount;
            sstp.AF_ncycle = 4096;
            sstp.AF_WinX = 128;
            sstp.AF_WinY = 1024;
            sstp.AF2_Y_N = false;
            sstp.AF2_percent = 10;

            var afNameArr = new byte[256];
            var afName = Encoding.UTF8.GetBytes("AFTrajDump.dat");
            for (int i = 0; i < afName.Length; i++)
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
            sstp.RadioSuppression = _synthesisSettings.RadioSuppressionCoef;
            sstp.cntPoints = 1;
            sstp.ppx = new int[20];
            sstp.ppy = new int[20];

            sstp.simul_file = new byte[100];
            sstp.Nav_File = new byte[256];
            sstp.XNSum1024YNSum = _synthesisSettings.BlockRangeCompressionCoef * 1024 + _synthesisSettings.BlockAzimuthCompressionCoef;

            sstp.Times_mc = _synthesisSettings.MatrixExtensionCoef;
            sstp.dsp_zone_width = 512;
            sstp.frame_width = _rhg.Width;
            sstp.n_of_frames = 1;

            sstp.Hc = 8529.785156f;

            sstp.RGG_RLI_DSP_numbers = _synthesisSettings.MemoryChunksCount;
            sstp.reserved = new byte[924];
            sstp.Moco1_Range_Dependent = false;
            sstp.Cheb_approx_order = 0;
            sstp.permut_x = false;
            sstp.Ws_wFs = false;
            sstp.Vs_Inmem = true;
            sstp.FromHol_Y_N = false;
            sstp.fbp_moco = false;

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
            else if (e.KeyCode == Keys.Enter)
            {
                ConfirmParams();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            ConfirmParams();
        }

        private void ToggleDopplerControls(bool enabled)
        {
            var useDopplerFiltering = enabled;
            maxDopplerCb.Enabled = useDopplerFiltering;
            minDopplerCb.Enabled = useDopplerFiltering;
        }

        private void useDopplerFilteringCb_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDopplerControls(((CheckBox)sender).Checked);
        }
    }
}
