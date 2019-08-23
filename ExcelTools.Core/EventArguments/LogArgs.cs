using ExcelTools.Core.Enumerator;
using System;

namespace ExcelTools.Core.EventArgument
{
    public class LogArgs : EventArgs
    {
        public string ElementName;
        public EventLog ElementType;

        public LogArgs()
        {

        }

        public LogArgs(string elementName, EventLog elementType)
        {
            ElementName = elementName;
            ElementType = elementType;
        }
    }
}
