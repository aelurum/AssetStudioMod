using System;
using System.Buffers;

namespace AssetStudio
{
    public static class BigArrayPool<T>
    {
        public static ArrayPool<T> Shared { get; }

        static BigArrayPool()
        {
            Shared = ArrayPool<T>.Create(256 * 1024 * 1024, (int)MathF.Max(5, Environment.ProcessorCount / 2f));
        }
    }
}
