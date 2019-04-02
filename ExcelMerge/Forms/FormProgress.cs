using ClosedXML.Excel;
using ExcelMerge.Enumerator;
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
        private SelectedHeaderActionEnum _selectedHeaderAction;
        private DoWorkEventArgs _eventDoWork;
        private int _rowReturnFileCount;
        private int _columnReturnFileCount;

        public string NewFile { get; private set; }

        public FormProgress(string[] filePath, string destinyDirectory, SelectedHeaderActionEnum selectedHeaderActionEnum)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _filePath = filePath;
            _destinyDirectory = destinyDirectory;
            _selectedHeaderAction = selectedHeaderActionEnum;
            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("Planilha 1");

            progBarFile.Value = 0;
            progBarSheet.Value = 0;
            progBarRow.Value = 0;

            backWorker.RunWorkerAsync(); //executes the process asynchronously.
        }

        private string NewFileName(string destinyDirectory) =>
            $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

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

            if (cancel) //Checks if there was a request to cancel the operation.
            {
                // if yes, sets the Cancel property to true so that the WorkerCompleted event knows that the task has been canceled.
                e.Cancel = true;

                backWorker.ReportProgress(0); // zero the progress percentage of backgroundWorker1.
            }

            return cancel;
        }

        public string Execute()
        {
            SetTotals(_filePath);

            _rowReturnFileCount = 1;

            SetMaximumProgressBar(progBarFile, Progress.File.Length);
            for (int indexFile = 0; indexFile < Progress.File.Length; indexFile++) // Loop in files
            {
                if (CancellationPending(_eventDoWork))
                    return string.Empty;

                backWorker.ReportProgress(indexFile);

                Progress.File[indexFile].Progress = indexFile + 1;

                RefreshLabelAndProgressBar(
                    lblFile,
                    progBarFile,
                    Progress.File[indexFile].Progress,
                    $"Arquivo {Progress.File[indexFile].Progress} de {progBarFile.Maximum} ({Path.GetFileName(_filePath[indexFile])})");

                SetMaximumProgressBar(progBarSheet, Progress.File[indexFile].Sheet.Length);

                var sheets = new XLWorkbook(_filePath[indexFile]).Worksheets; // Get sheets from file

                for (int indexSheet = 0; indexSheet < Progress.File[indexFile].Sheet.Length; indexSheet++) // Loop in sheets
                {
                    if (CancellationPending(_eventDoWork))
                        return string.Empty;

                    Progress.File[indexFile].Sheet[indexSheet].Progress = indexSheet + 1;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    RefreshLabelAndProgressBar(
                        lblSheet,
                        progBarSheet,
                        Progress.File[indexFile].Sheet[indexSheet].Progress,
                        $"Planilha {Progress.File[indexFile].Sheet[indexSheet].Progress} de {progBarSheet.Maximum} ({sheet.Name})");

                    SetMaximumProgressBar(progBarRow, Progress.File[indexFile].Sheet[indexSheet].Rows.Total);
                    for (int indexRow = 0; indexRow < Progress.File[indexFile].Sheet[indexSheet].Rows.Total; indexRow++) // Loop in rows
                    {
                        if (CancellationPending(_eventDoWork))
                            return string.Empty;

                        Progress.File[indexFile].Sheet[indexSheet].Rows.Progress = indexRow + 1;

                        RefreshLabelAndProgressBar(
                            lblRow,
                            progBarRow,
                            Progress.File[indexFile].Sheet[indexSheet].Rows.Progress,
                            $"Linha {Progress.File[indexFile].Sheet[indexSheet].Rows.Progress} de {progBarRow.Maximum}");

                        var row = sheet.Row(indexRow + 1);

                        _columnReturnFileCount = 1;

                        switch (_selectedHeaderAction)
                        {
                            case SelectedHeaderActionEnum.ConsiderFirstFile:
                                if (indexFile == 0 && indexSheet == 0 && indexRow == 0)
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    _rowReturnFileCount += 1;
                                }
                                else if (indexRow != 0)
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    _rowReturnFileCount += 1;
                                }
                                break;
                            case SelectedHeaderActionEnum.IgnoreAll: // * Ignore all headers!!!
                                if (indexRow == 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    _rowReturnFileCount += 1;
                                    break;
                                }
                            case SelectedHeaderActionEnum.None:
                                if (!AddNewRow(row.RowUsed().CellCount(), row))
                                    return string.Empty;

                                _rowReturnFileCount += 1;
                                break;
                        }
                    }
                }
            }

            backWorker.ReportProgress(100);

            return SaveFile(_destinyDirectory);
        }

        private bool AddNewRow(int cellCount, IXLRow row)
        {
            for (int indexCell = 0; indexCell < cellCount; indexCell++) // Loop in cells
            {
                if (CancellationPending(_eventDoWork))
                    return false;

                _mainWorksheet.Cell(_rowReturnFileCount, _columnReturnFileCount).Value = row.Cell(indexCell + 1).Value.ToString();

                _columnReturnFileCount += 1;
            }

            return true;
        }

        private void SetMaximumProgressBar(ProgressBar prog, int maximum)
        {
            prog.BeginInvoke(new Action(() => { prog.Maximum = maximum; }));

            Thread.Sleep(1);
        }

        private void RefreshLabelAndProgressBar(Label label, ProgressBar prog, int progress, string textLabel)
        {
            label.BeginInvoke(new Action(() => { label.Text = textLabel; }));

            prog.BeginInvoke(new Action(() => { prog.Value = progress; }));

            Thread.Sleep(1);
        }

        /// <summary>
        /// Here we call our methods with the time-consuming tasks.
        /// </summary>
        private void backWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _eventDoWork = e;
            NewFile = Execute();
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

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e) =>
            btnCancelar_Click(sender, e);
    }
}