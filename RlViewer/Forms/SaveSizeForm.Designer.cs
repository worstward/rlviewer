﻿namespace RlViewer.Forms
{
    partial class SaveSizeForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.y1CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.x2CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.ySizeCoordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.x1CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.xSizeCoordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.y2CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.widthTextBox = new System.Windows.Forms.MaskedTextBox();
            this.heightTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 391);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 373);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры сохранения";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 51);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(109, 17);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "По координатам";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.y2CoordTextBox);
            this.panel2.Controls.Add(this.x1CoordTextBox);
            this.panel2.Controls.Add(this.x2CoordTextBox);
            this.panel2.Controls.Add(this.y1CoordTextBox);
            this.panel2.Location = new System.Drawing.Point(6, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(261, 135);
            this.panel2.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.heightTextBox);
            this.panel3.Controls.Add(this.widthTextBox);
            this.panel3.Controls.Add(this.xSizeCoordTextBox);
            this.panel3.Controls.Add(this.ySizeCoordTextBox);
            this.panel3.Location = new System.Drawing.Point(6, 234);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(261, 123);
            this.panel3.TabIndex = 5;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 211);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(164, 17);
            this.radioButton3.TabIndex = 6;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "По координатам и размеру";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 28);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(115, 17);
            this.radioButton1.TabIndex = 8;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Все изображение";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // y1CoordTextBox
            // 
            this.y1CoordTextBox.Location = new System.Drawing.Point(9, 83);
            this.y1CoordTextBox.Mask = "0000000";
            this.y1CoordTextBox.Name = "y1CoordTextBox";
            this.y1CoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.y1CoordTextBox.TabIndex = 9;
            // 
            // x2CoordTextBox
            // 
            this.x2CoordTextBox.Location = new System.Drawing.Point(142, 33);
            this.x2CoordTextBox.Mask = "0000000";
            this.x2CoordTextBox.Name = "x2CoordTextBox";
            this.x2CoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.x2CoordTextBox.TabIndex = 10;
            // 
            // ySizeCoordTextBox
            // 
            this.ySizeCoordTextBox.Location = new System.Drawing.Point(9, 79);
            this.ySizeCoordTextBox.Mask = "0000000";
            this.ySizeCoordTextBox.Name = "ySizeCoordTextBox";
            this.ySizeCoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.ySizeCoordTextBox.TabIndex = 11;
            // 
            // x1CoordTextBox
            // 
            this.x1CoordTextBox.Location = new System.Drawing.Point(9, 33);
            this.x1CoordTextBox.Mask = "0000000";
            this.x1CoordTextBox.Name = "x1CoordTextBox";
            this.x1CoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.x1CoordTextBox.TabIndex = 11;
            // 
            // xSizeCoordTextBox
            // 
            this.xSizeCoordTextBox.Location = new System.Drawing.Point(9, 31);
            this.xSizeCoordTextBox.Mask = "0000000";
            this.xSizeCoordTextBox.Name = "xSizeCoordTextBox";
            this.xSizeCoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.xSizeCoordTextBox.TabIndex = 13;
            // 
            // y2CoordTextBox
            // 
            this.y2CoordTextBox.Location = new System.Drawing.Point(142, 83);
            this.y2CoordTextBox.Mask = "0000000";
            this.y2CoordTextBox.Name = "y2CoordTextBox";
            this.y2CoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.y2CoordTextBox.TabIndex = 12;
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(142, 31);
            this.widthTextBox.Mask = "0000000";
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(100, 20);
            this.widthTextBox.TabIndex = 13;
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(142, 79);
            this.heightTextBox.Mask = "0000000";
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(100, 20);
            this.heightTextBox.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "X1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Y1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(139, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "X2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Y2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(139, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Width";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Height";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(204, 391);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SaveSizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 426);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "SaveSizeForm";
            this.Text = "SaveSizeForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MaskedTextBox xSizeCoordTextBox;
        private System.Windows.Forms.MaskedTextBox ySizeCoordTextBox;
        private System.Windows.Forms.MaskedTextBox x1CoordTextBox;
        private System.Windows.Forms.MaskedTextBox x2CoordTextBox;
        private System.Windows.Forms.MaskedTextBox y1CoordTextBox;
        private System.Windows.Forms.MaskedTextBox heightTextBox;
        private System.Windows.Forms.MaskedTextBox widthTextBox;
        private System.Windows.Forms.MaskedTextBox y2CoordTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
    }
}