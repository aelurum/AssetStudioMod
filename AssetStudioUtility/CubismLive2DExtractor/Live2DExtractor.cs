////
// Based on UnityLive2DExtractor by Perfare
// https://github.com/Perfare/UnityLive2DExtractor
////

using AssetStudio;
using AssetStudio.Addons.CubismLive2D;
using CubismLive2DExtractor.CubismUnityClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static AssetStudio.Addons.CubismLive2D.CubismHelpers;
using static CubismLive2DExtractor.CubismParsers;
using Object = AssetStudio.Object;

namespace CubismLive2DExtractor
{
    public sealed class Live2DExtractor
    {
        public static AssemblyLoader Assembly { get; set; }
        private CubismModel Model { get; set; }
        private Live2DExtractorOptions Options { get; }
        private List<MonoBehaviour> FadeMotions { get; set; }
        private List<AnimationClip> AnimationClips { get; set; }
        private List<MonoBehaviour> Expressions { get; set; }
        private List<MonoBehaviour> ParametersCdi { get; set; }
        private List<MonoBehaviour> PartsCdi { get; set; }
        private List<MonoBehaviour> PoseParts { get; set; }
        private List<Texture2D> Texture2Ds { get; set; }
        private MonoBehaviour PhysicsMono { get; set; }
        private MonoBehaviour FadeMotionLst { get; set; }
        private MonoBehaviour ExpressionLst { get; set; }
        private HashSet<string> ParameterNames { get; set; }
        private HashSet<string> PartNames { get; set; }
        private HashSet<string> EyeBlinkParameters { get; set; }
        private HashSet<string> LipSyncParameters { get; set; }

