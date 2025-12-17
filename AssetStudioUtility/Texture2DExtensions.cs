using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AssetStudio
{
    public static class Texture2DExtensions
    {
        public static bool TryConvertToImage(this Texture2D m_Texture2D, bool flip, out Image<Bgra32> image)
        {
            image = null;
            var converter = new Texture2DConverter(m_Texture2D);
            var buff = BigArrayPool<byte>.Shared.Rent(converter.OutputDataSize);
            var spanBuff = buff.AsSpan(0, converter.OutputDataSize);
            try
            {
                if (!converter.DecodeTexture2D(buff)) 
                    return false;

                image = Image.LoadPixelData<Bgra32>(spanBuff, m_Texture2D.m_Width, m_Texture2D.m_Height);

                if (flip)
                {
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                return true;
            }
            finally
            {
                BigArrayPool<byte>.Shared.Return(buff, clearArray: true);
            }
        }

        public static MemoryStream ConvertToStream(this Texture2D m_Texture2D, ImageFormat imageFormat, bool flip)
        {
            if (!TryConvertToImage(m_Texture2D, flip, out var image)) 
                return null;

            using (image)
            {
                return image.ConvertToStream(imageFormat);
            }
        }
    }
}
