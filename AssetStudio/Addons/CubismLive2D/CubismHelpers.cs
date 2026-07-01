namespace AssetStudio.Addons.CubismLive2D
{
    public static class CubismHelpers
    {
        public enum CubismMonoBehaviourType
        {
            Unknown = -1,
            FadeController,
            FadeMotionList,
            FadeMotion,
            ExpressionController,
            ExpressionList,
            Expression,
            Physics,
            DisplayInfo,
            PosePart,
            ModelPPtr,
            Moc,
            RenderTexture,
        }

        public static bool TryGetCubismMonoBehaviourType(MonoBehaviour m_MonoBehaviour, out CubismMonoBehaviourType type)
        {
            type = CubismMonoBehaviourType.Unknown;
            var orderedDict = m_MonoBehaviour.ToType();
            if (orderedDict == null)
                return false;

            foreach (var key in orderedDict.Keys)
            {
                var fieldName = ((string)key).ToLower();
                switch (fieldName)
                {
                    case "cubismfademotionlist":
                        type = CubismMonoBehaviourType.FadeController;
                        break;
                    case "cubismfademotionobjects":
                        type = CubismMonoBehaviourType.FadeMotionList;
                        break;
                    case "parameterids":
                        type = CubismMonoBehaviourType.FadeMotion;
                        break;
                    case "expressionslist":
                        type = CubismMonoBehaviourType.ExpressionController;
                        break;
                    case "cubismexpressionobjects":
                        type = CubismMonoBehaviourType.ExpressionList;
                        break;
                    case "parameters":
                        type = CubismMonoBehaviourType.Expression;
                        break;
                    case "_rig":
                        type = CubismMonoBehaviourType.Physics;
                        break;
                    case "name":
                        type = CubismMonoBehaviourType.DisplayInfo;
                        break;
                    case "groupindex":
                        type = CubismMonoBehaviourType.PosePart;
                        break;
                    case "_moc":
                        type = CubismMonoBehaviourType.ModelPPtr;
                        break;
                    case "_bytes":
                        type = CubismMonoBehaviourType.Moc;
                        break;
                    case "_maintexture":
                        type = CubismMonoBehaviourType.RenderTexture;
                        break;
                }

                if (type != CubismMonoBehaviourType.Unknown)
                    return true;
            }

            return false;
        }
    }
}
