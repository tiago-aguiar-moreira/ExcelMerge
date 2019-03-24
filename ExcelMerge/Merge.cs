using ClosedXML.Excel;
using System;
using System.IO;
using System.Linq;

namespace ExcelMerge
{
    public class MergeProgessRows
    {
        public int Progress { get; set; }
        public int Total { get; set; }

        public MergeProgessRows(int lengthRows)
        {
            Progress = 0;
            Total = lengthRows;
        }
    }

    public class MergeProgessSheets
    {
        public int Progress { get; set; }
        public MergeProgessRows Rows { get; set; }

        public MergeProgessSheets(int lengthRows)
        {
            Progress = 0;
            Rows = new MergeProgessRows(lengthRows);
        }
    }

    public class MergeProgessFiles
    {
        public int Progress { get; set; }
        public MergeProgessSheets[] Sheet { get; set; }

        public MergeProgessFiles(int lengthSheets)
        {
            Progress = 0;
            Sheet = new MergeProgessSheets[lengthSheets];
        }
    }
    
    public class MergeProgess
    {
        public MergeProgessFiles[] File { get; set; }

        public MergeProgess(int lengthFiles)
        {
            File = new MergeProgessFiles[lengthFiles];
        }
    }

    public class Merge
    {
        private IXLWorkbook _mainWorkbook;
        private IXLWorksheet _mainWorksheet;
        private MergeProgess Progress { get; set; }

        public Merge()
        {
            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("First");
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

        public string Execute(string[] filePath, string destinyDirectory)
        {
            SetTotals(filePath);

            int _rowReturnFileCount = 1;

            for (int indexFile = 0; indexFile < Progress.File.Length; indexFile++) // Loop in files
            {
                var sheets = new XLWorkbook(filePath[indexFile]).Worksheets; // Get sheets from file

                Progress.File[indexFile].Progress = indexFile + 1;

                for (int indexSheet = 0; indexSheet < Progress.File[indexFile].Sheet.Length; indexSheet++) // Loop in sheets
                {
                    Progress.File[indexFile].Sheet[indexSheet].Progress = indexSheet + 1;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    for (int indexRow = 0; indexRow < Progress.File[indexFile].Sheet[indexSheet].Rows.Total; indexRow++) // Loop in rows
                    {
                        Progress.File[indexFile].Sheet[indexSheet].Rows.Progress = indexRow + 1;

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

            return SaveFile(destinyDirectory);
        }
    }
}