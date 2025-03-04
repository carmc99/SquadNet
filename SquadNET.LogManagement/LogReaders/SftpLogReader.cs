// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.Configuration;
using Renci.SshNet;

namespace SquadNET.LogManagement.LogReaders
{
    public class SftpLogReader : ILogReader
    {
        private readonly string remoteFilePath;
        private readonly SftpClient sftpClient;
        private long lastPosition = 0;

        public SftpLogReader(IConfiguration configuration)
        {
            string host = configuration["LogReaders:Sftp:Host"];
            int port = int.TryParse(configuration["LogReaders:Sftp:Port"], out int parsedPort) ? parsedPort : 22;
            string user = configuration["LogReaders:Sftp:User"];
            string password = configuration["LogReaders:Sftp:Password"];
            remoteFilePath = configuration["LogReaders:Sftp:RemoteFilePath"];

            var connectionInfo = new ConnectionInfo(host, port, user,
                new PasswordAuthenticationMethod(user, password));

            sftpClient = new SftpClient(connectionInfo);
        }

        public event Action OnConnectionLost;

        public event Action OnConnectionRestored;

        public event Action<string> OnError;

        public event Action OnFileCreated;

        public event Action OnFileDeleted;

        public event Action OnFileRenamed;

        public event Action<string> OnLogLine;

        public event Action OnWatchStarted;

        public event Action OnWatchStopped;

        public Task UnwatchAsync()
        {
            sftpClient.Disconnect();
            OnWatchStopped?.Invoke();
            return Task.CompletedTask;
        }

        public async Task WatchAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                sftpClient.Connect();
                OnWatchStarted?.Invoke();

                lastPosition = sftpClient.GetAttributes(remoteFilePath).Size;

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!sftpClient.IsConnected)
                    {
                        OnConnectionLost?.Invoke();
                        sftpClient.Connect();
                        OnConnectionRestored?.Invoke();
                    }

                    long fileSize = sftpClient.GetAttributes(remoteFilePath).Size;
                    if (fileSize < lastPosition)
                    {
                        lastPosition = 0;
                    }

                    using var stream = new MemoryStream();
                    try
                    {
                        sftpClient.DownloadFile(remoteFilePath, stream);
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke($"Error al descargar el archivo de log: {ex.Message}");
                        await Task.Delay(5000, cancellationToken);
                        continue;
                    }

                    stream.Position = lastPosition; // Mover a la última posición leída
                    using var reader = new StreamReader(stream);

                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(line)) // Ignorar líneas vacías
                        {
                            OnLogLine?.Invoke(line);
                        }
                    }

                    // Actualizar lastPosition para evitar releer líneas antiguas
                    lastPosition = stream.Length;

                    await Task.Delay(5000, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error en WatchAsync: {ex.Message}");
            }
        }
    }
}