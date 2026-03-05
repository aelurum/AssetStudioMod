using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace AssetStudio
{
    public static class ImageExtensions
    {
        public static void WriteToStream(this Image image, Stream stream, ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(stream);
                    break;
                case ImageFormat.Png:
                    image.SaveAsPng(stream);
                    break;
                case ImageFormat.Bmp:
                    image.Save(stream, new BmpEncoder
                    {
                        BitsPerPixel = BmpBitsPerPixel.Pixel32,
                        SupportTransparency = true
                    });
                    break;
                case ImageFormat.Tga:
                    image.Save(stream, new TgaEncoder
                    {
                        BitsPerPixel = TgaBitsPerPixel.Pixel32,
                        Compression = TgaCompression.None
                    });
                    break;
                case ImageFormat.Webp:
                    image.Save(stream, new WebpEncoder
                    {
                        FileFormat = WebpFileFormatType.Lossless,
                        Quality = 50
                    });
                    break;
            }
        }

        public static MemoryStream ConvertToStream(this Image image, ImageFormat imageFormat)
        {
            var stream = new MemoryStream();
            image.WriteToStream(stream, imageFormat);
            return stream;
        }

        public static void ApplyRgbMask(this Image<Bgra32> srcTex, Image<Bgra32> texMask, bool fastResize = false)
        {
            using (texMask)
            {
                if (srcTex.Width != texMask.Width || srcTex.Height != texMask.Height)
                {
                    var resampler = fastResize ? KnownResamplers.NearestNeighbor : KnownResamplers.Bicubic;
                    texMask.Mutate(x => x.Resize(srcTex.Width, srcTex.Height, resampler));
                }

                srcTex.ProcessPixelRows(texMask, (sourceTex, targetTexMask) =>
                {
                    for (var y = 0; y < texMask.Height; y++)
                    {
                        var texRow = sourceTex.GetRowSpan(y);
                        var maskRow = targetTexMask.GetRowSpan(y);
                        for (var x = 0; x < maskRow.Length; x++)
                        {
                            var grayscale = (byte)((maskRow[x].R + maskRow[x].G + maskRow[x].B) / 3);
                            texRow[x].A = grayscale;
                        }
                    }
                });
            }
        }
    }
}
