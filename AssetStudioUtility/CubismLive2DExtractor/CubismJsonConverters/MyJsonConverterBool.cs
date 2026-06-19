using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CubismLive2DExtractor.CubismJsonConverters
{
    public sealed class MyJsonConverterBool : JsonConverter<bool>
    {
       public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.True: return true;
                case JsonTokenType.False: return false;
                case JsonTokenType.Number: return reader.GetByte() == 1;
                default: throw new JsonException($"Unable to convert \"{reader.TokenType}\" token to boolean.");
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
