using System;

namespace Nexus.Logging.Attribute
{
    /// <summary>
    /// Attribute for ignoring a method from the stack trace.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LogTraceIgnoreAttribute : System.Attribute
    {
        
    }
}