using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using Texture2DDecoder;

namespace AssetStudio
{
    public class Texture2DConverter
    {
        private Texture2D m_Texture2D;
        private int m_Width;
        private int m_Height;
        private TextureFormat m_TextureFormat;
        private int texDataSize;
        private int outputDataSize;
        private bool canConvert;

        private bool switchSwizzled;
        private int gobsPerBlock;
        private Size blockSize;

        public int OutputDataSize => outputDataSize;

        public Texture2DConverter(Texture2D m_Texture2D)
        {
            this.m_Texture2D = m_Texture2D;
            m_Width = m_Texture2D.m_Width;
            m_Height = m_Texture2D.m_Height;
            texDataSize = m_Texture2D.image_data.Size;
            if (texDataSize == 0 || m_Width == 0 || m_Height == 0)
                return;

            m_TextureFormat = m_Texture2D.m_TextureFormat;
            // not guaranteed, you can have a swizzled texture without m_PlatformBlob
            // but officially, I don't think this can happen. maybe check which engine
            // version this started happening in...
            switchSwizzled = m_Texture2D.platform == BuildTarget.Switch && m_Texture2D.m_PlatformBlob.Length != 0;
            if (switchSwizzled)
            {
                SetupSwitchSwizzle();
            }
            outputDataSize = m_Width * m_Height * 4;

            canConvert = true;
        }

        private void SetupSwitchSwizzle()
        {
            //apparently there is another value to worry about, but seeing as it's
            //always 0 and I have nothing else to test against, this will probably
            //work fine for now
            gobsPerBlock = 1 << BitConverter.ToInt32(m_Texture2D.m_PlatformBlob, 8);

            switch (m_TextureFormat)
            {
                //in older versions of unity, rgb24 has a platformBlob which shouldn't
                //be possible. it turns out in this case, the image is just rgba32.
                //probably shouldn't be modifying the texture2d here, but eh, who cares
                case TextureFormat.RGB24:
                    m_TextureFormat = TextureFormat.RGBA32;
                    break;
                case TextureFormat.BGR24:
                    m_TextureFormat = TextureFormat.BGRA32;
                    break;
            }

            blockSize = Texture2DSwitchDeswizzler.GetTextureFormatBlockSize(m_TextureFormat);
        }

        public ulong GetTextureHash() => canConvert
            ? m_Texture2D.GetImageDataHash()
            : 0;

        public bool GetBgra32RawImageData(byte[] destArray, bool flip)
        {
            if (!canConvert)
                return false;

            var dataSpan = destArray.AsSpan(0, outputDataSize);
            if (!DecodeTexture2D(dataSpan)) 
                return false;

            if (flip)
            {
                var stride = m_Width * 4;
                Span<byte> tmp = new byte[stride];
                for (var i = 0; i < m_Height / 2; i++) //flip
                {
                    var j = m_Height - i - 1;
                    var srcOffset = i * stride;
                    var dstOffset = j * stride;
                    dataSpan.Slice(dstOffset, stride).CopyTo(tmp);
                    dataSpan.Slice(srcOffset, stride).CopyTo(dataSpan.Slice(dstOffset));
                    tmp.CopyTo(dataSpan.Slice(srcOffset));
                }
            }

            return true;
        }

