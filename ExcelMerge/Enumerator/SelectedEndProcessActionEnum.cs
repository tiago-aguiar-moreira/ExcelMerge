using System.ComponentModel;

namespace ExcelMerge.Enumerator
{
    public enum EndProcessActionEnum
    {
        [Description("Nenhuma")]
        None = 0,
        [Description("Abrir o arquivo")]
        OpenFile = 1,
        [Description("Abrir diretório")]
        OpenDir = 2,
        [Description("Perguntar se deve abrir o arquivo")]
        AskIfShouldOpenFile = 3,
        [Description("Perguntar se deve abrir o diretório")]
        AskIfShouldOpenDir = 4
    }
}