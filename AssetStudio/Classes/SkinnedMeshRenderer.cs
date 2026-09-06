using System.Collections.Generic;
using System.Text.Json;

namespace AssetStudio
{
    //unity 2.1+
    public sealed class SkinnedMeshRenderer : Renderer
    {
        public PPtr<Mesh> m_Mesh;
        public List<PPtr<Transform>> m_Bones;
        public float[] m_BlendShapeWeights;

        public SkinnedMeshRenderer() { }

        public SkinnedMeshRenderer(ObjectReader reader, JsonSerializerOptions jsonOptions)
        {
            this.CopyObjectDataFromReader(reader);
            var parsedAsset = version < (2, 1)
                ? JsonSerializer.Deserialize<SkinnedMeshFilter>(TypeTreeHelper.ReadTypeByteArray(reader), jsonOptions).ToSkinnedMeshRenderer()
                : JsonSerializer.Deserialize<SkinnedMeshRenderer>(TypeTreeHelper.ReadTypeByteArray(reader), jsonOptions);
            m_GameObject = parsedAsset.m_GameObject;
            m_Materials = parsedAsset.m_Materials;
            m_StaticBatchInfo = parsedAsset.m_StaticBatchInfo;
            m_SubsetIndices = parsedAsset.m_SubsetIndices;
            m_Mesh = parsedAsset.m_Mesh;
            m_Bones = parsedAsset.m_Bones;
            m_BlendShapeWeights = parsedAsset.m_BlendShapeWeights;
        }

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
        public PPtr<Mesh> m_LodMesh;
        public List<PPtr<Transform>> m_Bones;
        public PPtr<GameObject> m_GameObject;
        public List<PPtr<Material>> m_Materials;
        private readonly ObjectReader _reader;

        public SkinnedMeshFilter(){ }

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
            m_LodMesh = new PPtr<Mesh>(reader);
        }

        public SkinnedMeshRenderer ToSkinnedMeshRenderer()
        {
            var skinnedMeshRenderer = new SkinnedMeshRenderer()
            {
                m_GameObject = m_GameObject,
                m_Materials = m_Materials,
                m_Mesh = m_LodMesh,
                m_Bones = m_Bones,
            };
            if (_reader != null)
            {
                skinnedMeshRenderer.CopyObjectDataFromReader(_reader);
            }

            return skinnedMeshRenderer;
        }
    }
}
