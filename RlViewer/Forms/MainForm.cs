using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;


namespace RlViewer.Forms
{

    public partial class MainForm : Form
    {

        public MainForm()
        {
            //var mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("SSTP_inSharedMem", (long)(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 20));
            //System.Threading.EventWaitHandle handle = new System.Threading.EventWaitHandle(false, EventResetMode.AutoReset, "SSTP_Ready");
            //handle.WaitOne();

            //var buf = new byte[Marshal.SizeOf(typeof(Behaviors.Synthesis.ServerSarTaskParams))];
            //var mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting("SSTP_inSharedMem");
            //var mmfStr = mmf.CreateViewStream();


            //mmfStr.Read(buf, 0, buf.Length);

            //var sstp = Behaviors.Converters.StructIO.ReadStruct<Behaviors.Synthesis.ServerSarTaskParams>(buf);
            //var asd = sstp;

            //using (var s = new Forms.SynthesisParamsForm(@"C:\Users\lenovo\Desktop\РЛИ\arm12_20160604_s001_001.k"))
            //{

            //}
           // System.Diagnostics.Process.Start(@"C:\Users\lenovo\Desktop\tnk_2\server_sar_base_tcp_x64.exe", "-1 -2");

            var synForm = new Forms.SynthesisParamsForm(@"C:\Users\lenovo\Desktop\РЛИ\arm12_20160604_s001_001.k");
            
                if (synForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                var interop = new Behaviors.Synthesis.ServerSarInterop(@"C:\Users\lenovo\Desktop\tnk_2\server_sar_base_tcp_x64.exe", synForm.GenerateSstp(1, 123123, 0, 0));
                interop.StartSynthesis();
            

            InitializeComponent();
            _guiFacade = new UI.GuiFacade(pictureBox1.Size, action => Invoke(action));
            _guiFacade.OnPointOfViewMaxChanged += (s, e) => CheckScrollBarVisibility();
            _guiFacade.OnProgressVisibilityChanged += (s, e) => InitProgressControls(e.IsVisible);
            _guiFacade.OnProgressChanged += (s, e) => ReportProgress(e.Progress);
            _guiFacade.OnImageDrawn += (s, image) => pictureBox1.Image = image;
            _guiFacade.OnAlignPossibilityChanged += (s, e) => alignBtn.Enabled = e.IsPossible;
            _guiFacade.OnErrorOccured += (s, e) => Forms.FormsHelper.ShowErrorMsg(e.ErrorText);

            _keyProcessor = new UI.KeyPressProcessor(() => _guiFacade.Undo(markPointRb.Checked, markAreaRb.Checked),
                () =>
                {
                    navigationDgv.Rows.Clear();
                    this.Text = OpenFile();
                },
                 () => _guiFacade.Save(), () => ShowFileInfo(), () => ShowLog(),
                 () => MakeReport(), () => _guiFacade.AggregateFiles(), () => EmbedNavigation(),
                 () => _guiFacade.ShowCache());

            InitForm();
            InitDataBindings();

            dragRb.Checked = true;
        }

        private void InitForm()
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;

            AddToolTips();
            navigationDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            alignBtn.Enabled = false;
            filterTrackBar.SmallChange = 1;
            filterTrackBar.LargeChange = 1;
            filterTrackBar.Minimum = -16;
            filterTrackBar.Maximum = 16;
            filterTrackBar.Value = 0;

            horizontalScrollBar.Visible = false;
            verticalScrollBar.Visible = false;

            InitProgressControls(false);
        }

        private void InitDataBindings()
        {
            var scaleBinding = new Binding("Text", _guiFacade, "ScaleFactor", true, DataSourceUpdateMode.OnPropertyChanged);
            scaleBinding.Format += delegate(object sender, ConvertEventArgs convertedArgs)
            {
                convertedArgs.Value = string.Format("Масштаб: {0}%", (Convert.ToSingle(convertedArgs.Value)) * 100);
            };


            scaleLabel.DataBindings.Add(scaleBinding);
            distanceLabel.DataBindings.Add("Text", _guiFacade, "RulerDistance", false, DataSourceUpdateMode.OnPropertyChanged);
            statusLabel.DataBindings.Add("Text", _guiFacade, "CurrentTaskName", false, DataSourceUpdateMode.OnPropertyChanged);
            verticalScrollBar.DataBindings.Add("Value", _guiFacade, "YPointOfView", false, DataSourceUpdateMode.OnPropertyChanged);
            verticalScrollBar.DataBindings.Add("Maximum", _guiFacade, "YPointOfViewMax", false, DataSourceUpdateMode.OnPropertyChanged);
            horizontalScrollBar.DataBindings.Add("Value", _guiFacade, "XPointOfView", false, DataSourceUpdateMode.OnPropertyChanged);
            horizontalScrollBar.DataBindings.Add("Maximum", _guiFacade, "XPointOfViewMax", false, DataSourceUpdateMode.OnPropertyChanged);
        }


