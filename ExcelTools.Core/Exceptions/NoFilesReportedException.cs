using System;

namespace ExcelTools.Core.Exceptions
{
    public class NoFilesReportedException : Exception
    {
        public NoFilesReportedException()
        {

        }

        public NoFilesReportedException(string message) 
            : base(message)
        {

        }

        public NoFilesReportedException(string message, Exception inner) 
            : base(message, inner)
        {

        }
    }
}
