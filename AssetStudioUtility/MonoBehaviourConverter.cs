using System.Collections.Generic;

namespace AssetStudio
{
    public static class MonoBehaviourConverter
    {
        public static TypeTree ConvertToTypeTree(this MonoBehaviour m_MonoBehaviour, AssemblyLoader assemblyLoader)
        {
            var m_Type = new TypeTree();
            m_Type.m_Nodes = new List<TypeTreeNode>();
            var helper = new SerializedTypeHelper(m_MonoBehaviour.version);
            helper.AddMonoBehaviour(m_Type.m_Nodes, 0);
            if (m_MonoBehaviour.m_Script.TryGet(out var m_Script))
            {
                var typeDef = assemblyLoader.GetTypeDefinition(m_Script.m_AssemblyName, string.IsNullOrEmpty(m_Script.m_Namespace) ? m_Script.m_ClassName : $"{m_Script.m_Namespace}.{m_Script.m_ClassName}");
                if (typeDef != null)
                {
                    var typeDefinitionConverter = new TypeDefinitionConverter(typeDef, helper, 1);
                    m_Type.m_Nodes.AddRange(typeDefinitionConverter.ConvertToTypeTreeNodes());
                }
                else
                {
                    switch (m_Script.m_ClassName)
                    {
                        case "CubismModel":
                            helper.AddMonoCubismModel(m_Type.m_Nodes, 1);
                            break;
                        case "CubismMoc":
                            helper.AddMonoCubismMoc(m_Type.m_Nodes, 1);
                            break;
                        case "CubismFadeController":
                            helper.AddMonoCubismFadeController(m_Type.m_Nodes, 1);
                            break;
                        case "CubismFadeMotionList":
                            helper.AddMonoCubismFadeList(m_Type.m_Nodes, 1);
                            break;
                        case "CubismFadeMotionData":
                            helper.AddMonoCubismFadeData(m_Type.m_Nodes, m_MonoBehaviour.serializedType, 1);
                            break;
                        case "CubismExpressionController":
                            helper.AddMonoCubismExpressionController(m_Type.m_Nodes, 1);
                            break;
                        case "CubismExpressionList":
                            helper.AddMonoCubismExpressionList(m_Type.m_Nodes, 1);
                            break;
                        case "CubismExpressionData":
                            helper.AddMonoCubismExpressionData(m_Type.m_Nodes, 1);
                            break;
                        case "CubismDisplayInfoParameterName":
                            helper.AddMonoCubismDisplayInfo(m_Type.m_Nodes, 1);
                            break;
                        case "CubismDisplayInfoPartName":
                            helper.AddMonoCubismDisplayInfo(m_Type.m_Nodes, 1);
                            break;
                        case "CubismPosePart":
                            helper.AddMonoCubismPosePart(m_Type.m_Nodes, 1);
                            break;
                        case "CubismPhysicsController":
                            helper.AddMonoCubismPhysics(m_Type.m_Nodes, m_MonoBehaviour.serializedType, 1);
                            break;
                    }
                }
            }
            return m_Type;
        }
    }
}
