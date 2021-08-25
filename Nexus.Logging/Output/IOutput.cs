using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Entry;

namespace Nexus.Logging.Output
{
    public interface IOutput
    {
        /// <summary>
        /// Whether to include the date with the logs.
        /// </summary>
        bool IncludeDate { get; set; }
        
        /// <summary>
        /// Additional information to show in the log entry.
        /// </summary>
        List<string> AdditionalLogInfo { get; set; }

        /// <summary>
        /// Namespace whitelist used for filtering stack traces.
        /// If it is empty, all namespaces will be allowed.
        /// </summary>
        List<string> NamespaceWhitelist { get; set; }

        /// <summary>
        /// Namespace blacklist used for filtering stack traces.
        /// </summary>
        List<string> NamespaceBlacklist { get; set; }
        
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        /// <param name="level">Log level to output with.</param>
        void LogMessage(object message, LogLevel level);
    }
}