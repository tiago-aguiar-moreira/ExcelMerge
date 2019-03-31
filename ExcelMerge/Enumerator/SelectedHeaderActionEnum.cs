using System.ComponentModel;

namespace ExcelMerge.Enumerator
{
    public enum SelectedHeaderActionEnum
    {
        [Description("Nenhuma ação")]
        None = 0,
        [Description("Considerar do primeiro arquivo")]
        ConsiderFirstFile = 1,
        [Description("Ignorar todos")]
        IgnoreAll = 2
    }
}
