namespace AssetStudio
{
    public enum TextureFormat
    {
        /// <summary>
        /// Alpha-only texture format, 8 bit integer.
        /// </summary>
        Alpha8 = 1,
        /// <summary>
        /// A 16 bits/pixel texture format. Texture stores color with an alpha channel.
        /// </summary>
        ARGB4444,
        /// <summary>
        /// Three channel (RGB) texture format, 8-bits unsigned integer per channel.
        /// </summary>
        RGB24,
        /// <summary>
        /// Four channel (RGBA) texture format, 8-bits unsigned integer per channel.
        /// </summary>
        RGBA32,
        /// <summary>
        /// Color with alpha texture format, 8-bits per channel.
        /// </summary>
        ARGB32,
        /// <summary>
        /// </summary>
        ARGBFloat,
        /// <summary>
        /// A 16 bit color texture format.
        /// </summary>
        RGB565,
        /// <summary> 
        /// </summary>
        BGR24,
        /// <summary>
        /// Single channel (R) texture format, 16 bit integer.
        /// </summary>
        R16,
        /// <summary>
        /// Compressed color texture format.
        /// </summary>
        DXT1,
        /// <summary>
        /// Compressed color with alpha channel texture format.
        /// </summary>
        DXT3,
        /// <summary>
        /// Compressed color with alpha channel texture format.
        /// </summary>
        DXT5,
        /// <summary>
        /// Color and alpha texture format, 4 bit per channel.
        /// </summary>
        RGBA4444,
        /// <summary>
        /// Color with alpha texture format, 8-bits per channel.
        /// </summary>
        BGRA32,
        /// <summary>
        /// Scalar (R)  texture format, 16 bit floating point.
        /// </summary>
        RHalf,
        /// <summary>
        /// Two color (RG)  texture format, 16 bit floating point per channel.
        /// </summary>
        RGHalf,
        /// <summary>
        /// RGB color and alpha texture format, 16 bit floating point per channel.
        /// </summary>
        RGBAHalf,
        /// <summary>
        /// Scalar (R) texture format, 32 bit floating point.
        /// </summary>
        RFloat,
        /// <summary>
        /// Two color (RG)  texture format, 32 bit floating point per channel.
        /// </summary>
        RGFloat,
        /// <summary>
        /// RGB color and alpha texture format,  32-bit floats per channel.
        /// </summary>
        RGBAFloat,
        /// <summary>
        /// A format that uses the YUV color space and is often used for video encoding or playback.
        /// </summary>
        YUY2,
        /// <summary>
        /// RGB HDR format, with 9 bit mantissa per channel and a 5 bit shared exponent.
        /// </summary>
        RGB9e5Float,
        /// <summary>
        /// </summary>
        RGBFloat,
        /// <summary>
        /// HDR compressed color texture format.
        /// </summary>
        BC6H,
        /// <summary>
        /// High quality compressed color texture format.
        /// </summary>
        BC7,
        /// <summary>
        /// Compressed one channel (R) texture format.
        /// </summary>
        BC4,
        /// <summary>
        /// Compressed two-channel (RG) texture format.
        /// </summary>
        BC5,
        /// <summary>
        /// Compressed color texture format with Crunch compression for smaller storage sizes.
        /// </summary>
        DXT1Crunched,
        /// <summary>
        /// Compressed color with alpha channel texture format with Crunch compression for smaller storage sizes.
        /// </summary>
        DXT5Crunched,
        /// <summary>
        /// PowerVR (iOS) 2 bits/pixel compressed color texture format.
        /// </summary>
        PVRTC_RGB2,
        /// <summary>
        /// PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format.
        /// </summary>
        PVRTC_RGBA2,
        /// <summary>
        /// PowerVR (iOS) 4 bits/pixel compressed color texture format.
        /// </summary>
        PVRTC_RGB4,
        /// <summary>
        /// PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format.
        /// </summary>
        PVRTC_RGBA4,
        /// <summary>
        /// ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        /// </summary>
        ETC_RGB4,
        /// <summary>
        /// ATC (ATITC) 4 bits/pixel compressed RGB texture format.
        /// </summary>
        ATC_RGB4,
        /// <summary>
        /// ATC (ATITC) 8 bits/pixel compressed RGB texture format.
        /// </summary>
        ATC_RGBA8,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 4 bits/pixel compressed unsigned single-channel texture format.
        /// </summary>
        EAC_R = 41,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 4 bits/pixel compressed signed single-channel texture format.
        /// </summary>
        EAC_R_SIGNED,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 8 bits/pixel compressed unsigned dual-channel (RG) texture format.
        /// </summary>
        EAC_RG,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 8 bits/pixel compressed signed dual-channel (RG) texture format.
        /// </summary>
        EAC_RG_SIGNED,
        /// <summary>
        /// ETC2 (GL ES 3.0) 4 bits/pixel compressed RGB texture format.
        /// </summary>
        ETC2_RGB,
        /// <summary>
        /// ETC2 (GL ES 3.0) 4 bits/pixel RGB+1-bit alpha texture format.
        /// </summary>
        ETC2_RGBA1,
        /// <summary>
        /// ETC2 (GL ES 3.0) 8 bits/pixel compressed RGBA texture format.
        /// </summary>
        ETC2_RGBA8,
        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        ASTC_RGB_4x4,
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        ASTC_RGB_5x5,
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        ASTC_RGB_6x6,
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        ASTC_RGB_8x8,
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        ASTC_RGB_10x10,
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        ASTC_RGB_12x12,
        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        ASTC_RGBA_4x4,
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        ASTC_RGBA_5x5,
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        ASTC_RGBA_6x6,
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        ASTC_RGBA_8x8,
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        ASTC_RGBA_10x10,
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        ASTC_RGBA_12x12,
        /// <summary>
        /// ETC 4 bits/pixel compressed RGB texture format.
        /// </summary>
        ETC_RGB4_3DS,
        /// <summary>
        /// ETC 4 bits/pixel RGB + 4 bits/pixel Alpha compressed texture format.
        /// </summary>
        ETC_RGBA8_3DS,
        /// <summary>
        /// Two color (RG) texture format, 8-bits per channel.
        /// </summary>
        RG16,
        /// <summary>
        /// Single channel (R) texture format, 8 bit integer.
        /// </summary>
        R8,
        /// <summary>
        /// Compressed color texture format with Crunch compression for smaller storage sizes.
        /// </summary>
        ETC_RGB4Crunched,
        /// <summary>
        /// Compressed color with alpha channel texture format using Crunch compression for smaller storage sizes.
        /// </summary>
        ETC2_RGBA8Crunched,
        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        ASTC_HDR_4x4,
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        ASTC_HDR_5x5,
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        ASTC_HDR_6x6,
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        ASTC_HDR_8x8,
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        ASTC_HDR_10x10,
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        ASTC_HDR_12x12,
        /// <summary>
        /// Two channel (RG) texture format, 16-bits unsigned integer per channel.
        /// </summary>
        RG32,
        /// <summary>
        /// Three channel (RGB) texture format, 16-bits unsigned integer per channel.
        /// </summary>
        RGB48,
        /// <summary>
        /// Four channel (RGBA) texture format, 16-bits unsigned integer per channel.
        /// </summary>
        RGBA64,
        /// <summary>
        /// Single channel (R) texture format, 8-bits signed integer.
        /// </summary>
        R8_SIGNED,
        /// <summary>
        /// Two channel (RG) texture format, 8-bits signed integer per channel.
        /// </summary>
        RG16_SIGNED,
        /// <summary>
        /// Three channel (RGB) texture format, 8-bits signed integer per channel.
        /// </summary>
        RGB24_SIGNED,
        /// <summary>
        /// Four channel (RGBA) texture format, 8-bits signed integer per channel.
        /// </summary>
        RGBA32_SIGNED,
        /// <summary>
        /// Single channel (R) texture format, 16-bits signed integer.
        /// </summary>
        R16_SIGNED,
        /// <summary>
        /// Two channel (RG) texture format, 16-bits signed integer per channel.
        /// </summary>
        RG32_SIGNED,
        /// <summary>
        ///  Three color (RGB) texture format, 16-bits signed integer per channel.
        /// </summary>
        RGB48_SIGNED,
        /// <summary>
        /// Four channel (RGBA) texture format, 16-bits signed integer per channel.
        /// </summary>
        RGBA64_SIGNED,

        //Tuanjie engine
        //(adding 1000 for compatibility with Unity formats)
        /// <summary>
        /// Offset for Tuanjie texture formats.
        /// </summary>
        TUANJIE_OFFSET = 1000,
        /// <summary>
        /// PNG (32 bits) compressed RGBA texture format.
        /// </summary>
        PNG_32_RGBA = TUANJIE_OFFSET + 75,
        /// <summary>
        /// PNG (24 bits) compressed RGB texture format.
        /// </summary>
        PNG_24_RGB = TUANJIE_OFFSET + 76,
        /// <summary>
        /// PNG (8 bits) compressed RGBA texture format.
        /// </summary>
        PNG_8_RGBA = TUANJIE_OFFSET + 77,
    }

    public static class TextureFormatExtensions
    {
        public static TextureFormat ToTuanjie(this TextureFormat format)
        {
            return format > TextureFormat.RGBA64 && format < TextureFormat.TUANJIE_OFFSET
                ? TextureFormat.TUANJIE_OFFSET + (int)format
                : format;
        }

        public static bool IsPng(this TextureFormat format) => format >= TextureFormat.PNG_32_RGBA && format <= TextureFormat.PNG_8_RGBA;
    }
}
