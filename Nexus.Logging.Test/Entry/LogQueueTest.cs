using Nexus.Logging.Entry;
using NUnit.Framework;

namespace Nexus.Logging.Test.Entry
{
    public class LogQueueTest
    {
        /// <summary>
        /// Tests the PopEntry method.
        /// </summary>
        [Test]
        public void TestPopEntry()
        {
            var queue = new LogQueue();
            Assert.That(queue.PopEntry(), Is.Null);
            queue.AddEntry(new LogEntry()
            {
                Message = "Message 1",
            });
            queue.AddEntry(new LogEntry()
            {
                Message = "Message 2",
            });
            Assert.That(queue.PopEntry().Message, Is.EqualTo("Message 1"));
            Assert.That(queue.PopEntry().Message, Is.EqualTo("Message 2"));
            Assert.That(queue.PopEntry(), Is.Null);
        }
    }
}