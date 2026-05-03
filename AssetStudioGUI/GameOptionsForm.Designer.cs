namespace AssetStudioGUI
{
    partial class GameOptionsForm
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
            this.unityVersionGroupBox = new System.Windows.Forms.GroupBox();
            this.legacyUnityVersionComboBox = new System.Windows.Forms.ComboBox();
            this.legacyUnityVersionLabel = new System.Windows.Forms.Label();
            this.customUnityVersionTextBox = new System.Windows.Forms.TextBox();
            this.customUnityVersionLabel = new System.Windows.Forms.Label();
            this.decompressionGroupBox = new System.Windows.Forms.GroupBox();
            this.customBlockCompressionComboBox = new System.Windows.Forms.ComboBox();
            this.customBlockCompressionLabel = new System.Windows.Forms.Label();
            this.customBlockInfoCompressionComboBox = new System.Windows.Forms.ComboBox();
            this.customBlockInfoCompressionLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.closeButton = new System.Windows.Forms.Button();
            this.saveToDiskButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tuanjieGroupBox = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tMeshModeUnityRadioButton = new System.Windows.Forms.RadioButton();
            this.tMeshModeTuanjieRadioButton = new System.Windows.Forms.RadioButton();
            this.tMeshModeAutoRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.unityVersionGroupBox.SuspendLayout();
            this.decompressionGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tuanjieGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // unityVersionGroupBox
            // 
            this.unityVersionGroupBox.Controls.Add(this.legacyUnityVersionComboBox);
            this.unityVersionGroupBox.Controls.Add(this.legacyUnityVersionLabel);
            this.unityVersionGroupBox.Controls.Add(this.customUnityVersionTextBox);
            this.unityVersionGroupBox.Controls.Add(this.customUnityVersionLabel);
            this.unityVersionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.unityVersionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.unityVersionGroupBox.Name = "unityVersionGroupBox";
            this.unityVersionGroupBox.Size = new System.Drawing.Size(357, 99);
            this.unityVersionGroupBox.TabIndex = 0;
            this.unityVersionGroupBox.TabStop = false;
            this.unityVersionGroupBox.Text = "Unity version";
            // 
            // legacyUnityVersionComboBox
            // 
            this.legacyUnityVersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.legacyUnityVersionComboBox.Items.AddRange(new object[] {
            "None",
            "1.2 mac",
            "1.5 - 1.6 mac",
            "2.0 mac",
            "2.1 mac / 1.7 iphone",
            "2.5",
            "2.6"});
            this.legacyUnityVersionComboBox.Location = new System.Drawing.Point(205, 49);
            this.legacyUnityVersionComboBox.Name = "legacyUnityVersionComboBox";
            this.legacyUnityVersionComboBox.Size = new System.Drawing.Size(146, 23);
            this.legacyUnityVersionComboBox.TabIndex = 3;
            this.legacyUnityVersionComboBox.SelectionChangeCommitted += new System.EventHandler(this.legacyUnityVersionComboBox_SelectionChangeCommitted);
            // 
            // legacyUnityVersionLabel
            // 
            this.legacyUnityVersionLabel.AutoSize = true;
            this.legacyUnityVersionLabel.Location = new System.Drawing.Point(6, 52);
            this.legacyUnityVersionLabel.Name = "legacyUnityVersionLabel";
            this.legacyUnityVersionLabel.Size = new System.Drawing.Size(193, 15);
            this.legacyUnityVersionLabel.TabIndex = 2;
            this.legacyUnityVersionLabel.Text = "Custom legacy Unity version (WIP):";
            // 
            // customUnityVersionTextBox
            // 
            this.customUnityVersionTextBox.Location = new System.Drawing.Point(137, 20);
            this.customUnityVersionTextBox.Name = "customUnityVersionTextBox";
            this.customUnityVersionTextBox.Size = new System.Drawing.Size(121, 23);
            this.customUnityVersionTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.customUnityVersionTextBox, "Specify full Unity version, including letters at the end\r\nExample: 2022.3.2f1");
            this.customUnityVersionTextBox.Enter += new System.EventHandler(this.customUnityVersionTextBox_Enter);
            this.customUnityVersionTextBox.Leave += new System.EventHandler(this.customUnityVersionTextBox_Leave);
            // 
            // customUnityVersionLabel
            // 
            this.customUnityVersionLabel.AutoSize = true;
            this.customUnityVersionLabel.Location = new System.Drawing.Point(6, 23);
            this.customUnityVersionLabel.Name = "customUnityVersionLabel";
            this.customUnityVersionLabel.Size = new System.Drawing.Size(124, 15);
            this.customUnityVersionLabel.TabIndex = 0;
            this.customUnityVersionLabel.Text = "Custom Unity version:";
            this.toolTip1.SetToolTip(this.customUnityVersionLabel, "Specify full Unity version, including letters at the end\r\nExample: 2022.3.2f1");
            // 
            // decompressionGroupBox
            // 
            this.decompressionGroupBox.Controls.Add(this.customBlockCompressionComboBox);
            this.decompressionGroupBox.Controls.Add(this.customBlockCompressionLabel);
            this.decompressionGroupBox.Controls.Add(this.customBlockInfoCompressionComboBox);
            this.decompressionGroupBox.Controls.Add(this.customBlockInfoCompressionLabel);
            this.decompressionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.decompressionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.decompressionGroupBox.Name = "decompressionGroupBox";
            this.decompressionGroupBox.Size = new System.Drawing.Size(357, 92);
            this.decompressionGroupBox.TabIndex = 0;
            this.decompressionGroupBox.TabStop = false;
            this.decompressionGroupBox.Text = "Decompression options";
            // 
            // customBlockCompressionComboBox
            // 
            this.customBlockCompressionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.customBlockInfoCompressionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.tabControl1.Controls.Add(this.tabPage3);
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
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(363, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Asset Bundles";
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage3.Controls.Add(this.tuanjieGroupBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(363, 291);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Meshes";
            // 
            // tuanjieGroupBox
            // 
            this.tuanjieGroupBox.AutoSize = true;
            this.tuanjieGroupBox.Controls.Add(this.panel1);
            this.tuanjieGroupBox.Controls.Add(this.label1);
            this.tuanjieGroupBox.Location = new System.Drawing.Point(3, 3);
            this.tuanjieGroupBox.Name = "tuanjieGroupBox";
            this.tuanjieGroupBox.Size = new System.Drawing.Size(357, 82);
            this.tuanjieGroupBox.TabIndex = 0;
            this.tuanjieGroupBox.TabStop = false;
            this.tuanjieGroupBox.Text = "Tuanjie engine";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.tMeshModeUnityRadioButton);
            this.panel1.Controls.Add(this.tMeshModeTuanjieRadioButton);
            this.panel1.Controls.Add(this.tMeshModeAutoRadioButton);
            this.panel1.Location = new System.Drawing.Point(32, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 23);
            this.panel1.TabIndex = 1;
            // 
            // tMeshModeUnityRadioButton
            // 
            this.tMeshModeUnityRadioButton.AutoSize = true;
            this.tMeshModeUnityRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.tMeshModeUnityRadioButton.Location = new System.Drawing.Point(156, 0);
            this.tMeshModeUnityRadioButton.Name = "tMeshModeUnityRadioButton";
            this.tMeshModeUnityRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tMeshModeUnityRadioButton.Size = new System.Drawing.Size(90, 23);
            this.tMeshModeUnityRadioButton.TabIndex = 2;
            this.tMeshModeUnityRadioButton.Text = "Unity mesh";
            this.tMeshModeUnityRadioButton.UseVisualStyleBackColor = true;
            // 
            // tMeshModeTuanjieRadioButton
            // 
            this.tMeshModeTuanjieRadioButton.AutoSize = true;
            this.tMeshModeTuanjieRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.tMeshModeTuanjieRadioButton.Location = new System.Drawing.Point(56, 0);
            this.tMeshModeTuanjieRadioButton.Name = "tMeshModeTuanjieRadioButton";
            this.tMeshModeTuanjieRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tMeshModeTuanjieRadioButton.Size = new System.Drawing.Size(100, 23);
            this.tMeshModeTuanjieRadioButton.TabIndex = 1;
            this.tMeshModeTuanjieRadioButton.Text = "Tuanjie mesh";
            this.tMeshModeTuanjieRadioButton.UseVisualStyleBackColor = true;
            // 
            // tMeshModeAutoRadioButton
            // 
            this.tMeshModeAutoRadioButton.AutoSize = true;
            this.tMeshModeAutoRadioButton.Checked = true;
            this.tMeshModeAutoRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.tMeshModeAutoRadioButton.Location = new System.Drawing.Point(0, 0);
            this.tMeshModeAutoRadioButton.Name = "tMeshModeAutoRadioButton";
            this.tMeshModeAutoRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tMeshModeAutoRadioButton.Size = new System.Drawing.Size(56, 23);
            this.tMeshModeAutoRadioButton.TabIndex = 0;
            this.tMeshModeAutoRadioButton.TabStop = true;
            this.tMeshModeAutoRadioButton.Text = "Auto";
            this.tMeshModeAutoRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parsing mode";
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
            this.Name = "GameOptionsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game-specific options";
            this.unityVersionGroupBox.ResumeLayout(false);
            this.unityVersionGroupBox.PerformLayout();
            this.decompressionGroupBox.ResumeLayout(false);
            this.decompressionGroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tuanjieGroupBox.ResumeLayout(false);
            this.tuanjieGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox unityVersionGroupBox;
        private System.Windows.Forms.GroupBox decompressionGroupBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox customBlockInfoCompressionComboBox;
        private System.Windows.Forms.Label customBlockInfoCompressionLabel;
        private System.Windows.Forms.TextBox customUnityVersionTextBox;
        private System.Windows.Forms.Label customUnityVersionLabel;
        private System.Windows.Forms.ComboBox customBlockCompressionComboBox;
        private System.Windows.Forms.Label customBlockCompressionLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveToDiskButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label legacyUnityVersionLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox legacyUnityVersionComboBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox tuanjieGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton tMeshModeUnityRadioButton;
        private System.Windows.Forms.RadioButton tMeshModeTuanjieRadioButton;
        private System.Windows.Forms.RadioButton tMeshModeAutoRadioButton;
    }
}