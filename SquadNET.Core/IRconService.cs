namespace SquadNET.Core
{
    public interface IRconService
    {
        void Connect();
        void Disconnect();

        Task<string> ExecuteCommandAsync<T>(Command<T> command, T commandType, params object[] args) where T : Enum;
    }
}