        private void ReportProgress(int progress)
        {
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripProgressBar>(progressBar, pb => { pb.Value = progress; });
            ThreadHelper.ThreadSafeUpdateToolStrip<ToolStripStatusLabel>(progressLabel, pl =>
            { pl.Text = string.Format("{0} %", progress.ToString()); });
        }




        private void InitProgressControls(bool isVisible)
        {
            progressBar.Visible = isVisible;
            progressBar.Value = 0;
            statusLabel.Visible = isVisible;
            progressLabel.Visible = isVisible;
            progressLabel.Text = "0%";
            cancelBtn.Visible = isVisible;

        }


        private UI.KeyPressProcessor _keyProcessor;

        private UI.GuiFacade _guiFacade;


        public string OpenFile()
        {
            string caption = this.Text;

            if (!_guiFacade.Settings.UseCustomFileOpenDlg)
            {
                using (var openFileDlg = new OpenFileDialog() { Filter = Resources.OpenFilter })
                {
                    openFileDlg.Title = "Радиолокационный файл";
                    if (openFileDlg.ShowDialog() == DialogResult.OK)
                    {
                        caption = _guiFacade.OpenFile(openFileDlg.FileName);
                    }
                }
            }
            else
            {
                caption = _guiFacade.OpenFileCustomDialog();
            }

            return caption;
        }








        private void ToggleNavigation()
        {
            TogglePanel(navigationPanelCb.Checked, naviSplitter);
        }

        private void ToggleFilters()
        {
            TogglePanel(filterPanelCb.Checked, filterSplitter);
        }

        private void TogglePanel(bool isPanelOpen, SplitContainer sp)
        {
            if (isPanelOpen)
            {
                sp.Panel2Collapsed = false;
            }
            else
            {
                sp.Panel2Collapsed = true;
            }
            _guiFacade.InitDrawing();
        }

        private bool _rightBtnPressed = false;
        private Cursor _toolCursor;

        private void MouseClickStarted(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _rightBtnPressed = true;
            }


            if (dragRb.Checked)
            {
                pictureBox1.Cursor = Cursors.SizeAll;
                _guiFacade.DragStart(e.Location);
                return;
            }

            if (_rightBtnPressed)
            {
                pictureBox1.Cursor = Cursors.SizeAll;
                _guiFacade.DragStart(e.Location);
                return;
            }

            if (analyzeRb.Checked)
            {
                _guiFacade.GetPointAmplitudeStart(e.Location, pictureBox1);
            }
            else
            {
                if (markAreaRb.Checked)
                {
                    _guiFacade.SelectAreaStart(e.Location);
                }
                else if (sharerRb.Checked)
                {
                    _guiFacade.ShareCoords(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    _guiFacade.SelectPointStart(e.Location);
                }

                else if (verticalSectionRb.Checked)
                {
                    _guiFacade.VerticalSectionStart(e.Location);
                }
                else if (horizontalSectionRb.Checked)
                {
                    _guiFacade.HorizontalSectionStart(e.Location);
                }
                else if (linearSectionRb.Checked)
                {
                    _guiFacade.LinearSectionStart(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    _guiFacade.RulerStart(e.Location);
                }
            }
        }

        private void MouseMoving(MouseEventArgs e)
        {
            if (dragRb.Checked || _rightBtnPressed)
            {
                _guiFacade.Drag(e.Location);
            }
            else
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    _guiFacade.Drag(e.Location);
                }

                if (markAreaRb.Checked)
                {
                    _guiFacade.SelectArea(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    _guiFacade.SelectPoint(e.Location);
                }
                else if (analyzeRb.Checked)
                {
                    _guiFacade.GetPointAmplitude(e.Location, pictureBox1);
                }
                else if (verticalSectionRb.Checked)
                {
                    _guiFacade.VerticalSection(e.Location);
                }
                else if (horizontalSectionRb.Checked)
                {
                    _guiFacade.HorizontalSection(e.Location);
                }
                else if (linearSectionRb.Checked)
                {
                    _guiFacade.LinearSection(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    _guiFacade.GetDistance(e.Location);
                }
            }

        }

        private void MouseClickFinished(MouseEventArgs e)
        {
            _guiFacade.DragFinish();
            _rightBtnPressed = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (markAreaRb.Checked)
                {
                    _guiFacade.SelectAreaFinish(e.Location);
                }
                else if (markPointRb.Checked)
                {
                    _guiFacade.SelectPointFinish(e.Location);
                }
                else if (analyzeRb.Checked)
                {
                    _guiFacade.StopPointAnalyzer(pictureBox1);
                }
                else if (verticalSectionRb.Checked || horizontalSectionRb.Checked || linearSectionRb.Checked)
                {
                    ShowSection(e.Location);
                }
                else if (rulerRb.Checked)
                {
                    _guiFacade.GetDistance(e.Location);
                }
            }

            pictureBox1.Cursor = _toolCursor;
        }



