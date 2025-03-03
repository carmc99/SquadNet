// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Rcon;

namespace SquadNET.Core.Squad.Commands
{
    public class SquadCommandTemplate : Command<SquadCommand>
    {
        public SquadCommandTemplate()
        {
            CommandTemplates.Add(SquadCommand.ShowServerInfo, "ShowServerInfo");
            CommandTemplates.Add(SquadCommand.ShowCurrentMap, "ShowCurrentMap");
            CommandTemplates.Add(SquadCommand.ShowNextMap, "ShowNextMap");
            CommandTemplates.Add(SquadCommand.ListCommands, "ListCommands true");
            CommandTemplates.Add(SquadCommand.ListSquads, "ListSquads");
            CommandTemplates.Add(SquadCommand.ListPlayers, "ListPlayers");
            CommandTemplates.Add(SquadCommand.ListLevels, "ListLevels");
            CommandTemplates.Add(SquadCommand.ListLayers, "ListLayers");

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
            CommandTemplates.Add(SquadCommand.SetNumReservedSlots, "AdminSetNumReservedSlots {0}");
            CommandTemplates.Add(SquadCommand.SetPublicQueueLimit, "AdminSetPublicQueueLimit {0}");
            CommandTemplates.Add(SquadCommand.SetFogOfWar, "AdminSetFogOfWar {0}");
            CommandTemplates.Add(SquadCommand.ForceAllVehicleAvailability, "AdminForceAllVehicleAvailability {0}");
            CommandTemplates.Add(SquadCommand.ForceAllDeployableAvailability, "AdminForceAllDeployableAvailability {0}");
            CommandTemplates.Add(SquadCommand.ForceAllRoleAvailability, "AdminForceAllRoleAvailability {0}");
            CommandTemplates.Add(SquadCommand.ForceAllActionAvailability, "AdminForceAllActionAvailability {0}");
            CommandTemplates.Add(SquadCommand.NoTeamChangeTimer, "AdminNoTeamChangeTimer {0}");
            CommandTemplates.Add(SquadCommand.DisableVehicleClaiming, "AdminDisableVehicleClaiming {0}");
            CommandTemplates.Add(SquadCommand.DisableVehicleTeamRequirement, "AdminDisableVehicleTeamRequirement {0}");
            CommandTemplates.Add(SquadCommand.DisableVehicleKitRequirement, "AdminDisableVehicleKitRequirement {0}");
            CommandTemplates.Add(SquadCommand.AlwaysValidPlacement, "AdminAlwaysValidPlacement {0}");
            CommandTemplates.Add(SquadCommand.AddCameraman, "AdminAddCameraman \"{0}\""); //TODO: No existe en rcon
        }
    }
}