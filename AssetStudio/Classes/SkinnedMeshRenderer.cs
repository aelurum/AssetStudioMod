using System.Collections.Generic;

namespace AssetStudio
{
    //unity 2.1+
    public sealed class SkinnedMeshRenderer : Renderer
    {
        public PPtr<Mesh> m_Mesh;
        public List<PPtr<Transform>> m_Bones;
        public float[] m_BlendShapeWeights;

        public SkinnedMeshRenderer() { }

        public SkinnedMeshRenderer(ObjectReader reader) : base(reader)
        {
            int m_Quality = reader.ReadInt32();
            var m_UpdateWhenOffscreen = reader.ReadBoolean();
            var m_SkinNormals = reader.ReadBoolean(); //3.1.0 and below
            reader.AlignStream();

            if (version < (2, 6)) //2.1 - 2.6 down
            {
                var m_DisableAnimationWhenOffscreen = new PPtr<Animation>(reader);
            }

            m_Mesh = new PPtr<Mesh>(reader);

            var numBones = reader.ReadInt32();
            m_Bones = new List<PPtr<Transform>>();
            for (var b = 0; b < numBones; b++)
            {
                m_Bones.Add(new PPtr<Transform>(reader));
            }

            if (version >= (4, 3)) //4.3 and up
            {
                m_BlendShapeWeights = reader.ReadSingleArray();
            }
        }
    }

    //unity 1.x - 2.1 down
    public sealed class SkinnedMeshFilter
    {
        private PPtr<Mesh> m_Mesh;
        private List<PPtr<Transform>> m_Bones;
        private PPtr<GameObject> m_GameObject;
        private List<PPtr<Material>> m_Materials;
        private readonly ObjectReader _reader;

        public SkinnedMeshFilter(ObjectReader reader)
        {
            _reader = reader;
            reader.Reset();
            m_GameObject = new PPtr<GameObject>(reader); //Component
            if (reader.version == (2, 0)) //Renderer
            {
                var m_Enabled = reader.ReadBoolean();
                var m_CastShadows = reader.ReadBoolean();
                var m_ReceiveShadows = reader.ReadBoolean();
                var m_MaterialsSize = reader.ReadInt32();
                m_Materials = new List<PPtr<Material>>();
                for (var i = 0; i < m_MaterialsSize; i++)
                {
                    m_Materials.Add(new PPtr<Material>(reader));
                }
            }
            if (reader.version >= (1, 5))
            {
                var m_UpdateWhenOffscreen = reader.ReadBoolean();
                var m_Quality = reader.ReadInt32();
                var m_SkinNormals = reader.ReadBoolean();
                var m_Animation = new PPtr<Animation>(reader);
            }
            var numBones = reader.ReadInt32();
            m_Bones = new List<PPtr<Transform>>();
            for (var b = 0; b < numBones; b++)
            {
                m_Bones.Add(new PPtr<Transform>(reader));
            }
            m_Mesh = new PPtr<Mesh>(reader);
        }

        public SkinnedMeshRenderer ToSkinnedMeshRenderer()
        {
            return new SkinnedMeshRenderer()
            {
                reader = _reader,
                assetsFile = _reader.assetsFile,
                serializedType = _reader.serializedType,
                version = _reader.version,
                platform = _reader.platform,
                type = _reader.type,
                classID = _reader.classID,
                m_PathID = _reader.m_PathID,
                byteSize = _reader.byteSize,
                m_GameObject = m_GameObject,
                m_Materials = m_Materials,
                m_Mesh = m_Mesh,
                m_Bones = m_Bones,
            };
        }
    }
}
