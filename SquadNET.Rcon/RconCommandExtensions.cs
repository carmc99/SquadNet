using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    public static class RconCommandExtensions
    {
        private static readonly Dictionary<RconCommand, string> CommandTemplates = new()
        {
            { RconCommand.WarnPlayer, "AdminWarn \"{0}\" {1}" },
            { RconCommand.KickPlayer, "AdminKick \"{0}\" {1}" },
            { RconCommand.BanPlayer, "AdminBan \"{0}\" {1} {2}" },
            { RconCommand.BroadcastMessage, "AdminBroadcast {0}" },
            { RconCommand.SetFogOfWar, "AdminSetFogOfWar {0}" },
            { RconCommand.ExecuteRaw, "{0}" }
        };

        public static string GetFormattedCommand(this RconCommand command, params object[] args)
        {
            return string.Format(CommandTemplates[command], args);
        }
    }
}
