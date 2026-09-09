using AssetStudio;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetStudioGUI
{
    internal static class Exporter
    {
        internal static readonly Fbx.Settings FbxSettings;
        private static readonly HashSet<string> ExportPathHashSet = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<char> InvalidChars = new HashSet<char>(Path.GetInvalidFileNameChars());

        static Exporter()
        {
            InvalidChars.UnionWith(new[] { '"', '<', '>', '|', ':', '*', '?', '\\', '/' });

            if (string.IsNullOrEmpty(Properties.Settings.Default.fbxSettings))
            {
                FbxSettings = new Fbx.Settings();
                Properties.Settings.Default.fbxSettings = FbxSettings.ToBase64();
                Properties.Settings.Default.Save();
            }
            else
            {
                FbxSettings = Fbx.Settings.FromBase64(Properties.Settings.Default.fbxSettings);
            }
        }

        private static bool TryExportFile(string dir, AssetItem item, string extension, out string fullPath, string mode = "Export")
        {
            var fileName = item.Text;
            var filenameFormatIndex = Properties.Settings.Default.filenameFormat;
            var canOverwrite = Properties.Settings.Default.overwriteExistingFiles;
            switch (filenameFormatIndex)
            {
                case 1: //assetName@pathID
                    fileName = $"{fileName} @{item.m_PathID}";
                    break;
                case 2: //pathID
                    fileName = item.m_PathID.ToString();
                    break;
            }
            fullPath = SafeFilePath(dir, $"{fileName}{extension}");
            if (ExportPathHashSet.Add(fullPath))
            {
                if (CanWrite(fullPath, dir, canOverwrite))
                {
                    return true;
                }
            }
            else if (filenameFormatIndex == 0) //assetName
            {
                fullPath = SafeFilePath(dir, $"{fileName}{item.UniqueID}{extension}");
                if (CanWrite(fullPath, dir, canOverwrite))
                {
                    return true;
                }
            }
            Logger.Warning($"{mode} failed. File \"{fullPath.Color(ColorConsole.BrightYellow)}\" already exist");
            return false;
        }

        private static bool CanWrite(string fullPath, string dir, bool canOverwrite)
        {
            if (!canOverwrite && File.Exists(fullPath)) 
                return false;
            Directory.CreateDirectory(dir);
            return true;
        }

        private static bool ExportVideoClip(AssetItem item, string exportPath)
        {
            var m_VideoClip = (VideoClip)item.Asset;
            if (m_VideoClip.m_ExternalResources.m_Size > 0)
            {
                if (!TryExportFile(exportPath, item, Path.GetExtension(m_VideoClip.m_OriginalPath), out var exportFullPath))
                    return false;
                m_VideoClip.m_VideoData.WriteData(exportFullPath);
                return true;
            }
            return false;
        }

        private static bool ExportMovieTexture(AssetItem item, string exportPath)
        {
            var m_MovieTexture = (MovieTexture)item.Asset;
            if (!TryExportFile(exportPath, item, ".ogv", out var exportFullPath))
                return false;
            File.WriteAllBytes(exportFullPath, m_MovieTexture.m_MovieData);
            return true;
        }

        private static bool ExportShader(AssetItem item, string exportPath)
        {
            if (item.Asset is Shader m_Shader)
            {
                if (!TryExportFile(exportPath, item, ".shader", out var exportFullPath))
                    return false;
                var str = m_Shader.Convert();
                File.WriteAllText(exportFullPath, str);
                return true;
            }
            return false;
        }

        private static bool ExportTextAsset(AssetItem item, string exportPath)
        {
            var m_TextAsset = (TextAsset)item.Asset;
            var extension = ".txt";
            if (Properties.Settings.Default.restoreExtensionName)
            {
                if (Path.HasExtension(m_TextAsset.m_Name))
                {
                    extension = "";
                }
                else
                {
                    var extFromContainer = Path.GetExtension(item.Container);
                    if (!string.IsNullOrEmpty(extFromContainer))
                    {
                        extension = extFromContainer;
                    }
                }
            }
            if (!TryExportFile(exportPath, item, extension, out var exportFullPath))
                return false;
            File.WriteAllBytes(exportFullPath, m_TextAsset.m_Script);
            return true;
        }

        private static bool ExportMonoBehaviour(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".json", out var exportFullPath))
                return false;
            var m_MonoBehaviour = (MonoBehaviour)item.Asset;
            var type = m_MonoBehaviour.ToType();
            if (type == null)
            {
                var m_Type = Studio.MonoBehaviourToTypeTree(m_MonoBehaviour);
                type = m_MonoBehaviour.ToType(m_Type);
            }
            var str = System.Text.Json.JsonSerializer.Serialize(type, Object.JsonOptions);
            File.WriteAllText(exportFullPath, str);
            return true;
        }

        private static bool ExportFont(AssetItem item, string exportPath)
        {
            if (item.Asset is Font m_Font && m_Font.m_FontData != null)
            {
                var extension = ".ttf";
                if (BinaryPrimitives.ReadInt32LittleEndian(m_Font.m_FontData) == 0x4F54544F) //"OTTO"
                {
                    extension = ".otf";
                }
                if (!TryExportFile(exportPath, item, extension, out var exportFullPath))
                    return false;
                File.WriteAllBytes(exportFullPath, m_Font.m_FontData);
                return true;
            }
            return false;
        }

        private static bool ExportMesh(AssetItem item, string exportPath)
        {
            var m_Mesh = (Mesh)item.Asset;
            m_Mesh.ProcessData();

            if (m_Mesh.m_VertexCount <= 0)
                return false;
            if (!TryExportFile(exportPath, item, ".obj", out var exportFullPath))
                return false;

            var sb = new StringBuilder();
            sb.AppendLine("g " + m_Mesh.m_Name);

            #region Vertices
            if (m_Mesh.m_Vertices == null || m_Mesh.m_Vertices.Length == 0)
            {
                return false;
            }
            int c = 3;
            if (m_Mesh.m_Vertices.Length == m_Mesh.m_VertexCount * 4)
            {
                c = 4;
            }
            for (int v = 0; v < m_Mesh.m_VertexCount; v++)
            {
                sb.AppendFormat("v {0} {1} {2}\r\n", -m_Mesh.m_Vertices[v * c], m_Mesh.m_Vertices[v * c + 1], m_Mesh.m_Vertices[v * c + 2]);
            }
            #endregion

            #region UV
            if (m_Mesh.m_UV0?.Length > 0)
            {
                c = 4;
                if (m_Mesh.m_UV0.Length == m_Mesh.m_VertexCount * 2)
                {
                    c = 2;
                }
                else if (m_Mesh.m_UV0.Length == m_Mesh.m_VertexCount * 3)
                {
                    c = 3;
                }
                for (int v = 0; v < m_Mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vt {0} {1}\r\n", m_Mesh.m_UV0[v * c], m_Mesh.m_UV0[v * c + 1]);
                }
            }
            #endregion

            #region Normals
            if (m_Mesh.m_Normals?.Length > 0)
            {
                if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 3)
                {
                    c = 3;
                }
                else if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 4)
                {
                    c = 4;
                }
                for (int v = 0; v < m_Mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vn {0} {1} {2}\r\n", -m_Mesh.m_Normals[v * c], m_Mesh.m_Normals[v * c + 1], m_Mesh.m_Normals[v * c + 2]);
                }
            }
            #endregion

            #region Face
            int sum = 0;
            for (var i = 0; i < m_Mesh.m_SubMeshes.Count; i++)
            {
                sb.AppendLine($"g {m_Mesh.m_Name}_{i}");
                int indexCount = (int)m_Mesh.m_SubMeshes[i].indexCount;
                var end = sum + indexCount / 3;
                for (int f = sum; f < end; f++)
                {
                    sb.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\r\n", m_Mesh.m_Indices[f * 3 + 2] + 1, m_Mesh.m_Indices[f * 3 + 1] + 1, m_Mesh.m_Indices[f * 3] + 1);
                }
                sum = end;
            }
            #endregion

            sb.Replace("NaN", "0");
            File.WriteAllText(exportFullPath, sb.ToString());
            return true;
        }

        public static bool ExportAnimator(AssetItem item, string exportPath, List<AssetItem> animationList = null)
        {
            var exportFullPath = SafeFilePath(exportPath, item.Text, item.Text + ".fbx");
            if (File.Exists(exportFullPath))
            {
                exportFullPath = SafeFilePath(exportPath, item.Text + item.UniqueID, item.Text + ".fbx");
            }
            if (!FbxSettings.ExportAnimations)
                animationList = new List<AssetItem>();
            var m_Animator = (Animator)item.Asset;
            var convert = animationList != null
                ? new ModelConverter(m_Animator, Properties.Settings.Default.convertType, animationList.Select(x => (AnimationClip)x.Asset).ToList())
                : new ModelConverter(m_Animator, Properties.Settings.Default.convertType);
            ExportFbx(convert, exportFullPath);
            return true;
        }

        private static void ExportFbx(ModelConverter convert, string exportPath)
        {
            ModelExporter.ExportFbx(exportPath, convert, FbxSettings);
            if (!FbxSettings.DumpMaterials || convert.MaterialAssetSet.Count == 0 || !File.Exists(exportPath))
                return;

            var exportDir = Path.Combine(Path.GetDirectoryName(exportPath), "Materials");
            Directory.CreateDirectory(exportDir);
            foreach (var mat in convert.MaterialAssetSet)
            {
                var jsonDoc = mat.ToJsonDoc();
                var str = System.Text.Json.JsonSerializer.Serialize(jsonDoc, Object.JsonOptions);
                var exportFullPath = SafeFilePath(exportDir, $"{mat.m_Name}.json");
                if (File.Exists(exportFullPath))
                {
                    exportFullPath = SafeFilePath(exportDir, $"{mat.m_Name} #{mat.GetHashCode():x}.json");
                }
                File.WriteAllText(exportFullPath, str);
            }
        }

        public static bool ExportRawFile(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".dat", out var exportFullPath, mode: "ExportRaw"))
                return false;
            switch (item.Asset)
            {
                case Texture2D m_Texture2D:
                    if (!string.IsNullOrEmpty(m_Texture2D.m_StreamData?.path))
                    {
                        m_Texture2D.image_data.WriteData(exportFullPath.Replace(".dat", "_data.dat"));
                    }
                    break;
                case AudioClip m_AudioClip:
                    if (!string.IsNullOrEmpty(m_AudioClip.m_Source))
                    {
                        m_AudioClip.m_AudioData.WriteData(exportFullPath.Replace(".dat", "_data.dat"));
                    }
                    break;
                case VideoClip m_VideoClip:
                    if (!string.IsNullOrEmpty(m_VideoClip.m_ExternalResources.m_Source))
                    {
                        m_VideoClip.m_VideoData.WriteData(exportFullPath.Replace(".dat", "_data.dat"));
                    }
                    break;
                case MonoBehaviour m_MonoBehaviour when Properties.Settings.Default.rawByteArrayFromMono:
                    var reader = m_MonoBehaviour.reader;
                    reader.Reset();
                    var assetData = reader.ReadBytes(28); //PPtr<GameObject> m_GameObject, m_Enabled, PPtr<MonoScript>
                    var assetNameLen = reader.ReadInt32();
                    reader.Position -= 4;
                    var assetNameBytes = reader.ReadBytes(assetNameLen + 4);
                    if (assetNameLen > 0)
                        reader.AlignStream();
                    var arrayLen = reader.ReadInt32();
                    if (arrayLen <= 0 || arrayLen > reader.Remaining)
                        break;
                    using (var outStream = new FileStream(exportFullPath.Replace(".dat", "_extracted.dat"), FileMode.Create))
                    {
                        reader.BaseStream.CopyTo(outStream, size: arrayLen);
                    }
                    using (var outStream = new FileStream(exportFullPath, FileMode.Create))
                    {
                        outStream.Write(assetData, 0, assetData.Length);
                        outStream.Write(assetNameBytes, 0, assetNameBytes.Length);
                        if (reader.Remaining > 0)
                            reader.BaseStream.CopyTo(outStream, size: reader.Remaining);
                    }
                    return true;
            }
            File.WriteAllBytes(exportFullPath, item.Asset.GetRawData());
            return true;
        }

        public static bool ExportDumpFile(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".txt", out var exportFullPath, mode: "Dump"))
                return false;
            var str = item.Asset.Dump();
            if (str == null && item.Asset is MonoBehaviour m_MonoBehaviour)
            {
                var m_Type = Studio.MonoBehaviourToTypeTree(m_MonoBehaviour);
                str = m_MonoBehaviour.Dump(m_Type);
            }
            if (string.IsNullOrEmpty(str))
            {
                str = item.Asset.DumpObject();
            }
            if (str != null)
            {
                File.WriteAllText(exportFullPath, str);
                return true;
            }
            return false;
        }

        public static bool ExportJsonDumpFile(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".json", out var exportFullPath, mode: "JsonDump"))
                return false;
            var m_Type = item.Asset is MonoBehaviour m_MonoBehaviour
                ? m_MonoBehaviour.serializedType.m_Type ?? Studio.MonoBehaviourToTypeTree(m_MonoBehaviour)
                : null;
            var jsonDoc = item.Asset.ToJsonDoc(m_Type);
            var str = System.Text.Json.JsonSerializer.Serialize(jsonDoc, Object.JsonOptions);
            File.WriteAllText(exportFullPath, str);
            return true;
        }

        public static bool ExportConvertFile(AssetItem item, string exportPath)
        {
            switch (item.Type)
            {
                case ClassIDType.Texture2D:
                case ClassIDType.Texture2DArrayImage:
                case ClassIDType.Texture2DArray:
                case ClassIDType.AudioClip:
                case ClassIDType.Sprite:
                    throw new System.NotImplementedException();
                case ClassIDType.VideoClip:
                    return ExportVideoClip(item, exportPath);
                case ClassIDType.MovieTexture:
                    return ExportMovieTexture(item, exportPath);
                case ClassIDType.Shader:
                    return ExportShader(item, exportPath);
                case ClassIDType.TextAsset:
                    return ExportTextAsset(item, exportPath);
                case ClassIDType.MonoBehaviour:
                    return ExportMonoBehaviour(item, exportPath);
                case ClassIDType.Font:
                    return ExportFont(item, exportPath);
                case ClassIDType.Mesh:
                    return ExportMesh(item, exportPath);
                case ClassIDType.Animator:
                    return ExportAnimator(item, exportPath);
                case ClassIDType.AnimationClip:
                    return false;
                default:
                    return ExportRawFile(item, exportPath);
            }
        }

        public static void ExportGameObject(GameObject gameObject, string exportPath, List<AssetItem> animationList = null)
        {
            if (!FbxSettings.ExportAnimations)
                animationList = new List<AssetItem>();
            var convert = animationList != null
                ? new ModelConverter(gameObject, Properties.Settings.Default.convertType, animationList.Select(x => (AnimationClip)x.Asset).ToList())
                : new ModelConverter(gameObject, Properties.Settings.Default.convertType);
            exportPath = SafeFilePath(exportPath, $"{gameObject.m_Name}.fbx");
            ExportFbx(convert, exportPath);
        }

        public static void ExportGameObjectMerge(List<GameObject> gameObject, string exportPath, List<AssetItem> animationList = null)
        {
            var rootName = Path.GetFileNameWithoutExtension(exportPath);
            if (!FbxSettings.ExportAnimations)
                animationList = new List<AssetItem>();
            var convert = animationList != null
                ? new ModelConverter(rootName, gameObject, Properties.Settings.Default.convertType, animationList.Select(x => (AnimationClip)x.Asset).ToList())
                : new ModelConverter(rootName, gameObject, Properties.Settings.Default.convertType);
            ExportFbx(convert, exportPath);
        }

        public static string SafeFileName(string str)
        {
            if (str.Length >= 255)
            {
                var newName = Path.GetRandomFileName();
                Logger.Warning("The asset file name is too long.\n" +
                               $"\"{str}\"\n" +
                               "Was replaced with:\n" +
                               $"\"{newName}\"");
                return newName;
            }

            return InvalidChars.Aggregate(str, (current, c) => current.Replace(c, '_')).Trim();
        }

        public static string SafeFilePath(string path, params string[] names)
        {
            var result = path;
            foreach (var name in names)
            {
                result = Path.Combine(result, SafeFileName(name));
            }

            if (result.Length >= 4096)
            {
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), SafeFileName(names.Last()));
                Logger.Warning("The export path is too long.\n" +
                               $"\"{result}\"\n" +
                               "Was reduced to:\n" +
                               $"\"{newPath}\"");
                return newPath;
            }

            return Path.GetInvalidPathChars().Aggregate(result, (current, c) => current.Replace(c, '_'));
        }

        public static string SafePathCombine(string pathA, string pathB)
        {
            return SafeFilePath(pathA, pathB.Split('\\', '/'));
        }

        public static void ClearHash()
        {
            ExportPathHashSet.Clear();
        }
    }
}
