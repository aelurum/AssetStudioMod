using System.Collections.Generic;

namespace AssetStudio
{
    public sealed class Animation : Component
    {
        public List<PPtr<AnimationClip>> m_Animations;

        public Animation(ObjectReader reader) : base(reader)
        {
            if (version >= (1, 5)) //Behaviour
            {
                var m_Enabled = reader.ReadByte();
                if (reader.version >= (2, 1))
                    reader.AlignStream();
            }
            var m_Animation = new PPtr<AnimationClip>(reader);
            int numAnimations = reader.ReadInt32();
            m_Animations = new List<PPtr<AnimationClip>>();
            for (var i = 0; i < numAnimations; i++)
            {
                if (version < (1, 5))
                {
                    var name = reader.ReadAlignedString();
                }
                m_Animations.Add(new PPtr<AnimationClip>(reader));
            }
        }
    }
}
