namespace AssetStudio
{
    public class TypeTreeNode
    {
        public string m_Type;
        public string m_Name;
        public int m_ByteSize;
        public int m_Index;
        public int m_TypeFlags; //m_IsArray
        public int m_Version;
        public int m_MetaFlag;
        public int m_Level;
        public uint m_TypeStrOffset;
        public uint m_NameStrOffset;
        public ulong m_RefTypeHash;

        public TypeTreeNode() { }

        public TypeTreeNode(TypeTreeNode node)
        {
            m_Type = node.m_Type;
            m_Name = node.m_Name;
            m_ByteSize = node.m_ByteSize;
            m_Index = node.m_Index;
            m_TypeFlags = node.m_TypeFlags;
            m_Version = node.m_Version;
            m_MetaFlag = node.m_MetaFlag;
            m_Level = node.m_Level;
            m_TypeStrOffset = node.m_TypeStrOffset;
            m_NameStrOffset = node.m_NameStrOffset;
            m_RefTypeHash = node.m_RefTypeHash;
        }

        public TypeTreeNode(string type, string name, int level, bool align)
        {
            m_Type = type;
            m_Name = name;
            m_Level = level;
            m_MetaFlag = align ? 0x4000 : 0;
        }
    }
}
