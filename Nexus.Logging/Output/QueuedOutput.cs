using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Entry;

namespace Nexus.Logging.Output
{
    public abstract class QueuedOutput : IOutput
    {
        /// <summary>
        /// Whether the queue is being processed.
        /// </summary>
        private bool _processing = false;

        /// <summary>
        /// Queue for the log entries.
        /// </summary>
        private readonly LogQueue _queue = new LogQueue();

        /// <summary>
        /// Semaphore for starting the processing.
        /// </summary>
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        /// <summary>
        /// Semaphore for processing.
        /// This is taken by the processing loop and can be used by CloseAsync to wait for processing to finish.
        /// </summary>
        private readonly SemaphoreSlim _processingSemaphore = new SemaphoreSlim(1);
        
        /// <summary>
        /// Whether to include the date with the logs.
        /// </summary>
        public bool IncludeDate { get; set; }

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
        /// Minimum level to output.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log. Can be an object, like an exception.</param>
        /// <param name="level">Log level to output with.</param>
        /// <param name="overridePostfix">Override postfix for the message.</param>
        public void LogMessage(object message, LogLevel level, string overridePostfix = null)
        {
            // Return if the log level is too slow.
            if (level < this.MinimumLevel) return;
            
            // Add the message to the queue.
            this._queue.AddEntry(new LogEntry()
            {
                Level = level,
                Message = message,
                Time = (this.IncludeDate ? DateTime.Now : default),
#if DEBUG
                Trace = new StackTrace(),
#endif
                AdditionalLogInfo = this.AdditionalLogInfo,
                OverridePostfix = overridePostfix,
                NamespaceWhitelist = this.NamespaceWhitelist,
                NamespaceBlacklist = this.NamespaceBlacklist,
            });
            
            // Start processing the queue.
            this._semaphore.Wait();
            if (!this._processing)
            {
                this._processing = true;
                this._processingSemaphore.Wait();
                Task.Run(async () =>
                {
                    while (true)
                    {
                        var entry = this._queue.PopEntry();
                        if (entry == null) break;
                        await this.ProcessMessage(entry);
                    }
                    this._processing = false;
                    this._processingSemaphore.Release();
                });
            }
            this._semaphore.Release();
        }

        /// <summary>
        /// Waits for the current log queue to be processed.
        /// </summary>
        public async Task WaitForCompletionAsync()
        {
            await this._processingSemaphore.WaitAsync();
            this._processingSemaphore.Release();
        }

        /// <summary>
        /// Processes a message.
        /// </summary>
        /// <param name="entry">Entry to process</param>
        public abstract Task ProcessMessage(LogEntry entry);
    }
}