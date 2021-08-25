# Nexus Logging
Nexus Logging is a .NET Standard library for logging
to consoles and files. It is intended to be use with
the [Uchu Server project](https://github.com/uchuserver/uchu).

# Example
```c#
// Create the logger.
var logger = new Logger();

// Create the output.
// 1 logger can have many outputs, even a custom one that implements IOutput.
// FileOutput is also provided.
var consoleLogger = new ConsoleOutput();
consoleLogger.IncludeDate = true; // Includes dates in the logs.
consoleLogger.MinimumLevel = LogLevel.Warning; // Only show Warning and above logs.
consoleLogger.NamespaceWhitelist.Add("My.Project"); // (Optional) Ensure only your project methods show with log messages.
logger.Outputs.Add(consoleLogger);

// Log some messages.
consoleLogger.Info("Not shown due to being Info");
consoleLogger.Warning("Shown due to being Warning");
consoleLogger.Error("Shown due to being Error");
```

# Contributing
Both issues and pull requests are accepted for this project.

# License
Nexus Logging is available under the terms of the MIT License. See [LICENSE](LICENSE) for details.