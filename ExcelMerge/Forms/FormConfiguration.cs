using ExcelMerge.Enumerator;
using ExcelMerge.Utils;
using System;
using System.Windows.Forms;

namespace ExcelMerge
{
    public partial class FormConfiguration : Form
    {
        private AppConfigModel _appConfig;
        private AppConfiguration _appConfiguration;

        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            _appConfiguration = new AppConfiguration();
            _appConfig = _appConfiguration.Load();

            LoadEndProcessoAction(_appConfig.SelectedEndProcessAction);
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

        private void FormConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            _appConfiguration.Save(_appConfig);
        }

        private void cbxAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            _appConfig.SelectedEndProcessAction = (SelectedEndProcessActionEnum)(sender as ComboBox).SelectedIndex;
        }
    }
}