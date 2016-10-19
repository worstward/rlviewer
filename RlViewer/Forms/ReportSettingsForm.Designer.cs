namespace RlViewer.Forms
{
    partial class ReportSettingsForm
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
            this.createReportBtn = new System.Windows.Forms.Button();
            this.cancelReportBtn = new System.Windows.Forms.Button();
            this.firstLineTb = new System.Windows.Forms.MaskedTextBox();
            this.lastLineTb = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.finishAtLastCb = new System.Windows.Forms.CheckBox();
            this.reportTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.timeCb = new System.Windows.Forms.CheckBox();
            this.headerInfoCb = new System.Windows.Forms.CheckBox();
            this.areaCb = new System.Windows.Forms.CheckBox();
            this.centerCb = new System.Windows.Forms.CheckBox();
            this.cornersCb = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // createReportBtn
            // 
            this.createReportBtn.Location = new System.Drawing.Point(12, 223);
            this.createReportBtn.Name = "createReportBtn";
            this.createReportBtn.Size = new System.Drawing.Size(75, 26);
            this.createReportBtn.TabIndex = 0;
            this.createReportBtn.Text = "Создать";
            this.createReportBtn.UseVisualStyleBackColor = true;
            this.createReportBtn.Click += new System.EventHandler(this.createReportBtn_Click);
            // 
            // cancelReportBtn
            // 
            this.cancelReportBtn.Location = new System.Drawing.Point(196, 224);
            this.cancelReportBtn.Name = "cancelReportBtn";
            this.cancelReportBtn.Size = new System.Drawing.Size(76, 25);
            this.cancelReportBtn.TabIndex = 1;
            this.cancelReportBtn.Text = "Отмена";
            this.cancelReportBtn.UseVisualStyleBackColor = true;
            this.cancelReportBtn.Click += new System.EventHandler(this.cancelReportBtn_Click);
            // 
            // firstLineTb
            // 
            this.firstLineTb.AllowPromptAsInput = false;
            this.firstLineTb.HidePromptOnLeave = true;
            this.firstLineTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.firstLineTb.Location = new System.Drawing.Point(180, 11);
            this.firstLineTb.Mask = "000000";
            this.firstLineTb.Name = "firstLineTb";
            this.firstLineTb.PromptChar = ' ';
            this.firstLineTb.ResetOnPrompt = false;
            this.firstLineTb.Size = new System.Drawing.Size(61, 20);
            this.firstLineTb.TabIndex = 2;
            this.firstLineTb.Text = "0";
            // 
            // lastLineTb
            // 
            this.lastLineTb.AllowPromptAsInput = false;
            this.lastLineTb.HidePromptOnLeave = true;
            this.lastLineTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.lastLineTb.Location = new System.Drawing.Point(180, 40);
            this.lastLineTb.Mask = "000000";
            this.lastLineTb.Name = "lastLineTb";
            this.lastLineTb.PromptChar = ' ';
            this.lastLineTb.ResetOnPrompt = false;
            this.lastLineTb.Size = new System.Drawing.Size(61, 20);
            this.lastLineTb.TabIndex = 3;
            this.lastLineTb.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Отступ от начала файла, строк";
            // 
            // finishAtLastCb
            // 
            this.finishAtLastCb.AutoSize = true;
            this.finishAtLastCb.Location = new System.Drawing.Point(9, 71);
            this.finishAtLastCb.Name = "finishAtLastCb";
            this.finishAtLastCb.Size = new System.Drawing.Size(139, 17);
            this.finishAtLastCb.TabIndex = 6;
            this.finishAtLastCb.Text = "Читать файл до конца";
            this.finishAtLastCb.UseVisualStyleBackColor = true;
            this.finishAtLastCb.CheckedChanged += new System.EventHandler(this.finishAtLastCb_CheckedChanged);
            // 
            // reportTypeComboBox
            // 
            this.reportTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportTypeComboBox.FormattingEnabled = true;
            this.reportTypeComboBox.Location = new System.Drawing.Point(120, 99);
            this.reportTypeComboBox.Name = "reportTypeComboBox";
            this.reportTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.reportTypeComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Тип отчета";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(260, 182);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.firstLineTb);
            this.tabPage1.Controls.Add(this.reportTypeComboBox);
            this.tabPage1.Controls.Add(this.lastLineTb);
            this.tabPage1.Controls.Add(this.finishAtLastCb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(252, 156);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основные параметры";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.timeCb);
            this.tabPage2.Controls.Add(this.headerInfoCb);
            this.tabPage2.Controls.Add(this.areaCb);
            this.tabPage2.Controls.Add(this.centerCb);
            this.tabPage2.Controls.Add(this.cornersCb);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(252, 156);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Блоки отчета";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // timeCb
            // 
            this.timeCb.AutoSize = true;
            this.timeCb.Location = new System.Drawing.Point(7, 108);
            this.timeCb.Name = "timeCb";
            this.timeCb.Size = new System.Drawing.Size(99, 17);
            this.timeCb.TabIndex = 4;
            this.timeCb.Text = "Время работы";
            this.timeCb.UseVisualStyleBackColor = true;
            // 
            // headerInfoCb
            // 
            this.headerInfoCb.AutoSize = true;
            this.headerInfoCb.Location = new System.Drawing.Point(7, 85);
            this.headerInfoCb.Name = "headerInfoCb";
            this.headerInfoCb.Size = new System.Drawing.Size(110, 17);
            this.headerInfoCb.TabIndex = 3;
            this.headerInfoCb.Text = "Инфо заголовка";
            this.headerInfoCb.UseVisualStyleBackColor = true;
            // 
            // areaCb
            // 
            this.areaCb.AutoSize = true;
            this.areaCb.Location = new System.Drawing.Point(7, 62);
            this.areaCb.Name = "areaCb";
            this.areaCb.Size = new System.Drawing.Size(123, 17);
            this.areaCb.TabIndex = 2;
            this.areaCb.Text = "Площадь засветки";
            this.areaCb.UseVisualStyleBackColor = true;
            // 
            // centerCb
            // 
            this.centerCb.AutoSize = true;
            this.centerCb.Location = new System.Drawing.Point(7, 39);
            this.centerCb.Name = "centerCb";
            this.centerCb.Size = new System.Drawing.Size(86, 17);
            this.centerCb.TabIndex = 1;
            this.centerCb.Text = "Центр зоны";
            this.centerCb.UseVisualStyleBackColor = true;
            // 
            // cornersCb
            // 
            this.cornersCb.AutoSize = true;
            this.cornersCb.Location = new System.Drawing.Point(7, 16);
            this.cornersCb.Name = "cornersCb";
            this.cornersCb.Size = new System.Drawing.Size(82, 17);
            this.cornersCb.TabIndex = 0;
            this.cornersCb.Text = "Углы зоны";
            this.cornersCb.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Отступ от конца файла, строк";
            // 
            // ReportSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelReportBtn);
            this.Controls.Add(this.createReportBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "ReportSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры отчета";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReportSettingsForm_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createReportBtn;
        private System.Windows.Forms.Button cancelReportBtn;
        private System.Windows.Forms.MaskedTextBox firstLineTb;
        private System.Windows.Forms.MaskedTextBox lastLineTb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox finishAtLastCb;
        private System.Windows.Forms.ComboBox reportTypeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox headerInfoCb;
        private System.Windows.Forms.CheckBox areaCb;
        private System.Windows.Forms.CheckBox centerCb;
        private System.Windows.Forms.CheckBox cornersCb;
        private System.Windows.Forms.CheckBox timeCb;
        private System.Windows.Forms.Label label3;
    }
}