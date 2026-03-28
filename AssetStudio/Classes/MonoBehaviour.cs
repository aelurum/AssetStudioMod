namespace AssetStudio
{
    public class MonoBehaviour : Behaviour
    {
        public PPtr<MonoScript> m_Script;
        public string m_Name;

        public MonoBehaviour() { }

        public MonoBehaviour(ObjectReader reader) : base(reader)
        {
            m_Script = new PPtr<MonoScript>(reader);
            if (version >= (2, 1))
                m_Name = reader.ReadAlignedString();
        }
    }
}
