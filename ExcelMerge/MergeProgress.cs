namespace ExcelMerge
{
    public class MergeProgessSheets
    {
        public int Progress { get; set; }

        public int Total { get; set; }

        public MergeProgessSheets(int length)
        {
            Progress = 0;
            Total = length;
        }
    }

    public class MergeProgessFiles
    {
        public int Progress { get; set; }

        public MergeProgessSheets Sheet { get; set; }

        public MergeProgessFiles(int lengthSheets)
        {
            Progress = 0;
            Sheet = new MergeProgessSheets(lengthSheets);
        }
    }
}