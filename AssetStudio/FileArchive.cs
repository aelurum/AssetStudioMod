using System.Collections.Generic;

namespace AssetStudio
{
    public abstract class FileArchive
    {
        public readonly List<StreamFile> FileList = new List<StreamFile>();
    }
}
