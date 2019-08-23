using System;

namespace ExcelTools.Core.EventArgument
{
    public class FinishedArgs : EventArgs
    {
        public string FileName;

        public FinishedArgs()
        {

        }

        public FinishedArgs(string filePath)
            => FileName = filePath;
    }
}
