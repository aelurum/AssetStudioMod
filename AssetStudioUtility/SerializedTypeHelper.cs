using System.Collections.Generic;

namespace AssetStudio
{
    public class SerializedTypeHelper
    {
        private readonly UnityVersion version;
        private static readonly ulong[] supportedCubismPhysicsTemplates;

        static SerializedTypeHelper()
        {
            supportedCubismPhysicsTemplates = new ulong[]
            {
                4546329767881508904, //v1; default
                831634147207739970, 14438253902330556531, 6958134001336818095, //v2
                8727007011848355119, 5987673968064922522, 9047731275260549053, //v2; custom
            };
        }

        public SerializedTypeHelper(UnityVersion version)
        {
            this.version = version;
        }

        public void AddMonoBehaviour(List<TypeTreeNode> nodes, int indent)
        {
            nodes.Add(new TypeTreeNode("MonoBehaviour", "Base", indent, false));
            AddPPtr(nodes, "GameObject", "m_GameObject", indent + 1);
            nodes.Add(new TypeTreeNode("UInt8", "m_Enabled", indent + 1, true));
            AddPPtr(nodes, "MonoScript", "m_Script", indent + 1);
            AddString(nodes, "m_Name", indent + 1);
        }

        public void AddPPtr(List<TypeTreeNode> nodes, string type, string name, int indent)
        {
            nodes.Add(new TypeTreeNode($"PPtr<{type}>", name, indent, false));
            nodes.Add(new TypeTreeNode("int", "m_FileID", indent + 1, false));
            if (version >= 5) //5.0 and up
            {
                nodes.Add(new TypeTreeNode("SInt64", "m_PathID", indent + 1, false));
            }
            else
            {
                nodes.Add(new TypeTreeNode("int", "m_PathID", indent + 1, false));
            }
        }

        public void AddString(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("string", name, indent, false));
            nodes.Add(new TypeTreeNode("Array", "Array", indent + 1, true));
            nodes.Add(new TypeTreeNode("int", "size", indent + 2, false));
            nodes.Add(new TypeTreeNode("char", "data", indent + 2, false));
        }

        public void AddArray(List<TypeTreeNode> nodes, int indent)
        {
            nodes.Add(new TypeTreeNode("Array", "Array", indent, false));
            nodes.Add(new TypeTreeNode("int", "size", indent + 1, false));
        }

