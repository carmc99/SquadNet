using SquadNET.Core.Squad;
using SquadNET.Core.Squad.Entities;
using System.Net.Sockets;

namespace SquadNET.Core
{
    public interface IRconService
    {
        public event Action OnConnected;

        public event Action<Packet> OnPacketReceived;

        public event Action<ChatMessageInfo> OnChatMessageReceived;

        public event Action<Exception> OnExceptionThrown;

        public event Action<byte[]> OnBytesReceived;
        void Connect();
        void Disconnect();

        Task<string> ExecuteCommandAsync<T>(Command<T> command, T commandType, params object[] args) where T : Enum;
    }
}
