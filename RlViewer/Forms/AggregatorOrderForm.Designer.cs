namespace RlViewer.Forms
{
    partial class AggregatorOrderForm
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
            this.sourceFilesListBox = new System.Windows.Forms.ListBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.upBtn = new System.Windows.Forms.Button();
            this.downBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sourceFilesListBox
            // 
            this.sourceFilesListBox.FormattingEnabled = true;
            this.sourceFilesListBox.Location = new System.Drawing.Point(12, 12);
            this.sourceFilesListBox.Name = "sourceFilesListBox";
            this.sourceFilesListBox.Size = new System.Drawing.Size(275, 173);
            this.sourceFilesListBox.TabIndex = 0;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(12, 266);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 3;
            this.okBtn.Text = "Ok";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // upBtn
            // 
            this.upBtn.BackgroundImage = global::RlViewer.Properties.Resources.Up;
            this.upBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.upBtn.Location = new System.Drawing.Point(293, 12);
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(33, 35);
            this.upBtn.TabIndex = 2;
            this.upBtn.UseVisualStyleBackColor = true;
            this.upBtn.Click += new System.EventHandler(this.upBtn_Click);
            // 
            // downBtn
            // 
            this.downBtn.BackgroundImage = global::RlViewer.Properties.Resources.Down;
            this.downBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.downBtn.Location = new System.Drawing.Point(293, 148);
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(33, 37);
            this.downBtn.TabIndex = 1;
            this.downBtn.UseVisualStyleBackColor = true;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(251, 266);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // AggregatorOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 301);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.upBtn);
            this.Controls.Add(this.downBtn);
            this.Controls.Add(this.sourceFilesListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "AggregatorOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Порядок совмещения";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AggregatorOrderForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox sourceFilesListBox;
        private System.Windows.Forms.Button downBtn;
        private System.Windows.Forms.Button upBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}