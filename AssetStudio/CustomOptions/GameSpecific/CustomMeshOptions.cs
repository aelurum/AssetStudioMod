namespace AssetStudio.CustomOptions.GameSpecific
{
    public sealed class CustomMeshOptions : CustomOption
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
