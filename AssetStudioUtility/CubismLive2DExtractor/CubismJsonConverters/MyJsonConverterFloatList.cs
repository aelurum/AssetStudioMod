using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CubismLive2DExtractor.CubismJsonConverters
{
    public class MyJsonConverterFloatList : JsonConverter<List<float>>
    {
        public override List<float> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<List<float>>(ref reader, new JsonSerializerOptions
            {
                NumberHandling = options.NumberHandling
            });
        }

        public override void Write(Utf8JsonWriter writer, List<float> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var n in value)
            {
                if (float.IsNaN(n) || float.IsInfinity(n))
                {
                    if (options.NumberHandling == JsonNumberHandling.AllowNamedFloatingPointLiterals)
                    {
                        writer.WriteStringValue($"{n.ToString(CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        writer.WriteStringValue(JsonSerializer.Serialize(value));
                    }
                }
                else
                {
                   writer.WriteNumberValue(MathF.Round(n, 3));
                }
            }
            writer.WriteEndArray();
        }
    }
}
