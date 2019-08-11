using ExcelMerge.Model;
using ExcelMerge.Utils;
using System.IO;
using System.Windows.Forms;

namespace ExcelMerge.Configuration
{
    public static class AppConfigManager
    {
        private const string ConfigExtension = ".exe.config.json";

        private static string GetDirectoryNameFromExecutable() 
            => Path.GetDirectoryName(Application.ExecutablePath);

        private static string GetNameConfig() 
            => Path.GetFileNameWithoutExtension(Application.ExecutablePath);

        private static string GetPathAppConfig() 
            => $"{GetDirectoryNameFromExecutable()}\\{GetNameConfig()}{ConfigExtension}";

        private static void CreateDefaultConfig()
        {
            string pathAppConfig = GetPathAppConfig();

            if (!File.Exists(pathAppConfig))
                JSONUtils.SaveToFile(new AppConfigModel(), pathAppConfig);
        }

        public static AppConfigModel Load()
        {
            CreateDefaultConfig();

            return JSONUtils.LoadFromFile<AppConfigModel>(GetPathAppConfig());
        }

        public static void Save(AppConfigModel appConfig)
            => JSONUtils.SaveToFile(appConfig, GetPathAppConfig());
    }
}