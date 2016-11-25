namespace RlViewer.Forms
{
    partial class SettingsForm
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
            this.commonTab = new System.Windows.Forms.TabPage();
            this.forceImageHeightAdjustingCb = new System.Windows.Forms.CheckBox();
            this.useCustomFileOpenDlgCb = new System.Windows.Forms.CheckBox();
            this.adminReminderCb = new System.Windows.Forms.CheckBox();
            this.viewTab = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tileOutputCb = new System.Windows.Forms.ComboBox();
            this.forceTileGenCheckBox = new System.Windows.Forms.CheckBox();
            this.allowViewCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.logPaletteCb = new System.Windows.Forms.CheckBox();
            this.inverseCheckBox = new System.Windows.Forms.CheckBox();
            this.highResCb = new System.Windows.Forms.CheckBox();
            this.toolstab = new System.Windows.Forms.TabPage();
            this.rbfInterpolationcSettingsGb = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.regularizationCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.layersNumTb = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.baseRadiusTb = new System.Windows.Forms.MaskedTextBox();
            this.areasOrPointsForAligningCb = new System.Windows.Forms.CheckBox();
            this.surfaceTypeLbl = new System.Windows.Forms.Label();
            this.surfaceTypeCb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.areaSizeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sectionSizeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.synthesisTab = new System.Windows.Forms.TabPage();
            this.serverSarPathAreaGb = new System.Windows.Forms.GroupBox();
            this.serverSarPathTb = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.showServerSarCb = new System.Windows.Forms.CheckBox();
            this.showSynthesisCommonTabCb = new System.Windows.Forms.CheckBox();
            this.deleteOnCancelCb = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.plot3dSizeTb = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.azimuthCompressCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.rangeCompressCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.forceSynthesisCb = new System.Windows.Forms.CheckBox();
            this.comboBoxPics1 = new RlViewer.Settings.ComboBoxPics();
            this.useEmbeddedServerSarCb = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.commonTab.SuspendLayout();
            this.viewTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolstab.SuspendLayout();
            this.rbfInterpolationcSettingsGb.SuspendLayout();
            this.synthesisTab.SuspendLayout();
            this.serverSarPathAreaGb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.commonTab);
            this.tabControl1.Controls.Add(this.viewTab);
            this.tabControl1.Controls.Add(this.toolstab);
            this.tabControl1.Controls.Add(this.synthesisTab);
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(278, 329);
            this.tabControl1.TabIndex = 1;
            // 
            // commonTab
            // 
            this.commonTab.Controls.Add(this.forceImageHeightAdjustingCb);
            this.commonTab.Controls.Add(this.useCustomFileOpenDlgCb);
            this.commonTab.Controls.Add(this.adminReminderCb);
            this.commonTab.Location = new System.Drawing.Point(4, 22);
            this.commonTab.Name = "commonTab";
            this.commonTab.Padding = new System.Windows.Forms.Padding(3);
            this.commonTab.Size = new System.Drawing.Size(270, 303);
            this.commonTab.TabIndex = 3;
            this.commonTab.Text = "Общие";
            this.commonTab.UseVisualStyleBackColor = true;
            // 
            // forceImageHeightAdjustingCb
            // 
            this.forceImageHeightAdjustingCb.AutoSize = true;
            this.forceImageHeightAdjustingCb.Location = new System.Drawing.Point(6, 106);
            this.forceImageHeightAdjustingCb.Name = "forceImageHeightAdjustingCb";
            this.forceImageHeightAdjustingCb.Size = new System.Drawing.Size(200, 30);
            this.forceImageHeightAdjustingCb.TabIndex = 2;
            this.forceImageHeightAdjustingCb.Text = "Приводить высоту изображения к\r\n количеству строк";
            this.forceImageHeightAdjustingCb.UseVisualStyleBackColor = true;
            this.forceImageHeightAdjustingCb.CheckedChanged += new System.EventHandler(this.forceImageHeightAdjustingCb_CheckedChanged);
            // 
            // useCustomFileOpenDlgCb
            // 
            this.useCustomFileOpenDlgCb.AutoSize = true;
            this.useCustomFileOpenDlgCb.Location = new System.Drawing.Point(6, 24);
            this.useCustomFileOpenDlgCb.Name = "useCustomFileOpenDlgCb";
            this.useCustomFileOpenDlgCb.Size = new System.Drawing.Size(207, 30);
            this.useCustomFileOpenDlgCb.TabIndex = 1;
            this.useCustomFileOpenDlgCb.Text = "Использовать собственный диалог\r\nоткрытия файлов";
            this.useCustomFileOpenDlgCb.UseVisualStyleBackColor = true;
            this.useCustomFileOpenDlgCb.CheckedChanged += new System.EventHandler(this.useCustomFileOpenDlgCb_CheckedChanged);
            // 
            // adminReminderCb
            // 
            this.adminReminderCb.AutoSize = true;
            this.adminReminderCb.Location = new System.Drawing.Point(6, 60);
            this.adminReminderCb.Name = "adminReminderCb";
            this.adminReminderCb.Size = new System.Drawing.Size(207, 30);
            this.adminReminderCb.TabIndex = 0;
            this.adminReminderCb.Text = "Предупреждать об отсутствии прав\r\nадминистратора";
            this.adminReminderCb.UseVisualStyleBackColor = true;
            this.adminReminderCb.CheckedChanged += new System.EventHandler(this.adminReminderCb_CheckedChanged);
            // 
            // viewTab
            // 
            this.viewTab.Controls.Add(this.label3);
            this.viewTab.Controls.Add(this.tileOutputCb);
            this.viewTab.Controls.Add(this.forceTileGenCheckBox);
            this.viewTab.Controls.Add(this.allowViewCheckBox);
            this.viewTab.Controls.Add(this.groupBox3);
            this.viewTab.Controls.Add(this.highResCb);
            this.viewTab.Location = new System.Drawing.Point(4, 22);
            this.viewTab.Name = "viewTab";
            this.viewTab.Padding = new System.Windows.Forms.Padding(3);
            this.viewTab.Size = new System.Drawing.Size(270, 303);
            this.viewTab.TabIndex = 0;
            this.viewTab.Text = "Отображение";
            this.viewTab.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Алгоритм вывода";
            // 
            // tileOutputCb
            // 
            this.tileOutputCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tileOutputCb.FormattingEnabled = true;
            this.tileOutputCb.Items.AddRange(new object[] {
            "Линейный",
            "Логарифмический",
            "Линейно-Логарифмический"});
            this.tileOutputCb.Location = new System.Drawing.Point(6, 220);
            this.tileOutputCb.Name = "tileOutputCb";
            this.tileOutputCb.Size = new System.Drawing.Size(170, 21);
            this.tileOutputCb.TabIndex = 19;
            this.tileOutputCb.SelectedIndexChanged += new System.EventHandler(this.tileOutputCb_SelectedIndexChanged);
            // 
            // forceTileGenCheckBox
            // 
            this.forceTileGenCheckBox.AutoSize = true;
            this.forceTileGenCheckBox.Location = new System.Drawing.Point(6, 173);
            this.forceTileGenCheckBox.Name = "forceTileGenCheckBox";
            this.forceTileGenCheckBox.Size = new System.Drawing.Size(204, 17);
            this.forceTileGenCheckBox.TabIndex = 18;
            this.forceTileGenCheckBox.Text = "Принудительная генерация тайлов";
            this.forceTileGenCheckBox.UseVisualStyleBackColor = true;
            this.forceTileGenCheckBox.CheckedChanged += new System.EventHandler(this.forceTileGenCheckBox_CheckedChanged);
            // 
            // allowViewCheckBox
            // 
            this.allowViewCheckBox.AutoSize = true;
            this.allowViewCheckBox.Location = new System.Drawing.Point(6, 138);
            this.allowViewCheckBox.Name = "allowViewCheckBox";
            this.allowViewCheckBox.Size = new System.Drawing.Size(247, 17);
            this.allowViewCheckBox.TabIndex = 17;
            this.allowViewCheckBox.Text = "Просмотр изображения во время загрузки";
            this.allowViewCheckBox.UseVisualStyleBackColor = true;
            this.allowViewCheckBox.CheckedChanged += new System.EventHandler(this.allowViewCheckBox_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxPics1);
            this.groupBox3.Controls.Add(this.logPaletteCb);
            this.groupBox3.Controls.Add(this.inverseCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 78);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Палитра";
            // 
            // logPaletteCb
            // 
            this.logPaletteCb.AutoSize = true;
            this.logPaletteCb.Location = new System.Drawing.Point(118, 19);
            this.logPaletteCb.Name = "logPaletteCb";
            this.logPaletteCb.Size = new System.Drawing.Size(128, 17);
            this.logPaletteCb.TabIndex = 15;
            this.logPaletteCb.Text = "Группировать цвета";
            this.logPaletteCb.UseVisualStyleBackColor = true;
            this.logPaletteCb.CheckedChanged += new System.EventHandler(this.logPaletteCb_CheckedChanged);
            // 
            // inverseCheckBox
            // 
            this.inverseCheckBox.AutoSize = true;
            this.inverseCheckBox.Location = new System.Drawing.Point(118, 42);
            this.inverseCheckBox.Name = "inverseCheckBox";
            this.inverseCheckBox.Size = new System.Drawing.Size(104, 17);
            this.inverseCheckBox.TabIndex = 14;
            this.inverseCheckBox.Text = "Инвертировать";
            this.inverseCheckBox.UseVisualStyleBackColor = true;
            this.inverseCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // highResCb
            // 
            this.highResCb.AutoSize = true;
            this.highResCb.Location = new System.Drawing.Point(6, 106);
            this.highResCb.Name = "highResCb";
            this.highResCb.Size = new System.Drawing.Size(253, 17);
            this.highResCb.TabIndex = 16;
            this.highResCb.Text = "Высокое разрешение при масштабировании";
            this.highResCb.UseVisualStyleBackColor = true;
            this.highResCb.CheckedChanged += new System.EventHandler(this.highResCb_CheckedChanged);
            // 
            // toolstab
            // 
            this.toolstab.Controls.Add(this.rbfInterpolationcSettingsGb);
            this.toolstab.Controls.Add(this.areasOrPointsForAligningCb);
            this.toolstab.Controls.Add(this.surfaceTypeLbl);
            this.toolstab.Controls.Add(this.surfaceTypeCb);
            this.toolstab.Controls.Add(this.label2);
            this.toolstab.Controls.Add(this.areaSizeTextBox);
            this.toolstab.Controls.Add(this.label1);
            this.toolstab.Controls.Add(this.sectionSizeTextBox);
            this.toolstab.Location = new System.Drawing.Point(4, 22);
            this.toolstab.Name = "toolstab";
            this.toolstab.Padding = new System.Windows.Forms.Padding(3);
            this.toolstab.Size = new System.Drawing.Size(270, 303);
            this.toolstab.TabIndex = 2;
            this.toolstab.Text = "Инструменты";
            this.toolstab.UseVisualStyleBackColor = true;
            // 
            // rbfInterpolationcSettingsGb
            // 
            this.rbfInterpolationcSettingsGb.Controls.Add(this.label7);
            this.rbfInterpolationcSettingsGb.Controls.Add(this.regularizationCoefTb);
            this.rbfInterpolationcSettingsGb.Controls.Add(this.label8);
            this.rbfInterpolationcSettingsGb.Controls.Add(this.layersNumTb);
            this.rbfInterpolationcSettingsGb.Controls.Add(this.label9);
            this.rbfInterpolationcSettingsGb.Controls.Add(this.baseRadiusTb);
            this.rbfInterpolationcSettingsGb.Location = new System.Drawing.Point(6, 152);
            this.rbfInterpolationcSettingsGb.Name = "rbfInterpolationcSettingsGb";
            this.rbfInterpolationcSettingsGb.Size = new System.Drawing.Size(255, 139);
            this.rbfInterpolationcSettingsGb.TabIndex = 14;
            this.rbfInterpolationcSettingsGb.TabStop = false;
            this.rbfInterpolationcSettingsGb.Text = "Параметры многослойной РБФ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 26);
            this.label7.TabIndex = 25;
            this.label7.Text = "Регуляризационный\r\n коэффициент";
            // 
            // regularizationCoefTb
            // 
            this.regularizationCoefTb.AllowPromptAsInput = false;
            this.regularizationCoefTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.regularizationCoefTb.Location = new System.Drawing.Point(145, 97);
            this.regularizationCoefTb.Mask = "\\0.000";
            this.regularizationCoefTb.Name = "regularizationCoefTb";
            this.regularizationCoefTb.PromptChar = ' ';
            this.regularizationCoefTb.ResetOnPrompt = false;
            this.regularizationCoefTb.Size = new System.Drawing.Size(100, 20);
            this.regularizationCoefTb.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Количество слоев";
            // 
            // layersNumTb
            // 
            this.layersNumTb.AllowPromptAsInput = false;
            this.layersNumTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.layersNumTb.Location = new System.Drawing.Point(145, 57);
            this.layersNumTb.Mask = "00";
            this.layersNumTb.Name = "layersNumTb";
            this.layersNumTb.PromptChar = ' ';
            this.layersNumTb.ResetOnPrompt = false;
            this.layersNumTb.Size = new System.Drawing.Size(100, 20);
            this.layersNumTb.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Начальный радиус";
            // 
            // baseRadiusTb
            // 
            this.baseRadiusTb.AllowPromptAsInput = false;
            this.baseRadiusTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.baseRadiusTb.Location = new System.Drawing.Point(145, 26);
            this.baseRadiusTb.Mask = "0000";
            this.baseRadiusTb.Name = "baseRadiusTb";
            this.baseRadiusTb.PromptChar = ' ';
            this.baseRadiusTb.ResetOnPrompt = false;
            this.baseRadiusTb.Size = new System.Drawing.Size(100, 20);
            this.baseRadiusTb.TabIndex = 20;
            // 
            // areasOrPointsForAligningCb
            // 
            this.areasOrPointsForAligningCb.AutoSize = true;
            this.areasOrPointsForAligningCb.Location = new System.Drawing.Point(10, 85);
            this.areasOrPointsForAligningCb.Name = "areasOrPointsForAligningCb";
            this.areasOrPointsForAligningCb.Size = new System.Drawing.Size(241, 17);
            this.areasOrPointsForAligningCb.TabIndex = 9;
            this.areasOrPointsForAligningCb.Text = "Использовать области для выравнивания";
            this.areasOrPointsForAligningCb.UseVisualStyleBackColor = true;
            this.areasOrPointsForAligningCb.CheckedChanged += new System.EventHandler(this.pointsOrAreasForAligningCb_CheckedChanged);
            // 
            // surfaceTypeLbl
            // 
            this.surfaceTypeLbl.AutoSize = true;
            this.surfaceTypeLbl.Location = new System.Drawing.Point(7, 112);
            this.surfaceTypeLbl.Name = "surfaceTypeLbl";
            this.surfaceTypeLbl.Size = new System.Drawing.Size(114, 26);
            this.surfaceTypeLbl.TabIndex = 13;
            this.surfaceTypeLbl.Text = "Тип выравнивающей\r\n поверхности";
            // 
            // surfaceTypeCb
            // 
            this.surfaceTypeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.surfaceTypeCb.FormattingEnabled = true;
            this.surfaceTypeCb.Items.AddRange(new object[] {
            "РБФ NN",
            "РБФ NN коэф",
            "РБФ многослойная",
            "РБФ многослойная коэф",
            "Кастомная"});
            this.surfaceTypeCb.Location = new System.Drawing.Point(127, 112);
            this.surfaceTypeCb.Name = "surfaceTypeCb";
            this.surfaceTypeCb.Size = new System.Drawing.Size(134, 21);
            this.surfaceTypeCb.TabIndex = 12;
            this.surfaceTypeCb.SelectedIndexChanged += new System.EventHandler(this.surfaceTypeCb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Область маркера \r\nвыравнивания, отсчетов";
            // 
            // areaSizeTextBox
            // 
            this.areaSizeTextBox.AllowPromptAsInput = false;
            this.areaSizeTextBox.Location = new System.Drawing.Point(161, 49);
            this.areaSizeTextBox.Mask = "00";
            this.areaSizeTextBox.Name = "areaSizeTextBox";
            this.areaSizeTextBox.PromptChar = ' ';
            this.areaSizeTextBox.ResetOnPrompt = false;
            this.areaSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.areaSizeTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Размер сечения, отсчетов";
            // 
            // sectionSizeTextBox
            // 
            this.sectionSizeTextBox.AllowPromptAsInput = false;
            this.sectionSizeTextBox.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.sectionSizeTextBox.Location = new System.Drawing.Point(161, 15);
            this.sectionSizeTextBox.Mask = "0000";
            this.sectionSizeTextBox.Name = "sectionSizeTextBox";
            this.sectionSizeTextBox.PromptChar = ' ';
            this.sectionSizeTextBox.ResetOnPrompt = false;
            this.sectionSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.sectionSizeTextBox.TabIndex = 0;
            // 
            // synthesisTab
            // 
            this.synthesisTab.Controls.Add(this.useEmbeddedServerSarCb);
            this.synthesisTab.Controls.Add(this.forceSynthesisCb);
            this.synthesisTab.Controls.Add(this.serverSarPathAreaGb);
            this.synthesisTab.Controls.Add(this.showServerSarCb);
            this.synthesisTab.Controls.Add(this.showSynthesisCommonTabCb);
            this.synthesisTab.Controls.Add(this.deleteOnCancelCb);
            this.synthesisTab.Location = new System.Drawing.Point(4, 22);
            this.synthesisTab.Name = "synthesisTab";
            this.synthesisTab.Padding = new System.Windows.Forms.Padding(3);
            this.synthesisTab.Size = new System.Drawing.Size(270, 303);
            this.synthesisTab.TabIndex = 4;
            this.synthesisTab.Text = "Синтез";
            this.synthesisTab.UseVisualStyleBackColor = true;
            // 
            // serverSarPathAreaGb
            // 
            this.serverSarPathAreaGb.Controls.Add(this.serverSarPathTb);
            this.serverSarPathAreaGb.Controls.Add(this.button2);
            this.serverSarPathAreaGb.Location = new System.Drawing.Point(6, 220);
            this.serverSarPathAreaGb.Name = "serverSarPathAreaGb";
            this.serverSarPathAreaGb.Size = new System.Drawing.Size(251, 67);
            this.serverSarPathAreaGb.TabIndex = 9;
            this.serverSarPathAreaGb.TabStop = false;
            this.serverSarPathAreaGb.Text = "Путь к программе синтеза";
            // 
            // serverSarPathTb
            // 
            this.serverSarPathTb.Location = new System.Drawing.Point(6, 34);
            this.serverSarPathTb.Name = "serverSarPathTb";
            this.serverSarPathTb.Size = new System.Drawing.Size(205, 20);
            this.serverSarPathTb.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::RlViewer.Properties.Resources.SelectSynthApp;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(217, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(28, 28);
            this.button2.TabIndex = 7;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // showServerSarCb
            // 
            this.showServerSarCb.AutoSize = true;
            this.showServerSarCb.Location = new System.Drawing.Point(6, 52);
            this.showServerSarCb.Name = "showServerSarCb";
            this.showServerSarCb.Size = new System.Drawing.Size(191, 17);
            this.showServerSarCb.TabIndex = 6;
            this.showServerSarCb.Text = "Отображать программу синтеза";
            this.showServerSarCb.UseVisualStyleBackColor = true;
            // 
            // showSynthesisCommonTabCb
            // 
            this.showSynthesisCommonTabCb.AutoSize = true;
            this.showSynthesisCommonTabCb.Location = new System.Drawing.Point(6, 15);
            this.showSynthesisCommonTabCb.Name = "showSynthesisCommonTabCb";
            this.showSynthesisCommonTabCb.Size = new System.Drawing.Size(201, 17);
            this.showSynthesisCommonTabCb.TabIndex = 5;
            this.showSynthesisCommonTabCb.Text = "Расширенные параметры синтеза";
            this.showSynthesisCommonTabCb.UseVisualStyleBackColor = true;
            // 
            // deleteOnCancelCb
            // 
            this.deleteOnCancelCb.AutoSize = true;
            this.deleteOnCancelCb.Location = new System.Drawing.Point(6, 88);
            this.deleteOnCancelCb.Name = "deleteOnCancelCb";
            this.deleteOnCancelCb.Size = new System.Drawing.Size(253, 17);
            this.deleteOnCancelCb.TabIndex = 0;
            this.deleteOnCancelCb.Text = "Удалять синтезированный файл при отмене";
            this.deleteOnCancelCb.UseVisualStyleBackColor = true;
            this.deleteOnCancelCb.CheckedChanged += new System.EventHandler(this.deleteOnCancelCb_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 446);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 26);
            this.label6.TabIndex = 11;
            this.label6.Text = "Область трехмерного\r\nграфика, отсчетов";
            this.label6.Visible = false;
            // 
            // plot3dSizeTb
            // 
            this.plot3dSizeTb.AllowPromptAsInput = false;
            this.plot3dSizeTb.Location = new System.Drawing.Point(183, 462);
            this.plot3dSizeTb.Mask = "000";
            this.plot3dSizeTb.Name = "plot3dSizeTb";
            this.plot3dSizeTb.PromptChar = ' ';
            this.plot3dSizeTb.ResetOnPrompt = false;
            this.plot3dSizeTb.Size = new System.Drawing.Size(100, 20);
            this.plot3dSizeTb.TabIndex = 10;
            this.plot3dSizeTb.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.azimuthCompressCoefTb);
            this.groupBox1.Controls.Add(this.rangeCompressCoefTb);
            this.groupBox1.Location = new System.Drawing.Point(120, 461);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(57, 36);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Коэф. сжатия по азимуту";
            // 
            // azimuthCompressCoefTb
            // 
            this.azimuthCompressCoefTb.AllowPromptAsInput = false;
            this.azimuthCompressCoefTb.Location = new System.Drawing.Point(157, 39);
            this.azimuthCompressCoefTb.Mask = "0";
            this.azimuthCompressCoefTb.Name = "azimuthCompressCoefTb";
            this.azimuthCompressCoefTb.PromptChar = ' ';
            this.azimuthCompressCoefTb.ResetOnPrompt = false;
            this.azimuthCompressCoefTb.Size = new System.Drawing.Size(100, 20);
            this.azimuthCompressCoefTb.TabIndex = 7;
            // 
            // rangeCompressCoefTb
            // 
            this.rangeCompressCoefTb.AllowPromptAsInput = false;
            this.rangeCompressCoefTb.Location = new System.Drawing.Point(157, 13);
            this.rangeCompressCoefTb.Mask = "0";
            this.rangeCompressCoefTb.Name = "rangeCompressCoefTb";
            this.rangeCompressCoefTb.PromptChar = ' ';
            this.rangeCompressCoefTb.ResetOnPrompt = false;
            this.rangeCompressCoefTb.Size = new System.Drawing.Size(100, 20);
            this.rangeCompressCoefTb.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 445);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Коэф. сжатия по дальности";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(12, 347);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ок";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(216, 347);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Отмена";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // forceSynthesisCb
            // 
            this.forceSynthesisCb.AutoSize = true;
            this.forceSynthesisCb.Location = new System.Drawing.Point(6, 126);
            this.forceSynthesisCb.Name = "forceSynthesisCb";
            this.forceSynthesisCb.Size = new System.Drawing.Size(162, 17);
            this.forceSynthesisCb.TabIndex = 10;
            this.forceSynthesisCb.Text = "Приоритет нового синтеза";
            this.forceSynthesisCb.UseVisualStyleBackColor = true;
            this.forceSynthesisCb.CheckedChanged += new System.EventHandler(this.forceSynthesisCb_CheckedChanged);
            // 
            // comboBoxPics1
            // 
            this.comboBoxPics1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxPics1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPics1.FormattingEnabled = true;
            this.comboBoxPics1.Location = new System.Drawing.Point(6, 19);
            this.comboBoxPics1.Name = "comboBoxPics1";
            this.comboBoxPics1.Size = new System.Drawing.Size(73, 21);
            this.comboBoxPics1.TabIndex = 17;
            this.comboBoxPics1.SelectedIndexChanged += new System.EventHandler(this.comboBoxPics1_SelectedIndexChanged);
            // 
            // useEmbeddedServerSarCb
            // 
            this.useEmbeddedServerSarCb.AutoSize = true;
            this.useEmbeddedServerSarCb.Location = new System.Drawing.Point(6, 164);
            this.useEmbeddedServerSarCb.Name = "useEmbeddedServerSarCb";
            this.useEmbeddedServerSarCb.Size = new System.Drawing.Size(199, 17);
            this.useEmbeddedServerSarCb.TabIndex = 11;
            this.useEmbeddedServerSarCb.Text = "Использовать вшитую программу";
            this.useEmbeddedServerSarCb.UseVisualStyleBackColor = true;
            this.useEmbeddedServerSarCb.CheckedChanged += new System.EventHandler(this.useEmbeddedServerSarCb_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 381);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.plot3dSizeTb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.commonTab.ResumeLayout(false);
            this.commonTab.PerformLayout();
            this.viewTab.ResumeLayout(false);
            this.viewTab.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolstab.ResumeLayout(false);
            this.toolstab.PerformLayout();
            this.rbfInterpolationcSettingsGb.ResumeLayout(false);
            this.rbfInterpolationcSettingsGb.PerformLayout();
            this.synthesisTab.ResumeLayout(false);
            this.synthesisTab.PerformLayout();
            this.serverSarPathAreaGb.ResumeLayout(false);
            this.serverSarPathAreaGb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage viewTab;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox inverseCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage toolstab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox sectionSizeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox areaSizeTextBox;
        private System.Windows.Forms.CheckBox logPaletteCb;
        private System.Windows.Forms.CheckBox highResCb;
        private Settings.ComboBoxPics comboBoxPics1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox rangeCompressCoefTb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox azimuthCompressCoefTb;
        private System.Windows.Forms.CheckBox areasOrPointsForAligningCb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox plot3dSizeTb;
        private System.Windows.Forms.TabPage commonTab;
        private System.Windows.Forms.CheckBox adminReminderCb;
        private System.Windows.Forms.CheckBox useCustomFileOpenDlgCb;
        private System.Windows.Forms.Label surfaceTypeLbl;
        private System.Windows.Forms.ComboBox surfaceTypeCb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox tileOutputCb;
        private System.Windows.Forms.CheckBox forceTileGenCheckBox;
        private System.Windows.Forms.CheckBox allowViewCheckBox;
        private System.Windows.Forms.GroupBox rbfInterpolationcSettingsGb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox regularizationCoefTb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox layersNumTb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox baseRadiusTb;
        private System.Windows.Forms.CheckBox forceImageHeightAdjustingCb;
        private System.Windows.Forms.TabPage synthesisTab;
        private System.Windows.Forms.CheckBox showServerSarCb;
        private System.Windows.Forms.CheckBox showSynthesisCommonTabCb;
        private System.Windows.Forms.CheckBox deleteOnCancelCb;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox serverSarPathTb;
        private System.Windows.Forms.GroupBox serverSarPathAreaGb;
        private System.Windows.Forms.CheckBox forceSynthesisCb;
        private System.Windows.Forms.CheckBox useEmbeddedServerSarCb;
    }
}