using System.IO;
using System.Threading.Tasks;
using Nexus.Logging.Entry;

namespace Nexus.Logging.Output
{
    public class FileOutput : QueuedOutput
    {
        /// <summary>
        /// Location to write the logs.
        /// </summary>
        public string FileLocation { get; set; } = "output.log";
        
        /// <summary>
        /// Processes a message.
        /// </summary>
        /// <param name="entry">Entry to process</param>
        public override Task ProcessMessage(LogEntry entry)
        {
            File.AppendAllLines(this.FileLocation, entry.GetLines(140));
            return Task.CompletedTask;
        }
    }
}