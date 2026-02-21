namespace AssetStudio.CustomOptions
{
    public partial class ImportOptions
    {
        public sealed class CustomBundleOptions
        {
            private CompressionType _customBlockCompression = CompressionType.Auto;
            private CompressionType _customBlockInfoCompression = CompressionType.Auto;
            private string _customCachePath;
            private bool _decompressToDisk;

            public CompressionType CustomBlockCompression
            {
                get => _customBlockCompression;
                set => _customBlockCompression = SetOption(nameof(CustomBlockCompression), value, _customBlockCompression);
            }

            public CompressionType CustomBlockInfoCompression
            {
                get => _customBlockInfoCompression;
                set => _customBlockInfoCompression = SetOption(nameof(CustomBlockInfoCompression), value, _customBlockInfoCompression);
            }

            public string CustomCachePath
            {
                get => _customCachePath;
                set => _customCachePath = SetOption(nameof(CustomCachePath), value, _customCachePath);
            }

            public bool DecompressToDisk
            {
                get => _decompressToDisk;
                set => _decompressToDisk = SetOption(nameof(DecompressToDisk), value, _decompressToDisk);
            }

            public CustomBundleOptions() { }

            private static T SetOption<T>(string option, T value, T curValue)
            {
                if (value.Equals(curValue))
                    return curValue;

                var val = string.IsNullOrEmpty(value.ToString())
                    ? "None"
                    : value.ToString();
                Logger.Info($"- {option}: {val}");
                return value;
            }
        }
    }
}
