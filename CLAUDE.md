# CLAUDE.md - AI Assistant Guide for SquadNet

> Last Updated: 2026-01-17
>
> This document provides comprehensive guidance for AI assistants working with the SquadNet codebase.

## Project Overview

**SquadNet** is a .NET 6+ framework for monitoring and managing Squad game servers. It provides:
- Real-time log parsing from multiple sources (FTP, SFTP, Tail)
- Event-driven architecture with 42+ game event types
- RCON integration for server administration
- Extensible plugin system for custom event handling
- Background service for continuous monitoring
- Database integration for data persistence

**Technology Stack:**
- .NET 6+
- C# 10+
- Entity Framework Core 9.0.3
- MediatR (CQRS pattern)
- FluentValidation
- xUnit + Moq (testing)
- Serilog (logging)

**Inspired by:** SquadJS (Node.js framework)

---

## Repository Structure

```
SquadNet/
├── SquadNET.Core/                    # Core abstractions, interfaces, event models
│   ├── Squad/
│   │   ├── Events/
│   │   │   ├── Models/               # Event models with [RegexPattern] attributes
│   │   │   └── SquadEventType.cs     # 42 event type enum
│   │   ├── Parsers/                  # Event parsers using regex patterns
│   │   └── Commands/                 # RCON command templates and definitions
│   ├── DictionaryModelConverter.cs   # Converts regex groups to typed models
│   └── IRconService.cs               # RCON service interface
│
├── SquadNET.Application/             # Business logic (CQRS with MediatR)
│   ├── Squad/
│   │   ├── {Domain}/
│   │   │   ├── Queries/              # MediatR queries (Request/Handler/Validator)
│   │   │   ├── Repositories/         # Repository interfaces and EF implementations
│   │   │   └── Models/               # Database entities
│   │   └── ParseLineQueryHandler.cs  # Central parser orchestrator
│   └── DbContextBase.cs              # EF Core context base
│
├── SquadNET.LogManagement/           # Log reading implementations
│   ├── LogReaders/
│   │   ├── TailLogReader.cs          # Local file monitoring (FileSystemWatcher)
│   │   ├── FtpLogReader.cs           # FTP polling (FluentFTP)
│   │   └── SftpLogReader.cs          # SFTP polling (SSH.NET)
│   └── LogReaderFactory.cs           # Factory for creating readers
│
├── SquadNET.Rcon/                    # RCON client implementation
│   └── SquadRcon.cs                  # Main RCON service
│
├── SquadNET.Plugins.Abstractions/    # Plugin system base
│   ├── IPlugin.cs                    # Plugin interface
│   ├── Plugin.cs                     # Base class with helpers
│   └── PluginManager.cs              # Plugin lifecycle manager
│
├── SquadNET.Plugins/                 # Sample plugin implementations
│   └── ChatMessagePlugin.cs          # Example: logs chat to file
│
├── SquadNET.SquadMonitoringService/  # Background service (primary entry point)
│   ├── Program.cs                    # Service host configuration
│   ├── SquadEventProcessingService.cs # Log monitoring + event processing
│   ├── SquadDataUpdateService.cs     # Periodic RCON polling + DB updates
│   └── appsettings.json              # Configuration
│
├── SquadNET.API/                     # REST API (under development)
├── SquadNET.CLI/                     # Command-line interface (under development)
├── SquadNET.Extensions.Exceptions/   # Custom exception extensions
└── SquadNET.Test/                    # xUnit tests
    ├── Squad/
    │   ├── SquadTestBase.cs          # Base test class with helpers
    │   └── TestData/                 # JSON test data files
    └── LogReaderTests/               # Log reader integration tests
```

---

## Architecture Patterns

### 1. Event-Driven Architecture

**Flow:**
```
Log Line → ILogReader.OnLogLine Event
         → ParseLineQueryHandler (MediatR)
         → Try All Parsers
         → Extract Event (EventName + EventData)
         → PluginManager.EmitEvent()
         → All Plugins.OnEventRaised()
```

