using SquadNET.Core.Squad;
using SquadNET.Core.Squad.Entities;
using System.Net.Sockets;

namespace SquadNET.Core
{
    public interface IRconService
    {
        public event Action Connected;

        public event Action<Packet> PacketReceived;

        public event Action<ChatMessageInfo> ChatMessageReceived;

        public event Action<Exception> ExceptionThrown;

        public event Action<byte[]> BytesReceived;
        void Connect();
        void Disconnect();

        Task<string> ExecuteCommandAsync<T>(Command<T> command, T commandType, params object[] args) where T : Enum;
    }
}
