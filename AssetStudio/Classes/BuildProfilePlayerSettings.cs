namespace AssetStudio
{
    public sealed class BuildProfilePlayerSettings : NamedObject
    {
        public string companyName;
        public string productName;

        public BuildProfilePlayerSettings(ObjectReader reader) : base(reader)
        {
            var productGUID = reader.Position += 16;
            var AndroidProfiler = reader.ReadBoolean();
            var AndroidFilterTouchesWhenObscured = reader.ReadBoolean();
            var AndroidEnableSustainedPerformanceMode = reader.ReadBoolean();
            reader.AlignStream();
            var defaultScreenOrientation = reader.ReadInt32();
            var targetDevice = reader.ReadInt32();
            var useOnDemandResources = reader.ReadBoolean();
            reader.AlignStream();
            var accelerometerFrequency = reader.ReadInt32();
            companyName = reader.ReadAlignedString();
            productName = reader.ReadAlignedString();
        }
    }
}
