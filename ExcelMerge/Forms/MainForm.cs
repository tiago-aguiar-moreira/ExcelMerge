using ExcelMerge.Enumerator;
using System.Diagnostics;
using ExcelMerge.Utils;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExcelMerge.Configuration;

namespace ExcelMerge
{
    public partial class MainForm : Form 
    {
        private const string _msgFileList = "Arquivos selecionados...";
        private string _directoryApp = Path.GetDirectoryName(Application.ExecutablePath);
        private bool _addedFile = false;

        public MainForm()
        {
            InitializeComponent();
            this.SetBasicConfigs();
            lbxSelectedFiles.Items.Add(_msgFileList);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _directoryApp;
                //ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                //ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = true;
                ofd.Title = Text;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!_addedFile)
                    {
                        _addedFile = true;
                        lbxSelectedFiles.Items.Clear();
                    }

                    ofd.FileNames.ToList().ForEach(f => lbxSelectedFiles.Items.Add(f));
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxSelectedFiles.SelectedItems.Count > 0)
            {
                for (int i = lbxSelectedFiles.SelectedItems.Count - 1; i >= 0; i--)
                {
                    lbxSelectedFiles.Items.RemoveAt(lbxSelectedFiles.SelectedIndices[i]);
                }

                if (lbxSelectedFiles.Items.Count <= 0)
                {
                    _addedFile = false;
                    lbxSelectedFiles.Items.Add(_msgFileList);
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (_addedFile)
            {
                var appConfigManager = new AppConfigurationManager().Load();

                string newFile = new Merge().Execute(
                    lbxSelectedFiles.Items.Cast<string>().ToArray(),
                    string.IsNullOrEmpty(appConfigManager.DefaultDirectorySaveFiles) ? _directoryApp : appConfigManager.DefaultDirectorySaveFiles);

                ExecuteAction(newFile, appConfigManager.SelectedEndProcessAction);
            }
            else
            {
                MessageBox.Show("Nenhum arquivo arquivo foi adicionado");
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
                        "Ação ao fim do processamento",
                        "Deseja abrir o arquivo gerado?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(path);
                    }
                    break;
                case SelectedEndProcessActionEnum.AskIfShouldOpenDir:
                    if (MessageBox.Show(
                        this,
                        "Ação ao fim do processamento",
                        "Deseja abrir o diretório onde o arquivo foi gerado?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(Path.GetDirectoryName(path));
                    }
                    break;
                default:
                    MessageBox.Show("A ação configurada sobre o arquivo gerado é inválida!\nRevise sua configrurações.");
                    break;
            }
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.Open(new FormConfiguration());
        }
    }
}