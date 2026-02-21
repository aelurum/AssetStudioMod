using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace AssetStudio
{
    public static class SpanExtensions
    {
#if NETFRAMEWORK
        private static readonly byte[] Buff = new byte[4];
#endif

        public static ushort ReadUInt16(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadUInt16((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

        public static ushort ReadUInt16(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadUInt16BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(start));
        }

        public static short ReadInt16(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadInt16((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

        public static short ReadInt16(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadInt16BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadInt16LittleEndian(data.Slice(start));
        }

        public static uint ReadUInt32(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadUInt32((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

        public static uint ReadUInt32(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadUInt32BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(start));
        }

        public static int ReadInt32(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadInt32((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

        public static int ReadInt32(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadInt32BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadInt32LittleEndian(data.Slice(start));
        }

        public static long ReadInt64(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadInt64((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

        public static long ReadInt64(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadInt64BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadInt64LittleEndian(data.Slice(start));
        }

        public static Half ReadHalf(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadHalf((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

        public static float ReadSingle(this Span<byte> data, int start = 0, bool isBigEndian = false)
        {
            return ReadSingle((ReadOnlySpan<byte>)data, start, isBigEndian);
        }

#if NETFRAMEWORK
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadHalf(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? throw new NotImplementedException()
                : HalfHelper.ToHalf((ushort)BinaryPrimitives.ReadInt16LittleEndian(data.Slice(start)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadSingle(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            data = data.Slice(start, 4);
            if ((isBigEndian && BitConverter.IsLittleEndian) || (!isBigEndian && !BitConverter.IsLittleEndian))
                (Buff[0], Buff[1], Buff[2], Buff[3]) = (data[3], data[2], data[1], data[0]);
            else
                (Buff[0], Buff[1], Buff[2], Buff[3]) = (data[0], data[1], data[2], data[3]);

            return BitConverter.ToSingle(Buff, 0);
        }
#else
        public static Half ReadHalf(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadHalfBigEndian(data.Slice(start))
                : BinaryPrimitives.ReadHalfLittleEndian(data.Slice(start));
        }

        public static float ReadSingle(this ReadOnlySpan<byte> data, int start = 0, bool isBigEndian = false)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadSingleBigEndian(data[start..])
                : BinaryPrimitives.ReadSingleLittleEndian(data[start..]);
        }
#endif

        public static string ReadStringToNull(this Span<byte> data, int start = 0, int maxLength = 32767)
        {
            return ReadStringToNull((ReadOnlySpan<byte>)data, start, maxLength);
        }

        public static string ReadStringToNull(this ReadOnlySpan<byte> data, int start = 0, int maxLength = 32767)
        {
            Span<byte> bytes = stackalloc byte[maxLength];
            data = data.Slice(start);
            var count = 0;
            while (count != data.Length && count < maxLength)
            {
                var b = data[count];
                if (b == 0)
                {
                    break;
                }
                bytes[count] = b;
                count++;
            }
            bytes = bytes.Slice(0, count);
#if NETFRAMEWORK
            return Encoding.UTF8.GetString(bytes.ToArray());
#else
            return Encoding.UTF8.GetString(bytes);
#endif
        }
    }
}
