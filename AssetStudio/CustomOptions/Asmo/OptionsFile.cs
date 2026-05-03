using System;
using System.Buffers.Binary;

namespace AssetStudio.CustomOptions.Asmo
{
    public class OptionsFile
    {
        private static readonly byte[] OptionsFileSign = { 0x41, 0x53, 0x4D, 0x4F }; //ASMO
        public static readonly string Extension = ".asmo";

        public byte[] Signature { get; private set; } = OptionsFileSign;
        public short Version { get; private set; } = 1;
        public short Reserved { get; set; }
        public uint DataCrc { get; set; }
        public int DataSize { get; set; }
        public byte[] Data { get; set; }

        public OptionsFile() { }

        public OptionsFile(EndianBinaryReader reader)
        {
            Signature = reader.ReadBytes(4);
            Version = reader.ReadInt16();
            Reserved = reader.ReadInt16();
            DataCrc = reader.ReadUInt32();
            DataSize = reader.ReadInt32();
            CheckHeader(reader.BaseStream.Length);
            Data = reader.ReadBytes(DataSize);
        }

        public void CheckHeader(long fileLength)
        {
            if (!Signature.AsSpan().SequenceEqual(OptionsFileSign))
                throw new NotSupportedException("Incorrect options file signature.");

            if (Version != 1)
                throw new NotSupportedException("Incorrect options file version.");

            if (DataSize <= 0 || DataSize > fileLength)
                throw new NotSupportedException("Incorrect data size.");
        }

        public byte[] ToByteArray()
        {
            var buffer = new byte[16 + DataSize];
            OptionsFileSign.AsSpan().CopyTo(buffer);
            BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(4), Version);
            BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(6), Reserved);
            BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan(8), DataCrc);
            BinaryPrimitives.WriteInt32BigEndian(buffer.AsSpan(12), DataSize);
            Data.AsSpan().CopyTo(buffer.AsSpan(16));

            return buffer;
        }
    }
}
