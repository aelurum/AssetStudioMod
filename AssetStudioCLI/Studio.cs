using AssetStudio;
using AssetStudio.Addons.CubismLive2D;
using AssetStudioCLI.Options;
using CubismLive2DExtractor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ansi = AssetStudio.ColorConsole;

namespace AssetStudioCLI
{
    internal static class Studio
    {
        public static AssetsManager assetsManager = new AssetsManager();
        public static List<AssetItem> parsedAssetsList = new List<AssetItem>();
        public static List<BaseNode> gameObjectTree = new List<BaseNode>();
        public static AssemblyLoader assemblyLoader = new AssemblyLoader();
        public static L2DAssetsManager l2dAssetsManager = new L2DAssetsManager();
        private static Dictionary<AssetStudio.Object, string> containers = new Dictionary<AssetStudio.Object, string>();

        static Studio()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Progress.Default = new Progress<int>(ShowCurProgressValue);
            Progress.SetInstance(1, new Progress<int>(ShowCurProgressValue));
            assetsManager.Options.CustomCachePath = CLIOptions.o_cachePath.Value;
            assetsManager.Options.DecompressToDisk = CLIOptions.f_decompressToDisk.Value;
            assetsManager.Options.ParseByTypeTree = !CLIOptions.f_avoidLoadingViaTypetree.Value;
            assetsManager.Options.GameSpecific.CustomUnityVersion = CLIOptions.o_unityVersion.Value;
            assetsManager.Options.GameSpecific.Bundles.CustomBlockInfoCompression = CLIOptions.o_bundleBlockInfoCompression.Value;
            assetsManager.Options.GameSpecific.Bundles.CustomBlockCompression = CLIOptions.o_bundleBlockCompression.Value;
            assetsManager.ClearOptionLoaders();
        }

        private static void ShowCurProgressValue(int value)
        {
            Console.Write($"[{value:000}%]\r");
        }

        public static void ExtractBundles()
        {
            var extractedCount = 0;
            var path = CLIOptions.inputPathList[0];
            var savePath = CLIOptions.o_outputFolder.Value;
            Progress.Reset();
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                var totalCount = files.Length;
                for (var i = 0; i < totalCount; i++)
                {
                    var file = files[i];
                    var fileOriPath = Path.GetDirectoryName(file);
                    var fileSavePath = fileOriPath.Replace(path, savePath);
                    extractedCount += ExtractFile(file, fileSavePath);
                    Progress.Report(i + 1, totalCount);
                }
            }
            else if (File.Exists(path))
            {
                extractedCount += ExtractFile(path, savePath);
            }

