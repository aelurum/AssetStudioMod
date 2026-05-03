namespace AssetStudioGUI
{
    partial class StudioOptionsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.guiComponentsGroupBox = new System.Windows.Forms.GroupBox();
            this.buildSceneHierarchyCheckBox = new System.Windows.Forms.CheckBox();
            this.cacheGroupBox = new System.Windows.Forms.GroupBox();
            this.alwaysDecompressToDiskCheckBox = new System.Windows.Forms.CheckBox();
            this.cacheFreeSpaceLabel = new System.Windows.Forms.Label();
            this.cachePathLabel = new System.Windows.Forms.Label();
            this.cachePathSelectButton = new System.Windows.Forms.Button();
            this.cachePathResetButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.meshPreProcessingCheckBox = new System.Windows.Forms.CheckBox();
            this.parseByTypetreeCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.OKbutton = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.displayAllCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.guiComponentsGroupBox.SuspendLayout();
            this.cacheGroupBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(371, 333);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage1.Controls.Add(this.guiComponentsGroupBox);
            this.tabPage1.Controls.Add(this.cacheGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(363, 305);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // guiComponentsGroupBox
            // 
            this.guiComponentsGroupBox.AutoSize = true;
            this.guiComponentsGroupBox.Controls.Add(this.buildSceneHierarchyCheckBox);
            this.guiComponentsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.guiComponentsGroupBox.Location = new System.Drawing.Point(3, 126);
            this.guiComponentsGroupBox.Name = "guiComponentsGroupBox";
            this.guiComponentsGroupBox.Size = new System.Drawing.Size(357, 63);
            this.guiComponentsGroupBox.TabIndex = 1;
            this.guiComponentsGroupBox.TabStop = false;
            this.guiComponentsGroupBox.Text = "GUI components";
            // 
            // buildSceneHierarchyCheckBox
            // 
            this.buildSceneHierarchyCheckBox.AutoSize = true;
            this.buildSceneHierarchyCheckBox.Checked = true;
            this.buildSceneHierarchyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buildSceneHierarchyCheckBox.Location = new System.Drawing.Point(6, 22);
            this.buildSceneHierarchyCheckBox.Name = "buildSceneHierarchyCheckBox";
            this.buildSceneHierarchyCheckBox.Size = new System.Drawing.Size(138, 19);
            this.buildSceneHierarchyCheckBox.TabIndex = 0;
            this.buildSceneHierarchyCheckBox.Text = "Build scene hierarchy";
            this.toolTip1.SetToolTip(this.buildSceneHierarchyCheckBox, "You can disable tree structure building if you don\'t use the Scene Hierarchy tab");
            this.buildSceneHierarchyCheckBox.UseVisualStyleBackColor = true;
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
            // alwaysDecompressToDiskCheckBox
            // 
            this.alwaysDecompressToDiskCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alwaysDecompressToDiskCheckBox.Location = new System.Drawing.Point(9, 82);
            this.alwaysDecompressToDiskCheckBox.Name = "alwaysDecompressToDiskCheckBox";
            this.alwaysDecompressToDiskCheckBox.Size = new System.Drawing.Size(345, 19);
            this.alwaysDecompressToDiskCheckBox.TabIndex = 4;
            this.alwaysDecompressToDiskCheckBox.Text = "Decompress asset bundles to disk (cache dir)";
            this.toolTip1.SetToolTip(this.alwaysDecompressToDiskCheckBox, "If not selected, any bundles less than 2GB will be decompressed to RAM");
            this.alwaysDecompressToDiskCheckBox.UseVisualStyleBackColor = true;
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
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(363, 305);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Assets";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.displayAllCheckBox);
            this.groupBox2.Controls.Add(this.parseByTypetreeCheckBox);
            this.groupBox2.Controls.Add(this.meshPreProcessingCheckBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 113);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Asset parsing";
            // 
            // meshPreProcessingCheckBox
            // 
            this.meshPreProcessingCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.meshPreProcessingCheckBox.Location = new System.Drawing.Point(6, 47);
            this.meshPreProcessingCheckBox.Name = "meshPreProcessingCheckBox";
            this.meshPreProcessingCheckBox.Size = new System.Drawing.Size(348, 19);
            this.meshPreProcessingCheckBox.TabIndex = 2;
            this.meshPreProcessingCheckBox.Text = "Pre-process Mesh assets during loading";
            this.toolTip1.SetToolTip(this.meshPreProcessingCheckBox, "Requires more RAM");
            this.meshPreProcessingCheckBox.UseVisualStyleBackColor = true;
            // 
            // parseByTypetreeCheckBox
            // 
            this.parseByTypetreeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parseByTypetreeCheckBox.Checked = true;
            this.parseByTypetreeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.parseByTypetreeCheckBox.Location = new System.Drawing.Point(6, 72);
            this.parseByTypetreeCheckBox.Name = "parseByTypetreeCheckBox";
            this.parseByTypetreeCheckBox.Size = new System.Drawing.Size(348, 19);
            this.parseByTypetreeCheckBox.TabIndex = 1;
            this.parseByTypetreeCheckBox.Text = "Parse assets using their typetree";
            this.parseByTypetreeCheckBox.UseVisualStyleBackColor = true;
            // 
            // OKbutton
            // 
            this.OKbutton.AutoSize = true;
            this.OKbutton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.OKbutton.Location = new System.Drawing.Point(201, 354);
            this.OKbutton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(88, 27);
            this.OKbutton.TabIndex = 1;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = false;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // Cancel
            // 
            this.Cancel.AutoSize = true;
            this.Cancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(295, 354);
            this.Cancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(88, 27);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // displayAllCheckBox
            // 
            this.displayAllCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.displayAllCheckBox.Location = new System.Drawing.Point(6, 22);
            this.displayAllCheckBox.Name = "displayAllCheckBox";
            this.displayAllCheckBox.Size = new System.Drawing.Size(348, 19);
            this.displayAllCheckBox.TabIndex = 3;
            this.displayAllCheckBox.Text = "Display all assets";
            this.toolTip1.SetToolTip(this.displayAllCheckBox, "Check this option will display all types assets. Not extractable assets can expor" +
        "t the RAW file.");
            this.displayAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // StudioOptionsForm
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(395, 393);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StudioOptionsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Studio options";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.guiComponentsGroupBox.ResumeLayout(false);
            this.guiComponentsGroupBox.PerformLayout();
            this.cacheGroupBox.ResumeLayout(false);
            this.cacheGroupBox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox cacheGroupBox;
        private System.Windows.Forms.CheckBox alwaysDecompressToDiskCheckBox;
        private System.Windows.Forms.Label cacheFreeSpaceLabel;
        private System.Windows.Forms.Label cachePathLabel;
        private System.Windows.Forms.Button cachePathSelectButton;
        private System.Windows.Forms.Button cachePathResetButton;
        private System.Windows.Forms.CheckBox parseByTypetreeCheckBox;
        private System.Windows.Forms.CheckBox buildSceneHierarchyCheckBox;
        private System.Windows.Forms.GroupBox guiComponentsGroupBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox meshPreProcessingCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.CheckBox displayAllCheckBox;
    }
}