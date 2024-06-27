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
            Assert.That(logEntry.GetPrefix(), Is.EqualTo("[Info ]"));
            logEntry.Level = LogLevel.Debug;
            Assert.That(logEntry.GetPrefix(), Is.EqualTo("[Debug]"));
            logEntry.Time = new DateTime(637654124218362434);
            Assert.That(logEntry.GetPrefix(), Is.EqualTo("[2021-08-24 14:33:41.836] [Debug]"));
            logEntry.AdditionalLogInfo.Add("Test1");
            logEntry.AdditionalLogInfo.Add("Test2");
            Assert.That(logEntry.GetPrefix(), Is.EqualTo("[2021-08-24 14:33:41.836] [Debug] [Test1] [Test2]"));
            Assert.That(logEntry.GetPrefix(80), Is.EqualTo("[2021-08-24 14:33:41.836] [Deb...]"));
        }
        
        /// <summary>
        /// Tests the GetPostfix method.
        /// </summary>
        [Test]
        public void TestGetPostfix()
        {
            var logEntry = new LogEntry();
            Assert.That(logEntry.GetPostfix(), Is.EqualTo(""));
            logEntry.OverridePostfix = "Test";
            Assert.That(logEntry.GetPostfix(), Is.EqualTo("[Test]"));
            logEntry.OverridePostfix = "";
            Assert.That(logEntry.GetPostfix(), Is.EqualTo(""));
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
            Assert.That(logEntry.GetLines(40), Is.EqualTo(new List<string>() { "[Debug] Test message                    "}));
            logEntry.Message = "Some long message that wraps between multiple lines";
            Assert.That(logEntry.GetLines(40), Is.EqualTo(new List<string>() { "[Debug] Some long message that wraps    ", "        between multiple lines"}));
            logEntry.Message = "Test message 1\nTest message 2\nTest message 3";
            Assert.That(logEntry.GetLines(40), Is.EqualTo(new List<string>() { "[Debug] Test message 1                  ", "        Test message 2", "        Test message 3"}));
        }
    }
}