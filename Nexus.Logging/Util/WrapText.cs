using System.Collections.Generic;

namespace Nexus.Logging.Util
{
    public static class WrapText
    {
        /// <summary>
        /// Wraps a message if it overflows a max length.
        /// </summary>
        /// <param name="message">Message to wrap.</param>
        /// <param name="maxLength">Max length of the messages.</param>
        /// <returns>The list of messages that are wrapped.</returns>
        public static List<string> Wrap(string message, int maxLength)
        {
            // Clean the message.
            message = message.Replace("\t", "    ").TrimEnd();
            
            // If there are new lines, get the individual results and join them together.
            var lines = new List<string>();
            if (message.Contains("\n"))
            {
                foreach (var line in message.Split('\n'))
                {
                    lines.AddRange(Wrap(line, maxLength));
                }
                return lines;
            }
            
            // Add lines for the message until the end is reached.
            var currentLine = "";
            foreach (var word in message.Split(' '))
            {
                if (word.Length <= maxLength && currentLine == "")
                {
                    // Set the line as the word if it is short enough.
                    currentLine = word;
                }
                else if (currentLine.Length + word.Length + 1 <= maxLength && currentLine != "")
                {
                    // Add the word to the line.
                    currentLine += " " + word;
                }
                else
                {
                    // Store the current line.
                    if (currentLine != "")
                    {
                        lines.Add(currentLine);
                    }

                    // Add lines for the message until the word is short enough.
                    var currentWord = word;
                    while (currentWord.Length > maxLength)
                    {
                        lines.Add(currentWord.Substring(0, maxLength));
                        currentWord = currentWord.Substring(maxLength);
                    }
                    currentLine = currentWord;
                }
            }
            if (currentLine != "")
            {
                lines.Add(currentLine);
            }
            
            // Return the wrapped lines.
            return lines;
        }


        /// <summary>
        /// Truncates a message if it is too long.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxLength">Max length of the message.</param>
        /// <returns></returns>
        public static string Truncate(string message, int maxLength)
        {
            return message.Length > maxLength ? message.Substring(0, maxLength - 3) + "..." : message;
        }
    }
}