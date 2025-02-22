namespace SquadNET.Core.Squad.Events
{
    /// <summary>
    /// Enumeración de eventos que pueden ser emitidos por el sistema.
    /// </summary>
    public enum LogEventType
    {
        CHAT_MESSAGE,
        POSSESSED_ADMIN_CAMERA,
        UNPOSSESSED_ADMIN_CAMERA,
        RCON_ERROR,
        ADMIN_BROADCAST,
        DEPLOYABLE_DAMAGED,
        NEW_GAME,
        PLAYER_CONNECTED,
        PLAYER_DISCONNECTED,
        PLAYER_DAMAGED,
        PLAYER_WOUNDED,
        PLAYER_DIED,
        PLAYER_REVIVED,
        TEAMKILL,
        PLAYER_POSSESS,
        PLAYER_UNPOSSESS,
        TICK_RATE,
        PLAYER_TEAM_CHANGE,
        PLAYER_SQUAD_CHANGE,
        UPDATED_PLAYER_INFORMATION,
        UPDATED_LAYER_INFORMATION,
        UPDATED_A2S_INFORMATION,
        PLAYER_AUTO_KICKED,
        PLAYER_WARNED,
        PLAYER_KICKED,
        PLAYER_BANNED,
        SQUAD_CREATED
    }
}
