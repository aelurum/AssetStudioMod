using AssetStudio.Libs.Lzma;
using AssetStudio.Libs.Oodle;
using K4os.Compression.LZ4;
using System;
using System.IO;
using System.Text.RegularExpressions;
using ZstdSharp;

namespace AssetStudio
{
    public static class BundleDecompressionHelper
    {
        private static readonly Decompressor ZstdDecompressor = new Decompressor();
        private static readonly string MsgPattern = @"\. ";

        public static MemoryStream DecompressLzmaStream(MemoryStream inStream)
        {
            return SevenZipLzma.DecompressStream(inStream);
        }

        public static long DecompressLzmaStream(Stream compressedStream, Stream decompressedStream, long compressedSize, long decompressedSize, ref string errorMsg)
        {
            var numWrite = -1L;
            try
            {
                numWrite = SevenZipLzma.DecompressStream(compressedStream, decompressedStream, compressedSize, decompressedSize);
            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());
                errorMsg = $"({Regex.Split(e.Message, MsgPattern, RegexOptions.CultureInvariant)[0]})";
            }
            return numWrite;
        }

        public static int DecompressBlock(CompressionType type, ReadOnlySpan<byte> srcBuffer, Span<byte> dstBuffer, ref string errorMsg)
        {
            var numWrite = -1;
            try
            {
                switch (type)
                {
                    case CompressionType.Lz4:
                    case CompressionType.Lz4HC:
                        numWrite = LZ4Codec.Decode(srcBuffer, dstBuffer);
                        break;
                    case CompressionType.Zstd:
                        numWrite = ZstdDecompressor.Unwrap(srcBuffer, dstBuffer);
                        break;
                    case CompressionType.Oodle:
                        numWrite = OodleLZ.Decompress(srcBuffer, dstBuffer);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());
                errorMsg = $"({Regex.Split(e.Message, MsgPattern, RegexOptions.CultureInvariant)[0]})";
            }
            return numWrite;
        }
    }
}
