using System.Threading;

namespace Nexus.Logging.Entry
{
    public class LogQueueEntry
    {
        /// <summary>
        /// Log entry for the queue.
        /// </summary>
        public LogEntry Entry { get; set; }
        
        /// <summary>
        /// Next entry in the queue.
        /// </summary>
        public LogQueueEntry NextEntry { get; set; }
    }
    
    public class LogQueue
    {
        /// <summary>
        /// Next entry in the log queue.
        /// </summary>
        private LogQueueEntry _nextEntry;
        
        /// <summary>
        /// Last entry in the log queue.
        /// </summary>
        private LogQueueEntry _lastEntry;
        
        /// <summary>
        /// Semaphore for adding items to the log queue.
        /// </summary>
        private readonly SemaphoreSlim _logSemaphore = new SemaphoreSlim(1);

        /// <summary>
        /// Adds a log entry to the queue.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        public void AddEntry(LogEntry entry)
        {
            // Lock the queue.
            this._logSemaphore.Wait();
            
            // Add the log.
            var newEntry = new LogQueueEntry()
            {
                Entry = entry,
            };
            if (this._nextEntry == null)
            {
                this._nextEntry = newEntry;
                this._lastEntry = newEntry;
            }
            else
            {
                this._lastEntry.NextEntry = newEntry;
                this._lastEntry = newEntry;
            }
            
            // Release the queue.
            this._logSemaphore.Release();
        }

        /// <summary>
        /// Pops an entry from the log queue.
        /// </summary>
        /// <returns>The next log entry to use.</returns>
        public LogEntry PopEntry()
        {
            // Lock the queue.
            this._logSemaphore.Wait();
            
            // Pop the next entry.
            var nextEntry = this._nextEntry;
            this._nextEntry = nextEntry?.NextEntry;
            if (this._nextEntry == null)
            {
                this._lastEntry = null;
            }
            
            // Release the queue and return the last entry.
            this._logSemaphore.Release();
            return nextEntry?.Entry;
        }
    }
}