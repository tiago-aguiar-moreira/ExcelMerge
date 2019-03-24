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
            Total = lengthRows;
        }
    }

    public class MergeProgessSheets
    {
        public int Progress { get; set; }
        public MergeProgessRows Rows { get; set; }

        public MergeProgessSheets(int lengthRows)
        {
            Rows = new MergeProgessRows(lengthRows);
        }
    }

    public class MergeProgessFiles
    {
        public int Progress { get; set; }
        public MergeProgessSheets[] Sheets { get; set; }

        public MergeProgessFiles(int lengthSheets)
        {
            Sheets = new MergeProgessSheets[lengthSheets];
        }
    }
    
    public class MergeProgess
    {
        public MergeProgessFiles[] Files { get; set; }

        public MergeProgess(int lengthFiles)
        {
            Files = new MergeProgessFiles[lengthFiles];
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

            for (int indexFilePath = 0; indexFilePath < Progress.Files.Length; indexFilePath++) // Loop in files
            {
                var sheets = new XLWorkbook(filePath[indexFilePath]).Worksheets; // Get sheets from file

                Progress.Files[indexFilePath] = new MergeProgessFiles(sheets.Count)
                {
                    Sheets = new MergeProgessSheets[sheets.Count]
                };

                for (int indexSheet = 0; indexSheet < Progress.Files[indexFilePath].Sheets.Length; indexSheet++) // Loop in sheets
                {
                    var lengthRow = sheets.Worksheet(indexSheet + 1).RowsUsed().Count();                    

                    Progress.Files[indexFilePath].Sheets[indexSheet] = new MergeProgessSheets(lengthRow);
                }
            }
        }

        public string Execute(string[] filePath, string destinyDirectory)
        {
            SetTotals(filePath);

            int _rowReturnFileCount = 1;

            for (int indexFile = 0; indexFile < Progress.Files.Length; indexFile++) // Loop in files
            {
                var sheets = new XLWorkbook(filePath[indexFile]).Worksheets; // Get sheets from file

                //Progress.Files.Progress = indexFile + 1;

                for (int indexSheet = 0; indexSheet < Progress.Files[indexFile].Sheets.Length; indexSheet++) // Loop in sheets
                {
                    //Progress.Files.Sheets.Progress = indexSheet;

                    var sheet = sheets.Worksheet(indexSheet + 1);

                    for (int indexRow = 0; indexRow < Progress.Files[indexFile].Sheets[indexSheet].Rows.Total; indexRow++) // Loop in rows
                    {
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