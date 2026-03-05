using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AssetStudio
{
    public static class SpriteExtensions
    {
        public static bool TryGetImage(this Sprite m_Sprite, out Image<Bgra32> spriteImage, SpriteMaskMode spriteMaskMode = SpriteMaskMode.On)
        {
            return new SpriteConverter(m_Sprite).TryConvertToImage(out spriteImage, spriteMaskMode);
        }
    }
}
