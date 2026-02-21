using System;
using System.IO;

namespace AssetStudio
{
    public class FileReader : EndianBinaryReader
    {
        public string FullPath;
        public string FileName;
        public FileType FileType;

        private static readonly byte[] gzipMagic = { 0x1f, 0x8b };
        private static readonly byte[] brotliMagic = { 0x62, 0x72, 0x6F, 0x74, 0x6C, 0x69 };
        private static readonly byte[] zipMagic = { 0x50, 0x4B, 0x03, 0x04 };
        private static readonly byte[] zipSpannedMagic = { 0x50, 0x4B, 0x07, 0x08 };
        private static readonly byte[] unityFsMagic = {0x55, 0x6E, 0x69, 0x74, 0x79, 0x46, 0x53, 0x00};
        private static readonly int headerBuffLen = 1152;
        private static byte[] headerBuff = new byte[headerBuffLen];

        public FileReader(string path) : this(path, File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) { }

        public FileReader(string path, Stream stream) : base(stream, EndianType.BigEndian)
        {
            FullPath = Path.GetFullPath(path);
            FileName = Path.GetFileName(path);
            FileType = CheckFileType();
        }

        private FileType CheckFileType()
        {
            var buff = headerBuff.AsSpan();
            buff.Clear();
            var dataLen = Read(headerBuff, 0, headerBuffLen);
            Position = 0;

            var signature = buff.ReadStringToNull(maxLength: 20);
            switch (signature)
            {
                case "UnityWeb":
                case "UnityRaw":
                case "UnityArchive":
                case "UnityFS":
                    CheckBundleDataOffset(buff);
                    return FileType.BundleFile;
                case "UnityWebData1.0":
                case "TuanjieWebData1.0":
                    return FileType.WebFile;
                default:
                {
                    var magic = Span<byte>.Empty;

                    magic = dataLen > 2 ? buff.Slice(0, 2) : magic;
                    if (magic.SequenceEqual(gzipMagic))
                    {
                        return FileType.GZipFile;
                    }

                    magic = dataLen > 38 ? buff.Slice(32, 6) : magic;
                    if (magic.SequenceEqual(brotliMagic))
                    {
                        return FileType.BrotliFile;
                    }

                    if (IsSerializedFile(buff))
                    {
                        return FileType.AssetsFile;
                    }

                    magic = dataLen > 4 ? buff.Slice(0, 4): magic;
                    if (magic.SequenceEqual(zipMagic) || magic.SequenceEqual(zipSpannedMagic))
                    {
                        return FileType.ZipFile;
                    }

                    if (CheckBundleDataOffset(buff))
                    {
                        return FileType.BundleFile;
                    }

                    return FileType.ResourceFile;
                }
            }
        }

        private bool IsSerializedFile(ReadOnlySpan<byte> buff)
        {
            var fileSize = BaseStream.Length;
            if (fileSize < 20)
            {
                return false;
            }
            var isBigEndian = Endian == EndianType.BigEndian;

            //var m_MetadataSize = buff.ReadUInt32(0, isBigEndian);
            long m_FileSize = buff.ReadUInt32(4, isBigEndian);
            var m_Version = buff.ReadUInt32(8, isBigEndian);
            long m_DataOffset = buff.ReadUInt32(12, isBigEndian);
            //var m_Endianess = buff[16];
            //var m_Reserved = buff.Slice(17, 3);
            if (m_Version >= 22)
            {
                if (fileSize < 48)
                {
                    return false;
                }
                //m_MetadataSize = buff.ReadUInt32(20, isBigEndian);
                m_FileSize = buff.ReadInt64(24, isBigEndian);
                m_DataOffset = buff.ReadInt64(32, isBigEndian);
            }
            if (m_FileSize != fileSize || m_DataOffset > fileSize)
            {
                return false;
            }
           
            return true;
        }

        private bool CheckBundleDataOffset(ReadOnlySpan<byte> buff)
        {
            var lastOffset = buff.LastIndexOf(unityFsMagic);
            if (lastOffset <= 0)
                return false;

            var firstOffset = buff.IndexOf(unityFsMagic);
            if (firstOffset == lastOffset || lastOffset - firstOffset < 200)
            {
                Position = lastOffset;
                return true;
            }

            var pos = firstOffset + 12;
            pos += buff.Slice(pos).ReadStringToNull().Length + 1;
            pos += buff.Slice(pos).ReadStringToNull().Length + 1;
            var bundleSize = buff.ReadInt64(pos, Endian == EndianType.BigEndian);
            if (bundleSize > 200 && firstOffset + bundleSize < lastOffset)
            {
                Position = firstOffset;
                return true;
            }

            Position = lastOffset;
            return true;
        }
    }
}
