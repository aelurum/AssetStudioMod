using AssetStudio;

namespace AssetStudioCLI
{
    internal class AssetItem
    {
        private static long _counter;

        public Object Asset;
        public SerializedFile SourceFile;
        public string Container = string.Empty;
        public string TypeString;
        public long m_PathID;
        public long FullSize;
        public ClassIDType Type;
        public string Text;
        public readonly string UniqueID;
        public ulong Hash;
        public GameObjectNode Node;

        public AssetItem(Object asset)
        {
            Asset = asset;
            SourceFile = asset.assetsFile;
            Type = asset.type;
            TypeString = Type.ToString();
            m_PathID = asset.m_PathID;
            FullSize = asset.byteSize;
            UniqueID = $" #{_counter++}";
            Hash = 0;
        }
    }
}
