using ClosedXML.Excel;
using ExcelTools.Core.Enumerator;
using ExcelTools.Core.EventArgument;
using ExcelTools.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelTools.Core
{
    public delegate void OnProgressChangedEventHandler(object sender, ProgressArgs e);

    public delegate void OnLogEventHandler(object sender, LogArgs e);

    public delegate void OnFinishedEventHandler(object sender, FinishedArgs e);

    public class ExcelMerge
    {
        private ParamsMergeModel[] _fileMerge;
        private int _indexFile;
        private int _indexSheet;
        private bool _copiedOnlyFirstHeader;
        private bool _cancel;
        private string _destinyDirectory;
        private HeaderActionEnum _headerAction;
        public event OnProgressChangedEventHandler OnProgress;
        public event OnLogEventHandler OnLog;
        public event OnFinishedEventHandler OnFinished;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="destinyDirectory">Directory for save the file generated</param>
        /// <param name="selectedHeaderAction">How to generate the header</param>
        public ExcelMerge()
        {
            
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ExcelMerge()
        {
            OnProgress = null;
            OnLog = null;
            OnFinished = null;
        }

        #region Private methods
        private string NewFileName()
            => $"{_destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

        private bool LoadFromWorksheet(IXLWorksheet worksheet, out IList<string[]> list)
        {
            list = new List<string[]>();
            if (LoadFromWorksheet(worksheet, out IXLRange ragenUsed))
            {
                var values = ragenUsed.RowsUsed()
                    .Select(s => s.Cell(1).Value.ToString().Split((char)_fileMerge[_indexFile].SeparatorCSV));

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

            switch (_headerAction)
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

        private int GetRowCount(IXLWorksheet worksheet)
        {
            var mainFirstTableCell = worksheet.FirstCellUsed();
            var mainLastTableCell = worksheet.LastCellUsed();

            return mainFirstTableCell == null || mainLastTableCell == null
                ? 1
                : worksheet.Range(mainFirstTableCell.Address, mainLastTableCell.Address).RowCount() + 1;
        }

        private void SaveFile(XLWorkbook workbook)
        {
            OnLog?.Invoke(this, new LogArgs(string.Empty, EventLog.BeforeSaveFile));
            var newFileName = NewFileName();
            workbook.Worksheet(1).Columns().AdjustToContents();
            workbook.SaveAs(newFileName);
            OnLog?.Invoke(this, new LogArgs(newFileName, EventLog.AfterSaveFile));
            OnFinished?.Invoke(this, new FinishedArgs(newFileName));
        }

        #endregion

        #region Public methods

        public void Cancel()
            => _cancel = true;

        public void Execute(ParamsMergeModel[] fileMerge, string destinyDirectory, HeaderActionEnum selectedHeaderAction)
        {
            if (fileMerge.Length <= 0)
                return;

            _destinyDirectory = destinyDirectory;
            _headerAction = selectedHeaderAction;
            _fileMerge = fileMerge;
            

            using (var mainWorkbook = new XLWorkbook(XLEventTracking.Disabled)) //new Workbook
            {
                var mainWorksheet = mainWorkbook.Worksheets.Add("Main");

                for (_indexFile = 0; _indexFile < _fileMerge.Length; _indexFile++) // Loop in files
                {
                    if (_cancel)
                        return;

                    using (var workBookFile = new XLWorkbook(_fileMerge[_indexFile].GetPath(), XLEventTracking.Disabled)) // Load from file
                    {

                        OnLog?.Invoke(this, new LogArgs(_fileMerge[_indexFile].FileName, EventLog.ReadFile));
                        OnProgress?.Invoke(this, new ProgressArgs(_indexFile + 1));

                        for (_indexSheet = 0; _indexSheet < workBookFile.Worksheets.Count; _indexSheet++) // Loop in sheets
                        {
                            if (_cancel)
                                return;

                            var worksheet = workBookFile.Worksheets.Worksheet(_indexSheet + 1);

                            OnLog?.Invoke(this, new LogArgs(worksheet.Name, EventLog.ReadSheet));

                            if (_fileMerge[_indexFile].SeparatorCSV == null)
                            {
                                if (LoadFromWorksheet(worksheet, out IXLRange ragenUsed))
                                    mainWorksheet.Cell($"A{GetRowCount(mainWorksheet)}").Value = ragenUsed;
                            }
                            else
                            {
                                if (LoadFromWorksheet(worksheet, out IList<string[]> list))
                                    mainWorksheet.Cell($"A{GetRowCount(mainWorksheet)}").Value = list;
                            }
                        }
                    }

                    SaveFile(mainWorkbook);
                }
            }
        }
        #endregion
    }
}