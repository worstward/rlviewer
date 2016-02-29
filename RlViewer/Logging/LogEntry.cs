using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Logging
{
    public class LogEntry
    {
        public LogEntry(DateTime eventTime, SeverityGrades severity, string message)
        {
            _eventTime = eventTime;
            _severity = severity;
            _message = message;
        }


        private DateTime _eventTime;

        public DateTime EventTime
        {
            get { return _eventTime; }
        }

        private SeverityGrades _severity;

        public SeverityGrades Severity
        {
            get { return _severity; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
        }


        public override string ToString()
        {
            return string.Format("{0}: {1} event happened: {2}", _eventTime, _severity.ToStringValue(), _message);
        }
    }




}