        private string GetSectionFormCaption()
        {
            string caption = string.Empty;

            if (horizontalSectionRb.Checked)
            {
                caption = "Горизонтальное сечение";
            }
            else if (verticalSectionRb.Checked)
            {
                caption = "Вертикальное сечение";
            }
            else if (linearSectionRb.Checked)
            {
                caption = "Произвольное сечение";
            }

            return caption;
        }


        private void ShowSection(Point mouseCoords)
        {
            using (var sectionForm = new Forms.SectionGraphForm(_guiFacade.GetSectionInfo(mouseCoords), GetSectionFormCaption()))
            {
                sectionForm.ShowDialog();
            }
        }


        private void CheckScrollBarVisibility()
        {
            horizontalScrollBar.Visible = horizontalScrollBar.Maximum > 0 ? true : false;
            verticalScrollBar.Visible = verticalScrollBar.Maximum > 0 ? true : false;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            navigationDgv.Rows.Clear();
            Text = OpenFile();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            _guiFacade.DrawImage();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            _guiFacade.DrawImage();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _guiFacade.InitDrawing(canvasSize: pictureBox1.Size);
        }

        public void MakeReport()
        {
            var reporterSettings = RlViewer.Settings.Settings.LoadSettings<Settings.ReporterSettings>();

            using (var reportSettingsForm = new Forms.ReportSettingsForm(reporterSettings))
            {
                if (reportSettingsForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                reporterSettings.ToXml<Settings.ReporterSettings>();

                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Файлы для формирования отчета";
                    ofd.Multiselect = true;
                    ofd.Filter = Resources.OpenFilter;
                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    using (var fsd = new SaveFileDialog())
                    {
                        fsd.Title = "Имя для файла отчета";
                        fsd.Filter = "Документ MS Word|*.docx";
                        if (fsd.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        _guiFacade.MakeReport(ofd.FileNames, fsd.FileName, reporterSettings);
                    }
                }
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMoving(e);
            coordinatesLabel.Text = _guiFacade.ShowMousePosition(e.Location);

            if (navigationPanelCb.Checked)
            {
                var currentNavigation = _guiFacade.ShowNavigation(e);
                if (currentNavigation != null)
                {
                    navigationDgv.Rows.Clear();
                    foreach (var item in currentNavigation)
                    {
                        navigationDgv.Rows.Add(item.ParameterName, item.ParameterValue);
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseClickStarted(e);
            _guiFacade.DrawImage();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MouseClickFinished(e);
            _guiFacade.DrawImage();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            _guiFacade.ScaleImage(e.Delta, e.Location);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Focus();
        }


        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _guiFacade.ChangeFilterValue(filterTrackBar.Value);
            filterLbl.Text = string.Format("Уровень фильтра: {0}", filterTrackBar.Value);
        }

        private void contrastRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                filterTrackBar.Value = _guiFacade.GetFilter(Behaviors.Filters.FilterType.Contrast, 4);
            }
        }

        private void gammaCorrRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                filterTrackBar.Value = _guiFacade.GetFilter(Behaviors.Filters.FilterType.GammaCorrection, 0);
            }
        }

