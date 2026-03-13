using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AssetStudio
{
    public static class SpriteExtensions
    {
        public static bool TryGetImage(this Sprite m_Sprite, out Image<Bgra32> spriteImage, SpriteMaskMode spriteMaskMode = SpriteMaskMode.On, bool keepSpriteSize = true)
        {
            return new SpriteConverter(m_Sprite, keepSpriteSize).TryConvertToImage(out spriteImage, spriteMaskMode);
        }
    }
}
