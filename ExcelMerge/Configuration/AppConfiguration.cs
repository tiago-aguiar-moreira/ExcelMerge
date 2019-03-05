using ExcelMerge.Utils;
using System.IO;
using System.Windows.Forms;

namespace ExcelMerge
{
    public static class AppConfiguration
    {
        private const string ConfigExtension = ".json";

        private static string GetDirectoryNameFromExecutable() => Path.GetDirectoryName(Application.ExecutablePath);

        public static AppConfigModel Load(string fileNameWithoutExtension)
        {
            string configPath = $"{GetDirectoryNameFromExecutable()}\\{fileNameWithoutExtension}{ConfigExtension}";

            if (!File.Exists(configPath))
                JSONUtils.SaveToFile(new AppConfigModel(), configPath);

            return JSONUtils.LoadFromFile<AppConfigModel>(configPath);
        }
    }
}
