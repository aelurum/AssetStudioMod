using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssetStudio
{
    public sealed class SpriteBone
    {
        public string name;
        public string guid;
        public Vector3 position;
        public Quaternion rotation;
        public float length;
        public int parentId;
        public Color color;

        public SpriteBone(ObjectReader reader)
        {
            name = reader.ReadAlignedString();
            guid = reader.ReadAlignedString();
            position = reader.ReadVector3();
            rotation = reader.ReadQuaternion();
            length = reader.ReadSingle();
            parentId = reader.ReadInt32();
            color = reader.ReadColor4();
        }
    }

    public sealed class SpriteInstanceData
    {
        public string spriteName;
        public Rectf rect;
        public Vector4 border;
        public Vector2 pivot;
        public float pixelsToUnits;
        public int m_IndexFormat;
        public List<SubMesh> m_SubMeshes;
        public byte[] m_IndexBuffer;
        public VertexData m_VertexData;
        public Matrix4x4[] m_Bindpose;
        public BlendShapeData m_BlendShapes;
        public List<SpriteBone> spriteBones;
        public List<Vector2[]> physicsShape;

        public SpriteInstanceData(ObjectReader reader)
        {
            spriteName = reader.ReadAlignedString();
            rect = new Rectf(reader);
            border = reader.ReadVector4();
            pivot = reader.ReadVector2();
            pixelsToUnits = reader.ReadSingle();
            m_IndexFormat = reader.ReadInt32();

            var m_SubMeshesCount = reader.ReadInt32();
            m_SubMeshes = new List<SubMesh>();
            for (var i = 0; i < m_SubMeshesCount; i++)
            {
                m_SubMeshes.Add(new SubMesh(reader));
            }

            m_IndexBuffer = reader.ReadUInt8Array();
            reader.AlignStream();
            m_VertexData = new VertexData(reader);
            m_Bindpose = reader.ReadMatrixArray();
            m_BlendShapes = new BlendShapeData(reader);

            var spriteBonesCount = reader.ReadInt32();
            spriteBones = new List<SpriteBone>();
            for (var i = 0; i < spriteBonesCount; i++)
            {
                spriteBones.Add(new SpriteBone(reader));
            }

            var physicsShapeCount = reader.ReadInt32();
            physicsShape = new List<Vector2[]>();
            for (var i = 0; i < physicsShapeCount; i++)
            {
                physicsShape.Add(reader.ReadVector2Array());
            }
        }
    }

    public sealed class SpriteAtlasData
    {
        public PPtr<Texture2D> texture;
        public PPtr<Texture2D> alphaTexture;
        public Rectf textureRect;
        public Vector2 textureRectOffset;
        public Vector2 atlasRectOffset;
        public Vector4 uvTransform;
        public float downscaleMultiplier;
        public SpriteSettings settingsRaw;
        public List<SecondarySpriteTexture> secondaryTextures;
        public SpriteInstanceData spriteInstanceData;

        public SpriteAtlasData(ObjectReader reader)
        {
            var version = reader.version;
            texture = new PPtr<Texture2D>(reader);
            alphaTexture = new PPtr<Texture2D>(reader);
            textureRect = new Rectf(reader);
            textureRectOffset = reader.ReadVector2();
            if (version >= (2017, 2)) //2017.2 and up
            {
                atlasRectOffset = reader.ReadVector2();
            }
            uvTransform = reader.ReadVector4();
            downscaleMultiplier = reader.ReadSingle();
            settingsRaw = new SpriteSettings(reader);
            if (version >= (2020, 2)) //2020.2 and up
            {
                var secondaryTexturesCount = reader.ReadInt32();
                secondaryTextures = new List<SecondarySpriteTexture>();
                for (var i = 0; i < secondaryTexturesCount; i++)
                {
                    secondaryTextures.Add(new SecondarySpriteTexture(reader));
                }
                reader.AlignStream();
            }
            if (version >= (6000, 6)) //6000.6.0a3 and up
            {
                spriteInstanceData = new SpriteInstanceData(reader);
            }
        }
    }

    public sealed class SpriteAtlas : NamedObject
    {
        public List<PPtr<Sprite>> m_PackedSprites = new List<PPtr<Sprite>>();
        public string[] m_PackedSpriteNamesToIndex;
        [JsonConverter(typeof(JsonConverterHelper.RenderDataMapConverter))]
        public Dictionary<KeyValuePair<Guid, long>, SpriteAtlasData> m_RenderDataMap;
        public string m_Tag;
        public bool m_IsVariant;
        public Guid m_Guid;

        public SpriteAtlas(ObjectReader reader) : base(reader)
        {
            if (version < (6000, 6)) //6000.6 down
            {
                var m_PackedSpritesCount = reader.ReadInt32();
                for (var i = 0; i < m_PackedSpritesCount; i++)
                {
                    m_PackedSprites.Add(new PPtr<Sprite>(reader));
                }

                m_PackedSpriteNamesToIndex = reader.ReadStringArray();
            }

            var m_RenderDataMapCount = reader.ReadInt32();
            m_RenderDataMap = new Dictionary<KeyValuePair<Guid, long>, SpriteAtlasData>();
            for (var i = 0; i < m_RenderDataMapCount; i++)
            {
                var first = new Guid(reader.ReadBytes(16));
                var second = reader.ReadInt64();
                var value = new SpriteAtlasData(reader);
                m_RenderDataMap.Add(new KeyValuePair<Guid, long>(first, second), value);
            }
            m_Tag = reader.ReadAlignedString();
            m_IsVariant = reader.ReadBoolean();
            reader.AlignStream();

            if (version >= (6000, 5)) //6000.5.0a4 and up
            {
                m_Guid = new Guid(reader.ReadBytes(16));
            }
        }
    }
}
