using System.Collections.Generic;
using Nexus.Logging.Util;
using NUnit.Framework;

namespace Nexus.Logging.Test.Util
{
    public class WrapTextTest
    {
        /// <summary>
        /// Tests the Wrap method.
        /// </summary>
        [Test]
        public void TestWrap()
        {
            // Test wrapping short words.
            Assert.AreEqual(WrapText.Wrap("", 8), new List<string>());
            Assert.AreEqual(WrapText.Wrap("1234", 8), new List<string>() { "1234" });
            Assert.AreEqual(WrapText.Wrap("12345678", 8), new List<string>() { "12345678" });
            
            // Test wrapping long words.
            Assert.AreEqual(WrapText.Wrap("1234567890123", 8), new List<string>() { "12345678", "90123" });
            Assert.AreEqual(WrapText.Wrap("12345678901234567890", 8), new List<string>() { "12345678", "90123456", "7890" });
            
            // Test wrapping multiple short words.
            Assert.AreEqual(WrapText.Wrap("1234 56", 8), new List<string>() { "1234 56" });
            Assert.AreEqual(WrapText.Wrap("1234 56789 01 23456", 8), new List<string>() { "1234", "56789 01", "23456" });
            
            // Test wrapping multiple long words.
            Assert.AreEqual(WrapText.Wrap("1234 5678901234 567", 8), new List<string>() { "1234", "56789012", "34 567" });
            
            // Test white spaces.
            Assert.AreEqual(WrapText.Wrap("1234\n56 789", 8), new List<string>() { "1234", "56 789" });
            Assert.AreEqual(WrapText.Wrap("12    56", 8), new List<string>() { "12    56" });
            Assert.AreEqual(WrapText.Wrap("12\t56", 8), new List<string>() { "12    56" });
        }

        /// <summary>
        /// Tests the truncate method.
        /// </summary>
        [Test]
        public void TestTruncate()
        {
            Assert.AreEqual(WrapText.Truncate("01235", 8), "01235");
            Assert.AreEqual(WrapText.Truncate("01235678", 8), "01235678");
            Assert.AreEqual(WrapText.Truncate("012356789", 8), "01235...");
            Assert.AreEqual(WrapText.Truncate("0123567891023", 8), "01235...");
        }
    }
}