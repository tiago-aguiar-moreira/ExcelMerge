using ExcelMerge.Enumerator;

namespace ExcelMerge.Model
{
    public class AppConfigModel
    {
        public EndProcessActionEnum EndProcessAction { get; set; }
        public HeaderActionEnum HeaderAction { get; set; }
        public string DefaultDirectorySaveFiles { get; set; }
        public string RecentDirectorySaveFiles { get; set; }
        public byte HeaderLength { get; set; }
        public bool ShowConfigs { get; set; }

        public AppConfigModel()
        {
            EndProcessAction = EndProcessActionEnum.None;
            HeaderAction = HeaderActionEnum.None;
            DefaultDirectorySaveFiles = string.Empty;
            RecentDirectorySaveFiles = string.Empty;
            HeaderLength = 1;
        }
    }
}