        public void AddAnimationCurve(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("AnimationCurve", name, indent, false));
            nodes.Add(new TypeTreeNode("vector", "m_Curve", indent + 1, false));
            AddArray(nodes, indent + 2); //TODO 2017 and up Array align but no effect 
            nodes.Add(new TypeTreeNode("Keyframe", "data", indent + 3, false));
            nodes.Add(new TypeTreeNode("float", "time", indent + 4, false));
            nodes.Add(new TypeTreeNode("float", "value", indent + 4, false));
            nodes.Add(new TypeTreeNode("float", "inSlope", indent + 4, false));
            nodes.Add(new TypeTreeNode("float", "outSlope", indent + 4, false));
            if (version >= 2018) //2018 and up
            {
                nodes.Add(new TypeTreeNode("int", "weightedMode", indent + 4, false));
                nodes.Add(new TypeTreeNode("float", "inWeight", indent + 4, false));
                nodes.Add(new TypeTreeNode("float", "outWeight", indent + 4, false));
            }
            nodes.Add(new TypeTreeNode("int", "m_PreInfinity", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "m_PostInfinity", indent + 1, false));
            if (version >= (5, 3)) //5.3 and up
            {
                nodes.Add(new TypeTreeNode("int", "m_RotationOrder", indent + 1, false));
            }
        }

        public void AddGradient(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("Gradient", name, indent, false));
            if (version >= (5, 6)) //5.6 and up
            {
                AddColorRGBA(nodes, "key0", indent + 1);
                AddColorRGBA(nodes, "key1", indent + 1);
                AddColorRGBA(nodes, "key2", indent + 1);
                AddColorRGBA(nodes, "key3", indent + 1);
                AddColorRGBA(nodes, "key4", indent + 1);
                AddColorRGBA(nodes, "key5", indent + 1);
                AddColorRGBA(nodes, "key6", indent + 1);
                AddColorRGBA(nodes, "key7", indent + 1);
            }
            else
            {
                AddColor32(nodes, "key0", indent + 1);
                AddColor32(nodes, "key1", indent + 1);
                AddColor32(nodes, "key2", indent + 1);
                AddColor32(nodes, "key3", indent + 1);
                AddColor32(nodes, "key4", indent + 1);
                AddColor32(nodes, "key5", indent + 1);
                AddColor32(nodes, "key6", indent + 1);
                AddColor32(nodes, "key7", indent + 1);
            }
            nodes.Add(new TypeTreeNode("UInt16", "ctime0", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime1", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime2", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime3", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime4", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime5", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime6", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "ctime7", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime0", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime1", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime2", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime3", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime4", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime5", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime6", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt16", "atime7", indent + 1, false));
            if (version >= (2022, 2)) //2022.2 and up
            {
                nodes.Add(new TypeTreeNode("UInt8", "m_Mode", indent + 1, false));
                nodes.Add(new TypeTreeNode("SInt8", "m_ColorSpace", indent + 1, false));
            }
            else if (version >= (5, 5)) //5.5 and up
            {
                nodes.Add(new TypeTreeNode("int", "m_Mode", indent + 1, false));
            }
            nodes.Add(new TypeTreeNode("UInt8", "m_NumColorKeys", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt8", "m_NumAlphaKeys", indent + 1, true));
        }

        public void AddGUIStyle(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("GUIStyle", name, indent, false));
            AddString(nodes, "m_Name", indent + 1);
            AddGUIStyleState(nodes, "m_Normal", indent + 1);
            AddGUIStyleState(nodes, "m_Hover", indent + 1);
            AddGUIStyleState(nodes, "m_Active", indent + 1);
            AddGUIStyleState(nodes, "m_Focused", indent + 1);
            AddGUIStyleState(nodes, "m_OnNormal", indent + 1);
            AddGUIStyleState(nodes, "m_OnHover", indent + 1);
            AddGUIStyleState(nodes, "m_OnActive", indent + 1);
            AddGUIStyleState(nodes, "m_OnFocused", indent + 1);
            AddRectOffset(nodes, "m_Border", indent + 1);
            if (version >= 4) //4 and up
            {
                AddRectOffset(nodes, "m_Margin", indent + 1);
                AddRectOffset(nodes, "m_Padding", indent + 1);
            }
            else
            {
                AddRectOffset(nodes, "m_Padding", indent + 1);
                AddRectOffset(nodes, "m_Margin", indent + 1);
            }
            AddRectOffset(nodes, "m_Overflow", indent + 1);
            AddPPtr(nodes, "Font", "m_Font", indent + 1);
            if (version >= 4) //4 and up
            {
                nodes.Add(new TypeTreeNode("int", "m_FontSize", indent + 1, false));
                nodes.Add(new TypeTreeNode("int", "m_FontStyle", indent + 1, false));
                nodes.Add(new TypeTreeNode("int", "m_Alignment", indent + 1, false));
                nodes.Add(new TypeTreeNode("bool", "m_WordWrap", indent + 1, false));
                nodes.Add(new TypeTreeNode("bool", "m_RichText", indent + 1, true));
                nodes.Add(new TypeTreeNode("int", "m_TextClipping", indent + 1, false));
                nodes.Add(new TypeTreeNode("int", "m_ImagePosition", indent + 1, false));
                AddVector2f(nodes, "m_ContentOffset", indent + 1);
                nodes.Add(new TypeTreeNode("float", "m_FixedWidth", indent + 1, false));
                nodes.Add(new TypeTreeNode("float", "m_FixedHeight", indent + 1, false));
                nodes.Add(new TypeTreeNode("bool", "m_StretchWidth", indent + 1, false));
                nodes.Add(new TypeTreeNode("bool", "m_StretchHeight", indent + 1, true));
            }
            else
            {
                nodes.Add(new TypeTreeNode("int", "m_ImagePosition", indent + 1, false));
                nodes.Add(new TypeTreeNode("int", "m_Alignment", indent + 1, false));
                nodes.Add(new TypeTreeNode("bool", "m_WordWrap", indent + 1, true));
                nodes.Add(new TypeTreeNode("int", "m_TextClipping", indent + 1, false));
                AddVector2f(nodes, "m_ContentOffset", indent + 1);
                AddVector2f(nodes, "m_ClipOffset", indent + 1);
                nodes.Add(new TypeTreeNode("float", "m_FixedWidth", indent + 1, false));
                nodes.Add(new TypeTreeNode("float", "m_FixedHeight", indent + 1, false));
                if (version >= 3) //3 and up
                {
                    nodes.Add(new TypeTreeNode("int", "m_FontSize", indent + 1, false));
                    nodes.Add(new TypeTreeNode("int", "m_FontStyle", indent + 1, false));
                }
                nodes.Add(new TypeTreeNode("bool", "m_StretchWidth", indent + 1, true));
                nodes.Add(new TypeTreeNode("bool", "m_StretchHeight", indent + 1, true));
            }
        }

        public void AddGUIStyleState(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("GUIStyleState", name, indent, false));
            AddPPtr(nodes, "Texture2D", "m_Background", indent + 1);
            AddColorRGBA(nodes, "m_TextColor", indent + 1);
        }

        public void AddVector2f(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("Vector2f", name, indent, false));
            nodes.Add(new TypeTreeNode("float", "x", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "y", indent + 1, false));
        }

        public void AddRectOffset(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("RectOffset", name, indent, false));
            nodes.Add(new TypeTreeNode("int", "m_Left", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "m_Right", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "m_Top", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "m_Bottom", indent + 1, false));
        }

        public void AddColorRGBA(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("ColorRGBA", name, indent, false));
            nodes.Add(new TypeTreeNode("float", "r", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "g", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "b", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "a", indent + 1, false));
        }

        public void AddColor32(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("ColorRGBA", name, indent, false));
            nodes.Add(new TypeTreeNode("unsigned int", "rgba", indent + 1, false));
        }

        public void AddMatrix4x4(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("Matrix4x4f", name, indent, false));
            nodes.Add(new TypeTreeNode("float", "e00", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e01", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e02", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e03", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e10", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e11", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e12", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e13", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e20", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e21", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e22", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e23", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e30", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e31", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e32", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "e33", indent + 1, false));
        }

        public void AddSphericalHarmonicsL2(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("SphericalHarmonicsL2", name, indent, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 0]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 1]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 2]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 3]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 4]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 5]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 6]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 7]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 8]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[ 9]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[10]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[11]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[12]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[13]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[14]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[15]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[16]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[17]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[18]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[19]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[20]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[21]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[22]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[23]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[24]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[25]", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "sh[26]", indent + 1, false));
        }

        public void AddPropertyName(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("PropertyName", name, indent, false));
            AddString(nodes, "id", indent + 1);
        }

        #region CubismLive2D
        public void AddMonoCubismModel(List<TypeTreeNode> nodes, int indent)
        {
            AddPPtr(nodes, "CubismMoc", "_moc", indent);
        }

        public void AddMonoCubismMoc(List<TypeTreeNode> nodes, int indent)
        {
            nodes.Add(new TypeTreeNode("vector", "_bytes", indent, align: true));
            AddArray(nodes, indent + 2);
            nodes.Add(new TypeTreeNode("UInt8", "data", indent + 2, false));
        }

        public void AddMonoCubismPosePart(List<TypeTreeNode> nodes, int indent)
        {
            nodes.Add(new TypeTreeNode("int", "GroupIndex", indent, false));
            nodes.Add(new TypeTreeNode("int", "PartIndex", indent, false));
            nodes.Add(new TypeTreeNode("vector", "Link", indent, align: false));
            AddArray(nodes, indent + 2);
            AddString(nodes, "data", indent + 2);
        }

        public void AddMonoCubismDisplayInfo(List<TypeTreeNode> nodes, int indent)
        {
            AddString(nodes, "Name", indent);
            AddString(nodes, "DisplayName", indent);
        }

        public void AddMonoCubismFadeController(List<TypeTreeNode> nodes, int indent)
        {
            AddPPtr(nodes, "CubismFadeMotionList", "CubismFadeMotionList", indent);
        }

        public void AddMonoCubismFadeList(List<TypeTreeNode> nodes, int indent)
        {
            nodes.Add(new TypeTreeNode("vector", "MotionInstanceIds", indent, align: false));
            AddArray(nodes, indent + 2);
            nodes.Add(new TypeTreeNode("int", "data", indent + 2, align: false));
            nodes.Add(new TypeTreeNode("vector", "CubismFadeMotionObjects", indent, align: false));
            AddArray(nodes, indent + 2);
            AddPPtr(nodes, "CubismFadeMotionData", "data", indent + 2);
        }

        public void AddMonoCubismFadeData(List<TypeTreeNode> nodes, SerializedType type, int indent)
        {
            AddString(nodes, "MotionName", indent);
            if (type.Match(11957159347687761472))
            {
                nodes.Add(new TypeTreeNode("float", "ModelFadeInTime", indent, false));
                nodes.Add(new TypeTreeNode("float", "ModelFadeOutTime", indent, false));
            }
            nodes.Add(new TypeTreeNode("float", "FadeInTime", indent, false));
            nodes.Add(new TypeTreeNode("float", "FadeOutTime", indent, false));
            nodes.Add(new TypeTreeNode("vector", "ParameterIds", indent, align: false));
            AddArray(nodes, indent + 2);
            AddString(nodes, "data", indent + 2);
            nodes.Add(new TypeTreeNode("vector", "ParameterCurves", indent, align: false));
            AddArray(nodes, indent + 2);
            AddAnimationCurve(nodes, "data", indent + 2);
            nodes.Add(new TypeTreeNode("vector", "ParameterFadeInTimes", indent, align: false));
            AddArray(nodes, indent + 2);
            nodes.Add(new TypeTreeNode("float", "data", indent + 2, align: false));
            nodes.Add(new TypeTreeNode("vector", "ParameterFadeOutTimes", indent, align: false));
            AddArray(nodes, indent + 2);
            nodes.Add(new TypeTreeNode("float", "data", indent + 2, align: false));
            nodes.Add(new TypeTreeNode("float", "MotionLength", indent, align: false));
        }

        public void AddMonoCubismExpressionController(List<TypeTreeNode> nodes, int indent)
        {
            AddPPtr(nodes, "CubismExpressionList", "ExpressionsList", indent);
            nodes.Add(new TypeTreeNode("int", "CurrentExpressionIndex", indent, false));
        }

        public void AddMonoCubismExpressionList(List<TypeTreeNode> nodes, int indent)
        {
            nodes.Add(new TypeTreeNode("vector", "CubismExpressionObjects", indent, align: false));
            AddArray(nodes, indent + 2);
            AddPPtr(nodes, "CubismExpressionData", "data", indent + 2);
        }

        private void AddMonoCubismExpressionParameter(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("SerializableExpressionParameter", name, indent, false));
            AddString(nodes, "Id", indent + 1);
            nodes.Add(new TypeTreeNode("float", "Value", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "Blend", indent + 1, false));
        }

        public void AddMonoCubismExpressionData(List<TypeTreeNode> nodes, int indent)
        {
            AddString(nodes, "Type", indent);
            nodes.Add(new TypeTreeNode("float", "FadeInTime", indent, false));
            nodes.Add(new TypeTreeNode("float", "FadeOutTime", indent, false));
            nodes.Add(new TypeTreeNode("SerializableExpressionParameter", "Parameters", indent, align: false));
            AddArray(nodes, indent + 2);
            AddMonoCubismExpressionParameter(nodes, "data", indent + 2);
        }

        private void AddMonoCubismPhysicsInput(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("CubismPhysicsInput", name, indent, false));
            AddString(nodes, "SourceId", indent + 1);
            AddVector2f(nodes, "ScaleOfTranslation", indent + 1);
            nodes.Add(new TypeTreeNode("float", "AngleScale", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Weight", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "SourceComponent", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt8", "IsInverted", indent + 1, true));
        }

        private void AddMonoCubismPhysicsOutput(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("CubismPhysicsOutput", name, indent, false));
            AddString(nodes, "DestinationId", indent + 1);
            nodes.Add(new TypeTreeNode("int", "ParticleIndex", indent + 1, false));
            AddVector2f(nodes, "TranslationScale", indent + 1);
            nodes.Add(new TypeTreeNode("float", "AngleScale", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Weight", indent + 1, false));
            nodes.Add(new TypeTreeNode("int", "SourceComponent", indent + 1, false));
            nodes.Add(new TypeTreeNode("UInt8", "IsInverted", indent + 1, true));
        }

        private void AddMonoCubismPhysicsParticle(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("CubismPhysicsParticle", name, indent, false));
            AddVector2f(nodes, "InitialPosition", indent + 1);
            nodes.Add(new TypeTreeNode("float", "Mobility", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Delay", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Acceleration", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Radius", indent + 1, false));
        }

        private void AddMonoCubismPhysicsNormalizationTuplet(List<TypeTreeNode> nodes, string name, int indent)
        {
            nodes.Add(new TypeTreeNode("CubismPhysicsNormalizationTuplet", name, indent, false));
            nodes.Add(new TypeTreeNode("float", "Maximum", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Minimum", indent + 1, false));
            nodes.Add(new TypeTreeNode("float", "Default", indent + 1, false));
        }

        private void AddMonoCubismPhysicsSubRig(List<TypeTreeNode> nodes, string name, SerializedType type, int indent)
        {
            nodes.Add(new TypeTreeNode("CubismPhysicsSubRig", name, indent, false));
            if (!type.Match(4546329767881508904, 8727007011848355119, 5987673968064922522)) //v2+
            {
                AddString(nodes, "Name", indent + 1);
            }
            nodes.Add(new TypeTreeNode("CubismPhysicsInput", "Input", indent + 1, align: false));
            AddArray(nodes, indent + 2);
            AddMonoCubismPhysicsInput(nodes, "data", indent + 3);
            nodes.Add(new TypeTreeNode("CubismPhysicsOutput", "Output", indent + 1, align: false));
            AddArray(nodes, indent + 2);
            AddMonoCubismPhysicsOutput(nodes, "data", indent + 3);
            nodes.Add(new TypeTreeNode("CubismPhysicsParticle", "Particles", indent + 1, align: false));
            AddArray(nodes, indent + 2);
            AddMonoCubismPhysicsParticle(nodes, "data", indent + 3);
            nodes.Add(new TypeTreeNode("CubismPhysicsNormalization", "Normalization", indent + 1, align: false));
            AddMonoCubismPhysicsNormalizationTuplet(nodes, "Position", indent + 2);
            AddMonoCubismPhysicsNormalizationTuplet(nodes, "Angle", indent + 2);
        }

        public void AddMonoCubismPhysics(List<TypeTreeNode> nodes, SerializedType type, int indent)
        {
            if (!type.Match(supportedCubismPhysicsTemplates))
                return;

            if (type.Match(9047731275260549053))
            {
                nodes.Add(new TypeTreeNode("UInt8", "EnablePhysics", indent, true));
            }
            nodes.Add(new TypeTreeNode("CubismPhysicsRig", "_rig", indent, false));
            nodes.Add(new TypeTreeNode("CubismPhysicsSubRig", "SubRigs", indent + 1, false));
            AddArray(nodes, indent + 2);
            AddMonoCubismPhysicsSubRig(nodes, "data", type, indent + 3);
            if (!type.Match(4546329767881508904)) //v2+
            {
                AddVector2f(nodes, "Gravity", indent + 1);
                AddVector2f(nodes, "Wind", indent + 1);
                if (!type.Match(5987673968064922522))
                {
                    nodes.Add(new TypeTreeNode("float", "Fps", indent + 1, false));
                }
            }
            if (type.Match(5987673968064922522))
            {
                nodes.Add(new TypeTreeNode("UInt8", "IsUpdate", indent + 1, true));
            }
        }
        #endregion
    }
}
