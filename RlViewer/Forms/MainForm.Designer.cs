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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.dragRb = new System.Windows.Forms.RadioButton();
            this.markPointRb = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.markAreaRb = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оФайлеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.логToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gammaCorrRb = new System.Windows.Forms.RadioButton();
            this.contrastRb = new System.Windows.Forms.RadioButton();
            this.brightnessRb = new System.Windows.Forms.RadioButton();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.filterLbl = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.loadCancelBtn = new System.Windows.Forms.Button();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 93);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(874, 410);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(12, 506);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(874, 17);
            this.hScrollBar1.TabIndex = 3;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(889, 93);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 410);
            this.vScrollBar1.TabIndex = 4;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // dragRb
            // 
            this.dragRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.dragRb.AutoSize = true;
            this.dragRb.Location = new System.Drawing.Point(6, 19);
            this.dragRb.Name = "dragRb";
            this.dragRb.Size = new System.Drawing.Size(40, 23);
            this.dragRb.TabIndex = 5;
            this.dragRb.TabStop = true;
            this.dragRb.Text = "Drag";
            this.dragRb.UseVisualStyleBackColor = true;
            // 
            // markPointRb
            // 
            this.markPointRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.markPointRb.AutoSize = true;
            this.markPointRb.Location = new System.Drawing.Point(52, 19);
            this.markPointRb.Name = "markPointRb";
            this.markPointRb.Size = new System.Drawing.Size(49, 23);
            this.markPointRb.TabIndex = 6;
            this.markPointRb.TabStop = true;
            this.markPointRb.Text = "Метка";
            this.markPointRb.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.markAreaRb);
            this.groupBox1.Controls.Add(this.dragRb);
            this.groupBox1.Controls.Add(this.markPointRb);
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 58);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // markAreaRb
            // 
            this.markAreaRb.Appearance = System.Windows.Forms.Appearance.Button;
            this.markAreaRb.AutoSize = true;
            this.markAreaRb.Location = new System.Drawing.Point(107, 19);
            this.markAreaRb.Name = "markAreaRb";
            this.markAreaRb.Size = new System.Drawing.Size(60, 23);
            this.markAreaRb.TabIndex = 7;
            this.markAreaRb.TabStop = true;
            this.markAreaRb.Text = "Область";
            this.markAreaRb.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(915, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.сохранитьToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
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
            this.логToolStripMenuItem1});
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.infoToolStripMenuItem.Text = "Инфо";
            // 
            // оФайлеToolStripMenuItem
            // 
            this.оФайлеToolStripMenuItem.Name = "оФайлеToolStripMenuItem";
            this.оФайлеToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.оФайлеToolStripMenuItem.Text = "О файле";
            this.оФайлеToolStripMenuItem.Click += new System.EventHandler(this.оФайлеToolStripMenuItem_Click);
            // 
            // логToolStripMenuItem1
            // 
            this.логToolStripMenuItem1.Name = "логToolStripMenuItem1";
            this.логToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.логToolStripMenuItem1.Text = "Лог";
            this.логToolStripMenuItem1.Click += new System.EventHandler(this.логToolStripMenuItem1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gammaCorrRb);
            this.groupBox2.Controls.Add(this.contrastRb);
            this.groupBox2.Controls.Add(this.brightnessRb);
            this.groupBox2.Location = new System.Drawing.Point(195, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 58);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Фильтр";
            // 
            // gammaCorrRb
            // 
            this.gammaCorrRb.AutoSize = true;
            this.gammaCorrRb.Location = new System.Drawing.Point(158, 20);
            this.gammaCorrRb.Name = "gammaCorrRb";
            this.gammaCorrRb.Size = new System.Drawing.Size(116, 17);
            this.gammaCorrRb.TabIndex = 2;
            this.gammaCorrRb.TabStop = true;
            this.gammaCorrRb.Text = "Гамма коррекция";
            this.gammaCorrRb.UseVisualStyleBackColor = true;
            this.gammaCorrRb.CheckedChanged += new System.EventHandler(this.gammaCorrRb_CheckedChanged);
            // 
            // contrastRb
            // 
            this.contrastRb.AutoSize = true;
            this.contrastRb.Location = new System.Drawing.Point(87, 20);
            this.contrastRb.Name = "contrastRb";
            this.contrastRb.Size = new System.Drawing.Size(72, 17);
            this.contrastRb.TabIndex = 1;
            this.contrastRb.TabStop = true;
            this.contrastRb.Text = "Контраст";
            this.contrastRb.UseVisualStyleBackColor = true;
            this.contrastRb.CheckedChanged += new System.EventHandler(this.contrastRb_CheckedChanged);
            // 
            // brightnessRb
            // 
            this.brightnessRb.AutoSize = true;
            this.brightnessRb.Location = new System.Drawing.Point(7, 20);
            this.brightnessRb.Name = "brightnessRb";
            this.brightnessRb.Size = new System.Drawing.Size(68, 17);
            this.brightnessRb.TabIndex = 0;
            this.brightnessRb.TabStop = true;
            this.brightnessRb.Text = "Яркость";
            this.brightnessRb.UseVisualStyleBackColor = true;
            this.brightnessRb.CheckedChanged += new System.EventHandler(this.brightnessRb_CheckedChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(471, 38);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(212, 45);
            this.trackBar1.TabIndex = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // filterLbl
            // 
            this.filterLbl.AutoSize = true;
            this.filterLbl.Location = new System.Drawing.Point(477, 70);
            this.filterLbl.Name = "filterLbl";
            this.filterLbl.Size = new System.Drawing.Size(100, 13);
            this.filterLbl.TabIndex = 11;
            this.filterLbl.Text = "Уровень фильтра:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 479);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(792, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // loadCancelBtn
            // 
            this.loadCancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadCancelBtn.Location = new System.Drawing.Point(847, 479);
            this.loadCancelBtn.Name = "loadCancelBtn";
            this.loadCancelBtn.Size = new System.Drawing.Size(56, 23);
            this.loadCancelBtn.TabIndex = 17;
            this.loadCancelBtn.Text = "Отмена";
            this.loadCancelBtn.UseVisualStyleBackColor = true;
            this.loadCancelBtn.Click += new System.EventHandler(this.loadCancelBtn_Click);
            // 
            // percentageLabel
            // 
            this.percentageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.percentageLabel.AutoSize = true;
            this.percentageLabel.BackColor = System.Drawing.Color.Transparent;
            this.percentageLabel.Location = new System.Drawing.Point(817, 484);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(24, 13);
            this.percentageLabel.TabIndex = 18;
            this.percentageLabel.Text = "0 %";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 532);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.loadCancelBtn);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.filterLbl);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.RadioButton dragRb;
        private System.Windows.Forms.RadioButton markPointRb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton gammaCorrRb;
        private System.Windows.Forms.RadioButton contrastRb;
        private System.Windows.Forms.RadioButton brightnessRb;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label filterLbl;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button loadCancelBtn;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.Label percentageLabel;
        private System.Windows.Forms.RadioButton markAreaRb;
        private System.Windows.Forms.ToolStripMenuItem оФайлеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem логToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
    }
}

