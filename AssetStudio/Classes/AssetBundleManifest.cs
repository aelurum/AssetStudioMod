using System.Collections.Generic;

namespace AssetStudio
{
    public class AssetBundleInfo
    {
        public string AssetBundleHash;
        public int[] AssetBundleDependencies;

        public AssetBundleInfo(ObjectReader reader)
        {
            AssetBundleHash = reader.ReadBytesAsHexString(16);
            AssetBundleDependencies = reader.ReadInt32Array();
        }
    }

    public sealed class AssetBundleManifest : NamedObject
    {
        public List<KeyValuePair<int, string>> AssetBundleNames;
        public int[] AssetBundlesWithVariant;
        public List<KeyValuePair<int, AssetBundleInfo>> AssetBundleInfos;

        public AssetBundleManifest(ObjectReader reader) : base(reader)
        {
            var AssetBundleNamesCount = reader.ReadInt32();
            AssetBundleNames = new List<KeyValuePair<int, string>>();
            for (var i = 0; i < AssetBundleNamesCount; i++)
            {
                AssetBundleNames.Add(new KeyValuePair<int, string>(reader.ReadInt32(), reader.ReadAlignedString()));
            }

            AssetBundlesWithVariant = reader.ReadInt32Array();

            var AssetBundleInfosCount = reader.ReadInt32();
            AssetBundleInfos = new List<KeyValuePair<int, AssetBundleInfo>>();
            for (var i = 0; i < AssetBundleInfosCount; i++)
            {
                AssetBundleInfos.Add(new KeyValuePair<int, AssetBundleInfo>(reader.ReadInt32(), new AssetBundleInfo(reader)));
            }
        }
    }
}
