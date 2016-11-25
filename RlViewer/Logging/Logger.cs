using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace RlViewer.Logging
{
    public static class Logger
    {
        private static string _logFileName = "log.txt";

        private static string GetLogName(string fileName, LogType type)
        {
            string logName = string.Empty;

            switch (type)
            {
                case LogType.Common:
                    logName = "common_" + fileName;
                    break;
                case LogType.Synthesis:
                    logName = "synthesis_" + fileName;
                    break;
                default:
                    throw new ArgumentException("LogType");
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logName);
        }


        private static BlockingCollection<LogEntry> _logs = new BlockingCollection<LogEntry>();
        public static BlockingCollection<LogEntry> Logs
        {
            get
            {
                return _logs;
            }
        }

        public static void Log(SeverityGrades severity, string message, bool saveEntry = true, LogType type = LogType.Common)
        {
            var logEntry = new LogEntry(DateTime.Now, severity, message);

            Logs.Add(logEntry);
            LogEntryAdded(null, null);

            if (saveEntry)
            {
                SaveEntry(logEntry, type);
            }
        }

        public static event EventHandler LogEntryAdded = delegate { };

        private static object loggingLocker = new object();
        private static void SaveEntry(LogEntry entry, LogType type)
        {


            StreamWriter stream = null;
            try
            {
                stream = new StreamWriter(File.Open(GetLogName(_logFileName, type), FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                stream.WriteLine(entry.ToString());
            }
            catch
            {
                return;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }

        }

    }
}
