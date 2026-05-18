using System.Windows.Forms;
using AssetStudio;

namespace AssetStudioGUI
{
    internal class AssetItem : ListViewItem
    {
        private static long _counter;

        public Object Asset;
        public SerializedFile SourceFile;
        public string Container = string.Empty;
        public string TypeString;
        public long m_PathID;
        public long FullSize;
        public ClassIDType Type;
        public string InfoText;
        public readonly string UniqueID;
        public ulong Hash;
        public GameObjectTreeNode TreeNode;

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

        public void SetSubItems()
        {
            SubItems.AddRange(new[]
            {
                Container, //Container
                TypeString, //Type
                m_PathID.ToString(), //PathID
                FullSize.ToString(), //Size
            });
        }

        public static void Reset()
        {
            _counter = 0;
        }
    }
}
