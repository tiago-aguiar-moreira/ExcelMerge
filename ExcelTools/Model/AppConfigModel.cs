using ExcelTools.Core.Enumerator;

namespace ExcelTools.App.Model
{
    public class AppConfigModel
    {
        public EndProcessActionEnum EndProcessAction { get; set; }
        public HeaderActionEnum HeaderAction { get; set; }
        public string DefaultDirectorySaveFiles { get; set; }
        public string RecentDirectorySaveFiles { get; set; }
        public byte HeaderLength { get; set; }
        public char? SeparadorCSV { get; set; }
        public string LastFileGenerated { get; set; }

        public AppConfigModel()
        {
            EndProcessAction = EndProcessActionEnum.None;
            HeaderAction = HeaderActionEnum.None;
            DefaultDirectorySaveFiles = string.Empty;
            RecentDirectorySaveFiles = string.Empty;
            HeaderLength = 1;
            SeparadorCSV = null;
            LastFileGenerated = string.Empty;
        }
    }
}