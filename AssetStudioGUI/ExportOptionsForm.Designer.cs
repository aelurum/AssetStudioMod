namespace AssetStudioGUI
{
    partial class ExportOptionsForm
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
            this.OKbutton = new System.Windows.Forms.Button();
            this.rawByteArrayFromMono = new System.Windows.Forms.CheckBox();
            this.overwriteExistingFiles = new System.Windows.Forms.CheckBox();
            this.parallelExportMaxLabel = new System.Windows.Forms.Label();
            this.parallelExportCheckBox = new System.Windows.Forms.CheckBox();
            this.parallelExportUpDown = new System.Windows.Forms.NumericUpDown();
            this.filenameFormatLabel = new System.Windows.Forms.Label();
            this.filenameFormatComboBox = new System.Windows.Forms.ComboBox();
            this.exportSpriteWithAlphaMask = new System.Windows.Forms.CheckBox();
            this.openAfterExport = new System.Windows.Forms.CheckBox();
            this.restoreExtensionName = new System.Windows.Forms.CheckBox();
            this.assetGroupOptions = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.convertAudio = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.towebp = new System.Windows.Forms.RadioButton();
            this.totga = new System.Windows.Forms.RadioButton();
            this.tojpg = new System.Windows.Forms.RadioButton();
            this.topng = new System.Windows.Forms.RadioButton();
            this.tobmp = new System.Windows.Forms.RadioButton();
            this.converttexture = new System.Windows.Forms.CheckBox();
            this.l2dAssetSearchByFilenameCheckBox = new System.Windows.Forms.CheckBox();
            this.l2dModelGroupComboBox = new System.Windows.Forms.ComboBox();
            this.l2dModelGroupLabel = new System.Windows.Forms.Label();
            this.l2dMotionExportMethodPanel = new System.Windows.Forms.Panel();
            this.l2dAnimationClipRadioButton = new System.Windows.Forms.RadioButton();
            this.l2dMonoBehaviourRadioButton = new System.Windows.Forms.RadioButton();
            this.l2dMotionExportMethodLabel = new System.Windows.Forms.Label();
            this.l2dForceBezierCheckBox = new System.Windows.Forms.CheckBox();
            this.fbxResetButton = new System.Windows.Forms.Button();
            this.uvBindingsLabel = new System.Windows.Forms.Label();
            this.uvIndicesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.uvTypesListBox = new System.Windows.Forms.ListBox();
            this.exportAllUvsAsDiffuseMaps = new System.Windows.Forms.CheckBox();
            this.exportBlendShape = new System.Windows.Forms.CheckBox();
            this.exportAnimations = new System.Windows.Forms.CheckBox();
            this.scaleFactor = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fbxFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fbxVersion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.boneSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.exportSkins = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.filterPrecision = new System.Windows.Forms.NumericUpDown();
            this.castToBone = new System.Windows.Forms.CheckBox();
            this.exportAllNodes = new System.Windows.Forms.CheckBox();
            this.eulerFilter = new System.Windows.Forms.CheckBox();
            this.optionTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imagesTabPage = new System.Windows.Forms.TabPage();
            this.keepSpriteSizeCheckBox = new System.Windows.Forms.CheckBox();
            this.fbxTabPage = new System.Windows.Forms.TabPage();
            this.dumpMaterialsCheckBox = new System.Windows.Forms.CheckBox();
            this.live2DTabPage = new System.Windows.Forms.TabPage();
            this.Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.parallelExportUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.l2dMotionExportMethodPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boneSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterPrecision)).BeginInit();
            this.optionsTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.imagesTabPage.SuspendLayout();
            this.fbxTabPage.SuspendLayout();
            this.live2DTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKbutton
            // 
            this.OKbutton.AutoSize = true;
            this.OKbutton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.OKbutton.Location = new System.Drawing.Point(228, 442);
            this.OKbutton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(88, 27);
            this.OKbutton.TabIndex = 2;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = false;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // rawByteArrayFromMono
            // 
            this.rawByteArrayFromMono.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rawByteArrayFromMono.AutoSize = true;
            this.rawByteArrayFromMono.Location = new System.Drawing.Point(7, 184);
            this.rawByteArrayFromMono.Name = "rawByteArrayFromMono";
            this.rawByteArrayFromMono.Size = new System.Drawing.Size(352, 19);
            this.rawByteArrayFromMono.TabIndex = 11;
            this.rawByteArrayFromMono.Text = "ExportRaw: Extract raw byte array from MonoBehaviour assets";
            this.rawByteArrayFromMono.UseVisualStyleBackColor = true;
            // 
            // overwriteExistingFiles
            // 
            this.overwriteExistingFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.overwriteExistingFiles.AutoSize = true;
            this.overwriteExistingFiles.Location = new System.Drawing.Point(7, 83);
            this.overwriteExistingFiles.Name = "overwriteExistingFiles";
            this.overwriteExistingFiles.Size = new System.Drawing.Size(145, 19);
            this.overwriteExistingFiles.TabIndex = 5;
            this.overwriteExistingFiles.Text = "Overwrite existing files";
            this.overwriteExistingFiles.UseVisualStyleBackColor = true;
            // 
            // parallelExportMaxLabel
            // 
            this.parallelExportMaxLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.parallelExportMaxLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.parallelExportMaxLabel.Location = new System.Drawing.Point(49, 0);
            this.parallelExportMaxLabel.Name = "parallelExportMaxLabel";
            this.parallelExportMaxLabel.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.parallelExportMaxLabel.Size = new System.Drawing.Size(81, 26);
            this.parallelExportMaxLabel.TabIndex = 9;
            this.parallelExportMaxLabel.Text = "Max: ";
            this.optionTooltip.SetToolTip(this.parallelExportMaxLabel, "*The maximum number matches the number of CPU cores");
            // 
            // parallelExportCheckBox
            // 
            this.parallelExportCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parallelExportCheckBox.AutoSize = true;
            this.parallelExportCheckBox.Checked = true;
            this.parallelExportCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.parallelExportCheckBox.Location = new System.Drawing.Point(7, 132);
            this.parallelExportCheckBox.Name = "parallelExportCheckBox";
            this.parallelExportCheckBox.Size = new System.Drawing.Size(228, 19);
            this.parallelExportCheckBox.TabIndex = 7;
            this.parallelExportCheckBox.Text = "Export in parallel with number of tasks";
            this.optionTooltip.SetToolTip(this.parallelExportCheckBox, "*Requires slightly more RAM than in single-task mode");
            this.parallelExportCheckBox.UseVisualStyleBackColor = true;
            this.parallelExportCheckBox.CheckedChanged += new System.EventHandler(this.parallelExportCheckBox_CheckedChanged);
            // 
            // parallelExportUpDown
            // 
            this.parallelExportUpDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.parallelExportUpDown.Location = new System.Drawing.Point(0, 0);
            this.parallelExportUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.parallelExportUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.parallelExportUpDown.Name = "parallelExportUpDown";
            this.parallelExportUpDown.Size = new System.Drawing.Size(49, 23);
            this.parallelExportUpDown.TabIndex = 8;
            this.parallelExportUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // filenameFormatLabel
            // 
            this.filenameFormatLabel.AutoSize = true;
            this.filenameFormatLabel.Location = new System.Drawing.Point(210, 7);
            this.filenameFormatLabel.Name = "filenameFormatLabel";
            this.filenameFormatLabel.Size = new System.Drawing.Size(97, 15);
            this.filenameFormatLabel.TabIndex = 2;
            this.filenameFormatLabel.Text = "File name format";
            // 
            // filenameFormatComboBox
            // 
            this.filenameFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filenameFormatComboBox.DropDownWidth = 198;
            this.filenameFormatComboBox.FormattingEnabled = true;
            this.filenameFormatComboBox.Items.AddRange(new object[] {
            "asset name",
            "asset name@pathID",
            "pathID",
            "asset name@hash (images only)"});
            this.filenameFormatComboBox.Location = new System.Drawing.Point(210, 25);
            this.filenameFormatComboBox.Name = "filenameFormatComboBox";
            this.filenameFormatComboBox.Size = new System.Drawing.Size(149, 23);
            this.filenameFormatComboBox.TabIndex = 3;
            // 
            // exportSpriteWithAlphaMask
            // 
            this.exportSpriteWithAlphaMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportSpriteWithAlphaMask.AutoSize = true;
            this.exportSpriteWithAlphaMask.Checked = true;
            this.exportSpriteWithAlphaMask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportSpriteWithAlphaMask.Location = new System.Drawing.Point(7, 70);
            this.exportSpriteWithAlphaMask.Name = "exportSpriteWithAlphaMask";
            this.exportSpriteWithAlphaMask.Size = new System.Drawing.Size(278, 19);
            this.exportSpriteWithAlphaMask.TabIndex = 2;
            this.exportSpriteWithAlphaMask.Text = "Export sprites with alpha mask applied (if exists)";
            this.exportSpriteWithAlphaMask.UseVisualStyleBackColor = true;
            // 
            // openAfterExport
            // 
            this.openAfterExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openAfterExport.AutoSize = true;
            this.openAfterExport.Checked = true;
            this.openAfterExport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openAfterExport.Location = new System.Drawing.Point(7, 107);
            this.openAfterExport.Name = "openAfterExport";
            this.openAfterExport.Size = new System.Drawing.Size(153, 19);
            this.openAfterExport.TabIndex = 6;
            this.openAfterExport.Text = "Open folder after export";
            this.openAfterExport.UseVisualStyleBackColor = true;
            // 
            // restoreExtensionName
            // 
            this.restoreExtensionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreExtensionName.AutoSize = true;
            this.restoreExtensionName.Checked = true;
            this.restoreExtensionName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.restoreExtensionName.Location = new System.Drawing.Point(7, 59);
            this.restoreExtensionName.Name = "restoreExtensionName";
            this.restoreExtensionName.Size = new System.Drawing.Size(300, 19);
            this.restoreExtensionName.TabIndex = 4;
            this.restoreExtensionName.Text = "Try to restore/Use original TextAsset extension name";
            this.optionTooltip.SetToolTip(this.restoreExtensionName, "If not checked, AssetStudio will export all TextAssets with the \".txt\" extension");
            this.restoreExtensionName.UseVisualStyleBackColor = true;
            // 
            // assetGroupOptions
            // 
            this.assetGroupOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.assetGroupOptions.FormattingEnabled = true;
            this.assetGroupOptions.Items.AddRange(new object[] {
            "type name",
            "container path",
            "container path full (with name)",
            "source file name",
            "scene hierarchy",
            "do not group"});
            this.assetGroupOptions.Location = new System.Drawing.Point(7, 25);
            this.assetGroupOptions.Name = "assetGroupOptions";
            this.assetGroupOptions.Size = new System.Drawing.Size(192, 23);
            this.assetGroupOptions.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Group exported assets by";
            // 
            // convertAudio
            // 
            this.convertAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.convertAudio.AutoSize = true;
            this.convertAudio.Checked = true;
            this.convertAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertAudio.Location = new System.Drawing.Point(7, 159);
            this.convertAudio.Name = "convertAudio";
            this.convertAudio.Size = new System.Drawing.Size(237, 19);
            this.convertAudio.TabIndex = 10;
            this.convertAudio.Text = "Convert FMOD AudioClip to WAV(PCM)";
            this.convertAudio.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.towebp);
            this.panel1.Controls.Add(this.totga);
            this.panel1.Controls.Add(this.tojpg);
            this.panel1.Controls.Add(this.topng);
            this.panel1.Controls.Add(this.tobmp);
            this.panel1.Location = new System.Drawing.Point(34, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 23);
            this.panel1.TabIndex = 1;
            // 
            // towebp
            // 
            this.towebp.AutoSize = true;
            this.towebp.Dock = System.Windows.Forms.DockStyle.Left;
            this.towebp.Location = new System.Drawing.Point(208, 0);
            this.towebp.Name = "towebp";
            this.towebp.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.towebp.Size = new System.Drawing.Size(61, 23);
            this.towebp.TabIndex = 4;
            this.towebp.Text = "Webp";
            this.towebp.UseVisualStyleBackColor = true;
            // 
            // totga
            // 
            this.totga.AutoSize = true;
            this.totga.Dock = System.Windows.Forms.DockStyle.Left;
            this.totga.Location = new System.Drawing.Point(160, 0);
            this.totga.Name = "totga";
            this.totga.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.totga.Size = new System.Drawing.Size(48, 23);
            this.totga.TabIndex = 3;
            this.totga.Text = "Tga";
            this.totga.UseVisualStyleBackColor = true;
            // 
            // tojpg
            // 
            this.tojpg.AutoSize = true;
            this.tojpg.Dock = System.Windows.Forms.DockStyle.Left;
            this.tojpg.Location = new System.Drawing.Point(106, 0);
            this.tojpg.Name = "tojpg";
            this.tojpg.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tojpg.Size = new System.Drawing.Size(54, 23);
            this.tojpg.TabIndex = 2;
            this.tojpg.Text = "Jpeg";
            this.tojpg.UseVisualStyleBackColor = true;
            // 
            // topng
            // 
            this.topng.AutoSize = true;
            this.topng.Checked = true;
            this.topng.Dock = System.Windows.Forms.DockStyle.Left;
            this.topng.Location = new System.Drawing.Point(55, 0);
            this.topng.Name = "topng";
            this.topng.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.topng.Size = new System.Drawing.Size(51, 23);
            this.topng.TabIndex = 1;
            this.topng.TabStop = true;
            this.topng.Text = "Png";
            this.topng.UseVisualStyleBackColor = true;
            // 
            // tobmp
            // 
            this.tobmp.AutoSize = true;
            this.tobmp.Dock = System.Windows.Forms.DockStyle.Left;
            this.tobmp.Location = new System.Drawing.Point(0, 0);
            this.tobmp.Name = "tobmp";
            this.tobmp.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tobmp.Size = new System.Drawing.Size(55, 23);
            this.tobmp.TabIndex = 0;
            this.tobmp.Text = "Bmp";
            this.tobmp.UseVisualStyleBackColor = true;
            // 
            // converttexture
            // 
            this.converttexture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.converttexture.AutoSize = true;
            this.converttexture.Checked = true;
            this.converttexture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.converttexture.Location = new System.Drawing.Point(7, 7);
            this.converttexture.Name = "converttexture";
            this.converttexture.Size = new System.Drawing.Size(123, 19);
            this.converttexture.TabIndex = 0;
            this.converttexture.Text = "Convert Texture2D";
            this.converttexture.UseVisualStyleBackColor = true;
            // 
            // l2dAssetSearchByFilenameCheckBox
            // 
            this.l2dAssetSearchByFilenameCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.l2dAssetSearchByFilenameCheckBox.AutoSize = true;
            this.l2dAssetSearchByFilenameCheckBox.Location = new System.Drawing.Point(7, 141);
            this.l2dAssetSearchByFilenameCheckBox.Name = "l2dAssetSearchByFilenameCheckBox";
            this.l2dAssetSearchByFilenameCheckBox.Size = new System.Drawing.Size(297, 19);
            this.l2dAssetSearchByFilenameCheckBox.TabIndex = 5;
            this.l2dAssetSearchByFilenameCheckBox.Text = "Search for model-related Live2D assets by file name";
            this.optionTooltip.SetToolTip(this.l2dAssetSearchByFilenameCheckBox, "Preferred option if all l2d assets of a single model are stored in a single file " +
        "or containers are obfuscated");
            this.l2dAssetSearchByFilenameCheckBox.UseVisualStyleBackColor = true;
            // 
            // l2dModelGroupComboBox
            // 
            this.l2dModelGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.l2dModelGroupComboBox.FormattingEnabled = true;
            this.l2dModelGroupComboBox.Items.AddRange(new object[] {
            "container path",
            "source file name",
            "model name"});
            this.l2dModelGroupComboBox.Location = new System.Drawing.Point(7, 25);
            this.l2dModelGroupComboBox.Name = "l2dModelGroupComboBox";
            this.l2dModelGroupComboBox.Size = new System.Drawing.Size(151, 23);
            this.l2dModelGroupComboBox.TabIndex = 1;
            // 
            // l2dModelGroupLabel
            // 
            this.l2dModelGroupLabel.AutoSize = true;
            this.l2dModelGroupLabel.Location = new System.Drawing.Point(7, 7);
            this.l2dModelGroupLabel.Name = "l2dModelGroupLabel";
            this.l2dModelGroupLabel.Size = new System.Drawing.Size(148, 15);
            this.l2dModelGroupLabel.TabIndex = 0;
            this.l2dModelGroupLabel.Text = "Group exported models by";
            // 
            // l2dMotionExportMethodPanel
            // 
            this.l2dMotionExportMethodPanel.AutoSize = true;
            this.l2dMotionExportMethodPanel.Controls.Add(this.l2dAnimationClipRadioButton);
            this.l2dMotionExportMethodPanel.Controls.Add(this.l2dMonoBehaviourRadioButton);
            this.l2dMotionExportMethodPanel.Location = new System.Drawing.Point(33, 78);
            this.l2dMotionExportMethodPanel.Name = "l2dMotionExportMethodPanel";
            this.l2dMotionExportMethodPanel.Size = new System.Drawing.Size(315, 31);
            this.l2dMotionExportMethodPanel.TabIndex = 3;
            // 
            // l2dAnimationClipRadioButton
            // 
            this.l2dAnimationClipRadioButton.AccessibleName = "AnimationClipV2";
            this.l2dAnimationClipRadioButton.AutoSize = true;
            this.l2dAnimationClipRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.l2dAnimationClipRadioButton.Location = new System.Drawing.Point(193, 0);
            this.l2dAnimationClipRadioButton.Name = "l2dAnimationClipRadioButton";
            this.l2dAnimationClipRadioButton.Size = new System.Drawing.Size(102, 31);
            this.l2dAnimationClipRadioButton.TabIndex = 1;
            this.l2dAnimationClipRadioButton.Text = "AnimationClip";
            this.l2dAnimationClipRadioButton.UseVisualStyleBackColor = true;
            // 
            // l2dMonoBehaviourRadioButton
            // 
            this.l2dMonoBehaviourRadioButton.AccessibleName = "MonoBehaviour";
            this.l2dMonoBehaviourRadioButton.AutoSize = true;
            this.l2dMonoBehaviourRadioButton.Checked = true;
            this.l2dMonoBehaviourRadioButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.l2dMonoBehaviourRadioButton.Location = new System.Drawing.Point(0, 0);
            this.l2dMonoBehaviourRadioButton.Name = "l2dMonoBehaviourRadioButton";
            this.l2dMonoBehaviourRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.l2dMonoBehaviourRadioButton.Size = new System.Drawing.Size(193, 31);
            this.l2dMonoBehaviourRadioButton.TabIndex = 0;
            this.l2dMonoBehaviourRadioButton.TabStop = true;
            this.l2dMonoBehaviourRadioButton.Text = "MonoBehaviour (Fade motion)";
            this.optionTooltip.SetToolTip(this.l2dMonoBehaviourRadioButton, "If no Fade motions are found, the AnimationClip method will be used");
            this.l2dMonoBehaviourRadioButton.UseVisualStyleBackColor = true;
            // 
            // l2dMotionExportMethodLabel
            // 
            this.l2dMotionExportMethodLabel.AutoSize = true;
            this.l2dMotionExportMethodLabel.Location = new System.Drawing.Point(7, 60);
            this.l2dMotionExportMethodLabel.Name = "l2dMotionExportMethodLabel";
            this.l2dMotionExportMethodLabel.Size = new System.Drawing.Size(128, 15);
            this.l2dMotionExportMethodLabel.TabIndex = 2;
            this.l2dMotionExportMethodLabel.Text = "Motion export method";
            // 
            // l2dForceBezierCheckBox
            // 
            this.l2dForceBezierCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.l2dForceBezierCheckBox.AutoSize = true;
            this.l2dForceBezierCheckBox.Location = new System.Drawing.Point(7, 117);
            this.l2dForceBezierCheckBox.Name = "l2dForceBezierCheckBox";
            this.l2dForceBezierCheckBox.Size = new System.Drawing.Size(338, 19);
            this.l2dForceBezierCheckBox.TabIndex = 4;
            this.l2dForceBezierCheckBox.Text = "Smooth motions (calc Linear segments as Bezier segments)";
            this.optionTooltip.SetToolTip(this.l2dForceBezierCheckBox, "May help if the exported motions look jerky/not smooth enough");
            this.l2dForceBezierCheckBox.UseVisualStyleBackColor = true;
            // 
            // fbxResetButton
            // 
            this.fbxResetButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.fbxResetButton.Location = new System.Drawing.Point(280, 340);
            this.fbxResetButton.Name = "fbxResetButton";
            this.fbxResetButton.Size = new System.Drawing.Size(88, 27);
            this.fbxResetButton.TabIndex = 21;
            this.fbxResetButton.Text = "Reset";
            this.fbxResetButton.UseVisualStyleBackColor = false;
            this.fbxResetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // uvBindingsLabel
            // 
            this.uvBindingsLabel.AutoSize = true;
            this.uvBindingsLabel.Location = new System.Drawing.Point(7, 184);
            this.uvBindingsLabel.Name = "uvBindingsLabel";
            this.uvBindingsLabel.Size = new System.Drawing.Size(97, 15);
            this.uvBindingsLabel.TabIndex = 17;
            this.uvBindingsLabel.Text = "UV type bindings";
            // 
            // uvIndicesCheckedListBox
            // 
            this.uvIndicesCheckedListBox.FormattingEnabled = true;
            this.uvIndicesCheckedListBox.IntegralHeight = false;
            this.uvIndicesCheckedListBox.Items.AddRange(new object[] {
            "UV0",
            "UV1",
            "UV2",
            "UV3",
            "UV4",
            "UV5",
            "UV6",
            "UV7"});
            this.uvIndicesCheckedListBox.Location = new System.Drawing.Point(18, 206);
            this.uvIndicesCheckedListBox.Name = "uvIndicesCheckedListBox";
            this.uvIndicesCheckedListBox.ScrollAlwaysVisible = true;
            this.uvIndicesCheckedListBox.Size = new System.Drawing.Size(151, 93);
            this.uvIndicesCheckedListBox.TabIndex = 18;
            this.optionTooltip.SetToolTip(this.uvIndicesCheckedListBox, "Checked UVs will be exported (if they exist)");
            this.uvIndicesCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.uvIndicesCheckedListBox_SelectedIndexChanged);
            // 
            // uvTypesListBox
            // 
            this.uvTypesListBox.FormattingEnabled = true;
            this.uvTypesListBox.IntegralHeight = false;
            this.uvTypesListBox.ItemHeight = 15;
            this.uvTypesListBox.Items.AddRange(new object[] {
            "Diffuse",
            "NormalMap",
            "Displacement",
            "Specular",
            "Bump",
            "Emissive",
            "Ambient",
            "Shininess",
            "Reflection",
            "Transparency"});
            this.uvTypesListBox.Location = new System.Drawing.Point(205, 206);
            this.uvTypesListBox.Name = "uvTypesListBox";
            this.uvTypesListBox.ScrollAlwaysVisible = true;
            this.uvTypesListBox.Size = new System.Drawing.Size(151, 93);
            this.uvTypesListBox.TabIndex = 19;
            this.uvTypesListBox.SelectedIndexChanged += new System.EventHandler(this.uvTypesListBox_SelectedIndexChanged);
            // 
            // exportAllUvsAsDiffuseMaps
            // 
            this.exportAllUvsAsDiffuseMaps.AccessibleDescription = "";
            this.exportAllUvsAsDiffuseMaps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportAllUvsAsDiffuseMaps.AutoSize = true;
            this.exportAllUvsAsDiffuseMaps.Location = new System.Drawing.Point(7, 305);
            this.exportAllUvsAsDiffuseMaps.Name = "exportAllUvsAsDiffuseMaps";
            this.exportAllUvsAsDiffuseMaps.Size = new System.Drawing.Size(183, 19);
            this.exportAllUvsAsDiffuseMaps.TabIndex = 20;
            this.exportAllUvsAsDiffuseMaps.Text = "Export all UVs as diffuse maps";
            this.optionTooltip.SetToolTip(this.exportAllUvsAsDiffuseMaps, "Check if some UV maps are missing after export (e.g. in Blender). But this can al" +
        "so cause some bugs with UVs.");
            this.exportAllUvsAsDiffuseMaps.UseVisualStyleBackColor = true;
            this.exportAllUvsAsDiffuseMaps.CheckedChanged += new System.EventHandler(this.exportAllUvsAsDiffuseMaps_CheckedChanged);
            // 
            // exportBlendShape
            // 
            this.exportBlendShape.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportBlendShape.AutoSize = true;
            this.exportBlendShape.Checked = true;
            this.exportBlendShape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportBlendShape.Location = new System.Drawing.Point(7, 104);
            this.exportBlendShape.Name = "exportBlendShape";
            this.exportBlendShape.Size = new System.Drawing.Size(124, 19);
            this.exportBlendShape.TabIndex = 4;
            this.exportBlendShape.Text = "Export blendshape";
            this.exportBlendShape.UseVisualStyleBackColor = true;
            // 
            // exportAnimations
            // 
            this.exportAnimations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportAnimations.AutoSize = true;
            this.exportAnimations.Checked = true;
            this.exportAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportAnimations.Location = new System.Drawing.Point(7, 80);
            this.exportAnimations.Name = "exportAnimations";
            this.exportAnimations.Size = new System.Drawing.Size(122, 19);
            this.exportAnimations.TabIndex = 3;
            this.exportAnimations.Text = "Export animations";
            this.exportAnimations.UseVisualStyleBackColor = true;
            // 
            // scaleFactor
            // 
            this.scaleFactor.DecimalPlaces = 2;
            this.scaleFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.scaleFactor.Location = new System.Drawing.Point(309, 65);
            this.scaleFactor.Name = "scaleFactor";
            this.scaleFactor.Size = new System.Drawing.Size(58, 23);
            this.scaleFactor.TabIndex = 12;
            this.scaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "ScaleFactor";
            // 
            // fbxFormat
            // 
            this.fbxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fbxFormat.FormattingEnabled = true;
            this.fbxFormat.Items.AddRange(new object[] {
            "Binary",
            "Ascii"});
            this.fbxFormat.Location = new System.Drawing.Point(297, 95);
            this.fbxFormat.Name = "fbxFormat";
            this.fbxFormat.Size = new System.Drawing.Size(70, 23);
            this.fbxFormat.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(220, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "FBXFormat";
            // 
            // fbxVersion
            // 
            this.fbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fbxVersion.FormattingEnabled = true;
            this.fbxVersion.Items.AddRange(new object[] {
            "6.1",
            "7.1",
            "7.2",
            "7.3",
            "7.4",
            "7.5"});
            this.fbxVersion.Location = new System.Drawing.Point(313, 126);
            this.fbxVersion.Name = "fbxVersion";
            this.fbxVersion.Size = new System.Drawing.Size(54, 23);
            this.fbxVersion.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(226, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "FBXVersion";
            // 
            // boneSize
            // 
            this.boneSize.Location = new System.Drawing.Point(309, 35);
            this.boneSize.Name = "boneSize";
            this.boneSize.Size = new System.Drawing.Size(58, 23);
            this.boneSize.TabIndex = 10;
            this.boneSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "BoneSize";
            // 
            // exportSkins
            // 
            this.exportSkins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportSkins.AutoSize = true;
            this.exportSkins.Checked = true;
            this.exportSkins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportSkins.Location = new System.Drawing.Point(7, 55);
            this.exportSkins.Name = "exportSkins";
            this.exportSkins.Size = new System.Drawing.Size(89, 19);
            this.exportSkins.TabIndex = 2;
            this.exportSkins.Text = "Export skins";
            this.exportSkins.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "FilterPrecision";
            // 
            // filterPrecision
            // 
            this.filterPrecision.DecimalPlaces = 2;
            this.filterPrecision.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.filterPrecision.Location = new System.Drawing.Point(309, 5);
            this.filterPrecision.Name = "filterPrecision";
            this.filterPrecision.Size = new System.Drawing.Size(58, 23);
            this.filterPrecision.TabIndex = 8;
            this.filterPrecision.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // castToBone
            // 
            this.castToBone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.castToBone.AutoSize = true;
            this.castToBone.Location = new System.Drawing.Point(7, 128);
            this.castToBone.Name = "castToBone";
            this.castToBone.Size = new System.Drawing.Size(143, 19);
            this.castToBone.TabIndex = 5;
            this.castToBone.Text = "All nodes cast to bone";
            this.castToBone.UseVisualStyleBackColor = true;
            // 
            // exportAllNodes
            // 
            this.exportAllNodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportAllNodes.AutoSize = true;
            this.exportAllNodes.Checked = true;
            this.exportAllNodes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportAllNodes.Location = new System.Drawing.Point(7, 31);
            this.exportAllNodes.Name = "exportAllNodes";
            this.exportAllNodes.Size = new System.Drawing.Size(110, 19);
            this.exportAllNodes.TabIndex = 1;
            this.exportAllNodes.Text = "Export all nodes";
            this.exportAllNodes.UseVisualStyleBackColor = true;
            // 
            // eulerFilter
            // 
            this.eulerFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eulerFilter.AutoSize = true;
            this.eulerFilter.Checked = true;
            this.eulerFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.eulerFilter.Location = new System.Drawing.Point(7, 7);
            this.eulerFilter.Name = "eulerFilter";
            this.eulerFilter.Size = new System.Drawing.Size(78, 19);
            this.eulerFilter.TabIndex = 0;
            this.eulerFilter.Text = "EulerFilter";
            this.eulerFilter.UseVisualStyleBackColor = true;
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Controls.Add(this.generalTabPage);
            this.optionsTabControl.Controls.Add(this.imagesTabPage);
            this.optionsTabControl.Controls.Add(this.fbxTabPage);
            this.optionsTabControl.Controls.Add(this.live2DTabPage);
            this.optionsTabControl.Location = new System.Drawing.Point(12, 12);
            this.optionsTabControl.Margin = new System.Windows.Forms.Padding(12);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(398, 415);
            this.optionsTabControl.TabIndex = 1;
            // 
            // generalTabPage
            // 
            this.generalTabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.generalTabPage.Controls.Add(this.panel2);
            this.generalTabPage.Controls.Add(this.rawByteArrayFromMono);
            this.generalTabPage.Controls.Add(this.label6);
            this.generalTabPage.Controls.Add(this.overwriteExistingFiles);
            this.generalTabPage.Controls.Add(this.parallelExportCheckBox);
            this.generalTabPage.Controls.Add(this.convertAudio);
            this.generalTabPage.Controls.Add(this.assetGroupOptions);
            this.generalTabPage.Controls.Add(this.filenameFormatLabel);
            this.generalTabPage.Controls.Add(this.restoreExtensionName);
            this.generalTabPage.Controls.Add(this.filenameFormatComboBox);
            this.generalTabPage.Controls.Add(this.openAfterExport);
            this.generalTabPage.Location = new System.Drawing.Point(4, 24);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Size = new System.Drawing.Size(390, 387);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.parallelExportMaxLabel);
            this.panel2.Controls.Add(this.parallelExportUpDown);
            this.panel2.Location = new System.Drawing.Point(251, 130);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(130, 26);
            this.panel2.TabIndex = 12;
            // 
            // imagesTabPage
            // 
            this.imagesTabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.imagesTabPage.Controls.Add(this.keepSpriteSizeCheckBox);
            this.imagesTabPage.Controls.Add(this.panel1);
            this.imagesTabPage.Controls.Add(this.converttexture);
            this.imagesTabPage.Controls.Add(this.exportSpriteWithAlphaMask);
            this.imagesTabPage.Location = new System.Drawing.Point(4, 24);
            this.imagesTabPage.Name = "imagesTabPage";
            this.imagesTabPage.Size = new System.Drawing.Size(390, 387);
            this.imagesTabPage.TabIndex = 3;
            this.imagesTabPage.Text = "Images";
            // 
            // keepSpriteSizeCheckBox
            // 
            this.keepSpriteSizeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keepSpriteSizeCheckBox.AutoSize = true;
            this.keepSpriteSizeCheckBox.Checked = true;
            this.keepSpriteSizeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keepSpriteSizeCheckBox.Location = new System.Drawing.Point(7, 95);
            this.keepSpriteSizeCheckBox.Name = "keepSpriteSizeCheckBox";
            this.keepSpriteSizeCheckBox.Size = new System.Drawing.Size(154, 19);
            this.keepSpriteSizeCheckBox.TabIndex = 3;
            this.keepSpriteSizeCheckBox.Text = "Keep original sprite sizes";
            this.keepSpriteSizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // fbxTabPage
            // 
            this.fbxTabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.fbxTabPage.Controls.Add(this.dumpMaterialsCheckBox);
            this.fbxTabPage.Controls.Add(this.fbxResetButton);
            this.fbxTabPage.Controls.Add(this.eulerFilter);
            this.fbxTabPage.Controls.Add(this.uvBindingsLabel);
            this.fbxTabPage.Controls.Add(this.exportAllNodes);
            this.fbxTabPage.Controls.Add(this.uvIndicesCheckedListBox);
            this.fbxTabPage.Controls.Add(this.castToBone);
            this.fbxTabPage.Controls.Add(this.uvTypesListBox);
            this.fbxTabPage.Controls.Add(this.filterPrecision);
            this.fbxTabPage.Controls.Add(this.exportAllUvsAsDiffuseMaps);
            this.fbxTabPage.Controls.Add(this.label1);
            this.fbxTabPage.Controls.Add(this.exportBlendShape);
            this.fbxTabPage.Controls.Add(this.exportSkins);
            this.fbxTabPage.Controls.Add(this.exportAnimations);
            this.fbxTabPage.Controls.Add(this.label2);
            this.fbxTabPage.Controls.Add(this.scaleFactor);
            this.fbxTabPage.Controls.Add(this.boneSize);
            this.fbxTabPage.Controls.Add(this.label5);
            this.fbxTabPage.Controls.Add(this.label3);
            this.fbxTabPage.Controls.Add(this.fbxFormat);
            this.fbxTabPage.Controls.Add(this.fbxVersion);
            this.fbxTabPage.Controls.Add(this.label4);
            this.fbxTabPage.Location = new System.Drawing.Point(4, 24);
            this.fbxTabPage.Name = "fbxTabPage";
            this.fbxTabPage.Size = new System.Drawing.Size(390, 387);
            this.fbxTabPage.TabIndex = 1;
            this.fbxTabPage.Text = "FBX";
            // 
            // dumpMaterialsCheckBox
            // 
            this.dumpMaterialsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dumpMaterialsCheckBox.AutoSize = true;
            this.dumpMaterialsCheckBox.Location = new System.Drawing.Point(7, 153);
            this.dumpMaterialsCheckBox.Name = "dumpMaterialsCheckBox";
            this.dumpMaterialsCheckBox.Size = new System.Drawing.Size(110, 19);
            this.dumpMaterialsCheckBox.TabIndex = 6;
            this.dumpMaterialsCheckBox.Text = "Dump materials";
            this.dumpMaterialsCheckBox.UseVisualStyleBackColor = true;
            // 
            // live2DTabPage
            // 
            this.live2DTabPage.BackColor = System.Drawing.SystemColors.Menu;
            this.live2DTabPage.Controls.Add(this.l2dAssetSearchByFilenameCheckBox);
            this.live2DTabPage.Controls.Add(this.l2dModelGroupLabel);
            this.live2DTabPage.Controls.Add(this.l2dModelGroupComboBox);
            this.live2DTabPage.Controls.Add(this.l2dForceBezierCheckBox);
            this.live2DTabPage.Controls.Add(this.l2dMotionExportMethodLabel);
            this.live2DTabPage.Controls.Add(this.l2dMotionExportMethodPanel);
            this.live2DTabPage.Location = new System.Drawing.Point(4, 24);
            this.live2DTabPage.Name = "live2DTabPage";
            this.live2DTabPage.Size = new System.Drawing.Size(390, 387);
            this.live2DTabPage.TabIndex = 2;
            this.live2DTabPage.Text = "Cubism Live2D";
            // 
            // Cancel
            // 
            this.Cancel.AutoSize = true;
            this.Cancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(322, 442);
            this.Cancel.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(88, 27);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ExportOptionsForm
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(422, 481);
            this.Controls.Add(this.optionsTabControl);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.Cancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportOptionsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export options";
            this.Load += new System.EventHandler(this.ExportOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.parallelExportUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.l2dMotionExportMethodPanel.ResumeLayout(false);
            this.l2dMotionExportMethodPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boneSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterPrecision)).EndInit();
            this.optionsTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.generalTabPage.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.imagesTabPage.ResumeLayout(false);
            this.imagesTabPage.PerformLayout();
            this.fbxTabPage.ResumeLayout(false);
            this.fbxTabPage.PerformLayout();
            this.live2DTabPage.ResumeLayout(false);
            this.live2DTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.CheckBox converttexture;
        private System.Windows.Forms.RadioButton tojpg;
        private System.Windows.Forms.RadioButton topng;
        private System.Windows.Forms.RadioButton tobmp;
        private System.Windows.Forms.RadioButton totga;
        private System.Windows.Forms.CheckBox convertAudio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown boneSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox exportSkins;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown filterPrecision;
        private System.Windows.Forms.CheckBox castToBone;
        private System.Windows.Forms.CheckBox exportAllNodes;
        private System.Windows.Forms.CheckBox eulerFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox fbxVersion;
        private System.Windows.Forms.ComboBox fbxFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown scaleFactor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox exportBlendShape;
        private System.Windows.Forms.CheckBox exportAnimations;
        private System.Windows.Forms.ComboBox assetGroupOptions;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox restoreExtensionName;
        private System.Windows.Forms.CheckBox openAfterExport;
        private System.Windows.Forms.CheckBox exportAllUvsAsDiffuseMaps;
        private System.Windows.Forms.ToolTip optionTooltip;
        private System.Windows.Forms.CheckBox exportSpriteWithAlphaMask;
        private System.Windows.Forms.RadioButton towebp;
        private System.Windows.Forms.CheckBox l2dForceBezierCheckBox;
        private System.Windows.Forms.Label l2dMotionExportMethodLabel;
        private System.Windows.Forms.RadioButton l2dAnimationClipRadioButton;
        private System.Windows.Forms.RadioButton l2dMonoBehaviourRadioButton;
        private System.Windows.Forms.Panel l2dMotionExportMethodPanel;
        private System.Windows.Forms.ComboBox filenameFormatComboBox;
        private System.Windows.Forms.Label filenameFormatLabel;
        private System.Windows.Forms.NumericUpDown parallelExportUpDown;
        private System.Windows.Forms.CheckBox parallelExportCheckBox;
        private System.Windows.Forms.Label parallelExportMaxLabel;
        private System.Windows.Forms.Label l2dModelGroupLabel;
        private System.Windows.Forms.ComboBox l2dModelGroupComboBox;
        private System.Windows.Forms.CheckBox l2dAssetSearchByFilenameCheckBox;
        private System.Windows.Forms.CheckedListBox uvIndicesCheckedListBox;
        private System.Windows.Forms.ListBox uvTypesListBox;
        private System.Windows.Forms.Label uvBindingsLabel;
        private System.Windows.Forms.Button fbxResetButton;
        private System.Windows.Forms.CheckBox overwriteExistingFiles;
        private System.Windows.Forms.CheckBox rawByteArrayFromMono;
        private System.Windows.Forms.TabControl optionsTabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage fbxTabPage;
        private System.Windows.Forms.TabPage live2DTabPage;
        private System.Windows.Forms.TabPage imagesTabPage;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox keepSpriteSizeCheckBox;
        private System.Windows.Forms.CheckBox dumpMaterialsCheckBox;
    }
}