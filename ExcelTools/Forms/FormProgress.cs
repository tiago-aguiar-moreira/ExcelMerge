using ExcelTools.App.Configuration;
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
        private DoWorkEventArgs _doWorkEventArgs;
        private string _fileName;
        private readonly string _destinyDirectory;
        private readonly ExcelMerge _exMerge;
        private readonly ParamsMergeModel[] _fileMerge;
        private readonly HeaderAction _headerAction;
        private readonly EndProcessAction _processAction;
        private readonly Stopwatch _stopwatch;

        public FormProgress(ParamsMergeModel[] fileMerge, string destinyDirectory, HeaderAction headerAction, EndProcessAction processAction)
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

            _exMerge = new ExcelMerge();
            _exMerge.OnLog += new OnLogEventHandler(EventLog);
            _exMerge.OnProgress += new OnProgressChangedEventHandler(EventProgressChanged);
            _exMerge.OnFinished += new OnFinishedEventHandler(EventFinished);
        }

        private void FormProgress_Load(object sender, EventArgs e)
            => worker.RunWorkerAsync(); //executes the process asynchronously

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _doWorkEventArgs.Cancel = true;
            _exMerge.Cancel();
            worker.CancelAsync();
        }

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCount();
            btnCancelar_Click(sender, e);
        }
        #endregion

        #region Events from Excel class
        public void EventLog(object sender, LogArgs log)
        {
            if (richTxt.Created)
            {
                richTxt.BeginInvoke(new Action(() =>
                {
                    var text = $"{DateTime.Now.ToString(_formatDateTime, CultureInfo.InvariantCulture)}{Environment.NewLine}";

                    switch (log.ElementType)
                    {
                        case Core.Enumerator.EventLog.ReadFile:
                            text += $"Arquivo: {log.ElementName}";
                            _fileName = log.ElementName;
                            break;
                        case Core.Enumerator.EventLog.ReadSheet:
                            text += $"Planilha: {log.ElementName}";
                            break;
                        case Core.Enumerator.EventLog.BeforeSaveFile:
                            text += "Gerando arquivo...";
                            break;
                        case Core.Enumerator.EventLog.AfterSaveFile:
                            text += $"Arquivo gerado: {log.ElementName}";
                            break;
                        default:
                            break;
                    }

                    text += $"{Environment.NewLine}";
                    text += $"{Environment.NewLine}";

                    richTxt.AppendText(text);
                }));
            }
        }

        public void EventProgressChanged(object sender, ProgressArgs progress)
        {
            if (progBarFile.Created)
            {
                progBarFile.BeginInvoke(new Action(() =>
                {
                    progBarFile.Value += progress.Value;
                }));
            }

            if (lblFile.Created)
            {
                lblFile.BeginInvoke(new Action(() =>
                {
                    lblFile.Text = $"Arquivo {progBarFile.Value} de {progBarFile.Maximum} ({_fileName})";
                }));
            }
        }

        public void EventFinished(object sender, FinishedArgs finished)
        {
            var appConfig = AppConfigManager.Load();
            appConfig.LastFileGenerated = finished.FileName;
            AppConfigManager.Save(appConfig);

            switch (_processAction)
            {
                case EndProcessAction.None:
                    break;
                case EndProcessAction.OpenFile:
                    Process.Start(finished.FileName);
                    break;
                case EndProcessAction.OpenDir:
                    Process.Start(Path.GetDirectoryName(finished.FileName));
                    break;
                case EndProcessAction.AskIfShouldOpenFile:
                    if (MessageBox.Show(
                        this,
                        "Deseja abrir o arquivo gerado?",
                        "Ação ao fim do processamento",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(finished.FileName);
                    }
                    break;
                case EndProcessAction.AskIfShouldOpenDir:
                    if (MessageBox.Show(
                        this,
                        "Deseja abrir o diretório onde o arquivo foi gerado?",
                        "Ação ao fim do processamento",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(Path.GetDirectoryName(finished.FileName));
                    }
                    break;
                default:
                    MessageBox.Show("A ação configurada sobre o arquivo gerado é inválida!\nVerifique as configrurações.");
                    break;
            }
        }
        #endregion

        #region Worker events
        /// <summary>
        /// Here we call our methods with the time-consuming tasks.
        /// </summary>
        private void backWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _doWorkEventArgs = e;
            timer.Start();
            _stopwatch.Start();
            _exMerge.Execute(_fileMerge, _destinyDirectory, _headerAction);

            if (btnCancelar.Created)
            {
                btnCancelar.BeginInvoke(new Action(() =>
                {
                    btnCancelar.Enabled = false;
                }));
            }
        }

        /// <summary>
        /// After the task is completed, this method is called to implement what should be done immediately upon completion of the task.
        /// </summary>
        private void backWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StopCount();

            if (Created)
            {
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