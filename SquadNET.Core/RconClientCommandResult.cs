// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core
{
    public class RconClientCommandResult
    {
        public readonly PacketInfo[] RequestPacketInfos;
        private readonly List<PacketInfo> PacketInfosValue = [];
        private readonly TaskCompletionSource<IReadOnlyList<PacketInfo>> TaskCompletionSource;

        public RconClientCommandResult(PacketInfo[] requestPacketInfos)
        {
            RequestPacketInfos = requestPacketInfos;
            TaskCompletionSource =
                new TaskCompletionSource<IReadOnlyList<PacketInfo>>();
        }

        public IReadOnlyList<PacketInfo> PacketInfos => PacketInfosValue;
        public Task<IReadOnlyList<PacketInfo>> Result => TaskCompletionSource.Task;

        public void AddPacketInfo(
            PacketInfo PacketInfo
        )
        {
            PacketInfosValue.Add(PacketInfo);
        }

        public void Cancel()
        {
            TaskCompletionSource.SetCanceled();
        }

        public void ClearPacketInfos()
        {
            PacketInfosValue.Clear();
        }

        public void Complete()
        {
            try
            {
                TaskCompletionSource.SetResult(PacketInfosValue);
            }
            catch
            {
                Console.WriteLine("some error occurred");
            }
        }
    }
}