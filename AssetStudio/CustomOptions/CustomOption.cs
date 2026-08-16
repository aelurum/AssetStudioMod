namespace AssetStudio.CustomOptions
{
    public abstract class CustomOption
    {
        protected static T SetOption<T>(string option, T value, T curValue)
        {
            if (value.Equals(curValue))
                return curValue;

            var val = string.IsNullOrEmpty(value.ToString())
                ? "None"
                : value.ToString();
            Logger.Info($"- {option}: {val}");
            return value;
        }
    }
}
