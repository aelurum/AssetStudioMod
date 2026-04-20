using FMOD;
using System;
using System.Runtime.InteropServices;
using WavHelper;

namespace AssetStudio
{
    public sealed class AudioClipConverter
    {
        private static FMOD.System system;
        private static byte[] oggMagic = { 0x4F, 0x67, 0x67, 0x53 }; //OggS
        private static byte[] fsb5Magic = { 0x46, 0x53, 0x42, 0x35 }; //FSB5
        private static byte[] fsb4Magic = { 0x46, 0x53, 0x42, 0x34 }; //FSB4

        private readonly AudioClip m_AudioClip;
        private byte[] audioBuff;
        private CREATESOUNDEXINFO exinfo;
        private readonly SOUND_TYPE fmodSoundType;
        private readonly bool isLegacyRawWav;

        public bool IsSupport { get; }

        static AudioClipConverter()
        {
            var result = Factory.System_Create(out system);
            if (result != RESULT.OK)
            {
                Logger.Error($"FMOD error! {result} - {Error.String(result)}");
            }
            result = system.init(1, INITFLAGS.NORMAL, IntPtr.Zero);
            if (result != RESULT.OK)
            {
                Logger.Error($"FMOD error! {result} - {Error.String(result)}");
            }
        }

        public AudioClipConverter(AudioClip audioClip, byte[] audioBuff)
        {
            m_AudioClip = audioClip;
            this.audioBuff = audioBuff;

            exinfo = new CREATESOUNDEXINFO();
            exinfo.cbsize = Marshal.SizeOf(exinfo);
            exinfo.length = (uint)m_AudioClip.m_Size;
            var mode = MODE.OPENMEMORY | MODE.LOWMEM | MODE.MPEGSEARCH;
            if (m_AudioClip.IsLegacyRawWav())
            {
                exinfo.format = (LegacyAudioType)m_AudioClip.m_Format == LegacyAudioType.RawWavU8Mono ? SOUND_FORMAT.PCM8 : SOUND_FORMAT.PCM16;
                exinfo.numchannels = m_AudioClip.m_Channels;
                exinfo.defaultfrequency = m_AudioClip.m_Frequency;
                mode |= MODE.OPENRAW;
                isLegacyRawWav = true;
            }
            var result = system.createStream(audioBuff, mode, ref exinfo, out var sound);
            if (result != RESULT.OK)
                return;

            _ = sound.getFormat(out fmodSoundType, out var soundFormat, out _, out _);
            sound.release();
            sound.clearHandle();
            IsSupport = soundFormat > SOUND_FORMAT.NONE && soundFormat < SOUND_FORMAT.BITSTREAM;
        }

        public bool TryConvertToWav(out byte[] wavBuffer, ref string debugLog)
        {
            wavBuffer = null;
            if (!IsSupport)
                return false;

            var mode = MODE.OPENMEMORY | MODE.LOWMEM | MODE.ACCURATETIME | MODE.MPEGSEARCH;
            if (isLegacyRawWav)
            {
                mode |= MODE.OPENRAW;
                if (m_AudioClip.m_AudioData.Endian == EndianType.BigEndian && exinfo.format == SOUND_FORMAT.PCM16)
                {
                    SwapPcm16Endian(audioBuff.AsSpan(0, (int)m_AudioClip.m_Size));
                }
            }
            var result = system.createSound(audioBuff, mode, ref exinfo, out var sound);
            if (ErrorCheck(result, ref debugLog))
                return false;
            result = sound.getNumSubSounds(out var numsubsounds);
            if (ErrorCheck(result, ref debugLog))
                return false;
            if (numsubsounds > 0)
            {
                result = sound.getSubSound(0, out var subsound);
                if (ErrorCheck(result, ref debugLog))
                    return false;
                wavBuffer = SoundToWav(subsound, ref debugLog);
                subsound.release();
                subsound.clearHandle();
            }
            else
            {
                wavBuffer = SoundToWav(sound, ref debugLog);
            }
            sound.release();
            sound.clearHandle();

            return true;
        }

        private byte[] SoundToWav(Sound sound, ref string debugLog)
        {
            var convertToPcm16 = false;
            var audioFormat = WavAudioFormat.PCM;

            debugLog += "[Fmod] Detecting sound format..\n";
            var result = sound.getFormat(out SOUND_TYPE soundType, out SOUND_FORMAT soundFormat, out int channels, out int bits);
            if (ErrorCheck(result, ref debugLog))
                return null;
            debugLog += $"Detected sound type: {soundType}\n" +
                        $"Detected sound format: {soundFormat}\n" +
                        $"Detected channels: {channels}\n" +
                        $"Detected bit depth: {bits}\n";
            if (soundFormat == SOUND_FORMAT.PCMFLOAT)
            {
                switch (m_AudioClip.m_BitsPerSample)
                {
                    case 16:
                        convertToPcm16 = true;
                        bits = 16;
                        break;
                    case 32:
                        audioFormat = WavAudioFormat.IEEEfloat;
                        break;
                }
            }
            result = sound.getDefaults(out var frequency, out _);
            if (ErrorCheck(result, ref debugLog))
                return null;
            result = sound.getLength(out var length, TIMEUNIT.PCMBYTES);
            if (ErrorCheck(result, ref debugLog))
                return null;
            result = sound.@lock(0, length, out var ptr1, out var ptr2, out var len1, out var len2);
            if (ErrorCheck(result, ref debugLog))
                return null;
            var wavDataLength = convertToPcm16 ? len1 / 2 : len1;
            var wavBuffer = new byte[wavDataLength + 44];
            if (bits == 8)
            {
                ReadAsPcm8U(ptr1, wavBuffer, 44, len1, ref debugLog);
            }
            else if (convertToPcm16)
            {
                ReadAsPcm16(ptr1, wavBuffer, 44, len1, ref debugLog);
            }
            else
            {
                Marshal.Copy(ptr1, wavBuffer, 44, (int)len1);
            }
            result = sound.unlock(ptr1, ptr2, len1, len2);
            if (ErrorCheck(result, ref debugLog))
                return null;
            //添加wav头
            var wavHeader = new WavHeader(wavDataLength, audioFormat, channels, (uint)frequency, bits);
            wavHeader.WriteToArray(wavBuffer);
            return wavBuffer;
        }

