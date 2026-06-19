using AssetStudio;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public partial class StudioOptionsForm : Form
    {
        private readonly AssetsManagerOptions _studioOptions;
        private string _cachePath;

        public StudioOptionsForm(AssetsManagerOptions studioOptions)
        {
            InitializeComponent();
            _studioOptions = studioOptions;

            cachePathLabel.AutoSize = IsWine; //to disable AutoEllipsis in Wine
            _cachePath = _studioOptions.CustomCachePath;
            DisplayCachePath();
            alwaysDecompressToDiskCheckBox.Checked = studioOptions.DecompressToDisk;
            buildSceneHierarchyCheckBox.Checked = Properties.Settings.Default.buildTreeStructure;
            
            displayAllCheckBox.Checked = Properties.Settings.Default.displayAll;
            meshPreProcessingCheckBox.Checked = _studioOptions.MeshPreProcessing;
            parseByTypetreeCheckBox.Checked = _studioOptions.ParseByTypeTree;
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
            Directory.CreateDirectory(_cachePath);
            DisplayCachePath();
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            #region General
            _studioOptions.CustomCachePath = _cachePath;
            Properties.Settings.Default.cachePath = _cachePath;

            var decompressToDisk = alwaysDecompressToDiskCheckBox.Checked;
            _studioOptions.DecompressToDisk = decompressToDisk;
            Properties.Settings.Default.decompressToDisk = decompressToDisk;

            Properties.Settings.Default.buildTreeStructure = buildSceneHierarchyCheckBox.Checked;
            #endregion

            #region Assets
            Properties.Settings.Default.displayAll = displayAllCheckBox.Checked;
            
            var meshPreProcessing = meshPreProcessingCheckBox.Checked;
            _studioOptions.MeshPreProcessing = meshPreProcessing;
            Properties.Settings.Default.meshPreProcessing = meshPreProcessing;
            
            var parseByTypeTree = parseByTypetreeCheckBox.Checked;
            _studioOptions.ParseByTypeTree = parseByTypeTree;
            Properties.Settings.Default.useTypetreeLoading = parseByTypeTree;
            #endregion

            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static bool IsWine => Process.GetProcessesByName("winlogon").Length == 0;
    }
}
