using ClosedXML.Excel;
using ExcelMerge.Enumerator;
using ExcelMerge.Model;
using ExcelMerge.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ExcelMerge.Forms
{
    public partial class FormProgress : Form
    {
        private const string _formatDateTime = "dd/MM/yyyy HH:mm:ss.fff";
        private readonly string _destinyDirectory;
        private readonly FileMergeModel[] _fileMerge;
        private readonly HeaderActionEnum _selectedHeaderAction;
        private int _indexFile;
        private int _indexSheet;
        private bool _copiedOnlyFirstHeader;
        private DoWorkEventArgs _eventDoWork;

        public string NewFile { get; private set; }

        public FormProgress(FileMergeModel[] fileMerge, string destinyDirectory, HeaderActionEnum selectedHeaderActionEnum)
        {
            InitializeComponent();
            this.SetBaseConfigs();

            _fileMerge = fileMerge;
            _destinyDirectory = destinyDirectory;
            _selectedHeaderAction = selectedHeaderActionEnum;

            richTxt.Clear();
            progBarFile.Value = 0;

            backWorker.RunWorkerAsync(); //executes the process asynchronously
        }

        private string NewFileName(string destinyDirectory)
            => $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

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

        private void UpdateProgressFile()
        {
            progBarFile.BeginInvoke(new Action(() => { progBarFile.Value += +1; }));

            lblFile.BeginInvoke(new Action(() =>
            {
                lblFile.Text = $"Arquivo {progBarFile.Value} de {progBarFile.Maximum} ({_fileMerge[_indexFile].FileName})";
            }));

            Thread.Sleep(1);
        }

        private void UpdateLogReadFile()
        {
            richTxt.BeginInvoke(new Action(() =>
            {
                var text = $"{DateTime.Now.ToString(_formatDateTime, CultureInfo.InvariantCulture)}{Environment.NewLine}";
                text += $"Arquivo: {_fileMerge[_indexFile].FileName}{Environment.NewLine}";
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

        private bool LoadFromWorksheet(IXLWorksheet worksheet, out IList<string[]> list, char separatorCSV)
        {
            list = new List<string[]>();
            if (LoadFromWorksheet(worksheet, out IXLRange ragenUsed))
            {
                var values = ragenUsed.RowsUsed()
                    .Select(s => s.Cell(1).Value.ToString().Split(separatorCSV));

                foreach (var value in values)
                {
                    list.Add(value);
                }

                return true;
            }
            
            return false;
        }

        private bool LoadFromWorksheet(IXLWorksheet worksheet, out IXLRange range)
        {
            var numberRow = _fileMerge[_indexFile].HeaderLength + (_fileMerge[_indexFile].HeaderLength == 0 ? 1 : 0);
            var finalCell = worksheet.LastCellUsed().Address;
            var cellAdressInitial = string.Empty;

            switch (_selectedHeaderAction)
            {
                case HeaderActionEnum.ConsiderFirstFile:
                    if (_copiedOnlyFirstHeader)
                    {
                        cellAdressInitial = $"A{numberRow + 1}";
                    }
                    else
                    {
                        if (_indexFile == 0 && _indexSheet == 0)
                        {
                            cellAdressInitial = $"A{numberRow}";
                            _copiedOnlyFirstHeader = true;
                        }
                    }
                    break;
                case HeaderActionEnum.IgnoreAll:
                    cellAdressInitial = $"A{numberRow + 1}";
                    break;
                case HeaderActionEnum.None:
                default:
                    cellAdressInitial = "A1";
                    break;
            }

            range = worksheet.Range(worksheet.Cell(cellAdressInitial).Address, finalCell);
                        
            return worksheet.RangeUsed().RowCount() >= numberRow;
        }

        public string Execute()
        {
            if (_fileMerge.Length <= 0)
                return string.Empty;

            SetMaximumProgressBar();

            using (var mainWorkbook = new XLWorkbook(XLEventTracking.Disabled)) //new Workbook
            {
                var mainWorksheet = mainWorkbook.Worksheets.Add("Main");

                for (_indexFile = 0; _indexFile < _fileMerge.Length; _indexFile++) // Loop in files
                {
                    using (var workBookFile = new XLWorkbook(_fileMerge[_indexFile].GetPath(), XLEventTracking.Disabled)) // Load from file
                    {
                        if (CancellationPending(_eventDoWork))
                            return string.Empty;

                        backWorker.ReportProgress(_indexFile);

                        UpdateLogReadFile();
                        UpdateProgressFile();

                        for (_indexSheet = 0; _indexSheet < workBookFile.Worksheets.Count; _indexSheet++) // Loop in sheets
                        {
                            if (CancellationPending(_eventDoWork))
                                return string.Empty;

                            var worksheet = workBookFile.Worksheets.Worksheet(_indexSheet + 1);

                            UpdateLogReadSheet(worksheet.Name, false);

                            if (_fileMerge[_indexFile].SeparatorCSV == null)
                            {
                                if (LoadFromWorksheet(worksheet, out IXLRange ragenUsed))
                                    mainWorksheet.Cell($"A{GetRowCount(mainWorksheet)}").Value = ragenUsed;
                            }
                            else
                            {
                                if (LoadFromWorksheet(worksheet, out IList<string[]> list, (char)_fileMerge[_indexFile].SeparatorCSV))
                                    mainWorksheet.Cell($"A{GetRowCount(mainWorksheet)}").Value = list;
                            }
                        }
                    }
                }

                #region Save File

                backWorker.ReportProgress(100);

                var newFileName = NewFileName(_destinyDirectory);
                mainWorkbook.Worksheet(1).Columns().AdjustToContents();
                mainWorkbook.SaveAs(newFileName);
                return newFileName;

                #endregion
            }
        }

        private int GetRowCount(IXLWorksheet worksheet)
        {
            var mainFirstTableCell = worksheet.FirstCellUsed();
            var mainLastTableCell = worksheet.LastCellUsed();

            return mainFirstTableCell == null || mainLastTableCell == null
                ? 1
                : worksheet.Range(mainFirstTableCell.Address, mainLastTableCell.Address).RowCount() + 1;
        }

        private void SetMaximumProgressBar()
        {
            progBarFile.BeginInvoke(new Action(() =>
            {
                progBarFile.Maximum = _fileMerge.Length;
            }));

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

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
            => btnCancelar_Click(sender, e);
    }
}