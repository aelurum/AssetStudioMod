using System.Text.Json.Serialization;

namespace CubismLive2DExtractor
{
    public class CubismExpression3Json
    {
        public string Type { get; set; }
        public float FadeInTime { get; set; }
        public float FadeOutTime { get; set; }
        public SerializableExpressionParameter[] Parameters { get; set; }

        public class SerializableExpressionParameter
        {
            public string Id { get; set; }
            public float Value { get; set; }
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public BlendType Blend { get; set; }
        }
    }
}
