using AssetStudio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AssetStudioGUI
{
    public sealed class DirectBitmap : IDisposable
    {
        public DirectBitmap(Image<Bgra32> image)
        {
            Width = image.Width;
            Height = image.Height;
            Bits = BigArrayPool<byte>.Shared.Rent(Width * Height * 4);
            image.CopyPixelDataTo(Bits);
            m_handle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            m_bitmap = new Bitmap(Width, Height, Stride, PixelFormat.Format32bppArgb, m_handle.AddrOfPinnedObject());
        }

        public DirectBitmap(Texture2D texture2D, bool flip)
        {
            Width = texture2D.m_Width;
            Height = texture2D.m_Height;
            var converter = new Texture2DConverter(texture2D);
            var dataSize = converter.GetImageDataSize();
            if (dataSize <= 0)
                return;

            Bits = BigArrayPool<byte>.Shared.Rent(dataSize);
            if (converter.GetBgra32RawImageData(Bits, flip))
            {
                m_handle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
                m_bitmap = new Bitmap(Width, Height, Stride, PixelFormat.Format32bppArgb, m_handle.AddrOfPinnedObject());
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_bitmap != null)
                {
                    m_bitmap.Dispose();
                    m_handle.Free();
                }
                if (Bits != null)
                {
                    BigArrayPool<byte>.Shared.Return(Bits);
                    Bits = null;
                }
            }
            m_bitmap = null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public int Height { get; }
        public int Width { get; }
        public int Stride => Width * 4;
        public byte[] Bits { get; private set; }
        public Bitmap Bitmap => m_bitmap;

        private Bitmap m_bitmap;
        private GCHandle m_handle;
    }
}
