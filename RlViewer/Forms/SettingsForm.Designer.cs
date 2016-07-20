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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxPics1 = new RlViewer.Settings.ComboBoxPics();
            this.logPaletteCb = new System.Windows.Forms.CheckBox();
            this.inverseCheckBox = new System.Windows.Forms.CheckBox();
            this.highResCb = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tileOutputCb = new System.Windows.Forms.ComboBox();
            this.forceTileGenCheckBox = new System.Windows.Forms.CheckBox();
            this.allowViewCheckBox = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.areasOrPointsForAligningCb = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.azimuthCompressCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rangeCompressCoefTb = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.areaSizeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sectionSizeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(13, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(278, 187);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.highResCb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(270, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Отображение";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxPics1);
            this.groupBox3.Controls.Add(this.logPaletteCb);
            this.groupBox3.Controls.Add(this.inverseCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 94);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Палитра";
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
            // logPaletteCb
            // 
            this.logPaletteCb.AutoSize = true;
            this.logPaletteCb.Location = new System.Drawing.Point(6, 46);
            this.logPaletteCb.Name = "logPaletteCb";
            this.logPaletteCb.Size = new System.Drawing.Size(128, 17);
            this.logPaletteCb.TabIndex = 15;
            this.logPaletteCb.Text = "Группировать цвета";
            this.logPaletteCb.UseVisualStyleBackColor = true;
            this.logPaletteCb.CheckedChanged += new System.EventHandler(this.logPaletteCb_CheckedChanged);
            // 
            // inverseCheckBox
            // 
            this.inverseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inverseCheckBox.AutoSize = true;
            this.inverseCheckBox.Location = new System.Drawing.Point(6, 69);
            this.inverseCheckBox.Name = "inverseCheckBox";
            this.inverseCheckBox.Size = new System.Drawing.Size(104, 17);
            this.inverseCheckBox.TabIndex = 14;
            this.inverseCheckBox.Text = "Инвертировать";
            this.inverseCheckBox.UseVisualStyleBackColor = true;
            this.inverseCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // highResCb
            // 
            this.highResCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.highResCb.AutoSize = true;
            this.highResCb.Location = new System.Drawing.Point(11, 129);
            this.highResCb.Name = "highResCb";
            this.highResCb.Size = new System.Drawing.Size(253, 17);
            this.highResCb.TabIndex = 16;
            this.highResCb.Text = "Высокое разрешение при масштабировании";
            this.highResCb.UseVisualStyleBackColor = true;
            this.highResCb.CheckedChanged += new System.EventHandler(this.highResCb_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.tileOutputCb);
            this.tabPage2.Controls.Add(this.forceTileGenCheckBox);
            this.tabPage2.Controls.Add(this.allowViewCheckBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(270, 161);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Подготовка вывода";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 3;
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
            this.tileOutputCb.Location = new System.Drawing.Point(6, 94);
            this.tileOutputCb.Name = "tileOutputCb";
            this.tileOutputCb.Size = new System.Drawing.Size(170, 21);
            this.tileOutputCb.TabIndex = 2;
            this.tileOutputCb.SelectedIndexChanged += new System.EventHandler(this.tileOutputCb_SelectedIndexChanged);
            // 
            // forceTileGenCheckBox
            // 
            this.forceTileGenCheckBox.AutoSize = true;
            this.forceTileGenCheckBox.Location = new System.Drawing.Point(6, 41);
            this.forceTileGenCheckBox.Name = "forceTileGenCheckBox";
            this.forceTileGenCheckBox.Size = new System.Drawing.Size(204, 17);
            this.forceTileGenCheckBox.TabIndex = 1;
            this.forceTileGenCheckBox.Text = "Принудительная генерация тайлов";
            this.forceTileGenCheckBox.UseVisualStyleBackColor = true;
            this.forceTileGenCheckBox.CheckedChanged += new System.EventHandler(this.forceTileGenCheckBox_CheckedChanged);
            // 
            // allowViewCheckBox
            // 
            this.allowViewCheckBox.AutoSize = true;
            this.allowViewCheckBox.Location = new System.Drawing.Point(6, 18);
            this.allowViewCheckBox.Name = "allowViewCheckBox";
            this.allowViewCheckBox.Size = new System.Drawing.Size(247, 17);
            this.allowViewCheckBox.TabIndex = 0;
            this.allowViewCheckBox.Text = "Просмотр изображения во время загрузки";
            this.allowViewCheckBox.UseVisualStyleBackColor = true;
            this.allowViewCheckBox.CheckedChanged += new System.EventHandler(this.allowViewCheckBox_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.areasOrPointsForAligningCb);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.areaSizeTextBox);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.sectionSizeTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(270, 161);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Инструменты";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // areasOrPointsForAligningCb
            // 
            this.areasOrPointsForAligningCb.AutoSize = true;
            this.areasOrPointsForAligningCb.Location = new System.Drawing.Point(10, 72);
            this.areasOrPointsForAligningCb.Name = "areasOrPointsForAligningCb";
            this.areasOrPointsForAligningCb.Size = new System.Drawing.Size(241, 17);
            this.areasOrPointsForAligningCb.TabIndex = 9;
            this.areasOrPointsForAligningCb.Text = "Использовать области для выравнивания";
            this.areasOrPointsForAligningCb.UseVisualStyleBackColor = true;
            this.areasOrPointsForAligningCb.CheckedChanged += new System.EventHandler(this.pointsOrAreasForAligningCb_CheckedChanged);
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
            this.azimuthCompressCoefTb.Click += new System.EventHandler(this.azimuthCompressCoefTb_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Коэф. сжатия по дальности";
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
            this.rangeCompressCoefTb.Click += new System.EventHandler(this.rangeCompressCoefTb_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Сторона области \r\nмаркера, отсчетов";
            // 
            // areaSizeTextBox
            // 
            this.areaSizeTextBox.AllowPromptAsInput = false;
            this.areaSizeTextBox.Location = new System.Drawing.Point(161, 46);
            this.areaSizeTextBox.Mask = "0";
            this.areaSizeTextBox.Name = "areaSizeTextBox";
            this.areaSizeTextBox.PromptChar = ' ';
            this.areaSizeTextBox.ResetOnPrompt = false;
            this.areaSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.areaSizeTextBox.TabIndex = 3;
            this.areaSizeTextBox.Click += new System.EventHandler(this.areaSizeTextBox_Click);
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
            this.sectionSizeTextBox.Click += new System.EventHandler(this.sectionSizeTextBox_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(13, 270);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ок";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(216, 270);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Отмена";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.azimuthCompressCoefTb);
            this.groupBox1.Controls.Add(this.rangeCompressCoefTb);
            this.groupBox1.Location = new System.Drawing.Point(4, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 67);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 313);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(319, 384);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox inverseCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox forceTileGenCheckBox;
        private System.Windows.Forms.CheckBox allowViewCheckBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox sectionSizeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox areaSizeTextBox;
        private System.Windows.Forms.CheckBox logPaletteCb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox tileOutputCb;
        private System.Windows.Forms.CheckBox highResCb;
        private Settings.ComboBoxPics comboBoxPics1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox rangeCompressCoefTb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox azimuthCompressCoefTb;
        private System.Windows.Forms.CheckBox areasOrPointsForAligningCb;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}