namespace AssetStudio.CustomOptions
{
    public partial class ImportOptions
    {
        public sealed class CustomBundleOptions
        {
            private CompressionType _customBlockCompression = CompressionType.Auto;
            private CompressionType _customBlockInfoCompression = CompressionType.Auto;
            private bool _decompressToDisk;

            public CompressionType CustomBlockCompression
            {
                get => _customBlockCompression;
                set => _customBlockCompression = SetOption(nameof(CustomBlockCompression), value);
            }

            public CompressionType CustomBlockInfoCompression
            {
                get => _customBlockInfoCompression;
                set => _customBlockInfoCompression = SetOption(nameof(CustomBlockInfoCompression), value);
            }

            public bool DecompressToDisk
            {
                get => _decompressToDisk;
                set => _decompressToDisk = SetOption(nameof(DecompressToDisk), value);
            }

            public CustomBundleOptions() { }

            private static T SetOption<T>(string option, T value)
            {
                Logger.Info($"- {option}: {value}");
                return value;
            }
        }
    }
}
