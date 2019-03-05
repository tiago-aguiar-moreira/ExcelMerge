using ExcelMerge.Utils;
using System.IO;
using System.Windows.Forms;

namespace ExcelMerge
{
    public class AppConfiguration
    {
        private const string ConfigExtension = ".config.json";

        private string _pathAppConfig;

        private string GetDirectoryNameFromExecutable() => Path.GetDirectoryName(Application.ExecutablePath);

        private string GetNameConfig() => Path.GetFileNameWithoutExtension(Application.ExecutablePath);

        private string GetPathAppConfig() => $"{GetDirectoryNameFromExecutable()}\\{GetNameConfig()}{ConfigExtension}";

        public AppConfiguration() => _pathAppConfig = GetPathAppConfig();

        public AppConfigModel Load()
        {
            CreateDefaultConfig();

            return JSONUtils.LoadFromFile<AppConfigModel>(_pathAppConfig);
        }

        private void CreateDefaultConfig()
        {
            if (!File.Exists(_pathAppConfig))
                JSONUtils.SaveToFile(new AppConfigModel(), _pathAppConfig);
        }

        public void Save(AppConfigModel appConfig) => JSONUtils.SaveToFile(appConfig, _pathAppConfig);
    }
}