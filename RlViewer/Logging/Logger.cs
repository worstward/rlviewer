using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Logging
{

    //TODO: logs filtering based on severity e
    public static class Logger
    {
        private static string logFileName = "log.txt";

        private static List<LogEntry> _logs = new List<LogEntry>();
        public static List<LogEntry> Logs
        {
            get
            {
                return _logs;
            }
        }

        public static void Log(SeverityGrades severity, string message)
        {
            var logEntry = new LogEntry(DateTime.Now, severity, message);

            _logs.Add(logEntry);
            SaveEntry(logEntry);
        }

        private static object _saveLocker = new object();
        private static void SaveEntry(LogEntry entry)
        {
            lock (_saveLocker)
            {
                using (var stream = new StreamWriter(logFileName, true))
                {
                    stream.WriteLine(entry.ToString());
                }
            }
        }


    }
}