        private static void ReadAsPcm8U(IntPtr srcPtr, byte[] wavBuffer, int offset, uint pcmDataLen, ref string debugLog)
        {
            for (var i = 0; i < pcmDataLen; i++)
            {
                wavBuffer[offset++] = (byte)(Marshal.ReadByte(srcPtr, i) - 0x80);
            }
            debugLog += "Finished FMOD_PCM8_SIGNED -> PCM8_UNSIGNED converting\n";
        }

        private static void ReadAsPcm16(IntPtr srcPtr, byte[] wavBuffer, int offset, uint pcmDataLen, ref string debugLog)
        {
            var pcmFloatVal = new byte[4];
            for (var i = 0; i < pcmDataLen; i += 4)
            {
                for (var j = 0; j < 4; j++)
                {
                    pcmFloatVal[j] = Marshal.ReadByte(srcPtr, i + j);
                }
                var pcm16Val = (short)MathHelper.Clamp(BitConverter.ToSingle(pcmFloatVal, 0) * short.MaxValue, short.MinValue, short.MaxValue);
                wavBuffer[offset] = (byte)(pcm16Val & 255);
                wavBuffer[offset + 1] = (byte)(pcm16Val >> 8);
                offset += 2;
            }
            debugLog += "Finished FMOD_PCMFLOAT -> PCM16 converting\n";
        }

        //For raw wav data in unity 1.x - 2.5
        //we can handle it with fmod as well, but this way should be better, I guess
        public byte[] WrapWavData(ref string debugLog)
        {
            if (!isLegacyRawWav)
                return null;

            var legacyType = (LegacyAudioType)m_AudioClip.m_Format;
            var audioSize = (int)m_AudioClip.m_Size;
            var channels = m_AudioClip.m_Channels;
            var sampleRate = m_AudioClip.m_Frequency;
            var audioFormat = WavAudioFormat.PCM;
            var bits = legacyType == LegacyAudioType.RawWavU8Mono ? 8 : 16;

            debugLog += "[Legacy AudioClip] Generating wav header..\n";
            if (m_AudioClip.m_AudioData.Endian == EndianType.BigEndian && bits == 16)
            {
                SwapPcm16Endian(audioBuff.AsSpan(0, audioSize));
            }
            var wavBuffer = new byte[audioSize + 44];
            var wavHeader = new WavHeader((uint)audioSize, audioFormat, channels, (uint)sampleRate, bits);
            wavHeader.WriteToArray(wavBuffer);
            audioBuff.AsSpan(0, audioSize).CopyTo(wavBuffer.AsSpan(44));

            return wavBuffer;
        }

        public static void SwapPcm16Endian(Span<byte> buffer)
        {
            for (var i = 0; i < buffer.Length; i += 2)
            {
                buffer.Slice(i, 2).Reverse();
            }
        }

        private static bool ErrorCheck(RESULT result, ref string log)
        {
            if (result != RESULT.OK)
            {
                log += $"FMOD error! {result} - {Error.String(result)}\n";
                return true;
            }
            return false;
        }

        public string GetExtensionName()
        {
            //version based
            if (isLegacyRawWav)
                return ".wav";

            //signature based
            var magic = audioBuff.AsSpan(0, 4);
            if (magic.SequenceEqual(fsb5Magic) || magic.SequenceEqual(fsb4Magic))
                return ".fsb";
            if (magic.SequenceEqual(oggMagic))
                return ".ogg";

            //audioClip based
            if (m_AudioClip.version < 5)
            {
                if (m_AudioClip.version < (2, 6))
                {
                    switch (fmodSoundType) //fmod based
                    {
                        case SOUND_TYPE.MPEG: return ".mp3";
                        case SOUND_TYPE.OGGVORBIS: return ".ogg";
                    }
                }

                switch (m_AudioClip.m_Type)
                {
                    case FMODSoundType.AAC: return ".m4a";
                    case FMODSoundType.AIFF: return ".aif";
                    case FMODSoundType.ASF: return ".wma";
                    case FMODSoundType.FLAC: return ".flac";
                    case FMODSoundType.FSB: return ".fsb";
                    case FMODSoundType.IT: return ".it";
                    case FMODSoundType.MOD: return ".mod";
                    case FMODSoundType.MPEG: return ".mp3";
                    case FMODSoundType.OGGVORBIS: return ".ogg";
                    case FMODSoundType.S3M: return ".s3m";
                    case FMODSoundType.WAV: return ".wav";
                    case FMODSoundType.XM: return ".xm";
                    case FMODSoundType.XMA: return ".xma";
                    case FMODSoundType.VAG: return ".vag";
                    case FMODSoundType.AUDIOQUEUE: return ".fsb";
                }

            }
            else
            {
                switch (m_AudioClip.m_CompressionFormat)
                {
                    case AudioCompressionFormat.AAC: return ".m4a";
                }
            }

            return ".AudioClip";
        }
    }
}
