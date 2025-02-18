using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    /// <summary>
    /// Enum containing all available RCON commands for Squad.
    /// </summary>
    public enum SquadCommand
    {
        /// <summary>Warns a player with a specified reason.</summary>
        /// <example>AdminWarn "PlayerName" "Reason"</example>
        WarnPlayer,

        /// <summary>Warns a player by ID with a specified reason.</summary>
        /// <example>AdminWarnById 75 "Reason"</example>
        WarnPlayerById,

        /// <summary>Kicks a player from the server with a specified reason.</summary>
        /// <example>AdminKick "PlayerName" "Spamming"</example>
        KickPlayer,

        /// <summary>Kicks a player by ID from the server with a specified reason.</summary>
        /// <example>AdminKickById 75 "Spamming"</example>
        KickPlayerById,

        /// <summary>Bans a player from the server for a specific duration.</summary>
        /// <example>AdminBan "PlayerName" "1M" "Cheating"</example>
        BanPlayer,

        /// <summary>Bans a player by ID from the server for a specific duration.</summary>
        /// <example>AdminBanById 75 "1M" "Cheating"</example>
        BanPlayerById,

        /// <summary>Sends a message to all players on the server.</summary>
        /// <example>AdminBroadcast "Important message"</example>
        BroadcastMessage,

        /// <summary>Ends the current match immediately.</summary>
        /// <example>AdminEndMatch</example>
        EndMatch,

        /// <summary>Changes the current layer and travels to the specified one.</summary>
        /// <example>AdminChangeLayer "LayerName"</example>
        ChangeLayer,

        /// <summary>Sets the next layer to be used after the current match.</summary>
        /// <example>AdminSetNextLayer "LayerName"</example>
        SetNextLayer,

        /// <summary>Sets the maximum number of players allowed on the server.</summary>
        /// <example>AdminSetMaxNumPlayers 80</example>
        SetMaxNumPlayers,

        /// <summary>Sets or removes the server password.</summary>
        /// <example>AdminSetServerPassword "MySecurePassword"</example>
        SetServerPassword,

        /// <summary>Adjusts the time speed on the server.</summary>
        /// <example>AdminSlomo 0.5</example>
        Slomo,

        /// <summary>Forces a player to switch teams.</summary>
        /// <example>AdminForceTeamChange "PlayerName"</example>
        ForceTeamChange,

        /// <summary>Forces a player by ID to switch teams.</summary>
        /// <example>AdminForceTeamChangeById 75</example>
        ForceTeamChangeById,

        /// <summary>Lists recently disconnected players along with their IDs and names.</summary>
        /// <example>AdminListDisconnectedPlayers</example>
        ListDisconnectedPlayers,

        /// <summary>Removes the commander role from a player.</summary>
        /// <example>AdminDemoteCommander "PlayerName"</example>
        DemoteCommander,

        /// <summary>Removes the commander role from a player by ID.</summary>
        /// <example>AdminDemoteCommanderById 75</example>
        DemoteCommanderById,

        /// <summary>Disbands a squad in a specified team.</summary>
        /// <example>AdminDisbandSquad 2 3</example>
        DisbandSquad,

        /// <summary>Removes a player from their squad without kicking them from the server.</summary>
        /// <example>AdminRemovePlayerFromSquad "PlayerName"</example>
        RemovePlayerFromSquad,

        /// <summary>Removes a player from their squad by ID without kicking them from the server.</summary>
        /// <example>AdminRemovePlayerFromSquadById 75</example>
        RemovePlayerFromSquadById,

        /// <summary>Restarts the current match.</summary>
        /// <example>AdminRestartMatch</example>
        RestartMatch,

        /// <summary>Executes a custom RCON command.</summary>
        /// <example>ExecuteRaw "SomeCustomCommand"</example>
        ExecuteRaw
    }
}
