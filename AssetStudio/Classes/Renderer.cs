using System.Collections.Generic;

namespace AssetStudio
{
    public class StaticBatchInfo
    {
        public ushort firstSubMesh;
        public ushort subMeshCount;

        public StaticBatchInfo() { }

        public StaticBatchInfo(ObjectReader reader)
        {
            firstSubMesh = reader.ReadUInt16();
            subMeshCount = reader.ReadUInt16();
        }
    }

    public abstract class Renderer : Component
    {
        public List<PPtr<Material>> m_Materials;
        public StaticBatchInfo m_StaticBatchInfo;
        public uint[] m_SubsetIndices;

        protected Renderer() { }

        protected Renderer(ObjectReader reader) : base(reader)
        {
            if (version < 5) //5.0 down
            {
                if (version >= (1, 5))
                {
                    var m_Enabled = reader.ReadBoolean();
                    if (version >= (2, 0))
                    {
                        var m_CastShadows = reader.ReadBoolean();
                        var m_ReceiveShadows = reader.ReadBoolean();
                        if (version >= (2, 1))
                        {
                            var m_LightmapIndex = reader.ReadByte();
                        }
                    }
                }
            }
            else //5.0 and up
            {
                if (version >= (5, 4)) //5.4 and up
                {
                    var m_Enabled = reader.ReadBoolean();
                    var m_CastShadows = reader.ReadByte();
                    var m_ReceiveShadows = reader.ReadByte();
                    if (version >= (2017, 2)) //2017.2 and up
                    {
                        var m_DynamicOccludee = reader.ReadByte();
                    }
                    if (version >= 2021) //2021.1 and up
                    {
                        var m_StaticShadowCaster = reader.ReadByte();
                    }
                    var m_MotionVectors = reader.ReadByte();
                    var m_LightProbeUsage = reader.ReadByte();
                    var m_ReflectionProbeUsage = reader.ReadByte();
                    if (version >= (2019, 3)) //2019.3 and up
                    {
                        var m_RayTracingMode = reader.ReadByte();
                    }
                    if (version >= 2020) //2020.1 and up
                    {
                        var m_RayTraceProcedural = reader.ReadByte();
                    }
                    if (version.IsTuanjie) //2022.3.2t3(1.0.0) and up
                    {
                        var m_virtualGeometry = reader.ReadByte();
                        if (version < (2022, 3, 62, 2)) //2022.3.62t2(1.8.0) down
                        {
                            var m_virtualGeometryShadow = reader.ReadByte();
                        }
                        if (version >= (2022, 3, 48, 3)) //2022.3.48t3(1.4.0) and up
                        {
                            reader.AlignStream();
                            var m_ShadingRate = reader.ReadByte();
                            if (version >= (2022, 3, 61)) //2022.3.61t1(1.6.0) and up
                            {
                                var m_ForceDisableGRD = reader.ReadByte();
                            }
                            if (version >= (2022, 3, 62, 12)) //2022.3.62t12(1.10.0) and up
                            {
                                var m_ForceDisableSmallMeshCull = reader.ReadByte();
                            }
                        }
                    }
                    if (version >= (2023, 2)) //2023.2 and up
                    {
                        var m_RayTracingAccelStructBuildFlagsOverride = reader.ReadByte();
                        var m_RayTracingAccelStructBuildFlags = reader.ReadByte();
                    }
                    if (version >= (2023, 3)) //2023.3 and up
                    {
                        var m_SmallMeshCulling = reader.ReadByte();
                    }
                    reader.AlignStream();
                    if (version >= (6000, 2)) //6000.2 and up
                    {
                        var m_ForceMeshLod = reader.ReadInt16();
                        reader.AlignStream();
                        var m_MeshLodSelectionBias = reader.ReadSingle();
                    }
                }
                else
                {
                    var m_Enabled = reader.ReadBoolean();
                    reader.AlignStream();
                    var m_CastShadows = reader.ReadByte();
                    var m_ReceiveShadows = reader.ReadBoolean();
                    reader.AlignStream();
                }

                if (version >= 2018) //2018 and up
                {
                    var m_RenderingLayerMask = reader.ReadUInt32();
                }

                if (version >= (2018, 3)) //2018.3 and up
                {
                    var m_RendererPriority = reader.ReadInt32();
                }

                var m_LightmapIndex = reader.ReadUInt16();
                var m_LightmapIndexDynamic = reader.ReadUInt16();
            }

            if (version >= 3) //3.0 and up
            {
                var m_LightmapTilingOffset = reader.ReadVector4();
            }

            if (version >= 5) //5.0 and up
            {
                var m_LightmapTilingOffsetDynamic = reader.ReadVector4();
            }

            var m_MaterialsSize = reader.ReadInt32();
            m_Materials = new List<PPtr<Material>>();
            for (var i = 0; i < m_MaterialsSize; i++)
            {
                m_Materials.Add(new PPtr<Material>(reader));
            }
            if (reader.version < (2, 1)) //2.1 down
                return;

            if (version < 3) //3.0 down
            {
                //var unk = reader.ReadVector3(); //unity-iphone 1.7
                var m_LightmapTilingOffset = reader.ReadVector4();
                if (m_LightmapTilingOffset.W > 0) //if unity-iphone 1.7
                    m_LightmapTilingOffset = reader.ReadVector3();
            }
            else //3.0 and up
            {
                if (version >= (5, 5)) //5.5 and up
                {
                    m_StaticBatchInfo = new StaticBatchInfo(reader);
                }
                else
                {
                    m_SubsetIndices = reader.ReadUInt32Array();
                }

                var m_StaticBatchRoot = new PPtr<Transform>(reader);
            }

            if (version >= (5, 4)) //5.4 and up
            {
                var m_ProbeAnchor = new PPtr<Transform>(reader);
                var m_LightProbeVolumeOverride = new PPtr<GameObject>(reader);
            }
            else if (version >= (3, 5)) //3.5 - 5.3
            {
                var m_UseLightProbes = reader.ReadBoolean();
                reader.AlignStream();

                if (version >= 5) //5.0 and up
                {
                    var m_ReflectionProbeUsage = reader.ReadInt32();
                }

                var m_LightProbeAnchor = new PPtr<Transform>(reader); //5.0 and up m_ProbeAnchor
            }

            if (version >= (4, 3)) //4.3 and up
            {
                if (version == (4, 3) //4.3.x
                    || (version == (5, 6, 0) && version.IsBeta) //5.6.0bx
                    || (version == (5, 6, 0) && version.Build < 3)) //5.6.0f1 - 5.6.0f2
                {
                    var m_SortingLayer = reader.ReadInt16();
                }
                else
                {
                    var m_SortingLayerID = reader.ReadInt32();
                }

                if (version > (5, 6, 0) || (version >= (5, 6, 0, 3) && !version.IsBeta)) //5.6.0f3 and up
                {
                    var m_SortingLayer = reader.ReadInt16();
                }

                var m_SortingOrder = reader.ReadInt16();
                reader.AlignStream();
            }

            if (version >= (6000, 3)) //6000.3 and up
            {
                var m_MaskInteraction = reader.ReadInt32();
            }
        }
    }
}
