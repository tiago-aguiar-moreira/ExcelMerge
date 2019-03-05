using ExcelMerge.Enumerator;
using ExcelMerge.Utils;
using System;
using System.Windows.Forms;

namespace ExcelMerge
{
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            var appConfig = AppConfiguration.Load("AppConfig");

            LoadEndProcessoAction(appConfig.SelectedEndProcessAction);
        }

        private void LoadEndProcessoAction(SelectedEndProcessActionEnum selectedEndProcessAction)
        {
            var descriptions = EnumUtils.GetDescription<SelectedEndProcessActionEnum>();

            foreach (var item in descriptions)
            {
                cbxAction.Items.Add(item);
            }

            cbxAction.SelectedIndex = (int)selectedEndProcessAction;
        }
    }
}
