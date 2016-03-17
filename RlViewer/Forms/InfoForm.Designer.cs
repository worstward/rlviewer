namespace RlViewer.Forms
{
    partial class InfoForm
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
            infoTabsControl = new System.Windows.Forms.TabControl();
            SuspendLayout();
            // 
            // infoTabsControl
            // 
            infoTabsControl.Location = new System.Drawing.Point(12, 12);
            infoTabsControl.Name = "infoTabsControl";
            infoTabsControl.SelectedIndex = 0;
            infoTabsControl.Size = new System.Drawing.Size(545, 351);
            infoTabsControl.TabIndex = 0;
            // 
            // InfoForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(569, 375);
            Controls.Add(infoTabsControl);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
            MaximumSize = new System.Drawing.Size(585, 409);
            Name = "InfoForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Информация о файле";
            KeyDown += new System.Windows.Forms.KeyEventHandler(InfoForm_KeyDown);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl infoTabsControl;

    }
}