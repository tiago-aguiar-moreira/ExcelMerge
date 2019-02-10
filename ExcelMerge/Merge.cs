using ClosedXML.Excel;
using System;
using System.IO;

namespace ExcelMerge
{
    public class Merge
    {
        private IXLWorkbook _mainWorkbook;
        private IXLWorksheet _mainWorksheet;

        public Merge()
        {
            _mainWorkbook = new XLWorkbook();
            _mainWorksheet = _mainWorkbook.Worksheets.Add("First");
        }

        private IXLWorksheets Load(string path) => File.Exists(path) ? new XLWorkbook(path).Worksheets : null;

        private string NewFileName(string destinyDirectory) => $"{destinyDirectory}\\ExcelMerge_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

        private string SaveFile(string destinyDirectory)
        {
            var newFileName = NewFileName(destinyDirectory);
            _mainWorkbook.SaveAs(newFileName);
            return newFileName;
        }

        public string Execute(string[] filePath, string destinyDirectory)
        {
            int _rowCount = 1;

            foreach (string path in filePath) // Loop in files
            {
                var sheets = Load(path); // Get sheets from file
                
                foreach (IXLWorksheet sheet in sheets) // Loop in sheets
                {
                    foreach (var row in sheet.RowsUsed()) // Loop in rows
                    {
                        int _columnCount = 1;

                        foreach (var cell in row.Cells()) // Loop in cells
                        {
                            _mainWorksheet.Cell(_rowCount, _columnCount).Value = cell.Value.ToString();
                            
                            _columnCount += 1;
                        }

                        _rowCount += 1;
                    }
                }
            }

            return SaveFile(destinyDirectory);
        }
    }
}