**Key Files:**
- Event Types: `SquadNET.Core/Squad/Events/SquadEventType.cs` (42 events)
- Event Models: `SquadNET.Core/Squad/Events/Models/*.cs`
- Central Parser: `SquadNET.Application/Squad/ParseLineQueryHandler.cs`

### 2. CQRS with MediatR

All business logic uses **Command/Query Responsibility Segregation**:

```csharp
public static class ChatMessageQuery
{
    // Request DTO
    public class Request : IRequest<ChatMessageEventModel>
    {
        public string LogLine { get; set; }
    }

    // Handler (business logic)
    public class Handler : IRequestHandler<Request, ChatMessageEventModel>
    {
        public async Task<ChatMessageEventModel> Handle(Request request, ...)
        {
            // Implementation
        }
    }

    // Validator (FluentValidation)
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.LogLine).NotEmpty();
        }
    }
}
```

**Convention:** Queries are grouped in static classes with nested Request/Handler/Validator classes.

### 3. Regex Pattern Attribute System

Event models use declarative regex patterns:

```csharp
[RegexPattern(@"\[ChatAll\] \[SteamID:(?<steamId>\d+)\] (?<playerName>.+?) : (?<message>.+)")]
public class ChatMessageEventModel : ISquadEventData
{
    public string SteamId { get; set; }
    public string PlayerName { get; set; }
    public string Message { get; set; }
}
```

**Parser Implementation:**
```csharp
public class ChatMessageParser : IParser<ChatMessageEventModel>
{
    public ChatMessageEventModel Parse(string input)
    {
        var regex = RegexPatternHelper.GetRegex<ChatMessageEventModel>();
        var match = regex.Match(input);

        var dict = RegexPatternHelper.MatchToDictionary(match);
        return DictionaryModelConverter.ConvertDictionaryToModel<ChatMessageEventModel>(dict);
    }
}
```

**Key Components:**
- `RegexPatternHelper` - Extracts regex from attributes
- `DictionaryModelConverter` - Converts named groups to typed models

### 4. Repository Pattern

Database access uses repository abstraction:

```csharp
public interface IServerInfoRepository
{
    Task<ServerInfoModel> GetAsync();
    Task UpdateAsync(ServerInfoModel model);
}

public class EFServerInfoRepository : IServerInfoRepository
{
    private readonly DbContextBase _context;

    // Implementation using EF Core
}
```

**Supported Providers:** SQLite, SQL Server, InMemory

### 5. Factory Pattern

Log readers use factory creation:

```csharp
ILogReader reader = logReaderFactory.Create(LogReaderType.Sftp);
reader.OnLogLine += (sender, line) => { /* handle */ };
reader.StartWatching();
```

### 6. Plugin System

Extensible event handling via plugins:

```csharp
public class MyPlugin : Plugin
{
    public override string Name => "MyPlugin";

    public override void OnEventRaised(string eventName, ISquadEventData eventData)
    {
        if (eventName == SquadEventType.PlayerDied.ToString())
        {
            var death = (PlayerDiedEventModel)eventData;
            // Handle player death
        }
    }
}
```

**Plugin Loading:** DLLs in `/plugins` folder are auto-discovered and registered.

---

## Development Workflows

### Adding a New Event Type

1. **Add Event Type to Enum**
   ```csharp
   // SquadNET.Core/Squad/Events/SquadEventType.cs
   public enum SquadEventType
   {
       // ... existing events
       MyNewEvent,
   }
   ```

2. **Create Event Model with Regex Pattern**
   ```csharp
   // SquadNET.Core/Squad/Events/Models/MyNewEventModel.cs
   [RegexPattern(@"regex pattern with named groups")]
   public class MyNewEventModel : ISquadEventData
   {
       public string Property1 { get; set; }
       public int Property2 { get; set; }
   }
   ```

