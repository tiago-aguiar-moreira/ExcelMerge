namespace ExcelMerge.Model
{
    public class MergeProgessSheetsModel
    {
        public int Progress { get; set; }
        public int Total { get; set; }
        public MergeProgessSheetsModel(int length)
        {
            Progress = 0;
            Total = length;
        }
    }   
}