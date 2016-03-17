namespace RlViewer.Forms
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
            button1 = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButton3 = new System.Windows.Forms.RadioButton();
            radioButton1 = new System.Windows.Forms.RadioButton();
            radioButton2 = new System.Windows.Forms.RadioButton();
            panel3 = new System.Windows.Forms.Panel();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            heightTextBox = new System.Windows.Forms.MaskedTextBox();
            widthTextBox = new System.Windows.Forms.MaskedTextBox();
            xSizeCoordTextBox = new System.Windows.Forms.MaskedTextBox();
            ySizeCoordTextBox = new System.Windows.Forms.MaskedTextBox();
            panel2 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            y2CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            x1CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            x2CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            y1CoordTextBox = new System.Windows.Forms.MaskedTextBox();
            button2 = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(18, 391);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 9;
            button1.Text = "Ok";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(panel3);
            groupBox1.Controls.Add(panel2);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(273, 373);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Параметры сохранения";
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new System.Drawing.Point(6, 211);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new System.Drawing.Size(164, 17);
            radioButton3.TabIndex = 6;
            radioButton3.TabStop = true;
            radioButton3.Text = "По координатам и размеру";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(6, 28);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(115, 17);
            radioButton1.TabIndex = 8;
            radioButton1.TabStop = true;
            radioButton1.Text = "Все изображение";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += new System.EventHandler(radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(6, 51);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(109, 17);
            radioButton2.TabIndex = 4;
            radioButton2.TabStop = true;
            radioButton2.Text = "По координатам";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += new System.EventHandler(radioButton2_CheckedChanged);
            // 
            // panel3
            // 
            panel3.Controls.Add(label8);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(heightTextBox);
            panel3.Controls.Add(widthTextBox);
            panel3.Controls.Add(xSizeCoordTextBox);
            panel3.Controls.Add(ySizeCoordTextBox);
            panel3.Location = new System.Drawing.Point(6, 234);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(261, 123);
            panel3.TabIndex = 5;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(139, 63);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(38, 13);
            label8.TabIndex = 18;
            label8.Text = "Height";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(139, 15);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(35, 13);
            label7.TabIndex = 17;
            label7.Text = "Width";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(9, 63);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(14, 13);
            label6.TabIndex = 16;
            label6.Text = "Y";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(9, 15);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(14, 13);
            label5.TabIndex = 15;
            label5.Text = "X";
            // 
            // heightTextBox
            // 
            heightTextBox.Location = new System.Drawing.Point(142, 79);
            heightTextBox.Mask = "0000000";
            heightTextBox.Name = "heightTextBox";
            heightTextBox.Size = new System.Drawing.Size(100, 20);
            heightTextBox.TabIndex = 8;
            // 
            // widthTextBox
            // 
            widthTextBox.Location = new System.Drawing.Point(142, 31);
            widthTextBox.Mask = "0000000";
            widthTextBox.Name = "widthTextBox";
            widthTextBox.Size = new System.Drawing.Size(100, 20);
            widthTextBox.TabIndex = 7;
            // 
            // xSizeCoordTextBox
            // 
            xSizeCoordTextBox.Location = new System.Drawing.Point(9, 31);
            xSizeCoordTextBox.Mask = "0000000";
            xSizeCoordTextBox.Name = "xSizeCoordTextBox";
            xSizeCoordTextBox.Size = new System.Drawing.Size(100, 20);
            xSizeCoordTextBox.TabIndex = 5;
            // 
            // ySizeCoordTextBox
            // 
            ySizeCoordTextBox.Location = new System.Drawing.Point(9, 79);
            ySizeCoordTextBox.Mask = "0000000";
            ySizeCoordTextBox.Name = "ySizeCoordTextBox";
            ySizeCoordTextBox.Size = new System.Drawing.Size(100, 20);
            ySizeCoordTextBox.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(y2CoordTextBox);
            panel2.Controls.Add(x1CoordTextBox);
            panel2.Controls.Add(x2CoordTextBox);
            panel2.Controls.Add(y1CoordTextBox);
            panel2.Location = new System.Drawing.Point(6, 74);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(261, 135);
            panel2.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(139, 67);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(20, 13);
            label4.TabIndex = 16;
            label4.Text = "Y2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(139, 17);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(20, 13);
            label3.TabIndex = 15;
            label3.Text = "X2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(9, 67);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(20, 13);
            label2.TabIndex = 14;
            label2.Text = "Y1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(20, 13);
            label1.TabIndex = 13;
            label1.Text = "X1";
            // 
            // y2CoordTextBox
            // 
            y2CoordTextBox.Location = new System.Drawing.Point(142, 83);
            y2CoordTextBox.Mask = "0000000";
            y2CoordTextBox.Name = "y2CoordTextBox";
            y2CoordTextBox.Size = new System.Drawing.Size(100, 20);
            y2CoordTextBox.TabIndex = 4;
            // 
            // x1CoordTextBox
            // 
            x1CoordTextBox.Location = new System.Drawing.Point(9, 33);
            x1CoordTextBox.Mask = "0000000";
            x1CoordTextBox.Name = "x1CoordTextBox";
            x1CoordTextBox.Size = new System.Drawing.Size(100, 20);
            x1CoordTextBox.TabIndex = 1;
            // 
            // x2CoordTextBox
            // 
            x2CoordTextBox.Location = new System.Drawing.Point(142, 33);
            x2CoordTextBox.Mask = "0000000";
            x2CoordTextBox.Name = "x2CoordTextBox";
            x2CoordTextBox.Size = new System.Drawing.Size(100, 20);
            x2CoordTextBox.TabIndex = 3;
            // 
            // y1CoordTextBox
            // 
            y1CoordTextBox.Location = new System.Drawing.Point(9, 83);
            y1CoordTextBox.Mask = "0000000";
            y1CoordTextBox.Name = "y1CoordTextBox";
            y1CoordTextBox.Size = new System.Drawing.Size(100, 20);
            y1CoordTextBox.TabIndex = 2;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(204, 391);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 10;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // SaveSizeForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(297, 426);
            Controls.Add(button2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
            Name = "SaveSizeForm";
            Text = "SaveSizeForm";
            KeyDown += new System.Windows.Forms.KeyEventHandler(SaveSizeForm_KeyDown);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);

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