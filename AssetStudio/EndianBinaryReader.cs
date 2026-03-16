using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;

namespace AssetStudio
{
    public class EndianBinaryReader : BinaryReader
    {
        private readonly byte[] buffer;

        public EndianType Endian;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReadToBuff(byte[] buff, int index, int count)
        {
            var read = Read(buff, index, count);
            if (read == 0)
                throw new EndOfStreamException();
        }

        public EndianBinaryReader(Stream stream, EndianType endian = EndianType.BigEndian) : base(stream)
        {
            Endian = endian;
            buffer = new byte[8];
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public override short ReadInt16()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 2);
                return BinaryPrimitives.ReadInt16BigEndian(buffer);
            }
            return base.ReadInt16();
        }

        public override int ReadInt32()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 4);
                return BinaryPrimitives.ReadInt32BigEndian(buffer);
            }
            return base.ReadInt32();
        }

        public override long ReadInt64()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 8);
                return BinaryPrimitives.ReadInt64BigEndian(buffer);
            }
            return base.ReadInt64();
        }

        public override ushort ReadUInt16()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 2);
                return BinaryPrimitives.ReadUInt16BigEndian(buffer);
            }
            return base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 4);
                return BinaryPrimitives.ReadUInt32BigEndian(buffer);
            }
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 8);
                return BinaryPrimitives.ReadUInt64BigEndian(buffer);
            }
            return base.ReadUInt64();
        }

#if NETFRAMEWORK
        public override float ReadSingle()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 4);
                buffer.AsSpan(0, 4).Reverse();
                return BitConverter.ToSingle(buffer, 0);
            }
            return base.ReadSingle();
        }

        public override double ReadDouble()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 8);
                buffer.AsSpan().Reverse();
                return BitConverter.ToDouble(buffer, 0);
            }
            return base.ReadDouble();
        }
#else
        public override float ReadSingle()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 4);
                return BinaryPrimitives.ReadSingleBigEndian(buffer);
            }
            return base.ReadSingle();
        }

        public override double ReadDouble()
        {
            if (Endian == EndianType.BigEndian)
            {
                ReadToBuff(buffer, 0, 8);
                return BinaryPrimitives.ReadDoubleBigEndian(buffer);
            }
            return base.ReadDouble();
        }
#endif
    }
}
