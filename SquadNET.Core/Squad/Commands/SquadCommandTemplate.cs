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
            CommandTemplates.Add(SquadCommand.WarnPlayerById, "AdminWarnById {0} {1}");
            CommandTemplates.Add(SquadCommand.KickPlayer, "AdminKick \"{0}\" {1}");
            CommandTemplates.Add(SquadCommand.KickPlayerById, "AdminKickById {0} {1}");
            CommandTemplates.Add(SquadCommand.BanPlayer, "AdminBan \"{0}\" {1} {2}");
            CommandTemplates.Add(SquadCommand.BanPlayerById, "AdminBanById {0} {1} {2}");
            CommandTemplates.Add(SquadCommand.BroadcastMessage, "AdminBroadcast {0}");
            CommandTemplates.Add(SquadCommand.EndMatch, "AdminEndMatch");
            CommandTemplates.Add(SquadCommand.ChangeLayer, "AdminChangeLayer {0}");
            CommandTemplates.Add(SquadCommand.SetNextLayer, "AdminSetNextLayer {0}");
            CommandTemplates.Add(SquadCommand.SetMaxNumPlayers, "AdminSetMaxNumPlayers {0}");
            CommandTemplates.Add(SquadCommand.SetServerPassword, "AdminSetServerPassword {0}");
            CommandTemplates.Add(SquadCommand.Slomo, "AdminSlomo {0}");
            CommandTemplates.Add(SquadCommand.ForceTeamChange, "AdminForceTeamChange \"{0}\"");
            CommandTemplates.Add(SquadCommand.ForceTeamChangeById, "AdminForceTeamChangeById {0}");
            CommandTemplates.Add(SquadCommand.ListDisconnectedPlayers, "AdminListDisconnectedPlayers");
            CommandTemplates.Add(SquadCommand.DemoteCommander, "AdminDemoteCommander \"{0}\"");
            CommandTemplates.Add(SquadCommand.DemoteCommanderById, "AdminDemoteCommanderById {0}");
            CommandTemplates.Add(SquadCommand.DisbandSquad, "AdminDisbandSquad {0} {1}");
            CommandTemplates.Add(SquadCommand.RemovePlayerFromSquad, "AdminRemovePlayerFromSquad \"{0}\"");
            CommandTemplates.Add(SquadCommand.RemovePlayerFromSquadById, "AdminRemovePlayerFromSquadById {0}");
            CommandTemplates.Add(SquadCommand.RestartMatch, "AdminRestartMatch");
            CommandTemplates.Add(SquadCommand.ExecuteRaw, "{0}");
        }
    }
}
