using System.IO;

namespace ExcelMerge.Model
{
    public class FileMergeModel
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

        public FileMergeModel(string path)
        {
            FileName = path;
            Directory = path;
        }

        public string GetPath() => $"{_directory}\\{_fileName}";
    }
}
