﻿using ClosedXML.Excel;
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
        //private int _numberHeaderColumns;
        private string _destinyDirectory;
        private FileMerge[] _fileMerge;
        private MergeProgessFiles[] _progressFile;
        private SelectedHeaderActionEnum _selectedHeaderAction;
        private DoWorkEventArgs _eventDoWork;
        private IXLWorkbook _mainWorkbook;
        private IXLWorksheet _mainWorksheet;
        private const string _formatDateTime = "dd/MM/yyyy HH:mm:ss.fff";

        public string NewFile { get; private set; }

        public FormProgress(FileMerge[] fileMerge, string destinyDirectory, SelectedHeaderActionEnum selectedHeaderActionEnum, byte headerLength)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _fileMerge = fileMerge;
            _destinyDirectory = destinyDirectory;
            _selectedHeaderAction = selectedHeaderActionEnum;
            _headerLength = headerLength;
            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("Planilha 1");
            _rowReturnFileCount = 1;

            richTxt.Clear();
            progBarFile.Value = 0;
            progBarSheet.Value = 0;

            backWorker.RunWorkerAsync(); //executes the process asynchronously
        }

        private string NewFileName(string destinyDirectory)
            => $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

        private string SaveFile(string destinyDirectory)
        {
            var newFileName = NewFileName(destinyDirectory);
            _mainWorkbook.SaveAs(newFileName);
            backWorker.ReportProgress(100);
            return newFileName;
        }

        private void SetTotals(FileMerge[] filePath)
        {
            _progressFile = new MergeProgessFiles[filePath.Length];

            for (int index = 0; index < _progressFile.Length; index++) // Loop in files
            {
                var sheets = new XLWorkbook(filePath[index].Path).Worksheets; // Get sheets from file

                _progressFile[index] = new MergeProgessFiles(sheets.Count);
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
            richTxt.BeginInvoke(new Action(() =>
            {
                var text = $"{DateTime.Now.ToString(_formatDateTime, CultureInfo.InvariantCulture)}{Environment.NewLine}";
                
                text += $"Planilha: {sheetName}";
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

        private IXLRow GetRow(IXLRow row, string separator)
        {
            var valueRow = row.Cell(1).Value.ToString();

            if (!string.IsNullOrEmpty(separator))
            {
                var values = valueRow.Split(separator.ToArray());

                row.Clear();
                for (int cellNumber = 0; cellNumber < values.Length; cellNumber++)
                {
                    row.Cell(cellNumber + 1).Value = values[cellNumber];
                }
            }

            return row;
        }

        /// <summary>
        /// Get sheets from file path
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns></returns>
        private IXLWorksheets GetWorksheets(string path) => new XLWorkbook(path).Worksheets; // 

        private int GetInitialIndex() => _headerLength - 1;

        private void SetIncrementRowFileCount() => _rowReturnFileCount += 1;
        public string Execute()
        {
            SetTotals(_fileMerge);

            if (_progressFile.Length <= 0)
                return string.Empty;

            SetMaximumProgressBar(progBarFile, _progressFile.Length);

            for (int indexFile = 0; indexFile < _progressFile.Length; indexFile++) // Loop in files
            {
                if (CancellationPending(_eventDoWork))
                    return string.Empty;

                backWorker.ReportProgress(indexFile);

                var fileName = Path.GetFileName(_fileMerge[indexFile].Path);
                //_fileCSV = Path.GetExtension(_filePath[indexFile]) == ".csv";

                UpdateLogReadFile(fileName);
                UpdateProgress(_progressFile[indexFile], indexFile, fileName);

                var sheets = GetWorksheets(_fileMerge[indexFile].Path);
                
                SetMaximumProgressBar(progBarSheet, _progressFile[indexFile].Sheet.Total);

                for (int indexSheet = 0; indexSheet < _progressFile[indexFile].Sheet.Total; indexSheet++) // Loop in sheets
                {
                    if (CancellationPending(_eventDoWork))
                        return string.Empty;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    UpdateLogReadSheet(sheet.Name, false);
                    UpdateProgress(_progressFile[indexFile].Sheet, indexSheet, sheet.Name);

                    for (int indexRow = GetInitialIndex(); indexRow < sheet.RowsUsed().Count(); indexRow++) // Loop in rows
                    {
                        if (CancellationPending(_eventDoWork))
                            return string.Empty;

                        var row = GetRow(sheet.Row(indexRow + 1), _fileMerge[indexFile].Separator);

                        _columnReturnFileCount = 1;

                        switch (_selectedHeaderAction)
                        {
                            case SelectedHeaderActionEnum.ConsiderFirstFile:
                                if (indexFile == 0 && indexSheet == 0 && indexRow == GetInitialIndex())
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    SetIncrementRowFileCount();
                                }
                                else if (indexRow != GetInitialIndex())
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    SetIncrementRowFileCount();
                                }
                                break;
                            case SelectedHeaderActionEnum.IgnoreAll: // * Ignore all headers!!!
                                if (indexRow == GetInitialIndex())
                                {
                                    continue;
                                }
                                else
                                {
                                    if (!AddNewRow(row.RowUsed().CellCount(), row))
                                        return string.Empty;

                                    SetIncrementRowFileCount();
                                    break;
                                }
                            case SelectedHeaderActionEnum.None:
                                if (!AddNewRow(row.RowUsed().CellCount(), row))
                                    return string.Empty;

                                SetIncrementRowFileCount();
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

    public class FileMerge
    {
        public string Path { get; set; }
        public string Separator { get; set; }

        public FileMerge(string name)
        {
            Path = name;
            Separator = string.Empty;
        }

    }
}