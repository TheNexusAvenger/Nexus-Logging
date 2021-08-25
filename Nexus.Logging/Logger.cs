using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Output;

namespace Nexus.Logging
{
    public class Logger
    {
        /// <summary>
        /// Outputs for the logs.
        /// </summary>
        public List<IOutput> Outputs = new List<IOutput>();
        
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        /// <param name="level">Log level to output with.</param>
        public void Log(object message, LogLevel level)
        {
            foreach (var output in this.Outputs)
            {
                output?.LogMessage(message, level);
            }
        }

        /// <summary>
        /// Logs a message as a Debug level.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        public void Debug(object message)
        {
            this.Log(message, LogLevel.Debug);
        }

        /// <summary>
        /// Logs a message as an Information level.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        public void Info(object message)
        {
            this.Log(message, LogLevel.Information);
        }

        /// <summary>
        /// Logs a message as a Warning level.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        public void Warn(object message)
        {
            this.Log(message, LogLevel.Warning);
        }
        
        /// <summary>
        /// Logs a message as a Error level.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        public void Error(object message)
        {
            this.Log(message, LogLevel.Error);
        }
    }
}