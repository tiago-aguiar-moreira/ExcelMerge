using ExcelMerge.Configuration;
using ExcelMerge.Enumerator;
using ExcelMerge.Model;
using ExcelMerge.Utils;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows.Forms;

namespace ExcelMerge
{
    public partial class FormConfiguration : Form
    {
        private AppConfigModel _appConfig;
        private AppConfigurationManager _appConfigManager;
        private CommonOpenFileDialog _fileDialog;

        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            _appConfigManager = new AppConfigurationManager();
            _appConfig = _appConfigManager.Load();

            LoadEndProcessoAction(_appConfig.SelectedEndProcessAction);

            txtDefaultDirectorySaveFiles.Text = _appConfig.DefaultDirectorySaveFiles;
            _fileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = _appConfig.DefaultDirectorySaveFiles
            };
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
            _appConfigManager.Save(_appConfig);
        }

        private void cbxAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            _appConfig.SelectedEndProcessAction = (SelectedEndProcessActionEnum)(sender as ComboBox).SelectedIndex;
        }

        private void btnBrowserFolder_Click(object sender, EventArgs e)
        {
            if (_fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _appConfig.DefaultDirectorySaveFiles = _fileDialog.FileName;
                txtDefaultDirectorySaveFiles.Text = _fileDialog.FileName;
            }
        }

        private void txtDefaultDirectorySaveFiles_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var textBox = sender as TextBox;
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
            }
        }
    }
}