            var status = extractedCount > 0
                ? $"Finished extracting {extractedCount} file(s) to \"{savePath.Color(Ansi.BrightCyan)}\""
                : "Nothing extracted (not extractable or file(s) already exist)";
            Logger.Default.Log(LoggerEvent.Info, status, ignoreLevel: true);
        }

        public static int ExtractFile(string fileName, string savePath)
        {
            var extractedCount = 0;
            var reader = new FileReader(fileName);
            switch (reader.FileType)
            {
                case FileType.BundleFile:
                    extractedCount += ExtractBundleFile(reader, savePath);
                    break;
                case FileType.WebFile:
                    extractedCount += ExtractWebDataFile(reader, savePath);
                    break;
                default:
                    reader.Dispose();
                    break;
            }
            return extractedCount;
        }

        private static int ExtractBundleFile(FileReader reader, string savePath)
        {
            Logger.Info($"Decompressing {reader.FileName} ...");

            Logger.Debug($"Bundle offset: {reader.Position}");
            var count = 0;
            var bundleStream = new OffsetStream(reader);
            var bundleReader = new FileReader(reader.FullPath, bundleStream);
            var bundleFile = new BundleFile(bundleReader, assetsManager.Options);
            var extractPath = Path.Combine(savePath, reader.FileName + "_unpacked");
            if (bundleFile.FileList.Count > 0)
            {
                count += ExtractStreamFile(extractPath, bundleFile);
            }
            while (bundleFile.IsDataAfterBundle)
            {
                bundleStream.Offset = reader.Position;
                bundleReader = new FileReader($"{reader.FullPath}_0x{bundleStream.Offset:X}", bundleStream);
                if (bundleReader.FileType != FileType.BundleFile)
                    break;
                if (bundleReader.Position > 0)
                {
                    bundleStream.Offset += bundleReader.Position;
                    bundleReader.FullPath = $"{reader.FullPath}_0x{bundleStream.Offset:X}";
                    bundleReader.FileName = $"{reader.FileName}_0x{bundleStream.Offset:X}";
                }
                Logger.Info($"[MultiBundle] Decompressing \"{reader.FileName}\" from offset: 0x{bundleStream.Offset:X}..");
                bundleFile = new BundleFile(bundleReader, assetsManager.Options, isMultiBundle: true);
                if (bundleFile.FileList.Count > 0)
                {
                    count += ExtractStreamFile(extractPath, bundleFile);
                }
            }
            bundleStream.Dispose();
            return count;
        }

        private static int ExtractWebDataFile(FileReader reader, string savePath)
        {
            Logger.Info($"Decompressing {reader.FileName} ...");
            var webFile = new WebFile(reader);
            reader.Dispose();
            if (webFile.FileList.Count > 0)
            {
                var extractPath = Path.Combine(savePath, reader.FileName + "_unpacked");
                return ExtractStreamFile(extractPath, webFile);
            }
            return 0;
        }

        private static int ExtractStreamFile(string extractPath, FileArchive fileArchive)
        {
            var extractedCount = 0;
            var isWebFile = fileArchive is WebFile;
            foreach (var file in fileArchive.FileList)
            {
                if (file.stream == null)
                    continue;

                var filePath = Path.Combine(extractPath, file.path);
                var fileDirectory = Path.GetDirectoryName(filePath);
                Directory.CreateDirectory(fileDirectory);
                if (!File.Exists(filePath))
                {
                    using (var fileStream = File.Create(filePath))
                    {
                        file.stream.Position = 0;
                        file.stream.CopyTo(fileStream, file.stream.Length);
                    }
                    extractedCount++;
                }
                if (isWebFile) file.stream.Dispose();
            }
            if (!isWebFile)
            {
                fileArchive.FileList.FirstOrDefault(x => x.stream != null)?.stream.Dispose();
            }
            return extractedCount;
        }

        public static bool LoadAssets()
        {
            var isLoaded = false;
            
            if (!CLIOptions.f_loadAllAssets.Value)
            {
                assetsManager.SetAssetFilter(CLIOptions.o_exportAssetTypes.Value);
            }
            assetsManager.LoadFilesAndFolders(out _, CLIOptions.inputPathList);
            if (assetsManager.AssetsFileList.Count == 0)
            {
                Logger.Warning("No Unity file can be loaded.");
            }
            else
            {
                isLoaded = true;
            }

            return isLoaded;
        }

        public static void ParseAssets()
        {
            Logger.Info("Parse assets...");

            var fileAssetsList = new List<AssetItem>();
            var tex2dArrayAssetList = new List<AssetItem>();
            var objectCount = assetsManager.AssetsFileList.Sum(x => x.Objects.Count);
            var objectAssetItemDic = new Dictionary<AssetStudio.Object, AssetItem>(objectCount);
            var isL2dMode = CLIOptions.o_workMode.Value == WorkMode.Live2D;

            Progress.Reset();
            var i = 0;
            foreach (var assetsFile in assetsManager.AssetsFileList)
            {
                var preloadTable = new List<PPtr<AssetStudio.Object>>();
                foreach (var asset in assetsFile.Objects)
                {
                    var assetItem = new AssetItem(asset);
                    objectAssetItemDic.Add(asset, assetItem);
                    var isExportable = false;
                    switch (asset)
                    {
                        case PreloadData m_PreloadData:
                            preloadTable = m_PreloadData.m_Assets;
                            break;
                        case AssetBundle m_AssetBundle:
                            var isStreamedSceneAssetBundle = m_AssetBundle.m_IsStreamedSceneAssetBundle;
                            if (!isStreamedSceneAssetBundle)
                            {
                                preloadTable = m_AssetBundle.m_PreloadTable;
                            }
                            assetItem.Text = string.IsNullOrEmpty(m_AssetBundle.m_AssetBundleName) ? m_AssetBundle.m_Name : m_AssetBundle.m_AssetBundleName;

                            foreach (var m_Container in m_AssetBundle.m_Container)
                            {
                                var preloadIndex = m_Container.Value.preloadIndex;
                                var preloadSize = isStreamedSceneAssetBundle ? preloadTable.Count : m_Container.Value.preloadSize;
                                var preloadEnd = preloadIndex + preloadSize;
                                for (var k = preloadIndex; k < preloadEnd; k++)
                                {
                                    var pptr = preloadTable[k];
                                    if (pptr.TryGet(out var obj))
                                    {
                                        containers[obj] = m_Container.Key;
                                    }
                                }
                            }
                            break;
                        case ResourceManager m_ResourceManager:
                            foreach (var m_Container in m_ResourceManager.m_Container)
                            {
                                if (m_Container.Value.TryGet(out var obj))
                                {
                                    containers[obj] = m_Container.Key;
                                }
                            }
                            break;
                        case Texture2D m_Texture2D:
                            if (!string.IsNullOrEmpty(m_Texture2D.m_StreamData?.path))
                                assetItem.FullSize = asset.byteSize + m_Texture2D.m_StreamData.size;
                            assetItem.Text = m_Texture2D.m_Name;
                            break;
                        case Texture2DArray m_Texture2DArray:
                            if (!string.IsNullOrEmpty(m_Texture2DArray.m_StreamData?.path))
                                assetItem.FullSize = asset.byteSize + m_Texture2DArray.m_StreamData.size;
                            assetItem.Text = m_Texture2DArray.m_Name;
                            tex2dArrayAssetList.Add(assetItem);
                            break;
                        case AudioClip m_AudioClip:
                            if (!string.IsNullOrEmpty(m_AudioClip.m_Source))
                                assetItem.FullSize = asset.byteSize + m_AudioClip.m_Size;
                            assetItem.Text = m_AudioClip.m_Name;
                            break;
                        case VideoClip m_VideoClip:
                            if (!string.IsNullOrEmpty(m_VideoClip.m_OriginalPath))
                                assetItem.FullSize = asset.byteSize + m_VideoClip.m_ExternalResources.m_Size;
                            assetItem.Text = m_VideoClip.m_Name;
                            break;
                        case Shader m_Shader:
                            assetItem.Text = m_Shader.m_ParsedForm?.m_Name ?? m_Shader.m_Name;
                            break;
                        case MonoBehaviour m_MonoBehaviour:
                            var assetName = m_MonoBehaviour.m_Name;
                            if (m_MonoBehaviour.m_Script.TryGet(out var m_Script))
                            {
                                assetName = assetName == "" ? m_Script.m_ClassName : assetName;
                                switch (m_Script.m_ClassName)
                                {
                                    case "CubismMoc" when !l2dAssetsManager.ModelDict.ContainsKey(m_MonoBehaviour):
                                        l2dAssetsManager.ModelDict[m_MonoBehaviour] = new CubismModel(m_MonoBehaviour);
                                        break;
                                    case "CubismRenderer":
                                    case "CubismDisplayInfoParameterName":
                                    case "CubismDisplayInfoPartName":
                                    case "CubismPosePart":
                                        l2dAssetsManager.BindCubismAssetsByMono(m_MonoBehaviour, m_Script.m_ClassName);
                                        break;
                                }
                            }
                            else if (CubismHelpers.TryGetCubismMonoBehaviourType(m_MonoBehaviour, out var type))
                            {
                                switch (type)
                                {
                                    case CubismHelpers.CubismMonoBehaviourType.Moc when !l2dAssetsManager.ModelDict.ContainsKey(m_MonoBehaviour):
                                        l2dAssetsManager.ModelDict[m_MonoBehaviour] = new CubismModel(m_MonoBehaviour);
                                        break;
                                    case CubismHelpers.CubismMonoBehaviourType.RenderTexture:
                                    case CubismHelpers.CubismMonoBehaviourType.PosePart:
                                        l2dAssetsManager.BindCubismAssetsByMono(m_MonoBehaviour, type);
                                        break;
                                }
                            }
                            assetItem.Text = assetName;
                            break;
                        case GameObject m_GameObject:
                            assetItem.Text = m_GameObject.m_Name;
                            if (m_GameObject.CubismModel != null)
                            {
                                l2dAssetsManager.BindCubismAssetsByGO(m_GameObject, assemblyLoader);
                            }
                            break;
                        case Animator m_Animator:
                            if (m_Animator.m_GameObject.TryGet(out var gameObject))
                            {
                                assetItem.Text = gameObject.m_Name;
                            }
                            break;
                        case NamedObject m_NamedObject:
                            assetItem.Text = m_NamedObject.m_Name;
                            break;
                    }
                    if (string.IsNullOrEmpty(assetItem.Text))
                    {
                        assetItem.Text = assetItem.TypeString + assetItem.UniqueID;
                    }

                    isExportable = CLIOptions.o_exportAssetTypes.Value.Contains(asset.type);
                    if (isExportable || (CLIOptions.f_loadAllAssets.Value && CLIOptions.o_exportAssetTypes.Value == CLIOptions.o_exportAssetTypes.DefaultValue))
                    {
                        fileAssetsList.Add(assetItem);
                    }

                    asset.Name = assetItem.Text;
                    Progress.Report(++i, objectCount);
                }

                foreach (var asset in fileAssetsList)
                {
                    if (containers.TryGetValue(asset.Asset, out var container))
                    {
                        asset.Container = container;

                        if (asset.Asset is GameObject m_GameObject && m_GameObject.CubismModel != null)
                        {
                            m_GameObject.CubismModel.Container = container;
                        }
                    }
                }
                foreach (var tex2dAssetItem in tex2dArrayAssetList)
                {
                    var m_Texture2DArray = (Texture2DArray)tex2dAssetItem.Asset;
                    for (var layer = 0; layer < m_Texture2DArray.m_Depth; layer++)
                    {
                        var fakeObj = new Texture2D(m_Texture2DArray, layer);
                        m_Texture2DArray.TextureList.Add(fakeObj);
                    }
                }
                parsedAssetsList.AddRange(fileAssetsList);
                fileAssetsList.Clear();
                tex2dArrayAssetList.Clear();
                if (!isL2dMode)
                {
                    containers.Clear();
                }
            }

            if (CLIOptions.o_workMode.Value == WorkMode.SplitObjects || CLIOptions.o_groupAssetsBy.Value == AssetGroupOption.SceneHierarchy)
            {
                BuildTreeStructure(objectAssetItemDic);
            }

            var log = $"Finished loading {assetsManager.AssetsFileList.Count} files with {parsedAssetsList.Count} exportable asset(s)";
            var m_ObjectsCount = assetsManager.AssetsFileList.Sum(x => x.m_Objects.LongCount(y => CLIOptions.o_exportAssetTypes.Value.Any(k => (int)k == y.classID)));
            var objectsCount = assetsManager.AssetsFileList.Sum(x => x.Objects.LongCount(y => CLIOptions.o_exportAssetTypes.Value.Any(k => k == y.type)));
            if (m_ObjectsCount != objectsCount)
            {
                log += $" and {m_ObjectsCount - objectsCount} assets failed to read";
            }
            Logger.Info(log);
        }

        public static void BuildTreeStructure(Dictionary<AssetStudio.Object, AssetItem> objectAssetItemDic)
        {
            Logger.Info("Building tree structure...");

            var treeNodeDictionary = new Dictionary<GameObject, GameObjectNode>();
            var assetsFileCount = assetsManager.AssetsFileList.Count;
            int j = 0;
            Progress.Reset();
            foreach (var assetsFile in assetsManager.AssetsFileList)
            {
                var fileNode = new BaseNode(assetsFile.fileName);  //RootNode

                foreach (var obj in assetsFile.Objects)
                {
                    if (obj is GameObject m_GameObject)
                    {
                        if (!treeNodeDictionary.TryGetValue(m_GameObject, out var currentNode))
                        {
                            currentNode = new GameObjectNode(m_GameObject);
                            treeNodeDictionary.Add(m_GameObject, currentNode);
                        }
                        
                        foreach (var pptr in m_GameObject.m_Components)
                        {
                            if (pptr.TryGet(out var m_Component))
                            {
                                objectAssetItemDic[m_Component].Node = currentNode;
                                if (m_Component is MeshFilter m_MeshFilter)
                                {
                                    if (m_MeshFilter.m_Mesh.TryGet(out var m_Mesh))
                                    {
                                        objectAssetItemDic[m_Mesh].Node = currentNode;
                                    }
                                }
                                else if (m_Component is SkinnedMeshRenderer m_SkinnedMeshRenderer)
                                {
                                    if (m_SkinnedMeshRenderer.m_Mesh.TryGet(out var m_Mesh))
                                    {
                                        objectAssetItemDic[m_Mesh].Node = currentNode;
                                    }
                                }
                            }
                        }

                        var parentNode = fileNode;
                        if (m_GameObject.m_Transform != null)
                        {
                            if (m_GameObject.m_Transform.m_Father.TryGet(out var m_Father))
                            {
                                if (m_Father.m_GameObject.TryGet(out var parentGameObject))
                                {
                                    if (!treeNodeDictionary.TryGetValue(parentGameObject, out var parentGameObjectNode))
                                    {
                                        parentGameObjectNode = new GameObjectNode(parentGameObject);
                                        treeNodeDictionary.Add(parentGameObject, parentGameObjectNode);
                                    }
                                    parentNode = parentGameObjectNode;
                                }
                            }
                        }
                        parentNode.nodes.Add(currentNode);
                    }
                }

                if (fileNode.nodes.Count > 0)
                {
                    GenerateFullPath(fileNode, fileNode.Text);
                    gameObjectTree.Add(fileNode);
                }

                Progress.Report(++j, assetsFileCount);
            }

            treeNodeDictionary.Clear();
            objectAssetItemDic.Clear();
        }

        private static void GenerateFullPath(BaseNode treeNode, string path)
        {
            treeNode.FullPath = path;
            foreach (var node in treeNode.nodes)
            {
                if (node.nodes.Count > 0)
                {
                    GenerateFullPath(node, Path.Combine(path, node.Text));
                }
                else
                {
                    node.FullPath = Path.Combine(path, node.Text);
                }
            }
        }

        public static void ShowExportableAssetsInfo()
        {
            var exportableAssetsCountDict = new Dictionary<ClassIDType, int>();
            var info = "======";
            if (parsedAssetsList.Count > 0)
            {
                info += $"\n\n[Unity Version]" +
                        $"\n# {parsedAssetsList[0].Asset.version}";

                foreach (var asset in parsedAssetsList)
                {
                    if (exportableAssetsCountDict.ContainsKey(asset.Type))
                    {
                        exportableAssetsCountDict[asset.Type] += 1;
                    }
                    else
                    {
                        exportableAssetsCountDict.Add(asset.Type, 1);
                    }
                }

                info += "\n\n[Exportable Assets Count]";
                foreach (var assetType in exportableAssetsCountDict.Keys)
                {
                    info += $"\n# {assetType}: {exportableAssetsCountDict[assetType]}";
                }
                if (exportableAssetsCountDict.Count > 1)
                {
                    info += $"\n#\n# Total: {parsedAssetsList.Count} assets";
                }

                info += $"\n\n[Cubism Live2D]\n# Exportable Models: {l2dAssetsManager.ModelDict.Count}";
            }
            else
            {
                info += "\n\nNo exportable assets found.";
            }

            if (CLIOptions.o_logLevel.Value > LoggerEvent.Info)
            {
                Console.WriteLine(info);
            }
            else
            {
                Logger.Info(info);
            }
        }

        public static void Filter()
        {
            switch (CLIOptions.o_workMode.Value)
            {
                case WorkMode.Live2D:
                case WorkMode.SplitObjects:
                case WorkMode.Animator:
                    break;
                default:
                    FilterAssets();
                    break;
            }
        }

        private static void FilterAssets()
        {
            var assetsCount = parsedAssetsList.Count;
            var filteredAssets = new List<AssetItem>();
            var regexMode = CLIOptions.f_filterWithRegex.Value;
            Regex regex;

            switch(CLIOptions.filterBy)
            {
                case FilterBy.Name when regexMode:
                    regex = new Regex(CLIOptions.o_filterByName.Value[0]);
                    filteredAssets = parsedAssetsList.FindAll(x => regex.IsMatch(x.Text));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"which Names match {regex.ToString().Color(Ansi.BrightYellow)} regexp."
                    );
                    break;
                case FilterBy.Name:
                    filteredAssets = parsedAssetsList.FindAll(x => CLIOptions.o_filterByName.Value.Any(y => x.Text.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"that contain {$"\"{string.Join("\", \"", CLIOptions.o_filterByName.Value)}\"".Color(Ansi.BrightYellow)} in their Names."
                    );
                    break;
                case FilterBy.Container when regexMode:
                    regex = new Regex(CLIOptions.o_filterByContainer.Value[0]);
                    filteredAssets = parsedAssetsList.FindAll(x => regex.IsMatch(x.Container));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"which Containers match {regex.ToString().Color(Ansi.BrightYellow)} regexp."
                    );
                    break;
                case FilterBy.Container:
                    filteredAssets = parsedAssetsList.FindAll(x => CLIOptions.o_filterByContainer.Value.Any(y => x.Container.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"that contain {$"\"{string.Join("\", \"", CLIOptions.o_filterByContainer.Value)}\"".Color(Ansi.BrightYellow)} in their Containers."
                    );
                    break;
                case FilterBy.PathID:
                    filteredAssets = parsedAssetsList.FindAll(x => CLIOptions.o_filterByPathID.Value.Any(y => x.m_PathID.ToString().IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"that contain {$"\"{string.Join("\", \"", CLIOptions.o_filterByPathID.Value)}\"".Color(Ansi.BrightYellow)} in their PathIDs."
                    );
                    break;
                case FilterBy.NameOrContainer when regexMode:
                    regex = new Regex(CLIOptions.o_filterByText.Value[0]);
                    filteredAssets = parsedAssetsList.FindAll(x => regex.IsMatch(x.Text) || regex.IsMatch(x.Container));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"which Names or Containers match {regex.ToString().Color(Ansi.BrightYellow)} regexp."
                    );
                    break;
                case FilterBy.NameOrContainer:
                    filteredAssets = parsedAssetsList.FindAll(x =>
                        CLIOptions.o_filterByText.Value.Any(y => x.Text.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        CLIOptions.o_filterByText.Value.Any(y => x.Container.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0)
                    );
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"that contain {$"\"{string.Join("\", \"", CLIOptions.o_filterByText.Value)}\"".Color(Ansi.BrightYellow)} in their Names or Containers."
                    );
                    break;
                case FilterBy.NameAndContainer when regexMode:
                    var nameRegex = new Regex(CLIOptions.o_filterByName.Value[0]);
                    var containerRegex = new Regex(CLIOptions.o_filterByContainer.Value[0]);
                    filteredAssets = parsedAssetsList.FindAll(x => nameRegex.IsMatch(x.Text) && containerRegex.IsMatch(x.Container));
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"which Containers match {containerRegex.ToString().Color(Ansi.BrightYellow)} regexp " +
                        $"and which Names match {nameRegex.ToString().Color(Ansi.BrightYellow)} regexp."
                    );
                    break;
                case FilterBy.NameAndContainer:
                    filteredAssets = parsedAssetsList.FindAll(x =>
                        CLIOptions.o_filterByName.Value.Any(y => x.Text.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0) &&
                        CLIOptions.o_filterByContainer.Value.Any(y => x.Container.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0)
                    );
                    Logger.Info(
                        $"Found [{filteredAssets.Count}/{assetsCount}] asset(s) " +
                        $"that contain {$"\"{string.Join("\", \"", CLIOptions.o_filterByContainer.Value)}\"".Color(Ansi.BrightYellow)} in their Containers " +
                        $"and {$"\"{string.Join("\", \"", CLIOptions.o_filterByName.Value)}\"".Color(Ansi.BrightYellow)} in their Names."
                    );
                    break;
            }
        
            if (CLIOptions.f_filterExcludeMode.Value)
            {
                var excludeCount = assetsCount - filteredAssets.Count;
                Logger.Info($"Excluding {excludeCount} asset(s) that match the filter.");
                filteredAssets = parsedAssetsList.Except(filteredAssets).ToList();
            }

            parsedAssetsList.Clear();
            parsedAssetsList = filteredAssets;
        }

        public static void ExportAssets()
        {
            var workMode = CLIOptions.o_workMode.Value;
            var savePath = CLIOptions.o_outputFolder.Value;
            var toExportCount = parsedAssetsList.Count;
            var exportedCount = 0;

            var groupOption = CLIOptions.o_groupAssetsBy.Value;
            var parallelExportCount = CLIOptions.o_maxParallelExportTasks.Value;
            var toExportAssetDict = new ConcurrentDictionary<AssetItem, string>();
            var toParallelExportAssetDict = new ConcurrentDictionary<AssetItem, string>();
            var modeStr = workMode == WorkMode.Dump || workMode == WorkMode.JsonDump ? "Dump" : "Export";

            if (CLIOptions.o_stripPathPrefix.Value != null)
            {
                foreach (var asset in parsedAssetsList)
                {
                    var containerPath = asset.Container;
                    if (string.IsNullOrEmpty(containerPath))
                    {
                        continue;
                    }
                    if (!Path.Combine(Path.GetDirectoryName(containerPath), Path.GetFileName(containerPath)).StartsWith(CLIOptions.o_stripPathPrefix.Value))
                    {
                        Logger.Warning($"Asset container path \"{asset.Container}\" does not start with the specified path prefix \"{CLIOptions.o_stripPathPrefix.Value}\"");
                        Logger.Warning("strip path prefix option will be ignored.");
                        CLIOptions.o_stripPathPrefix.Value = null;
                        break;
                    }
                }
            }

            if (CLIOptions.o_stripPathPrefix.Value != null)
            {
                Logger.Info($"Asset container path prefix \"{CLIOptions.o_stripPathPrefix.Value}\" will be stripped off.");
            }

            Parallel.ForEach(parsedAssetsList, asset =>
            {
                string exportPath;
                switch (groupOption)
                {
                    case AssetGroupOption.TypeName:
                        exportPath = Path.Combine(savePath, asset.TypeString);
                        break;
                    case AssetGroupOption.ContainerPath:
                    case AssetGroupOption.ContainerPathFull:
                        if (!string.IsNullOrEmpty(asset.Container))
                        {
                            var containerPath = Path.GetDirectoryName(asset.Container);
                            if (CLIOptions.o_stripPathPrefix.Value != null)
                            {
                                containerPath = containerPath.Substring(CLIOptions.o_stripPathPrefix.Value.Length);
                            }
                            exportPath = Path.Combine(savePath, containerPath);
                            if (groupOption == AssetGroupOption.ContainerPathFull)
                            {
                                exportPath = Exporter.SafeFilePath(exportPath, Path.GetFileNameWithoutExtension(asset.Container));
                            }
                        }
                        else
                        {
                            exportPath = savePath;
                        }
                        break;
                    case AssetGroupOption.SourceFileName:
                        if (string.IsNullOrEmpty(asset.SourceFile.originalPath))
                        {
                            exportPath = Path.Combine(savePath, asset.SourceFile.fileName + "_export");
                        }
                        else
                        {
                            exportPath = Path.Combine(savePath, Path.GetFileName(asset.SourceFile.originalPath) + "_export", asset.SourceFile.fileName);
                        }
                        break;
                    case AssetGroupOption.SceneHierarchy:
                        if (asset.Node != null)
                        {
                            exportPath = Exporter.SafePathCombine(savePath, asset.Node.FullPath);
                        }
                        else
                        {
                            exportPath = Path.Combine(savePath, "_sceneRoot", asset.TypeString);
                        }
                        break;
                    default:
                        exportPath = savePath;
                        break;
                }
                exportPath += Path.DirectorySeparatorChar;

                if (workMode == WorkMode.Export)
                {
                    switch (asset.Type)
                    {
                        case ClassIDType.Texture2D:
                        case ClassIDType.Sprite:
                        case ClassIDType.AudioClip:
                            toParallelExportAssetDict.TryAdd(asset, exportPath);
                            break;
                        case ClassIDType.Texture2DArray:
                            var m_Texture2DArray = (Texture2DArray)asset.Asset;
                            toExportCount += m_Texture2DArray.TextureList.Count - 1;
                            foreach (var texture in m_Texture2DArray.TextureList)
                            {
                                var fakeItem = new AssetItem(texture)
                                {
                                    Text = texture.m_Name,
                                    Container = asset.Container,
                                };
                                toParallelExportAssetDict.TryAdd(fakeItem, exportPath);
                            }
                            break;
                        default:
                            toExportAssetDict.TryAdd(asset, exportPath);
                            break;
                    }
                }
                else
                {
                    toExportAssetDict.TryAdd(asset, exportPath);
                }
            });
            foreach (var toExportAsset in toExportAssetDict)
            {
                var asset = toExportAsset.Key;
                var exportPath = toExportAsset.Value;
                var isExported = false;
                try
                {
                    switch (workMode)
                    {
                        case WorkMode.ExportRaw:
                            Logger.Debug($"{workMode}: {asset.Type} : {asset.Container} : {asset.Text}");
                            isExported = Exporter.ExportRawFile(asset, exportPath);
                            break;
                        case WorkMode.Dump:
                            Logger.Debug($"{workMode}: {asset.Type} : {asset.Container} : {asset.Text}");
                            isExported = Exporter.ExportDumpFile(asset, exportPath);
                            break;
                        case WorkMode.JsonDump:
                            Logger.Debug($"{workMode}: {asset.Type} : {asset.Container} : {asset.Text}");
                            isExported = Exporter.ExportJsonDumpFile(asset, exportPath);
                            break;
                        case WorkMode.Export:
                            Logger.Debug($"{workMode}: {asset.Type} : {asset.Container} : {asset.Text}");
                            isExported = Exporter.ExportConvertFile(asset, exportPath);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"{asset.SourceFile.originalPath ?? asset.SourceFile.fullName}: [{$"{asset.Type}: {asset.Text}".Color(Ansi.BrightRed)}] : {modeStr} error\n{ex}");
                }

                if (isExported)
                {
                    exportedCount++;
                }
                Console.Write($"{modeStr}ed [{exportedCount}/{toExportCount}]\r");
            }
            Exporter.ClearHash();

            Parallel.ForEach(toParallelExportAssetDict, new ParallelOptions { MaxDegreeOfParallelism = parallelExportCount }, toExportAsset =>
            {
                var asset = toExportAsset.Key;
                var exportPath = toExportAsset.Value;
                try
                {
                    if (ParallelExporter.ParallelExportConvertFile(asset, exportPath, out var debugLog))
                    {
                        Interlocked.Increment(ref exportedCount);
                        Logger.Debug(debugLog);
                        Console.Write($"{modeStr}ed [{exportedCount}/{toExportCount}]\r");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"{asset.SourceFile.originalPath ?? asset.SourceFile.fullName}: [{$"{asset.Type}: {asset.Text}".Color(Ansi.BrightRed)}] : Export error\n{ex}");
                }
            });
            ParallelExporter.ClearHash();
            Console.WriteLine("");

            if (exportedCount == 0)
            {
                Logger.Default.Log(LoggerEvent.Info, $"Nothing {modeStr.ToLower()}ed.", ignoreLevel: true);
            }
            else
            {
                var outPath = CLIOptions.o_outputFolder.Value.ColorIf(toExportCount > exportedCount, Ansi.BrightYellow, Ansi.BrightGreen);
                Logger.Default.Log(LoggerEvent.Info, $"Finished {modeStr.ToLower()}ing {exportedCount} asset(s) to \"{outPath}\".", ignoreLevel: true);
            }

            if (toExportCount > exportedCount)
            {
                Logger.Default.Log(LoggerEvent.Info, $"{toExportCount - exportedCount} asset(s) skipped (not extractable or file(s) already exist).", ignoreLevel: true);
            }
        }

        public static void ExportAssetList()
        {
            var savePath = CLIOptions.o_outputFolder.Value;

            switch (CLIOptions.o_exportAssetList.Value)
            {
                case ExportListType.XML:
                    var filename = Path.Combine(savePath, "assets.xml");
                    var doc = new XDocument(
                        new XElement("Assets",
                            new XAttribute("filename", filename),
                            new XAttribute("createdAt", DateTime.UtcNow.ToString("s")),
                            parsedAssetsList.Select(
                                asset => new XElement("Asset",
                                    new XElement("Name", asset.Text),
                                    new XElement("Container", asset.Container),
                                    new XElement("Type", new XAttribute("id", (int)asset.Type), asset.TypeString),
                                    new XElement("PathID", asset.m_PathID),
                                    new XElement("Source", asset.SourceFile.fullName),
                                    new XElement("TreeNode", asset.Node != null ? asset.Node.FullPath : ""),
                                    new XElement("Size", asset.FullSize)
                                )
                            )
                        )
                    );
                    doc.Save(filename);

                   break;
            }
            Logger.Info($"Finished exporting asset list with {parsedAssetsList.Count} items.");
        }

        public static void ExportSplitObjects()
        {
            var savePath = CLIOptions.o_outputFolder.Value;
            var filterList = CLIOptions.o_filterByName.Value;
            var isFiltered = CLIOptions.filterBy == FilterBy.Name;
            var regexMode = CLIOptions.f_filterWithRegex.Value;
            var regex = regexMode ? new Regex(filterList[0]) : null;

            var exportableObjects = new List<GameObjectNode>();
            var exportedCount = 0;
            var k = 0;

            Logger.Info("Searching for objects to export..");
            Progress.Reset();
            var count = gameObjectTree.Sum(x => x.nodes.Count);
            foreach (var node in gameObjectTree)
            {
                foreach (GameObjectNode j in node.nodes)
                {
                    if (isFiltered && regexMode)
                    {
                        if (!regex.IsMatch(j.Text))
                            continue;
                    }
                    else if (isFiltered)
                    {
                        if (!filterList.Any(str => j.Text.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0))
                            continue;
                    }
                    var gameObjects = new List<GameObject>();
                    CollectNode(j, gameObjects);

                    if ((j.gameObject.m_Transform == null && j.gameObject.m_Animator == null)
                        || gameObjects.All(x => x.m_SkinnedMeshRenderer == null && x.m_MeshFilter == null))
                    {
                        Progress.Report(++k, count);
                        continue;
                    }
                    exportableObjects.Add(j);
                }
            }
            gameObjectTree.Clear();
            var exportableCount = exportableObjects.Count;
            var log = $"Found {exportableCount} exportable object(s) ";
            if (isFiltered)
            {
                log += regexMode
                    ? $"which names match {regex.ToString().Color(Ansi.BrightYellow)} regexp"
                    : $"that contain {$"\"{string.Join("\", \"", filterList)}\"".Color(Ansi.BrightYellow)} in their names";
            }
            Logger.Info(log);
            if (exportableCount > 0)
            {
                Progress.Reset();
                k = 0;

                foreach (var gameObjectNode in exportableObjects)
                {
                    var gameObject = gameObjectNode.gameObject;
                    var filename = gameObject.m_Name;
                    var targetPath = $"{Exporter.SafeFilePath(savePath, filename)}{Path.DirectorySeparatorChar}";
                    //重名文件处理
                    for (int i = 1; ; i++)
                    {
                        if (Directory.Exists(targetPath))
                        {
                            targetPath = $"{Exporter.SafeFilePath(savePath, filename)} ({i}){Path.DirectorySeparatorChar}";
                        }
                        else
                        {
                            break;
                        }
                    }
                    Directory.CreateDirectory(targetPath);
                    //导出FBX
                    Logger.Info($"Exporting {filename}.fbx");
                    Progress.Report(k, exportableCount);
                    try
                    {
                        Exporter.ExportGameObject(gameObject, targetPath);
                        Logger.Debug($"{gameObject.type} \"{filename}\" saved to \"{targetPath}\"");
                        exportedCount++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Export GameObject:{gameObject.m_Name} error", ex);
                    }
                    k++;
                }
            }
            var status = exportedCount > 0
                ? $"Finished exporting [{exportedCount}/{exportableCount}] object(s) to \"{CLIOptions.o_outputFolder.Value.Color(Ansi.BrightCyan)}\""
                : "Nothing exported";
            Logger.Default.Log(LoggerEvent.Info, status, ignoreLevel: true);
        }

        private static void CollectNode(GameObjectNode node, List<GameObject> gameObjects)
        {
            gameObjects.Add(node.gameObject);
            foreach (GameObjectNode i in node.nodes)
            {
                CollectNode(i, gameObjects);
            }
        }

        public static void ExportAnimator()
        {
            var animationList = CLIOptions.o_fbxAnimMode.Value == AnimationExportMode.Auto
                ? null
                : new List<AssetItem>();
            var exportAllAnimations = CLIOptions.o_fbxAnimMode.Value == AnimationExportMode.All;

            Logger.Info("Searching for Animator assets...");
            var animatorList = new List<AssetItem>();
            foreach (var asset in parsedAssetsList)
            {
                switch (asset.Type)
                {
                    case ClassIDType.Animator:
                        animatorList.Add(asset);
                        break;
                    case ClassIDType.AnimationClip when exportAllAnimations:
                        animationList?.Add(asset);
                        break;
                }
            }
            parsedAssetsList = animatorList;
            Logger.Info($"Found {parsedAssetsList.Count} exportable Animator asset(s).");
            if (parsedAssetsList.Count > 0 && CLIOptions.filterBy != FilterBy.None)
            {
                FilterAssets();
            }

            var savePath = CLIOptions.o_outputFolder.Value;
            var toExportCount = parsedAssetsList.Count;
            var exportedCount = 0;
            Progress.Reset();
            foreach (var asset in parsedAssetsList)
            {
                var isExported = false;
                Logger.Info($"[{exportedCount + 1}/{toExportCount}] Exporting \"{asset.Text}\"...");
                try
                {
                    Logger.Debug($"Animator Export: {asset.Type} : {asset.Container} : {asset.Text}");
                    isExported = Exporter.ExportAnimator(asset, savePath, animationList);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{asset.SourceFile.originalPath ?? asset.SourceFile.fullName}: [{$"{asset.Type}: {asset.Text}".Color(Ansi.BrightRed)}] : Export error\n{ex}");
                }
                if (isExported)
                {
                    exportedCount++;
                }
            }
            var status = exportedCount > 0
                ? $"Finished exporting [{exportedCount}/{toExportCount}] Animator asset(s) to \"{CLIOptions.o_outputFolder.Value.Color(Ansi.BrightCyan)}\""
                : "Nothing exported";
            Logger.Default.Log(LoggerEvent.Info, status, ignoreLevel: true);
        }

        private static Dictionary<MonoBehaviour, string> GenerateMocPathDict(Dictionary<MonoBehaviour, CubismModel> mocDict, bool searchByFilename)
        {
            var tempMocPathDict = new Dictionary<MonoBehaviour, (string, string)>();
            var mocPathDict = new Dictionary<MonoBehaviour, string>();
            foreach (var mocMono in l2dAssetsManager.ModelDict.Keys)
            {
                if (containers.TryGetValue(mocMono, out var fullContainerPath))
                {
                    var pathSepIndex = fullContainerPath.LastIndexOf('/');
                    var basePath = pathSepIndex > 0
                        ? fullContainerPath.Substring(0, pathSepIndex)
                        : fullContainerPath;
                    tempMocPathDict.Add(mocMono, (fullContainerPath, basePath));
                }
                else if (searchByFilename)
                {
                    tempMocPathDict.Add(mocMono, (mocMono.assetsFile.fullName, mocMono.assetsFile.fullName));
                }
            }

            if (tempMocPathDict.Count > 0)
            {
                var basePathSet = tempMocPathDict.Values.Select(x => x.Item2).ToHashSet();
                var useFullContainerPath = tempMocPathDict.Count != basePathSet.Count;
                foreach (var moc in mocDict.Keys)
                {
                    var mocPath = useFullContainerPath
                        ? tempMocPathDict[moc].Item1 //fullContainerPath
                        : tempMocPathDict[moc].Item2; //basePath
                    if (searchByFilename)
                    {
                        mocPathDict.Add(moc, moc.assetsFile.fullName);
                        if (mocDict.TryGetValue(moc, out var model) && model != null)
                            model.Container = mocPath;
                    }
                    else
                    {
                        mocPathDict.Add(moc, mocPath);
                    }
                }
                tempMocPathDict.Clear();
            }
            return mocPathDict;
        }

        private static Dictionary<MonoBehaviour, List<AssetStudio.Object>> GenerateMocAssetDict(bool isFiltered)
        {
            var searchByFilename = CLIOptions.f_l2dAssetSearchByFilename.Value;
            var l2dContainers = searchByFilename
                ? new Dictionary<AssetStudio.Object, string>()
                : containers;
            var mocDict = isFiltered
                ? FilterMocDict()
                : l2dAssetsManager.ModelDict;

            var mode = searchByFilename ? "file names" : "containers";
            Logger.Info($"Searching for Live2D assets using {mode}...");

            var mocPathDict = GenerateMocPathDict(mocDict, searchByFilename);
            if (!searchByFilename && mocPathDict.Count != mocDict.Count)
            {
                Logger.Warning($"Some Live2D models cannot be exported using containers\nTry to specify \"{"--l2d-search-by-filename".Color(Ansi.BrightCyan)}\" flag");
            }

            if (searchByFilename)
            {
                foreach (var asset in parsedAssetsList)
                {
                    switch (asset.Type)
                    {
                        case ClassIDType.AnimationClip:
                        case ClassIDType.Texture2D:
                        case ClassIDType.MonoBehaviour:
                            l2dContainers[asset.Asset] = asset.Asset.assetsFile.fullName;
                            break;
                    }
                }
            }

            var assetDict = new Dictionary<MonoBehaviour, List<AssetStudio.Object>>();
            foreach (var mocKvp in mocPathDict)
            {
                var mocPath = searchByFilename
                    ? mocKvp.Key.assetsFile.fullName
                    : mocKvp.Value;
                var result = l2dContainers.Select(assetKvp =>
                {
                    if (!assetKvp.Value.Contains(mocPath))
                        return null;
                    var mocPathSpan = mocPath.AsSpan();
                    var modelNameFromPath = mocPathSpan.Slice(mocPathSpan.LastIndexOf('/') + 1);
#if NET9_0_OR_GREATER
                    foreach (var range in assetKvp.Value.AsSpan().Split('/'))
                    {
                        if (modelNameFromPath.SequenceEqual(assetKvp.Value.AsSpan()[range]))
                            return assetKvp.Key;
                    }
#else
                    foreach (var str in assetKvp.Value.Split('/'))
                    {
                        if (modelNameFromPath.SequenceEqual(str.AsSpan()))
                            return assetKvp.Key;
                    }
#endif
                    return null;
                }).Where(x => x != null).ToList();

                if (result.Count > 0)
                {
                    assetDict[mocKvp.Key] = result;
                }
            }
            if (searchByFilename)
                l2dContainers.Clear();

            return assetDict;
        }

        private static Dictionary<MonoBehaviour, CubismModel> FilterMocDict()
        {
            var regexMode = CLIOptions.f_filterWithRegex.Value;
            var filterList = CLIOptions.o_filterByText.Value;
            var filteredDict = new Dictionary<MonoBehaviour, CubismModel>();
            var regex = regexMode ? new Regex(filterList[0]) : null;
            foreach (var kvp in l2dAssetsManager.ModelDict)
            {
                var mocName = kvp.Key.m_Name;
                var mocContainer = containers.TryGetValue(kvp.Key, out var container)
                    ? container
                    : "";
                if (regexMode)
                {
                    if (regex.IsMatch(mocContainer) || regex.IsMatch(mocName))
                        filteredDict[kvp.Key] = kvp.Value;
                    continue;
                }
                if (filterList.Any(str => mocContainer.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0 || mocName.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    filteredDict[kvp.Key] = kvp.Value;
                }
            }
            var log = regexMode
                ? $"Found {filteredDict.Count} Live2D model(s) which names or containers match {regex.ToString().Color(Ansi.BrightYellow)} regexp"
                : $"Found {filteredDict.Count} Live2D model(s) that contain {$"\"{string.Join("\", \"", filterList)}\"".Color(Ansi.BrightYellow)} in their names or containers";
            Logger.Info(log);

            return filteredDict;
        }

        public static void ExportLive2D()
        {
            if (l2dAssetsManager.ModelDict.Count == 0)
            {
                Logger.Default.Log(LoggerEvent.Info, "Live2D Cubism models were not found.", ignoreLevel: true);
                return;
            }
            var baseDestPath = Path.Combine(CLIOptions.o_outputFolder.Value, "Live2DOutput");
            var modelGroupOption = CLIOptions.o_l2dGroupOption.Value;
            var l2dOptions = new Live2DOptions()
            {
                MotionMode = CLIOptions.o_l2dMotionMode.Value,
                ForceBezier = CLIOptions.f_l2dForceBezier.Value,
                SearchByFilename = CLIOptions.f_l2dAssetSearchByFilename.Value,
            };
            if (l2dAssetsManager.ModelDict.Keys.First().serializedType?.m_Type == null && CLIOptions.o_assemblyPath.Value == "")
            {
                Logger.Warning("Specifying the assembly folder may be needed for proper extraction\n" +
                               $"Use \"{"--assembly-folder <path>".Color(Ansi.BrightCyan)}\" to specify it");
            }
            Progress.Reset();

            var assetDict = GenerateMocAssetDict(CLIOptions.filterBy == FilterBy.NameOrContainer);
            var totalModelCount = assetDict.Count;
            Logger.Info($"Found {totalModelCount} model(s).");
            var parallelTaskCount = CLIOptions.o_maxParallelExportTasks.Value;
            var modelCounter = 0;
            Live2DExtractor.Assembly = assemblyLoader;
            foreach (var assetGroupKvp in assetDict)
            {
                var mocMono = assetGroupKvp.Key;
                var srcContainer = containers.TryGetValue(mocMono, out var result)
                    ? result
                    : mocMono.assetsFile.fullName;
                l2dAssetsManager.ModelDict.TryGetValue(mocMono, out var cubismModel);

                Logger.Info($"[{modelCounter + 1}/{totalModelCount}] Exporting Live2D from: \"{srcContainer.Color(Ansi.BrightCyan)}\"...");
                try
                {
                    var cubismExtractor = new Live2DExtractor(assetGroupKvp.Value, cubismModel, l2dOptions);
                    var fileName = string.IsNullOrEmpty(mocMono.assetsFile.originalPath)
                        ? Path.GetFileNameWithoutExtension(mocMono.assetsFile.fileName)
                        : Path.GetFileNameWithoutExtension(mocMono.assetsFile.originalPath);
                    var modelName = !string.IsNullOrEmpty(cubismModel?.Name)
                        ? cubismModel.Name
                        : fileName;
                    Logger.Info($"Model name: \"{modelName}\"");

                    string modelPath;
                    switch (modelGroupOption)
                    {
                        case Live2DModelGroupOption.SourceFileName:
                            modelPath = fileName;
                            break;
                        case Live2DModelGroupOption.ModelName:
                            modelPath = modelName;
                            break;
                        default: //ContainerPath
                            var container = l2dOptions.SearchByFilename && cubismModel != null
                                ? cubismModel.Container
                                : srcContainer;
                            container = container == mocMono.assetsFile.fullName
                                ? fileName
                                : container;
                            modelPath = Path.HasExtension(container)
                                ? container.Replace(Path.GetExtension(container), "")
                                : container;
                            break;
                    }

                    var destPath = Exporter.SafePathCombine(baseDestPath, modelPath);
                    cubismExtractor.ExtractCubismModel(mocMono, destPath, parallelTaskCount);
                    modelCounter++;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Live2D model export error: \"{srcContainer}\"", ex);
                }
                Progress.Report(modelCounter, totalModelCount);
            }

            var status = modelCounter > 0 ?
                $"Finished exporting [{modelCounter}/{totalModelCount}] Live2D model(s) to \"{CLIOptions.o_outputFolder.Value.Color(Ansi.BrightCyan)}\"" :
                "Nothing exported.";
            Logger.Default.Log(LoggerEvent.Info, status, ignoreLevel: true);
        }

        public static void Clear()
        {
            assetsManager.Clear();
            assemblyLoader.Clear();
        }
    }
}
