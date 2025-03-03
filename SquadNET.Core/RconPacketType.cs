// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core
{
    public struct DecodedPacket
    {
        public string Body;
        public ushort Count;
        public byte Id;
        public int Size;
        public int Type;
    }

    public static class RconPacketType
    {
        public const byte EndPacketId = 0x02;
        public const byte MidPacketId = 0x01;
        public const int ServerDataAuth = 0x03;
        public const int ServerDataAuthResponse = 0x02;
        public const int ServerDataChatMessage = 0x01;
        public const int ServerDataExecCommand = 0x02;
        public const int ServerDataResponseValue = 0x00;
    }
}