using System;
using System.Collections.Generic;
using System.Text;

namespace Coderr.Client.Contracts
{
    /// <summary>
    /// A log entry
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// When this log entry was written
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 0 = trace, 1 = debug, 2 = info, 3 = warning, 4 = error, 5 = critical
        /// </summary>
        public int LogLevel { get; set; }

        /// <summary>
        /// Logged message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Exception as string (if any was attached to this log entry)
        /// </summary>
        public string Exception { get; set; }
    }
}
