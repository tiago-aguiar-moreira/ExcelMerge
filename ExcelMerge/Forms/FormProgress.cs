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
        private const string _formatDateTime = "dd/MM/yyyy HH:mm:ss.fff";
        private int _rowReturnFileCount;
        private readonly string _destinyDirectory;
        private readonly FileMerge[] _fileMerge;
        private readonly HeaderActionEnum _selectedHeaderAction;
        private readonly IXLWorkbook _mainWorkbook;
        private readonly IXLWorksheet _mainWorksheet;
        private MergeProgessFiles[] _progressFile;
        private DoWorkEventArgs _eventDoWork;

        public string NewFile { get; private set; }

        public FormProgress(FileMerge[] fileMerge, string destinyDirectory, HeaderActionEnum selectedHeaderActionEnum)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _fileMerge = fileMerge;
            _destinyDirectory = destinyDirectory;
            _selectedHeaderAction = selectedHeaderActionEnum;
            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("Planilha 1");
            _rowReturnFileCount = 1;

            richTxt.Clear();
            progBarFile.Value = 0;
            progBarSheet.Value = 0;

            backWorker.RunWorkerAsync(); //executes the process asynchronously
        }

        private string NewFileName(string destinyDirectory) => $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

        private string SaveFile(string destinyDirectory)
        {
            var newFileName = NewFileName(destinyDirectory);
            _mainWorkbook.SaveAs(newFileName);
            backWorker.ReportProgress(100);
            return newFileName;
        }

        private void SetTotals(FileMerge[] file)
        {
            _progressFile = new MergeProgessFiles[file.Length];

            for (int index = 0; index < _progressFile.Length; index++) // Loop in files
            {
                var sheets = new XLWorkbook(file[index].GetPath()).Worksheets; // Get sheets from file

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
        private IXLWorksheets GetWorksheets(FileMerge fileMerge) => new XLWorkbook(fileMerge.GetPath()).Worksheets;

        private int GetInitialIndex(int indexFile) => _fileMerge[indexFile].HeaderLength - 1;

        private void SetIncrementRowFileCount() => _rowReturnFileCount += 1;

        public int GetTotalSheets(int index) => _progressFile[index].Sheet.Total;

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

                UpdateLogReadFile(_fileMerge[indexFile].FileName);
                UpdateProgress(_progressFile[indexFile], indexFile, _fileMerge[indexFile].FileName);

                var sheets = GetWorksheets(_fileMerge[indexFile]);
                
                SetMaximumProgressBar(progBarSheet, GetTotalSheets(indexFile));

                for (int indexSheet = 0; indexSheet < GetTotalSheets(indexFile); indexSheet++) // Loop in sheets
                {
                    if (CancellationPending(_eventDoWork))
                        return string.Empty;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    UpdateLogReadSheet(sheet.Name, false);
                    UpdateProgress(_progressFile[indexFile].Sheet, indexSheet, sheet.Name);

                    for (int indexRow = GetInitialIndex(indexFile); indexRow < sheet.RowsUsed().Count(); indexRow++) // Loop in rows
                    {
                        if (CancellationPending(_eventDoWork))
                            return string.Empty;

                        var row = GetRow(sheet.Row(indexRow + 1), _fileMerge[indexFile].SeparatorCSV);

                        switch (_selectedHeaderAction)
                        {
                            case HeaderActionEnum.ConsiderFirstFile:
                                if ((indexFile == 0 && indexSheet == 0 && indexRow == GetInitialIndex(indexFile)) || (indexRow != GetInitialIndex(indexFile)))
                                {
                                    if (!AddNewRow(row))
                                        return string.Empty;

                                    SetIncrementRowFileCount();
                                }
                                break;
                            case HeaderActionEnum.IgnoreAll: // * Ignore all headers!!!
                                if (indexRow == GetInitialIndex(indexFile))
                                    continue;

                                if (!AddNewRow(row))
                                    return string.Empty;

                                SetIncrementRowFileCount();
                                break;
                            case HeaderActionEnum.None:
                                if (!AddNewRow(row))
                                    return string.Empty;

                                SetIncrementRowFileCount();
                                break;
                        }
                    }
                }
            }

            return SaveFile(_destinyDirectory);
        }

        private bool AddNewRow(IXLRow row)
        {
            int columnReturnFileCount = 1;
            for (int indexCell = 0; indexCell < row.RowUsed().CellCount(); indexCell++) // Loop in cells
            {
                if (CancellationPending(_eventDoWork))
                    return false;

                _mainWorksheet.Cell(_rowReturnFileCount, columnReturnFileCount).Value = row.Cell(indexCell + 1).Value.ToString();

                columnReturnFileCount += 1;
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

            btnCancelar.BeginInvoke(new Action(() => { btnCancelar.Enabled = false; }));
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

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e) => btnCancelar_Click(sender, e);
    }

    public class FileMerge
    {
        private string _fileName;
        private string _directory;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = Path.GetFileName(value);
            }
        }
        public string Directory
        {
            get
            {
                return _directory;
            }
            set
            {
                _directory = Path.GetDirectoryName(value);
            }
        }
        public byte HeaderLength { get; set; }
        public string SeparatorCSV { get; set; }

        public FileMerge(string path)
        {
            FileName = path;
            Directory = path;
        }

        public string GetPath() => $"{_directory}\\{_fileName}";
    }
}