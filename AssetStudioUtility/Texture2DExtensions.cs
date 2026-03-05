using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace AssetStudio
{
    public static class Texture2DExtensions
    {
        public static bool TryConvertToImage(this Texture2D m_Texture2D, bool flip, out Image<Bgra32> image)
        {
            return new Texture2DConverter(m_Texture2D).TryConvertToBgra32Image(flip, out image);
        }

        public static MemoryStream ConvertToStream(this Texture2D m_Texture2D, ImageFormat imageFormat, bool flip)
        {
            var converter = new Texture2DConverter(m_Texture2D);
            if (!converter.TryConvertToBgra32Image(flip, out var image))
                return null;

            using (image)
            {
                return image.ConvertToStream(imageFormat);
            }
        }
    }
}
