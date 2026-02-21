using AssetStudio;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public partial class ImportOptionsForm : Form
    {
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
            unityVerAutoRadioButton.Checked = true;
            cachePathLabel.AutoSize = IsWine; //to disable AutoEllipsis in Wine
            _cachePath = _assetsManager.Options.BundleOptions.CustomCachePath;
            DisplayCachePath();
            alwaysDecompressToDiskCheckBox.Checked = assetsManager.Options.BundleOptions.DecompressToDisk;
            customBlockInfoCompressionComboBox.SelectedIndex = SetComboBoxIndex(assetsManager.Options.BundleOptions.CustomBlockInfoCompression);
            customBlockCompressionComboBox.SelectedIndex = SetComboBoxIndex(assetsManager.Options.BundleOptions.CustomBlockCompression);
        }

        private void ApplyOptions()
        {
            if (string.IsNullOrEmpty(customUnityVersionTextBox.Text))
            {
                _assetsManager.Options.CustomUnityVersion = null;
            }
            else
            {
                try
                {
                    _assetsManager.Options.CustomUnityVersion = new UnityVersion(customUnityVersionTextBox.Text);
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

            Properties.Settings.Default.Save();
        }

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

        private static int SetComboBoxIndex(CompressionType compressionType)
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
