using System;
using System.Text.Json;

namespace AssetStudio
{
    public sealed class BuildSettings : Object
    {
        public string[] levels;
        public string[] scenes;
        public string m_Version;

        public BuildSettings() { }

        public BuildSettings(ObjectReader reader, ReadOnlySpan<byte> type, JsonSerializerOptions jsonOptions) : base(reader)
        {
            var parsedSettings = JsonSerializer.Deserialize<BuildSettings>(type, jsonOptions);
            m_Version = parsedSettings.m_Version;
        }

        public BuildSettings(ObjectReader reader) : base(reader)
        {
            if (reader.version < (5, 1)) //5.1 down
            {
                levels = reader.ReadStringArray();
            }
            else
            {
                scenes = reader.ReadStringArray();
            }
        }
    }
}
