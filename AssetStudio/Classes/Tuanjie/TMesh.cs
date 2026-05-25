using System.Collections.Generic;

namespace AssetStudio.Classes.Tuanjie.Mesh
{
    public sealed class GIFacetInfo
    {
        public float[] matrix; //Matrix3x4f
        public byte orientation;

        public GIFacetInfo(ObjectReader reader)
        {
            matrix = reader.ReadSingleArray(12);
            orientation = reader.ReadByte();
            reader.AlignStream();
        }
    }

    public sealed class GIFacetMesh
    {
        public MinMaxAABB m_aabb;
        public bool m_doubleSide;
        public List<GIFacetInfo> m_GIFacetInfo;

        public GIFacetMesh(ObjectReader reader)
        {
            m_aabb = new MinMaxAABB(reader);
            m_doubleSide = reader.ReadBoolean();
            reader.AlignStream();

            var m_GIFacetInfoCount = reader.ReadInt32();
            m_GIFacetInfo = new List<GIFacetInfo>();
            for (var i = 0; i < m_GIFacetInfoCount; i++)
            {
                m_GIFacetInfo.Add(new GIFacetInfo(reader));
            }
        }
    }

    public sealed class GISDFMeshLOD
    {
        public uint brickMappingSizeX;
        public uint brickMappingSizeY;
        public uint brickMappingSizeZ;
        public Vector3 volumeToMappingScale;
        public Vector3 volumeToMappingBias;
        public float SDFDecodeScale;
        public float SDFDecodeBias;
        public uint[] activeBrickIDs;
        public byte[] activeBrickData;

        public GISDFMeshLOD(ObjectReader reader)
        {
            brickMappingSizeX = reader.ReadUInt32();
            brickMappingSizeY = reader.ReadUInt32();
            brickMappingSizeZ = reader.ReadUInt32();
            volumeToMappingScale = reader.ReadVector3();
            volumeToMappingBias = reader.ReadVector3();
            SDFDecodeScale = reader.ReadSingle();
            SDFDecodeBias = reader.ReadSingle();
            activeBrickIDs = reader.ReadUInt32Array();
            activeBrickData = reader.ReadUInt8Array();
            reader.AlignStream();
        }
    }

    public sealed class GISDFMesh
    {
        public AABB localAABB;
        public float sdfGenBias;
        public bool is2Sided;
        public List<GISDFMeshLOD> LODs;

        public GISDFMesh(ObjectReader reader)
        {
            localAABB = new AABB(reader);
            sdfGenBias = reader.ReadSingle();
            is2Sided = reader.ReadBoolean();
            reader.AlignStream();

            var LODsCount = reader.ReadInt32();
            LODs = new List<GISDFMeshLOD>();
            for (var i = 0; i < LODsCount; i++)
            {
                LODs.Add(new GISDFMeshLOD(reader));
            }
        }
    }

    public sealed class VGPackedHierarchyNode
    {
        public Vector4[] LODBounds = new Vector4[8];
        public Vector3[] AABBCenter = new Vector3[8];
        public uint[] MinLODError_MaxParentLODError = new uint[8];
        public Vector3[] AABBExtent = new Vector3[8];
        public uint[] ChildStartIndex = new uint[8];
        public uint[] PageIndex_PageCount_PageBlockCount = new uint[8];

        public VGPackedHierarchyNode(ObjectReader reader)
        {
            for (var i = 0; i < 8; i++)
            {
                LODBounds[i] = reader.ReadVector4();
                AABBCenter[i] = reader.ReadVector3();
                MinLODError_MaxParentLODError[i] = reader.ReadUInt32();
                AABBExtent[i] = reader.ReadVector3();
                ChildStartIndex[i] = reader.ReadUInt32();
                PageIndex_PageCount_PageBlockCount[i] = reader.ReadUInt32();
            }
        }
    }

    public sealed class VGPageStreamingInfo
    {
        public uint offset;
        public uint wholeSize;
        public uint dataSize;
        public uint dependencyOffset;
        public uint dependencyCount;
        public uint flags;

        public VGPageStreamingInfo(ObjectReader reader)
        {
            offset = reader.ReadUInt32();
            wholeSize = reader.ReadUInt32();
            dataSize = reader.ReadUInt32();
            dependencyOffset = reader.ReadUInt32();
            dependencyCount = reader.ReadUInt32();
            flags = reader.ReadUInt32();
        }
    }

    public sealed class SharedClusterData
    {
        public int m_LightmapUseUV1;
        public float m_fileScale;
        public byte[] rootClusterPage;
        public List<VGPackedHierarchyNode> hierarchyNodes;
        public List<VGPageStreamingInfo> pageStreamingInfos;
        public uint[] pageIndicesOfDependencies;
        public byte[] streamableClusterPageSize;

        public SharedClusterData(ObjectReader reader, byte rev)
        {
            m_LightmapUseUV1 = reader.ReadInt32();
            if (reader.version < (2022, 3, 62)) //2022.3.62t1(1.7.0) down
            {
                m_fileScale = reader.ReadSingle();
            }
            if (rev == 1)
            {
                var NumInputTriangles = reader.ReadUInt32();
                var NumInputVertices = reader.ReadUInt32();
                var NumInputMeshes = reader.ReadUInt16();
                var NumInputTexCoords = reader.ReadUInt16();
                var ResourceFlags = reader.ReadUInt32();
            }
            rootClusterPage = reader.ReadUInt8Array();
            reader.AlignStream();
            if (rev == 1)
            {
                var imposterAtlasCount = reader.ReadInt32();
                reader.Position += imposterAtlasCount * 2; //skip ushort[] imposterAtlas
                reader.AlignStream();
            }
            var hierarchyNodesCount = reader.ReadInt32();
            hierarchyNodes = new List<VGPackedHierarchyNode>();
            for (var i = 0; i < hierarchyNodesCount; i++)
            {
                hierarchyNodes.Add(new VGPackedHierarchyNode(reader));
            }
            if (rev == 1)
            {
                var hierarchyRootOffsetsCount = reader.ReadInt32();
                reader.Position += hierarchyRootOffsetsCount * 4; //skip uint[] hierarchyRootOffsets
            }
            var pageStreamingInfosCount = reader.ReadInt32();
            pageStreamingInfos = new List<VGPageStreamingInfo>();
            for (var i = 0; i < pageStreamingInfosCount; i++)
            {
                pageStreamingInfos.Add(new VGPageStreamingInfo(reader));
            }
            pageIndicesOfDependencies = reader.ReadUInt32Array();
            if (rev == 2)
            {
                var inputTrianglesCount = reader.ReadUInt32();
                var inputVerticesCount = reader.ReadUInt32();
                var inputMeshesCount = reader.ReadUInt16();
                var inputTexCoordsCount = reader.ReadUInt16();
                var resourceFlags = reader.ReadUInt32();
                var imposterAtlasCount = reader.ReadInt32();
                reader.Position += imposterAtlasCount * 2; //skip ushort[] imposterAtlas
                reader.AlignStream();
                var hierarchyRootOffsetsCount = reader.ReadInt32();
                reader.Position += hierarchyRootOffsetsCount * 4; //skip uint[] hierarchyRootOffsets
            }
            streamableClusterPageSize = reader.ReadUInt8Array();
            reader.AlignStream();
        }
    }
}
