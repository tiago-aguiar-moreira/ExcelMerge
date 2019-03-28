using ClosedXML.Excel;
using ExcelMerge.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ExcelMerge.Forms
{
    public partial class FormProgress : Form
    {
        private IXLWorkbook _mainWorkbook;
        private IXLWorksheet _mainWorksheet;
        private MergeProgess Progress;
        private string[] _filePath;
        private string _destinyDirectory;

        public string NewFile { get; set; }

        public FormProgress(string[] filePath, string destinyDirectory)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _filePath = filePath;
            _destinyDirectory = destinyDirectory;

            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("First");

            progBarFile.Value = 0;
            progBarSheet.Value = 0;
            progBarRow.Value = 0;

            //executa o processo de forma assincrona.
            backWorker.RunWorkerAsync();
        }

        private string NewFileName(string destinyDirectory) => $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

        private string SaveFile(string destinyDirectory)
        {
            var newFileName = NewFileName(destinyDirectory);
            _mainWorkbook.SaveAs(newFileName);
            return newFileName;
        }

        private void SetTotals(string[] filePath)
        {
            Progress = new MergeProgess(filePath.Length);

            for (int indexFilePath = 0; indexFilePath < Progress.File.Length; indexFilePath++) // Loop in files
            {
                var sheets = new XLWorkbook(filePath[indexFilePath]).Worksheets; // Get sheets from file

                Progress.File[indexFilePath] = new MergeProgessFiles(sheets.Count)
                {
                    Sheet = new MergeProgessSheets[sheets.Count]
                };

                for (int indexSheet = 0; indexSheet < Progress.File[indexFilePath].Sheet.Length; indexSheet++) // Loop in sheets
                {
                    var lengthRow = sheets.Worksheet(indexSheet + 1).RowsUsed().Count();

                    Progress.File[indexFilePath].Sheet[indexSheet] = new MergeProgessSheets(lengthRow);
                }
            }
        }

        private bool CancellationPending(DoWorkEventArgs e)
        {
            var cancel = backWorker.CancellationPending;

            //Verifica se houve uma requisição para cancelar a operação.
            if (cancel)
            {
                //se sim, define a propriedade Cancel para true para que o evento WorkerCompleted saiba que a tarefa foi cancelada.
                e.Cancel = true;

                //zera o percentual de progresso do backgroundWorker1.
                backWorker.ReportProgress(0);
            }

            return cancel;
        }

        public string Execute(DoWorkEventArgs e)
        {
            SetTotals(_filePath);

            int _rowReturnFileCount = 1;

            SetMaximumProgressBar(progBarFile, Progress.File.Length);
            for (int indexFile = 0; indexFile < Progress.File.Length; indexFile++) // Loop in files
            {
                if (CancellationPending(e))
                    return string.Empty;

                backWorker.ReportProgress(indexFile);

                var sheets = new XLWorkbook(_filePath[indexFile]).Worksheets; // Get sheets from file

                Progress.File[indexFile].Progress = indexFile + 1;

                RefreshLabelAndProgressBar(
                    lblFile,
                    progBarFile,
                    Progress.File[indexFile].Progress,
                    $"Arquivo {Progress.File[indexFile].Progress.ToString()} de {progBarFile.Maximum} ({Path.GetFileName(_filePath[indexFile])})");

                SetMaximumProgressBar(progBarSheet, Progress.File[indexFile].Sheet.Length);
                for (int indexSheet = 0; indexSheet < Progress.File[indexFile].Sheet.Length; indexSheet++) // Loop in sheets
                {
                    if (CancellationPending(e))
                        return string.Empty;

                    Progress.File[indexFile].Sheet[indexSheet].Progress = indexSheet + 1;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    RefreshLabelAndProgressBar(
                        lblSheet,
                        progBarSheet,
                        Progress.File[indexFile].Progress,
                        $"Planilha {Progress.File[indexFile].Progress.ToString()} de {progBarSheet.Maximum} ({sheet.Name})");

                    SetMaximumProgressBar(progBarRow, Progress.File[indexFile].Sheet[indexSheet].Rows.Total);
                    for (int indexRow = 0; indexRow < Progress.File[indexFile].Sheet[indexSheet].Rows.Total; indexRow++) // Loop in rows
                    {
                        if (CancellationPending(e))
                            return string.Empty;

                        Progress.File[indexFile].Sheet[indexSheet].Rows.Progress = indexRow + 1;

                        RefreshLabelAndProgressBar(
                            lblRow,
                            progBarRow,
                            Progress.File[indexFile].Sheet[indexSheet].Rows.Progress,
                            $"Linha {Progress.File[indexFile].Sheet[indexSheet].Rows.Progress.ToString()} de {progBarRow.Maximum}");

                        var row = sheet.Row(indexRow + 1);

                        int _columnReturnFileCount = 1;

                        for (int indexCell = 0; indexCell <= row.RowUsed().CellCount(); indexCell++) // Loop in cells
                        {
                            _mainWorksheet.Cell(_rowReturnFileCount, _columnReturnFileCount).Value = row.Cell(indexCell + 1).Value.ToString();

                            _columnReturnFileCount += 1;
                        }

                        _rowReturnFileCount += 1;
                    }
                }
            }

            //Finalmente, caso tudo esteja ok, finaliza o progresso em 100%.
            backWorker.ReportProgress(100);

            return SaveFile(_destinyDirectory);
        }

        private void SetMaximumProgressBar(ProgressBar prog, int maximum) => prog.BeginInvoke(new Action(() => { prog.Maximum = maximum; }));

        private void RefreshLabelAndProgressBar(Label label, ProgressBar prog, int progress, string textLabel)
        {
            //var progress = index + 1;
            //label.Text = $"{progress.ToString()} de {prog.Maximum}";

            label.BeginInvoke(new Action(() =>
            {
                label.Text = textLabel;
            }));

            prog.BeginInvoke(new Action(() =>
            {
                prog.Value = progress;
            }));
            Thread.Sleep(1);
        }

        /// <summary>
        /// //Aqui chamamos os nossos metodos com as tarefas demoradas.
        /// </summary>
        private void backWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            NewFile = Execute(e);
        }

        /// <summary>
        /// Aqui implementamos o que desejamos fazer enquanto o progresso da tarefa é modificado,[incrementado].
        /// </summary>
        private void backWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Incrementa o valor da progressbar com o valor atual do progresso da tarefa.
            //progBarFile.Value = e.ProgressPercentage;

            //informa o percentual na forma de texto.
            //lblFile.Text = e.ProgressPercentage.ToString() + "%";
        }

        /// <summary>
        /// Após a tarefa ser concluida, esse metodo e chamado para
        /// implementar o que deve ser feito imediatamente após a conclusão da tarefa.
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

            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Cancelamento da tarefa com fim determinado [backWorker]
            if (backWorker.IsBusy)//se o backWorker estiver ocupado
            {
                // notifica a thread que o cancelamento foi solicitado.
                // Cancela a tarefa DoWork 
                backWorker.CancelAsync();
            }

            btnCancelar.Enabled = false;
        }

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnCancelar_Click(sender, e);
        }
    }
}