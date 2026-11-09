using System.Collections.Generic;

namespace CubismLive2DExtractor
{
    public class ImportedKeyframedAnimation
    {
        public string Name { get; set; }
        public float SampleRate { get; set; }
        public float Duration { get; set; }

        public List<ImportedAnimationKeyframedTrack> TrackList { get; set; } = new List<ImportedAnimationKeyframedTrack>();
        public List<ImportedEvent> Events = new List<ImportedEvent>();

        public ImportedAnimationKeyframedTrack FindTrack(string name)
        {
            var track = TrackList.Find(x => x.Name == name);
            if (track == null)
            {
                track = new ImportedAnimationKeyframedTrack { Name = name };
                TrackList.Add(track);
            }
            return track;
        }
    }

    public class ImportedKeyframe<T>
    {
        public float time { get; set; }
        public T value { get; set; }
        public T inSlope { get; set; }
        public T outSlope { get; set; }
        public float[] coeff { get; set; }
        // True for densely-sampled keys (dense/constant clip data): zero slopes there
        // really mean linear interpolation between samples, not authored easing.
        public bool sampled { get; set; }

        public ImportedKeyframe(float time, T value, T inSlope, T outSlope, float[] coeff, bool sampled = false)
        {
            this.time = time;
            this.value = value;
            this.inSlope = inSlope;
            this.outSlope = outSlope;
            this.coeff = coeff;
            this.sampled = sampled;
        }

        public float Evaluate(float sampleTime)
        {
            float t = sampleTime - time;
            return (t * (t * (t * coeff[0] + coeff[1]) + coeff[2])) + coeff[3];
        }
    }

    public class ImportedAnimationKeyframedTrack
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public List<ImportedKeyframe<float>> Curve = new List<ImportedKeyframe<float>>();
    }

    public class ImportedEvent
    {
        public float time { get; set; }
        public string value { get; set; }
    }
}
