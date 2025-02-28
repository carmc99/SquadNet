using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using SquadNET.LogManagement.LogReaders;

public class TailLogReaderTests
{
    private static readonly string TestDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogManagement");
    private static readonly string TestFilePath = Path.Combine(TestDirectory, "SquadGame.log");

    [Fact]
    public async Task TailLogReader_Should_Invoke_OnLogLine_When_File_Changes()
    {
        // Arrange
        Mock<IConfiguration> configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["LogReaders:Tail:FilePath"]).Returns(TestFilePath);

        TailLogReader logReader = new(configMock.Object);
        bool eventFired = false;
        logReader.OnLogLine += (line) => eventFired = true;

        // Act
        await logReader.WatchAsync();
        await Task.Delay(1000); // Give time for watcher to start

        // Create and write to the test log file
        await File.WriteAllTextAsync(TestFilePath, "Test Log Line\n");

        // Act
        await logReader.WatchAsync();
        await Task.Delay(500); // Allow time for the watcher to detect the change
        await logReader.UnwatchAsync();

        // Assert
        Assert.True(eventFired);
    }
}
