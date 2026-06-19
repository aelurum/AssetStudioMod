using AssetStudio;
using AssetStudio.Addons.CubismLive2D;
using CubismLive2DExtractor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Object = AssetStudio.Object;

namespace AssetStudioGUI
{
    internal enum GuiColorTheme
    {
        System,
        Light,
        Dark
    }

    internal enum ExportType
    {
        Convert,
        Raw,
        Dump,
        JsonDump,
    }

    internal enum ExportFilter
    {
        All,
        Selected,
        Filtered
    }

    internal enum ExportL2DFilter
    {
        All,
        Selected,
        SelectedWithFadeList,
        SelectedWithFade,
        SelectedWithClips,
    }

    internal enum ExportListType
    {
        XML
    }

    internal enum AssetGroupOption
    {
        TypeName,
        ContainerPath,
        ContainerPathFull,
        SourceFileName,
        SceneHierarchy,
    }

    internal enum ListSearchFilterMode
    {
        Include,
        Exclude,
        RegexName,
        RegexContainer,
    }

    [Flags]
    internal enum SelectedAssetType
    {
        Animator = 0x01,
        AnimationClip = 0x02,
        MonoBehaviourMoc = 0x04,
        MonoBehaviourFade = 0x08,
        MonoBehaviourFadeLst = 0x10
    }

    internal static class Studio
    {
        public static AssetsManager assetsManager = new AssetsManager();
        public static AssemblyLoader assemblyLoader = new AssemblyLoader();
        public static List<AssetItem> exportableAssets = new List<AssetItem>();
        public static List<AssetItem> visibleAssets = new List<AssetItem>();
        public static L2DAssetManager l2dAssetManager = new L2DAssetManager();
        internal static Action<string> StatusStripUpdate = x => { };
        internal static Fbx.Settings FbxSettings;

        public static int ExtractFolder(string path, string savePath)
        {
            int extractedCount = 0;
            Progress.Reset();
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileOriPath = Path.GetDirectoryName(file);
                var fileSavePath = fileOriPath.Replace(path, savePath);
                extractedCount += ExtractFile(file, fileSavePath);
                Progress.Report(i + 1, files.Length);
            }
            return extractedCount;
        }

        public static int ExtractFile(string[] fileNames, string savePath)
        {
            int extractedCount = 0;
            Progress.Reset();
            for (var i = 0; i < fileNames.Length; i++)
            {
                var fileName = fileNames[i];
                extractedCount += ExtractFile(fileName, savePath);
                Progress.Report(i + 1, fileNames.Length);
            }
            return extractedCount;
        }

