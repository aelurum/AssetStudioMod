using AssetStudio;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace AssetStudioGUI
{
    internal static class ParallelExporter
    {
        private static readonly ConcurrentDictionary<string, bool> ExportPathDict = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public static bool ExportTexture2D(AssetItem item, string exportPath, out string debugLog)
        {
            debugLog = "";
            var m_Texture2D = (Texture2D)item.Asset;
            if (Properties.Settings.Default.convertTexture)
            {
                var type = Properties.Settings.Default.convertType;
                var converter = new Texture2DConverter(m_Texture2D);
                if (Properties.Settings.Default.filenameFormat == 3) //asset name@hash
                    item.Hash = converter.GetTextureHash();
                if (!TryExportFile(exportPath, item, $".{type}", out var exportFullPath))
                    return false;

                if (GUILogger.ShowDebugMessage)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Converting {item.TypeString} \"{m_Texture2D.m_Name}\" to {type}..");
                    sb.AppendLine($"Width: {m_Texture2D.m_Width}");
                    sb.AppendLine($"Height: {m_Texture2D.m_Height}");
                    sb.AppendLine($"Format: {m_Texture2D.m_TextureFormat}");
                    switch (m_Texture2D.m_TextureSettings.m_FilterMode)
                    {
                        case 0: sb.AppendLine("Filter Mode: Point "); break;
                        case 1: sb.AppendLine("Filter Mode: Bilinear "); break;
                        case 2: sb.AppendLine("Filter Mode: Trilinear "); break;
                    }
                    sb.AppendLine($"Anisotropic level: {m_Texture2D.m_TextureSettings.m_Aniso}");
                    sb.AppendLine($"Mip map bias: {m_Texture2D.m_TextureSettings.m_MipBias}");
                    switch (m_Texture2D.m_TextureSettings.m_WrapMode)
                    {
                        case 0: sb.AppendLine($"Wrap mode: Repeat"); break;
                        case 1: sb.AppendLine($"Wrap mode: Clamp"); break;
                    }
                    debugLog += sb.ToString();
                }

                if ((type == ImageFormat.Png && m_Texture2D.m_TextureFormat.IsPng())
                    || (type == ImageFormat.Webp && m_Texture2D.m_TextureFormat.IsWebP()))
                {
                    File.WriteAllBytes(exportFullPath, m_Texture2D.image_data.GetData());
                    debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                    return true;
                }

                if (!converter.TryConvertToBgra32Image(flip: true, out var image))
                {
                    Logger.Warning($"{debugLog}Failed to convert texture \"{m_Texture2D.m_Name}\" into image");
                    return false;
                }
                using (image)
                {
                    using (var file = File.OpenWrite(exportFullPath))
                    {
                        image.WriteToStream(file, type);
                    }
                    debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                    return true;
                }
            }
            else
            {
                if (!TryExportFile(exportPath, item, ".tex", out var exportFullPath))
                    return false;
                File.WriteAllBytes(exportFullPath, m_Texture2D.image_data.GetData());
                debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                return true;
            }
        }

        public static bool ExportSprite(AssetItem item, string exportPath, out string debugLog)
        {
            debugLog = "";
            var type = Properties.Settings.Default.convertType;
            var spriteMaskMode = Properties.Settings.Default.exportSpriteWithMask ? SpriteMaskMode.On : SpriteMaskMode.Off;
            var converter = new SpriteConverter((Sprite)item.Asset, Properties.Settings.Default.keepSpriteSize);
            if (Properties.Settings.Default.filenameFormat == 3) //asset name@hash
                item.Hash = converter.GetSpriteHash(maskMode: spriteMaskMode);
            if (!TryExportFile(exportPath, item, $".{type}", out var exportFullPath))
                return false;
            
            if (converter.TryConvertToImage(out var image, maskMode: spriteMaskMode))
            {
                using (image)
                {
                    using (var file = File.OpenWrite(exportFullPath))
                    {
                        image.WriteToStream(file, type);
                    }
                    debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                    return true;
                }
            }
            Logger.Warning($"Failed to convert sprite \"{item.Text}\" into image");
            return false;
        }

        public static bool ExportAudioClip(AssetItem item, string exportPath, out string debugLog)
        {
            debugLog = string.Empty;
            var m_AudioClip = (AudioClip)item.Asset;
            var audioBuff = BigArrayPool<byte>.Shared.Rent(m_AudioClip.m_AudioData.Size);
            try
            {
                string exportFullPath;
                var dataLen = m_AudioClip.m_AudioData.GetData(audioBuff);
                if (dataLen <= 0)
                {
                    Logger.Warning($"Failed to export \"{item.Text}\": AudioData was not found");
                    return false;
                }
                var isWav = m_AudioClip.m_Type == FMODSoundType.WAV || m_AudioClip.IsLegacyRawWav();
                var converter = new AudioClipConverter(m_AudioClip, audioBuff);
                if (Properties.Settings.Default.convertAudio && !isWav && converter.IsSupport)
                {
                    if (!TryExportFile(exportPath, item, ".wav", out exportFullPath))
                        return false;

                    if (GUILogger.ShowDebugMessage)
                    {
                        debugLog += $"Converting {item.TypeString} \"{m_AudioClip.m_Name}\" to wav..\n";
                        debugLog += GenerateAudioClipInfo(m_AudioClip);
                    }

                    if (!converter.TryConvertToWav(out var buffer, ref debugLog))
                    {
                        Logger.Warning($"{debugLog}Failed to export \"{item.Text}\": Failed to convert audio to wav");
                        return false;
                    }
                    File.WriteAllBytes(exportFullPath, buffer);
                }
                else
                {
                    if (!TryExportFile(exportPath, item, converter.GetExtensionName(), out exportFullPath))
                        return false;

                    if (GUILogger.ShowDebugMessage)
                    {
                        debugLog += $"Exporting {item.TypeString} \"{m_AudioClip.m_Name}\"..\n";
                        debugLog += GenerateAudioClipInfo(m_AudioClip);
                    }

                    using (var file = File.OpenWrite(exportFullPath))
                    {
                        if (m_AudioClip.IsLegacyRawWav())
                        {
                            var buffer = converter.WrapWavData(ref debugLog);
                            file.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            file.Write(audioBuff, 0, m_AudioClip.m_AudioData.Size);
                        }
                    }
                }
                debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                return true;
            }
            finally
            {
                BigArrayPool<byte>.Shared.Return(audioBuff, clearArray: true);
            }
        }

        private static string GenerateAudioClipInfo(AudioClip m_AudioClip)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"AudioClip compression format: {m_AudioClip.GetCompressionFormatString()}");
            if (m_AudioClip.m_Length > 0)
                sb.AppendLine($"AudioClip length: {m_AudioClip.m_Length:0.0##}");
            if (m_AudioClip.m_Frequency > 0)
                sb.AppendLine($"AudioClip sample rate: {m_AudioClip.m_Frequency}");
            if (m_AudioClip.m_Channels > 0)
                sb.AppendLine($"AudioClip channel count: {m_AudioClip.m_Channels}");
            if (m_AudioClip.version >= 5)
                sb.AppendLine($"AudioClip bit depth: {m_AudioClip.m_BitsPerSample}");

            return sb.ToString();
        }

        private static bool TryExportFile(string dir, AssetItem item, string extension, out string fullPath)
        {
            var fileName = item.Text;
            var filenameFormatIndex = Properties.Settings.Default.filenameFormat;
            var canOverwrite = Properties.Settings.Default.overwriteExistingFiles;
            switch (filenameFormatIndex)
            {
                case 1: //assetName@pathID
                    fileName = $"{fileName} @{item.m_PathID}";
                    break;
                case 2: //pathID
                    fileName = item.m_PathID.ToString();
                    break;
                case 3 when item.Hash > 0: //asset name@hash
                    fileName = $"{fileName} @{item.Hash:x}";
                    break;
            }
            extension = extension.ToLower();
            fullPath = Exporter.SafeFilePath(dir, $"{fileName}{extension}");
            if (ExportPathDict.TryAdd(fullPath, true))
            {
                if (CanWrite(fullPath, dir, canOverwrite))
                {
                    return true;
                }
            }
            else if (filenameFormatIndex == 0 || (filenameFormatIndex == 3 && item.Hash == 0)) //assetName, asset name@hash
            {
                fullPath = Exporter.SafeFilePath(dir, $"{fileName}{item.UniqueID}{extension}");
                if (CanWrite(fullPath, dir, canOverwrite))
                {
                    return true;
                }
            }
            Logger.Warning($"Export failed. File \"{fullPath.Color(ColorConsole.BrightYellow)}\" already exist");
            return false;
        }

        private static bool CanWrite(string fullPath, string dir, bool canOverwrite)
        {
            if (!canOverwrite && File.Exists(fullPath))
                return false;
            Directory.CreateDirectory(dir);
            return true;
        }

        public static bool ParallelExportConvertFile(AssetItem item, string exportPath, out string debugLog)
        {
            switch (item.Type)
            {
                case ClassIDType.Texture2D:
                case ClassIDType.Texture2DArrayImage:
                    return ExportTexture2D(item, exportPath, out debugLog);
                case ClassIDType.Sprite:
                    return ExportSprite(item, exportPath, out debugLog);
                case ClassIDType.AudioClip:
                    return ExportAudioClip(item, exportPath, out debugLog);
                default:
                    throw new NotImplementedException();
            }
        }

        public static void ClearHash()
        {
            ExportPathDict.Clear();
        }
    }
}
