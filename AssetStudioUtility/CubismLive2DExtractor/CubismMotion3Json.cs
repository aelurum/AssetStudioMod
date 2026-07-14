// File Format Specifications
// https://github.com/Live2D/CubismSpecs/blob/master/FileFormats/motion3.json.md

using AssetStudio;
using CubismLive2DExtractor.CubismJsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace CubismLive2DExtractor
{
    public class CubismMotion3Json
    {
        public int Version;
        public SerializableMeta Meta;
        public SerializableCurve[] Curves;
        public SerializableUserData[] UserData;

        public class SerializableMeta
        {
            public float Duration;
            public float Fps;
            public bool Loop;
            public bool AreBeziersRestricted;
            public float FadeInTime;
            public float FadeOutTime;
            public int CurveCount;
            public int TotalSegmentCount;
            public int TotalPointCount;
            public int UserDataCount;
            public int TotalUserDataSize;
        }

        public class SerializableCurve
        {
            public string Target;
            public string Id;
            public float FadeInTime;
            public float FadeOutTime;
            [JsonConverter(typeof(MyJsonConverterFloatList))]
            public List<float> Segments;
        }

        public class SerializableUserData
        {
            public float Time;
            public string Value;
        }

        private static void AddSegments(
            Keyframe<float> curve,
            Keyframe<float> preCurve,
            Keyframe<float> nextCurve,
            SerializableCurve cubismCurve,
            bool forceBezier,
            ref int totalPointCount,
            ref int totalSegmentCount,
            ref int j
        )
        {
            if (Math.Abs(curve.time - preCurve.time - 0.01f) < 0.0001f) // InverseSteppedSegment
            {
                if (nextCurve.value == curve.value)
                {
                    cubismCurve.Segments.Add(3f); // Segment ID
                    cubismCurve.Segments.Add(nextCurve.time);
                    cubismCurve.Segments.Add(nextCurve.value);
                    j += 1;
                    totalPointCount += 1;
                    totalSegmentCount++;
                    return;
                }
            }
            if (float.IsPositiveInfinity(curve.inSlope)) // SteppedSegment
            {
                cubismCurve.Segments.Add(2f); // Segment ID
                cubismCurve.Segments.Add(curve.time);
                cubismCurve.Segments.Add(curve.value);
                totalPointCount += 1;
            }
            else if (preCurve.outSlope == 0f && Math.Abs(curve.inSlope) < 0.0001f && !forceBezier) // LinearSegment
            {
                cubismCurve.Segments.Add(0f); // Segment ID
                cubismCurve.Segments.Add(curve.time);
                cubismCurve.Segments.Add(curve.value);
                totalPointCount += 1;
            }
            else // BezierSegment
            {
                var tangentLength = (curve.time - preCurve.time) / 3f;
                cubismCurve.Segments.Add(1f); // Segment ID
                cubismCurve.Segments.Add(preCurve.time + tangentLength);
                cubismCurve.Segments.Add(preCurve.outSlope * tangentLength + preCurve.value);
                cubismCurve.Segments.Add(curve.time - tangentLength);
                cubismCurve.Segments.Add(curve.value - curve.inSlope * tangentLength);
                cubismCurve.Segments.Add(curve.time);
                cubismCurve.Segments.Add(curve.value);
                totalPointCount += 3;
            }
            totalSegmentCount++;
        }

        public CubismMotion3Json(CubismUnityClasses.CubismFadeMotionData fadeMotion, HashSet<string> paramNames, HashSet<string> partNames, bool forceBezier)
        {
            Version = 3;
            Meta = new SerializableMeta
            {
                // Duration of the motion in seconds.
                Duration = fadeMotion.MotionLength,
                // Framerate of the motion in seconds.
                Fps = 30,
                // [Optional] Status of the looping of the motion.
                Loop = true,
                // [Optional] Status of the restriction of Bezier handles'X translations.
                AreBeziersRestricted = true,
                // [Optional] Time of the overall Fade-In for easing in seconds.
                FadeInTime = fadeMotion.FadeInTime,
                // [Optional] Time of the overall Fade-Out for easing in seconds.
                FadeOutTime = fadeMotion.FadeOutTime,
                // The total number of curves.
                CurveCount = (int)fadeMotion.ParameterCurves.LongCount(x => x.m_Curve.Count > 0),
                // [Optional] The total number of UserData.
                UserDataCount = 0
            };
            // Motion curves.
            Curves = new SerializableCurve[Meta.CurveCount];

            var totalSegmentCount = 0;
            var totalPointCount = 0;
            var actualCurveCount = 0;
            for (var i = 0; i < fadeMotion.ParameterCurves.Length; i++)
            {
                if (fadeMotion.ParameterCurves[i].m_Curve.Count == 0)
                    continue;

                string target;
                string paramId = fadeMotion.ParameterIds[i];
                switch (paramId)
                {
                    case "Opacity":
                    case "EyeBlink":
                    case "LipSync":
                        target = "Model";
                        break;
                    default:
                        if (paramNames.Contains(paramId))
                        {
                            target = "Parameter";
                        }
                        else if (partNames.Contains(paramId))
                        {
                            target = "PartOpacity";
                        }
                        else
                        {
                            target = paramId.ToLower().Contains("part") ? "PartOpacity" : "Parameter";
                            Logger.Debug($"[{fadeMotion.m_Name}] Binding failed: Unable to find \"{paramId}\" among the model parts/parameters");
                        }
                        break;
                }
                Curves[actualCurveCount] = new SerializableCurve
                {
                    // Target type.
                    Target = target,
                    // Identifier for mapping curve to target.
                    Id = paramId,
                    // [Optional] Time of the Fade - In for easing in seconds.
                    FadeInTime = fadeMotion.ParameterFadeInTimes[i],
                    // [Optional] Time of the Fade - Out for easing in seconds.
                    FadeOutTime = fadeMotion.ParameterFadeOutTimes[i],
                    // Flattened segments.
                    Segments = new List<float>
                    {
                        // First point
                        fadeMotion.ParameterCurves[i].m_Curve[0].time,
                        fadeMotion.ParameterCurves[i].m_Curve[0].value
                    }
                };
                for (var j = 1; j < fadeMotion.ParameterCurves[i].m_Curve.Count; j++)
                {
                    var curve = fadeMotion.ParameterCurves[i].m_Curve[j];
                    var preCurve = fadeMotion.ParameterCurves[i].m_Curve[j - 1];
                    var next = fadeMotion.ParameterCurves[i].m_Curve.ElementAtOrDefault(j + 1);
                    var nextCurve = next ?? new Keyframe<float>();
                    AddSegments(curve, preCurve, nextCurve, Curves[actualCurveCount], forceBezier, ref totalPointCount, ref totalSegmentCount, ref j);
                }
                actualCurveCount++;
                totalPointCount++;
            }

            // The total number of segments (from all curves).
            Meta.TotalSegmentCount = totalSegmentCount;
            // The total number of points (from all segments of all curves).
            Meta.TotalPointCount = totalPointCount;

            UserData = Array.Empty<SerializableUserData>();
            // [Optional] The total size of UserData in bytes.
            Meta.TotalUserDataSize = 0;
        }

        public CubismMotion3Json(ImportedKeyframedAnimation animation, bool forceBezier)
        {
            Version = 3;
            Meta = new SerializableMeta
            {
                Duration = animation.Duration,
                Fps = animation.SampleRate,
                Loop = true,
                AreBeziersRestricted = true,
                FadeInTime = 0,
                FadeOutTime = 0,
                CurveCount = animation.TrackList.Count,
                UserDataCount = animation.Events.Count
            };
            Curves = new SerializableCurve[Meta.CurveCount];

            var totalSegmentCount = 0;
            var totalPointCount = 0;
            for (var i = 0; i < Meta.CurveCount; i++)
            {
                var track = animation.TrackList[i];
                Curves[i] = new SerializableCurve
                {
                    Target = track.Target,
                    Id = track.Name,
                    FadeInTime = -1,
                    FadeOutTime = -1,
                    Segments = new List<float>
                    {
                        0f,
                        track.Curve[0].value
                    }
                };
                for (var j = 1; j < track.Curve.Count; j++)
                {
                    var curve = CreateKeyFrame(track.Curve[j]);
                    var preCurve = CreateKeyFrame(track.Curve[j - 1]);
                    var next = track.Curve.ElementAtOrDefault(j + 1);
                    var nextCurve = next != null ? CreateKeyFrame(next) : new Keyframe<float>();
                    AddSegments(curve, preCurve, nextCurve, Curves[i], forceBezier, ref totalPointCount, ref totalSegmentCount, ref j);
                }
                totalPointCount++;
            }
            Meta.TotalSegmentCount = totalSegmentCount;
            Meta.TotalPointCount = totalPointCount;

            UserData = new SerializableUserData[Meta.UserDataCount];
            var totalUserDataSize = 0;
            for (var i = 0; i < Meta.UserDataCount; i++)
            {
                var @event = animation.Events[i];
                UserData[i] = new SerializableUserData
                {
                    Time = @event.time,
                    Value = @event.value
                };
                totalUserDataSize += @event.value.Length;
            }
            Meta.TotalUserDataSize = totalUserDataSize;
        }

        private static Keyframe<float> CreateKeyFrame(ImportedKeyframe<float> iKeyframe)
        {
            return new Keyframe<float>
            {
                time = iKeyframe.time,
                value = iKeyframe.value,
                inSlope = iKeyframe.inSlope,
                outSlope = iKeyframe.outSlope,
                weightedMode = 0,
                inWeight = 0,
                outWeight = 0,
            };
        }
    }
}
