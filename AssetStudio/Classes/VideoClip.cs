namespace AssetStudio
{
    public class StreamedResource
    {
        public string m_Source;
        public long m_Offset; //ulong
        public long m_Size; //ulong

        public StreamedResource(ObjectReader reader)
        {
            m_Source = reader.ReadAlignedString();
            m_Offset = reader.ReadInt64();
            m_Size = reader.ReadInt64();
        }
    }

    public sealed class VideoClip : NamedObject
    {
        public ResourceReader m_VideoData;
        public string m_OriginalPath;
        public StreamedResource m_ExternalResources;
        public uint Width;
        public uint Height;
        public double m_FrameRate;
        public int m_Format;
        public bool m_HasSplitAlpha;

        public VideoClip(ObjectReader reader) : base(reader)
        {
            m_OriginalPath = reader.ReadAlignedString();
            var m_ProxyWidth = reader.ReadUInt32();
            var m_ProxyHeight = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            if (version >= (2017, 2)) //2017.2 and up
            {
                var m_PixelAspecRatioNum = reader.ReadUInt32();
                var m_PixelAspecRatioDen = reader.ReadUInt32();
            }
            m_FrameRate = reader.ReadDouble();
            var m_FrameCount = reader.ReadUInt64();
            m_Format = reader.ReadInt32();
            var m_AudioChannelCount = reader.ReadUInt16Array();
            reader.AlignStream();
            var m_AudioSampleRate = reader.ReadUInt32Array();
            var m_AudioLanguage = reader.ReadStringArray();
            if (version >= 2020) //2020.1 and up
            {
                var m_VideoShadersSize = reader.ReadInt32();
                for (var i = 0; i < m_VideoShadersSize; i++)
                {
                    var m_VideoShaders = new PPtr<Shader>(reader);
                }
            }
            m_ExternalResources = new StreamedResource(reader);
            m_HasSplitAlpha = reader.ReadBoolean();
            if (version >= 2020) //2020.1 and up
            {
                var m_sRGB = reader.ReadBoolean();
            }

            m_VideoData = !string.IsNullOrEmpty(m_ExternalResources.m_Source)
                ? new ResourceReader(m_ExternalResources.m_Source, assetsFile, m_ExternalResources.m_Offset, m_ExternalResources.m_Size)
                : new ResourceReader(reader, reader.BaseStream.Position, m_ExternalResources.m_Size);
        }
    }
}
