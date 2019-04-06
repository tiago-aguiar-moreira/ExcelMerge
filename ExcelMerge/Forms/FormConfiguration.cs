using ExcelMerge.Configuration;
using ExcelMerge.Enumerator;
using ExcelMerge.Model;
using ExcelMerge.Utils;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExcelMerge
{
    public partial class FormConfiguration : Form
    {
        private AppConfigModel _appConfig;

        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            this.SetBaseConfigs();

            _appConfig = AppConfigurationManager.Load();

            LoadEndProcessoAction(_appConfig.SelectedEndProcessAction);
            LoadHeaderAction(_appConfig.SelectedHeaderAction);

            txtDefaultDirectorySaveFiles.Text = _appConfig.DefaultDirectorySaveFiles;
        }

        private void LoadEndProcessoAction(SelectedEndProcessActionEnum selectedEndProcessAction)
        {
            var descriptions = EnumUtils.GetDescription<SelectedEndProcessActionEnum>();

            descriptions.ToList().ForEach(f => cbxAction.Items.Add(f));

            cbxAction.SelectedIndex = (int)selectedEndProcessAction;
        }

        private void LoadHeaderAction(SelectedHeaderActionEnum selectedHeaderAction)
        {
            var descriptions = EnumUtils.GetDescription<SelectedHeaderActionEnum>();

            descriptions.ToList().ForEach(f => cbxHeader.Items.Add(f));

            cbxHeader.SelectedIndex = (int)selectedHeaderAction;
        }

        private void FormConfiguration_FormClosing(object sender, FormClosingEventArgs e) => 
            AppConfigurationManager.Save(_appConfig);

        private void cbxAction_SelectedIndexChanged(object sender, EventArgs e) => 
            _appConfig.SelectedEndProcessAction = (SelectedEndProcessActionEnum)(sender as ComboBox).SelectedIndex;

        private void btnBrowserFolder_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog())
            {
                fileDialog.IsFolderPicker = true;
                fileDialog.InitialDirectory = _appConfig.DefaultDirectorySaveFiles;

                if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    txtDefaultDirectorySaveFiles.Text = fileDialog.FileName;
                }
            }
        }

        private void txtDefaultDirectorySaveFiles_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                return;
            }

            if (Directory.Exists(textBox.Text.Trim()))
            {
                _appConfig.DefaultDirectorySaveFiles = textBox.Text.Trim();
            }
            else
            {
                MessageBox.Show(
                    this,
                    "O diretório selecionado não é válido!",
                    "Diretório inválido",                    
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                textBox.Clear();
                textBox.Focus();
            }
        }

        private void txtDefaultDirectorySaveFiles_TextChanged(object sender, EventArgs e) => 
            _appConfig.DefaultDirectorySaveFiles = (sender as TextBox).Text;

        private void cbxHeader_SelectedIndexChanged(object sender, EventArgs e) =>
            _appConfig.SelectedHeaderAction = (SelectedHeaderActionEnum)(sender as ComboBox).SelectedIndex;

        private void headerLength_ValueChanged(object sender, EventArgs e) =>
             _appConfig.HeaderLength = byte.Parse(Math.Truncate((sender as NumericUpDown).Value).ToString());
    }
}