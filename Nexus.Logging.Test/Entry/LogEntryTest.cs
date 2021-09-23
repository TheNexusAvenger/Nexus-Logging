using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nexus.Logging.Entry;
using NUnit.Framework;

namespace Nexus.Logging.Test.Entry
{
    public class LogEntryTest
    {
        /// <summary>
        /// Tests the GetPrefix method.
        /// </summary>
        [Test]
        public void TestGetPrefix()
        {
            var logEntry = new LogEntry();
            logEntry.Level = LogLevel.Information;
            Assert.AreEqual(logEntry.GetPrefix(), "[Info ]");
            logEntry.Level = LogLevel.Debug;
            Assert.AreEqual(logEntry.GetPrefix(), "[Debug]");
            logEntry.Time = new DateTime(637654124218362434);
            Assert.AreEqual(logEntry.GetPrefix(), "[2021-08-24 14:33:41.836] [Debug]");
            logEntry.AdditionalLogInfo.Add("Test1");
            logEntry.AdditionalLogInfo.Add("Test2");
            Assert.AreEqual(logEntry.GetPrefix(), "[2021-08-24 14:33:41.836] [Debug] [Test1] [Test2]");
            Assert.AreEqual(logEntry.GetPrefix(80), "[2021-08-24 14:33:41.836] [Deb...]");
        }
        
        /// <summary>
        /// Tests the GetPostfix method.
        /// </summary>
        [Test]
        public void TestGetPostfix()
        {
            var logEntry = new LogEntry();
            Assert.AreEqual(logEntry.GetPostfix(), "");
            logEntry.OverridePostfix = "Test";
            Assert.AreEqual(logEntry.GetPostfix(), "[Test]");
            logEntry.OverridePostfix = "";
            Assert.AreEqual(logEntry.GetPostfix(), "");
        }

        /// <summary>
        /// Tests the GetLines method.
        /// </summary>
        [Test]
        public void TestGetLines()
        {
            var logEntry = new LogEntry();
            logEntry.Level = LogLevel.Debug;
            logEntry.Message = "Test message";
            Assert.AreEqual(logEntry.GetLines(40), new List<string>() { "[Debug] Test message                    "});
            logEntry.Message = "Some long message that wraps between multiple lines";
            Assert.AreEqual(logEntry.GetLines(40), new List<string>() { "[Debug] Some long message that wraps    ", "        between multiple lines"});
            logEntry.Message = "Test message 1\nTest message 2\nTest message 3";
            Assert.AreEqual(logEntry.GetLines(40), new List<string>() { "[Debug] Test message 1                  ", "        Test message 2", "        Test message 3"});
        }
    }
}