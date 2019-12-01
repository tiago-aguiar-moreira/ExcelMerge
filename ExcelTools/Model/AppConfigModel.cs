using ExcelTools.App.Enumerators;
using ExcelTools.Core.Enumerator;

namespace ExcelTools.App.Model
{
    public class AppConfigModel
    {
        public EndProcessAction EndProcessAction { get; set; }
        public HeaderAction HeaderAction { get; set; }
        public string DefaultDirectorySaveFiles { get; set; }
        public string RecentDirectorySaveFiles { get; set; }
        public byte HeaderLength { get; set; }
        public char? SeparadorCSV { get; set; }
        public string LastFileGenerated { get; set; }
        
        public AppConfigModel()
        {
            EndProcessAction = EndProcessAction.None;
            HeaderAction = HeaderAction.None;
            DefaultDirectorySaveFiles = string.Empty;
            RecentDirectorySaveFiles = string.Empty;
            HeaderLength = 1;
            SeparadorCSV = null;
            LastFileGenerated = string.Empty;
        }
    }
}