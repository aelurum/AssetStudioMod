using AssetStudio;
using System;

namespace CubismLive2DExtractor.CubismUnityClasses
{
    public sealed class CubismFadeMotionData : MonoBehaviour
    {
        public string MotionName { get; set; }
        public float FadeInTime { get; set; }
        public float FadeOutTime { get; set; }
        public string[] ParameterIds { get; set; } = Array.Empty<string>();
        public AnimationCurve<float>[] ParameterCurves { get; set; }
        public float[] ParameterFadeInTimes { get; set; }
        public float[] ParameterFadeOutTimes { get; set; }
        public float MotionLength { get; set; }
    }
}
