using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AssetStudio
{
    public static class BinaryReaderExtensions
    {
        public static void AlignStream(this BinaryReader reader, int alignment = 4)
        {
            var pos = reader.BaseStream.Position;
            var mod = pos % alignment;
            if (mod != 0)
            {
                reader.BaseStream.Position += alignment - mod;
            }
        }

        public static string ReadAlignedString(this BinaryReader reader)
        {
            var length = reader.ReadInt32();
            if (length > reader.BaseStream.Length - reader.BaseStream.Position)
                throw new EndOfStreamException();
            if (length > 0)
            {
                var stringData = reader.ReadBytes(length);
                var result = Encoding.UTF8.GetString(stringData);
                reader.AlignStream();
                return result;
            }
            return "";
        }

        public static string ReadStringToNull(this BinaryReader reader, int maxLength = 32767, Encoding encoding = null)
        {
            if (encoding?.CodePage == 1200) //Unicode (UTF-16LE)
                return reader.ReadUnicodeStringToNull(maxLength * 2);

            Span<byte> bytes = stackalloc byte[maxLength];
            var count = 0;
            while (reader.BaseStream.Position != reader.BaseStream.Length && count < maxLength)
            {
                var b = reader.ReadByte();
                if (b == 0)
                {
                    break;
                }
                bytes[count] = b;
                count++;
            }
            bytes = bytes.Slice(0, count);
#if NETFRAMEWORK
            return encoding?.GetString(bytes.ToArray()) ?? Encoding.UTF8.GetString(bytes.ToArray());
#else
            return encoding?.GetString(bytes) ?? Encoding.UTF8.GetString(bytes);
#endif
        }

        private static string ReadUnicodeStringToNull(this BinaryReader reader, int maxLength)
        {
            var bytes = new List<byte>();
            var count = 0;
            while (reader.BaseStream.Position != reader.BaseStream.Length && count < maxLength)
            {
                var b = reader.ReadBytes(2);
                if (b.Length < 2 || (b[0] == 0 && b[1] == 0))
                {
                    break;
                }
                bytes.AddRange(b);
                count += 2;
            }
            return Encoding.Unicode.GetString(bytes.ToArray());
        }

        public static string ReadBytesAsHexString(this BinaryReader reader, int length)
        {
#if NETFRAMEWORK
            var bytes = reader.ReadBytes(length);
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
#else
            Span<byte> bytes = length < 256 ? stackalloc byte[length] : new byte[length];
            _ = reader.Read(bytes);
            return Convert.ToHexString(bytes);
#endif
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Color ReadColor4(this BinaryReader reader)
        {
            return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        private static T[] ReadArray<T>(BinaryReader reader, int byteLen) where T : struct
        {
            if (byteLen < 0)
                throw new ArgumentOutOfRangeException(nameof(byteLen));
            if (reader.BaseStream.Position + byteLen > reader.BaseStream.Length)
                throw new EndOfStreamException();
            
            var bytes = reader.ReadBytes(byteLen);

            var span = MemoryMarshal.Cast<byte, T>(bytes);
            return span.ToArray();
        }

        public static bool[] ReadBooleanArray(this BinaryReader reader)
        {
            return ReadArray<bool>(reader, reader.ReadInt32());
        }

        public static byte[] ReadUInt8Array(this BinaryReader reader)
        {
            return reader.ReadBytes(reader.ReadInt32());
        }

        public static ushort[] ReadUInt16Array(this BinaryReader reader)
        {
            return ReadArray<ushort>(reader, reader.ReadInt32() * 2);
        }

        public static int[] ReadInt32Array(this BinaryReader reader, int length = -1)
        {
            if (length == -1)
                length = reader.ReadInt32();
            return ReadArray<int>(reader, length * 4);
        }

        public static uint[] ReadUInt32Array(this BinaryReader reader, int length = -1)
        {
            if (length == -1)
                length = reader.ReadInt32();
            return ReadArray<uint>(reader, length * 4);
        }

        public static uint[][] ReadUInt32ArrayArray(this BinaryReader reader)
        {
            var length = reader.ReadInt32();
            var list = new List<uint[]>();
            for (var i = 0; i < length; i++)
            {
                list.Add(ReadArray<uint>(reader, reader.ReadInt32() * 4));
            }
            return list.ToArray();
        }

        public static float[] ReadSingleArray(this BinaryReader reader, int length = -1)
        {
            if (length == -1)
                length = reader.ReadInt32();
            return ReadArray<float>(reader, length * 4);
        }

        public static string[] ReadStringArray(this BinaryReader reader)
        {
            var length = reader.ReadInt32();
            var list = new List<string>();
            for (var i = 0; i < length; i++)
            {
                list.Add(reader.ReadAlignedString());
            }
            return list.ToArray();
        }

        public static Vector2[] ReadVector2Array(this BinaryReader reader)
        {
            return ReadArray<Vector2>(reader, reader.ReadInt32() * 8);
        }

        public static Vector4[] ReadVector4Array(this BinaryReader reader)
        {
            return ReadArray<Vector4>(reader, reader.ReadInt32() * 16);
        }

        public static Matrix4x4[] ReadMatrixArray(this BinaryReader reader)
        {
            return ReadArray<Matrix4x4>(reader, reader.ReadInt32() * 64);
        }
    }
}
