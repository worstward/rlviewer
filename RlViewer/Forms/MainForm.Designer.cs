namespace RlViewer.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.horizontalScrollBar = new System.Windows.Forms.HScrollBar();
            this.verticalScrollBar = new System.Windows.Forms.VScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьОтчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вшитьНавигациюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.совместитьФайлыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оФайлеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.логToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.статусКешаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.logCorrectionRb = new System.Windows.Forms.RadioButton();
            this.resetFilterBtn = new System.Windows.Forms.Button();
            this.gammaCorrRb = new System.Windows.Forms.RadioButton();
            this.contrastRb = new System.Windows.Forms.RadioButton();
            this.brightnessRb = new System.Windows.Forms.RadioButton();
            this.filterLbl = new System.Windows.Forms.Label();
            this.filterTrackBar = new System.Windows.Forms.TrackBar();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.naviSplitter = new System.Windows.Forms.SplitContainer();
            this.filterSplitter = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.navigationDgv = new System.Windows.Forms.DataGridView();
            this.paramColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.dragRb = new System.Windows.Forms.RadioButton();
            this.markPointRb = new System.Windows.Forms.RadioButton();
            this.alignBtn = new System.Windows.Forms.Button();
            this.markAreaRb = new System.Windows.Forms.RadioButton();
            this.analyzeRb = new System.Windows.Forms.RadioButton();
            this.verticalSectionRb = new System.Windows.Forms.RadioButton();
            this.horizontalSectionRb = new System.Windows.Forms.RadioButton();
            this.linearSectionRb = new System.Windows.Forms.RadioButton();
            this.rulerRb = new System.Windows.Forms.RadioButton();
            this.findPointBtn = new System.Windows.Forms.Button();
            this.statisticsBtn = new System.Windows.Forms.Button();
            this.sharerRb = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.mirrorImageBtn = new System.Windows.Forms.Button();
            this.zoomInBtn = new System.Windows.Forms.Button();
            this.zoomOutBtn = new System.Windows.Forms.Button();
            this.navigationPanelCb = new System.Windows.Forms.CheckBox();
            this.filterPanelCb = new System.Windows.Forms.CheckBox();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.coordinatesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.distanceLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.cancelBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.squareAreaRb = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviSplitter)).BeginInit();
            this.naviSplitter.Panel1.SuspendLayout();
            this.naviSplitter.Panel2.SuspendLayout();
            this.naviSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterSplitter)).BeginInit();
            this.filterSplitter.Panel1.SuspendLayout();
            this.filterSplitter.Panel2.SuspendLayout();
            this.filterSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigationDgv)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // horizontalScrollBar
            // 
            this.horizontalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBar.Location = new System.Drawing.Point(17, 611);
            this.horizontalScrollBar.Name = "horizontalScrollBar";
            this.horizontalScrollBar.Size = new System.Drawing.Size(872, 17);
            this.horizontalScrollBar.TabIndex = 3;
            this.horizontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // verticalScrollBar
            // 
            this.verticalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBar.Location = new System.Drawing.Point(889, 93);
            this.verticalScrollBar.Name = "verticalScrollBar";
            this.verticalScrollBar.Size = new System.Drawing.Size(17, 515);
            this.verticalScrollBar.TabIndex = 4;
            this.verticalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip1.Size = new System.Drawing.Size(915, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.создатьОтчетToolStripMenuItem,
            this.вшитьНавигациюToolStripMenuItem,
            this.совместитьФайлыToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // создатьОтчетToolStripMenuItem
            // 
            this.создатьОтчетToolStripMenuItem.Name = "создатьОтчетToolStripMenuItem";
            this.создатьОтчетToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.создатьОтчетToolStripMenuItem.Text = "Создать отчет";
            this.создатьОтчетToolStripMenuItem.Click += new System.EventHandler(this.создатьОтчетToolStripMenuItem_Click);
            // 
            // вшитьНавигациюToolStripMenuItem
            // 
            this.вшитьНавигациюToolStripMenuItem.Name = "вшитьНавигациюToolStripMenuItem";
            this.вшитьНавигациюToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.вшитьНавигациюToolStripMenuItem.Text = "Вшить навигацию";
            this.вшитьНавигациюToolStripMenuItem.Click += new System.EventHandler(this.вшитьНавигациюToolStripMenuItem_Click);
            // 
            // совместитьФайлыToolStripMenuItem
            // 
            this.совместитьФайлыToolStripMenuItem.Name = "совместитьФайлыToolStripMenuItem";
            this.совместитьФайлыToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.совместитьФайлыToolStripMenuItem.Text = "Совместить файлы";
            this.совместитьФайлыToolStripMenuItem.Click += new System.EventHandler(this.совместитьФайлыToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.settingsToolStripMenuItem.Text = "Настройки";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оФайлеToolStripMenuItem,
            this.логToolStripMenuItem1,
            this.статусКешаToolStripMenuItem,
            this.toolStripSeparator1,
            this.оПрограммеToolStripMenuItem});
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.infoToolStripMenuItem.Text = "Инфо";
            // 
            // оФайлеToolStripMenuItem
            // 
            this.оФайлеToolStripMenuItem.Name = "оФайлеToolStripMenuItem";
            this.оФайлеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оФайлеToolStripMenuItem.Text = "О файле";
            this.оФайлеToolStripMenuItem.Click += new System.EventHandler(this.оФайлеToolStripMenuItem_Click);
            // 
            // логToolStripMenuItem1
            // 
            this.логToolStripMenuItem1.Name = "логToolStripMenuItem1";
            this.логToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.логToolStripMenuItem1.Text = "Лог";
            this.логToolStripMenuItem1.Click += new System.EventHandler(this.логToolStripMenuItem1_Click);
            // 
            // статусКешаToolStripMenuItem
            // 
            this.статусКешаToolStripMenuItem.Name = "статусКешаToolStripMenuItem";
            this.статусКешаToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.статусКешаToolStripMenuItem.Text = "Статус кеша";
            this.статусКешаToolStripMenuItem.Click += new System.EventHandler(this.статусКешаToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.logCorrectionRb);
            this.groupBox2.Controls.Add(this.resetFilterBtn);
            this.groupBox2.Controls.Add(this.gammaCorrRb);
            this.groupBox2.Controls.Add(this.contrastRb);
            this.groupBox2.Controls.Add(this.brightnessRb);
            this.groupBox2.Controls.Add(this.filterLbl);
            this.groupBox2.Controls.Add(this.filterTrackBar);
            this.groupBox2.Location = new System.Drawing.Point(9, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 98);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Фильтр";
            // 
            // logCorrectionRb
            // 
            this.logCorrectionRb.Location = new System.Drawing.Point(0, 0);
            this.logCorrectionRb.Name = "logCorrectionRb";
            this.logCorrectionRb.Size = new System.Drawing.Size(104, 24);
            this.logCorrectionRb.TabIndex = 0;
            // 
            // resetFilterBtn
            // 
            this.resetFilterBtn.BackColor = System.Drawing.Color.Transparent;
            this.resetFilterBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resetFilterBtn.BackgroundImage")));
            this.resetFilterBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.resetFilterBtn.FlatAppearance.BorderSize = 0;
            this.resetFilterBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.resetFilterBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetFilterBtn.Location = new System.Drawing.Point(296, 35);
            this.resetFilterBtn.Name = "resetFilterBtn";
            this.resetFilterBtn.Size = new System.Drawing.Size(30, 30);
            this.resetFilterBtn.TabIndex = 13;
            this.resetFilterBtn.UseVisualStyleBackColor = false;
            this.resetFilterBtn.Click += new System.EventHandler(this.resetFilterBtn_Click);
            // 
            // gammaCorrRb
            // 
            this.gammaCorrRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.gammaCorrRb.BackgroundImage = global::RlViewer.Properties.Resources.Gamma;
            this.gammaCorrRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gammaCorrRb.FlatAppearance.BorderSize = 0;
            this.gammaCorrRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gammaCorrRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gammaCorrRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gammaCorrRb.Location = new System.Drawing.Point(77, 35);
            this.gammaCorrRb.Name = "gammaCorrRb";
            this.gammaCorrRb.Size = new System.Drawing.Size(30, 30);
            this.gammaCorrRb.TabIndex = 2;
            this.gammaCorrRb.TabStop = true;
            this.gammaCorrRb.UseVisualStyleBackColor = true;
            this.gammaCorrRb.CheckedChanged += new System.EventHandler(this.gammaCorrRb_CheckedChanged);
            // 
            // contrastRb
            // 
            this.contrastRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.contrastRb.BackgroundImage = global::RlViewer.Properties.Resources.Contrast;
            this.contrastRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.contrastRb.FlatAppearance.BorderSize = 0;
            this.contrastRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.contrastRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.contrastRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contrastRb.Location = new System.Drawing.Point(44, 35);
            this.contrastRb.Name = "contrastRb";
            this.contrastRb.Size = new System.Drawing.Size(30, 30);
            this.contrastRb.TabIndex = 1;
            this.contrastRb.TabStop = true;
            this.contrastRb.UseVisualStyleBackColor = true;
            this.contrastRb.CheckedChanged += new System.EventHandler(this.contrastRb_CheckedChanged);
            // 
            // brightnessRb
            // 
            this.brightnessRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.brightnessRb.BackgroundImage = global::RlViewer.Properties.Resources.Brightness;
            this.brightnessRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.brightnessRb.Checked = true;
            this.brightnessRb.FlatAppearance.BorderSize = 0;
            this.brightnessRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.brightnessRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.brightnessRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brightnessRb.Location = new System.Drawing.Point(11, 35);
            this.brightnessRb.Name = "brightnessRb";
            this.brightnessRb.Size = new System.Drawing.Size(30, 30);
            this.brightnessRb.TabIndex = 0;
            this.brightnessRb.TabStop = true;
            this.brightnessRb.UseVisualStyleBackColor = true;
            this.brightnessRb.CheckedChanged += new System.EventHandler(this.brightnessRb_CheckedChanged);
            // 
            // filterLbl
            // 
            this.filterLbl.AutoSize = true;
            this.filterLbl.Location = new System.Drawing.Point(153, 55);
            this.filterLbl.Name = "filterLbl";
            this.filterLbl.Size = new System.Drawing.Size(109, 13);
            this.filterLbl.TabIndex = 11;
            this.filterLbl.Text = "Уровень фильтра: 0";
            // 
            // filterTrackBar
            // 
            this.filterTrackBar.AutoSize = false;
            this.filterTrackBar.Location = new System.Drawing.Point(143, 33);
            this.filterTrackBar.Name = "filterTrackBar";
            this.filterTrackBar.Size = new System.Drawing.Size(135, 32);
            this.filterTrackBar.TabIndex = 10;
            this.filterTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.filterTrackBar.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // percentageLabel
            // 
            this.percentageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.percentageLabel.AutoSize = true;
            this.percentageLabel.BackColor = System.Drawing.Color.Transparent;
            this.percentageLabel.Location = new System.Drawing.Point(691, 628);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(24, 13);
            this.percentageLabel.TabIndex = 18;
            this.percentageLabel.Text = "0 %";
            // 
            // naviSplitter
            // 
            this.naviSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.naviSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.naviSplitter.IsSplitterFixed = true;
            this.naviSplitter.Location = new System.Drawing.Point(17, 89);
            this.naviSplitter.Name = "naviSplitter";
            // 
            // naviSplitter.Panel1
            // 
            this.naviSplitter.Panel1.Controls.Add(this.filterSplitter);
            // 
            // naviSplitter.Panel2
            // 
            this.naviSplitter.Panel2.Controls.Add(this.navigationDgv);
            this.naviSplitter.Panel2Collapsed = true;
            this.naviSplitter.Size = new System.Drawing.Size(872, 519);
            this.naviSplitter.SplitterDistance = 649;
            this.naviSplitter.SplitterWidth = 6;
            this.naviSplitter.TabIndex = 19;
            this.naviSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // filterSplitter
            // 
            this.filterSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.filterSplitter.IsSplitterFixed = true;
            this.filterSplitter.Location = new System.Drawing.Point(0, 0);
            this.filterSplitter.Name = "filterSplitter";
            this.filterSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // filterSplitter.Panel1
            // 
            this.filterSplitter.Panel1.Controls.Add(this.pictureBox1);
            // 
            // filterSplitter.Panel2
            // 
            this.filterSplitter.Panel2.Controls.Add(this.chart1);
            this.filterSplitter.Panel2.Controls.Add(this.groupBox2);
            this.filterSplitter.Panel2Collapsed = true;
            this.filterSplitter.Size = new System.Drawing.Size(872, 519);
            this.filterSplitter.SplitterDistance = 365;
            this.filterSplitter.SplitterWidth = 6;
            this.filterSplitter.TabIndex = 0;
            this.filterSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(866, 513);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chart1.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(358, 3);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.IsVisibleInLegend = false;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(298, 98);
            this.chart1.TabIndex = 10;
            this.chart1.Text = "chart1";
            // 
            // navigationDgv
            // 
            this.navigationDgv.AllowUserToAddRows = false;
            this.navigationDgv.AllowUserToDeleteRows = false;
            this.navigationDgv.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.navigationDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.navigationDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.navigationDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.paramColumn,
            this.valueColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.navigationDgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.navigationDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationDgv.Location = new System.Drawing.Point(0, 0);
            this.navigationDgv.MultiSelect = false;
            this.navigationDgv.Name = "navigationDgv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.navigationDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.navigationDgv.RowHeadersVisible = false;
            this.navigationDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.navigationDgv.Size = new System.Drawing.Size(96, 100);
            this.navigationDgv.TabIndex = 0;
            // 
            // paramColumn
            // 
            this.paramColumn.HeaderText = "Параметр";
            this.paramColumn.Name = "paramColumn";
            this.paramColumn.ReadOnly = true;
            this.paramColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // valueColumn
            // 
            this.valueColumn.HeaderText = "Значение";
            this.valueColumn.Name = "valueColumn";
            this.valueColumn.ReadOnly = true;
            this.valueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.dragRb);
            this.flowLayoutPanel1.Controls.Add(this.markPointRb);
            this.flowLayoutPanel1.Controls.Add(this.alignBtn);
            this.flowLayoutPanel1.Controls.Add(this.markAreaRb);
            this.flowLayoutPanel1.Controls.Add(this.analyzeRb);
            this.flowLayoutPanel1.Controls.Add(this.verticalSectionRb);
            this.flowLayoutPanel1.Controls.Add(this.horizontalSectionRb);
            this.flowLayoutPanel1.Controls.Add(this.linearSectionRb);
            this.flowLayoutPanel1.Controls.Add(this.rulerRb);
            this.flowLayoutPanel1.Controls.Add(this.findPointBtn);
            this.flowLayoutPanel1.Controls.Add(this.statisticsBtn);
            this.flowLayoutPanel1.Controls.Add(this.sharerRb);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.mirrorImageBtn);
            this.flowLayoutPanel1.Controls.Add(this.zoomInBtn);
            this.flowLayoutPanel1.Controls.Add(this.zoomOutBtn);
            this.flowLayoutPanel1.Controls.Add(this.navigationPanelCb);
            this.flowLayoutPanel1.Controls.Add(this.filterPanelCb);
            this.flowLayoutPanel1.Controls.Add(this.scaleLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 26);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(894, 40);
            this.flowLayoutPanel1.TabIndex = 27;
            // 
            // dragRb
            // 
            this.dragRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.dragRb.BackColor = System.Drawing.Color.Transparent;
            this.dragRb.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dragRb.BackgroundImage")));
            this.dragRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.dragRb.Checked = true;
            this.dragRb.FlatAppearance.BorderSize = 0;
            this.dragRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.dragRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dragRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dragRb.Location = new System.Drawing.Point(3, 3);
            this.dragRb.Name = "dragRb";
            this.dragRb.Size = new System.Drawing.Size(30, 30);
            this.dragRb.TabIndex = 5;
            this.dragRb.TabStop = true;
            this.dragRb.UseVisualStyleBackColor = false;
            // 
            // markPointRb
            // 
            this.markPointRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.markPointRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.markPointRb.FlatAppearance.BorderSize = 0;
            this.markPointRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.markPointRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.markPointRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.markPointRb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.markPointRb.Image = ((System.Drawing.Image)(resources.GetObject("markPointRb.Image")));
            this.markPointRb.Location = new System.Drawing.Point(39, 3);
            this.markPointRb.Name = "markPointRb";
            this.markPointRb.Size = new System.Drawing.Size(30, 30);
            this.markPointRb.TabIndex = 6;
            this.markPointRb.UseVisualStyleBackColor = true;
            // 
            // alignBtn
            // 
            this.alignBtn.BackgroundImage = global::RlViewer.Properties.Resources.Align;
            this.alignBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.alignBtn.FlatAppearance.BorderSize = 0;
            this.alignBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.alignBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.alignBtn.Location = new System.Drawing.Point(75, 3);
            this.alignBtn.Name = "alignBtn";
            this.alignBtn.Size = new System.Drawing.Size(30, 30);
            this.alignBtn.TabIndex = 11;
            this.alignBtn.UseVisualStyleBackColor = true;
            this.alignBtn.Click += new System.EventHandler(this.alignBtn_Click);
            // 
            // markAreaRb
            // 
            this.markAreaRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.markAreaRb.BackColor = System.Drawing.Color.Transparent;
            this.markAreaRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.markAreaRb.FlatAppearance.BorderSize = 0;
            this.markAreaRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.markAreaRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.markAreaRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.markAreaRb.Image = ((System.Drawing.Image)(resources.GetObject("markAreaRb.Image")));
            this.markAreaRb.Location = new System.Drawing.Point(111, 3);
            this.markAreaRb.Name = "markAreaRb";
            this.markAreaRb.Size = new System.Drawing.Size(30, 30);
            this.markAreaRb.TabIndex = 7;
            this.markAreaRb.UseVisualStyleBackColor = false;
            // 
            // analyzeRb
            // 
            this.analyzeRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.analyzeRb.BackColor = System.Drawing.Color.Transparent;
            this.analyzeRb.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("analyzeRb.BackgroundImage")));
            this.analyzeRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.analyzeRb.FlatAppearance.BorderSize = 0;
            this.analyzeRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.analyzeRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.analyzeRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.analyzeRb.Location = new System.Drawing.Point(147, 3);
            this.analyzeRb.Name = "analyzeRb";
            this.analyzeRb.Size = new System.Drawing.Size(30, 30);
            this.analyzeRb.TabIndex = 8;
            this.analyzeRb.UseVisualStyleBackColor = false;
            // 
            // verticalSectionRb
            // 
            this.verticalSectionRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.verticalSectionRb.BackColor = System.Drawing.Color.Transparent;
            this.verticalSectionRb.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalSectionRb.BackgroundImage")));
            this.verticalSectionRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.verticalSectionRb.FlatAppearance.BorderSize = 0;
            this.verticalSectionRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.verticalSectionRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.verticalSectionRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.verticalSectionRb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.verticalSectionRb.Location = new System.Drawing.Point(183, 3);
            this.verticalSectionRb.Name = "verticalSectionRb";
            this.verticalSectionRb.Size = new System.Drawing.Size(30, 30);
            this.verticalSectionRb.TabIndex = 10;
            this.verticalSectionRb.UseVisualStyleBackColor = false;
            // 
            // horizontalSectionRb
            // 
            this.horizontalSectionRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.horizontalSectionRb.BackColor = System.Drawing.Color.Transparent;
            this.horizontalSectionRb.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalSectionRb.BackgroundImage")));
            this.horizontalSectionRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.horizontalSectionRb.FlatAppearance.BorderSize = 0;
            this.horizontalSectionRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.horizontalSectionRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.horizontalSectionRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.horizontalSectionRb.Location = new System.Drawing.Point(219, 3);
            this.horizontalSectionRb.Name = "horizontalSectionRb";
            this.horizontalSectionRb.Size = new System.Drawing.Size(30, 30);
            this.horizontalSectionRb.TabIndex = 9;
            this.horizontalSectionRb.UseVisualStyleBackColor = false;
            // 
            // linearSectionRb
            // 
            this.linearSectionRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.linearSectionRb.BackColor = System.Drawing.Color.Transparent;
            this.linearSectionRb.BackgroundImage = global::RlViewer.Properties.Resources.LinearSection;
            this.linearSectionRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.linearSectionRb.FlatAppearance.BorderSize = 0;
            this.linearSectionRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linearSectionRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.linearSectionRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.linearSectionRb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.linearSectionRb.Location = new System.Drawing.Point(255, 3);
            this.linearSectionRb.Name = "linearSectionRb";
            this.linearSectionRb.Size = new System.Drawing.Size(30, 30);
            this.linearSectionRb.TabIndex = 14;
            this.linearSectionRb.UseVisualStyleBackColor = false;
            // 
            // rulerRb
            // 
            this.rulerRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.rulerRb.BackColor = System.Drawing.Color.Transparent;
            this.rulerRb.BackgroundImage = global::RlViewer.Properties.Resources.Ruler;
            this.rulerRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rulerRb.FlatAppearance.BorderSize = 0;
            this.rulerRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.rulerRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rulerRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rulerRb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rulerRb.Location = new System.Drawing.Point(291, 3);
            this.rulerRb.Name = "rulerRb";
            this.rulerRb.Size = new System.Drawing.Size(30, 30);
            this.rulerRb.TabIndex = 12;
            this.rulerRb.UseVisualStyleBackColor = false;
            this.rulerRb.CheckedChanged += new System.EventHandler(this.rulerRb_CheckedChanged);
            // 
            // findPointBtn
            // 
            this.findPointBtn.BackgroundImage = global::RlViewer.Properties.Resources.FindPoint;
            this.findPointBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.findPointBtn.FlatAppearance.BorderSize = 0;
            this.findPointBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.findPointBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findPointBtn.Location = new System.Drawing.Point(327, 3);
            this.findPointBtn.Name = "findPointBtn";
            this.findPointBtn.Size = new System.Drawing.Size(30, 30);
            this.findPointBtn.TabIndex = 13;
            this.findPointBtn.UseVisualStyleBackColor = true;
            this.findPointBtn.Click += new System.EventHandler(this.findPointBtn_Click);
            // 
            // statisticsBtn
            // 
            this.statisticsBtn.BackgroundImage = global::RlViewer.Properties.Resources.stat;
            this.statisticsBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.statisticsBtn.FlatAppearance.BorderSize = 0;
            this.statisticsBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.statisticsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.statisticsBtn.Location = new System.Drawing.Point(363, 3);
            this.statisticsBtn.Name = "statisticsBtn";
            this.statisticsBtn.Size = new System.Drawing.Size(30, 30);
            this.statisticsBtn.TabIndex = 15;
            this.statisticsBtn.UseVisualStyleBackColor = true;
            this.statisticsBtn.Click += new System.EventHandler(this.statisticsBtn_Click);
            // 
            // sharerRb
            // 
            this.sharerRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.sharerRb.BackColor = System.Drawing.Color.Transparent;
            this.sharerRb.BackgroundImage = global::RlViewer.Properties.Resources.Sharer1;
            this.sharerRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.sharerRb.FlatAppearance.BorderSize = 0;
            this.sharerRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.sharerRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.sharerRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sharerRb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sharerRb.Location = new System.Drawing.Point(399, 3);
            this.sharerRb.Name = "sharerRb";
            this.sharerRb.Size = new System.Drawing.Size(30, 30);
            this.sharerRb.TabIndex = 26;
            this.sharerRb.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(435, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 27;
            // 
            // mirrorImageBtn
            // 
            this.mirrorImageBtn.BackgroundImage = global::RlViewer.Properties.Resources.mirror;
            this.mirrorImageBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mirrorImageBtn.FlatAppearance.BorderSize = 0;
            this.mirrorImageBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.mirrorImageBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mirrorImageBtn.Location = new System.Drawing.Point(441, 3);
            this.mirrorImageBtn.Name = "mirrorImageBtn";
            this.mirrorImageBtn.Size = new System.Drawing.Size(30, 30);
            this.mirrorImageBtn.TabIndex = 28;
            this.mirrorImageBtn.UseVisualStyleBackColor = true;
            this.mirrorImageBtn.Click += new System.EventHandler(this.mirrorImageBtn_Click);
            // 
            // zoomInBtn
            // 
            this.zoomInBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomInBtn.BackgroundImage = global::RlViewer.Properties.Resources.ZoomIn;
            this.zoomInBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.zoomInBtn.FlatAppearance.BorderSize = 0;
            this.zoomInBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.zoomInBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomInBtn.Location = new System.Drawing.Point(477, 3);
            this.zoomInBtn.Name = "zoomInBtn";
            this.zoomInBtn.Size = new System.Drawing.Size(30, 30);
            this.zoomInBtn.TabIndex = 25;
            this.zoomInBtn.UseVisualStyleBackColor = true;
            this.zoomInBtn.Click += new System.EventHandler(this.zoomInBtn_Click);
            // 
            // zoomOutBtn
            // 
            this.zoomOutBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomOutBtn.BackgroundImage = global::RlViewer.Properties.Resources.ZoomOut;
            this.zoomOutBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.zoomOutBtn.FlatAppearance.BorderSize = 0;
            this.zoomOutBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.zoomOutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomOutBtn.Location = new System.Drawing.Point(513, 3);
            this.zoomOutBtn.Name = "zoomOutBtn";
            this.zoomOutBtn.Size = new System.Drawing.Size(30, 30);
            this.zoomOutBtn.TabIndex = 26;
            this.zoomOutBtn.UseVisualStyleBackColor = true;
            this.zoomOutBtn.Click += new System.EventHandler(this.zoomOutBtn_Click);
            // 
            // navigationPanelCb
            // 
            this.navigationPanelCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.navigationPanelCb.Appearance = System.Windows.Forms.Appearance.Button;
            this.navigationPanelCb.BackgroundImage = global::RlViewer.Properties.Resources.Navigation;
            this.navigationPanelCb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.navigationPanelCb.FlatAppearance.BorderSize = 0;
            this.navigationPanelCb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.navigationPanelCb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.navigationPanelCb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.navigationPanelCb.Location = new System.Drawing.Point(549, 3);
            this.navigationPanelCb.Name = "navigationPanelCb";
            this.navigationPanelCb.Size = new System.Drawing.Size(30, 30);
            this.navigationPanelCb.TabIndex = 20;
            this.navigationPanelCb.UseVisualStyleBackColor = true;
            this.navigationPanelCb.CheckedChanged += new System.EventHandler(this.naviPanelCb_CheckedChanged);
            // 
            // filterPanelCb
            // 
            this.filterPanelCb.Appearance = System.Windows.Forms.Appearance.Button;
            this.filterPanelCb.BackgroundImage = global::RlViewer.Properties.Resources.filter;
            this.filterPanelCb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.filterPanelCb.FlatAppearance.BorderSize = 0;
            this.filterPanelCb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.filterPanelCb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.filterPanelCb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterPanelCb.Location = new System.Drawing.Point(585, 3);
            this.filterPanelCb.Name = "filterPanelCb";
            this.filterPanelCb.Size = new System.Drawing.Size(30, 30);
            this.filterPanelCb.TabIndex = 24;
            this.filterPanelCb.UseVisualStyleBackColor = true;
            this.filterPanelCb.CheckedChanged += new System.EventHandler(this.filterPanelCb_CheckedChanged);
            // 
            // scaleLabel
            // 
            this.scaleLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.scaleLabel.Location = new System.Drawing.Point(621, 3);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(85, 30);
            this.scaleLabel.TabIndex = 22;
            this.scaleLabel.Text = "Масштаб: 100%";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coordinatesLabel,
            this.distanceLabel,
            this.toolStripStatusLabel3,
            this.statusLabel,
            this.progressBar,
            this.progressLabel,
            this.cancelBtn});
            this.statusStrip1.Location = new System.Drawing.Point(0, 628);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(915, 22);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // distanceLabel
            // 
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(721, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // progressBar
            // 
            this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = false;
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(30, 17);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cancelBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cancelBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.ShowDropDownArrow = false;
            this.cancelBtn.Size = new System.Drawing.Size(47, 20);
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // squareAreaRb
            // 
            this.squareAreaRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.squareAreaRb.BackColor = System.Drawing.Color.Transparent;
            this.squareAreaRb.BackgroundImage = global::RlViewer.Properties.Resources.Plot3d;
            this.squareAreaRb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.squareAreaRb.FlatAppearance.BorderSize = 0;
            this.squareAreaRb.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.squareAreaRb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.squareAreaRb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.squareAreaRb.Location = new System.Drawing.Point(873, 72);
            this.squareAreaRb.Name = "squareAreaRb";
            this.squareAreaRb.Size = new System.Drawing.Size(30, 30);
            this.squareAreaRb.TabIndex = 25;
            this.squareAreaRb.UseVisualStyleBackColor = false;
            this.squareAreaRb.Visible = false;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 650);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.verticalScrollBar);
            this.Controls.Add(this.horizontalScrollBar);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.naviSplitter);
            this.Controls.Add(this.squareAreaRb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterTrackBar)).EndInit();
            this.naviSplitter.Panel1.ResumeLayout(false);
            this.naviSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.naviSplitter)).EndInit();
            this.naviSplitter.ResumeLayout(false);
            this.filterSplitter.Panel1.ResumeLayout(false);
            this.filterSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filterSplitter)).EndInit();
            this.filterSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigationDgv)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar horizontalScrollBar;
        private System.Windows.Forms.VScrollBar verticalScrollBar;
        private System.Windows.Forms.RadioButton dragRb;
        private System.Windows.Forms.RadioButton markPointRb;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton gammaCorrRb;
        private System.Windows.Forms.RadioButton contrastRb;
        private System.Windows.Forms.RadioButton brightnessRb;
        private System.Windows.Forms.TrackBar filterTrackBar;
        private System.Windows.Forms.Label filterLbl;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.Label percentageLabel;
        private System.Windows.Forms.RadioButton markAreaRb;
        private System.Windows.Forms.ToolStripMenuItem оФайлеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem логToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.SplitContainer naviSplitter;
        private System.Windows.Forms.CheckBox navigationPanelCb;
        private System.Windows.Forms.DataGridView navigationDgv;
        private System.Windows.Forms.RadioButton analyzeRb;
        private System.Windows.Forms.RadioButton horizontalSectionRb;
        private System.Windows.Forms.RadioButton verticalSectionRb;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Button alignBtn;
        private System.Windows.Forms.RadioButton rulerRb;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel progressLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripDropDownButton cancelBtn;
        private System.Windows.Forms.ToolStripStatusLabel coordinatesLabel;
        private System.Windows.Forms.ToolStripStatusLabel distanceLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn paramColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.Button resetFilterBtn;
        private System.Windows.Forms.ToolStripMenuItem статусКешаToolStripMenuItem;
        private System.Windows.Forms.Button findPointBtn;
        private System.Windows.Forms.SplitContainer filterSplitter;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox filterPanelCb;
        private System.Windows.Forms.RadioButton linearSectionRb;
        private System.Windows.Forms.Button zoomInBtn;
        private System.Windows.Forms.Button zoomOutBtn;
        private System.Windows.Forms.ToolStripMenuItem создатьОтчетToolStripMenuItem;
        private System.Windows.Forms.Button statisticsBtn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton squareAreaRb;
        private System.Windows.Forms.ToolStripMenuItem вшитьНавигациюToolStripMenuItem;
        private System.Windows.Forms.RadioButton sharerRb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripMenuItem совместитьФайлыToolStripMenuItem;
        private System.Windows.Forms.Button mirrorImageBtn;
        private System.Windows.Forms.RadioButton logCorrectionRb;
    }
}

