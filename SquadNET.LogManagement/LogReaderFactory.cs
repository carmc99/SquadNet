using Microsoft.Extensions.DependencyInjection;
using SquadNET.LogManagement.LogReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
