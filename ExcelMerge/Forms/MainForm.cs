using ExcelMerge.Configuration;
using ExcelMerge.Enumerator;
using ExcelMerge.Forms;
using ExcelMerge.Utils;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExcelMerge
{
    public partial class MainForm : Form
    {
        private string _directoryApp;
        private BindingList<string> _listFiles;
        private ListChangedType[] listEvents = new ListChangedType[]
        {
            ListChangedType.ItemAdded,
            ListChangedType.ItemDeleted,
            ListChangedType.Reset
        };

        public MainForm()
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _directoryApp = Path.GetDirectoryName(Application.ExecutablePath);

            _listFiles = new BindingList<string>();
            _listFiles.ListChanged += new ListChangedEventHandler(list_ListChanged);

            lbxFiles.DataSource = _listFiles;
        }

        private void list_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (listEvents.Contains(e.ListChangedType))
            {
                btnDeleteAll.Enabled = _listFiles.Any();
                btnDelete.Enabled = _listFiles.Any();
                btnRun.Enabled = _listFiles.Any();
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
                ofd.InitialDirectory = AppConfigurationManager.Load().RecentDirectorySaveFiles;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ofd.FileNames.ToList().ForEach(f => _listFiles.Add(f));
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) => lbxFiles.SelectedItems.Cast<string>().ToList().ForEach(f => _listFiles.Remove(f));

        private void btnDeleteAll_Click(object sender, EventArgs e) => _listFiles.Clear();

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                (sender as Button).Enabled = !(sender as Button).Enabled;
                var appConfig = AppConfigurationManager.Load();

                var frmProgress = new FormProgress(
                    _listFiles.ToArray(),
                    string.IsNullOrEmpty(appConfig.DefaultDirectorySaveFiles) ? _directoryApp : appConfig.DefaultDirectorySaveFiles,
                    appConfig.SelectedHeaderAction);

                frmProgress.ShowDialog();

                if (!string.IsNullOrEmpty(frmProgress.NewFile))
                {
                    ExecuteAction(frmProgress.NewFile, appConfig.SelectedEndProcessAction);
                }

                appConfig.RecentDirectorySaveFiles = Path.GetDirectoryName(_listFiles.LastOrDefault());

                AppConfigurationManager.Save(appConfig);
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

        private void ExecuteAction(string path, SelectedEndProcessActionEnum processAction)
        {
            switch (processAction)
            {
                case SelectedEndProcessActionEnum.None:
                    break;
                case SelectedEndProcessActionEnum.OpenFile:
                    Process.Start(path);
                    break;
                case SelectedEndProcessActionEnum.OpenDir:
                    Process.Start(Path.GetDirectoryName(path));
                    break;
                case SelectedEndProcessActionEnum.AskIfShouldOpenFile:
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
                case SelectedEndProcessActionEnum.AskIfShouldOpenDir:
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

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e) => 
            FormUtils.Open(new FormConfiguration());
    }
}