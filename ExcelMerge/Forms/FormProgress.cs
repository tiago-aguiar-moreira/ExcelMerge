using ClosedXML.Excel;
using ExcelMerge.Enumerator;
using ExcelMerge.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ExcelMerge.Forms
{
    public partial class FormProgress : Form
    {
        private int _rowReturnFileCount;
        private int _columnReturnFileCount;
        private int _headerLength;
        private int _numberHeaderColumns;
        private string _destinyDirectory;
        private string[] _filePath;
        private SelectedHeaderActionEnum _selectedHeaderAction;
        private DoWorkEventArgs _eventDoWork;
        private IXLWorkbook _mainWorkbook;
        private IXLWorksheet _mainWorksheet;
        private MergeProgess _progress;
        private const string _formatDateTime = "dd/MM/yyyy HH:mm:ss.fff";

        public string NewFile { get; private set; }

        public FormProgress(string[] filePath, string destinyDirectory, SelectedHeaderActionEnum selectedHeaderActionEnum, byte headerLength)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _filePath = filePath;
            _destinyDirectory = destinyDirectory;
            _selectedHeaderAction = selectedHeaderActionEnum;
            _headerLength = headerLength;
            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("Planilha 1");
            _rowReturnFileCount = 1;
            _numberHeaderColumns = 0;

            richTxt.Clear();
            progBarFile.Value = 0;
            progBarSheet.Value = 0;
            progBarRow.Value = 0;

            backWorker.RunWorkerAsync(); //executes the process asynchronously
        }

        private string NewFileName(string destinyDirectory) =>
            $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

        private string SaveFile(string destinyDirectory)
        {
            var newFileName = NewFileName(destinyDirectory);
            _mainWorkbook.SaveAs(newFileName);
            backWorker.ReportProgress(100);
            return newFileName;
        }

        private void SetTotals(string[] filePath)
        {
            _progress = new MergeProgess(filePath.Length);

            for (int indexFilePath = 0; indexFilePath < _progress.File.Length; indexFilePath++) // Loop in files
            {
                var sheets = new XLWorkbook(filePath[indexFilePath]).Worksheets; // Get sheets from file

                _progress.File[indexFilePath] = new MergeProgessFiles(sheets.Count)
                {
                    Sheet = new MergeProgessSheets[sheets.Count]
                };

                for (int indexSheet = 0; indexSheet < _progress.File[indexFilePath].Sheet.Length; indexSheet++) // Loop in sheets
                {
                    var lengthRow = sheets.Worksheet(indexSheet + 1).RowsUsed().Count();

                    _progress.File[indexFilePath].Sheet[indexSheet] = new MergeProgessSheets(lengthRow);
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

        private void UpdateProgress(MergeProgessFiles files, int progress, string name)
        {
            files.Progress = progress + 1;

            RefreshLabelAndProgressBar(
                lblFile,
                progBarFile,
                files.Progress,
                $"Arquivo {files.Progress} de {progBarFile.Maximum} ({name})");
        }

        private void UpdateProgress(MergeProgessSheets sheets, int progress, string name)
        {
            sheets.Progress = progress + 1;

            RefreshLabelAndProgressBar(
                lblSheet,
                progBarSheet,
                sheets.Progress,
                $"Planilha {sheets.Progress} de {progBarSheet.Maximum} ({name})");
        }

        private void UpdateProgress(MergeProgessRows rows, int progress)
        {
            rows.Progress = progress + 1;

            RefreshLabelAndProgressBar(
                lblRow,
                progBarRow,
                rows.Progress,
                $"Linha {rows.Progress} de {progBarRow.Maximum}");
        }

        private void UpdateLogReadFile(string filePath)
        {
            richTxt.BeginInvoke(new Action(() =>
            {
                var text = $"{DateTime.Now.ToString(_formatDateTime, CultureInfo.InvariantCulture)}{Environment.NewLine}";
                text += $"Arquivo: {filePath}{Environment.NewLine}";
                text += $"{Environment.NewLine}";

                richTxt.AppendText(text);
            }));
        }

        private void UpdateLogReadSheet(string sheetName, bool error)
        {
            UpdateLogReadSheet(sheetName, error, 0);
        }

        private void UpdateLogReadSheet(string sheetName, bool error, int totalRows)
        {
            richTxt.BeginInvoke(new Action(() =>
            {
                var text = $"{DateTime.Now.ToString(_formatDateTime, CultureInfo.InvariantCulture)}{Environment.NewLine}";
                if (totalRows > 0)
                {
                    text += $"Planilha: {sheetName} - Total de linhas: {totalRows}{Environment.NewLine}"; 
                }
                else
                {
                    text += $"Planilha: {sheetName} - Erro: Cabeçalho divergente do parametrizado{Environment.NewLine}";
                }
                
                text += $"{Environment.NewLine}";

                richTxt.SelectionStart = richTxt.TextLength;
                if (error)
                {
                    richTxt.SelectionLength = 0;
                    richTxt.SelectionColor = Color.Red;
                    richTxt.AppendText(text);
                    richTxt.SelectionColor = richTxt.ForeColor;
                }
                else
                {
                    richTxt.AppendText(text);
                }

                richTxt.ScrollToCaret();
            }));
        }

        public string Execute()
        {
            SetTotals(_filePath);
            SetMaximumProgressBar(progBarFile, _progress.File.Length);

            for (int indexFile = 0; indexFile < _progress.File.Length; indexFile++) // Loop in files
            {
                if (CancellationPending(_eventDoWork))
                    return string.Empty;

                backWorker.ReportProgress(indexFile);

                var fileName = Path.GetFileName(_filePath[indexFile]);

                UpdateLogReadFile(fileName);
                UpdateProgress(_progress.File[indexFile], indexFile, fileName);

                var sheets = new XLWorkbook(_filePath[indexFile]).Worksheets; // Get sheets from file

                SetMaximumProgressBar(progBarSheet, _progress.File[indexFile].Sheet.Length);
                for (int indexSheet = 0; indexSheet < _progress.File[indexFile].Sheet.Length; indexSheet++) // Loop in sheets
                {
                    if (CancellationPending(_eventDoWork))
                        return string.Empty;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    UpdateLogReadSheet(sheet.Name, false, _progress.File[indexFile].Sheet[indexSheet].Rows.Total);
                    UpdateProgress(_progress.File[indexFile].Sheet[indexSheet], indexSheet, sheet.Name);

                    SetMaximumProgressBar(progBarRow, _progress.File[indexFile].Sheet[indexSheet].Rows.Total);

                    if (_headerLength > _progress.File[indexFile].Sheet[indexSheet].Rows.Total)
                    {
                        var filePath = _filePath[indexFile];

                        UpdateLogReadSheet(sheet.Name, true);
                        continue;
                    }

                    var initialIndex = _headerLength - 1;
                    for (int indexRow = initialIndex; indexRow < _progress.File[indexFile].Sheet[indexSheet].Rows.Total; indexRow++) // Loop in rows
                    {
                        if (CancellationPending(_eventDoWork))
                            return string.Empty;

                        UpdateProgress(_progress.File[indexFile].Sheet[indexSheet].Rows, indexRow);

                        var row = sheet.Row(indexRow + 1);

                        #region Validate number of header columns
                        /* If the number of columns in the worksheet to be imported is greater than the 
                        number of columns in the first worksheet, the worksheet is discarded. */

                        if (_numberHeaderColumns == 0)
                        {
                            _numberHeaderColumns = row.RowUsed().CellCount();
                        }

                        if (_numberHeaderColumns != row.RowUsed().CellCount())
                        {
                            break;
                        }
                        #endregion

                        _columnReturnFileCount = 1;

                        switch (_selectedHeaderAction)
                        {
                            case SelectedHeaderActionEnum.ConsiderFirstFile:
                                if (indexFile == 0 && indexSheet == 0 && indexRow == initialIndex)
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    _rowReturnFileCount += 1;
                                }
                                else if (indexRow != initialIndex)
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    _rowReturnFileCount += 1;
                                }
                                break;
                            case SelectedHeaderActionEnum.IgnoreAll: // * Ignore all headers!!!
                                if (indexRow == initialIndex)
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

            btnCancelar.Enabled = false;
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