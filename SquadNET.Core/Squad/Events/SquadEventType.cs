// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events
{
    /// <summary>
    /// Enumeración de eventos que pueden ser emitidos por el sistema.
    /// </summary>
    public enum SquadEventType
    {
        ChatMessage,
        AdminCameraPossessed,
        AdminCameraUnpossessed,
        RconError,
        AdminBroadcast,
        DeployableDamaged,
        GameStarted,
        GameEnded,
        RoundTickets,
        RoundWinner,
        PlayerConnected,
        PlayerDisconnected,
        PlayerDamaged,
        PlayerWounded,
        PlayerDied,
        PlayerRevived,
        TeamKill,
        PlayerPossessed,
        PlayerUnpossessed,
        ServerTickRateUpdated,
        PlayerTeamChanged,
        PlayerSquadChanged,
        PlayerInfoUpdated,
        LayerInfoUpdated,
        ServerQueryInfoUpdated,
        PlayerAutoKicked,
        PlayerWarned,
        PlayerKicked,
        PlayerBanned,
        SquadCreated
    }
}