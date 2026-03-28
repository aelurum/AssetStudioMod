using AssetStudio.Addons.CubismLive2D;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssetStudio
{
    public sealed class GameObject : EditorExtension
    {
        public List<PPtr<Component>> m_Components;
        public string m_Name;

        public Transform m_Transform;
        public MeshRenderer m_MeshRenderer;
        public MeshFilter m_MeshFilter;
        public SkinnedMeshRenderer m_SkinnedMeshRenderer;
        public Animator m_Animator;
        public Animation m_Animation;
        [JsonIgnore]
        public CubismModel CubismModel;

        public GameObject(ObjectReader reader) : base(reader)
        {
            var m_ComponentSize = reader.ReadInt32();
            m_Components = new List<PPtr<Component>>();
            for (var i = 0; i < m_ComponentSize; i++)
            {
                if (version < (5, 5)) //5.5 down
                {
                    var first = reader.ReadInt32();
                }
                m_Components.Add(new PPtr<Component>(reader));
            }

            if (version < (2, 1)) //2.1 down
            {
                var m_IsActive = reader.ReadByte();
                var m_Layer = version < (1, 5) ? reader.ReadUInt16() : reader.ReadUInt32();
                var m_Tag = reader.ReadUInt16();
            }
            else
            {
                var m_Layer = reader.ReadInt32();
            }

            if (version.IsTuanjie && version >= (2022, 3, 2, 11)) //2022.3.2t11(1.1.3) and up
            {
                var m_HasEditorInfo = reader.ReadBoolean();
                reader.AlignStream();
            }

            m_Name = version >= (1, 5) ? reader.ReadAlignedString() : $"GameObject {reader.m_PathID}";
        }
    }
}
