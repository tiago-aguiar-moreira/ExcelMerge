namespace ExcelMerge.Model
{
    public class MergeProgessFilesModel
    {
        public int Progress { get; set; }

        public MergeProgessSheetsModel Sheet { get; set; }

        public MergeProgessFilesModel(int lengthSheets)
        {
            Progress = 0;
            Sheet = new MergeProgessSheetsModel(lengthSheets);
        }
    }
}
