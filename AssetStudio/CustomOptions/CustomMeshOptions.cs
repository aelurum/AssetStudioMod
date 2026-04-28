namespace AssetStudio.CustomOptions
{
    public partial class ImportOptions
    {
        public sealed class CustomMeshOptions
        {
            public enum MeshParsingMode
            {
                Auto,
                Tuanjie,
                Unity,
            }

            private MeshParsingMode _tuanjieMeshParsingMode;

            public MeshParsingMode TuanjieMeshParsingMode
            {
                get => _tuanjieMeshParsingMode;
                set => _tuanjieMeshParsingMode = SetOption(nameof(TuanjieMeshParsingMode), value, _tuanjieMeshParsingMode);
            }

            public CustomMeshOptions() { }
        }
    }
}
