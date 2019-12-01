using System.ComponentModel;

namespace ExcelTools.Core.Enumerator
{
    public enum HeaderAction
    {
        [Description("Nenhuma ação")]
        None = 0,
        [Description("Considerar do primeiro arquivo")]
        ConsiderFirstFile = 1,
        [Description("Ignorar todos")]
        IgnoreAll = 2
    }
}