        public Live2DExtractor(List<Object> modelAssetsList, CubismModel model, Live2DExtractorOptions l2dOptions)
        {
            Options = l2dOptions;
            Expressions = new List<MonoBehaviour>();
            FadeMotions = Options.SelectedFadeMotions ?? new List<MonoBehaviour>();
            AnimationClips = Options.SelectedClipMotions ?? new List<AnimationClip>();
            FadeMotionLst = Options.SelectedFadeMotionList;
            Texture2Ds = new List<Texture2D>();
            EyeBlinkParameters = new HashSet<string>();
            LipSyncParameters = new HashSet<string>();
            ParameterNames = new HashSet<string>();
            PartNames = new HashSet<string>();
            ParametersCdi = new List<MonoBehaviour>();
            PartsCdi = new List<MonoBehaviour>();
            PoseParts = new List<MonoBehaviour>();
            var renderTextureSet = new HashSet<Texture2D>();
            var isRenderReadable = true;
            var searchRenderTextures = true;
            var searchModelParamCdi = true;
            var searchModelPartCdi = true;
            var searchPoseParts = true;
            var searchFadeMotions =
                Options.SelectedClipMotions == null
                && Options.SelectedFadeMotions == null
                && Options.SelectedFadeMotionList == null;

            Logger.Debug("Sorting model assets..");

            if (model != null)
            {
                Model = model;
                PhysicsMono = Model.PhysicsController;
                if (searchFadeMotions && TryGetFadeList(Model.FadeController, out var fadeMono))
                {
                    FadeMotionLst = fadeMono;
                }
                if (TryGetExpressionList(Model.ExpressionController, out var expressionMono))
                {
                    ExpressionLst = expressionMono;
                }
                if (Model.RenderTextureList.Count > 0)
                {
                    var renderList = Model.RenderTextureList;
                    foreach (var renderMono in renderList)
                    {
                        isRenderReadable = TryGetRenderTexture(renderMono, out var tex);
                        if (!isRenderReadable)
                            break;
                        renderTextureSet.Add(tex);
                    }
                    searchRenderTextures = renderTextureSet.Count == 0;
                }
                if (Model.ParamDisplayInfoList.Count > 0)
                {
                    ParametersCdi = Model.ParamDisplayInfoList;
                    searchModelParamCdi = false;
                }
                if (Model.PartDisplayInfoList.Count > 0)
                {
                    PartsCdi = Model.PartDisplayInfoList;
                    searchModelPartCdi = false;
                }
                if (Model.PosePartList.Count > 0)
                {
                    PoseParts = Model.PosePartList;
                    searchPoseParts = false;
                }
                if (Model.ClipMotionList.Count > 0 && Options.SelectedClipMotions == null)
                {
                    AnimationClips.AddRange(Model.ClipMotionList);
                }
            }
            foreach (var asset in modelAssetsList)
            {
                switch (asset)
                {
                    case MonoBehaviour m_MonoBehaviour:
                        if (m_MonoBehaviour.m_Script.TryGet(out var m_Script))
                        {
                            switch (m_Script.m_ClassName)
                            {
                                case "CubismPhysicsController":
                                    if (PhysicsMono == null)
                                        PhysicsMono = m_MonoBehaviour;
                                    break;
                                case "CubismExpressionData":
                                    if (ExpressionLst == null)
                                        Expressions.Add(m_MonoBehaviour);
                                    break;
                                case "CubismFadeMotionData":
                                    if (searchFadeMotions)
                                    {
                                        FadeMotions.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case "CubismFadeMotionList":
                                    if (searchFadeMotions)
                                    {
                                        FadeMotionLst = m_MonoBehaviour;
                                    }
                                    break;
                                case "CubismEyeBlinkParameter":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var blinkGameObject))
                                    {
                                        EyeBlinkParameters.Add(blinkGameObject.m_Name);
                                    }
                                    break;
                                case "CubismMouthParameter":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var mouthGameObject))
                                    {
                                        LipSyncParameters.Add(mouthGameObject.m_Name);
                                    }
                                    break;
                                case "CubismParameter":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var paramGameObject))
                                    {
                                        ParameterNames.Add(paramGameObject.m_Name);
                                    }
                                    break;
                                case "CubismPart":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var partGameObject))
                                    {
                                        PartNames.Add(partGameObject.m_Name);
                                    }
                                    break;
                                case "CubismDisplayInfoParameterName":
                                    if (searchModelParamCdi && m_MonoBehaviour.m_GameObject.TryGet(out _))
                                    {
                                        ParametersCdi.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case "CubismDisplayInfoPartName":
                                    if (searchModelPartCdi && m_MonoBehaviour.m_GameObject.TryGet(out _))
                                    {
                                        PartsCdi.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case "CubismPosePart":
                                    if (searchPoseParts && m_MonoBehaviour.m_GameObject.TryGet(out _))
                                    {
                                        PoseParts.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case "CubismRenderer":
                                    if (searchRenderTextures && isRenderReadable)
                                    {
                                        isRenderReadable = TryGetRenderTexture(m_MonoBehaviour, out var renderTex);
                                        if (isRenderReadable)
                                            renderTextureSet.Add(renderTex);
                                    }
                                    break;
                            }
                        }
                        else if (TryGetCubismMonoBehaviourType(m_MonoBehaviour, out var type))
                        {
                            switch (type)
                            {
                                case CubismMonoBehaviourType.Physics:
                                    if (PhysicsMono == null)
                                        PhysicsMono = m_MonoBehaviour;
                                    break;
                                case CubismMonoBehaviourType.Expression:
                                    if (ExpressionLst == null)
                                        Expressions.Add(m_MonoBehaviour);
                                    break;
                                case CubismMonoBehaviourType.ExpressionList:
                                    if (ExpressionLst == null)
                                        ExpressionLst = m_MonoBehaviour;
                                    break;
                                case CubismMonoBehaviourType.FadeMotion:
                                    if (searchFadeMotions)
                                    {
                                        FadeMotions.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case CubismMonoBehaviourType.FadeMotionList:
                                    if (searchFadeMotions)
                                    {
                                        FadeMotionLst = m_MonoBehaviour;
                                    }
                                    break;
                                case CubismMonoBehaviourType.PosePart:
                                    if (searchPoseParts && m_MonoBehaviour.m_GameObject.TryGet(out _))
                                    {
                                        PoseParts.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case CubismMonoBehaviourType.RenderTexture:
                                    if (searchRenderTextures && isRenderReadable)
                                    {
                                        isRenderReadable = TryGetRenderTexture(m_MonoBehaviour, out var renderTex);
                                        if (isRenderReadable)
                                            renderTextureSet.Add(renderTex);
                                    }
                                    break;
                            }
                        }
                        break;
                    case AnimationClip m_AnimationClip:
                        if (Options.SelectedClipMotions == null)
                        {
                            AnimationClips.Add(m_AnimationClip);
                        }
                        break;
                    case Texture2D m_Texture2D:
                        Texture2Ds.Add(m_Texture2D);
                        break;
                }
            }

            if (renderTextureSet.Count > 0)
            {
                Texture2Ds = renderTextureSet.ToList();
            }
            else if (Options.SearchByFilename)
            {
                Texture2Ds = Texture2Ds.Where(x => x.m_Name.ToLower().Contains("texture")).ToList();
            }

            if (AnimationClips.Count > 0)
            {
                AnimationClips = AnimationClips.Distinct().ToList();
            }
        }

        public void ExtractCubismModel(MonoBehaviour mocMono, string destPath, int parallelTaskCount = 1)
        {
            var modelName = Model?.Name ?? destPath.Split('/', '\\').Last();
            destPath += Path.DirectorySeparatorChar;
            Directory.CreateDirectory(destPath);

            #region moc3
            using (var cubismMoc = new CubismMoc(mocMono))
            {
                var sb = new StringBuilder();
                sb.AppendLine("Model Stats:");
                sb.AppendLine($"SDK Version: {cubismMoc.VersionDescription}");
                if (cubismMoc.Version > 0)
                {
                    sb.AppendLine($"Canvas Width: {cubismMoc.CanvasWidth}");
                    sb.AppendLine($"Canvas Height: {cubismMoc.CanvasHeight}");
                    sb.AppendLine($"Center X: {cubismMoc.CentralPosX}");
                    sb.AppendLine($"Center Y: {cubismMoc.CentralPosY}");
                    sb.AppendLine($"Pixel Per Unit: {cubismMoc.PixelPerUnit}");
                    sb.AppendLine($"Part Count: {cubismMoc.PartCount}");
                    sb.AppendLine($"Parameter Count: {cubismMoc.ParamCount}\n");
                    sb.AppendLine($"Bound AnimationClips: {Model?.ClipMotionList.Count}");
                    sb.AppendLine($"Bound ParamDisplayInfoList: {Model?.ParamDisplayInfoList.Count}");
                    sb.AppendLine($"Bound PartDisplayInfoList: {Model?.PartDisplayInfoList.Count}");
                    sb.AppendLine($"Bound PosePartList: {Model?.PosePartList.Count}");
                    Logger.Debug(sb.ToString());

                    ParameterNames = cubismMoc.ParamNames;
                    PartNames = cubismMoc.PartNames;
                }
                cubismMoc.SaveMoc3($"{destPath}{modelName}.moc3");
            }
            #endregion

            #region textures
            var textures = new SortedSet<string>();
            var destTexturePath = Path.Combine(destPath, "textures") + Path.DirectorySeparatorChar;

            if (Texture2Ds.Count == 0)
            {
                Logger.Warning($"No textures found for \"{modelName}\" model");
            }
            else
            {
                Directory.CreateDirectory(destTexturePath);
            }

            var textureBag = new ConcurrentBag<string>();
            var savePathHash = new ConcurrentDictionary<string, bool>();
            Parallel.ForEach(Texture2Ds, new ParallelOptions { MaxDegreeOfParallelism = parallelTaskCount }, texture2D =>
            {
                var savePath = $"{destTexturePath}{texture2D.m_Name}.png";
                if (!savePathHash.TryAdd(savePath, true))
                {
                    savePath = $"{destTexturePath}{texture2D.m_Name}_#{texture2D.GetHashCode()}.png";
                    if (!savePathHash.TryAdd(savePath, true))
                        return;
                }

                if (texture2D.TryConvertToImage(flip: true, out var image))
                {
                    using (var file = File.OpenWrite(savePath))
                    {
                        using (image)
                        {
                            image.WriteToStream(file, ImageFormat.Png);
                        }
                    }
                    textureBag.Add($"textures/{texture2D.m_Name}.png");
                }
            });
            textures.UnionWith(textureBag);
            #endregion

            #region cdi3.json
            var isCdiExported = false;
            if (ParametersCdi.Count > 0 || PartsCdi.Count > 0)
            {
                try
                {
                    isCdiExported = ExportCdiJson(destPath, modelName);
                }
                catch (Exception e)
                {
                    Logger.Warning($"An error occurred while exporting cdi3.json\n{e}");
                }
            }
            #endregion

            #region motion3.json
            var motions = new SortedDictionary<string, JsonNode>();
            var destMotionPath = Path.Combine(destPath, "motions") + Path.DirectorySeparatorChar;
            var motionFps = 0f;

            if (Options.MotionMode == Live2DMotionMode.MonoBehaviour) //Fade motions from MonoBehaviour
            {
                if (FadeMotionLst != null) //Fade motions from fadeMotionList
                {
                    Logger.Debug("Parsing fade motion list..");
                    var fadeMotionLstDict = ParseMonoBehaviour(FadeMotionLst, CubismMonoBehaviourType.FadeMotionList, Assembly);
                    if (fadeMotionLstDict != null)
                    {
                        var cubismFadeList = JsonSerializer.Deserialize<CubismFadeMotionList>(JsonSerializer.SerializeToUtf8Bytes(fadeMotionLstDict, CubismJsonOptions), CubismJsonOptions);
                        var fadeMotionAssetSet = new HashSet<MonoBehaviour>();
                        foreach (var motionPPtr in cubismFadeList.CubismFadeMotionObjects)
                        {
                            if (motionPPtr.TryGet<MonoBehaviour>(out var fadeMono, FadeMotionLst.assetsFile))
                            {
                                fadeMotionAssetSet.Add(fadeMono);
                            }
                        }

                        if (fadeMotionAssetSet.Count > 0)
                        {
                            FadeMotions = fadeMotionAssetSet.ToList();
                            Logger.Debug($"\"{FadeMotionLst.m_Name}\": found {fadeMotionAssetSet.Count} motion(s)");
                        }
                    }
                }

                if (FadeMotions.Count > 0)
                {
                    Logger.Debug("Motion export method: MonoBehaviour (Fade motion)");
                    ExportFadeMotions(destMotionPath, motions, ref motionFps);
                }
            }

            if (motions.Count == 0) //motions from AnimationClip
            {
                CubismMotion3Converter converter;
                var exportMethod = "AnimationClip";
                switch (Options.MotionMode)
                {
                    case Live2DMotionMode.AnimationClipV1 when Model?.ModelGameObject != null:
                        exportMethod += "V1";
                        converter = new CubismMotion3Converter(Model.ModelGameObject, AnimationClips);
                        break;
                    default: //AnimationClipV2
                        exportMethod += "V2";
                        if (Options.MotionMode == Live2DMotionMode.MonoBehaviour)
                        {
                            exportMethod = FadeMotions.Count > 0
                                ? exportMethod + " (unable to export motions using Fade motion method)"
                                : exportMethod + " (no Fade motions found)";
                        }
                        converter = new CubismMotion3Converter(AnimationClips, PartNames, ParameterNames);
                        break;
                }
                Logger.Debug($"Motion export method: {exportMethod}");

                ExportClipMotions(destMotionPath, converter, motions, ref motionFps);
            }

            if (motions.Count == 0)
            {
                Logger.Warning($"No exportable motions found for \"{modelName}\" model");
            }
            else
            {
                Logger.Info($"Exported {motions.Count} motion(s)");
            }
            #endregion

            #region exp3.json
            var expressions = new JsonArray();
            var destExpressionPath = Path.Combine(destPath, "expressions") + Path.DirectorySeparatorChar;

            if (ExpressionLst != null) //Expressions from Expression List
            {
                Logger.Debug("Parsing expression list..");
                var expLstDict = ParseMonoBehaviour(ExpressionLst, CubismMonoBehaviourType.ExpressionList, Assembly);
                if (expLstDict != null)
                {
                    var cubismExpList = JsonSerializer.Deserialize<CubismExpressionList>(JsonSerializer.SerializeToUtf8Bytes(expLstDict, CubismJsonOptions), CubismJsonOptions);
                    var expAssetSet = new HashSet<MonoBehaviour>();
                    foreach (var expPPtr in cubismExpList.CubismExpressionObjects)
                    {
                        if (expPPtr.TryGet<MonoBehaviour>(out var expMono, ExpressionLst.assetsFile))
                        {
                            expAssetSet.Add(expMono);
                        }
                    }

                    if (expAssetSet.Count > 0)
                    {
                        Expressions = expAssetSet.ToList();
                        Logger.Debug($"\"{ExpressionLst.m_Name}\": found {expAssetSet.Count} expression(s)");
                    }
                }
            }

            if (Expressions.Count > 0)
            {
                Directory.CreateDirectory(destExpressionPath);
            }
            foreach (var monoBehaviour in Expressions)
            {
                var expressionName = monoBehaviour.m_Name.Replace(".exp3", "");
                var expressionDict = ParseMonoBehaviour(monoBehaviour, CubismMonoBehaviourType.Expression, Assembly);
                if (expressionDict == null)
                    continue;
                
                var expression = JsonSerializer.Deserialize<CubismExpression3Json>(JsonSerializer.SerializeToUtf8Bytes(expressionDict, CubismJsonOptions), CubismJsonOptions);

                expressions.Add(new JsonObject
                {
                    ["Name"] = expressionName,
                    ["File"] = $"expressions/{expressionName}.exp3.json",
                });
                File.WriteAllBytes($"{destExpressionPath}{expressionName}.exp3.json", JsonSerializer.SerializeToUtf8Bytes(expression, CubismJsonOptions));
            }
            #endregion

            #region pose3.json
            var isPoseExported = false;
            if (PoseParts.Count > 0)
            {
                try
                {
                    isPoseExported = ExportPoseJson(destPath, modelName);
                }
                catch (Exception e)
                {
                    Logger.Warning($"An error occurred while exporting pose3.json\n{e}");
                }
            }
            #endregion

            #region physics3.json
            var isPhysicsExported = false;
            if (PhysicsMono != null)
            {
                var physicsDict = ParseMonoBehaviour(PhysicsMono, CubismMonoBehaviourType.Physics, Assembly);
                if (physicsDict != null)
                {
                    try
                    {
                        var buff = ParsePhysics(physicsDict, motionFps);
                        File.WriteAllText($"{destPath}{modelName}.physics3.json", buff);
                        isPhysicsExported = true;
                    }
                    catch (Exception e)
                    {
                        Logger.Warning($"Error in parsing physics data: {e.Message}");
                    }
                }
            }
            #endregion

            #region model3.json
            var groups = new List<CubismModel3Json.SerializableGroup>();

            //Try looking for group IDs among the parameter names manually
            if (EyeBlinkParameters.Count == 0)
            {
                EyeBlinkParameters = ParameterNames.Where(x =>
                    x.ToLower().Contains("eye")
                    && x.ToLower().Contains("open")
                    && (x.ToLower().Contains('l') || x.ToLower().Contains('r'))
                ).ToHashSet();
            }
            if (LipSyncParameters.Count == 0)
            {
                LipSyncParameters = ParameterNames.Where(x =>
                    x.ToLower().Contains("mouth")
                    && x.ToLower().Contains("open")
                    && x.ToLower().Contains('y')
                ).ToHashSet();
            }

            groups.Add(new CubismModel3Json.SerializableGroup
            {
                Target = "Parameter",
                Name = "EyeBlink",
                Ids = EyeBlinkParameters.ToArray()
            });
            groups.Add(new CubismModel3Json.SerializableGroup
            {
                Target = "Parameter",
                Name = "LipSync",
                Ids = LipSyncParameters.ToArray()
            });
            
            var model3 = new CubismModel3Json
            {
                Version = 3,
                Name = modelName,
                FileReferences = new CubismModel3Json.SerializableFileReferences
                {
                    Moc = $"{modelName}.moc3",
                    Textures = textures.ToArray(),
                    Physics = isPhysicsExported ? $"{modelName}.physics3.json" : null,
                    Pose = isPoseExported ? $"{modelName}.pose3.json" : null,
                    DisplayInfo = isCdiExported ? $"{modelName}.cdi3.json" : null,
                    Motions = new JsonObject(motions),
                    Expressions = expressions,
                },
                Groups = groups.ToArray()
            };
            File.WriteAllBytes($"{destPath}{modelName}.model3.json", JsonSerializer.SerializeToUtf8Bytes(model3, CubismJsonOptions));
            #endregion
        }

        private void ExportFadeMotions(string destMotionPath, SortedDictionary<string, JsonNode> motions, ref float fps)
        {
            Directory.CreateDirectory(destMotionPath);
            foreach (var fadeMotionMono in FadeMotions)
            {
                var fadeMotionDict = ParseMonoBehaviour(fadeMotionMono, CubismMonoBehaviourType.FadeMotion, Assembly);
                if (fadeMotionDict == null)
                    continue;

                var fadeMotion = JsonSerializer.Deserialize<CubismFadeMotionData>(JsonSerializer.SerializeToUtf8Bytes(fadeMotionDict, CubismJsonOptions), CubismJsonOptions);
                if (fadeMotion.ParameterIds.Length == 0)
                    continue;

                var motionJson = new CubismMotion3Json(fadeMotion, ParameterNames, PartNames, Options.ForceBezier);
                fps = motionJson.Meta.Fps;

                var animName = Path.GetFileNameWithoutExtension(fadeMotion.m_Name);
                if (motions.ContainsKey(animName))
                {
                    animName = $"{animName}_{fadeMotion.GetHashCode()}";
                    if (motions.ContainsKey(animName))
                        continue;
                }
                var motionPath = new JsonObject { ["File"] = $"motions/{animName}.motion3.json" };
                motions.Add(animName, new JsonArray(motionPath));
                File.WriteAllBytes($"{destMotionPath}{animName}.motion3.json", JsonSerializer.SerializeToUtf8Bytes(motionJson, CubismJsonOptions));
            }
        }

        private void ExportClipMotions(string destMotionPath, CubismMotion3Converter converter, SortedDictionary<string, JsonNode> motions, ref float fps)
        {
            if (converter == null)
                return;

            if (converter.AnimationList.Count > 0)
            {
                Directory.CreateDirectory(destMotionPath);
            }
            foreach (var animation in converter.AnimationList)
            {
                var animName = animation.Name;
                if (animation.TrackList.Count == 0)
                {
                    Logger.Warning($"Motion \"{animName}\" is empty. Export skipped");
                    continue;
                }
                var motionJson = new CubismMotion3Json(animation, Options.ForceBezier);
                fps = motionJson.Meta.Fps;

                if (motions.ContainsKey(animName))
                {
                    animName = $"{animName}_{animation.GetHashCode()}";

                    if (motions.ContainsKey(animName))
                        continue;
                }
                var motionPath = new JsonObject { ["File"] = $"motions/{animName}.motion3.json" };
                motions.Add(animName, new JsonArray(motionPath));
                File.WriteAllBytes($"{destMotionPath}{animName}.motion3.json", JsonSerializer.SerializeToUtf8Bytes(motionJson, CubismJsonOptions));
            }
        }

        private bool ExportPoseJson(string destPath, string modelName)
        {
            var groupDict = new SortedDictionary<int, List<CubismPose3Json.ControlNode>>();
            foreach (var posePartMono in PoseParts)
            {
                var posePartDict = ParseMonoBehaviour(posePartMono, CubismMonoBehaviourType.PosePart, Assembly);
                if (posePartDict == null)
                    break;

                if (!posePartMono.m_GameObject.TryGet(out var partObj))
                    continue;

                var poseNode = new CubismPose3Json.ControlNode
                {
                    Id = partObj.m_Name,
                    Link = Array.ConvertAll((object[])posePartDict["Link"], x => x?.ToString())
                };
                var groupIndex = (int)posePartDict["GroupIndex"];
                if (groupDict.ContainsKey(groupIndex))
                {
                    groupDict[groupIndex].Add(poseNode);
                }
                else
                {
                    groupDict.Add(groupIndex, new List<CubismPose3Json.ControlNode> {poseNode});
                }
            }

            if (groupDict.Count == 0)
                return false;

            var poseJson = new CubismPose3Json
            {
                Type = "Live2D Pose",
                Groups = new CubismPose3Json.ControlNode[groupDict.Count][]
            };
            var i = 0;
            foreach (var nodeList in groupDict.Values)
            {
                poseJson.Groups[i++] = nodeList.ToArray();
            }
            File.WriteAllBytes($"{destPath}{modelName}.pose3.json", JsonSerializer.SerializeToUtf8Bytes(poseJson, CubismJsonOptions));
            return true;
        }

        private bool ExportCdiJson(string destPath, string modelName)
        {
            var cdiJson = new CubismCdi3Json
            {
                Version = 3,
                ParameterGroups = Array.Empty<CubismCdi3Json.ParamGroupArray>()
            };

            var parameters = new SortedSet<CubismCdi3Json.ParamGroupArray>();
            foreach (var paramMono in ParametersCdi)
            {
                var displayName = GetDisplayName(paramMono);
                if (displayName == null)
                    break;

                paramMono.m_GameObject.TryGet(out var paramGameObject);
                var paramId = paramGameObject.m_Name;
                parameters.Add(new CubismCdi3Json.ParamGroupArray
                {
                    Id = paramId,
                    GroupId = "",
                    Name = displayName
                });
            }
            cdiJson.Parameters = parameters.ToArray();

            var parts = new SortedSet<CubismCdi3Json.PartArray>();
            foreach (var partMono in PartsCdi)
            {
                var displayName = GetDisplayName(partMono);
                if (displayName == null)
                    break;

                partMono.m_GameObject.TryGet(out var partGameObject);
                var paramId = partGameObject.m_Name;
                parts.Add(new CubismCdi3Json.PartArray
                {
                    Id = paramId,
                    Name = displayName
                });
            }
            cdiJson.Parts = parts.ToArray();

            if (parts.Count == 0 && parameters.Count == 0) 
                return false;

            File.WriteAllBytes($"{destPath}{modelName}.cdi3.json", JsonSerializer.SerializeToUtf8Bytes(cdiJson, CubismJsonOptions));
            return true;
        }

        private string GetDisplayName(MonoBehaviour cdiMono)
        {
            var dict = ParseMonoBehaviour(cdiMono, CubismMonoBehaviourType.DisplayInfo, Assembly);
            if (dict == null)
                return null;

            var name = (string)dict["Name"];
            if (dict.Contains("DisplayName"))
            {
                var displayName = (string)dict["DisplayName"];
                name = displayName != "" ? displayName : name;
            }
            return name;
        }

        private bool TryGetFadeList(MonoBehaviour m_MonoBehaviour, out MonoBehaviour listMono)
        {
            return TryGetAsset(m_MonoBehaviour, CubismMonoBehaviourType.FadeController, "CubismFadeMotionList", out listMono);
        }

        private bool TryGetExpressionList(MonoBehaviour m_MonoBehaviour, out MonoBehaviour listMono)
        {
            return TryGetAsset(m_MonoBehaviour, CubismMonoBehaviourType.ExpressionController, "ExpressionsList", out listMono);
        }

        private bool TryGetRenderTexture(MonoBehaviour m_MonoBehaviour, out Texture2D renderTex)
        {
            return TryGetAsset(m_MonoBehaviour, CubismMonoBehaviourType.RenderTexture, "_mainTexture", out renderTex);
        }

        private bool TryGetAsset<T>(MonoBehaviour m_MonoBehaviour, CubismMonoBehaviourType cubismMonoType, string pptrField, out T result) where T : Object
        {
            result = null;
            if (m_MonoBehaviour == null)
                return false;

            var pptrDict = (OrderedDictionary)ParseMonoBehaviour(m_MonoBehaviour, cubismMonoType, Assembly)?[pptrField];
            if (pptrDict == null)
                return false;

            var resultPPtr = GeneratePPtr<T>(pptrDict, m_MonoBehaviour.assetsFile);
            return resultPPtr.TryGet(out result);
        }

        private PPtr<T> GeneratePPtr<T>(OrderedDictionary pptrDict, SerializedFile assetsFile = null) where T : Object
        {
            return new PPtr<T>
            {
                m_FileID = (int)pptrDict["m_FileID"],
                m_PathID = (long)pptrDict["m_PathID"],
                AssetsFile = assetsFile
            };
        }
    }
}
