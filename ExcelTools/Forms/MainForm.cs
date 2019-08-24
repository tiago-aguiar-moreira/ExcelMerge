using ExcelTools.App.Configuration;
using ExcelTools.App.Forms;
using ExcelTools.App.Utils;
using ExcelTools.Core.Model;
using ExcelTools.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExcelTools.App
{
    public partial class MainForm : Form
    {
        private BindingList<ParamsMergeModel> _listFiles;
        private ListChangedType[] _listEvents;

        public MainForm()
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _listFiles = new BindingList<ParamsMergeModel>();
            _listFiles.ListChanged += new ListChangedEventHandler(list_ListChanged);
            
            _listEvents = new ListChangedType[]
            {
                ListChangedType.ItemAdded,
                ListChangedType.ItemDeleted,
                ListChangedType.Reset
            };

            gridVwFiles.DataSource = _listFiles;
            EnableButtons();
            SetLastFile();
        }

        private string GetDirectoryApp()
            => Path.GetDirectoryName(Application.ExecutablePath);

        private void list_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_listEvents.Contains(e.ListChangedType))
            {
                EnableButtons();
            }
        }

        private void EnableButtons()
            => btnRun.Enabled = btnDelete.Enabled = btnDeleteAll.Enabled = _listFiles.Any();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var appConfig = AppConfigManager.Load();

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = GetDirectoryApp();
                ofd.Filter = "Todos os arquivos (*.*)|*.*|Todos os Arquivos do Excel (*.xlsx;*.xls)|*.xlsx;*.xls";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = true;
                ofd.Title = Text;
                ofd.InitialDirectory = appConfig.RecentDirectorySaveFiles;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var item in ofd.FileNames)
                    {
                        if (!_listFiles.Any(a => a.GetPath().ToLower() == item.ToLower()))
                        {
                            _listFiles.Add(new ParamsMergeModel(item));
                        }
                    }

                    appConfig.RecentDirectorySaveFiles = _listFiles.LastOrDefault().Directory;
                    AppConfigManager.Save(appConfig);
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

        private void btnDeleteAll_Click(object sender, EventArgs e) 
            => _listFiles.Clear();

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                var appConfig = AppConfigManager.Load();

                var directoryDestiny = string.IsNullOrEmpty(appConfig.DefaultDirectorySaveFiles)
                    ? GetDirectoryApp()
                    : appConfig.DefaultDirectorySaveFiles;

                foreach (var file in _listFiles)
                {
                    if (file.HeaderLength <= 0)
                        file.HeaderLength = appConfig.HeaderLength;

                    if (file.SeparatorCSV == null)
                        file.SeparatorCSV = appConfig.SeparadorCSV;
                }

                var frmProgress = new FormProgress(
                    _listFiles.ToArray(),
                    directoryDestiny,
                    appConfig.HeaderAction,
                    appConfig.EndProcessAction);

                frmProgress.ShowDialog();
                frmProgress.Dispose();
                SetLastFile();
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
        }

        private void SetLastFile()
        {
            var lastFileGenerated = AppConfigManager.Load().LastFileGenerated;
            stLabel.Text = string.Format("Último arquivo gerado: {0}", lastFileGenerated);
            stLabel.ForeColor = File.Exists(lastFileGenerated) ? SystemColors.ControlText : Color.Red;
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            var frm = new FormConfig();
            frm.ShowDialog();
            frm.Dispose();
        }
    }
}