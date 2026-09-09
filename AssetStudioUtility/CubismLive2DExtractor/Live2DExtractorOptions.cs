using AssetStudio;
using System.Collections.Generic;

namespace CubismLive2DExtractor
{
    public sealed class Live2DExtractorOptions
    {
        public List<AnimationClip> SelectedClipMotions { get; set; }
        public List<MonoBehaviour> SelectedFadeMotions { get; set; }
        public MonoBehaviour SelectedFadeMotionList { get; set; }
        public Live2DMotionMode MotionMode { get; set; } = Live2DMotionMode.MonoBehaviour;
        public bool ForceBezier { get; set; }
        public bool SearchByFilename { get; set; }
        public bool FilterTextures { get; set; }
    }
}
