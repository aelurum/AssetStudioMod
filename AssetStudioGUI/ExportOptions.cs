using AssetStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public partial class ExportOptions : Form
    {
        private static Dictionary<int, int> uvBindings;

        public ExportOptions()
        {
            InitializeComponent();
            assetGroupOptions.SelectedIndex = Properties.Settings.Default.assetGroupOption;
            filenameFormatComboBox.SelectedIndex = Properties.Settings.Default.filenameFormat;
            overwriteExistingFiles.Checked = Properties.Settings.Default.overwriteExistingFiles;
            restoreExtensionName.Checked = Properties.Settings.Default.restoreExtensionName;
            converttexture.Checked = Properties.Settings.Default.convertTexture;
            exportSpriteWithAlphaMask.Checked = Properties.Settings.Default.exportSpriteWithMask;
            keepSpriteSizeCheckBox.Checked = Properties.Settings.Default.keepSpriteSize;
            convertAudio.Checked = Properties.Settings.Default.convertAudio;
            var defaultImageType = Properties.Settings.Default.convertType.ToString();
            ((RadioButton)panel1.Controls.Cast<Control>().First(x => x.Text == defaultImageType)).Checked = true;
            openAfterExport.Checked = Properties.Settings.Default.openAfterExport;
            var maxParallelTasks = Environment.ProcessorCount;
            var taskCount = Properties.Settings.Default.parallelExportCount;
            parallelExportUpDown.Maximum = maxParallelTasks;
            parallelExportUpDown.Value = taskCount <= 0 ? maxParallelTasks : Math.Min(taskCount, maxParallelTasks);
            parallelExportMaxLabel.Text += maxParallelTasks;
            parallelExportCheckBox.Checked = Properties.Settings.Default.parallelExport;
            rawByteArrayFromMono.Checked = Properties.Settings.Default.rawByteArrayFromMono;

            l2dModelGroupComboBox.SelectedIndex = (int)Properties.Settings.Default.l2dModelGroupOption;
            l2dAssetSearchByFilenameCheckBox.Checked = Properties.Settings.Default.l2dAssetSearchByFilename;
            var defaultMotionMode = Properties.Settings.Default.l2dMotionMode.ToString();
            ((RadioButton)l2dMotionExportMethodPanel.Controls.Cast<Control>().First(x => x.AccessibleName == defaultMotionMode)).Checked = true;
            l2dForceBezierCheckBox.Checked = Properties.Settings.Default.l2dForceBezier;

            SetFromFbxSettings(Studio.FbxSettings);
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.assetGroupOption = assetGroupOptions.SelectedIndex;
            Properties.Settings.Default.filenameFormat = filenameFormatComboBox.SelectedIndex;
            Properties.Settings.Default.overwriteExistingFiles = overwriteExistingFiles.Checked;
            Properties.Settings.Default.restoreExtensionName = restoreExtensionName.Checked;
            Properties.Settings.Default.convertTexture = converttexture.Checked;
            Properties.Settings.Default.exportSpriteWithMask = exportSpriteWithAlphaMask.Checked;
            Properties.Settings.Default.keepSpriteSize = keepSpriteSizeCheckBox.Checked;
            Properties.Settings.Default.convertAudio = convertAudio.Checked;
            var checkedImageType = (RadioButton)panel1.Controls.Cast<Control>().First(x => ((RadioButton)x).Checked);
            Properties.Settings.Default.convertType = (ImageFormat)Enum.Parse(typeof(ImageFormat), checkedImageType.Text);
            Properties.Settings.Default.openAfterExport = openAfterExport.Checked;
            Properties.Settings.Default.parallelExport = parallelExportCheckBox.Checked;
            Properties.Settings.Default.parallelExportCount = (int)parallelExportUpDown.Value;
            Properties.Settings.Default.rawByteArrayFromMono = rawByteArrayFromMono.Checked;

            Properties.Settings.Default.l2dModelGroupOption = (CubismLive2DExtractor.Live2DModelGroupOption)l2dModelGroupComboBox.SelectedIndex;
            Properties.Settings.Default.l2dAssetSearchByFilename = l2dAssetSearchByFilenameCheckBox.Checked;
            var checkedMotionMode = (RadioButton)l2dMotionExportMethodPanel.Controls.Cast<Control>().First(x => ((RadioButton)x).Checked);
            Properties.Settings.Default.l2dMotionMode = (CubismLive2DExtractor.Live2DMotionMode)Enum.Parse(typeof(CubismLive2DExtractor.Live2DMotionMode), checkedMotionMode.AccessibleName);
            Properties.Settings.Default.l2dForceBezier = l2dForceBezierCheckBox.Checked;

            Studio.FbxSettings.EulerFilter = eulerFilter.Checked;
            Studio.FbxSettings.FilterPrecision = (float)filterPrecision.Value;
            Studio.FbxSettings.ExportAllNodes = exportAllNodes.Checked;
            Studio.FbxSettings.ExportSkins = exportSkins.Checked;
            Studio.FbxSettings.ExportAnimations = exportAnimations.Checked;
            Studio.FbxSettings.ExportBlendShape = exportBlendShape.Checked;
            Studio.FbxSettings.CastToBone = castToBone.Checked;
            Studio.FbxSettings.ExportAllUvsAsDiffuseMaps = exportAllUvsAsDiffuseMaps.Checked;
            Studio.FbxSettings.BoneSize = (int)boneSize.Value;
            Studio.FbxSettings.ScaleFactor = (float)scaleFactor.Value;
            Studio.FbxSettings.FbxVersionIndex = fbxVersion.SelectedIndex;
            Studio.FbxSettings.FbxFormat = fbxFormat.SelectedIndex;
            for (var i = 0; i < uvIndicesCheckedListBox.Items.Count; i++)
            {
                var isChecked = uvIndicesCheckedListBox.GetItemChecked(i);
                var type = uvBindings[i];
                if ((isChecked && type < 0) || (!isChecked && type > 0))
                    uvBindings[i] *= -1;
            }
            Studio.FbxSettings.UvBindings = uvBindings;
            Properties.Settings.Default.fbxSettings = Studio.FbxSettings.ToBase64();

            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void parallelExportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            parallelExportUpDown.Enabled = parallelExportCheckBox.Checked;
        }

        private void uvIndicesCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (exportAllUvsAsDiffuseMaps.Checked)
                return;

            if (uvBindings.TryGetValue(uvIndicesCheckedListBox.SelectedIndex, out var uvType))
            {
                uvTypesListBox.SelectedIndex = (int)MathF.Abs(uvType) - 1;
            }
        }

        private void uvTypesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedUv = uvIndicesCheckedListBox.SelectedIndex;
            uvBindings[selectedUv] = uvTypesListBox.SelectedIndex + 1;
        }

        private void exportAllUvsAsDiffuseMaps_CheckedChanged(object sender, EventArgs e)
        {
            uvTypesListBox.Enabled = !exportAllUvsAsDiffuseMaps.Checked;
            uvIndicesCheckedListBox.Enabled = !exportAllUvsAsDiffuseMaps.Checked;
        }

        private void SetFromFbxSettings(Fbx.Settings fbxSettings)
        {
            eulerFilter.Checked = fbxSettings.EulerFilter;
            filterPrecision.Value = (decimal)fbxSettings.FilterPrecision;
            exportAllNodes.Checked = fbxSettings.ExportAllNodes;
            exportSkins.Checked = fbxSettings.ExportSkins;
            exportAnimations.Checked = fbxSettings.ExportAnimations;
            exportBlendShape.Checked = fbxSettings.ExportBlendShape;
            castToBone.Checked = fbxSettings.CastToBone;
            exportAllUvsAsDiffuseMaps.Checked = fbxSettings.ExportAllUvsAsDiffuseMaps;
            boneSize.Value = (decimal)fbxSettings.BoneSize;
            scaleFactor.Value = (decimal)fbxSettings.ScaleFactor;
            fbxVersion.SelectedIndex = fbxSettings.FbxVersionIndex;
            fbxFormat.SelectedIndex = fbxSettings.FbxFormat;
            uvBindings = new Dictionary<int, int>(fbxSettings.UvBindings);
            for (var i = 0; i < uvIndicesCheckedListBox.Items.Count; i++)
            {
                var isChecked = uvBindings[i] > 0;
                uvIndicesCheckedListBox.SetItemChecked(i, isChecked);
            }
            uvTypesListBox.Enabled = !exportAllUvsAsDiffuseMaps.Checked;
            uvIndicesCheckedListBox.Enabled = !exportAllUvsAsDiffuseMaps.Checked;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            SetFromFbxSettings(new Fbx.Settings());
            uvIndicesCheckedListBox_SelectedIndexChanged(sender, e);
        }

        //highDpi workaround (net472/.net-win7)
        private void ExportOptions_Load(object sender, EventArgs e)
        {
            if (Font.Height == 16)
                return;

            var k = Font.Height / 16.0f;
            var listHeight = (int)(uvTypesListBox.ItemHeight * 6 * k);
            uvTypesListBox.Height = uvIndicesCheckedListBox.Height = listHeight;
        }
    }
}
