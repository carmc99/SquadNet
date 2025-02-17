using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Events
{
    public class LogEvents
    {
        public const string CHAT_MESSAGE = "CHAT_MESSAGE";
        public const string POSSESSED_ADMIN_CAMERA = "POSSESSED_ADMIN_CAMERA";
        public const string UNPOSSESSED_ADMIN_CAMERA = "UNPOSSESSED_ADMIN_CAMERA";
        public const string RCON_ERROR = "RCON_ERROR";
        public const string ADMIN_BROADCAST = "ADMIN_BROADCAST";
        public const string DEPLOYABLE_DAMAGED = "DEPLOYABLE_DAMAGED";
        public const string NEW_GAME = "NEW_GAME";
        public const string PLAYER_CONNECTED = "PLAYER_CONNECTED";
        public const string PLAYER_DISCONNECTED = "PLAYER_DISCONNECTED";
        public const string PLAYER_DAMAGED = "PLAYER_DAMAGED";
        public const string PLAYER_WOUNDED = "PLAYER_WOUNDED";
        public const string PLAYER_DIED = "PLAYER_DIED";
        public const string PLAYER_REVIVED = "PLAYER_REVIVED";
        public const string TEAMKILL = "TEAMKILL";
        public const string PLAYER_POSSESS = "PLAYER_POSSESS";
        public const string PLAYER_UNPOSSESS = "PLAYER_UNPOSSESS";
        public const string TICK_RATE = "TICK_RATE";
        public const string PLAYER_TEAM_CHANGE = "PLAYER_TEAM_CHANGE";
        public const string PLAYER_SQUAD_CHANGE = "PLAYER_SQUAD_CHANGE";
        public const string UPDATED_PLAYER_INFORMATION = "UPDATED_PLAYER_INFORMATION";
        public const string UPDATED_LAYER_INFORMATION = "UPDATED_LAYER_INFORMATION";
        public const string UPDATED_A2S_INFORMATION = "UPDATED_A2S_INFORMATION";
        public const string PLAYER_AUTO_KICKED = "PLAYER_AUTO_KICKED";
        public const string PLAYER_WARNED = "PLAYER_WARNED";
        public const string PLAYER_KICKED = "PLAYER_KICKED";
        public const string PLAYER_BANNED = "PLAYER_BANNED";
        public const string SQUAD_CREATED = "SQUAD_CREATED";
    }
}
