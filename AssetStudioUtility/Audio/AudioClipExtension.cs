namespace AssetStudio
{
    public static class AudioClipExtension
    {
        public static string GetCompressionFormatString(this AudioClip m_AudioClip)
        {
            if (m_AudioClip.version < (2, 6))
            {
                switch ((LegacyAudioType)m_AudioClip.m_Format)
                {
                    case LegacyAudioType.RawWavU8Mono: return "PCM WAV (8-bit unsigned)";
                    case LegacyAudioType.RawWavS16Mono:
                    case LegacyAudioType.RawWavS16Stereo: return "PCM WAV (16-bit signed)";
                    case LegacyAudioType.CompressedAudio: return "Externally compressed audio (OGG/MP3/..)";
                    default: return $"Unknown ({m_AudioClip.m_Format})";
                }
            }
            if (m_AudioClip.version < 5)
            {
                switch (m_AudioClip.m_Type)
                {
                    case FMODSoundType.AAC: return "AAC";
                    case FMODSoundType.AIFF: return "AIFF";
                    case FMODSoundType.IT: return "Impulse tracker";
                    case FMODSoundType.MOD: return "Protracker / Fasttracker MOD";
                    case FMODSoundType.MPEG: return "MP2/MP3 MPEG";
                    case FMODSoundType.OGGVORBIS: return "Ogg vorbis";
                    case FMODSoundType.S3M: return "ScreamTracker 3";
                    case FMODSoundType.WAV: return "Microsoft WAV";
                    case FMODSoundType.XM: return "FastTracker 2 XM";
                    case FMODSoundType.XMA: return "Xbox360 XMA";
                    case FMODSoundType.VAG: return "PlayStation Portable ADPCM";
                    case FMODSoundType.AUDIOQUEUE: return "iPhone";
                    default: return $"Unknown ({m_AudioClip.m_Type})";
                }
            }
            else
            {
                switch (m_AudioClip.m_CompressionFormat)
                {
                    case AudioCompressionFormat.PCM: return "PCM";
                    case AudioCompressionFormat.Vorbis: return "Vorbis";
                    case AudioCompressionFormat.ADPCM: return "ADPCM";
                    case AudioCompressionFormat.MP3: return "MP3";
                    case AudioCompressionFormat.PSMVAG: return "PlayStation Portable ADPCM";
                    case AudioCompressionFormat.HEVAG: return "PSVita ADPCM";
                    case AudioCompressionFormat.XMA: return "Xbox360 XMA";
                    case AudioCompressionFormat.AAC: return "AAC";
                    case AudioCompressionFormat.GCADPCM: return "Nintendo 3DS/Wii DSP";
                    case AudioCompressionFormat.ATRAC9: return "PSVita ATRAC9";
                    default: return "Unknown";
                }
            }
        }

        public static bool IsLegacyRawWav(this AudioClip m_AudioClip)
        {
            return m_AudioClip.version < (2, 6)
                   && m_AudioClip.m_Format > 0
                   && (LegacyAudioType)m_AudioClip.m_Format < LegacyAudioType.CompressedAudio;
        }
    }
}
