namespace AssetStudio
{
    public abstract class EditorExtension : Object
    {
        protected EditorExtension() { }

        protected EditorExtension(ObjectReader reader) : base(reader)
        {
            if (platform == BuildTarget.NoTarget)
            {
                var m_PrefabParentObject = new PPtr<EditorExtension>(reader);
                if (version >= (3, 5))
                {
                    var m_PrefabInternal = new PPtr<Object>(reader); //PPtr<Prefab>
                }
                if (version >= (2018, 3))
                {
                    var m_PrefabAsset = new PPtr<Object>(reader);
                }
            }
        }
    }
}
