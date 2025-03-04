// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.LogManagement
{
    public class LogReaderFactory : ILogReaderFactory
    {
        private readonly IServiceProvider ServiceProvider;

        public LogReaderFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public ILogReader Create(LogReaderType type)
        {
            return ServiceProvider.GetKeyedService<ILogReader>(type);
        }
    }
}