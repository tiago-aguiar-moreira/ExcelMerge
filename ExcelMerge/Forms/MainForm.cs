using ExcelMerge.Configuration;
using ExcelMerge.Enumerator;
using ExcelMerge.Forms;
using ExcelMerge.Model;
using ExcelMerge.Utils;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExcelMerge
{
    public partial class MainForm : Form
    {
        private string _directoryApp;
        private BindingList<FileMergeModel> _listFiles;
        private ListChangedType[] _listEvents;
        private AppConfigModel _appConfig;

        public MainForm()
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _directoryApp = Path.GetDirectoryName(Application.ExecutablePath);
            _listFiles = new BindingList<FileMergeModel>();
            _listFiles.ListChanged += new ListChangedEventHandler(list_ListChanged);
            _appConfig = AppConfigManager.Load();
            _listEvents = new ListChangedType[]
            {
                ListChangedType.ItemAdded,
                ListChangedType.ItemDeleted,
                ListChangedType.Reset
            };

            gridVwFiles.DataSource = _listFiles;
            txtDefaultDirectorySaveFiles.Text = _appConfig.DefaultDirectorySaveFiles;
            txtHeaderLength.Value = _appConfig.HeaderLength;
            LoadEndProcessoAction(_appConfig.EndProcessAction);
            LoadHeaderAction(_appConfig.HeaderAction);
            pnlSettings.Visible = _appConfig.ShowConfigs;
        }

        private void LoadEndProcessoAction(EndProcessActionEnum selectedEndProcessAction)
        {
            var descriptions = EnumUtils.GetDescription<EndProcessActionEnum>();

            descriptions.ToList().ForEach(f => cbxAction.Items.Add(f));

            cbxAction.SelectedIndex = (int)selectedEndProcessAction;
        }

        private void LoadHeaderAction(HeaderActionEnum selectedHeaderAction)
        {
            var descriptions = EnumUtils.GetDescription<HeaderActionEnum>();

            descriptions.ToList().ForEach(f => cbxHeader.Items.Add(f));

            cbxHeader.SelectedIndex = (int)selectedHeaderAction;
        }

        private void list_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_listEvents.Contains(e.ListChangedType))
            {
                btnRun.Enabled = btnDelete.Enabled = btnDeleteAll.Enabled = _listFiles.Any();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _directoryApp;
                ofd.Filter = "Todos os arquivos (*.*)|*.*|Todos os Arquivos do Excel (*.xlsx;*.xls)|*.xlsx;*.xls";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = true;
                ofd.Title = Text;
                ofd.InitialDirectory = _appConfig.RecentDirectorySaveFiles;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var item in ofd.FileNames)
                    {
                        if (!_listFiles.Any(a => a.GetPath().ToLower() == item.ToLower()))
                        {
                            _listFiles.Add(new FileMergeModel(item));
                        }
                    }

                    _appConfig.RecentDirectorySaveFiles = _listFiles.LastOrDefault().Directory;
                    AppConfigManager.Save(_appConfig);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedRowCount = gridVwFiles.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount > 0)
            {
                for (int i = 0; i < selectedRowCount; i++)
                {
                    _listFiles.RemoveAt(gridVwFiles.SelectedRows[i].Index);
                }
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e) => _listFiles.Clear();

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                (sender as Button).Enabled = !(sender as Button).Enabled;

                var directoryDestiny = string.IsNullOrEmpty(_appConfig.DefaultDirectorySaveFiles)
                    ? _directoryApp 
                    : _appConfig.DefaultDirectorySaveFiles;

                foreach (var file in _listFiles)
                {
                    if (file.HeaderLength <= 0)
                        file.HeaderLength = byte.Parse(txtHeaderLength.Value.ToString());

                    if (string.IsNullOrEmpty(file.SeparatorCSV))
                        file.SeparatorCSV = txtSeparatorCSV.Text;
                }

                var frmProgress = new FormProgress(
                    _listFiles.ToArray(),
                    directoryDestiny,
                    _appConfig.HeaderAction);

                frmProgress.ShowDialog();

                if (!string.IsNullOrEmpty(frmProgress.NewFile))
                {
                    ExecuteAction(frmProgress.NewFile, _appConfig.EndProcessAction);
                }

                frmProgress.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Erro inesperado durante o processamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                (sender as Button).Enabled = !(sender as Button).Enabled;
            }
        }

        private void ExecuteAction(string path, EndProcessActionEnum processAction)
        {
            switch (processAction)
            {
                case EndProcessActionEnum.None:
                    break;
                case EndProcessActionEnum.OpenFile:
                    Process.Start(path);
                    break;
                case EndProcessActionEnum.OpenDir:
                    Process.Start(Path.GetDirectoryName(path));
                    break;
                case EndProcessActionEnum.AskIfShouldOpenFile:
                    if (MessageBox.Show(
                        this,
                        "Deseja abrir o arquivo gerado?",
                        "Ação ao fim do processamento",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(path);
                    }
                    break;
                case EndProcessActionEnum.AskIfShouldOpenDir:
                    if (MessageBox.Show(
                        this,
                        "Deseja abrir o diretório onde o arquivo foi gerado?",
                        "Ação ao fim do processamento",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(Path.GetDirectoryName(path));
                    }
                    break;
                default:
                    MessageBox.Show("A ação configurada sobre o arquivo gerado é inválida!\nRevise as configrurações.");
                    break;
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            _appConfig.ShowConfigs = !_appConfig.ShowConfigs;
            AppConfigManager.Save(_appConfig);
            pnlSettings.Visible = _appConfig.ShowConfigs;
        }

        private void cbxAction_SelectedIndexChanged(object sender, EventArgs e) =>
            _appConfig.EndProcessAction = (EndProcessActionEnum)(sender as ComboBox).SelectedIndex;

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

        private void TxtDefaultDirectorySaveFiles_TextChanged(object sender, EventArgs e) =>
            _appConfig.DefaultDirectorySaveFiles = (sender as TextBox).Text;

        private void CbxHeader_SelectedIndexChanged(object sender, EventArgs e) =>
            _appConfig.HeaderAction = (HeaderActionEnum)(sender as ComboBox).SelectedIndex;

        private void HeaderLength_ValueChanged(object sender, EventArgs e) =>
             _appConfig.HeaderLength = byte.Parse(Math.Truncate((sender as NumericUpDown).Value).ToString());

        private void SaveApp_Validating(object sender, CancelEventArgs e) =>
            AppConfigManager.Save(_appConfig);
    }
}