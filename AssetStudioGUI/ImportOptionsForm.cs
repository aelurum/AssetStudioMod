using AssetStudio;
using AssetStudio.CustomOptions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public partial class ImportOptionsForm : Form
    {
        private static readonly string SampleVersion = "e.g. 2022.3.2f1";
        private readonly AssetsManager _assetsManager;
        private string _cachePath;

        public ImportOptionsForm(AssetsManager assetsManager)
        {
            InitializeComponent();
            _assetsManager = assetsManager;
            if (assetsManager.Options.CustomUnityVersion != null)
            {
                customUnityVersionTextBox.Text = assetsManager.Options.CustomUnityVersion.FullVersion;
            }
            else
            {
                ResetUnityVersionTextBox();
            }
            legacyUnityVersionComboBox.SelectedIndex = SetLegacyUnityVersionComboBoxIndex();
            cachePathLabel.AutoSize = IsWine; //to disable AutoEllipsis in Wine
            _cachePath = _assetsManager.Options.BundleOptions.CustomCachePath;
            DisplayCachePath();
            alwaysDecompressToDiskCheckBox.Checked = assetsManager.Options.BundleOptions.DecompressToDisk;
            customBlockInfoCompressionComboBox.SelectedIndex = SetCompressionComboBoxIndex(assetsManager.Options.BundleOptions.CustomBlockInfoCompression);
            customBlockCompressionComboBox.SelectedIndex = SetCompressionComboBoxIndex(assetsManager.Options.BundleOptions.CustomBlockCompression);
            var tMeshMode = assetsManager.Options.MeshOptions.TuanjieMeshParsingMode;
            ((RadioButton)panel1.Controls.Cast<Control>().First(x => x.TabIndex == (int)tMeshMode)).Checked = true;
        }

        private void ApplyOptions()
        {
            if (string.IsNullOrEmpty(customUnityVersionTextBox.Text) || customUnityVersionTextBox.Text == SampleVersion)
            {
                _assetsManager.Options.CustomUnityVersion = null;
            }
            else
            {
                try
                {
                    _assetsManager.Options.CustomUnityVersion = new UnityVersion(customUnityVersionTextBox.Text);
                    legacyUnityVersionComboBox.SelectedIndex = SetLegacyUnityVersionComboBoxIndex();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }

            _assetsManager.Options.BundleOptions.CustomCachePath = _cachePath;
            Properties.Settings.Default.cachePath = _cachePath;

            var blockCompression = GetCustomCompressionTypes(customBlockCompressionComboBox.SelectedIndex);
            _assetsManager.Options.BundleOptions.CustomBlockCompression = blockCompression;
            var blockInfoCompression = GetCustomCompressionTypes(customBlockInfoCompressionComboBox.SelectedIndex);
            _assetsManager.Options.BundleOptions.CustomBlockInfoCompression = blockInfoCompression;

            var decompressToDisk = alwaysDecompressToDiskCheckBox.Checked;
            _assetsManager.Options.BundleOptions.DecompressToDisk = decompressToDisk;
            Properties.Settings.Default.decompressToDisk = decompressToDisk;

            var tMeshModeIndex = ((RadioButton)panel1.Controls.Cast<Control>().First(x => ((RadioButton)x).Checked)).TabIndex;
            _assetsManager.Options.MeshOptions.TuanjieMeshParsingMode = (ImportOptions.CustomMeshOptions.MeshParsingMode)tMeshModeIndex;

            Properties.Settings.Default.Save();
        }

        #region General
        private void customUnityVersionTextBox_Enter(object sender, EventArgs e)
        {
            if (customUnityVersionTextBox.Text == SampleVersion)
            {
                customUnityVersionTextBox.Text = "";
                customUnityVersionTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void customUnityVersionTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(customUnityVersionTextBox.Text))
            {
                ResetUnityVersionTextBox();
            }
        }

        private void ResetUnityVersionTextBox()
        {
            customUnityVersionTextBox.ForeColor = System.Drawing.SystemColors.GrayText;
            customUnityVersionTextBox.Text = SampleVersion;
        }

        private void legacyUnityVersionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var version = GetLegacyUnityVersion(legacyUnityVersionComboBox.SelectedIndex);
            if (version != null)
            {
                customUnityVersionTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
                customUnityVersionTextBox.Text = version.ToString();
            }
            else
            {
                ResetUnityVersionTextBox();
            }
        }

        private static UnityVersion GetLegacyUnityVersion(int index)
        {
            switch (index)
            {
                case 0: return null;
                case 1: return new UnityVersion(1, 2);
                case 2: return new UnityVersion(1, 6);
                case 3: return new UnityVersion(2, 0);
                case 4: return new UnityVersion(2, 1);
                case 5: return new UnityVersion(2, 5);
                case 6: return new UnityVersion(2, 6);
                default: throw new NotSupportedException();
            }
        }

        private int SetLegacyUnityVersionComboBoxIndex()
        {
            var version = _assetsManager.Options.CustomUnityVersion;

            switch (version?.Major)
            {
                case 2 when version.Minor >= 6: return 6;
                case 2 when version.Minor == 5: return 5;
                case 2 when version.Minor > 0: return 4;
                case 2: return 3;
                case 1 when version.Minor >= 7: return 4;
                case 1 when version.Minor >= 5: return 2;
                case 1: return 1;
            }
            
            return 0;
        }
        #endregion

        #region Asset Bundles
        private static CompressionType GetCustomCompressionTypes(int index)
        {
            switch (index)
            {
                case 0: return CompressionType.Auto;
                case 1: return CompressionType.Zstd;
                case 2: return CompressionType.Oodle;
                case 3: return CompressionType.Lz4HC;
                case 4: return CompressionType.Lzma;
                default: throw new NotSupportedException();
            }
        }

        private static int SetCompressionComboBoxIndex(CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.Auto: return 0;
                case CompressionType.Lzma: return 4;
                case CompressionType.Lz4:
                case CompressionType.Lz4HC: return 3;
                case CompressionType.Zstd: return 1;
                case CompressionType.Oodle: return 2;
                default: throw new NotSupportedException();
            }
        }

        private void DisplayCachePath()
        {
            cachePathLabel.Text = $"Cache directory: {_cachePath}";
            toolTip1.SetToolTip(cachePathLabel, _cachePath);
            var driveInfo = new DriveInfo(Directory.GetDirectoryRoot(_cachePath));
            cacheFreeSpaceLabel.Text = $"Available space: {driveInfo.AvailableFreeSpace * 1.0f / 0x40000000:.00}GB";
        }

        private void cachePathSelectButton_Click(object sender, EventArgs e)
        {
            var cachePathFolderDialog = new OpenFolderDialog();
            cachePathFolderDialog.Title = "Select the cache directory";
            if (cachePathFolderDialog.ShowDialog(this) != DialogResult.OK)
                return;

            _cachePath = cachePathFolderDialog.Folder;
            DisplayCachePath();
        }

        private void cachePathResetButton_Click(object sender, EventArgs e)
        {
            _cachePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            if (!Directory.Exists(_cachePath))
                Directory.CreateDirectory(_cachePath);
            DisplayCachePath();
        }
        #endregion

        private void saveToDiskButton_Click(object sender, EventArgs e)
        {
            ApplyOptions();
            var saveFolderDialog = new OpenFolderDialog();
            saveFolderDialog.Title = "Select the save folder";
            if (saveFolderDialog.ShowDialog(this) == DialogResult.OK)
            {
                var savePath = saveFolderDialog.Folder;
                _assetsManager.Options.SaveToFile(savePath);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            ApplyOptions();
        }

        private static bool IsWine => Process.GetProcessesByName("winlogon").Length == 0;
    }
}
