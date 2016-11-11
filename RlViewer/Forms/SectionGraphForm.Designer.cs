namespace RlViewer.Forms
{
    partial class SectionGraphForm
    {


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.amplitudeRb = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LogRb = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // amplitudeRb
            // 
            this.amplitudeRb.AutoSize = true;
            this.amplitudeRb.Location = new System.Drawing.Point(6, 0);
            this.amplitudeRb.Name = "amplitudeRb";
            this.amplitudeRb.Size = new System.Drawing.Size(127, 17);
            this.amplitudeRb.TabIndex = 0;
            this.amplitudeRb.Text = "Амплитудная шкала";
            this.amplitudeRb.UseVisualStyleBackColor = true;
            this.amplitudeRb.CheckedChanged += new System.EventHandler(this.amplitudeRb_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.LogRb);
            this.groupBox1.Controls.Add(this.amplitudeRb);
            this.groupBox1.Location = new System.Drawing.Point(12, 470);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 26);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // LogRb
            // 
            this.LogRb.AutoSize = true;
            this.LogRb.Location = new System.Drawing.Point(139, 0);
            this.LogRb.Name = "LogRb";
            this.LogRb.Size = new System.Drawing.Size(154, 17);
            this.LogRb.TabIndex = 1;
            this.LogRb.Text = "Логарифмическая шкала";
            this.LogRb.UseVisualStyleBackColor = true;
            this.LogRb.CheckedChanged += new System.EventHandler(this.LogRb_CheckedChanged);
            // 
            // SectionGraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 508);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "SectionGraphForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SectionGraphForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SectionGraphForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton amplitudeRb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton LogRb;
    }
}