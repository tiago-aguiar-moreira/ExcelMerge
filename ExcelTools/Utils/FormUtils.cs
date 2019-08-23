using System.Windows.Forms;

namespace ExcelTools.App.Utils
{
    public static class FormUtils
    {
        public static void SetBaseConfigs(this Form form)
        {
            form.Icon = Properties.Resources.Excel;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Excel Merge";
        }

        public static void Open(this Form form)
        {
            form.ShowDialog();
        }
    }
}
