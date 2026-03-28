namespace AssetStudio
{
    public class NamedObject : EditorExtension
    {
        public string m_Name;

        protected NamedObject() { }

        protected NamedObject(ObjectReader reader) : base(reader)
        {
            if (version >= (1, 5))
            {
                m_Name = reader.ReadAlignedString();
            }
        }
    }
}
