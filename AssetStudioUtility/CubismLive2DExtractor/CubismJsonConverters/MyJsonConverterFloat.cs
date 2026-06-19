using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CubismLive2DExtractor.CubismJsonConverters
{
    public class MyJsonConverterFloat : JsonConverter<float>
    {
        private readonly bool _asDecimal;

        public MyJsonConverterFloat(bool asDecimal = false)
        {
            _asDecimal = asDecimal;
        }

        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<float>(ref reader, new JsonSerializerOptions
            {
                NumberHandling = options.NumberHandling
            });
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                if (options.NumberHandling == JsonNumberHandling.AllowNamedFloatingPointLiterals)
                {
                    writer.WriteStringValue($"{value.ToString(CultureInfo.InvariantCulture)}");
                }
                else
                {
                    writer.WriteStringValue(JsonSerializer.Serialize(value));
                }
            }
            else
            {
                if (_asDecimal && value.Equals((int)value))
                {
                    writer.WriteNumberValue((decimal)value + 0.0m);
                }
                else
                {
                    writer.WriteNumberValue(MathF.Round(value, 3));
                }
            }
        }
    }
}
