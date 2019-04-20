using ExcelMerge.Enumerator;

namespace ExcelMerge.Model
{
    public class AppConfigModel
    {
        public SelectedEndProcessActionEnum SelectedEndProcessAction { get; set; }
        public SelectedHeaderActionEnum SelectedHeaderAction { get; set; }
        public string DefaultDirectorySaveFiles { get; set; }
        public string RecentDirectorySaveFiles { get; set; }
        public byte HeaderLength { get; set; }
        public bool ShowConfigs { get; set; }

        public AppConfigModel()
        {
            SelectedEndProcessAction = SelectedEndProcessActionEnum.None;
            SelectedHeaderAction = SelectedHeaderActionEnum.None;
            DefaultDirectorySaveFiles = string.Empty;
            RecentDirectorySaveFiles = string.Empty;
            HeaderLength = 1;
        }
    }
}
