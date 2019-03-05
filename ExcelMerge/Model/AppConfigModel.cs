using ExcelMerge.Enumerator;

namespace ExcelMerge
{
    public class AppConfigModel
    {
        public SelectedEndProcessActionEnum SelectedEndProcessAction { get; set; }

        public AppConfigModel() => SelectedEndProcessAction = SelectedEndProcessActionEnum.None;
    }    
}
