using System;

namespace ExcelTools.Core.EventArgument
{
    public class ProgressArgs : EventArgs
    {
        public int Value;

        public ProgressArgs()
        {

        }

        public ProgressArgs(int progress)
            => Value = progress;
    }
}
