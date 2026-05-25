namespace AssetStudio.Classes.Tuanjie.AnimationClip
{
    public sealed class ACLClip
    {
        public uint m_FrameCount;
        public uint m_BoneCount;
        public float m_SampleRate;
        public uint m_CurveCount;
        public byte[] m_Tracks;
        public uint[] m_ACLDecoderMap;
        public bool m_UseACLFastSampleMode;

        public ACLClip() { }

        public ACLClip(ObjectReader reader)
        {
            var version = reader.version;
            m_FrameCount = reader.ReadUInt32();
            m_BoneCount = reader.ReadUInt32();
            m_SampleRate = reader.ReadSingle();
            if (version >= (2022, 3, 55)) //2022.3.55t1(1.5.0) and up
            {
                m_CurveCount = reader.ReadUInt32();
            }
            if (version >= (2022, 3, 62, 8)) //2022.3.62t8(1.9.0) and up
            {
                var m_BeginTime = reader.ReadSingle();
            }
            m_Tracks = reader.ReadUInt8Array();
            if (version >= (2022, 3, 61)) //2022.3.61t1(1.6.0) and up
            {
                reader.AlignStream();
            }
            m_ACLDecoderMap = reader.ReadUInt32Array();
            if (version > (2022, 3, 55) || (version == (2022, 3, 55) && version.Build >= 4)) //2022.3.55t4(1.5.3) and up
            {
                m_UseACLFastSampleMode = reader.ReadBoolean();
                if (version >= (2022, 3, 61)) //2022.3.61t1(1.6.0) and up
                {
                    reader.AlignStream();
                }
            }
        }
    }
}
