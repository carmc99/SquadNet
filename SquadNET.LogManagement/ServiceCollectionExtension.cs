using Microsoft.Extensions.DependencyInjection;
using SquadNET.LogManagement.LogReaders;

namespace SquadNET.LogManagement
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLogManagement(this IServiceCollection services)
        {
            services.AddKeyedSingleton<ILogReader, TailLogReader>(LogReaderType.Tail);
            services.AddKeyedSingleton<ILogReader, FtpLogReader>(LogReaderType.Ftp);
            services.AddKeyedSingleton<ILogReader, SftpLogReader>(LogReaderType.Sftp);

            services.AddSingleton<ILogReaderFactory, LogReaderFactory>();
            return services;
        }
    }
}
