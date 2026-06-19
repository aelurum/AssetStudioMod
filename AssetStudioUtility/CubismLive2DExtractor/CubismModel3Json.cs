using System.Text.Json.Nodes;

namespace CubismLive2DExtractor
{
    public class CubismModel3Json
    {
        public int Version;
        public string Name;
        public SerializableFileReferences FileReferences;
        public SerializableGroup[] Groups;

        public class SerializableFileReferences
        {
            public string Moc;
            public string[] Textures;
            public string Physics;
            public string Pose;
            public string DisplayInfo;
            public JsonObject Motions;
            public JsonArray Expressions;
        }

        public class SerializableGroup
        {
            public string Target;
            public string Name;
            public string[] Ids;
        }
    }
}
