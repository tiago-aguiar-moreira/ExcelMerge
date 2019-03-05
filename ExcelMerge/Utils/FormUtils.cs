using System.Windows.Forms;

namespace ExcelMerge.Utils
{
    public static class FormUtils
    {
        public static void SetBasicConfigs(this Form form)
        {
            form.Icon = Properties.Resources.Excel;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public static void Open(this Form form)
        {
            form.SetBasicConfigs();            
            form.ShowDialog();
        }
    }
}
