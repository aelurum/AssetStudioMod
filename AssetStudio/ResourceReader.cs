using System.IO;
using System.Text.Json.Serialization;

namespace AssetStudio
{
    public class ResourceReader
    {
        private bool needSearch;
        private string path;
        private SerializedFile assetsFile;
        private long size;
        private BinaryReader reader;

        public int Size
        {
            get => (int)size;
            set => size = value;
        }
        public long Offset { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EndianType Endian { get; }

        public ResourceReader() { }

        public ResourceReader(string path, SerializedFile assetsFile, long offset, long size)
        {
            needSearch = true;
            this.path = path;
            this.assetsFile = assetsFile;
            this.Offset = offset;
            this.size = size;
            this.Endian = assetsFile.reader.Endian;
        }

        public ResourceReader(EndianBinaryReader reader, long offset, long size)
        {
            this.reader = reader;
            this.Offset = offset;
            this.size = size;
            this.Endian = reader.Endian;
        }

        private BinaryReader GetReader()
        {
            if (needSearch)
            {
                var resourceFileName = Path.GetFileName(path);
                if (assetsFile.assetsManager.resourceFileReaders.TryGetValue(resourceFileName, out reader))
                {
                    needSearch = false;
                    return reader;
                }
                var assetsFileDirectory = Path.GetDirectoryName(assetsFile.fullName);
                var resourceFilePath = Path.Combine(assetsFileDirectory, resourceFileName);
                if (!File.Exists(resourceFilePath))
                {
                    var findFiles = Directory.GetFiles(assetsFileDirectory, resourceFileName, SearchOption.AllDirectories);
                    if (findFiles.Length > 0)
                    {
                        resourceFilePath = findFiles[0];
                    }
                }
                if (File.Exists(resourceFilePath))
                {
                    needSearch = false;
                    if (assetsFile.assetsManager.resourceFileReaders.TryGetValue(resourceFileName, out reader))
                    {
                        return reader;
                    }
                    reader = new BinaryReader(File.OpenRead(resourceFilePath));
                    assetsFile.assetsManager.resourceFileReaders.TryAdd(resourceFileName, reader);
                    return reader;
                }
                throw new FileNotFoundException($"Can't find the resource file {resourceFileName}");
            }
            else
            {
                return reader;
            }
        }

        public byte[] GetData()
        {
            var binaryReader = GetReader();
            lock (binaryReader.BaseStream)
            {
                binaryReader.BaseStream.Position = Offset;
                return binaryReader.ReadBytes((int)size);
            }
        }

        public int GetData(byte[] buff, int startIndex = 0)
        {
            int dataLen;
            var binaryReader = GetReader();
            lock (binaryReader.BaseStream)
            {
                binaryReader.BaseStream.Position = Offset;
                dataLen = binaryReader.Read(buff, startIndex, (int)size);
            }
            return dataLen;
        }

        public void WriteData(string path)
        {
            var binaryReader = GetReader();
            binaryReader.BaseStream.Position = Offset;
            using (var writer = File.OpenWrite(path))
            {
                binaryReader.BaseStream.CopyTo(writer, size);
            }
        }
    }
}
