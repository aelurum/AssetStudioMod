using System.Text.Json;

namespace AssetStudio
{
    public sealed class MeshRenderer : Renderer
    {
        public MeshRenderer() { }

        public MeshRenderer(ObjectReader reader, byte[] type, JsonSerializerOptions jsonOptions)
        {
            this.CopyObjectDataFromReader(reader);
            var parsedAsset = JsonSerializer.Deserialize<MeshRenderer>(type, jsonOptions);
            m_GameObject = parsedAsset.m_GameObject;
            m_Materials = parsedAsset.m_Materials;
            m_StaticBatchInfo = parsedAsset.m_StaticBatchInfo;
            m_SubsetIndices = parsedAsset.m_SubsetIndices;
        }

        public MeshRenderer(ObjectReader reader) : base(reader)
        {

        }
    }
}