        public static int ExtractFile(string fileName, string savePath)
        {
            int extractedCount = 0;
            var reader = new FileReader(fileName);
            if (reader.FileType == FileType.BundleFile)
                extractedCount += ExtractBundleFile(reader, savePath);
            else if (reader.FileType == FileType.WebFile)
                extractedCount += ExtractWebDataFile(reader, savePath);
            else
                reader.Dispose();
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

        public static (string, List<TreeNode>) BuildAssetData()
        {
            Logger.Info("Building asset list...");

            string productName = null;
            var objectCount = assetsManager.AssetsFileList.Sum(x => x.Objects.Count);
            var objectAssetItemDic = new Dictionary<Object, AssetItem>(objectCount);
            var containers = new List<(PPtr<Object>, string)>();
            var tex2dArrayAssetList = new List<AssetItem>();
            var i = 0;
            Progress.Reset();
            foreach (var assetsFile in assetsManager.AssetsFileList)
            {
                var preloadTable = new List<PPtr<Object>>();

                foreach (var asset in assetsFile.Objects)
                {
                    var assetItem = new AssetItem(asset);
                    objectAssetItemDic.Add(asset, assetItem);
                    var exportable = false;
                    switch (asset)
                    {
                        case PreloadData m_PreloadData:
                            preloadTable = m_PreloadData.m_Assets;
                            break;
                        case GameObject m_GameObject:
                            assetItem.Text = m_GameObject.m_Name;
                            if (m_GameObject.CubismModel != null)
                            {
                                l2dAssetManager.BindCubismAssetsByGO(m_GameObject, assemblyLoader);
                            }
                            break;
                        case Texture2D m_Texture2D:
                            if (!string.IsNullOrEmpty(m_Texture2D.m_StreamData?.path))
                                assetItem.FullSize = asset.byteSize + m_Texture2D.m_StreamData.size;
                            assetItem.Text = m_Texture2D.m_Name;
                            exportable = true;
                            break;
                        case Texture2DArray m_Texture2DArray:
                            if (!string.IsNullOrEmpty(m_Texture2DArray.m_StreamData?.path))
                                assetItem.FullSize = asset.byteSize + m_Texture2DArray.m_StreamData.size;
                            assetItem.Text = m_Texture2DArray.m_Name;
                            tex2dArrayAssetList.Add(assetItem);
                            exportable = true;
                            break;
                        case AudioClip m_AudioClip:
                            if (!string.IsNullOrEmpty(m_AudioClip.m_Source))
                                assetItem.FullSize = asset.byteSize + m_AudioClip.m_Size;
                            assetItem.Text = m_AudioClip.m_Name;
                            exportable = true;
                            break;
                        case VideoClip m_VideoClip:
                            if (!string.IsNullOrEmpty(m_VideoClip.m_OriginalPath))
                                assetItem.FullSize = asset.byteSize + m_VideoClip.m_ExternalResources.m_Size;
                            assetItem.Text = m_VideoClip.m_Name;
                            exportable = true;
                            break;
                        case Shader m_Shader:
                            assetItem.Text = m_Shader.m_ParsedForm?.m_Name ?? m_Shader.m_Name;
                            exportable = true;
                            break;
                        case Mesh _:
                        case TextAsset _:
                        case AnimationClip _:
                        case Font _:
                        case MovieTexture _:
                        case Sprite _:
                            assetItem.Text = ((NamedObject)asset).m_Name;
                            exportable = true;
                            break;
                        case Animator m_Animator:
                            if (m_Animator.m_GameObject.TryGet(out var gameObject))
                            {
                                assetItem.Text = gameObject.m_Name;
                            }
                            exportable = true;
                            break;
                        case MonoBehaviour m_MonoBehaviour:
                            var assetName = m_MonoBehaviour.m_Name;
                            if (m_MonoBehaviour.m_Script.TryGet(out var m_Script))
                            {
                                assetName = assetName == "" ? m_Script.m_ClassName : assetName;
                                switch (m_Script.m_ClassName)
                                {
                                    case "CubismMoc":
                                    case "CubismRenderer":
                                    case "CubismDisplayInfoParameterName":
                                    case "CubismDisplayInfoPartName":
                                    case "CubismPosePart":
                                        l2dAssetManager.BindCubismAssetsByMono(m_MonoBehaviour, m_Script.m_ClassName);
                                        break;
                                }
                            }
                            assetItem.Text = assetName;
                            exportable = true;
                            break;
                        case PlayerSettings m_PlayerSettings:
                            productName = m_PlayerSettings.productName;
                            break;
                        case AssetBundle m_AssetBundle:
                            var isStreamedSceneAssetBundle = m_AssetBundle.m_IsStreamedSceneAssetBundle;
                            if (!isStreamedSceneAssetBundle)
                            {
                                preloadTable = m_AssetBundle.m_PreloadTable;
                            }
                            assetItem.Text = string.IsNullOrEmpty(m_AssetBundle.m_AssetBundleName)
                                ? m_AssetBundle.m_Name
                                : m_AssetBundle.m_AssetBundleName;
                            foreach (var m_Container in m_AssetBundle.m_Container)
                            {
                                var preloadIndex = m_Container.Value.preloadIndex;
                                var preloadSize = isStreamedSceneAssetBundle
                                    ? preloadTable.Count
                                    : m_Container.Value.preloadSize;
                                var preloadEnd = preloadIndex + preloadSize;
                                for (var k = preloadIndex; k < preloadEnd; k++)
                                {
                                    containers.Add((preloadTable[k], m_Container.Key));
                                }
                            }
                            break;
                        case ResourceManager m_ResourceManager:
                            foreach (var m_Container in m_ResourceManager.m_Container)
                            {
                                containers.Add((m_Container.Value, m_Container.Key));
                            }
                            break;
                        case NamedObject m_NamedObject:
                            assetItem.Text = m_NamedObject.m_Name;
                            break;
                    }
                    if (assetItem.Text == "")
                    {
                        assetItem.Text = assetItem.TypeString + assetItem.UniqueID;
                    }
                    if (Properties.Settings.Default.displayAll || exportable)
                    {
                        exportableAssets.Add(assetItem);
                    }
                    asset.Name = assetItem.Text;
                    Progress.Report(++i, objectCount);
                }
            }
            foreach (var (pptr, container) in containers)
            {
                if (pptr.TryGet(out var obj))
                {
                    objectAssetItemDic[obj].Container = container;
                    switch (obj)
                    {
                        case GameObject m_GameObject:
                            if (m_GameObject.CubismModel != null)
                            {
                                m_GameObject.CubismModel.Container = container;
                            }
                            break;
                        case AnimationClip _:
                        case Texture2D _:
                        case MonoBehaviour _:
                            l2dAssetManager.AssetContainers[obj] = container;
                            break;
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

                    var fakeItem = new AssetItem(fakeObj)
                    {
                        Text = fakeObj.m_Name,
                        Container = tex2dAssetItem.Container
                    };
                    exportableAssets.Add(fakeItem);
                }
            }
            foreach (var tmp in exportableAssets)
            {
                tmp.SetSubItems();
            }
            containers.Clear();
            tex2dArrayAssetList.Clear();

            visibleAssets = exportableAssets;

            if (!Properties.Settings.Default.buildTreeStructure)
            {
                Logger.Info("Building tree structure step is skipped");
                objectAssetItemDic.Clear();
                return (productName, new List<TreeNode>());
            }

            Logger.Info("Building tree structure...");

            var treeNodeCollection = new List<TreeNode>();
            var treeNodeDictionary = new Dictionary<GameObject, GameObjectTreeNode>();
            var assetsFileCount = assetsManager.AssetsFileList.Count;
            var j = 0;
            Progress.Reset();
            foreach (var assetsFile in assetsManager.AssetsFileList)
            {
                var fileNode = new TreeNode(assetsFile.fileName); //RootNode

                foreach (var obj in assetsFile.Objects)
                {
                    if (obj is GameObject m_GameObject)
                    {
                        if (!treeNodeDictionary.TryGetValue(m_GameObject, out var currentNode))
                        {
                            currentNode = new GameObjectTreeNode(m_GameObject);
                            treeNodeDictionary.Add(m_GameObject, currentNode);
                        }

                        foreach (var pptr in m_GameObject.m_Components)
                        {
                            if (pptr.TryGet(out var m_Component))
                            {
                                objectAssetItemDic[m_Component].TreeNode = currentNode;
                                if (m_Component is MeshFilter m_MeshFilter)
                                {
                                    if (m_MeshFilter.m_Mesh.TryGet(out var m_Mesh))
                                    {
                                        objectAssetItemDic[m_Mesh].TreeNode = currentNode;
                                    }
                                }
                                else if (m_Component is SkinnedMeshRenderer m_SkinnedMeshRenderer)
                                {
                                    if (m_SkinnedMeshRenderer.m_Mesh.TryGet(out var m_Mesh))
                                    {
                                        objectAssetItemDic[m_Mesh].TreeNode = currentNode;
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
                                        parentGameObjectNode = new GameObjectTreeNode(parentGameObject);
                                        treeNodeDictionary.Add(parentGameObject, parentGameObjectNode);
                                    }
                                    parentNode = parentGameObjectNode;
                                }
                            }
                        }
                        parentNode.Nodes.Add(currentNode);
                    }
                }

                if (fileNode.Nodes.Count > 0)
                {
                    treeNodeCollection.Add(fileNode);
                }

                Progress.Report(++j, assetsFileCount);
            }
            treeNodeDictionary.Clear();
            objectAssetItemDic.Clear();

            return (productName, treeNodeCollection);
        }

        public static Dictionary<UnityVersion, SortedDictionary<int, TypeTreeItem>> BuildClassStructure()
        {
            var typeMap = new Dictionary<UnityVersion, SortedDictionary<int, TypeTreeItem>>();
            foreach (var assetsFile in assetsManager.AssetsFileList)
            {
                if (typeMap.TryGetValue(assetsFile.version, out var curVer))
                {
                    foreach (var type in assetsFile.m_Types.Where(x => x.m_Type != null))
                    {
                        var key = type.classID;
                        if (type.m_ScriptTypeIndex >= 0)
                        {
                            key = -1 - type.m_ScriptTypeIndex;
                        }
                        curVer[key] = new TypeTreeItem(key, type.m_Type);
                    }
                }
                else
                {
                    var items = new SortedDictionary<int, TypeTreeItem>();
                    foreach (var type in assetsFile.m_Types.Where(x => x.m_Type != null))
                    {
                        var key = type.classID;
                        if (type.m_ScriptTypeIndex >= 0)
                        {
                            key = -1 - type.m_ScriptTypeIndex;
                        }
                        items[key] = new TypeTreeItem(key, type.m_Type);
                    }
                    typeMap.Add(assetsFile.version, items);
                }
            }
            return typeMap;
        }

        public static void ExportAssets(string savePath, List<AssetItem> toExportAssets, ExportType exportType)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                var groupOption = (AssetGroupOption)Properties.Settings.Default.assetGroupOption;
                var toExportAssetDict = new ConcurrentDictionary<AssetItem, string>();
                var toParallelExportAssetDict = new ConcurrentDictionary<AssetItem, string>();
                var exceptionMsgs = new ConcurrentDictionary<Exception, string>();
                var parallelExportCount = GetParallelExportCount();
                var mode = exportType == ExportType.Dump || exportType == ExportType.JsonDump ? "Dump" : "Export";
                var toExportCount = toExportAssets.Count;
                var exportedCount = 0;
                var i = 0;
                Progress.Reset();

                Parallel.ForEach(toExportAssets, asset =>
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
                                exportPath = Path.Combine(savePath, Path.GetDirectoryName(asset.Container));
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
                            if (asset.TreeNode != null)
                            {
                                exportPath = Exporter.SafePathCombine(savePath, asset.TreeNode.FullPath);
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

                    if (exportType == ExportType.Convert)
                    {
                        switch (asset.Type)
                        {
                            case ClassIDType.Texture2D:
                            case ClassIDType.Texture2DArrayImage:
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
                        Logger.Info($"[{exportedCount + 1}/{toExportCount}] {mode}ing {asset.TypeString}: {asset.Text}");
                        switch (exportType)
                        {
                            case ExportType.Raw:
                                isExported = Exporter.ExportRawFile(asset, exportPath);
                                break;
                            case ExportType.Dump:
                                isExported = Exporter.ExportDumpFile(asset, exportPath);
                                break;
                            case ExportType.JsonDump:
                                isExported = Exporter.ExportJsonDumpFile(asset, exportPath);
                                break;
                            case ExportType.Convert:
                                isExported = Exporter.ExportConvertFile(asset, exportPath);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"{mode} {asset.TypeString}: {asset.Text} error", ex);
                    }

                    if (isExported)
                    {
                        exportedCount++;
                    }
                    else
                    {
                        Logger.Warning($"Unable to {mode.ToLower()} {asset.TypeString}: {asset.Text}");
                    }

                    Progress.Report(++i, toExportCount);
                }
                Exporter.ClearHash();

                Parallel.ForEach(toParallelExportAssetDict, new ParallelOptions { MaxDegreeOfParallelism = parallelExportCount }, (toExportAsset, loopState) =>
                {
                    var asset = toExportAsset.Key;
                    var exportPath = toExportAsset.Value;
                    try
                    {
                        if (ParallelExporter.ParallelExportConvertFile(asset, exportPath, out var debugLog))
                        {
                            Interlocked.Increment(ref exportedCount);
                            if (GUILogger.ShowDebugMessage)
                            {
                                Logger.Debug(debugLog);
                                StatusStripUpdate($"[{exportedCount}/{toExportCount}] Exported {asset.TypeString}: {asset.Text}");
                            }
                            else
                            {
                                Logger.Info($"[{exportedCount}/{toExportCount}] Exported {asset.TypeString}: {asset.Text}");
                            }
                        }
                        Interlocked.Increment(ref i);
                        Progress.Report(i, toExportCount);
                    }
                    catch (Exception ex)
                    {
                        if (parallelExportCount == 1)
                        {
                            Logger.Error($"{mode} {asset.TypeString}: {asset.Text} error", ex);
                        }
                        else
                        {
                            loopState.Break();
                            exceptionMsgs.TryAdd(ex, $"Exception occurred when exporting {asset.TypeString}: {asset.Text}\n{ex}\n");
                        }
                    }
                });
                ParallelExporter.ClearHash();

                foreach (var ex in exceptionMsgs)
                {
                    Logger.Error(ex.Value);
                }

                var statusText = exportedCount == 0
                    ? "Nothing exported."
                    : $"Finished {mode.ToLower()}ing [{exportedCount}/{toExportCount}] assets.";
                if (toExportCount > exportedCount)
                {
                    statusText += exceptionMsgs.IsEmpty
                        ? $" {toExportCount - exportedCount} assets skipped (not extractable or files already exist)."
                        : " Export process was stopped because one or more exceptions occurred.";
                    Progress.Report(toExportCount, toExportCount);
                }
                Logger.Info(statusText);
                exceptionMsgs.Clear();

                if (Properties.Settings.Default.openAfterExport && exportedCount > 0)
                {
                    OpenFolderInExplorer(savePath);
                }
            });
        }

        public static void ExportAssetsList(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Progress.Reset();

                switch (exportListType)
                {
                    case ExportListType.XML:
                        var filename = Path.Combine(savePath, "assets.xml");
                        var doc = new XDocument(
                            new XElement("Assets",
                                new XAttribute("filename", filename),
                                new XAttribute("createdAt", DateTime.UtcNow.ToString("s")),
                                toExportAssets.Select(
                                    asset => new XElement("Asset",
                                        new XElement("Name", asset.Text),
                                        new XElement("Container", asset.Container),
                                        new XElement("Type", new XAttribute("id", (int)asset.Type), asset.TypeString),
                                        new XElement("PathID", asset.m_PathID),
                                        new XElement("Source", asset.SourceFile.fullName),
                                        new XElement("TreeNode", asset.TreeNode != null ? asset.TreeNode.FullPath : ""),
                                        new XElement("Size", asset.FullSize)
                                    )
                                )
                            )
                        );

                        doc.Save(filename);

                        break;
                }

                var statusText = $"Finished exporting asset list with {toExportAssets.Count} items.";

                Logger.Info(statusText);

                if (Properties.Settings.Default.openAfterExport && toExportAssets.Count > 0)
                {
                    OpenFolderInExplorer(savePath);
                }
            });
        }

        public static void ExportSplitObjects(string savePath, TreeNodeCollection nodes)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var count = nodes.Cast<TreeNode>().Sum(x => x.Nodes.Count);
                int k = 0;
                Progress.Reset();
                foreach (TreeNode node in nodes)
                {
                    //遍历一级子节点
                    foreach (GameObjectTreeNode j in node.Nodes)
                    {
                        //收集所有子节点
                        var gameObjects = new List<GameObject>();
                        CollectNode(j, gameObjects);
                        //跳过一些不需要导出的object
                        if ((j.gameObject.m_Transform == null && j.gameObject.m_Animator == null)
                            || gameObjects.All(x => x.m_SkinnedMeshRenderer == null && x.m_MeshFilter == null))
                        {
                            Progress.Report(++k, count);
                            continue;
                        }
                        //处理非法文件名
                        var fileName = j.Text;
                        //每个文件存放在单独的文件夹
                        var targetPath = $"{Exporter.SafeFilePath(savePath, fileName)}{Path.DirectorySeparatorChar}";
                        //重名文件处理
                        for (int i = 1;; i++)
                        {
                            if (Directory.Exists(targetPath))
                            {
                                targetPath = $"{Exporter.SafeFilePath(savePath, fileName)} ({i}){Path.DirectorySeparatorChar}";
                            }
                            else
                            {
                                break;
                            }
                        }
                        Directory.CreateDirectory(targetPath);
                        //导出FBX
                        Logger.Info($"Exporting {fileName}.fbx");
                        try
                        {
                            Exporter.ExportGameObject(j.gameObject, targetPath);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Export GameObject:{j.Text} error", ex);
                        }

                        Progress.Report(++k, count);
                        Logger.Info($"Finished exporting {fileName}.fbx");
                    }
                }
                if (Properties.Settings.Default.openAfterExport)
                {
                    OpenFolderInExplorer(savePath);
                }
                Logger.Info("Finished");
            });
        }

        private static void CollectNode(GameObjectTreeNode node, List<GameObject> gameObjects)
        {
            gameObjects.Add(node.gameObject);
            foreach (GameObjectTreeNode i in node.Nodes)
            {
                CollectNode(i, gameObjects);
            }
        }

        public static void ExportAnimatorWithAnimationClip(AssetItem animator, List<AssetItem> animationList, string exportPath)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Progress.Reset();
                Logger.Info($"Exporting {animator.Text}");
                try
                {
                    Exporter.ExportAnimator(animator, exportPath, animationList);
                    if (Properties.Settings.Default.openAfterExport)
                    {
                        OpenFolderInExplorer(exportPath);
                    }
                    Progress.Report(1, 1);
                    Logger.Info($"Finished exporting {animator.Text}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Export Animator:{animator.Text} error", ex);
                    Logger.Info("Error in export");
                }
            });
        }

        public static void ExportObjectsWithAnimationClip(string exportPath, TreeNodeCollection nodes, List<AssetItem> animationList = null)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var gameObjects = new List<GameObject>();
                GetSelectedParentNode(nodes, gameObjects);
                if (gameObjects.Count > 0)
                {
                    var count = gameObjects.Count;
                    int i = 0;
                    Progress.Reset();
                    foreach (var gameObject in gameObjects)
                    {
                        var fileName = gameObject.m_Name;
                        Logger.Info($"Exporting {fileName}");
                        try
                        {
                            var modelExportPath = $"{Exporter.SafeFilePath(exportPath, fileName)}{Path.DirectorySeparatorChar}";
                            for (var j = 1;; j++)
                            {
                                if (Directory.Exists(modelExportPath))
                                {
                                    modelExportPath = $"{Exporter.SafeFilePath(exportPath, fileName)} ({i}){Path.DirectorySeparatorChar}";
                                }
                                else
                                {
                                    break;
                                }
                            }
                            Exporter.ExportGameObject(gameObject, modelExportPath, animationList);
                            Logger.Info($"Finished exporting {fileName}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Export GameObject:{fileName} error", ex);
                            Logger.Info("Error in export");
                        }

                        Progress.Report(++i, count);
                    }
                    if (Properties.Settings.Default.openAfterExport)
                    {
                        OpenFolderInExplorer(exportPath);
                    }
                }
                else
                {
                    Logger.Info("No Object selected for export.");
                }
            });
        }

        public static void ExportObjectsMergeWithAnimationClip(string exportPath, List<GameObject> gameObjects, List<AssetItem> animationList = null)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var name = Path.GetFileName(exportPath);
                Progress.Reset();
                Logger.Info($"Exporting {name}");
                try
                {
                    Exporter.ExportGameObjectMerge(gameObjects, exportPath, animationList);
                    Progress.Report(1, 1);
                    Logger.Info($"Finished exporting {name}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Export Model:{name} error", ex);
                    Logger.Info("Error in export");
                }
                if (Properties.Settings.Default.openAfterExport)
                {
                    OpenFolderInExplorer(Path.GetDirectoryName(exportPath));
                }
            });
        }

        public static void GetSelectedParentNode(TreeNodeCollection nodes, List<GameObject> gameObjects)
        {
            foreach (TreeNode i in nodes)
            {
                if (i is GameObjectTreeNode gameObjectTreeNode && i.Checked)
                {
                    gameObjects.Add(gameObjectTreeNode.gameObject);
                }
                else
                {
                    GetSelectedParentNode(i.Nodes, gameObjects);
                }
            }
        }

        public static TypeTree MonoBehaviourToTypeTree(MonoBehaviour m_MonoBehaviour)
        {
            SelectAssemblyFolder();
            return m_MonoBehaviour.ConvertToTypeTree(assemblyLoader);
        }

        private static void SelectAssemblyFolder()
        {
            if (!assemblyLoader.Loaded)
            {
                var openFolderDialog = new OpenFolderDialog();
                openFolderDialog.Title = "Select Assembly Folder";
                if (openFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    assemblyLoader.Load(openFolderDialog.Folder);
                }
                else
                {
                    assemblyLoader.Loaded = true;
                }
            }
        }

        public static string DumpAsset(Object obj)
        {
            var str = obj.Dump();
            if (str == null && obj is MonoBehaviour m_MonoBehaviour)
            {
                var type = MonoBehaviourToTypeTree(m_MonoBehaviour);
                str = m_MonoBehaviour.Dump(type);
            }
            if (string.IsNullOrEmpty(str))
            {
                str = obj.DumpObject();
            }
            return str;
        }

        public static JsonDocument DumpAssetToJsonDoc(Object obj)
        {
            if (obj == null)
                return null;

            if (obj is MonoBehaviour m_MonoBehaviour)
            {
                var type = obj.serializedType?.m_Type ?? MonoBehaviourToTypeTree(m_MonoBehaviour);
                return m_MonoBehaviour.ToJsonDoc(type);
            }
            return obj.ToJsonDoc();
        }

        public static void OpenFolderInExplorer(string path)
        {
            if (!path.EndsWith($"{Path.DirectorySeparatorChar}"))
                path += Path.DirectorySeparatorChar;
            if (!Directory.Exists(path))
                return;

            var info = new ProcessStartInfo(path);
            info.UseShellExecute = true;
            Process.Start(info);
        }

        private static Dictionary<MonoBehaviour, string> GenerateMocPathDict(Dictionary<MonoBehaviour, CubismModel> mocDict, bool searchByFilename)
        {
            var tempMocPathDict = new Dictionary<MonoBehaviour, (string, string)>();
            var mocPathDict = new Dictionary<MonoBehaviour, string>();
            foreach (var mocMono in l2dAssetManager.ModelDict.Keys)
            {
                if (l2dAssetManager.AssetContainers.TryGetValue(mocMono, out var fullContainerPath))
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

        private static Dictionary<MonoBehaviour, List<Object>> GenerateMocAssetDict(Dictionary<MonoBehaviour, CubismModel> mocDict)
        {
            var searchByFilename = Properties.Settings.Default.l2dAssetSearchByFilename;
            var l2dContainers = searchByFilename
                ? new Dictionary<Object, string>()
                : l2dAssetManager.AssetContainers;

            var mode = searchByFilename ? "file names" : "containers";
            Logger.Info($"Searching for Live2D assets using {mode}...");

            var mocPathDict = GenerateMocPathDict(mocDict, searchByFilename);
            if (!searchByFilename && mocPathDict.Count != mocDict.Count)
            {
                Logger.Warning("Some Live2D models cannot be exported using containers\nTry to enable search by file name in the export options");
            }

            if (searchByFilename)
            {
                foreach (var asset in exportableAssets)
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

            var assetDict = new Dictionary<MonoBehaviour, List<Object>>();
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

        public static void ExportLive2D(string exportPath, List<MonoBehaviour> selMocs = null, List<AnimationClip> selClipMotions = null, List<MonoBehaviour> selFadeMotions = null, MonoBehaviour selFadeLst = null)
        {
            var baseDestPath = Path.Combine(exportPath, "Live2DOutput");
            var forceBezier = Properties.Settings.Default.l2dForceBezier;
            var modelGroupOption = Properties.Settings.Default.l2dModelGroupOption;
            var searchByFilename = Properties.Settings.Default.l2dAssetSearchByFilename;
            var motionMode = Properties.Settings.Default.l2dMotionMode;
            if (selClipMotions != null)
                motionMode = Live2DMotionMode.AnimationClipV2;
            else if (selFadeMotions != null || selFadeLst != null)
                motionMode = Live2DMotionMode.MonoBehaviour;
            var mocDict = selMocs != null
                ? selMocs.ToDictionary(moc => moc, moc => l2dAssetManager.ModelDict[moc])
                : l2dAssetManager.ModelDict;

            ThreadPool.QueueUserWorkItem(state =>
            {
                var assetDict = GenerateMocAssetDict(mocDict);
                if (mocDict.Keys.First().serializedType?.m_Type == null && !assemblyLoader.Loaded)
                {
                    Logger.Warning("Specifying the assembly folder may be needed for proper extraction");
                    SelectAssemblyFolder();
                }
                var totalModelCount = assetDict.Count;
                var modelCounter = 0;
                Live2DExtractor.MocDict = mocDict;
                Live2DExtractor.Assembly = assemblyLoader;
                foreach (var assetGroupKvp in assetDict)
                {
                    var srcContainer = l2dAssetManager.AssetContainers.TryGetValue(assetGroupKvp.Key, out var result)
                        ? result
                        : assetGroupKvp.Key.assetsFile.fullName;

                    Logger.Info($"[{modelCounter + 1}/{totalModelCount}] Exporting Live2D from: \"{srcContainer}\"...");
                    try
                    {
                        var cubismExtractor = new Live2DExtractor(assetGroupKvp, selClipMotions, selFadeMotions, selFadeLst);
                        var filename = string.IsNullOrEmpty(cubismExtractor.MocMono.assetsFile.originalPath)
                            ? Path.GetFileNameWithoutExtension(cubismExtractor.MocMono.assetsFile.fileName)
                            : Path.GetFileNameWithoutExtension(cubismExtractor.MocMono.assetsFile.originalPath);
                        var modelName = !string.IsNullOrEmpty(cubismExtractor.Model?.Name)
                            ? cubismExtractor.Model.Name
                            : filename;
                        Logger.Info($"Model name: \"{modelName}\"");

                        string modelPath;
                        switch (modelGroupOption)
                        {
                            case Live2DModelGroupOption.SourceFileName:
                                modelPath = filename;
                                break;
                            case Live2DModelGroupOption.ModelName:
                                modelPath = modelName;
                                break;
                            default: //ContainerPath
                                var container = searchByFilename && cubismExtractor.Model != null
                                    ? cubismExtractor.Model.Container
                                    : srcContainer;
                                container = container == assetGroupKvp.Key.assetsFile.fullName
                                    ? filename
                                    : container;
                                modelPath = Path.HasExtension(container)
                                    ? container.Replace(Path.GetExtension(container), "")
                                    : container;
                                break;
                        }
                        var destPath = Exporter.SafePathCombine(baseDestPath, modelPath);
                        cubismExtractor.ExtractCubismModel(destPath, motionMode, forceBezier, GetParallelExportCount());
                        modelCounter++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Live2D model export error: \"{srcContainer}\"", ex);
                    }
                    Progress.Report(modelCounter, totalModelCount);
                }

                Logger.Info($"Finished exporting [{modelCounter}/{totalModelCount}] Live2D model(s).");
                Progress.Report(1, 1);
                
                if (Properties.Settings.Default.openAfterExport && modelCounter > 0)
                {
                    OpenFolderInExplorer(exportPath);
                }
            });
        }

        private static int GetParallelExportCount()
        {
            var parallelExportCount = Properties.Settings.Default.parallelExportCount <= 0
                ? Environment.ProcessorCount - 1
                : Math.Min(Properties.Settings.Default.parallelExportCount, Environment.ProcessorCount - 1);

            return Properties.Settings.Default.parallelExport ? parallelExportCount : 1;
        }
    }
}
