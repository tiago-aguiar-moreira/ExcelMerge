using ExcelTools.App.Configuration;
using ExcelTools.App.Enumerators;
using ExcelTools.App.Model;
using ExcelTools.App.Utils;
using ExcelTools.Core.Enumerator;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace ExcelTools.App
{
    public partial class FormConfig : Form
    {
        private readonly AppConfigModel _appConfig;

        public FormConfig()
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _appConfig = AppConfigManager.Load();

            txtDefaultDirectorySaveFiles.Text = _appConfig.DefaultDirectorySaveFiles;
            txtHeaderLength.Value = _appConfig.HeaderLength;
            txtSeparatorCSV.Text = _appConfig?.SeparadorCSV?.ToString() ?? string.Empty;

            cbxAction.LoadComboFromEnum<EndProcessAction>((int)_appConfig.EndProcessAction);
            cbxHeader.LoadComboFromEnum<HeaderAction>((int)_appConfig.HeaderAction);
            cbxLanguage.LoadComboFromEnum<SystemLanguage>((int)_appConfig.Language);
        }

        private void BtnBrowserFolder_Click(object sender, EventArgs e)
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

        private void TxtDefaultDirectorySaveFiles_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                return;
            }

            if (Directory.Exists(textBox.Text.Trim()))
            {
                _appConfig.DefaultDirectorySaveFiles = textBox.Text.Trim();
                AppConfigManager.Save(_appConfig);
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

        private void TxtDefaultDirectorySaveFiles_TextChanged(object sender, EventArgs e)
            => _appConfig.DefaultDirectorySaveFiles = (sender as TextBox).Text;

        private void CbxHeader_SelectedIndexChanged(object sender, EventArgs e)
            => _appConfig.HeaderAction = (HeaderAction)(sender as ComboBox).SelectedIndex;

        private void HeaderLength_ValueChanged(object sender, EventArgs e)
            => _appConfig.HeaderLength = byte.Parse(Math.Truncate((sender as NumericUpDown).Value).ToString());

        private void TxtSeparatorCSV_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((sender as TextBox).Text))
                _appConfig.SeparadorCSV = null;
            else
                _appConfig.SeparadorCSV = (sender as TextBox).Text[0];
        }

        private void cbxAction_SelectedIndexChanged(object sender, EventArgs e)
            => _appConfig.EndProcessAction = (EndProcessAction)(sender as ComboBox).SelectedIndex;

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            AppConfigManager.Save(_appConfig);
            Close();
        }

        private void CbxLanguage_SelectedIndexChanged(object sender, EventArgs e)
            => _appConfig.Language = (SystemLanguage)(sender as ComboBox).SelectedIndex;
    }
}