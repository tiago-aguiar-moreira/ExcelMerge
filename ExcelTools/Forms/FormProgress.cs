using ExcelTools.App.Utils;
using ExcelTools.Core;
using ExcelTools.Core.Enumerator;
using ExcelTools.Core.EventArgument;
using ExcelTools.Core.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace ExcelTools.App.Forms
{
    public partial class FormProgress : Form
    {
        #region Form

        private const string _formatDateTime = "dd/MM/yyyy HH:mm:ss.fff";
        private readonly Excel _excel;
        private string _fileName;
        private ParamsMergeModel[] _fileMerge;
        private EndProcessActionEnum _processAction;
        private DoWorkEventArgs _doWorkEventArgs;

        public FormProgress(ParamsMergeModel[] fileMerge, string destinyDirectory, HeaderActionEnum headerAction, EndProcessActionEnum processAction)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            richTxt.Clear();
            progBarFile.Value = 0;
            progBarFile.Maximum = fileMerge.Length;

            _processAction = processAction;
            _fileMerge = fileMerge;

            _excel = new Excel(destinyDirectory, headerAction);
            _excel.OnLog += new OnLogEventHandler(Log);
            _excel.OnProgress += new OnProgressChangedEventHandler(ProgressChanged);
            _excel.OnFinished += new OnFinishedEventHandler(Finished);
        }

        private void FormProgress_Load(object sender, EventArgs e)
            => worker.RunWorkerAsync(); //executes the process asynchronously

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _doWorkEventArgs.Cancel = true;
            _excel.Cancel();
            // Notifica a thread que o cancelamento foi solicitado.
            // Cancela a tarefa DoWork 
            worker.CancelAsync();

            //(sender as Button).Enabled = false;
        }

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
            => btnCancelar_Click(sender, e);
        #endregion

        #region Events from Excel class
        public void Log(object sender, LogArgs e)
        {
            richTxt.BeginInvoke(new Action(() =>
            {
                var text = $"{DateTime.Now.ToString(_formatDateTime, CultureInfo.InvariantCulture)}{Environment.NewLine}";

                switch (e.ElementType)
                {
                    case Core.Enumerator.EventLog.ReadFile:
                        text += $"Arquivo: {e.ElementName}";
                        _fileName = e.ElementName;
                        break;
                    case Core.Enumerator.EventLog.ReadSheet:
                        text += $"Planilha: {e.ElementName}";
                        break;
                    case Core.Enumerator.EventLog.BeforeSaveFile:
                        text += "Gerando arquivo...";
                        break;
                    case Core.Enumerator.EventLog.AfterSaveFile:
                        text += $"Arquivo gerado: {e.ElementName}";
                        break;
                    default:
                        break;
                }

                text += $"{Environment.NewLine}";
                text += $"{Environment.NewLine}";

                richTxt.AppendText(text);
            }));
        }

        public void ProgressChanged(object sender, ProgressArgs progress)
        {
            progBarFile.BeginInvoke(new Action(() =>
            {
                progBarFile.Value += progress.Value;
            }));

            lblFile.BeginInvoke(new Action(() =>
            {
                lblFile.Text = $"Arquivo {progBarFile.Value} de {progBarFile.Maximum} ({_fileName})";
            }));
        }

        public void Finished(object sender, FinishedArgs finishedArgs)
        {
            switch (_processAction)
            {
                case EndProcessActionEnum.None:
                    break;
                case EndProcessActionEnum.OpenFile:
                    Process.Start(finishedArgs.FileName);
                    break;
                case EndProcessActionEnum.OpenDir:
                    Process.Start(Path.GetDirectoryName(finishedArgs.FileName));
                    break;
                case EndProcessActionEnum.AskIfShouldOpenFile:
                    if (MessageBox.Show(
                        this,
                        "Deseja abrir o arquivo gerado?",
                        "Ação ao fim do processamento",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(finishedArgs.FileName);
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
                        Process.Start(Path.GetDirectoryName(finishedArgs.FileName));
                    }
                    break;
                default:
                    MessageBox.Show("A ação configurada sobre o arquivo gerado é inválida!\nRevise as configrurações.");
                    break;
            }

            
        }
        #endregion

        #region Worker
        /// <summary>
        /// Here we call our methods with the time-consuming tasks.
        /// </summary>
        private void backWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _doWorkEventArgs = e;
            _excel.Merge(_fileMerge);

            btnCancelar.BeginInvoke(new Action(() => 
            {
                btnCancelar.Enabled = false;
            }));
        }

        /// <summary>
        /// After the task is completed, this method is called to implement what should be done immediately upon completion of the task.
        /// </summary>
        private void backWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(
                    this,
                    "Operação cancelada pelo usuário!",
                    "Processamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Close();
            }
            else if (e.Error != null)
            {
                MessageBox.Show(
                    this,
                    e.Error.Message,
                    "Processamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
            else
            {
                MessageBox.Show(
                    this,
                    "Processamento concluído com sucesso!",
                    "Processamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            BeginInvoke(new Action(() =>
            {
                Close();
            }));
        }
        #endregion
    }
}