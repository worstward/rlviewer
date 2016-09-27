namespace RlViewer.Forms
{
    partial class FindPointForm
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
            this.latitudeTb = new System.Windows.Forms.MaskedTextBox();
            this.longtitudeTb = new System.Windows.Forms.MaskedTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.xCoordTb = new System.Windows.Forms.MaskedTextBox();
            this.yCoordTb = new System.Windows.Forms.MaskedTextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.accuracyTb = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // latitudeTb
            // 
            this.latitudeTb.HidePromptOnLeave = true;
            this.latitudeTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.latitudeTb.Location = new System.Drawing.Point(15, 23);
            this.latitudeTb.Mask = "00° 00\' 00\'\' ";
            this.latitudeTb.Name = "latitudeTb";
            this.latitudeTb.PromptChar = ' ';
            this.latitudeTb.Size = new System.Drawing.Size(80, 20);
            this.latitudeTb.TabIndex = 0;
            // 
            // longtitudeTb
            // 
            this.longtitudeTb.HidePromptOnLeave = true;
            this.longtitudeTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.longtitudeTb.Location = new System.Drawing.Point(15, 62);
            this.longtitudeTb.Mask = "000° 00\' 00\'\' ";
            this.longtitudeTb.Name = "longtitudeTb";
            this.longtitudeTb.PromptChar = ' ';
            this.longtitudeTb.Size = new System.Drawing.Size(80, 20);
            this.longtitudeTb.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(245, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.accuracyTb);
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.latitudeTb);
            this.panel2.Controls.Add(this.longtitudeTb);
            this.panel2.Location = new System.Drawing.Point(170, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 134);
            this.panel2.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "E",
            "W"});
            this.comboBox2.Location = new System.Drawing.Point(98, 62);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(39, 20);
            this.comboBox2.TabIndex = 6;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "N",
            "S"});
            this.comboBox1.Location = new System.Drawing.Point(98, 23);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(39, 20);
            this.comboBox1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Долгота";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Широта";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.xCoordTb);
            this.panel1.Controls.Add(this.yCoordTb);
            this.panel1.Location = new System.Drawing.Point(12, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(147, 134);
            this.panel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "X";
            // 
            // xCoordTb
            // 
            this.xCoordTb.HidePromptOnLeave = true;
            this.xCoordTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.xCoordTb.Location = new System.Drawing.Point(30, 39);
            this.xCoordTb.Mask = "000000";
            this.xCoordTb.Name = "xCoordTb";
            this.xCoordTb.PromptChar = ' ';
            this.xCoordTb.Size = new System.Drawing.Size(80, 20);
            this.xCoordTb.TabIndex = 0;
            // 
            // yCoordTb
            // 
            this.yCoordTb.HidePromptOnLeave = true;
            this.yCoordTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.yCoordTb.Location = new System.Drawing.Point(30, 77);
            this.yCoordTb.Mask = "000000";
            this.yCoordTb.Name = "yCoordTb";
            this.yCoordTb.PromptChar = ' ';
            this.yCoordTb.Size = new System.Drawing.Size(80, 20);
            this.yCoordTb.TabIndex = 1;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(147, 17);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Локальные координаты";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(170, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(107, 17);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Гео координаты";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // accuracyTb
            // 
            this.accuracyTb.HidePromptOnLeave = true;
            this.accuracyTb.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Insert;
            this.accuracyTb.Location = new System.Drawing.Point(15, 111);
            this.accuracyTb.Mask = "\\0\\0° \\0\\0\' 00\'\' ";
            this.accuracyTb.Name = "accuracyTb";
            this.accuracyTb.PromptChar = ' ';
            this.accuracyTb.Size = new System.Drawing.Size(80, 20);
            this.accuracyTb.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "С точностью до";
            // 
            // FindPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 227);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FindPointForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Поиск точки";
            this.Shown += new System.EventHandler(this.FindPointForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindPointForm_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox latitudeTb;
        private System.Windows.Forms.MaskedTextBox longtitudeTb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox xCoordTb;
        private System.Windows.Forms.MaskedTextBox yCoordTb;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox accuracyTb;
    }
}