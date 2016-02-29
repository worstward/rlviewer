using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Logging
{

        public enum SeverityGrades
        {
            Blocking = 1,
            Error,
            Warning,
            Info
        }

        public static class SeverityGradesExt
        {
            public static string ToStringValue(this SeverityGrades severity)
            {
                string severityString;
                switch (severity)
                {
                    case SeverityGrades.Blocking:
                        severityString = "Blocking";
                        break;
                    case SeverityGrades.Error:
                        severityString = "Error";
                        break;
                    case SeverityGrades.Info:
                        severityString = "Info";
                        break;
                    case SeverityGrades.Warning:
                        severityString = "Warning";
                        break;
                    default:
                        severityString = "Unknown";
                        break;
                }

                return severityString;
            }
        }


}
