using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AssetStudio
{
    public enum SpriteMaskMode
    {
        Off,
        On,
        Preview,
        MaskOnly,
    }

    public class SpriteConverter
    {
        private Sprite sprite;
        private Rectf textureRect;
        private Vector2 textureRectOffset;
        private float downscaleMultiplier;
        private SpriteSettings settingsRaw;
        private bool keepSpriteSize;
        private int resultWidth;
        private int resultHeight;
        private bool canConvert;

        public readonly Texture2D m_Texture2D;
        public readonly Texture2D m_AlphaTexture2D;
        public int Width => resultWidth;
        public int Height => resultHeight;

        public SpriteConverter(Sprite m_Sprite, bool keepSize = true)
        {
            sprite = m_Sprite;
            keepSpriteSize = keepSize;

            if (m_Sprite.m_SpriteAtlas != null && m_Sprite.m_SpriteAtlas.TryGet(out var m_SpriteAtlas))
            {
                if (m_SpriteAtlas.m_RenderDataMap.TryGetValue(m_Sprite.m_RenderDataKey, out var spriteAtlasData) && spriteAtlasData.texture.TryGet(out m_Texture2D))
                {
                    spriteAtlasData.alphaTexture.TryGet(out m_AlphaTexture2D);
                    textureRect = spriteAtlasData.textureRect;
                    textureRectOffset = spriteAtlasData.textureRectOffset;
                    downscaleMultiplier = spriteAtlasData.downscaleMultiplier;
                    settingsRaw = spriteAtlasData.settingsRaw;
                    SetResultSpriteSize();
                    if (resultWidth > 0 && resultHeight > 0)
                    {
                        canConvert = true;
                    }
                }
            }
            if (!canConvert && m_Sprite.m_RD.texture.TryGet(out m_Texture2D))
            {
                m_Sprite.m_RD.alphaTexture.TryGet(out m_AlphaTexture2D);
                textureRect = m_Sprite.m_RD.textureRect;
                textureRectOffset = m_Sprite.m_RD.textureRectOffset;
                downscaleMultiplier = m_Sprite.m_RD.downscaleMultiplier;
                settingsRaw = m_Sprite.m_RD.settingsRaw;
                SetResultSpriteSize();
                if (resultWidth > 0 && resultHeight > 0)
                {
                    canConvert = true;
                }
            }
        }

        private void SetResultSpriteSize()
        {
            resultWidth = keepSpriteSize
                ? (int)MathF.Round(sprite.m_Rect.width)
                : (int)MathF.Round(textureRect.width);
            resultHeight = keepSpriteSize
                ? (int)MathF.Round(sprite.m_Rect.height)
                : (int)MathF.Round(textureRect.height);
        }

        public bool TryConvertToImage(out Image<Bgra32> spriteImage, SpriteMaskMode maskMode)
        {
            spriteImage = null;
            if (!canConvert)
                return false;

            if (m_AlphaTexture2D != null)
            {
                var alphaTex = CutSprite(m_AlphaTexture2D);
                if (maskMode != SpriteMaskMode.MaskOnly)
                {
                    spriteImage = CutSprite(m_Texture2D);
                }

                switch (maskMode)
                {
                    case SpriteMaskMode.MaskOnly:
                        spriteImage = alphaTex;
                        break;
                    case SpriteMaskMode.Preview when alphaTex != null:
                        spriteImage.ApplyRgbMask(alphaTex, fastResize: true);
                        break;
                    case SpriteMaskMode.On when alphaTex != null:
                        spriteImage.ApplyRgbMask(alphaTex);
                        break;
                }
                return spriteImage != null;
            }

            spriteImage = CutSprite(m_Texture2D);
            return spriteImage != null;
        }

        private Image<Bgra32> CutSprite(Texture2D texture2D)
        {
            var croppedImage = CutImage(texture2D);
            if (croppedImage == null)
                return null;

            if (!keepSpriteSize)
                return croppedImage;

            using (croppedImage)
            {
                var canvas = new Image<Bgra32>(resultWidth, resultHeight);
                canvas.Mutate(ctx => ctx.DrawImage(
                    croppedImage,
                    new Point((int)textureRectOffset.X, (int)(resultHeight - textureRectOffset.Y - croppedImage.Height)),
                    1f)
                );
                
                return canvas;
            }
        }

        private Image<Bgra32> CutImage(Texture2D texture2D)
        {
            if (!texture2D.TryConvertToImage(false, out var textureImage))
                return null;

            if (downscaleMultiplier > 0f && downscaleMultiplier != 1f)
            {
                var width = (int)(texture2D.m_Width / downscaleMultiplier);
                var height = (int)(texture2D.m_Height / downscaleMultiplier);
                textureImage.Mutate(x => x.Resize(width, height));
            }

            var rectX = (int)textureRect.x;
            var rectY = (int)textureRect.y;
            var rectRight = (int)MathF.Ceiling(textureRect.x + textureRect.width);
            var rectBottom = (int)MathF.Ceiling(textureRect.y + textureRect.height);
            rectRight = Math.Min(rectRight, textureImage.Width);
            rectBottom = Math.Min(rectBottom, textureImage.Height);
            var rect = new Rectangle(rectX, rectY, rectRight - rectX, rectBottom - rectY);
            var spriteImage = textureImage.Clone(x => x.Crop(rect));
            textureImage.Dispose();
            if (settingsRaw.packed == 1)
            {
                //RotateAndFlip
                switch (settingsRaw.packingRotation)
                {
                    case SpritePackingRotation.FlipHorizontal:
                        spriteImage.Mutate(x => x.Flip(FlipMode.Horizontal));
                        break;
                    case SpritePackingRotation.FlipVertical:
                        spriteImage.Mutate(x => x.Flip(FlipMode.Vertical));
                        break;
                    case SpritePackingRotation.Rotate180:
                        spriteImage.Mutate(x => x.Rotate(180));
                        break;
                    case SpritePackingRotation.Rotate90:
                        spriteImage.Mutate(x => x.Rotate(270));
                        break;
                }
            }

            //Tight
            if (settingsRaw.packingMode == SpritePackingMode.Tight)
            {
                try
                {
                    var matrix = Matrix3x2.CreateScale(sprite.m_PixelsToUnits);
                    matrix *= Matrix3x2.CreateTranslation(sprite.m_Rect.width * sprite.m_Pivot.X - textureRectOffset.X, sprite.m_Rect.height * sprite.m_Pivot.Y - textureRectOffset.Y);
                    var triangles = GetTriangles(sprite.m_RD);
                    var points = triangles.Select(x => x.Select(y => new PointF(y.X, y.Y)));
                    var pathBuilder = new PathBuilder(matrix);
                    foreach (var p in points)
                    {
                        pathBuilder.AddLines(p);
                        pathBuilder.CloseFigure();
                    }
                    var path = pathBuilder.Build();
                    var options = new DrawingOptions
                    {
                        GraphicsOptions = new GraphicsOptions
                        {
                            Antialias = false,
                            AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
                        }
                    };
                    if (triangles.Length < 1024)
                    {
                        var rectP = new RectangularPolygon(0, 0, rect.Width, rect.Height);
                        try
                        {
                            spriteImage.Mutate(x => x.Fill(options, SixLabors.ImageSharp.Color.Red, rectP.Clip(path.Clip())));
                            spriteImage.Mutate(x => x.Flip(FlipMode.Vertical));
                            return spriteImage;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // ignored
                        }
                    }
                    using (var mask = new Image<Bgra32>(rect.Width, rect.Height, SixLabors.ImageSharp.Color.Black))
                    {
                        mask.Mutate(x => x.Fill(options, SixLabors.ImageSharp.Color.Red, path));
                        var brush = new ImageBrush(mask);
                        spriteImage.Mutate(x => x.Fill(options, brush));
                        spriteImage.Mutate(x => x.Flip(FlipMode.Vertical));
                        return spriteImage;
                    }
                }
                catch (Exception e)
                {
                    Logger.Warning($"{sprite.m_Name} Unable to render the packed sprite correctly.\n{e}");
                }
            }

            //Rectangle
            spriteImage.Mutate(x => x.Flip(FlipMode.Vertical));
            return spriteImage;
        }

        private static Vector2[][] GetTriangles(SpriteRenderData m_RD)
        {
            if (m_RD.vertices != null) //5.6 down
            {
                var vertices = m_RD.vertices.Select(x => (Vector2)x.pos).ToArray();
                var triangleCount = m_RD.indices.Length / 3;
                var triangles = new Vector2[triangleCount][];
                for (int i = 0; i < triangleCount; i++)
                {
                    var first = m_RD.indices[i * 3];
                    var second = m_RD.indices[i * 3 + 1];
                    var third = m_RD.indices[i * 3 + 2];
                    var triangle = new[] { vertices[first], vertices[second], vertices[third] };
                    triangles[i] = triangle;
                }
                return triangles;
            }
            else //5.6 and up
            {
                var triangles = new List<Vector2[]>();
                var m_VertexData = m_RD.m_VertexData;
                var m_Channel = m_VertexData.m_Channels[0]; //kShaderChannelVertex
                var m_Stream = m_VertexData.m_Streams[m_Channel.stream];
                using (var vertexReader = new EndianBinaryReader(new MemoryStream(m_VertexData.m_DataSize), EndianType.LittleEndian))
                {
                    using (var indexReader = new EndianBinaryReader(new MemoryStream(m_RD.m_IndexBuffer), EndianType.LittleEndian))
                    {
                        foreach (var subMesh in m_RD.m_SubMeshes)
                        {
                            vertexReader.BaseStream.Position = m_Stream.offset + subMesh.firstVertex * m_Stream.stride + m_Channel.offset;

                            var vertices = new Vector2[subMesh.vertexCount];
                            for (int v = 0; v < subMesh.vertexCount; v++)
                            {
                                vertices[v] = vertexReader.ReadVector3();
                                vertexReader.BaseStream.Position += m_Stream.stride - 12;
                            }

                            indexReader.BaseStream.Position = subMesh.firstByte;

                            var triangleCount = subMesh.indexCount / 3u;
                            for (int i = 0; i < triangleCount; i++)
                            {
                                var first = indexReader.ReadUInt16() - subMesh.firstVertex;
                                var second = indexReader.ReadUInt16() - subMesh.firstVertex;
                                var third = indexReader.ReadUInt16() - subMesh.firstVertex;
                                var triangle = new[] { vertices[first], vertices[second], vertices[third] };
                                triangles.Add(triangle);
                            }
                        }
                    }
                }
                return triangles.ToArray();
            }
        }

        public ulong GetSpriteHash()
        {
            ulong spriteHash = 0;
            if (m_Texture2D == null)
                return spriteHash;

            spriteHash = m_Texture2D.GetImageDataHash();
            
            if (m_AlphaTexture2D != null)
            {
                spriteHash ^= m_AlphaTexture2D.GetImageDataHash();
            }

            return (m_Texture2D.m_Width, m_Texture2D.m_Height) == (resultWidth, resultHeight)
                ? spriteHash
                : spriteHash ^ GetRectHash();
        }

        private ulong GetRectHash()
        {
            Span<byte> buff = stackalloc byte[40];
            BinaryPrimitives.WriteInt32LittleEndian(buff, (int)textureRect.x);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(4), (int)textureRect.y);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(8), (int)textureRect.width);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(12), (int)textureRect.height);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(16), (int)textureRectOffset.X);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(20), (int)textureRectOffset.Y);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(24), (int)sprite.m_Rect.x);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(28), (int)sprite.m_Rect.y);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(32), (int)sprite.m_Rect.width);
            BinaryPrimitives.WriteInt32LittleEndian(buff.Slice(36), (int)sprite.m_Rect.height);

            return System.IO.Hashing.XxHash3.HashToUInt64(buff);
        }
    }
}