        public bool TryConvertToBgra32Image(bool flip, out Image<Bgra32> image)
        {
            image = null;
            if (!canConvert)
                return false;

            var imageBuff = BigArrayPool<byte>.Shared.Rent(outputDataSize);
            var imageBuffSpan = imageBuff.AsSpan(0, outputDataSize);
            try
            {
                if (!DecodeTexture2D(imageBuffSpan))
                    return false;

                image = Image.LoadPixelData<Bgra32>(imageBuffSpan, m_Width, m_Height);

                if (flip)
                {
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                return true;
            }
            finally
            {
                BigArrayPool<byte>.Shared.Return(imageBuff, clearArray: true);
            }
        }

        private bool DecodeTexture2D(Span<byte> destSpan)
        {
            if (!canConvert)
                return false;

            var flag = false;
            var buff = BigArrayPool<byte>.Shared.Rent(texDataSize);
            try
            {
                _ = m_Texture2D.image_data.GetData(buff);

                if (switchSwizzled)
                {
                    var spanBuff = buff.AsSpan(0, texDataSize);
                    var textureSize = new Size(m_Width, m_Height);
                    Texture2DSwitchDeswizzler.Unswizzle(ref spanBuff, textureSize, blockSize, gobsPerBlock);
                }

                switch (m_TextureFormat)
                {
                    case TextureFormat.Alpha8: //test pass
                        flag = DecodeAlpha8(buff, destSpan);
                        break;
                    case TextureFormat.ARGB4444: //test pass
                        SwapBytesForXbox(buff);
                        flag = DecodeARGB4444(buff, destSpan);
                        break;
                    case TextureFormat.RGB24: //test pass
                        flag = DecodeRGB24(buff, destSpan);
                        break;
                    case TextureFormat.RGBA32: //test pass
                        flag = DecodeRGBA32(buff, destSpan);
                        break;
                    case TextureFormat.ARGB32: //test pass
                        flag = DecodeARGB32(buff, destSpan);
                        break;
                    case TextureFormat.RGB565: //test pass
                        SwapBytesForXbox(buff);
                        flag = DecodeRGB565(buff, destSpan);
                        break;
                    case TextureFormat.R16: //test pass
                        flag = DecodeR16(buff, destSpan);
                        break;
                    case TextureFormat.DXT1: //test pass
                        SwapBytesForXbox(buff);
                        flag = DecodeDXT1(buff, destSpan);
                        break;
                    case TextureFormat.DXT3:
                        break;
                    case TextureFormat.DXT5: //test pass
                        SwapBytesForXbox(buff);
                        flag = DecodeDXT5(buff, destSpan);
                        break;
                    case TextureFormat.RGBA4444: //test pass
                        flag = DecodeRGBA4444(buff, destSpan);
                        break;
                    case TextureFormat.BGRA32: //test pass
                        flag = DecodeBGRA32(buff, destSpan);
                        break;
                    case TextureFormat.RHalf:
                        flag = DecodeRHalf(buff, destSpan);
                        break;
                    case TextureFormat.RGHalf:
                        flag = DecodeRGHalf(buff, destSpan);
                        break;
                    case TextureFormat.RGBAHalf: //test pass
                        flag = DecodeRGBAHalf(buff, destSpan);
                        break;
                    case TextureFormat.RFloat:
                        flag = DecodeRFloat(buff, destSpan);
                        break;
                    case TextureFormat.RGFloat:
                        flag = DecodeRGFloat(buff, destSpan);
                        break;
                    case TextureFormat.RGBAFloat:
                        flag = DecodeRGBAFloat(buff, destSpan);
                        break;
                    case TextureFormat.YUY2: //test pass
                        flag = DecodeYUY2(buff, destSpan);
                        break;
                    case TextureFormat.RGB9e5Float: //test pass
                        flag = DecodeRGB9e5Float(buff, destSpan);
                        break;
                    case TextureFormat.BC6H: //test pass
                        flag = DecodeBC6H(buff, destSpan);
                        break;
                    case TextureFormat.BC7: //test pass
                        flag = DecodeBC7(buff, destSpan);
                        break;
                    case TextureFormat.BC4: //test pass
                        flag = DecodeBC4(buff, destSpan);
                        break;
                    case TextureFormat.BC5: //test pass
                        flag = DecodeBC5(buff, destSpan);
                        break;
                    case TextureFormat.DXT1Crunched: //test pass
                        flag = DecodeDXT1Crunched(buff, destSpan);
                        break;
                    case TextureFormat.DXT5Crunched: //test pass
                        flag = DecodeDXT5Crunched(buff, destSpan);
                        break;
                    case TextureFormat.PVRTC_RGB2: //test pass
                    case TextureFormat.PVRTC_RGBA2: //test pass
                        flag = DecodePVRTC(buff, destSpan, true);
                        break;
                    case TextureFormat.PVRTC_RGB4: //test pass
                    case TextureFormat.PVRTC_RGBA4: //test pass
                        flag = DecodePVRTC(buff, destSpan, false);
                        break;
                    case TextureFormat.ETC_RGB4: //test pass
                    case TextureFormat.ETC_RGB4_3DS:
                        flag = DecodeETC1(buff, destSpan);
                        break;
                    case TextureFormat.ATC_RGB4: //test pass
                        flag = DecodeATCRGB4(buff, destSpan);
                        break;
                    case TextureFormat.ATC_RGBA8: //test pass
                        flag = DecodeATCRGBA8(buff, destSpan);
                        break;
                    case TextureFormat.EAC_R: //test pass
                        flag = DecodeEACR(buff, destSpan);
                        break;
                    case TextureFormat.EAC_R_SIGNED:
                        flag = DecodeEACRSigned(buff, destSpan);
                        break;
                    case TextureFormat.EAC_RG: //test pass
                        flag = DecodeEACRG(buff, destSpan);
                        break;
                    case TextureFormat.EAC_RG_SIGNED:
                        flag = DecodeEACRGSigned(buff, destSpan);
                        break;
                    case TextureFormat.ETC2_RGB: //test pass
                        flag = DecodeETC2(buff, destSpan);
                        break;
                    case TextureFormat.ETC2_RGBA1: //test pass
                        flag = DecodeETC2A1(buff, destSpan);
                        break;
                    case TextureFormat.ETC2_RGBA8: //test pass
                    case TextureFormat.ETC_RGBA8_3DS:
                        flag = DecodeETC2A8(buff, destSpan);
                        break;
                    case TextureFormat.ASTC_RGB_4x4: //test pass
                    case TextureFormat.ASTC_RGBA_4x4: //test pass
                    case TextureFormat.ASTC_HDR_4x4: //test pass
                        flag = DecodeASTC(buff, destSpan, 4);
                        break;
                    case TextureFormat.ASTC_RGB_5x5: //test pass
                    case TextureFormat.ASTC_RGBA_5x5: //test pass
                    case TextureFormat.ASTC_HDR_5x5: //test pass
                        flag = DecodeASTC(buff, destSpan, 5);
                        break;
                    case TextureFormat.ASTC_RGB_6x6: //test pass
                    case TextureFormat.ASTC_RGBA_6x6: //test pass
                    case TextureFormat.ASTC_HDR_6x6: //test pass
                        flag = DecodeASTC(buff, destSpan, 6);
                        break;
                    case TextureFormat.ASTC_RGB_8x8: //test pass
                    case TextureFormat.ASTC_RGBA_8x8: //test pass
                    case TextureFormat.ASTC_HDR_8x8: //test pass
                        flag = DecodeASTC(buff, destSpan, 8);
                        break;
                    case TextureFormat.ASTC_RGB_10x10: //test pass
                    case TextureFormat.ASTC_RGBA_10x10: //test pass
                    case TextureFormat.ASTC_HDR_10x10: //test pass
                        flag = DecodeASTC(buff, destSpan, 10);
                        break;
                    case TextureFormat.ASTC_RGB_12x12: //test pass
                    case TextureFormat.ASTC_RGBA_12x12: //test pass
                    case TextureFormat.ASTC_HDR_12x12: //test pass
                        flag = DecodeASTC(buff, destSpan, 12);
                        break;
                    case TextureFormat.RG16: //test pass
                        flag = DecodeRG16(buff, destSpan);
                        break;
                    case TextureFormat.R8: //test pass
                        flag = DecodeR8(buff, destSpan);
                        break;
                    case TextureFormat.ETC_RGB4Crunched: //test pass
                        flag = DecodeETC1Crunched(buff, destSpan);
                        break;
                    case TextureFormat.ETC2_RGBA8Crunched: //test pass
                        flag = DecodeETC2A8Crunched(buff, destSpan);
                        break;
                    case TextureFormat.RG32: //test pass
                        flag = DecodeRG32(buff, destSpan);
                        break;
                    case TextureFormat.RGB48: //test pass
                        flag = DecodeRGB48(buff, destSpan);
                        break;
                    case TextureFormat.RGBA64: //test pass
                        flag = DecodeRGBA64(buff, destSpan);
                        break;
                }
            }
            finally
            {
                BigArrayPool<byte>.Shared.Return(buff, clearArray: true);
            }

            return flag;
        }

        private void SwapBytesForXbox(Span<byte> buff)
        {
            if (m_Texture2D.platform == BuildTarget.XBOX360)
            {
                for (var i = 0; i < texDataSize / 2; i++)
                {
                    buff.Slice(i * 2, 2).Reverse();
                }
            }
        }

        private bool DecodeAlpha8(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            destSpan.Fill(0xFF);
            for (var i = 0; i < size; i++)
            {
                destSpan[i * 4 + 3] = buff[i]; //A
            }
            return true;
        }

        private bool DecodeARGB4444(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            var pixelNew = new byte[4].AsSpan();
            for (var i = 0; i < size; i++)
            {
                var pixelOldShort = BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2));
                pixelNew[0] = (byte)(pixelOldShort & 0x000f);
                pixelNew[1] = (byte)((pixelOldShort & 0x00f0) >> 4);
                pixelNew[2] = (byte)((pixelOldShort & 0x0f00) >> 8);
                pixelNew[3] = (byte)((pixelOldShort & 0xf000) >> 12);
                for (var j = 0; j < 4; j++)
                    pixelNew[j] = (byte)((pixelNew[j] << 4) | pixelNew[j]);
                pixelNew.CopyTo(destSpan.Slice(i * 4));
            }
            return true;
        }

