using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Attribute;
using Nexus.Logging.Util;

namespace Nexus.Logging.Entry
{
    public class LogEntry
    {
        /// <summary>
        /// Level of the log.
        /// </summary>
        public LogLevel Level { get; set; }
        
        /// <summary>
        /// Message of the log entry.
        /// </summary>
        public object Message { get; set; }
        
        /// <summary>
        /// Time to use for the log. If not specified, no time will be displayed.
        /// </summary>
        public DateTime Time { get; set; }
        
        /// <summary>
        /// Stack trace of the log.
        /// </summary>
        public StackTrace Trace { get; set; }

        /// <summary>
        /// Additional information to show in the log entry.
        /// </summary>
        public List<string> AdditionalLogInfo { get; set; } = new List<string>();

        /// <summary>
        /// Namespace whitelist used for filtering stack traces.
        /// If it is empty, all namespaces will be allowed.
        /// </summary>
        public List<string> NamespaceWhitelist { get; set; } = new List<string>();

        /// <summary>
        /// Namespace blacklist used for filtering stack traces.
        /// </summary>
        public List<string> NamespaceBlacklist { get; set; } = new List<string>();

        /// <summary>
        /// Returns the prefix of the message.
        /// </summary>
        /// <param name="maxWidth">Width of the message to use for wrapping.</param>
        public string GetPrefix(int maxWidth = 120)
        {
            // Build the list of entries.
            var entries = new List<string>();
            if (this.Time != default)
            {
                entries.Add(this.Time.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            }
            entries.Add(this.Level.ToString());
            entries.AddRange(this.AdditionalLogInfo);

            // Combine the entries and return them.
            if (entries.Count == 0) return "";
            return "[" + WrapText.Truncate(entries.Aggregate((i, j) => i + "] [" + j), (int) (maxWidth * 0.4)) + "]";
        }
        
        /// <summary>
        /// Returns the postfix of the message.
        /// </summary>
        /// <param name="maxWidth">Width of the message to use for wrapping.</param>
        public string GetPostfix(int maxWidth = 120)
        {
            // Return an empty string if no trace is defined.
            if (this.Trace == null) return "";
            
            // Iterate over the stace trace frames until a valid one is reached.
            foreach (var frame in this.Trace.GetFrames())
            {
                var method = frame.GetMethod();
                var type = method?.DeclaringType;
                if (type?.Namespace == null) continue;
                if (type.Namespace != null && type.Namespace.StartsWith("Nexus.Logging", StringComparison.InvariantCulture)) continue;
                if (this.NamespaceWhitelist.Count > 0 && this.NamespaceWhitelist.FirstOrDefault(name => type.Namespace.StartsWith(name, StringComparison.InvariantCulture)) == null) continue;
                if (this.NamespaceBlacklist.Count > 0 && this.NamespaceBlacklist.FirstOrDefault(name => type.Namespace.StartsWith(name, StringComparison.InvariantCulture)) != null) continue;
                if (method.GetCustomAttributes(typeof(IgnoreLoggingAttribute), true).Length > 0) continue;
                if (type.Name.Contains('<')) continue;
                if (method.Name.Contains('<')) continue;
                
                return "[" + WrapText.Truncate($"{type.Name}.{method.Name}",  (int) (maxWidth * 0.4)) + "]";
            }
            
            // Return an empty string if no trace frame works.
            return "";
        }

        /// <summary>
        /// Returns the lines to display for the log entry.
        /// </summary>
        /// <param name="maxWidth">Width of the message to use for wrapping.</param>
        public List<string> GetLines(int maxWidth = 120)
        {
            // Get the prefix and post fix.
            var prefix = this.GetPrefix(maxWidth);
            var postfix = this.GetPostfix(maxWidth);
            var maxLineWidth = maxWidth - prefix.Length - postfix.Length - 2;

            // Add the lines.
            var lines = new List<string>();
            var wrappedMessage = WrapText.Wrap(this.Message == null ? "null" : this.Message.ToString(), maxLineWidth);
            for (var i = 0; i < wrappedMessage.Count; i++)
            {
                var line = wrappedMessage[i];
                if (i == 0)
                {
                    lines.Add(prefix + " " + line + new string( ' ', maxWidth - (prefix.Length + line.Length + postfix.Length + 1)) + postfix);
                }
                else
                {
                    lines.Add(new string( ' ', prefix.Length + 1) + line);
                }
            }
            
            // Return the lines.
            return lines;
        }
    }
}