using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core
{
    public static class RconPacketType
    {
        public const int ServerDataAuth = 0x03;
        public const int ServerDataAuthResponse = 0x02;
        public const int ServerDataExecCommand = 0x02;
        public const int ServerDataResponseValue = 0x00;
        public const int ServerDataChatMessage = 0x01;
    }
}
