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
        NewGameStarted,
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