3. **Create Parser**
   ```csharp
   // SquadNET.Core/Squad/Parsers/MyNewEventParser.cs
   public class MyNewEventParser : IParser<MyNewEventModel>
   {
       public MyNewEventModel Parse(string input)
       {
           var regex = RegexPatternHelper.GetRegex<MyNewEventModel>();
           var match = regex.Match(input);
           if (!match.Success) return null;

           var dict = RegexPatternHelper.MatchToDictionary(match);
           return DictionaryModelConverter.ConvertDictionaryToModel<MyNewEventModel>(dict);
       }
   }
   ```

4. **Register Parser**
   ```csharp
   // SquadNET.Core/DependencyInjection/ServiceCollectionExtensions.cs
   services.AddTransient<IParser<MyNewEventModel>, MyNewEventParser>();
   ```

5. **Create MediatR Query**
   ```csharp
   // SquadNET.Application/Squad/{Domain}/Queries/MyNewEventQuery.cs
   public static class MyNewEventQuery
   {
       public class Request : IRequest<MyNewEventModel>
       {
           public string LogLine { get; set; }
       }

       public class Handler : IRequestHandler<Request, MyNewEventModel>
       {
           private readonly IParser<MyNewEventModel> _parser;

           public async Task<MyNewEventModel> Handle(Request request, ...)
           {
               return _parser.Parse(request.LogLine);
           }
       }
   }
   ```

6. **Update ParseLineQueryHandler**
   ```csharp
   // SquadNET.Application/Squad/ParseLineQueryHandler.cs
   // Add to the switch statement or parser list
   ```

7. **Add Tests**
   ```csharp
   // SquadNET.Test/Squad/MyNewEventParserTests.cs
   public class MyNewEventParserTests : SquadTestBase
   {
       [Theory]
       [MemberData(nameof(GetTestData))]
       public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
           string input, MyNewEventModel expected)
       {
           var parser = new MyNewEventParser();
           var result = parser.Parse(input);

           Assert.NotNull(result);
           Assert.Equal(expected.Property1, result.Property1);
       }

       public static IEnumerable<object[]> GetTestData() =>
           LoadTestData<TestCase>("my_new_event.json");
   }
   ```

8. **Create Test Data**
   ```json
   // SquadNET.Test/Squad/TestData/my_new_event.json
   [
       {
           "input": "log line example",
           "expected": {
               "property1": "value",
               "property2": 123
           }
       }
   ]
   ```

### Adding a New RCON Command

1. **Add Command to Enum**
   ```csharp
   // SquadNET.Core/Squad/Commands/SquadCommand.cs
   public enum SquadCommand
   {
       // ... existing commands
       MyNewCommand,
   }
   ```

2. **Add Command Template**
   ```csharp
   // SquadNET.Core/Squad/Commands/SquadCommandTemplate.cs
   public SquadCommandTemplate()
   {
       // ... existing mappings
       AddCommand(SquadCommand.MyNewCommand, "CommandString {0}");
   }
   ```

3. **Execute Command**
   ```csharp
   var command = new SquadCommandTemplate();
   var result = await rconService.ExecuteCommandAsync(
       command,
       SquadCommand.MyNewCommand,
       "parameter1"
   );
   ```

### Creating a New Plugin

1. **Create Plugin Class**
   ```csharp
   // MyCustomPlugin/MyPlugin.cs
   public class MyPlugin : Plugin
   {
       public override string Name => "MyPlugin";

       public override void Initialize()
       {
           // Setup resources, connections, etc.
           Logger.Information("MyPlugin initialized");
       }

       public override void OnEventRaised(string eventName, ISquadEventData eventData)
       {
           switch (eventName)
           {
               case nameof(SquadEventType.PlayerDied):
                   HandlePlayerDeath((PlayerDiedEventModel)eventData);
                   break;

               case nameof(SquadEventType.ChatMessage):
                   HandleChatMessage((ChatMessageEventModel)eventData);
                   break;
           }
       }

       private void HandlePlayerDeath(PlayerDiedEventModel death)
       {
           // Custom logic
       }

       public override void Shutdown()
       {
           // Cleanup resources
           Logger.Information("MyPlugin shut down");
       }
   }
   ```

