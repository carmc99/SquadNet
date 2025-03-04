// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core
{
    /// <summary>
    /// Defines the contract for an RCON (Remote Console) service that manages communication with a Squad server.
    /// </summary>
    public interface IRconService
    {
        /// <summary>
        /// Event triggered when raw byte data is received from the RCON server.
        /// </summary>
        public event Action<byte[]> OnBytesReceived;

        /// <summary>
        /// Event triggered when a chat message is received through the RCON server.
        /// </summary>
        public event Action<ChatMessageInfo> OnChatMessageReceived;

        /// <summary>
        /// Event triggered when a successful connection to the RCON server is established.
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// Event triggered when an exception occurs in the RCON communication.
        /// </summary>
        public event Action<Exception> OnExceptionThrown;

        /// <summary>
        /// Event triggered when an RCON packet is received.
        /// </summary>
        public event Action<PacketInfo> OnPacketReceived;

        /// <summary>
        /// Establishes a connection to the RCON server.
        /// </summary>
        public void Connect();

        /// <summary>
        /// Executes an RCON command asynchronously.
        /// </summary>
        /// <typeparam name="T">The enumeration type representing the command.</typeparam>
        /// <param name="command">The command template used for execution.</param>
        /// <param name="commandType">The specific command to be executed.</param>
        /// <param name="args">Optional arguments for the command.</param>
        /// <returns>A task representing the asynchronous operation, returning the command response as a string.</returns>
        Task<string> ExecuteCommandAsync<T>(Command<T> command, T commandType, params object[] args) where T : Enum;
    }
}