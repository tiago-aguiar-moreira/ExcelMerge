using ClosedXML.Excel;
using ExcelMerge.Enumerator;
using ExcelMerge.Model;
using ExcelMerge.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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
        private IXLWorksheet _mainWorksheet;
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
            lblFile.BeginInvoke(new Action(() => 
            {
                lblFile.Text = $"Arquivo {progBarFile.Value} de {progBarFile.Maximum} ({_fileMerge[_indexFile].FileName})";
            }));

            progBarFile.BeginInvoke(new Action(() => { progBarFile.Value += _indexFile + 1; }));

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

        //private bool GetHeader()
        //{
        //    switch (_selectedHeaderAction)
        //    {
        //        case HeaderActionEnum.None:
        //            break;
        //        case HeaderActionEnum.ConsiderFirstFile:
        //            break;
        //        case HeaderActionEnum.IgnoreAll:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private bool GetRangeDataFrom(IXLWorksheet workSheet, out IXLRange range)
        {
            var numberRow = _fileMerge[_indexFile].HeaderLength;
            var initialCell = workSheet.Cell($"A{(numberRow == 0 ? 1 : numberRow)}").Address;
            var finalCell = workSheet.LastCellUsed().Address;

            range = workSheet.Range(initialCell, finalCell);

            return workSheet.RangeUsed().RowCount() >= numberRow;
        }

        public string Execute()
        {
            if (_fileMerge.Length <= 0)
                return string.Empty;

            SetMaximumProgressBar();

            using (var mainWorkbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                _mainWorksheet = mainWorkbook.Worksheets.Add("Main");

                for (_indexFile = 0; _indexFile < _fileMerge.Length; _indexFile++) // Loop in files
                {
                    using (var workBookFile = new XLWorkbook(_fileMerge[_indexFile].GetPath(), XLEventTracking.Disabled))
                    {
                        if (CancellationPending(_eventDoWork))
                            return string.Empty;

                        backWorker.ReportProgress(_indexFile);

                        UpdateLogReadFile();
                        UpdateProgressFile();

                        for (int indexSheet = 0; indexSheet < mainWorkbook.Worksheets.Count; indexSheet++) // Loop in sheets
                        {
                            if (CancellationPending(_eventDoWork))
                                return string.Empty;

                            var workSheet = workBookFile.Worksheets.Worksheet(indexSheet + 1);

                            UpdateLogReadSheet(workSheet.Name, false);

                            // Encapsular (204 a 210) em um método cujo retorno será passado por parâmetrono método cell (210)
                            var mainFirstTableCell = _mainWorksheet.FirstCellUsed();
                            var mainLastTableCell = _mainWorksheet.LastCellUsed();

                            var mainRowCount = mainFirstTableCell == null || mainLastTableCell == null 
                                ? 1
                                : _mainWorksheet.Range(mainFirstTableCell.Address, mainLastTableCell.Address).RowCount() + 1;

                            IXLRange ragenUsed;
                            if (GetRangeDataFrom(workSheet, out ragenUsed))
                                _mainWorksheet.Cell($"A{mainRowCount}").Value = ragenUsed;

                            //for (int indexRow = GetInitialIndex(); indexRow < sheet.RowsUsed().Count(); indexRow++) // Loop in rows
                            //{
                            //    RefreshCurrentRow(lblProcessedRows, indexRow);

                            //    if (CancellationPending(_eventDoWork))
                            //        return string.Empty;

                            //    var row = GetRow(sheet.Row(indexRow + 1), _fileMerge[_indexFile].SeparatorCSV);

                            //    switch (_selectedHeaderAction)
                            //    {
                            //        case HeaderActionEnum.ConsiderFirstFile:
                            //            if ((_indexFile == 0 && indexSheet == 0 && indexRow == GetInitialIndex()) || (indexRow != GetInitialIndex()))
                            //            {
                            //                if (!AddNewRow(row))
                            //                    return string.Empty;

                            //                SetIncrementRowFileCount();
                            //            }
                            //            break;
                            //        case HeaderActionEnum.IgnoreAll: // * Ignore all headers!!!
                            //            if (indexRow == GetInitialIndex())
                            //                continue;

                            //            if (!AddNewRow(row))
                            //                return string.Empty;

                            //            SetIncrementRowFileCount();
                            //            break;
                            //        case HeaderActionEnum.None:
                            //            if (!AddNewRow(row))
                            //                return string.Empty;

                            //            SetIncrementRowFileCount();
                            //            break;
                            //    }
                            //}
                        }
                    }
                }

                #region Save File

                backWorker.ReportProgress(100);

                var newFileName = NewFileName(_destinyDirectory);
                mainWorkbook.SaveAs(newFileName);
                mainWorkbook.Dispose();
                return newFileName;

                #endregion
            }
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