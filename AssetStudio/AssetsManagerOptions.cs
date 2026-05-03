using AssetStudio.CustomOptions;
using AssetStudio.CustomOptions.Asmo;
using System;

namespace AssetStudio
{
    public class AssetsManagerOptions
    {
        public readonly GameSpecificOptions GameSpecific = new GameSpecificOptions();

        public bool ParseByTypeTree { get; set; } = true;
        public bool MeshPreProcessing { get; set; }
        public bool DecompressToDisk { get; set; }
        public string CustomCachePath { get; set; }

        internal void LoadGameSpecificOptionsFromFile(OptionsFile optionsFile)
        {
            try
            {
                if (!GameSpecific.FromFile(optionsFile))
                    return;

                Logger.Info("Game-specific options successfully loaded.");
            }
            catch (Exception e)
            {
                Logger.Warning($"Error while reading the game-specific options\n{e}");
            }
        }
    }
}
