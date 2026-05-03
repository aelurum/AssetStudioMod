namespace AssetStudio.CustomOptions
{
    public partial class GameSpecificOptions
    {
        public sealed class CustomBundleOptions
        {
            private CompressionType _customBlockCompression = CompressionType.Auto;
            private CompressionType _customBlockInfoCompression = CompressionType.Auto;

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

            public CustomBundleOptions() { }
        }
    }
}
