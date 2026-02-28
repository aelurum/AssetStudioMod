using System;
using System.IO;

namespace AssetStudio
{
    public class ObjectReader : EndianBinaryReader
    {
        public SerializedFile assetsFile;
        public long m_PathID;
        public long byteStart;
        public uint byteSize;
        public int classID;
        public ClassIDType type;
        public SerializedType serializedType;
        public BuildTarget platform;
        public SerializedFileFormatVersion m_Version;

        public UnityVersion version => assetsFile.version;

        public long Remaining => byteStart + byteSize - Position;

        public ObjectReader(EndianBinaryReader reader, SerializedFile assetsFile, ObjectInfo objectInfo) : base(reader.BaseStream, reader.Endian)
        {
            this.assetsFile = assetsFile;
            m_PathID = objectInfo.m_PathID;
            byteStart = objectInfo.byteStart;
            byteSize = objectInfo.byteSize;
            classID = objectInfo.classID;
            if (Enum.IsDefined(typeof(ClassIDType), objectInfo.classID))
            {
                type = (ClassIDType)objectInfo.classID;
            }
            else
            {
                type = ClassIDType.UnknownType;
            }
            serializedType = objectInfo.serializedType;
            platform = assetsFile.m_TargetPlatform;
            m_Version = assetsFile.header.m_Version;

            if (assetsFile.ExtractedTypeTreeHashes.Count > 0)
            {
                assetsFile.ReadExtractedTypeTree();
            }
        }

        public void ThrowIfTooLarge(float val)
        {
            if (val < 0 || val > Remaining)
                throw new EndOfStreamException();
        }

        public void Reset()
        {
            Position = byteStart;
        }
    }
}
