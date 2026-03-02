using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AssetStudio
{
    public class WebFile : FileArchive
    {
        private class WebData
        {
            public int dataOffset;
            public int dataLength;
            public string path;
        }

        public WebFile(EndianBinaryReader reader)
        {
            reader.Endian = EndianType.LittleEndian;
            var signature = reader.ReadStringToNull();
            var headLength = reader.ReadInt32();
            var dataList = new List<WebData>();
            while (reader.BaseStream.Position < headLength)
            {
                var data = new WebData();
                data.dataOffset = reader.ReadInt32();
                data.dataLength = reader.ReadInt32();
                var pathLength = reader.ReadInt32();
                data.path = Encoding.UTF8.GetString(reader.ReadBytes(pathLength));
                dataList.Add(data);
            }
            foreach (var data in dataList)
            {
                var file = new StreamFile();
                this.FileList.Add(file);
                file.path = data.path;
                file.fileName = Path.GetFileName(data.path);
                reader.BaseStream.Position = data.dataOffset;
                file.stream = new MemoryStream(reader.ReadBytes(data.dataLength));
            }
        }
    }
}
