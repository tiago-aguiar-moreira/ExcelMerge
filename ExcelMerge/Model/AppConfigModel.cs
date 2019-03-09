using ExcelMerge.Enumerator;

namespace ExcelMerge.Model
{
    public class AppConfigModel
    {
        public SelectedEndProcessActionEnum SelectedEndProcessAction { get; set; }
        public string DefaultDirectorySaveFiles { get; set; }
        public string RecentDirectorySaveFiles { get; set; }

        public AppConfigModel()
        {
            SelectedEndProcessAction = SelectedEndProcessActionEnum.None;
            DefaultDirectorySaveFiles = string.Empty;
            RecentDirectorySaveFiles = string.Empty;
        }
    }
}
