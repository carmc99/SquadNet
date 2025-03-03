// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Rcon
{
    /// <summary>
    /// Enum containing all available RCON commands for Squad.
    /// </summary>
    public enum SquadCommand
    {
        /// <summary>
        /// Retrieves the name of the next map and its associated layer.
        /// </summary>
        /// <example>ShowNextMap</example>
        ShowNextMap,

        /// <summary>
        /// Retrieves the name of the current map and its associated layer.
        /// </summary>
        /// <example>ShowCurrentMap</example>
        ShowCurrentMap,

        /// <summary>Lists all available RCON commands.</summary>
        /// <example>ListCommands true</example>
        ListCommands,

        /// <summary>Lists all squads on the server.</summary>
        /// <example>ListSquads</example>
        ListSquads,

        /// <summary>Lists all players currently connected.</summary>
        /// <example>ListPlayers</example>
        ListPlayers,

        /// <summary>Lists all available levels.</summary>
        /// <example>ListLevels</example>
        ListLevels,

        /// <summary>Lists all available layers.</summary>
        /// <example>ListLayers</example>
        ListLayers,

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
        ExecuteRaw,

        /// <summary>Sets the number of reserved slots on the server.</summary>
        /// <example>SetNumReservedSlots 10</example>
        SetNumReservedSlots,

        /// <summary>Sets the public queue limit for the server.</summary>
        /// <example>SetPublicQueueLimit 5</example>
        SetPublicQueueLimit,

        /// <summary>Enables or disables the fog of war on the server.</summary>
        /// <example>SetFogOfWar true</example>
        SetFogOfWar,

        /// <summary>Forces all vehicles to be available.</summary>
        /// <example>ForceAllVehicleAvailability true</example>
        ForceAllVehicleAvailability,

        /// <summary>Forces all deployable structures to be available.</summary>
        /// <example>ForceAllDeployableAvailability true</example>
        ForceAllDeployableAvailability,

        /// <summary>Forces all roles to be available.</summary>
        /// <example>ForceAllRoleAvailability true</example>
        ForceAllRoleAvailability,

        /// <summary>Forces all actions to be available.</summary>
        /// <example>ForceAllActionAvailability true</example>
        ForceAllActionAvailability,

        /// <summary>Modifies the cooldown time for team changes.</summary>
        /// <example>NoTeamChangeTimer 30</example>
        NoTeamChangeTimer,

        /// <summary>Disables vehicle claiming, making all vehicles available.</summary>
        /// <example>DisableVehicleClaiming true</example>
        DisableVehicleClaiming,

        /// <summary>Disables the team requirement for vehicle usage.</summary>
        /// <example>DisableVehicleTeamRequirement true</example>
        DisableVehicleTeamRequirement,

        /// <summary>Disables the kit requirement for vehicle usage.</summary>
        /// <example>DisableVehicleKitRequirement true</example>
        DisableVehicleKitRequirement,

        /// <summary>Allows placement of deployables and structures regardless of normal restrictions.</summary>
        /// <example>AlwaysValidPlacement true</example>
        AlwaysValidPlacement,

        /// <summary>Adds a player as a cameraman, allowing them to use free camera mode.</summary>
        /// <example>AddCameraman "PlayerName"</example>
        AddCameraman,

        /// <summary>
        /// Retrieves detailed information about the current state of the server,
        /// including player counts, queues, map information, and game version.
        /// </summary>
        /// <example>ShowServerInfo</example>
        ShowServerInfo,
    }
}