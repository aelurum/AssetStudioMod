using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetStudio
{
    public class SerializedFile
    {
        public AssetsManager assetsManager;
        public FileReader reader;
        public string fullName;
        public string originalPath;
        public string fileName;
        public UnityVersion version = new UnityVersion();
        public readonly List<Object> Objects;
        public readonly Dictionary<long, Object> ObjectsDic;
        public readonly Dictionary<string, SerializedType> ExtractedTypeTreeHashes = new Dictionary<string, SerializedType>();

        public SerializedFileHeader header;
        private byte m_FileEndianess;
        public BuildTarget m_TargetPlatform = BuildTarget.UnknownPlatform;
        public string targetPlatformString;
        private bool m_EnableTypeTree = true;
        public List<SerializedType> m_Types;
        public int bigIDEnabled = 0;
        public List<ObjectInfo> m_Objects;
        private List<LocalSerializedObjectIdentifier> m_ScriptTypes;
        public List<FileIdentifier> m_Externals;
        public List<SerializedType> m_RefTypes;
        public string userInformation;

        public SerializedFile(FileReader reader, AssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
            this.reader = reader;
            fullName = reader.FullPath;
            fileName = reader.FileName;

            // ReadHeader
            header = new SerializedFileHeader();
            header.m_MetadataSize = reader.ReadUInt32();
            header.m_FileSize = reader.ReadUInt32();
            header.m_Version = (SerializedFileFormatVersion)reader.ReadUInt32();
            header.m_DataOffset = reader.ReadUInt32();

            if (header.m_Version >= SerializedFileFormatVersion.Unknown_9)
            {
                header.m_Endianess = reader.ReadByte();
                header.m_Reserved = reader.ReadBytes(3);
                m_FileEndianess = header.m_Endianess;
            }
            else
            {
                reader.Position = header.m_FileSize - header.m_MetadataSize;
                m_FileEndianess = reader.ReadByte();
            }

            if (header.m_Version >= SerializedFileFormatVersion.LargeFilesSupport)
            {
                header.m_MetadataSize = reader.ReadUInt32();
                header.m_FileSize = reader.ReadInt64();
                header.m_DataOffset = reader.ReadInt64();
                reader.ReadInt64(); // unknown
            }

            // ReadMetadata
            if (m_FileEndianess == 0)
            {
                reader.Endian = EndianType.LittleEndian;
            }
            switch (header.m_Version) //default unity versions
            {
                case SerializedFileFormatVersion.Unsupported:
                    throw new NotSupportedException();
                case SerializedFileFormatVersion.Unknown_2:
                case SerializedFileFormatVersion.Unknown_3:
                    version = new UnityVersion(1, 0); //probably not supported as well
                    break;
                case SerializedFileFormatVersion.Unknown_5: //1.2 - 2.1 down
                    version = new UnityVersion(1, 6);
                    break;
                case SerializedFileFormatVersion.Unknown_6: //2.1 - 3.0 down
                    version = new UnityVersion(2, 5);
                    break;
            }
            if (header.m_Version >= SerializedFileFormatVersion.Unknown_7)
            {
                var versionPos = reader.Position;

                var verStr = reader.ReadStringToNull();
                if (!UnityVersion.TryParse(verStr, out version))
                {
                    if (assetsManager.Options.CustomUnityVersion == null)
                    {
                        Logger.Warning($"Failed to parse Unity version: \"{verStr}\"");
                        version = new UnityVersion();
                        return;
                    }
                    version = assetsManager.Options.CustomUnityVersion;
                    reader.Position = versionPos;
                    reader.ReadBytes(version.ToString().Length + 1);
                }
            }
            if (header.m_Version >= SerializedFileFormatVersion.Unknown_8)
            {
                var target = reader.ReadInt32();
                
                m_TargetPlatform = (BuildTarget)target;
                if (!Enum.IsDefined(typeof(BuildTarget), m_TargetPlatform))
                {
                    m_TargetPlatform = BuildTarget.UnknownPlatform;
                }

                targetPlatformString = version.IsTuanjie && Enum.IsDefined(typeof(TuanjieBuildTarget), target)
                    ? ((TuanjieBuildTarget)target).ToString()
                    : m_TargetPlatform.ToString();
            }
            if (header.m_Version >= SerializedFileFormatVersion.HasTypeTreeHashes)
            {
                m_EnableTypeTree = reader.ReadBoolean();
            }

            // Read Types
            int typeCount = reader.ReadInt32();
            m_Types = new List<SerializedType>();
            for (var i = 0; i < typeCount; i++)
            {
                m_Types.Add(ReadSerializedType(false));
            }

            if (header.m_Version >= SerializedFileFormatVersion.Unknown_7 && header.m_Version < SerializedFileFormatVersion.Unknown_14)
            {
                bigIDEnabled = reader.ReadInt32();
            }

            // Read Objects
            int objectCount = reader.ReadInt32();
            m_Objects = new List<ObjectInfo>();
            Objects = new List<Object>();
            ObjectsDic = new Dictionary<long, Object>();
            for (var i = 0; i < objectCount; i++)
            {
                var objectInfo = new ObjectInfo();
                if (bigIDEnabled != 0)
                {
                    objectInfo.m_PathID = reader.ReadInt64();
                }
                else if (header.m_Version < SerializedFileFormatVersion.Unknown_14)
                {
                    objectInfo.m_PathID = reader.ReadInt32();
                }
                else
                {
                    reader.AlignStream();
                    objectInfo.m_PathID = reader.ReadInt64();
                }

                if (header.m_Version >= SerializedFileFormatVersion.LargeFilesSupport)
                    objectInfo.byteStart = reader.ReadInt64();
                else
                    objectInfo.byteStart = reader.ReadUInt32();

                objectInfo.byteStart += header.m_DataOffset;
                objectInfo.byteSize = reader.ReadUInt32();
                objectInfo.typeID = reader.ReadInt32();
                if (header.m_Version < SerializedFileFormatVersion.RefactoredClassId)
                {
                    objectInfo.classID = reader.ReadUInt16();
                    objectInfo.serializedType = m_Types.Find(x => x.classID == objectInfo.typeID);
                }
                else
                {
                    var type = m_Types[objectInfo.typeID];
                    objectInfo.serializedType = type;
                    objectInfo.classID = type.classID;
                }
                if (header.m_Version < SerializedFileFormatVersion.HasScriptTypeIndex)
                {
                    objectInfo.isDestroyed = reader.ReadUInt16();
                }
                if (header.m_Version >= SerializedFileFormatVersion.HasScriptTypeIndex && header.m_Version < SerializedFileFormatVersion.RefactorTypeData)
                {
                    var m_ScriptTypeIndex = reader.ReadInt16();
                    if (objectInfo.serializedType != null)
                        objectInfo.serializedType.m_ScriptTypeIndex = m_ScriptTypeIndex;
                }
                if (header.m_Version == SerializedFileFormatVersion.SupportsStrippedObject || header.m_Version == SerializedFileFormatVersion.RefactoredClassId)
                {
                    objectInfo.stripped = reader.ReadByte();
                }
                m_Objects.Add(objectInfo);
            }

            if (header.m_Version >= SerializedFileFormatVersion.HasScriptTypeIndex)
            {
                int scriptCount = reader.ReadInt32();
                m_ScriptTypes = new List<LocalSerializedObjectIdentifier>();
                for (var i = 0; i < scriptCount; i++)
                {
                    var m_ScriptType = new LocalSerializedObjectIdentifier();
                    m_ScriptType.localSerializedFileIndex = reader.ReadInt32();
                    if (header.m_Version < SerializedFileFormatVersion.Unknown_14)
                    {
                        m_ScriptType.localIdentifierInFile = reader.ReadInt32();
                    }
                    else
                    {
                        reader.AlignStream();
                        m_ScriptType.localIdentifierInFile = reader.ReadInt64();
                    }
                    m_ScriptTypes.Add(m_ScriptType);
                }
            }

            int externalsCount = reader.ReadInt32();
            m_Externals = new List<FileIdentifier>();
            for (var i = 0; i < externalsCount; i++)
            {
                var m_External = new FileIdentifier();
                if (header.m_Version >= SerializedFileFormatVersion.Unknown_6)
                {
                    var tempEmpty = reader.ReadStringToNull();
                }
                if (header.m_Version >= SerializedFileFormatVersion.Unknown_5)
                {
                    m_External.guid = new Guid(reader.ReadBytes(16));
                    m_External.type = reader.ReadInt32();
                }
                m_External.pathName = reader.ReadStringToNull();
                m_External.fileName = Path.GetFileName(m_External.pathName);
                m_Externals.Add(m_External);
            }

            if (header.m_Version >= SerializedFileFormatVersion.SupportsRefObject)
            {
                int refTypesCount = reader.ReadInt32();
                m_RefTypes = new List<SerializedType>();
                for (var i = 0; i < refTypesCount; i++)
                {
                    m_RefTypes.Add(ReadSerializedType(true));
                }
            }

            if (header.m_Version >= SerializedFileFormatVersion.Unknown_5)
            {
                userInformation = reader.ReadStringToNull();
            }

            //try to set unity version from BuildSettings (unity 2.0 - 3.0 down)
            if (header.m_Version <= SerializedFileFormatVersion.Unknown_6)
            {
                var buildSettingsAsset = m_Objects.FirstOrDefault(x => (ClassIDType)x.classID == ClassIDType.BuildSettings);
                if (buildSettingsAsset == null)
                    return;

                var objReader = new ObjectReader(reader, this, buildSettingsAsset);
                if (buildSettingsAsset.serializedType?.m_Type != null)
                {
                    var settings = new BuildSettings(objReader, TypeTreeHelper.ReadTypeByteArray(objReader.serializedType.m_Type, objReader), Object.JsonOptions);
                    if (UnityVersion.TryParse(settings.m_Version, out var unityVer))
                    {
                        assetsManager.AutoLegacyVersion = unityVer.Major == 1 ? new UnityVersion(2, 1) : unityVer; //detected 1.x versions should mean unity-iphone
                        Logger.Debug($"Detected legacy version: {unityVer}");
                    }
                }
                else
                {
                    objReader.Position = objReader.byteStart + objReader.byteSize - 8;
                    var buff = objReader.ReadBytes(8).AsSpan();
                    if (UnityVersion.TryParse(buff.ReadStringToNull(), out var unityVer))
                    {
                        assetsManager.AutoLegacyVersion = unityVer.Major == 1 ? new UnityVersion(2, 1) : unityVer;
                        Logger.Debug($"Detected legacy version: {unityVer}");
                    }
                }
            }

            //reader.AlignStream(16);
        }

        private SerializedType ReadSerializedType(bool isRefType)
        {
            var type = new SerializedType();

            type.classID = reader.ReadInt32();

            if (header.m_Version >= SerializedFileFormatVersion.RefactoredClassId)
            {
                type.m_IsStrippedType = reader.ReadBoolean();
            }

            if (header.m_Version >= SerializedFileFormatVersion.RefactorTypeData)
            {
                type.m_ScriptTypeIndex = reader.ReadInt16();
            }

            if (header.m_Version >= SerializedFileFormatVersion.HasTypeTreeHashes)
            {
                if (isRefType && type.m_ScriptTypeIndex >= 0)
                {
                    type.m_ScriptID = reader.ReadBytes(16);
                }
                else if ((header.m_Version < SerializedFileFormatVersion.RefactoredClassId && type.classID < 0) || (header.m_Version >= SerializedFileFormatVersion.RefactoredClassId && type.classID == 114))
                {
                    type.m_ScriptID = reader.ReadBytes(16);
                }
                type.m_OldTypeHash = reader.ReadBytes(16);
            }

            if (m_EnableTypeTree)
            {
                type.m_Type = new TypeTree();
                type.m_Type.m_Nodes = new List<TypeTreeNode>();
                if (header.m_Version >= SerializedFileFormatVersion.ExtractedTypeTreeSupport)
                {
                    var typeTreeHash = reader.ReadBytesAsHexString(16); //xxh3 
                    var typeTreeSize = reader.ReadInt32();
                    if (typeTreeSize != 0)
                    {
                        TypeTreeBlobRead(type.m_Type);
                    }
                    else
                    {
                        type.m_Type = null;
                        ExtractedTypeTreeHashes.Add(typeTreeHash, type);
                    }
                }
                else if (header.m_Version >= SerializedFileFormatVersion.Unknown_12 || header.m_Version == SerializedFileFormatVersion.Unknown_10)
                {
                    TypeTreeBlobRead(type.m_Type);
                }
                else
                {
                    ReadTypeTree(type.m_Type);
                }
                if (header.m_Version >= SerializedFileFormatVersion.StoresTypeDependencies)
                {
                    if (isRefType)
                    {
                        type.m_KlassName = reader.ReadStringToNull();
                        type.m_NameSpace = reader.ReadStringToNull();
                        type.m_AsmName = reader.ReadStringToNull();
                    }
                    else
                    {
                        type.m_TypeDependencies = reader.ReadInt32Array();
                    }
                }
            }

            return type;
        }

        private void ReadTypeTree(TypeTree m_Type, int level = 0)
        {
            var typeTreeNode = new TypeTreeNode();
            m_Type.m_Nodes.Add(typeTreeNode);
            typeTreeNode.m_Level = level;
            typeTreeNode.m_Type = reader.ReadStringToNull();
            if (header.m_Version <= SerializedFileFormatVersion.Unknown_6 && typeTreeNode.m_Type == "PPtr<EditorExtensionImpl>")
            {
                m_TargetPlatform = BuildTarget.NoTarget;
                targetPlatformString = m_TargetPlatform.ToString();
            }
            if (header.m_Version == SerializedFileFormatVersion.Unknown_5 && (typeTreeNode.m_Type == "TransformComponent" || typeTreeNode.m_Type == "FileTexture"))
            {
                assetsManager.AutoLegacyVersion = assetsManager.AutoLegacyVersion ?? new UnityVersion(1, 2);
            }
            typeTreeNode.m_Name = reader.ReadStringToNull();
            typeTreeNode.m_ByteSize = reader.ReadInt32();
            if (header.m_Version == SerializedFileFormatVersion.Unknown_2)
            {
                var variableCount = reader.ReadInt32();
            }
            if (header.m_Version != SerializedFileFormatVersion.Unknown_3)
            {
                typeTreeNode.m_Index = reader.ReadInt32();
            }
            typeTreeNode.m_TypeFlags = reader.ReadInt32();
            typeTreeNode.m_Version = reader.ReadInt32();
            if (header.m_Version != SerializedFileFormatVersion.Unknown_3)
            {
                typeTreeNode.m_MetaFlag = reader.ReadInt32();
            }

            int childrenCount = reader.ReadInt32();
            for (var i = 0; i < childrenCount; i++)
            {
                ReadTypeTree(m_Type, level + 1);
            }
        }

        private void TypeTreeBlobRead(TypeTree m_Type, FileReader fileReader = null)
        {
            var typeTreeReader = fileReader ?? reader;
            if (header.m_Version >= SerializedFileFormatVersion.ExtractedTypeTreeSupport)
            {
                ReadTypeTreeHeader(typeTreeReader);
            }
            int numberOfNodes = typeTreeReader.ReadInt32();
            int stringBufferSize = typeTreeReader.ReadInt32();
            for (var i = 0; i < numberOfNodes; i++)
            {
                var typeTreeNode = new TypeTreeNode();
                m_Type.m_Nodes.Add(typeTreeNode);
                typeTreeNode.m_Version = typeTreeReader.ReadUInt16();
                typeTreeNode.m_Level = typeTreeReader.ReadByte();
                typeTreeNode.m_TypeFlags = typeTreeReader.ReadByte();
                typeTreeNode.m_TypeStrOffset = typeTreeReader.ReadUInt32();
                typeTreeNode.m_NameStrOffset = typeTreeReader.ReadUInt32();
                typeTreeNode.m_ByteSize = typeTreeReader.ReadInt32();
                typeTreeNode.m_Index = typeTreeReader.ReadInt32();
                typeTreeNode.m_MetaFlag = typeTreeReader.ReadInt32();
                if (header.m_Version >= SerializedFileFormatVersion.TypeTreeNodeWithTypeFlags)
                {
                    typeTreeNode.m_RefTypeHash = typeTreeReader.ReadUInt64();
                }
            }
            m_Type.m_StringBuffer = typeTreeReader.ReadBytes(stringBufferSize);

            var stringBufferSpan = new ReadOnlySpan<byte>(m_Type.m_StringBuffer);
            foreach (var m_Node in m_Type.m_Nodes)
            {
                m_Node.m_Type = ReadString(stringBufferSpan, m_Node.m_TypeStrOffset);
                m_Node.m_Name = ReadString(stringBufferSpan, m_Node.m_NameStrOffset);
            }

            string ReadString(ReadOnlySpan<byte> stringBuff, uint value)
            {
                var isOffset = (value & 0x80000000) == 0;
                if (isOffset)
                {
                    return stringBuff.Slice((int)value).ReadStringToNull();
                }
                var offset = value & 0x7FFFFFFF;
                if (CommonString.StringBuffer.TryGetValue(offset, out var str))
                {
                    return str;
                }
                return offset.ToString();
            }
        }

        private void ReadTypeTreeHeader(FileReader typeTreeReader)
        {
            var typeTreeSign = typeTreeReader.ReadInt32(); //0x7474686d ("tthm")
            var serializedFileVersion = (SerializedFileFormatVersion)typeTreeReader.ReadInt32();

            if (typeTreeSign != 0x7474686d)
            {
                Logger.Warning("Unknown TypeTree file format");
            }

            if (serializedFileVersion != header.m_Version)
            {
                Logger.Warning("Serialized file versions do not match");
            }
        }

        public void ReadExtractedTypeTree()
        {
            foreach (var kv in ExtractedTypeTreeHashes)
            {
                if (assetsManager.resourceFileReaders.TryGetValue(kv.Key, out var binaryReader))
                {
                    var fileReader = (FileReader)binaryReader;
                    fileReader.Endian = EndianType.LittleEndian;
                    fileReader.Position = 0;

                    kv.Value.m_Type = new TypeTree();
                    kv.Value.m_Type.m_Nodes = new List<TypeTreeNode>();
                    TypeTreeBlobRead(kv.Value.m_Type, fileReader);

                    ExtractedTypeTreeHashes[kv.Key] = null;
                }
            }
            var notFound = ExtractedTypeTreeHashes.Where(x=> x.Value != null).ToArray();
            if (notFound.Length > 0 && notFound.Length != ExtractedTypeTreeHashes.Count)
            {
                foreach (var kv in notFound)
                {
                    Logger.Warning($"Failed to find extracted typetree with hash \"{kv.Key}\" for \"{(ClassIDType)kv.Value.classID}\" asset type");
                }
                Logger.Warning("Not all typetree hashes were found in the loaded typetree file\n" +
                               "The typetree file is probably outdated or not designed for the loaded asset bundles");
            }
            ExtractedTypeTreeHashes.Clear();
        }

        public void AddObject(Object obj)
        {
            Objects.Add(obj);
            ObjectsDic.Add(obj.m_PathID, obj);
        }
    }
}
