namespace RlViewer.Forms
{
    partial class SynthesisParamsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.commonPage = new System.Windows.Forms.TabPage();
            this.signalPage = new System.Windows.Forms.TabPage();
            this.normalizingGb = new System.Windows.Forms.GroupBox();
            this.rliNormalizationCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.rhgNormalizationCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.rliNormalizationCoefLbl = new System.Windows.Forms.Label();
            this.rhgNormalizationCoefLbl = new System.Windows.Forms.Label();
            this.preprocessingGb = new System.Windows.Forms.GroupBox();
            this.radioSuppressionCoefLbl = new System.Windows.Forms.Label();
            this.radioSuppressionLbl = new System.Windows.Forms.Label();
            this.radioSuppressionTb = new System.Windows.Forms.TextBox();
            this.matrixExtensionCb = new System.Windows.Forms.ComboBox();
            this.frameGb = new System.Windows.Forms.GroupBox();
            this.frameSizeAzimuthCb = new System.Windows.Forms.ComboBox();
            this.frameRangeCoefCb = new System.Windows.Forms.ComboBox();
            this.frameRangeCompressionLabel = new System.Windows.Forms.Label();
            this.frameAzimuthCoefCb = new System.Windows.Forms.ComboBox();
            this.frameSizeLabel = new System.Windows.Forms.Label();
            this.frameAzimuthCompressionLabel = new System.Windows.Forms.Label();
            this.blockGb = new System.Windows.Forms.GroupBox();
            this.blockSizeAzimuthCb = new System.Windows.Forms.ComboBox();
            this.blockRangeCoefCb = new System.Windows.Forms.ComboBox();
            this.blockAzimuthCoefCb = new System.Windows.Forms.ComboBox();
            this.blockRangeCompressionLabel = new System.Windows.Forms.Label();
            this.blockAzimuthCompressionLabel = new System.Windows.Forms.Label();
            this.blockSizeLabel = new System.Windows.Forms.Label();
            this.eokPage = new System.Windows.Forms.TabPage();
            this.dopplerFilterGb = new System.Windows.Forms.GroupBox();
            this.maxDopplerCb = new System.Windows.Forms.ComboBox();
            this.minDopplerCb = new System.Windows.Forms.ComboBox();
            this.useDopplerFilteringCb = new System.Windows.Forms.CheckBox();
            this.minDopplerLbl = new System.Windows.Forms.Label();
            this.maxDopplerLbl = new System.Windows.Forms.Label();
            this.rangePartitioningGb = new System.Windows.Forms.GroupBox();
            this.pNLengthCb = new System.Windows.Forms.ComboBox();
            this.pNLengthLbl = new System.Windows.Forms.Label();
            this.pNShiftLbl = new System.Windows.Forms.Label();
            this.pNShiftTb = new System.Windows.Forms.MaskedTextBox();
            this.afPage = new System.Windows.Forms.TabPage();
            this.navigationPage = new System.Windows.Forms.TabPage();
            this.areaPage = new System.Windows.Forms.TabPage();
            this.boxingPage = new System.Windows.Forms.TabPage();
            this.additionalInfoPage = new System.Windows.Forms.TabPage();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.memoryChunksCountCb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.signalPage.SuspendLayout();
            this.normalizingGb.SuspendLayout();
            this.preprocessingGb.SuspendLayout();
            this.frameGb.SuspendLayout();
            this.blockGb.SuspendLayout();
            this.eokPage.SuspendLayout();
            this.dopplerFilterGb.SuspendLayout();
            this.rangePartitioningGb.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.commonPage);
            this.tabControl1.Controls.Add(this.signalPage);
            this.tabControl1.Controls.Add(this.eokPage);
            this.tabControl1.Controls.Add(this.afPage);
            this.tabControl1.Controls.Add(this.navigationPage);
            this.tabControl1.Controls.Add(this.areaPage);
            this.tabControl1.Controls.Add(this.boxingPage);
            this.tabControl1.Controls.Add(this.additionalInfoPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(573, 415);
            this.tabControl1.TabIndex = 0;
            // 
            // commonPage
            // 
            this.commonPage.Location = new System.Drawing.Point(4, 22);
            this.commonPage.Name = "commonPage";
            this.commonPage.Padding = new System.Windows.Forms.Padding(3);
            this.commonPage.Size = new System.Drawing.Size(565, 389);
            this.commonPage.TabIndex = 1;
            this.commonPage.Text = "Общие";
            this.commonPage.UseVisualStyleBackColor = true;
            // 
            // signalPage
            // 
            this.signalPage.Controls.Add(this.label1);
            this.signalPage.Controls.Add(this.memoryChunksCountCb);
            this.signalPage.Controls.Add(this.normalizingGb);
            this.signalPage.Controls.Add(this.preprocessingGb);
            this.signalPage.Controls.Add(this.frameGb);
            this.signalPage.Controls.Add(this.blockGb);
            this.signalPage.Location = new System.Drawing.Point(4, 22);
            this.signalPage.Name = "signalPage";
            this.signalPage.Padding = new System.Windows.Forms.Padding(3);
            this.signalPage.Size = new System.Drawing.Size(565, 389);
            this.signalPage.TabIndex = 0;
            this.signalPage.Text = "Сигнал";
            this.signalPage.UseVisualStyleBackColor = true;
            // 
            // normalizingGb
            // 
            this.normalizingGb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.normalizingGb.Controls.Add(this.rliNormalizationCoefTb);
            this.normalizingGb.Controls.Add(this.rhgNormalizationCoefTb);
            this.normalizingGb.Controls.Add(this.rliNormalizationCoefLbl);
            this.normalizingGb.Controls.Add(this.rhgNormalizationCoefLbl);
            this.normalizingGb.Location = new System.Drawing.Point(9, 234);
            this.normalizingGb.Name = "normalizingGb";
            this.normalizingGb.Size = new System.Drawing.Size(550, 55);
            this.normalizingGb.TabIndex = 5;
            this.normalizingGb.TabStop = false;
            this.normalizingGb.Text = "Нормировка";
            // 
            // rliNormalizationCoefTb
            // 
            this.rliNormalizationCoefTb.AllowPromptAsInput = false;
            this.rliNormalizationCoefTb.Location = new System.Drawing.Point(429, 22);
            this.rliNormalizationCoefTb.Mask = "0000000000000000";
            this.rliNormalizationCoefTb.Name = "rliNormalizationCoefTb";
            this.rliNormalizationCoefTb.PromptChar = ' ';
            this.rliNormalizationCoefTb.ResetOnPrompt = false;
            this.rliNormalizationCoefTb.Size = new System.Drawing.Size(62, 20);
            this.rliNormalizationCoefTb.TabIndex = 8;
            // 
            // rhgNormalizationCoefTb
            // 
            this.rhgNormalizationCoefTb.AllowPromptAsInput = false;
            this.rhgNormalizationCoefTb.Location = new System.Drawing.Point(151, 22);
            this.rhgNormalizationCoefTb.Mask = "0000000000000000";
            this.rhgNormalizationCoefTb.Name = "rhgNormalizationCoefTb";
            this.rhgNormalizationCoefTb.PromptChar = ' ';
            this.rhgNormalizationCoefTb.ResetOnPrompt = false;
            this.rhgNormalizationCoefTb.Size = new System.Drawing.Size(62, 20);
            this.rhgNormalizationCoefTb.TabIndex = 7;
            // 
            // rliNormalizationCoefLbl
            // 
            this.rliNormalizationCoefLbl.AutoSize = true;
            this.rliNormalizationCoefLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rliNormalizationCoefLbl.Location = new System.Drawing.Point(281, 25);
            this.rliNormalizationCoefLbl.Name = "rliNormalizationCoefLbl";
            this.rliNormalizationCoefLbl.Size = new System.Drawing.Size(128, 13);
            this.rliNormalizationCoefLbl.TabIndex = 5;
            this.rliNormalizationCoefLbl.Text = "Коэф нормировки РЛИ:";
            // 
            // rhgNormalizationCoefLbl
            // 
            this.rhgNormalizationCoefLbl.AutoSize = true;
            this.rhgNormalizationCoefLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rhgNormalizationCoefLbl.Location = new System.Drawing.Point(3, 25);
            this.rhgNormalizationCoefLbl.Name = "rhgNormalizationCoefLbl";
            this.rhgNormalizationCoefLbl.Size = new System.Drawing.Size(121, 13);
            this.rhgNormalizationCoefLbl.TabIndex = 4;
            this.rhgNormalizationCoefLbl.Text = "Коэф нормировки РГГ";
            // 
            // preprocessingGb
            // 
            this.preprocessingGb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.preprocessingGb.Controls.Add(this.radioSuppressionCoefLbl);
            this.preprocessingGb.Controls.Add(this.radioSuppressionLbl);
            this.preprocessingGb.Controls.Add(this.radioSuppressionTb);
            this.preprocessingGb.Controls.Add(this.matrixExtensionCb);
            this.preprocessingGb.Location = new System.Drawing.Point(6, 173);
            this.preprocessingGb.Name = "preprocessingGb";
            this.preprocessingGb.Size = new System.Drawing.Size(553, 55);
            this.preprocessingGb.TabIndex = 4;
            this.preprocessingGb.TabStop = false;
            this.preprocessingGb.Text = "Предобработка";
            // 
            // radioSuppressionCoefLbl
            // 
            this.radioSuppressionCoefLbl.AutoSize = true;
            this.radioSuppressionCoefLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioSuppressionCoefLbl.Location = new System.Drawing.Point(284, 22);
            this.radioSuppressionCoefLbl.Name = "radioSuppressionCoefLbl";
            this.radioSuppressionCoefLbl.Size = new System.Drawing.Size(147, 13);
            this.radioSuppressionCoefLbl.TabIndex = 3;
            this.radioSuppressionCoefLbl.Text = "Коэф расширения матрицы";
            // 
            // radioSuppressionLbl
            // 
            this.radioSuppressionLbl.AutoSize = true;
            this.radioSuppressionLbl.Location = new System.Drawing.Point(6, 22);
            this.radioSuppressionLbl.Name = "radioSuppressionLbl";
            this.radioSuppressionLbl.Size = new System.Drawing.Size(137, 13);
            this.radioSuppressionLbl.TabIndex = 2;
            this.radioSuppressionLbl.Text = "Коэф подавления помехи";
            // 
            // radioSuppressionTb
            // 
            this.radioSuppressionTb.Location = new System.Drawing.Point(155, 19);
            this.radioSuppressionTb.Name = "radioSuppressionTb";
            this.radioSuppressionTb.Size = new System.Drawing.Size(62, 20);
            this.radioSuppressionTb.TabIndex = 1;
            // 
            // matrixExtensionCb
            // 
            this.matrixExtensionCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.matrixExtensionCb.FormattingEnabled = true;
            this.matrixExtensionCb.Location = new System.Drawing.Point(447, 18);
            this.matrixExtensionCb.Name = "matrixExtensionCb";
            this.matrixExtensionCb.Size = new System.Drawing.Size(47, 21);
            this.matrixExtensionCb.TabIndex = 0;
            // 
            // frameGb
            // 
            this.frameGb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frameGb.Controls.Add(this.frameSizeAzimuthCb);
            this.frameGb.Controls.Add(this.frameRangeCoefCb);
            this.frameGb.Controls.Add(this.frameRangeCompressionLabel);
            this.frameGb.Controls.Add(this.frameAzimuthCoefCb);
            this.frameGb.Controls.Add(this.frameSizeLabel);
            this.frameGb.Controls.Add(this.frameAzimuthCompressionLabel);
            this.frameGb.Location = new System.Drawing.Point(284, 20);
            this.frameGb.Name = "frameGb";
            this.frameGb.Size = new System.Drawing.Size(275, 147);
            this.frameGb.TabIndex = 3;
            this.frameGb.TabStop = false;
            this.frameGb.Text = "Кадр";
            // 
            // frameSizeAzimuthCb
            // 
            this.frameSizeAzimuthCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frameSizeAzimuthCb.FormattingEnabled = true;
            this.frameSizeAzimuthCb.Location = new System.Drawing.Point(150, 24);
            this.frameSizeAzimuthCb.Name = "frameSizeAzimuthCb";
            this.frameSizeAzimuthCb.Size = new System.Drawing.Size(66, 21);
            this.frameSizeAzimuthCb.TabIndex = 8;
            // 
            // frameRangeCoefCb
            // 
            this.frameRangeCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frameRangeCoefCb.FormattingEnabled = true;
            this.frameRangeCoefCb.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16"});
            this.frameRangeCoefCb.Location = new System.Drawing.Point(150, 103);
            this.frameRangeCoefCb.Name = "frameRangeCoefCb";
            this.frameRangeCoefCb.Size = new System.Drawing.Size(66, 21);
            this.frameRangeCoefCb.TabIndex = 7;
            // 
            // frameRangeCompressionLabel
            // 
            this.frameRangeCompressionLabel.AutoSize = true;
            this.frameRangeCompressionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.frameRangeCompressionLabel.Location = new System.Drawing.Point(6, 106);
            this.frameRangeCompressionLabel.Name = "frameRangeCompressionLabel";
            this.frameRangeCompressionLabel.Size = new System.Drawing.Size(116, 13);
            this.frameRangeCompressionLabel.TabIndex = 4;
            this.frameRangeCompressionLabel.Text = "Сжатие по дальности";
            // 
            // frameAzimuthCoefCb
            // 
            this.frameAzimuthCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frameAzimuthCoefCb.FormattingEnabled = true;
            this.frameAzimuthCoefCb.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16"});
            this.frameAzimuthCoefCb.Location = new System.Drawing.Point(150, 64);
            this.frameAzimuthCoefCb.Name = "frameAzimuthCoefCb";
            this.frameAzimuthCoefCb.Size = new System.Drawing.Size(66, 21);
            this.frameAzimuthCoefCb.TabIndex = 6;
            // 
            // frameSizeLabel
            // 
            this.frameSizeLabel.AutoSize = true;
            this.frameSizeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.frameSizeLabel.Location = new System.Drawing.Point(6, 27);
            this.frameSizeLabel.Name = "frameSizeLabel";
            this.frameSizeLabel.Size = new System.Drawing.Size(138, 13);
            this.frameSizeLabel.TabIndex = 1;
            this.frameSizeLabel.Text = "Размер кадра по азимуту";
            // 
            // frameAzimuthCompressionLabel
            // 
            this.frameAzimuthCompressionLabel.AutoSize = true;
            this.frameAzimuthCompressionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.frameAzimuthCompressionLabel.Location = new System.Drawing.Point(6, 67);
            this.frameAzimuthCompressionLabel.Name = "frameAzimuthCompressionLabel";
            this.frameAzimuthCompressionLabel.Size = new System.Drawing.Size(104, 13);
            this.frameAzimuthCompressionLabel.TabIndex = 3;
            this.frameAzimuthCompressionLabel.Text = "Сжатие по азимуту";
            // 
            // blockGb
            // 
            this.blockGb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blockGb.Controls.Add(this.blockSizeAzimuthCb);
            this.blockGb.Controls.Add(this.blockRangeCoefCb);
            this.blockGb.Controls.Add(this.blockAzimuthCoefCb);
            this.blockGb.Controls.Add(this.blockRangeCompressionLabel);
            this.blockGb.Controls.Add(this.blockAzimuthCompressionLabel);
            this.blockGb.Controls.Add(this.blockSizeLabel);
            this.blockGb.Location = new System.Drawing.Point(6, 20);
            this.blockGb.Name = "blockGb";
            this.blockGb.Size = new System.Drawing.Size(272, 147);
            this.blockGb.TabIndex = 2;
            this.blockGb.TabStop = false;
            this.blockGb.Text = "Блок";
            // 
            // blockSizeAzimuthCb
            // 
            this.blockSizeAzimuthCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockSizeAzimuthCb.FormattingEnabled = true;
            this.blockSizeAzimuthCb.Location = new System.Drawing.Point(151, 24);
            this.blockSizeAzimuthCb.Name = "blockSizeAzimuthCb";
            this.blockSizeAzimuthCb.Size = new System.Drawing.Size(66, 21);
            this.blockSizeAzimuthCb.TabIndex = 5;
            // 
            // blockRangeCoefCb
            // 
            this.blockRangeCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockRangeCoefCb.FormattingEnabled = true;
            this.blockRangeCoefCb.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16"});
            this.blockRangeCoefCb.Location = new System.Drawing.Point(151, 103);
            this.blockRangeCoefCb.Name = "blockRangeCoefCb";
            this.blockRangeCoefCb.Size = new System.Drawing.Size(66, 21);
            this.blockRangeCoefCb.TabIndex = 4;
            // 
            // blockAzimuthCoefCb
            // 
            this.blockAzimuthCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockAzimuthCoefCb.FormattingEnabled = true;
            this.blockAzimuthCoefCb.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16"});
            this.blockAzimuthCoefCb.Location = new System.Drawing.Point(151, 64);
            this.blockAzimuthCoefCb.Name = "blockAzimuthCoefCb";
            this.blockAzimuthCoefCb.Size = new System.Drawing.Size(66, 21);
            this.blockAzimuthCoefCb.TabIndex = 3;
            // 
            // blockRangeCompressionLabel
            // 
            this.blockRangeCompressionLabel.AutoSize = true;
            this.blockRangeCompressionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.blockRangeCompressionLabel.Location = new System.Drawing.Point(6, 106);
            this.blockRangeCompressionLabel.Name = "blockRangeCompressionLabel";
            this.blockRangeCompressionLabel.Size = new System.Drawing.Size(116, 13);
            this.blockRangeCompressionLabel.TabIndex = 2;
            this.blockRangeCompressionLabel.Text = "Сжатие по дальности";
            // 
            // blockAzimuthCompressionLabel
            // 
            this.blockAzimuthCompressionLabel.AutoSize = true;
            this.blockAzimuthCompressionLabel.Location = new System.Drawing.Point(6, 67);
            this.blockAzimuthCompressionLabel.Name = "blockAzimuthCompressionLabel";
            this.blockAzimuthCompressionLabel.Size = new System.Drawing.Size(104, 13);
            this.blockAzimuthCompressionLabel.TabIndex = 1;
            this.blockAzimuthCompressionLabel.Text = "Сжатие по азимуту";
            // 
            // blockSizeLabel
            // 
            this.blockSizeLabel.AutoSize = true;
            this.blockSizeLabel.Location = new System.Drawing.Point(6, 27);
            this.blockSizeLabel.Name = "blockSizeLabel";
            this.blockSizeLabel.Size = new System.Drawing.Size(138, 13);
            this.blockSizeLabel.TabIndex = 0;
            this.blockSizeLabel.Text = "Размер блока по азимуту";
            // 
            // eokPage
            // 
            this.eokPage.Controls.Add(this.dopplerFilterGb);
            this.eokPage.Controls.Add(this.rangePartitioningGb);
            this.eokPage.Location = new System.Drawing.Point(4, 22);
            this.eokPage.Name = "eokPage";
            this.eokPage.Size = new System.Drawing.Size(565, 389);
            this.eokPage.TabIndex = 2;
            this.eokPage.Text = "ЕОК";
            this.eokPage.UseVisualStyleBackColor = true;
            // 
            // dopplerFilterGb
            // 
            this.dopplerFilterGb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dopplerFilterGb.Controls.Add(this.maxDopplerCb);
            this.dopplerFilterGb.Controls.Add(this.minDopplerCb);
            this.dopplerFilterGb.Controls.Add(this.useDopplerFilteringCb);
            this.dopplerFilterGb.Controls.Add(this.minDopplerLbl);
            this.dopplerFilterGb.Controls.Add(this.maxDopplerLbl);
            this.dopplerFilterGb.Location = new System.Drawing.Point(276, 12);
            this.dopplerFilterGb.Name = "dopplerFilterGb";
            this.dopplerFilterGb.Size = new System.Drawing.Size(286, 119);
            this.dopplerFilterGb.TabIndex = 6;
            this.dopplerFilterGb.TabStop = false;
            this.dopplerFilterGb.Text = "Фильтр допплеровских частот";
            // 
            // maxDopplerCb
            // 
            this.maxDopplerCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.maxDopplerCb.FormattingEnabled = true;
            this.maxDopplerCb.Location = new System.Drawing.Point(122, 86);
            this.maxDopplerCb.Name = "maxDopplerCb";
            this.maxDopplerCb.Size = new System.Drawing.Size(100, 21);
            this.maxDopplerCb.TabIndex = 6;
            // 
            // minDopplerCb
            // 
            this.minDopplerCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.minDopplerCb.FormattingEnabled = true;
            this.minDopplerCb.Location = new System.Drawing.Point(122, 57);
            this.minDopplerCb.Name = "minDopplerCb";
            this.minDopplerCb.Size = new System.Drawing.Size(100, 21);
            this.minDopplerCb.TabIndex = 5;
            // 
            // useDopplerFilteringCb
            // 
            this.useDopplerFilteringCb.AutoSize = true;
            this.useDopplerFilteringCb.Location = new System.Drawing.Point(14, 29);
            this.useDopplerFilteringCb.Name = "useDopplerFilteringCb";
            this.useDopplerFilteringCb.Size = new System.Drawing.Size(99, 17);
            this.useDopplerFilteringCb.TabIndex = 4;
            this.useDopplerFilteringCb.Text = "Использовать";
            this.useDopplerFilteringCb.UseVisualStyleBackColor = true;
            this.useDopplerFilteringCb.CheckedChanged += new System.EventHandler(this.useDopplerFilteringCb_CheckedChanged);
            // 
            // minDopplerLbl
            // 
            this.minDopplerLbl.AutoSize = true;
            this.minDopplerLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.minDopplerLbl.Location = new System.Drawing.Point(11, 60);
            this.minDopplerLbl.Name = "minDopplerLbl";
            this.minDopplerLbl.Size = new System.Drawing.Size(79, 13);
            this.minDopplerLbl.TabIndex = 2;
            this.minDopplerLbl.Text = "Ближняя зона";
            // 
            // maxDopplerLbl
            // 
            this.maxDopplerLbl.AutoSize = true;
            this.maxDopplerLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.maxDopplerLbl.Location = new System.Drawing.Point(11, 89);
            this.maxDopplerLbl.Name = "maxDopplerLbl";
            this.maxDopplerLbl.Size = new System.Drawing.Size(79, 13);
            this.maxDopplerLbl.TabIndex = 3;
            this.maxDopplerLbl.Text = "Дальняя зона";
            // 
            // rangePartitioningGb
            // 
            this.rangePartitioningGb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rangePartitioningGb.Controls.Add(this.pNLengthCb);
            this.rangePartitioningGb.Controls.Add(this.pNLengthLbl);
            this.rangePartitioningGb.Controls.Add(this.pNShiftLbl);
            this.rangePartitioningGb.Controls.Add(this.pNShiftTb);
            this.rangePartitioningGb.Location = new System.Drawing.Point(3, 12);
            this.rangePartitioningGb.Name = "rangePartitioningGb";
            this.rangePartitioningGb.Size = new System.Drawing.Size(267, 119);
            this.rangePartitioningGb.TabIndex = 5;
            this.rangePartitioningGb.TabStop = false;
            this.rangePartitioningGb.Text = "Деление по дальности";
            // 
            // pNLengthCb
            // 
            this.pNLengthCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pNLengthCb.FormattingEnabled = true;
            this.pNLengthCb.Location = new System.Drawing.Point(127, 57);
            this.pNLengthCb.Name = "pNLengthCb";
            this.pNLengthCb.Size = new System.Drawing.Size(100, 21);
            this.pNLengthCb.TabIndex = 4;
            // 
            // pNLengthLbl
            // 
            this.pNLengthLbl.AutoSize = true;
            this.pNLengthLbl.Location = new System.Drawing.Point(16, 60);
            this.pNLengthLbl.Name = "pNLengthLbl";
            this.pNLengthLbl.Size = new System.Drawing.Size(87, 13);
            this.pNLengthLbl.TabIndex = 2;
            this.pNLengthLbl.Text = "Ширина полосы";
            // 
            // pNShiftLbl
            // 
            this.pNShiftLbl.AutoSize = true;
            this.pNShiftLbl.Location = new System.Drawing.Point(16, 91);
            this.pNShiftLbl.Name = "pNShiftLbl";
            this.pNShiftLbl.Size = new System.Drawing.Size(102, 13);
            this.pNShiftLbl.TabIndex = 3;
            this.pNShiftLbl.Text = "Смещение полосы";
            // 
            // pNShiftTb
            // 
            this.pNShiftTb.AllowPromptAsInput = false;
            this.pNShiftTb.Location = new System.Drawing.Point(127, 88);
            this.pNShiftTb.Mask = "000000";
            this.pNShiftTb.Name = "pNShiftTb";
            this.pNShiftTb.PromptChar = ' ';
            this.pNShiftTb.ResetOnPrompt = false;
            this.pNShiftTb.Size = new System.Drawing.Size(100, 20);
            this.pNShiftTb.TabIndex = 1;
            // 
            // afPage
            // 
            this.afPage.Location = new System.Drawing.Point(4, 22);
            this.afPage.Name = "afPage";
            this.afPage.Size = new System.Drawing.Size(565, 389);
            this.afPage.TabIndex = 3;
            this.afPage.Text = "Автофокус";
            this.afPage.UseVisualStyleBackColor = true;
            // 
            // navigationPage
            // 
            this.navigationPage.Location = new System.Drawing.Point(4, 22);
            this.navigationPage.Name = "navigationPage";
            this.navigationPage.Size = new System.Drawing.Size(565, 389);
            this.navigationPage.TabIndex = 4;
            this.navigationPage.Text = "Навигация";
            this.navigationPage.UseVisualStyleBackColor = true;
            // 
            // areaPage
            // 
            this.areaPage.Location = new System.Drawing.Point(4, 22);
            this.areaPage.Name = "areaPage";
            this.areaPage.Size = new System.Drawing.Size(565, 389);
            this.areaPage.TabIndex = 5;
            this.areaPage.Text = "Область";
            this.areaPage.UseVisualStyleBackColor = true;
            // 
            // boxingPage
            // 
            this.boxingPage.Location = new System.Drawing.Point(4, 22);
            this.boxingPage.Name = "boxingPage";
            this.boxingPage.Size = new System.Drawing.Size(565, 389);
            this.boxingPage.TabIndex = 6;
            this.boxingPage.Text = "Упаковка";
            this.boxingPage.UseVisualStyleBackColor = true;
            // 
            // additionalInfoPage
            // 
            this.additionalInfoPage.Location = new System.Drawing.Point(4, 22);
            this.additionalInfoPage.Name = "additionalInfoPage";
            this.additionalInfoPage.Size = new System.Drawing.Size(565, 389);
            this.additionalInfoPage.TabIndex = 7;
            this.additionalInfoPage.Text = "Сообщение";
            this.additionalInfoPage.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(12, 434);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "Ок";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cancelBtn.Location = new System.Drawing.Point(510, 434);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // memoryChunksCountCb
            // 
            this.memoryChunksCountCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.memoryChunksCountCb.FormattingEnabled = true;
            this.memoryChunksCountCb.Location = new System.Drawing.Point(160, 322);
            this.memoryChunksCountCb.Name = "memoryChunksCountCb";
            this.memoryChunksCountCb.Size = new System.Drawing.Size(62, 21);
            this.memoryChunksCountCb.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Количество блоков памяти";
            // 
            // SynthesisParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 474);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SynthesisParamsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры синтеза";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SynthesisParams_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.signalPage.ResumeLayout(false);
            this.signalPage.PerformLayout();
            this.normalizingGb.ResumeLayout(false);
            this.normalizingGb.PerformLayout();
            this.preprocessingGb.ResumeLayout(false);
            this.preprocessingGb.PerformLayout();
            this.frameGb.ResumeLayout(false);
            this.frameGb.PerformLayout();
            this.blockGb.ResumeLayout(false);
            this.blockGb.PerformLayout();
            this.eokPage.ResumeLayout(false);
            this.dopplerFilterGb.ResumeLayout(false);
            this.dopplerFilterGb.PerformLayout();
            this.rangePartitioningGb.ResumeLayout(false);
            this.rangePartitioningGb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage commonPage;
        private System.Windows.Forms.TabPage signalPage;
        private System.Windows.Forms.TabPage eokPage;
        private System.Windows.Forms.TabPage afPage;
        private System.Windows.Forms.TabPage navigationPage;
        private System.Windows.Forms.TabPage areaPage;
        private System.Windows.Forms.TabPage boxingPage;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.GroupBox frameGb;
        private System.Windows.Forms.ComboBox frameSizeAzimuthCb;
        private System.Windows.Forms.ComboBox frameRangeCoefCb;
        private System.Windows.Forms.Label frameRangeCompressionLabel;
        private System.Windows.Forms.ComboBox frameAzimuthCoefCb;
        private System.Windows.Forms.Label frameSizeLabel;
        private System.Windows.Forms.Label frameAzimuthCompressionLabel;
        private System.Windows.Forms.GroupBox blockGb;
        private System.Windows.Forms.ComboBox blockSizeAzimuthCb;
        private System.Windows.Forms.ComboBox blockRangeCoefCb;
        private System.Windows.Forms.ComboBox blockAzimuthCoefCb;
        private System.Windows.Forms.Label blockRangeCompressionLabel;
        private System.Windows.Forms.Label blockAzimuthCompressionLabel;
        private System.Windows.Forms.Label blockSizeLabel;
        private System.Windows.Forms.GroupBox normalizingGb;
        private System.Windows.Forms.Label rliNormalizationCoefLbl;
        private System.Windows.Forms.Label rhgNormalizationCoefLbl;
        private System.Windows.Forms.GroupBox preprocessingGb;
        private System.Windows.Forms.Label radioSuppressionCoefLbl;
        private System.Windows.Forms.Label radioSuppressionLbl;
        private System.Windows.Forms.TextBox radioSuppressionTb;
        private System.Windows.Forms.ComboBox matrixExtensionCb;
        private System.Windows.Forms.GroupBox dopplerFilterGb;
        private System.Windows.Forms.CheckBox useDopplerFilteringCb;
        private System.Windows.Forms.Label minDopplerLbl;
        private System.Windows.Forms.Label maxDopplerLbl;
        private System.Windows.Forms.GroupBox rangePartitioningGb;
        private System.Windows.Forms.Label pNLengthLbl;
        private System.Windows.Forms.Label pNShiftLbl;
        private System.Windows.Forms.MaskedTextBox pNShiftTb;
        private System.Windows.Forms.ComboBox pNLengthCb;
        private System.Windows.Forms.ComboBox maxDopplerCb;
        private System.Windows.Forms.ComboBox minDopplerCb;
        private System.Windows.Forms.MaskedTextBox rliNormalizationCoefTb;
        private System.Windows.Forms.MaskedTextBox rhgNormalizationCoefTb;
        private System.Windows.Forms.TabPage additionalInfoPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox memoryChunksCountCb;
    }
}