        private void brightnessRb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                filterTrackBar.Value = _guiFacade.GetFilter(Behaviors.Filters.FilterType.Brightness, 4);
            }
        }


        private void ShowSettings()
        {
            using (var settgingsForm = new Forms.SettingsForm(_guiFacade.Settings))
            {
                if (settgingsForm.ShowDialog() == DialogResult.OK)
                {
                    _guiFacade.InitDrawing(true);
                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _guiFacade.CancelLoading();
            _guiFacade.Dispose();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _keyProcessor.ProcessKeyPress(e);
        }

        private void ShowFileInfo()
        {
            var info = _guiFacade.GetFileInfo();

            if (info == null)
            {
                return;
            }

            using (var iFrm = new Forms.InfoForm(info))
            {
                iFrm.ShowDialog();
            }
        }


        private void оФайлеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFileInfo();
        }


        private void ShowLog()
        {
            using (var logForm = new Forms.LogForm())
            {
                logForm.ShowDialog();
            }
        }



        private void логToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.Save();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _guiFacade.InitDrawing();
        }

        private void naviPanelCb_CheckedChanged(object sender, EventArgs e)
        {
            ToggleNavigation();
        }

        private void filterPanelCb_CheckedChanged(object sender, EventArgs e)
        {
            ToggleFilters();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = _guiFacade.OpenWithDoubleClick();
        }


        private void alignBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ResampleImage();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            _guiFacade.CancelLoading();
        }

        private void resetFilterBtn_Click(object sender, EventArgs e)
        {
            filterTrackBar.Value = 0;
            _guiFacade.ResetFilter();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            _guiFacade.MoveFileDragDrop(e);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            navigationDgv.Rows.Clear();
            Text = _guiFacade.OpenFileDragDrop(e);
        }


        private void ShowAbout()
        {
            using (var about = new Forms.About())
            {
                about.ShowDialog();
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        private void статусКешаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowCache();
        }

        private void findPointBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ShowFindPoint();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _guiFacade.InitDrawing();
        }

        private void rulerRb_CheckedChanged(object sender, EventArgs e)
        {
            _guiFacade.ResetRuler();
            distanceLabel.Text = string.Empty;
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ScaleImage(-1);
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.ScaleImage(1);
        }

        private void создатьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeReport();
        }

        private void statisticsBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.GetAreaStatistics();
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }



        private void EmbedNavigation()
        {
            using (var destFileOpenDlg = new OpenFileDialog() { Title = "Выберите исходный файл РЛИ", Filter = Resources.NaviEmbeddingFilterDest })
            {
                if (destFileOpenDlg.ShowDialog() == DialogResult.OK)
                {
                    using (var sourceFileOpenDlg = new OpenFileDialog() { Title = "Выберите исходный файл РГГ", Filter = Resources.NaviEmbeddingFilterSource })
                    {
                        if (sourceFileOpenDlg.ShowDialog() == DialogResult.OK)
                        {
                            _guiFacade.EmbedNavigation(sourceFileOpenDlg.FileName, destFileOpenDlg.FileName);
                        }
                    }
                }
            }
        }

        private void вшитьНавигациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmbedNavigation();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            _guiFacade.DrawItems(e.Graphics);
        }

        private void совместитьФайлыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _guiFacade.AggregateFiles();
        }

        private void mirrorImageBtn_Click(object sender, EventArgs e)
        {
            _guiFacade.MirrorImage();
        }


        private void AddToolTips()
        {
            Forms.FormsHelper.AddToolTip(alignBtn, "Выровнять");
            Forms.FormsHelper.AddToolTip(analyzeRb, "Анализ амплитуды");
            Forms.FormsHelper.AddToolTip(dragRb, "Перемещение по изображению");
            Forms.FormsHelper.AddToolTip(horizontalSectionRb, "Горизонтальное сечение");
            Forms.FormsHelper.AddToolTip(linearSectionRb, "Произвольное сечение");
            Forms.FormsHelper.AddToolTip(markAreaRb, "Область");
            Forms.FormsHelper.AddToolTip(markPointRb, "Отметка");
            Forms.FormsHelper.AddToolTip(navigationPanelCb, "Навигация");
            Forms.FormsHelper.AddToolTip(rulerRb, "Линейка");
            Forms.FormsHelper.AddToolTip(findPointBtn, "Поиск точки");
            Forms.FormsHelper.AddToolTip(verticalSectionRb, "Вертикальное сечение");
            Forms.FormsHelper.AddToolTip(brightnessRb, "Яркость");
            Forms.FormsHelper.AddToolTip(contrastRb, "Контрастность");
            Forms.FormsHelper.AddToolTip(gammaCorrRb, "Гамма");
            Forms.FormsHelper.AddToolTip(resetFilterBtn, "Сброс фильтров");
            Forms.FormsHelper.AddToolTip(filterPanelCb, "Фильтры");
            Forms.FormsHelper.AddToolTip(zoomInBtn, "Увеличить масштаб");
            Forms.FormsHelper.AddToolTip(zoomOutBtn, "Уменьшить масштаб");
            Forms.FormsHelper.AddToolTip(statisticsBtn, "Статистика");
            Forms.FormsHelper.AddToolTip(squareAreaRb, "Трехмерный график");
            Forms.FormsHelper.AddToolTip(sharerRb, "Сравнить точки");
            Forms.FormsHelper.AddToolTip(mirrorImageBtn, "Отразить изображение");
        }

        private void sharerRb_CheckedChanged(object sender, EventArgs e)
        {
            _guiFacade.AllowRemoteDataReceiving = ((RadioButton)sender).Checked;
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

        private void dragRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

        private void markPointRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

        private void markAreaRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

        private void analyzeRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Cross;
            _toolCursor = Cursors.Cross;
        }

        private void verticalSectionRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

        private void horizontalSectionRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

        private void linearSectionRb_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Arrow;
            _toolCursor = Cursors.Arrow;
        }

    }
}