        private bool DecodeRGB24(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            for (var i = 0; i < size; i++)
            {
                destSpan[i * 4] = buff[i * 3 + 2];
                destSpan[i * 4 + 1] = buff[i * 3 + 1];
                destSpan[i * 4 + 2] = buff[i * 3 + 0];
                destSpan[i * 4 + 3] = 255;
            }
            return true;
        }

        private bool DecodeRGBA32(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = buff[i + 2];
                destSpan[i + 1] = buff[i + 1];
                destSpan[i + 2] = buff[i + 0];
                destSpan[i + 3] = buff[i + 3];
            }
            return true;
        }

        private bool DecodeARGB32(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = buff[i + 3];
                destSpan[i + 1] = buff[i + 2];
                destSpan[i + 2] = buff[i + 1];
                destSpan[i + 3] = buff[i + 0];
            }
            return true;
        }

        private bool DecodeRGB565(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            for (var i = 0; i < size; i++)
            {
                var p = BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2));
                destSpan[i * 4] = (byte)((p << 3) | (p >> 2 & 7));
                destSpan[i * 4 + 1] = (byte)((p >> 3 & 0xfc) | (p >> 9 & 3));
                destSpan[i * 4 + 2] = (byte)((p >> 8 & 0xf8) | (p >> 13));
                destSpan[i * 4 + 3] = 255;
            }
            return true;
        }

        private bool DecodeR16(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            for (var i = 0; i < size; i++)
            {
                destSpan[i * 4] = 0; //B
                destSpan[i * 4 + 1] = 0; //G
                destSpan[i * 4 + 2] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2))); //R
                destSpan[i * 4 + 3] = 255; //A
            }
            return true;
        }

        private bool DecodeDXT1(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeDXT1(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeDXT5(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeDXT5(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeRGBA4444(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            var pixelNew = new byte[4].AsSpan();
            for (var i = 0; i < size; i++)
            {
                var pixelOldShort = BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2));
                pixelNew[0] = (byte)((pixelOldShort & 0x00f0) >> 4);
                pixelNew[1] = (byte)((pixelOldShort & 0x0f00) >> 8);
                pixelNew[2] = (byte)((pixelOldShort & 0xf000) >> 12);
                pixelNew[3] = (byte)(pixelOldShort & 0x000f);
                for (var j = 0; j < 4; j++)
                    pixelNew[j] = (byte)((pixelNew[j] << 4) | pixelNew[j]);
                pixelNew.CopyTo(destSpan.Slice(i * 4));
            }
            return true;
        }

        private bool DecodeBGRA32(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            buff.Slice(0, outputDataSize).CopyTo(destSpan);
            return true;
        }

        private bool DecodeRHalf(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = 0;
                destSpan[i + 1] = 0;
                destSpan[i + 2] = (byte)MathF.Round((float)buff.ReadHalf(i / 2) * 255f);
                destSpan[i + 3] = 255;
            }
            return true;
        }

        private bool DecodeRGHalf(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = 0;
                destSpan[i + 1] = (byte)MathF.Round((float)buff.ReadHalf(i + 2) * 255f);
                destSpan[i + 2] = (byte)MathF.Round((float)buff.ReadHalf(i) * 255f);
                destSpan[i + 3] = 255;
            }
            return true;
        }

        private bool DecodeRGBAHalf(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = (byte)MathF.Round((float)buff.ReadHalf(i * 2 + 4) * 255f);
                destSpan[i + 1] = (byte)MathF.Round((float)buff.ReadHalf(i * 2 + 2) * 255f);
                destSpan[i + 2] = (byte)MathF.Round((float)buff.ReadHalf(i * 2) * 255f);
                destSpan[i + 3] = (byte)MathF.Round((float)buff.ReadHalf(i * 2 + 6) * 255f);
            }
            return true;
        }

        private bool DecodeRFloat(byte[] buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = 0;
                destSpan[i + 1] = 0;
                destSpan[i + 2] = (byte)MathF.Round(BitConverter.ToSingle(buff, i) * 255f);
                destSpan[i + 3] = 255;
            }
            return true;
        }

        private bool DecodeRGFloat(byte[] buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = 0;
                destSpan[i + 1] = (byte)MathF.Round(BitConverter.ToSingle(buff, i * 2 + 4) * 255f);
                destSpan[i + 2] = (byte)MathF.Round(BitConverter.ToSingle(buff, i * 2) * 255f);
                destSpan[i + 3] = 255;
            }
            return true;
        }

        private bool DecodeRGBAFloat(byte[] buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = (byte)MathF.Round(BitConverter.ToSingle(buff, i * 4 + 8) * 255f);
                destSpan[i + 1] = (byte)MathF.Round(BitConverter.ToSingle(buff, i * 4 + 4) * 255f);
                destSpan[i + 2] = (byte)MathF.Round(BitConverter.ToSingle(buff, i * 4) * 255f);
                destSpan[i + 3] = (byte)MathF.Round(BitConverter.ToSingle(buff, i * 4 + 12) * 255f);
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ClampByte(int x)
        {
            return (byte)(byte.MaxValue < x ? byte.MaxValue : (x > byte.MinValue ? x : byte.MinValue));
        }

        private bool DecodeYUY2(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            int p = 0;
            int o = 0;
            int halfWidth = m_Width / 2;
            for (int j = 0; j < m_Height; j++)
            {
                for (int i = 0; i < halfWidth; ++i)
                {
                    int y0 = buff[p++];
                    int u0 = buff[p++];
                    int y1 = buff[p++];
                    int v0 = buff[p++];
                    int c = y0 - 16;
                    int d = u0 - 128;
                    int e = v0 - 128;
                    destSpan[o++] = ClampByte((298 * c + 516 * d + 128) >> 8); //B
                    destSpan[o++] = ClampByte((298 * c - 100 * d - 208 * e + 128) >> 8); //G
                    destSpan[o++] = ClampByte((298 * c + 409 * e + 128) >> 8); //R
                    destSpan[o++] = 255; //A 
                    c = y1 - 16;
                    destSpan[o++] = ClampByte((298 * c + 516 * d + 128) >> 8); //B
                    destSpan[o++] = ClampByte((298 * c - 100 * d - 208 * e + 128) >> 8); //G
                    destSpan[o++] = ClampByte((298 * c + 409 * e + 128) >> 8); //R
                    destSpan[o++] = 255; //A
                }
            }
            return true;
        }

        private bool DecodeRGB9e5Float(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                var n = BinaryPrimitives.ReadInt32LittleEndian(buff.Slice(i));
                var scale = n >> 27 & 0x1f;
                var scalef = MathF.Pow(2, scale - 24);
                var b = n >> 18 & 0x1ff;
                var g = n >> 9 & 0x1ff;
                var r = n & 0x1ff;
                destSpan[i] = (byte)MathF.Round(b * scalef * 255f);
                destSpan[i + 1] = (byte)MathF.Round(g * scalef * 255f);
                destSpan[i + 2] = (byte)MathF.Round(r * scalef * 255f);
                destSpan[i + 3] = 255;
            }
            return true;
        }

        private bool DecodeBC4(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeBC4(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeBC5(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeBC5(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeBC6H(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeBC6(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeBC7(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeBC7(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeDXT1Crunched(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            if (TryUnpackCrunch(buff, out var result))
            {
                return DecodeDXT1(result, destSpan);
            }
            return false;
        }

        private bool DecodeDXT5Crunched(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            if (TryUnpackCrunch(buff, out var result))
            {
                return DecodeDXT5(result, destSpan);
            }
            return false;
        }

        private bool DecodePVRTC(ReadOnlySpan<byte> buff, Span<byte> destSpan, bool is2bpp)
        {
            return TextureDecoder.DecodePVRTC(buff, m_Width, m_Height, destSpan, is2bpp);
        }

        private bool DecodeETC1(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeETC1(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeATCRGB4(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeATCRGB4(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeATCRGBA8(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeATCRGBA8(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeEACR(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeEACR(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeEACRSigned(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeEACRSigned(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeEACRG(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeEACRG(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeEACRGSigned(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeEACRGSigned(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeETC2(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeETC2(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeETC2A1(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeETC2A1(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeETC2A8(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            return TextureDecoder.DecodeETC2A8(buff, m_Width, m_Height, destSpan);
        }

        private bool DecodeASTC(ReadOnlySpan<byte> buff, Span<byte> destSpan, int blocksize)
        {
            return TextureDecoder.DecodeASTC(buff, m_Width, m_Height, blocksize, blocksize, destSpan);
        }

        private bool DecodeRG16(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            for (var i = 0; i < size; i++)
            {
                destSpan[i * 4] = 0; //B
                destSpan[i * 4 + 1] = buff[i * 2 + 1]; //G
                destSpan[i * 4 + 2] = buff[i * 2]; //R
                destSpan[i * 4 + 3] = 255; //A
            }
            return true;
        }

        private bool DecodeR8(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            for (var i = 0; i < size; i++)
            {
                destSpan[i * 4] = 0; //B
                destSpan[i * 4 + 1] = 0; //G
                destSpan[i * 4 + 2] = buff[i]; //R
                destSpan[i * 4 + 3] = 255; //A
            }
            return true;
        }

        private bool DecodeETC1Crunched(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            if (TryUnpackCrunch(buff, out var result))
            {
                return DecodeETC1(result, destSpan);
            }
            return false;
        }

        private bool DecodeETC2A8Crunched(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            if (TryUnpackCrunch(buff, out var result))
            {
                return DecodeETC2A8(result, destSpan);
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte DownScaleFrom16BitTo8Bit(ushort component)
        {
            return (byte)(((component * 255) + 32895) >> 16);
        }

        private bool DecodeRG32(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = 0; //B
                destSpan[i + 1] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i + 2))); //G
                destSpan[i + 2] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i))); //R
                destSpan[i + 3] = byte.MaxValue; //A
            }
            return true;
        }

        private bool DecodeRGB48(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            var size = m_Width * m_Height;
            for (var i = 0; i < size; i++)
            {
                destSpan[i * 4] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 6 + 4))); //B
                destSpan[i * 4 + 1] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 6 + 2))); //G
                destSpan[i * 4 + 2] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 6))); //R
                destSpan[i * 4 + 3] = byte.MaxValue; //A
            }
            return true;
        }

        private bool DecodeRGBA64(ReadOnlySpan<byte> buff, Span<byte> destSpan)
        {
            for (var i = 0; i < outputDataSize; i += 4)
            {
                destSpan[i] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2 + 4))); //B
                destSpan[i + 1] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2 + 2))); //G
                destSpan[i + 2] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2))); //R
                destSpan[i + 3] = DownScaleFrom16BitTo8Bit(BinaryPrimitives.ReadUInt16LittleEndian(buff.Slice(i * 2 + 6))); //A
            }
            return true;
        }

        private bool TryUnpackCrunch(ReadOnlySpan<byte> buff, out ReadOnlySpan<byte> result)
        {
            if (m_Texture2D.version >= (2017, 3) //2017.3 and up
                || m_TextureFormat == TextureFormat.ETC_RGB4Crunched
                || m_TextureFormat == TextureFormat.ETC2_RGBA8Crunched)
            {
                result = TextureDecoder.UnpackUnityCrunch(buff);
            }
            else
            {
                result = TextureDecoder.UnpackCrunch(buff);
            }
            return !result.IsEmpty;
        }
    }
}