2. **Build and Deploy**
   ```bash
   dotnet build MyCustomPlugin -c Release
   cp bin/Release/net6.0/MyCustomPlugin.dll /path/to/SquadNet/plugins/
   ```

3. **Plugin Auto-Discovery**
   - Plugins are automatically discovered from the `/plugins` folder
   - No manual registration required

### Adding Database Entities

1. **Create Model**
   ```csharp
   // SquadNET.Application/Squad/{Domain}/Models/MyEntityModel.cs
   public class MyEntityModel
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public DateTime CreatedAt { get; set; }
   }
   ```

2. **Add to DbContext**
   ```csharp
   // SquadNET.Application/DbContextBase.cs or specific context
   public DbSet<MyEntityModel> MyEntities { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       modelBuilder.Entity<MyEntityModel>(entity =>
       {
           entity.HasKey(e => e.Id);
           entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
       });
   }
   ```

3. **Create Repository Interface**
   ```csharp
   // SquadNET.Application/Squad/{Domain}/Repositories/IMyEntityRepository.cs
   public interface IMyEntityRepository
   {
       Task<MyEntityModel> GetByIdAsync(int id);
       Task<List<MyEntityModel>> GetAllAsync();
       Task CreateAsync(MyEntityModel entity);
       Task UpdateAsync(MyEntityModel entity);
       Task DeleteAsync(int id);
   }
   ```

4. **Implement Repository**
   ```csharp
   // SquadNET.Application/Squad/{Domain}/Repositories/EF/EFMyEntityRepository.cs
   public class EFMyEntityRepository : IMyEntityRepository
   {
       private readonly DbContextBase _context;

       public EFMyEntityRepository(DbContextBase context)
       {
           _context = context;
       }

       public async Task<MyEntityModel> GetByIdAsync(int id)
       {
           return await _context.MyEntities.FindAsync(id);
       }

       // ... other implementations
   }
   ```

5. **Register Repository**
   ```csharp
   // SquadNET.Application/DependencyInjection/ServiceCollectionExtension.cs
   services.AddScoped<IMyEntityRepository, EFMyEntityRepository>();
   ```

6. **Create Migration**
   ```bash
   dotnet ef migrations add AddMyEntity --project SquadNET.Application
   dotnet ef database update --project SquadNET.Application
   ```

---

## Key Conventions

### Naming Conventions

- **Event Models:** `{EventName}EventModel` (e.g., `ChatMessageEventModel`)
- **Parsers:** `{EventName}Parser` (e.g., `ChatMessageParser`)
- **Queries:** `{EventName}Query` with nested `Request`, `Handler`, `Validator`
- **Repositories:** `I{Entity}Repository` and `EF{Entity}Repository`
- **Plugins:** `{Purpose}Plugin` (e.g., `ChatMessagePlugin`)

### Code Style

- **Namespace Pattern:** `SquadNET.{Project}.{Feature}.{SubFeature}`
- **File Organization:** One class per file, file name matches class name
- **Access Modifiers:** Explicit for all members (no implicit private)
- **Async Methods:** All I/O operations should be async, suffix with `Async`

### Dependency Injection

- **Registration:** Use extension methods in `DependencyInjection/ServiceCollectionExtensions.cs`
- **Lifetime:**
  - Transient: Parsers, validators
  - Scoped: Repositories, MediatR handlers
  - Singleton: Services, factories, managers

