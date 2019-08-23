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

        private const string _msgLabelFile = "Progresso do processamento ({0})";
        private const string _formatDateTime = "dd/MM/yyyy HH:mm:ss.fff";
        private readonly ExcelMerge _excel;
        private string _fileName;
        private ParamsMergeModel[] _fileMerge;
        private EndProcessActionEnum _processAction;
        private string _destinyDirectory;
        private HeaderActionEnum _headerAction;
        private DoWorkEventArgs _doWorkEventArgs;
        private Stopwatch _stopwatch;

        public FormProgress(ParamsMergeModel[] fileMerge, string destinyDirectory, HeaderActionEnum headerAction, EndProcessActionEnum processAction)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            richTxt.Clear();
            progBarFile.Value = 0;
            progBarFile.Maximum = fileMerge.Length;

            _processAction = processAction;
            _fileMerge = fileMerge;
            _destinyDirectory = destinyDirectory;
            _headerAction = headerAction;

            _stopwatch = new Stopwatch();

            _excel = new ExcelMerge();
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
            worker.CancelAsync();
        }

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCount();
            btnCancelar_Click(sender, e);
        }
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
            timer.Start();
            _stopwatch.Start();
            _excel.Execute(_fileMerge, _destinyDirectory, _headerAction);

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
            StopCount();

            if (e.Cancelled)
            {
                MessageBox.Show(
                    this,
                    "Operação cancelada pelo usuário!",
                    "Processamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(
                    this,
                    e.Error.Message,
                    "Processamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

        private void StopCount()
        {
            timer.Stop();
            _stopwatch.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeElapsed = _stopwatch.Elapsed;

            lblCrono.BeginInvoke(new Action(() =>
            {
                lblCrono.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            timeElapsed.Hours,
                            timeElapsed.Minutes,
                            timeElapsed.Seconds);
            }));
        }
    }
}