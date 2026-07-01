using System.Collections.Generic;

namespace AssetStudio.Addons.CubismLive2D
{
    public class CubismModel
    {
        public string Name { get; set; }
        public string Container { get; set; }
        public MonoBehaviour CubismModelMono { get; set; }
        public MonoBehaviour PhysicsController { get; set; }
        public MonoBehaviour FadeController { get; set; }
        public MonoBehaviour ExpressionController { get; set; }
        public List<MonoBehaviour> RenderTextureList { get; set; }
        public List<MonoBehaviour> ParamDisplayInfoList { get; set; }
        public List<MonoBehaviour> PartDisplayInfoList { get; set; }
        public List<MonoBehaviour> PosePartList { get; set; }
        public List<AnimationClip> ClipMotionList { get; set; }
        public GameObject ModelGameObject { get; set; }

        public CubismModel(GameObject m_GameObject)
        {
            Name = m_GameObject.m_Name;
            ModelGameObject = m_GameObject;
            RenderTextureList = new List<MonoBehaviour>();
            ParamDisplayInfoList = new List<MonoBehaviour>();
            PartDisplayInfoList = new List<MonoBehaviour>();
            PosePartList = new List<MonoBehaviour>();
            ClipMotionList = new List<AnimationClip>();
        }

        public CubismModel(MonoBehaviour mocMono)
        {
            Name = mocMono.m_Name;
            ModelGameObject = null;
            RenderTextureList = new List<MonoBehaviour>();
            ParamDisplayInfoList = new List<MonoBehaviour>();
            PartDisplayInfoList = new List<MonoBehaviour>();
            PosePartList = new List<MonoBehaviour>();
            ClipMotionList = new List<AnimationClip>();
        }
    }
}
