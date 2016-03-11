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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.dragRb = new System.Windows.Forms.RadioButton();
            this.markPointRb = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.markAreaRb = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.paramColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            this.openToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.openToolStripMenuItem.Text = "Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
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
            this.brightnessRb.Checked = true;
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
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(18, 89);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(868, 408);
            this.splitContainer1.SplitterDistance = 666;
            this.splitContainer1.TabIndex = 19;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(660, 401);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(747, 49);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 23);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Навигация";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.paramColumn,
            this.valueColumn});
            this.dataGridView1.Location = new System.Drawing.Point(6, 4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(189, 404);
            this.dataGridView1.TabIndex = 0;
            // 
            // paramColumn
            // 
            this.paramColumn.Frozen = true;
            this.paramColumn.HeaderText = "Параметр";
            this.paramColumn.Name = "paramColumn";
            this.paramColumn.ReadOnly = true;
            // 
            // valueColumn
            // 
            this.valueColumn.Frozen = true;
            this.valueColumn.HeaderText = "Значение";
            this.valueColumn.Name = "valueColumn";
            this.valueColumn.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 532);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.loadCancelBtn);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.filterLbl);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn paramColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
    }
}

