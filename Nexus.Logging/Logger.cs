using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Output;

namespace Nexus.Logging
{
    public class Logger : ILoggerProvider
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
        /// /// <param name="overridePostfix">Override postfix for the message.</param>
        public void Log(object message, LogLevel level, string overridePostfix = null)
        {
            foreach (var output in this.Outputs)
            {
                output?.LogMessage(message, level, overridePostfix);
            }
        }

        /// <summary>
        /// Logs a message as a Trace level.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        public void Trace(object message)
        {
            this.Log(message, LogLevel.Trace);
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

        /// <summary>
        /// Creates a logger.
        /// </summary>
        /// <param name="categoryName">Name of the category to create the logger for. This becomes the postfix.</param>
        /// <returns>The logger for the category.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new WrappedLogger(this, categoryName);
        }

        /// <summary>
        /// Disposes of the logger.
        /// </summary>
        public void Dispose()
        {
            
        }
        
        /// <summary>
        /// Waits for all loggers to finish processing logs.
        /// </summary>
        public async Task WaitForCompletionAsync()
        {
            foreach (var output in this.Outputs)
            {
                if (!(output is QueuedOutput queuedOutput)) continue;
                await queuedOutput.WaitForCompletionAsync();
            }
        }
    }
    
    public class WrappedLogger : ILogger
    {
        /// <summary>
        /// Logger to write to.
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// Postfix to use with the log messages.
        /// </summary>
        private readonly string _postfix;

        /// <summary>
        /// Creates a wrapped logger.
        /// </summary>
        /// <param name="logger">Logger to wrap.</param>
        /// <param name="postfix">Postfix to log as.</param>
        public WrappedLogger(Logger logger, string postfix)
        {
            this._logger = logger;
            this._postfix = postfix;
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        /// <param name="level">Log level to output with.</param>
        public void Log(object message, LogLevel level)
        {
            this._logger.Log(message, level, this._postfix);
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="logLevel">Log level to log as.</param>
        /// <param name="eventId">Event id of the log.</param>
        /// <param name="state">State to log.</param>
        /// <param name="exception">Exception to log.</param>
        /// <param name="formatter">Formatter for the log.</param>
        /// <typeparam name="TState">Type of the state to log.</typeparam>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.Log(formatter(state, exception), logLevel);
        }

        /// <summary>
        /// Returns if the log level is enabled.
        /// </summary>
        /// <param name="logLevel">Log level to check.</param>
        /// <returns>Always true. The output determines the log level filter.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Begins the scope of the logger.
        /// </summary>
        /// <param name="state">State to log.</param>
        /// <typeparam name="TState">Type of the state to log.</typeparam>
        /// <returns>Disposable of the scope.</returns>
        public IDisposable BeginScope<TState>(TState state) => default;
    }
}