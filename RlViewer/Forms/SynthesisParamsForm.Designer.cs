namespace RlViewer.Forms
{
    partial class SynthesisParamsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SynthesisParamsForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.commonPage = new System.Windows.Forms.TabPage();
            this.signalPage = new System.Windows.Forms.TabPage();
            this.normalizingGb = new System.Windows.Forms.GroupBox();
            this.rliNormalizationCoefTb = new System.Windows.Forms.TextBox();
            this.rhgNormalizationCoefTb = new System.Windows.Forms.TextBox();
            this.rliNormalizationCoefLbl = new System.Windows.Forms.Label();
            this.rhgNormalizationCoefLbl = new System.Windows.Forms.Label();
            this.preprocessingGb = new System.Windows.Forms.GroupBox();
            this.radioSuppressionCoefLbl = new System.Windows.Forms.Label();
            this.radioSuppressionLbl = new System.Windows.Forms.Label();
            this.radioSuppressionTb = new System.Windows.Forms.TextBox();
            this.matrixExtensionCb = new System.Windows.Forms.ComboBox();
            this.frameGb = new System.Windows.Forms.GroupBox();
            this.frameSizeAzimuthCb = new System.Windows.Forms.ComboBox();
            this.frameRangeCoefCb = new System.Windows.Forms.ComboBox();
            this.frameRangeCompressionLabel = new System.Windows.Forms.Label();
            this.frameAzimuthCoefCb = new System.Windows.Forms.ComboBox();
            this.frameSizeLabel = new System.Windows.Forms.Label();
            this.frameAzimuthCompressionLabel = new System.Windows.Forms.Label();
            this.blockGb = new System.Windows.Forms.GroupBox();
            this.blockSizeAzimuthCb = new System.Windows.Forms.ComboBox();
            this.blockRangeCoefCb = new System.Windows.Forms.ComboBox();
            this.blockAzimuthCoefCb = new System.Windows.Forms.ComboBox();
            this.blockRangeCompressionLabel = new System.Windows.Forms.Label();
            this.blockAzimuthCompressionLabel = new System.Windows.Forms.Label();
            this.blockSizeLabel = new System.Windows.Forms.Label();
            this.eokPage = new System.Windows.Forms.TabPage();
            this.afPage = new System.Windows.Forms.TabPage();
            this.navigationPage = new System.Windows.Forms.TabPage();
            this.areaPage = new System.Windows.Forms.TabPage();
            this.boxingPage = new System.Windows.Forms.TabPage();
            this.additionalInfoPage = new System.Windows.Forms.TabPage();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.signalPage.SuspendLayout();
            this.normalizingGb.SuspendLayout();
            this.preprocessingGb.SuspendLayout();
            this.frameGb.SuspendLayout();
            this.blockGb.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.commonPage);
            this.tabControl1.Controls.Add(this.signalPage);
            this.tabControl1.Controls.Add(this.eokPage);
            this.tabControl1.Controls.Add(this.afPage);
            this.tabControl1.Controls.Add(this.navigationPage);
            this.tabControl1.Controls.Add(this.areaPage);
            this.tabControl1.Controls.Add(this.boxingPage);
            this.tabControl1.Controls.Add(this.additionalInfoPage);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // commonPage
            // 
            resources.ApplyResources(this.commonPage, "commonPage");
            this.commonPage.Name = "commonPage";
            this.commonPage.UseVisualStyleBackColor = true;
            // 
            // signalPage
            // 
            this.signalPage.Controls.Add(this.normalizingGb);
            this.signalPage.Controls.Add(this.preprocessingGb);
            this.signalPage.Controls.Add(this.frameGb);
            this.signalPage.Controls.Add(this.blockGb);
            resources.ApplyResources(this.signalPage, "signalPage");
            this.signalPage.Name = "signalPage";
            this.signalPage.UseVisualStyleBackColor = true;
            // 
            // normalizingGb
            // 
            this.normalizingGb.Controls.Add(this.rliNormalizationCoefTb);
            this.normalizingGb.Controls.Add(this.rhgNormalizationCoefTb);
            this.normalizingGb.Controls.Add(this.rliNormalizationCoefLbl);
            this.normalizingGb.Controls.Add(this.rhgNormalizationCoefLbl);
            resources.ApplyResources(this.normalizingGb, "normalizingGb");
            this.normalizingGb.Name = "normalizingGb";
            this.normalizingGb.TabStop = false;
            // 
            // rliNormalizationCoefTb
            // 
            resources.ApplyResources(this.rliNormalizationCoefTb, "rliNormalizationCoefTb");
            this.rliNormalizationCoefTb.Name = "rliNormalizationCoefTb";
            // 
            // rhgNormalizationCoefTb
            // 
            resources.ApplyResources(this.rhgNormalizationCoefTb, "rhgNormalizationCoefTb");
            this.rhgNormalizationCoefTb.Name = "rhgNormalizationCoefTb";
            // 
            // rliNormalizationCoefLbl
            // 
            resources.ApplyResources(this.rliNormalizationCoefLbl, "rliNormalizationCoefLbl");
            this.rliNormalizationCoefLbl.Name = "rliNormalizationCoefLbl";
            // 
            // rhgNormalizationCoefLbl
            // 
            resources.ApplyResources(this.rhgNormalizationCoefLbl, "rhgNormalizationCoefLbl");
            this.rhgNormalizationCoefLbl.Name = "rhgNormalizationCoefLbl";
            // 
            // preprocessingGb
            // 
            this.preprocessingGb.Controls.Add(this.radioSuppressionCoefLbl);
            this.preprocessingGb.Controls.Add(this.radioSuppressionLbl);
            this.preprocessingGb.Controls.Add(this.radioSuppressionTb);
            this.preprocessingGb.Controls.Add(this.matrixExtensionCb);
            resources.ApplyResources(this.preprocessingGb, "preprocessingGb");
            this.preprocessingGb.Name = "preprocessingGb";
            this.preprocessingGb.TabStop = false;
            // 
            // radioSuppressionCoefLbl
            // 
            resources.ApplyResources(this.radioSuppressionCoefLbl, "radioSuppressionCoefLbl");
            this.radioSuppressionCoefLbl.Name = "radioSuppressionCoefLbl";
            // 
            // radioSuppressionLbl
            // 
            resources.ApplyResources(this.radioSuppressionLbl, "radioSuppressionLbl");
            this.radioSuppressionLbl.Name = "radioSuppressionLbl";
            // 
            // radioSuppressionTb
            // 
            resources.ApplyResources(this.radioSuppressionTb, "radioSuppressionTb");
            this.radioSuppressionTb.Name = "radioSuppressionTb";
            // 
            // matrixExtensionCb
            // 
            this.matrixExtensionCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.matrixExtensionCb.FormattingEnabled = true;
            resources.ApplyResources(this.matrixExtensionCb, "matrixExtensionCb");
            this.matrixExtensionCb.Name = "matrixExtensionCb";
            // 
            // frameGb
            // 
            this.frameGb.Controls.Add(this.frameSizeAzimuthCb);
            this.frameGb.Controls.Add(this.frameRangeCoefCb);
            this.frameGb.Controls.Add(this.frameRangeCompressionLabel);
            this.frameGb.Controls.Add(this.frameAzimuthCoefCb);
            this.frameGb.Controls.Add(this.frameSizeLabel);
            this.frameGb.Controls.Add(this.frameAzimuthCompressionLabel);
            resources.ApplyResources(this.frameGb, "frameGb");
            this.frameGb.Name = "frameGb";
            this.frameGb.TabStop = false;
            // 
            // frameSizeAzimuthCb
            // 
            this.frameSizeAzimuthCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frameSizeAzimuthCb.FormattingEnabled = true;
            resources.ApplyResources(this.frameSizeAzimuthCb, "frameSizeAzimuthCb");
            this.frameSizeAzimuthCb.Name = "frameSizeAzimuthCb";
            // 
            // frameRangeCoefCb
            // 
            this.frameRangeCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frameRangeCoefCb.FormattingEnabled = true;
            this.frameRangeCoefCb.Items.AddRange(new object[] {
            resources.GetString("frameRangeCoefCb.Items"),
            resources.GetString("frameRangeCoefCb.Items1"),
            resources.GetString("frameRangeCoefCb.Items2"),
            resources.GetString("frameRangeCoefCb.Items3"),
            resources.GetString("frameRangeCoefCb.Items4")});
            resources.ApplyResources(this.frameRangeCoefCb, "frameRangeCoefCb");
            this.frameRangeCoefCb.Name = "frameRangeCoefCb";
            // 
            // frameRangeCompressionLabel
            // 
            resources.ApplyResources(this.frameRangeCompressionLabel, "frameRangeCompressionLabel");
            this.frameRangeCompressionLabel.Name = "frameRangeCompressionLabel";
            // 
            // frameAzimuthCoefCb
            // 
            this.frameAzimuthCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frameAzimuthCoefCb.FormattingEnabled = true;
            this.frameAzimuthCoefCb.Items.AddRange(new object[] {
            resources.GetString("frameAzimuthCoefCb.Items"),
            resources.GetString("frameAzimuthCoefCb.Items1"),
            resources.GetString("frameAzimuthCoefCb.Items2"),
            resources.GetString("frameAzimuthCoefCb.Items3"),
            resources.GetString("frameAzimuthCoefCb.Items4")});
            resources.ApplyResources(this.frameAzimuthCoefCb, "frameAzimuthCoefCb");
            this.frameAzimuthCoefCb.Name = "frameAzimuthCoefCb";
            // 
            // frameSizeLabel
            // 
            resources.ApplyResources(this.frameSizeLabel, "frameSizeLabel");
            this.frameSizeLabel.Name = "frameSizeLabel";
            // 
            // frameAzimuthCompressionLabel
            // 
            resources.ApplyResources(this.frameAzimuthCompressionLabel, "frameAzimuthCompressionLabel");
            this.frameAzimuthCompressionLabel.Name = "frameAzimuthCompressionLabel";
            // 
            // blockGb
            // 
            this.blockGb.Controls.Add(this.blockSizeAzimuthCb);
            this.blockGb.Controls.Add(this.blockRangeCoefCb);
            this.blockGb.Controls.Add(this.blockAzimuthCoefCb);
            this.blockGb.Controls.Add(this.blockRangeCompressionLabel);
            this.blockGb.Controls.Add(this.blockAzimuthCompressionLabel);
            this.blockGb.Controls.Add(this.blockSizeLabel);
            resources.ApplyResources(this.blockGb, "blockGb");
            this.blockGb.Name = "blockGb";
            this.blockGb.TabStop = false;
            // 
            // blockSizeAzimuthCb
            // 
            this.blockSizeAzimuthCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockSizeAzimuthCb.FormattingEnabled = true;
            resources.ApplyResources(this.blockSizeAzimuthCb, "blockSizeAzimuthCb");
            this.blockSizeAzimuthCb.Name = "blockSizeAzimuthCb";
            // 
            // blockRangeCoefCb
            // 
            this.blockRangeCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockRangeCoefCb.FormattingEnabled = true;
            this.blockRangeCoefCb.Items.AddRange(new object[] {
            resources.GetString("blockRangeCoefCb.Items"),
            resources.GetString("blockRangeCoefCb.Items1"),
            resources.GetString("blockRangeCoefCb.Items2"),
            resources.GetString("blockRangeCoefCb.Items3"),
            resources.GetString("blockRangeCoefCb.Items4")});
            resources.ApplyResources(this.blockRangeCoefCb, "blockRangeCoefCb");
            this.blockRangeCoefCb.Name = "blockRangeCoefCb";
            // 
            // blockAzimuthCoefCb
            // 
            this.blockAzimuthCoefCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockAzimuthCoefCb.FormattingEnabled = true;
            this.blockAzimuthCoefCb.Items.AddRange(new object[] {
            resources.GetString("blockAzimuthCoefCb.Items"),
            resources.GetString("blockAzimuthCoefCb.Items1"),
            resources.GetString("blockAzimuthCoefCb.Items2"),
            resources.GetString("blockAzimuthCoefCb.Items3"),
            resources.GetString("blockAzimuthCoefCb.Items4")});
            resources.ApplyResources(this.blockAzimuthCoefCb, "blockAzimuthCoefCb");
            this.blockAzimuthCoefCb.Name = "blockAzimuthCoefCb";
            // 
            // blockRangeCompressionLabel
            // 
            resources.ApplyResources(this.blockRangeCompressionLabel, "blockRangeCompressionLabel");
            this.blockRangeCompressionLabel.Name = "blockRangeCompressionLabel";
            // 
            // blockAzimuthCompressionLabel
            // 
            resources.ApplyResources(this.blockAzimuthCompressionLabel, "blockAzimuthCompressionLabel");
            this.blockAzimuthCompressionLabel.Name = "blockAzimuthCompressionLabel";
            // 
            // blockSizeLabel
            // 
            resources.ApplyResources(this.blockSizeLabel, "blockSizeLabel");
            this.blockSizeLabel.Name = "blockSizeLabel";
            // 
            // eokPage
            // 
            resources.ApplyResources(this.eokPage, "eokPage");
            this.eokPage.Name = "eokPage";
            this.eokPage.UseVisualStyleBackColor = true;
            // 
            // afPage
            // 
            resources.ApplyResources(this.afPage, "afPage");
            this.afPage.Name = "afPage";
            this.afPage.UseVisualStyleBackColor = true;
            // 
            // navigationPage
            // 
            resources.ApplyResources(this.navigationPage, "navigationPage");
            this.navigationPage.Name = "navigationPage";
            this.navigationPage.UseVisualStyleBackColor = true;
            // 
            // areaPage
            // 
            resources.ApplyResources(this.areaPage, "areaPage");
            this.areaPage.Name = "areaPage";
            this.areaPage.UseVisualStyleBackColor = true;
            // 
            // boxingPage
            // 
            resources.ApplyResources(this.boxingPage, "boxingPage");
            this.boxingPage.Name = "boxingPage";
            this.boxingPage.UseVisualStyleBackColor = true;
            // 
            // additionalInfoPage
            // 
            resources.ApplyResources(this.additionalInfoPage, "additionalInfoPage");
            this.additionalInfoPage.Name = "additionalInfoPage";
            this.additionalInfoPage.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            resources.ApplyResources(this.okBtn, "okBtn");
            this.okBtn.Name = "okBtn";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            resources.ApplyResources(this.cancelBtn, "cancelBtn");
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // SynthesisParamsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SynthesisParamsForm";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SynthesisParams_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.signalPage.ResumeLayout(false);
            this.normalizingGb.ResumeLayout(false);
            this.normalizingGb.PerformLayout();
            this.preprocessingGb.ResumeLayout(false);
            this.preprocessingGb.PerformLayout();
            this.frameGb.ResumeLayout(false);
            this.frameGb.PerformLayout();
            this.blockGb.ResumeLayout(false);
            this.blockGb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage commonPage;
        private System.Windows.Forms.TabPage signalPage;
        private System.Windows.Forms.TabPage eokPage;
        private System.Windows.Forms.TabPage afPage;
        private System.Windows.Forms.TabPage navigationPage;
        private System.Windows.Forms.TabPage areaPage;
        private System.Windows.Forms.TabPage boxingPage;
        private System.Windows.Forms.TabPage additionalInfoPage;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.GroupBox frameGb;
        private System.Windows.Forms.ComboBox frameSizeAzimuthCb;
        private System.Windows.Forms.ComboBox frameRangeCoefCb;
        private System.Windows.Forms.Label frameRangeCompressionLabel;
        private System.Windows.Forms.ComboBox frameAzimuthCoefCb;
        private System.Windows.Forms.Label frameSizeLabel;
        private System.Windows.Forms.Label frameAzimuthCompressionLabel;
        private System.Windows.Forms.GroupBox blockGb;
        private System.Windows.Forms.ComboBox blockSizeAzimuthCb;
        private System.Windows.Forms.ComboBox blockRangeCoefCb;
        private System.Windows.Forms.ComboBox blockAzimuthCoefCb;
        private System.Windows.Forms.Label blockRangeCompressionLabel;
        private System.Windows.Forms.Label blockAzimuthCompressionLabel;
        private System.Windows.Forms.Label blockSizeLabel;
        private System.Windows.Forms.GroupBox normalizingGb;
        private System.Windows.Forms.TextBox rliNormalizationCoefTb;
        private System.Windows.Forms.TextBox rhgNormalizationCoefTb;
        private System.Windows.Forms.Label rliNormalizationCoefLbl;
        private System.Windows.Forms.Label rhgNormalizationCoefLbl;
        private System.Windows.Forms.GroupBox preprocessingGb;
        private System.Windows.Forms.Label radioSuppressionCoefLbl;
        private System.Windows.Forms.Label radioSuppressionLbl;
        private System.Windows.Forms.TextBox radioSuppressionTb;
        private System.Windows.Forms.ComboBox matrixExtensionCb;
    }
}