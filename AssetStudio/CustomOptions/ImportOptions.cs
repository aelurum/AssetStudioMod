using AssetStudio.CustomOptions.Asmo;
using SevenZip;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetStudio.CustomOptions
{
    public partial class ImportOptions
    {
        private static JsonSerializerOptions jsonOptions;
        private static string fileName = "ImportOptions";
        private UnityVersion _customUnityVer;

        public event EventHandler UnityVersionChanged;

        public CustomBundleOptions BundleOptions { get; set; }
        public CustomMeshOptions MeshOptions { get; set; }
        public UnityVersion CustomUnityVersion { get => _customUnityVer; set => SetUnityVersion(value); }

        static ImportOptions()
        {
            jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                IncludeFields = true,
            };
        }

        public ImportOptions()
        {
            BundleOptions = new CustomBundleOptions();
            MeshOptions = new CustomMeshOptions();
            CustomUnityVersion = null;
        }

        public static ImportOptions FromOptionsFile(OptionsFile optionsFile)
        {
            if (optionsFile.Reserved != 0)
            {
                Logger.Debug("Skipped. Not an import options file.");
                return null;
            }

            var utf8Bytes = Convert.FromBase64String(Encoding.ASCII.GetString(optionsFile.Data));
            var dataCrc = CRC.CalculateDigest(utf8Bytes, 0, (uint)utf8Bytes.Length);
            if (optionsFile.DataCrc != dataCrc)
                throw new IOException("Options file data is corrupted.");

            return JsonSerializer.Deserialize<ImportOptions>(utf8Bytes, jsonOptions);
        }

        public void SaveToFile(string outputFolder)
        {
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(this, jsonOptions);
            var dataCrc = CRC.CalculateDigest(utf8Bytes, 0, (uint)utf8Bytes.Length);
            var base64String = Convert.ToBase64String(utf8Bytes);

            var optionsFile = new OptionsFile
            {
                DataCrc = dataCrc,
                DataSize = base64String.Length,
                Data = Encoding.ASCII.GetBytes(base64String),
            };

            var unityVer = _customUnityVer != null
                ? $"_{_customUnityVer}"
                : "";
            var path = Path.Combine(outputFolder, $"{fileName}{unityVer}{OptionsFile.Extension}");
            File.WriteAllBytes(path, optionsFile.ToByteArray());

            Logger.Info($"Options file saved to \"{path}\"");
        }

        private void SetUnityVersion(UnityVersion value)
        {
            if (_customUnityVer == value)
                return;
            if (value == null)
            {
                _customUnityVer = null;
                UnityVersionChanged?.Invoke(this, EventArgs.Empty);
                Logger.Info("- Specified Unity version: None");
                return;
            }
            if (string.IsNullOrEmpty(value.BuildType))
            {
                throw new NotSupportedException("Specified Unity version is not in a correct format.\n" +
                                                "Specify full Unity version, including letters at the end.\n" +
                                                "Example: 2022.3.2f1");
            }
            _customUnityVer = value;
            UnityVersionChanged?.Invoke(this, EventArgs.Empty);

            Logger.Info($"- Specified Unity version: {_customUnityVer}");
        }

        private static T SetOption<T>(string option, T value, T curValue)
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
