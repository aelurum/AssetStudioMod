using AssetStudio.FbxInterop;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

#if NETFRAMEWORK
using AssetStudio.PInvoke;
#endif

namespace AssetStudio
{
    public static partial class Fbx
    {
#if NETFRAMEWORK
        static Fbx()
        {
            DllLoader.PreloadDll(FbxDll.DllName);
        }
#endif

        public static Vector3 QuaternionToEuler(Quaternion q)
        {
            AsUtilQuaternionToEuler(q.X, q.Y, q.Z, q.W, out var x, out var y, out var z);
            return new Vector3(x, y, z);
        }

        public static Quaternion EulerToQuaternion(Vector3 v)
        {
            AsUtilEulerToQuaternion(v.X, v.Y, v.Z, out var x, out var y, out var z, out var w);
            return new Quaternion(x, y, z, w);
        }

        public static class Exporter
        {
            public static void Export(string path, IImported imported, Settings fbxSettings)
            {
                var file = new FileInfo(path);
                var dir = file.Directory;

                if (!dir.Exists)
                {
                    dir.Create();
                }

                var currentDir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(dir.FullName);

                var name = Path.GetFileName(path);

                using (var exporter = new FbxExporter(name, imported, fbxSettings))
                {
                    exporter.ExportAll();
                }

                Directory.SetCurrentDirectory(currentDir);
            }
        }

        public sealed class Settings
        {
            public bool EulerFilter { get; set; }
            public float FilterPrecision { get; set; }
            public bool ExportAllNodes { get; set; }
            public bool ExportSkins { get; set; }
            public bool ExportAnimations { get; set; }
            public bool ExportBlendShape { get; set; }
            public bool DumpMaterials { get; set; }
            public bool CastToBone { get; set; }
            public float BoneSize { get; set; }
            public bool ExportAllUvsAsDiffuseMaps { get; set; }
            public float ScaleFactor { get; set; }
            public int FbxVersionIndex { get; set; }
            public int FbxFormat { get; set; }
            public Dictionary<int,int> UvBindings { get; set; }
            public bool IsAscii => FbxFormat == 1;

            public Settings()
            {
                Init();
            }

            public Settings(bool eulerFilter, float filterPrecision, bool exportAllNodes, bool exportSkins, bool exportAnimations, bool exportBlendShape, bool dumpMaterials,
                bool castToBone, float boneSize, bool exportAllUvsAsDiffuseMaps, float scaleFactor, int fbxVersionIndex, int fbxFormat, Dictionary<int, int> uvBindings)
            {
                EulerFilter = eulerFilter;
                FilterPrecision = filterPrecision;
                ExportAllNodes = exportAllNodes;
                ExportSkins = exportSkins;
                ExportAnimations = exportAnimations;
                ExportBlendShape = exportBlendShape;
                DumpMaterials = dumpMaterials;
                CastToBone = castToBone;
                BoneSize = (int)boneSize;
                ExportAllUvsAsDiffuseMaps = exportAllUvsAsDiffuseMaps;
                ScaleFactor = scaleFactor;
                FbxVersionIndex = fbxVersionIndex;
                FbxFormat = fbxFormat;
                UvBindings = uvBindings;
            }

            public void Init()
            {
                var uvDict = new Dictionary<int, int>();
                for (var i = 0; i < 8; i++)
                {
                    uvDict[i] = i + 1;
                }

                EulerFilter = true;
                FilterPrecision = 0.25f;
                ExportAllNodes = true;
                ExportSkins = true;
                ExportAnimations = true;
                ExportBlendShape = true;
                DumpMaterials = false;
                CastToBone = false;
                ExportAllUvsAsDiffuseMaps = false;
                BoneSize = 10;
                ScaleFactor = 1.0f;
                FbxFormat = 0;
                FbxVersionIndex = 3;
                UvBindings = uvDict;
            }

            public static Settings FromBase64(string base64String)
            {
                var settingsData = System.Convert.FromBase64String(base64String);
                return JsonSerializer.Deserialize<Settings>(settingsData);
            }

            public string ToBase64()
            {
                return System.Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(this));
            }
        }
    }
}
