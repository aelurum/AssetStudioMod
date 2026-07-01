using AssetStudio;
using AssetStudio.Addons.CubismLive2D;
using System.Collections.Generic;
using System.Collections.Specialized;
using static CubismLive2DExtractor.CubismParsers;

namespace AssetStudioCLI
{
    public class L2DAssetsManager
    {
        public Dictionary<MonoBehaviour, CubismModel> ModelDict = new Dictionary<MonoBehaviour, CubismModel>();

        public L2DAssetsManager () { }

        public void BindCubismAssetsByGO(GameObject m_GameObject, AssemblyLoader assemblyLoader)
        {
            if (!TryGetCubismMoc(m_GameObject.CubismModel.CubismModelMono, assemblyLoader, out var mocMono))
                return;

            ModelDict[mocMono] = m_GameObject.CubismModel;
            BindAnimationClips(m_GameObject);
        }

        public void BindCubismAssetsByMono(MonoBehaviour m_MonoBehaviour, string m_ClassName)
        {
            if (!m_MonoBehaviour.m_GameObject.TryGet(out var m_GameObject))
                return;

            if (!TryGetModelGameObject(m_GameObject.m_Transform, out var modelGameObject))
                return;

            switch (m_ClassName)
            {
                case "CubismRenderer":
                    modelGameObject.CubismModel.RenderTextureList.Add(m_MonoBehaviour);
                    break;
                case "CubismDisplayInfoParameterName":
                    modelGameObject.CubismModel.ParamDisplayInfoList.Add(m_MonoBehaviour);
                    break;
                case "CubismDisplayInfoPartName":
                    modelGameObject.CubismModel.PartDisplayInfoList.Add(m_MonoBehaviour);
                    break;
                case "CubismPosePart":
                    modelGameObject.CubismModel.PosePartList.Add(m_MonoBehaviour);
                    break;
            }
        }

        public void BindCubismAssetsByMono(MonoBehaviour m_MonoBehaviour, CubismHelpers.CubismMonoBehaviourType type)
        {
            if (!m_MonoBehaviour.m_GameObject.TryGet(out var m_GameObject))
                return;

            if (!TryGetModelGameObject(m_GameObject.m_Transform, out var modelGameObject))
                return;

            switch (type)
            {
                case CubismHelpers.CubismMonoBehaviourType.RenderTexture:
                    modelGameObject.CubismModel.RenderTextureList.Add(m_MonoBehaviour);
                    break;
                case CubismHelpers.CubismMonoBehaviourType.PosePart:
                    modelGameObject.CubismModel.PosePartList.Add(m_MonoBehaviour);
                    break;
            }
        }

        private static bool TryGetCubismMoc(MonoBehaviour m_MonoBehaviour, AssemblyLoader assemblyLoader, out MonoBehaviour mocMono)
        {
            mocMono = null;
            var pptrDict = (OrderedDictionary)ParseMonoBehaviour(m_MonoBehaviour, CubismHelpers.CubismMonoBehaviourType.ModelPPtr, assemblyLoader)?["_moc"];
            if (pptrDict == null)
                return false;

            var mocPPtr = new PPtr<MonoBehaviour>
            {
                m_FileID = (int)pptrDict["m_FileID"],
                m_PathID = (long)pptrDict["m_PathID"],
                AssetsFile = m_MonoBehaviour.assetsFile
            };
            return mocPPtr.TryGet(out mocMono);
        }

        private static void BindAnimationClips(GameObject gameObject)
        {
            if (gameObject.m_Animator == null || gameObject.m_Animator.m_Controller.IsNull)
                return;

            if (!gameObject.m_Animator.m_Controller.TryGet(out var controller))
                return;

            AnimatorController animatorController;
            if (controller is AnimatorOverrideController overrideController)
            {
                if (!overrideController.m_Controller.TryGet(out animatorController))
                    return;
            }
            else
            {
                animatorController = (AnimatorController)controller;
            }

            foreach (var clipPptr in animatorController.m_AnimationClips)
            {
                if (clipPptr.TryGet(out var m_AnimationClip))
                {
                    gameObject.CubismModel.ClipMotionList.Add(m_AnimationClip);
                }
            }
        }

        private static bool TryGetModelGameObject(Transform m_Transform, out GameObject m_GameObject)
        {
            m_GameObject = null;
            if (m_Transform == null)
                return false;

            while (m_Transform.m_Father.TryGet(out var m_Father))
            {
                m_Transform = m_Father;
                if (m_Transform.m_GameObject.TryGet(out m_GameObject) && m_GameObject.CubismModel != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