**Pattern:**
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMyFeature(this IServiceCollection services)
    {
        services.AddTransient<IParser<MyModel>, MyParser>();
        services.AddScoped<IMyRepository, EFMyRepository>();
        return services;
    }
}
```

### Configuration

- **appsettings.json:** Primary configuration
- **Environment-specific:** `appsettings.Development.json`, `appsettings.Production.json`
- **Secrets:** Use User Secrets in development, environment variables in production
- **Validation:** Validate configuration on startup

**Example Configuration Section:**
```json
{
  "LogReaders": {
    "Type": "Sftp",
    "Sftp": {
      "Host": "example.com",
      "Port": 2022,
      "User": "username",
      "Password": "password",
      "LogFilePath": "/path/to/log.txt"
    }
  },
  "Filtering": {
    "IsEnabled": true,
    "ExcludePatterns": [
      "^LogNet:",
      "^LogSquad: Warning:"
    ]
  }
}
```

### Testing

- **Test Base:** Extend `SquadTestBase` for parser tests
- **Test Data:** Store in JSON files under `TestData/`
- **Naming:** `{ClassName}Tests` (e.g., `ChatMessageParserTests`)
- **Test Methods:** `GivenX_WhenY_ThenZ` pattern
- **Assertions:** Use xUnit's `Assert` class
- **Mocking:** Use Moq for dependencies

**Example Test:**
```csharp
public class ChatMessageParserTests : SquadTestBase
{
    [Theory]
    [MemberData(nameof(GetChatMessagesTestData))]
    public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
        string input,
        ChatMessageEventModel expected)
    {
        // Arrange
        var parser = new ChatMessageParser();

        // Act
        var result = parser.Parse(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.SteamId, result.SteamId);
        Assert.Equal(expected.PlayerName, result.PlayerName);
        Assert.Equal(expected.Message, result.Message);
    }

    public static IEnumerable<object[]> GetChatMessagesTestData() =>
        LoadTestData<ChatMessageTestCase>("chat_messages.json");
}
```

### Error Handling

- **Custom Exceptions:** Use `SquadNET.Extensions.Exceptions` for domain-specific exceptions
- **Logging:** Use Serilog for all logging
  - `Logger.Information()` - Important state changes
  - `Logger.Warning()` - Recoverable errors
  - `Logger.Error()` - Unrecoverable errors with exceptions
- **Try-Catch:** Only catch exceptions you can handle
- **RCON Errors:** Subscribe to `IRconService.OnExceptionThrown` event

---

## Common Tasks & Gotchas

### Working with Event Models

**CRITICAL:** All event models MUST:
1. Implement `ISquadEventData` (marker interface)
2. Have `[RegexPattern("...")]` attribute with named groups
3. Property names must match regex named groups (case-insensitive)
4. Be immutable or have public setters for converter

**Example:**
```csharp
// Regex named groups: steamId, playerName, message
[RegexPattern(@"\[ChatAll\] \[SteamID:(?<steamId>\d+)\] (?<playerName>.+?) : (?<message>.+)")]
public class ChatMessageEventModel : ISquadEventData
{
    public string SteamId { get; set; }    // Matches 'steamId' group
    public string PlayerName { get; set; }  // Matches 'playerName' group
    public string Message { get; set; }     // Matches 'message' group
}
```

### Working with ParseLineQueryHandler

The central parser tries ALL registered parsers until one succeeds:

```csharp
// In ParseLineQueryHandler
foreach (var parserType in _parserTypes)
{
    var parser = _serviceProvider.GetService(parserType);
    var result = parser.Parse(logLine);
    if (result != null)
    {
        return new Response
        {
            EventName = GetEventName(parserType),
            EventData = result
        };
    }
}
```

**Important:**
- Parsers are tried in registration order
- First successful parse wins
- Return `null` from parser if line doesn't match
- More specific patterns should be registered before generic ones

### Log Filtering

The `SquadEventProcessingService` filters log lines before parsing:

```csharp
// appsettings.json
{
  "Filtering": {
    "IsEnabled": true,
    "ExcludePatterns": [
      "^LogNet:",           // Exclude all LogNet entries
      "^LogSquad: Warning:", // Exclude Squad warnings
      "^LogTemp:"           // Exclude temp logs
    ]
  }
}
```

**Convention:** Use regex patterns to exclude noisy log entries early.

### RCON Connection Management

RCON service has connection lifecycle events:

```csharp
rconService.OnConnected += () => Logger.Information("RCON connected");
rconService.OnExceptionThrown += (ex) => Logger.Error(ex, "RCON error");

