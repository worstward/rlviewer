namespace RlViewer
{
    partial class InfoFrm
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
            this.infoTabsControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // infoTabsControl
            // 
            this.infoTabsControl.Location = new System.Drawing.Point(12, 12);
            this.infoTabsControl.Name = "infoTabsControl";
            this.infoTabsControl.SelectedIndex = 0;
            this.infoTabsControl.Size = new System.Drawing.Size(545, 351);
            this.infoTabsControl.TabIndex = 0;
            // 
            // InfoFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 375);
            this.Controls.Add(this.infoTabsControl);
            this.Name = "InfoFrm";
            this.Text = "InfoFrm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl infoTabsControl;

    }
}