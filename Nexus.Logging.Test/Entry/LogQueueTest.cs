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
            Assert.IsNull(queue.PopEntry());
            queue.AddEntry(new LogEntry()
            {
                Message = "Message 1",
            });
            queue.AddEntry(new LogEntry()
            {
                Message = "Message 2",
            });
            Assert.AreEqual(queue.PopEntry().Message, "Message 1");
            Assert.AreEqual(queue.PopEntry().Message, "Message 2");
            Assert.IsNull(queue.PopEntry());
        }
    }
}