await rconService.Connect(); // Must connect before commands
```

**Gotcha:** Commands will fail silently if not connected. Always check connection or subscribe to events.

### Plugin Manager Event Emission

Plugins receive ALL events:

```csharp
public override void OnEventRaised(string eventName, ISquadEventData eventData)
{
    // Filter events you care about
    if (eventName == SquadEventType.ChatMessage.ToString())
    {
        var chat = eventData as ChatMessageEventModel;
        // Handle chat
    }
}
```

**Performance Tip:** Use switch or early returns to filter unwanted events quickly.

### Database Provider Configuration

The system supports multiple database providers:

```csharp
// appsettings.json
{
  "Database": {
    "Provider": "Sqlite",  // or "SqlServer" or "InMemory"
    "ConnectionString": "Data Source=squadnet.db"
  }
}
```

**For Development:** Use `InMemory` for fast tests, `Sqlite` for persistence.

**For Production:** Use `SqlServer` for scalability.

### Background Service Lifecycle

Two background services with different purposes:

1. **SquadEventProcessingService** (Event Processing)
   - Starts log reader
   - Connects RCON
   - Parses log lines
   - Emits events to plugins

2. **SquadDataUpdateService** (Data Polling)
   - Polls RCON every 30 seconds
   - Updates server info, players, squads, layers
   - Persists to database

**Current State:** In `Program.cs`, `SquadEventProcessingService` is commented out. Check configuration before enabling.

### Test Data Management

Test data uses JSON with specific structure:

```json
[
  {
    "input": "[ChatAll] [SteamID:76561198123456789] PlayerName : Hello world",
    "expected": {
      "steamId": "76561198123456789",
      "playerName": "PlayerName",
      "message": "Hello world"
    }
  }
]
```

**Load with:**
```csharp
public static IEnumerable<object[]> GetTestData() =>
    LoadTestData<TestCase>("test_data.json");
```

---

## Configuration Reference

### Complete appsettings.json Example

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/squadnet-.log",
          "rollingInterval": "Day"
        }
      }
    ]
  },
  "Filtering": {
    "IsEnabled": true,
    "ExcludePatterns": [
      "^LogNet:",
      "^LogSquad: Warning:",
      "^LogTemp:"
    ]
  },
  "AdminSources": {
    "Type": "Sftp",
    "Sources": [
      {
        "Type": "Sftp",
        "Sftp": {
          "Host": "example.com",
          "Port": 2022,
          "User": "admin",
          "Password": "password",
          "AdminListPath": "/path/to/Admins.cfg"
        }
      }
    ]
  },
  "LogReaders": {
    "Type": "Tail",
    "Tail": {
      "FilePath": "/path/to/SquadGame.log"
    },
    "Ftp": {
      "Host": "ftp.example.com",
      "Port": 21,
      "User": "ftpuser",
      "Password": "ftppass",
      "LogFilePath": "/logs/SquadGame.log"
    },
    "Sftp": {
      "Host": "sftp.example.com",
      "Port": 2022,
      "User": "sftpuser",
      "Password": "sftppass",
      "LogFilePath": "/logs/SquadGame.log"
    }
  },
  "Rcon": {
    "Host": "127.0.0.1",
    "Port": "21114",
    "Password": "rconpassword"
  },
  "Database": {
    "Provider": "Sqlite",
    "ConnectionString": "Data Source=squadnet.db"
  }
}
```

