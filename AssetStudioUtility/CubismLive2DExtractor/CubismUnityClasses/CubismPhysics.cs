using AssetStudio;
using CubismLive2DExtractor.CubismJsonConverters;
using System.Text.Json.Serialization;

namespace CubismLive2DExtractor.CubismUnityClasses
{
    public class CubismPhysicsNormalizationTuplet
    {
        public float Maximum { get; set; }
        public float Minimum { get; set; }
        public float Default { get; set; }
    }

    public class CubismPhysicsNormalization
    {
        public CubismPhysicsNormalizationTuplet Position { get; set; }
        public CubismPhysicsNormalizationTuplet Angle { get; set; }
    }

    public class CubismPhysicsParticle
    {
        public Vector2 InitialPosition { get; set; }
        public float Mobility { get; set; }
        public float Delay { get; set; }
        public float Acceleration { get; set; }
        public float Radius { get; set; }
    }

    public class CubismPhysicsOutput
    {
        public string DestinationId { get; set; }
        public int ParticleIndex { get; set; }
        public Vector2 TranslationScale { get; set; }
        public float AngleScale { get; set; }
        public float Weight { get; set; }
        public CubismPhysicsSourceComponent SourceComponent { get; set; }
        [JsonConverter(typeof(MyJsonConverterBool))]
        public bool IsInverted { get; set; }
    }

    public enum CubismPhysicsSourceComponent
    {
        X,
        Y,
        Angle,
    }

    public class CubismPhysicsInput
    {
        public string SourceId { get; set; }
        public Vector2 ScaleOfTranslation { get; set; }
        public float AngleScale { get; set; }
        public float Weight { get; set; }
        public CubismPhysicsSourceComponent SourceComponent { get; set; }
        [JsonConverter(typeof(MyJsonConverterBool))]
        public bool IsInverted { get; set; }
    }

    public class CubismPhysicsSubRig
    {
        public CubismPhysicsInput[] Input { get; set; }
        public CubismPhysicsOutput[] Output { get; set; }
        public CubismPhysicsParticle[] Particles { get; set; }
        public CubismPhysicsNormalization Normalization { get; set; }
    }

    public class CubismPhysicsRig
    {
        public CubismPhysicsSubRig[] SubRigs { get; set; }
        public Vector2 Gravity { get; set; } = new Vector2(0, -1);
        public Vector2 Wind { get; set; }
        public float Fps { get; set; }
    }

    public sealed class CubismPhysics : MonoBehaviour
    {
        [JsonPropertyName("_rig")]
        public CubismPhysicsRig Rig { get; set; }
    }
}
