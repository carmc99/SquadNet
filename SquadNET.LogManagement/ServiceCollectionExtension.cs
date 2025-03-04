// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;
using SquadNET.LogManagement.LogReaders;

namespace SquadNET.LogManagement
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLogManagement(this IServiceCollection services)
        {
            services.AddKeyedScoped<ILogReader, TailLogReader>(LogReaderType.Tail);
            services.AddKeyedScoped<ILogReader, FtpLogReader>(LogReaderType.Ftp);
            services.AddKeyedScoped<ILogReader, SftpLogReader>(LogReaderType.Sftp);

            services.AddScoped<ILogReaderFactory, LogReaderFactory>();
            return services;
        }
    }
}