---

## Event Type Reference

### Player Events
- `PlayerConnected` - Player connecting to server
- `PlayerJoinSucceeded` - Player successfully joined
- `PlayerDisconnected` - Player left server
- `PlayerDied` - Player killed
- `PlayerWounded` - Player wounded (incapacitated)
- `PlayerRevived` - Player revived from wounded
- `PlayerDamaged` - Player took damage
- `PlayerTeamChanged` - Player switched teams
- `PlayerSquadChanged` - Player joined/left squad
- `PlayerPossess` - Player spawned/possessed character

### Chat Events
- `ChatMessage` - Player chat message
- `AdminBroadcast` - Admin broadcast message

### Round/Match Events
- `RoundTickets` - Ticket count update
- `RoundWinner` - Round ended with winner
- `GameStarted` - Match started
- `GameEnded` - Match ended
- `NewGame` - New game/layer loaded

### Admin Events
- `PlayerKicked` - Player kicked by admin
- `PlayerBanned` - Player banned by admin
- `PlayerWarned` - Player warned by admin

### Team/Squad Events
- `SquadCreated` - New squad created
- `SquadDisbanded` - Squad disbanded (not implemented)

### Server Events
- `ServerTickRateUpdated` - Server tick rate changed
- `LayerInfoUpdated` - Map/layer information updated

### And 20+ more event types...

Full list in: `SquadNET.Core/Squad/Events/SquadEventType.cs`

---

## RCON Command Reference

### Server Information
- `ShowServerInfo` - Get server name, version, players
- `ShowCurrentMap` - Get current map/layer
- `ShowNextMap` - Get next map/layer

### Player Lists
- `ListPlayers` - List all connected players
- `ListSquads` - List all squads and members
- `ListLevels` - List available maps
- `ListLayers` - List available layers
- `ListAdmins` - List online admins
- `ListDisconnectedPlayers` - Recently disconnected players

### Admin Actions
- `AdminWarn <player> <reason>` - Warn a player
- `AdminKick <player> <reason>` - Kick a player
- `AdminKickById <id> <reason>` - Kick by player ID
- `AdminBan <player> <duration> <reason>` - Ban a player
- `AdminBanById <id> <duration> <reason>` - Ban by player ID
- `AdminBroadcast <message>` - Broadcast to all players

### Match Control
- `AdminEndMatch` - End current match
- `AdminRestartMatch` - Restart current match
- `AdminChangeLayer <layer>` - Change to specific layer
- `AdminSetNextLayer <layer>` - Set next layer

### Server Settings
- `AdminSetMaxNumPlayers <count>` - Set max player count
- `AdminSetServerPassword <password>` - Set server password
- `AdminSlomo <speed>` - Change game speed (dev/testing)

### And 30+ more commands...

Full list in: `SquadNET.Core/Squad/Commands/SquadCommand.cs`

---

## Troubleshooting

### Parser Not Matching

**Problem:** Parser returns null for valid log line.

**Solutions:**
1. Test regex at https://regex101.com/ with sample log line
2. Check that regex uses named groups: `(?<groupName>pattern)`
3. Ensure model property names match group names (case-insensitive)
4. Verify `[RegexPattern]` attribute is on the model class
5. Check parser is registered in DI container

### Events Not Firing in Plugins

**Problem:** Plugin `OnEventRaised` not being called.

**Solutions:**
1. Verify plugin DLL is in `/plugins` folder
2. Check plugin inherits from `Plugin` base class
3. Ensure `PluginManager` is registered and initialized
4. Check log filtering isn't excluding relevant log lines
5. Verify log reader is connected and receiving lines

### RCON Commands Failing

**Problem:** RCON commands return null or timeout.

