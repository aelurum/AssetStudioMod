using System;
using System.Collections.Generic;
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

            if (assetsFile.IsTypeTreeExtracted)
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

        public override short ReadInt16()
        {
            if (Remaining < 2)
                throw new EndOfStreamException();

            return base.ReadInt16();
        }

        public override ushort ReadUInt16()
        {
            if (Remaining < 2)
                throw new EndOfStreamException();

            return base.ReadUInt16();
        }

        public override int ReadInt32()
        {
            if (Remaining < 4)
                throw new EndOfStreamException();
            
            return base.ReadInt32();
        }

        public override uint ReadUInt32()
        {
            if (Remaining < 4)
                throw new EndOfStreamException();

            return base.ReadUInt32();
        }

        public override float ReadSingle()
        {
            if (Remaining < 4)
                throw new EndOfStreamException();

            return base.ReadSingle();
        }
    }

    public static class ObjectReaderExtensions
    {
        public static string ReadAlignedString(this ObjectReader reader)
        {
            var needAlign = reader.version >= (2, 1); //aligned only since v2.1
            return reader.ReadAlignedString(needAlign);
        }

        public static string[] ReadStringArray(this ObjectReader reader)
        {
            var length = reader.ReadInt32();
            var list = new List<string>();
            var needAlign = reader.version >= (2, 1);
            for (var i = 0; i < length; i++)
            {
                list.Add(reader.ReadAlignedString(needAlign));
            }
            return list.ToArray();
        }
    }
}
