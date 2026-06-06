using System;
using System.Runtime.InteropServices;
#if NETFRAMEWORK
using AssetStudio.PInvoke;
#endif

namespace AssetStudio.Libs.Oodle
{
    public static class OodleLZ
    {
        private const string LibName = "ooz";

#if NETFRAMEWORK
        static OodleLZ()
        {
            DllLoader.PreloadDll(LibName);
        }
#endif

        [DllImport(LibName)]
        private static extern int Ooz_Decompress(
            in byte srcBuffer,
            UIntPtr srcLen,
            ref byte dstBuffer,
            UIntPtr dstLen,
            int fuzzSafetyFlag,
            int crcCheckFlag,
            int logVerbosityFlag,
            UIntPtr rawBuffer,
            UIntPtr rawBufferSize,
            UIntPtr chunkDecodeCallback,
            UIntPtr chunkDecodeContext,
            UIntPtr scratchBuf,
            UIntPtr scratchBufSize,
            int threadPhase);

        public static int Decompress(ReadOnlySpan<byte> srcSpanBuffer, Span<byte> dstSpanBuffer)
        {
            return Ooz_Decompress(in srcSpanBuffer[0], (UIntPtr)srcSpanBuffer.Length, ref dstSpanBuffer[0], (UIntPtr)dstSpanBuffer.Length,
                0, 0, 0, UIntPtr.Zero, UIntPtr.Zero, UIntPtr.Zero, UIntPtr.Zero, UIntPtr.Zero, UIntPtr.Zero, 0);
        }
    }
}
