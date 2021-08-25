using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Entry;

namespace Nexus.Logging.Output
{
    public class ConsoleOutput : QueuedOutput
    {
        /// <summary>
        /// Console colors for the log levels.
        /// </summary>
        private static readonly Dictionary<LogLevel, ConsoleColor> ConsoleColors = new Dictionary<LogLevel, ConsoleColor>
        {
            {LogLevel.Trace, ConsoleColor.Green},
            {LogLevel.Debug, ConsoleColor.Green},
            {LogLevel.Information, ConsoleColor.White},
            {LogLevel.Warning, ConsoleColor.Yellow},
            {LogLevel.Error, ConsoleColor.Red},
            {LogLevel.Critical, ConsoleColor.DarkRed},
            {LogLevel.None, ConsoleColor.White},
        };
        
        /// <summary>
        /// Processes a message.
        /// </summary>
        /// <param name="entry">Entry to process</param>
        public override async Task ProcessMessage(LogEntry entry)
        {
            // Set the color.
            var color = ConsoleColors[entry.Level];
            if (color == ConsoleColor.White)
            {
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = color;
            }
            
            // Log the lines.
            foreach (var line in entry.GetLines(Console.WindowWidth > 0 ? Console.WindowWidth - 1 : 120))
            {
                await Console.Out.WriteLineAsync(line).ConfigureAwait(false);
            }
            
            // Reset the color.
            Console.ResetColor();
        }
    }
}