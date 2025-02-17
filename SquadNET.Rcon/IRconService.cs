using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    public interface IRconService
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<string> ExecuteCommandAsync(RconCommand command, params object[] args);
    }
}
