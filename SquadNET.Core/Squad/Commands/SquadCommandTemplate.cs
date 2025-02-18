using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Commands
{
    public class SquadCommandTemplate : Command<SquadCommand>
    {
        public SquadCommandTemplate()
        {
            CommandTemplates.Add(SquadCommand.WarnPlayer, "AdminWarn \"{0}\" {1}");
            CommandTemplates.Add(SquadCommand.KickPlayer, "AdminKick \"{0}\" {1}");
            CommandTemplates.Add(SquadCommand.BanPlayer, "AdminBan \"{0}\" {1} {2}");
            CommandTemplates.Add(SquadCommand.BroadcastMessage, "AdminBroadcast {0}");
            CommandTemplates.Add(SquadCommand.SetFogOfWar, "AdminSetFogOfWar {0}");
            CommandTemplates.Add(SquadCommand.ExecuteRaw, "{0}");
        }
    }
}