**Solutions:**
1. Verify RCON host/port/password in appsettings.json
2. Check firewall allows connection to RCON port
3. Ensure `IRconService.Connect()` was called
4. Subscribe to `OnExceptionThrown` event for error details
5. Test RCON connection manually with another tool

### Database Migrations Failing

**Problem:** EF migrations fail or database not updating.

**Solutions:**
1. Ensure database provider is set correctly in appsettings.json
2. Check connection string is valid
3. Run `dotnet ef database update` with correct startup project
4. For SQLite, ensure file path is writable
5. For SQL Server, verify connection string and permissions

### Log Reader Not Reading

**Problem:** Log reader starts but no lines being processed.

**Solutions:**
1. **Tail:** Verify file path exists and is accessible
2. **FTP/SFTP:** Check host, port, credentials
3. **FTP/SFTP:** Ensure log file path on server is correct
4. Check `OnError` event for connection errors
5. Verify polling interval (default 5s for FTP/SFTP)
6. For FTP/SFTP, check network connectivity and firewall

---

## Best Practices for AI Assistants

### When Adding Features

1. **Always read existing code first** - Understand patterns before implementing
2. **Follow existing conventions** - Match naming, structure, and patterns
3. **Add tests** - Every new parser or feature needs tests
4. **Update this file** - Keep CLAUDE.md current with changes
5. **Use MediatR** - All business logic goes through MediatR handlers
6. **Leverage DI** - Register all services in extension methods

### When Debugging

1. **Check logs first** - Serilog captures detailed information
2. **Use test data** - Create minimal test cases in JSON
3. **Test parsers in isolation** - Don't require full service to test parsing
4. **Subscribe to events** - Most failures emit events (OnError, OnExceptionThrown)
5. **Verify configuration** - Many issues stem from appsettings.json

### When Refactoring

1. **Don't break existing parsers** - 40+ parsers rely on current infrastructure
2. **Maintain backward compatibility** - Plugins depend on stable interfaces
3. **Update all affected tests** - Test coverage is comprehensive
4. **Document breaking changes** - Update this file and commit messages

### Code Review Checklist

Before committing changes, verify:

- [ ] All new classes have XML documentation comments
- [ ] New features have corresponding tests
- [ ] Tests pass locally (`dotnet test`)
- [ ] No hardcoded credentials or secrets
- [ ] Configuration is in appsettings.json, not code
- [ ] Async methods use async/await properly
- [ ] Disposal of resources (IDisposable)
- [ ] Error handling and logging in place
- [ ] Dependency injection over direct instantiation
- [ ] Follows existing naming conventions

---

## Quick Reference

### Build & Run

```bash
# Build entire solution
dotnet build

# Run tests
dotnet test

# Run monitoring service
cd SquadNET.SquadMonitoringService
dotnet run

# Run with specific config
dotnet run --environment Production
```

### Common Git Workflow

```bash
# Create feature branch
git checkout -b feature/my-feature

# Make changes, commit
git add .
git commit -m "Add feature X"

# Push and create PR
git push -u origin feature/my-feature
```

### Database Migrations

```bash
# Add migration
dotnet ef migrations add MigrationName --project SquadNET.Application

# Update database
dotnet ef database update --project SquadNET.Application

# Remove last migration (if not applied)
dotnet ef migrations remove --project SquadNET.Application
```

---

## Additional Resources

- **Project Repository:** https://github.com/carmc99/SquadNet
- **SquadJS (Inspiration):** https://github.com/Team-Silver-Sphere/SquadJS
- **Squad Server Documentation:** https://squad.fandom.com/wiki/Server_Configuration

---

## Changelog

### 2026-01-17
- Initial CLAUDE.md creation
- Documented complete architecture and conventions
- Added comprehensive development workflows
- Included troubleshooting guide and best practices

---

**Note for AI Assistants:** This document is a living guide. When making significant changes to the codebase architecture, update this file to reflect the current state. Keep it as a single source of truth for working with SquadNet.
