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
            Info,
            Internal
        }

        public static class SeverityGradesExt
        {
            public static string ToStringValue(this SeverityGrades severity)
            {
                string severityString = null;

                try
                {
                    severityString = Enum.GetName(typeof(SeverityGrades), severity);
                }
                catch(Exception)
                {
                    severityString = "Unknown";
                }

                return severityString;
            }
        }


}
