namespace AssetStudioGUI
{
    partial class ImportOptionsForm
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
            this.components = new System.ComponentModel.Container();
            this.cachePathLabel = new System.Windows.Forms.Label();
            this.cachePathSelectButton = new System.Windows.Forms.Button();
            this.cachePathResetButton = new System.Windows.Forms.Button();
            this.unityVersionGroupBox = new System.Windows.Forms.GroupBox();
            this.customUnityVersionTextBox = new System.Windows.Forms.TextBox();
            this.customUnityVersionLabel = new System.Windows.Forms.Label();
            this.unityVerDetectionModePanel = new System.Windows.Forms.Panel();
            this.unityVerAutoRadioButton = new System.Windows.Forms.RadioButton();
            this.unityVerLegacy2RadioButton = new System.Windows.Forms.RadioButton();
            this.unityVerLegacyRadioButton = new System.Windows.Forms.RadioButton();
            this.unityVerDetectionModeLabel = new System.Windows.Forms.Label();
            this.decompressionGroupBox = new System.Windows.Forms.GroupBox();
            this.customBlockCompressionComboBox = new System.Windows.Forms.ComboBox();
            this.customBlockCompressionLabel = new System.Windows.Forms.Label();
            this.customBlockInfoCompressionComboBox = new System.Windows.Forms.ComboBox();
            this.customBlockInfoCompressionLabel = new System.Windows.Forms.Label();
            this.cacheFreeSpaceLabel = new System.Windows.Forms.Label();
            this.alwaysDecompressToDiskCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.closeButton = new System.Windows.Forms.Button();
            this.saveToDiskButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cacheGroupBox = new System.Windows.Forms.GroupBox();
            this.unityVersionGroupBox.SuspendLayout();
            this.unityVerDetectionModePanel.SuspendLayout();
            this.decompressionGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.cacheGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cachePathLabel
            // 
            this.cachePathLabel.AutoEllipsis = true;
            this.cachePathLabel.Location = new System.Drawing.Point(6, 19);
            this.cachePathLabel.Name = "cachePathLabel";
            this.cachePathLabel.Size = new System.Drawing.Size(348, 20);
            this.cachePathLabel.TabIndex = 0;
            this.cachePathLabel.Text = "Cache directory: <program folder>";
            // 
            // cachePathSelectButton
            // 
            this.cachePathSelectButton.Location = new System.Drawing.Point(9, 42);
            this.cachePathSelectButton.Name = "cachePathSelectButton";
            this.cachePathSelectButton.Size = new System.Drawing.Size(60, 25);
            this.cachePathSelectButton.TabIndex = 1;
            this.cachePathSelectButton.Text = "Select";
            this.cachePathSelectButton.UseVisualStyleBackColor = false;
            this.cachePathSelectButton.Click += new System.EventHandler(this.cachePathSelectButton_Click);
            // 
            // cachePathResetButton
            // 
            this.cachePathResetButton.Location = new System.Drawing.Point(75, 42);
            this.cachePathResetButton.Name = "cachePathResetButton";
            this.cachePathResetButton.Size = new System.Drawing.Size(60, 25);
            this.cachePathResetButton.TabIndex = 2;
            this.cachePathResetButton.Text = "Reset";
            this.cachePathResetButton.UseVisualStyleBackColor = false;
            this.cachePathResetButton.Click += new System.EventHandler(this.cachePathResetButton_Click);
            // 
            // unityVersionGroupBox
            // 
            this.unityVersionGroupBox.Controls.Add(this.customUnityVersionTextBox);
            this.unityVersionGroupBox.Controls.Add(this.customUnityVersionLabel);
            this.unityVersionGroupBox.Controls.Add(this.unityVerDetectionModePanel);
            this.unityVersionGroupBox.Controls.Add(this.unityVerDetectionModeLabel);
            this.unityVersionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.unityVersionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.unityVersionGroupBox.Name = "unityVersionGroupBox";
            this.unityVersionGroupBox.Size = new System.Drawing.Size(357, 126);
            this.unityVersionGroupBox.TabIndex = 0;
            this.unityVersionGroupBox.TabStop = false;
            this.unityVersionGroupBox.Text = "Unity version";
            // 
            // customUnityVersionTextBox
            // 
            this.customUnityVersionTextBox.Location = new System.Drawing.Point(137, 20);
            this.customUnityVersionTextBox.Name = "customUnityVersionTextBox";
            this.customUnityVersionTextBox.Size = new System.Drawing.Size(121, 23);
            this.customUnityVersionTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.customUnityVersionTextBox, "Specify full Unity version, including letters at the end\r\nExample: 2017.4.39f1\r\n");
            // 
            // customUnityVersionLabel
            // 
            this.customUnityVersionLabel.AutoSize = true;
            this.customUnityVersionLabel.Location = new System.Drawing.Point(6, 23);
            this.customUnityVersionLabel.Name = "customUnityVersionLabel";
            this.customUnityVersionLabel.Size = new System.Drawing.Size(124, 15);
            this.customUnityVersionLabel.TabIndex = 0;
            this.customUnityVersionLabel.Text = "Custom Unity version:";
            this.toolTip1.SetToolTip(this.customUnityVersionLabel, "Specify full Unity version, including letters at the end\r\nExample: 2017.4.39f1\r\n");
            // 
            // unityVerDetectionModePanel
            // 
            this.unityVerDetectionModePanel.Controls.Add(this.unityVerAutoRadioButton);
            this.unityVerDetectionModePanel.Controls.Add(this.unityVerLegacy2RadioButton);
            this.unityVerDetectionModePanel.Controls.Add(this.unityVerLegacyRadioButton);
            this.unityVerDetectionModePanel.Location = new System.Drawing.Point(35, 75);
            this.unityVerDetectionModePanel.Name = "unityVerDetectionModePanel";
            this.unityVerDetectionModePanel.Size = new System.Drawing.Size(289, 34);
            this.unityVerDetectionModePanel.TabIndex = 3;
            // 
            // unityVerAutoRadioButton
            // 
            this.unityVerAutoRadioButton.AutoSize = true;
            this.unityVerAutoRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.unityVerAutoRadioButton.Location = new System.Drawing.Point(152, 0);
            this.unityVerAutoRadioButton.Name = "unityVerAutoRadioButton";
            this.unityVerAutoRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.unityVerAutoRadioButton.Size = new System.Drawing.Size(103, 34);
            this.unityVerAutoRadioButton.TabIndex = 2;
            this.unityVerAutoRadioButton.TabStop = true;
            this.unityVerAutoRadioButton.Text = "Auto/Custom";
            this.unityVerAutoRadioButton.UseVisualStyleBackColor = true;
            // 
            // unityVerLegacy2RadioButton
            // 
            this.unityVerLegacy2RadioButton.AutoSize = true;
            this.unityVerLegacy2RadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.unityVerLegacy2RadioButton.Enabled = false;
            this.unityVerLegacy2RadioButton.Location = new System.Drawing.Point(76, 0);
            this.unityVerLegacy2RadioButton.Name = "unityVerLegacy2RadioButton";
            this.unityVerLegacy2RadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.unityVerLegacy2RadioButton.Size = new System.Drawing.Size(76, 34);
            this.unityVerLegacy2RadioButton.TabIndex = 1;
            this.unityVerLegacy2RadioButton.TabStop = true;
            this.unityVerLegacy2RadioButton.Text = "Legacy 2";
            this.toolTip1.SetToolTip(this.unityVerLegacy2RadioButton, "WIP");
            this.unityVerLegacy2RadioButton.UseVisualStyleBackColor = true;
            // 
            // unityVerLegacyRadioButton
            // 
            this.unityVerLegacyRadioButton.AutoSize = true;
            this.unityVerLegacyRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.unityVerLegacyRadioButton.Enabled = false;
            this.unityVerLegacyRadioButton.Location = new System.Drawing.Point(0, 0);
            this.unityVerLegacyRadioButton.Name = "unityVerLegacyRadioButton";
            this.unityVerLegacyRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.unityVerLegacyRadioButton.Size = new System.Drawing.Size(76, 34);
            this.unityVerLegacyRadioButton.TabIndex = 0;
            this.unityVerLegacyRadioButton.TabStop = true;
            this.unityVerLegacyRadioButton.Text = "Legacy 1";
            this.toolTip1.SetToolTip(this.unityVerLegacyRadioButton, "WIP");
            this.unityVerLegacyRadioButton.UseVisualStyleBackColor = true;
            // 
            // unityVerDetectionModeLabel
            // 
            this.unityVerDetectionModeLabel.AutoSize = true;
            this.unityVerDetectionModeLabel.Location = new System.Drawing.Point(6, 57);
            this.unityVerDetectionModeLabel.Name = "unityVerDetectionModeLabel";
            this.unityVerDetectionModeLabel.Size = new System.Drawing.Size(92, 15);
            this.unityVerDetectionModeLabel.TabIndex = 2;
            this.unityVerDetectionModeLabel.Text = "Detection mode";
            this.toolTip1.SetToolTip(this.unityVerDetectionModeLabel, "WIP");
            // 
            // decompressionGroupBox
            // 
            this.decompressionGroupBox.Controls.Add(this.customBlockCompressionComboBox);
            this.decompressionGroupBox.Controls.Add(this.customBlockCompressionLabel);
            this.decompressionGroupBox.Controls.Add(this.customBlockInfoCompressionComboBox);
            this.decompressionGroupBox.Controls.Add(this.customBlockInfoCompressionLabel);
            this.decompressionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.decompressionGroupBox.Location = new System.Drawing.Point(3, 126);
            this.decompressionGroupBox.Name = "decompressionGroupBox";
            this.decompressionGroupBox.Size = new System.Drawing.Size(357, 92);
            this.decompressionGroupBox.TabIndex = 1;
            this.decompressionGroupBox.TabStop = false;
            this.decompressionGroupBox.Text = "Decompression options";
            // 
            // customBlockCompressionComboBox
            // 
            this.customBlockCompressionComboBox.FormattingEnabled = true;
            this.customBlockCompressionComboBox.Items.AddRange(new object[] {
            "Auto",
            "Zstd",
            "Oodle",
            "Lz4/Lz4HC",
            "Lzma"});
            this.customBlockCompressionComboBox.Location = new System.Drawing.Point(166, 51);
            this.customBlockCompressionComboBox.Name = "customBlockCompressionComboBox";
            this.customBlockCompressionComboBox.Size = new System.Drawing.Size(121, 23);
            this.customBlockCompressionComboBox.TabIndex = 3;
            this.toolTip1.SetToolTip(this.customBlockCompressionComboBox, "Selected compression type will override detected type from an asset bundle");
            // 
            // customBlockCompressionLabel
            // 
            this.customBlockCompressionLabel.AutoSize = true;
            this.customBlockCompressionLabel.Location = new System.Drawing.Point(6, 54);
            this.customBlockCompressionLabel.Name = "customBlockCompressionLabel";
            this.customBlockCompressionLabel.Size = new System.Drawing.Size(133, 15);
            this.customBlockCompressionLabel.TabIndex = 2;
            this.customBlockCompressionLabel.Text = "Block compression type";
            // 
            // customBlockInfoCompressionComboBox
            // 
            this.customBlockInfoCompressionComboBox.FormattingEnabled = true;
            this.customBlockInfoCompressionComboBox.Items.AddRange(new object[] {
            "Auto",
            "Zstd",
            "Oodle",
            "Lz4/Lz4HC",
            "Lzma"});
            this.customBlockInfoCompressionComboBox.Location = new System.Drawing.Point(166, 22);
            this.customBlockInfoCompressionComboBox.Name = "customBlockInfoCompressionComboBox";
            this.customBlockInfoCompressionComboBox.Size = new System.Drawing.Size(121, 23);
            this.customBlockInfoCompressionComboBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.customBlockInfoCompressionComboBox, "Selected compression type will override detected type from an asset bundle");
            // 
            // customBlockInfoCompressionLabel
            // 
            this.customBlockInfoCompressionLabel.AutoSize = true;
            this.customBlockInfoCompressionLabel.Location = new System.Drawing.Point(6, 25);
            this.customBlockInfoCompressionLabel.Name = "customBlockInfoCompressionLabel";
            this.customBlockInfoCompressionLabel.Size = new System.Drawing.Size(154, 15);
            this.customBlockInfoCompressionLabel.TabIndex = 0;
            this.customBlockInfoCompressionLabel.Text = "BlockInfo compression type";
            // 
            // cacheFreeSpaceLabel
            // 
            this.cacheFreeSpaceLabel.AutoSize = true;
            this.cacheFreeSpaceLabel.Location = new System.Drawing.Point(160, 47);
            this.cacheFreeSpaceLabel.Name = "cacheFreeSpaceLabel";
            this.cacheFreeSpaceLabel.Size = new System.Drawing.Size(91, 15);
            this.cacheFreeSpaceLabel.TabIndex = 3;
            this.cacheFreeSpaceLabel.Text = "Available space:";
            // 
            // alwaysDecompressToDiskCheckBox
            // 
            this.alwaysDecompressToDiskCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alwaysDecompressToDiskCheckBox.Location = new System.Drawing.Point(9, 82);
            this.alwaysDecompressToDiskCheckBox.Name = "alwaysDecompressToDiskCheckBox";
            this.alwaysDecompressToDiskCheckBox.Size = new System.Drawing.Size(345, 19);
            this.alwaysDecompressToDiskCheckBox.TabIndex = 4;
            this.alwaysDecompressToDiskCheckBox.Text = "Always decompress asset bundles to disk (cache dir)";
            this.toolTip1.SetToolTip(this.alwaysDecompressToDiskCheckBox, "If not selected, any bundles less than 2GB will be decompressed to RAM");
            this.alwaysDecompressToDiskCheckBox.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(294, 336);
            this.closeButton.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(88, 27);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // saveToDiskButton
            // 
            this.saveToDiskButton.AutoSize = true;
            this.saveToDiskButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.saveToDiskButton.Location = new System.Drawing.Point(12, 336);
            this.saveToDiskButton.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.saveToDiskButton.Name = "saveToDiskButton";
            this.saveToDiskButton.Size = new System.Drawing.Size(89, 27);
            this.saveToDiskButton.TabIndex = 1;
            this.saveToDiskButton.Text = "Save to disk";
            this.saveToDiskButton.UseVisualStyleBackColor = false;
            this.saveToDiskButton.Click += new System.EventHandler(this.saveToDiskButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.AutoSize = true;
            this.applyButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.applyButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.applyButton.Location = new System.Drawing.Point(200, 336);
            this.applyButton.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(88, 27);
            this.applyButton.TabIndex = 2;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = false;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(371, 319);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage1.Controls.Add(this.unityVersionGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(363, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage2.Controls.Add(this.decompressionGroupBox);
            this.tabPage2.Controls.Add(this.cacheGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(363, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Asset Bundles";
            // 
            // cacheGroupBox
            // 
            this.cacheGroupBox.AutoSize = true;
            this.cacheGroupBox.Controls.Add(this.alwaysDecompressToDiskCheckBox);
            this.cacheGroupBox.Controls.Add(this.cacheFreeSpaceLabel);
            this.cacheGroupBox.Controls.Add(this.cachePathLabel);
            this.cacheGroupBox.Controls.Add(this.cachePathSelectButton);
            this.cacheGroupBox.Controls.Add(this.cachePathResetButton);
            this.cacheGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.cacheGroupBox.Location = new System.Drawing.Point(3, 3);
            this.cacheGroupBox.Name = "cacheGroupBox";
            this.cacheGroupBox.Size = new System.Drawing.Size(357, 123);
            this.cacheGroupBox.TabIndex = 0;
            this.cacheGroupBox.TabStop = false;
            this.cacheGroupBox.Text = "Disk cache";
            // 
            // ImportOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(394, 372);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.saveToDiskButton);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "ImportOptionsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import options";
            this.unityVersionGroupBox.ResumeLayout(false);
            this.unityVersionGroupBox.PerformLayout();
            this.unityVerDetectionModePanel.ResumeLayout(false);
            this.unityVerDetectionModePanel.PerformLayout();
            this.decompressionGroupBox.ResumeLayout(false);
            this.decompressionGroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.cacheGroupBox.ResumeLayout(false);
            this.cacheGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label cachePathLabel;
        private System.Windows.Forms.Button cachePathSelectButton;
        private System.Windows.Forms.Button cachePathResetButton;
        private System.Windows.Forms.GroupBox unityVersionGroupBox;
        private System.Windows.Forms.GroupBox decompressionGroupBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox customBlockInfoCompressionComboBox;
        private System.Windows.Forms.Label customBlockInfoCompressionLabel;
        private System.Windows.Forms.TextBox customUnityVersionTextBox;
        private System.Windows.Forms.Label customUnityVersionLabel;
        private System.Windows.Forms.ComboBox customBlockCompressionComboBox;
        private System.Windows.Forms.Label customBlockCompressionLabel;
        private System.Windows.Forms.CheckBox alwaysDecompressToDiskCheckBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveToDiskButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label cacheFreeSpaceLabel;
        private System.Windows.Forms.Panel unityVerDetectionModePanel;
        private System.Windows.Forms.RadioButton unityVerAutoRadioButton;
        private System.Windows.Forms.RadioButton unityVerLegacy2RadioButton;
        private System.Windows.Forms.RadioButton unityVerLegacyRadioButton;
        private System.Windows.Forms.Label unityVerDetectionModeLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox cacheGroupBox;
    }
}