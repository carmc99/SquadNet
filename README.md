# SquadNet

SquadNet is a framework designed to read and process logs from the *SquadGame* using various methods such as FTP, Tail, and SFTP. It also enables real-time event detection and triggering.

Additionally, it offers RCON integration, allowing administrative commands to be executed on Squad servers. SquadNet provides an extensible architecture for creating custom plugins, enabling users to define their own event processing logic.

> **Note:** The CLI and REST API are under development.

## Features

- **Log Monitoring**: Support for FTP, Tail, and SFTP.
- **Event System**: Captures and processes real-time events from logs.
- **RCON Support**: Allows command execution on Squad servers.
- **Extensibility**: Plugin system to customize event logic.
- **Background Service**: A service that continuously monitors log changes.
- **Unit Testing**: Set of tests to ensure parser accuracy.

## Installation

> Prerequisites:
> - .NET 6 or higher
> - Access to Squad server logs (FTP, SFTP, or direct access)
> - RCON configuration (optional)

1. Clone the repository:
   ```sh
   git clone https://github.com/carmc99/SquadNet.git
   cd SquadNet
   ```
2. Build the project:
   ```sh
   dotnet build
   ```
3. Run SquadNet:
   ```sh
   dotnet run
   ```

## Plugin Usage

SquadNet allows the creation of custom plugins to define specific event management logic.

Example of a plugin that logs chat messages:

```csharp
using SquadNET.Core.Squad.Events;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Plugins.Abstractions;
using System.Reflection;

namespace SquadNET.Plugins
{
    public class ChatMessagePlugin : Plugin
    {
        public override string Name => "ChatMessagePlugin";

        public override void OnEventRaised(string eventName, ISquadEventData eventData)
        {
            if (eventName == SquadEventType.ChatMessage.ToString())
            {
                if (eventData is ChatMessageEventModel chat)
                {
                    string message = chat.ToString();
                    WriteToLogFile(message);
                }
            }
        }

        private void WriteToLogFile(string message)
        {
            try
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string logFilePath = Path.Combine(assemblyPath, "ChatMessages.log");

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
```

## Roadmap

- [x] Log reading via FTP, Tail, and SFTP.
- [x] Real-time event system.
- [x] Basic RCON support.
- [x] Implementation of a service for continuous monitoring.
- [ ] Plugin System Improvement.
- [ ] CLI Development.
- [ ] REST API Development.
- [ ] Creation of additional plugins.
- [ ] Database integration for event storage.

## Accuracy Statement

SquadNet is an evolving project and is currently under development. While efforts are made to ensure the accuracy and reliability of its features, there may be bugs, incomplete functionalities, or changes in behavior as the project progresses.

## Inspiration

SquadNet is inspired by **SquadJS**, aiming to provide an alternative framework with a modular and extensible approach to Squad server log processing.

## Contributions

Contributions are welcome. If you wish to collaborate, follow these steps:

1. Fork the repository.
2. Create a branch with your feature or fix.
3. Submit a *pull request* with a detailed description of the changes.

## License

SquadNet is licensed under the **Business Source License 1.0 (BSL 1.0)**.

---


