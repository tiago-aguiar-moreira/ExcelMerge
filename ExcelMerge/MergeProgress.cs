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
}