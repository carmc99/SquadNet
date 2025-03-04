// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core
{
    public abstract class Command<T>
    {
        protected readonly Dictionary<T, string> CommandTemplates = [];

        public string GetFormattedCommand(T command, params object[] args)
        {
            if (!CommandTemplates.ContainsKey(command))
            {
                throw new InvalidOperationException($"The command '{command}' is not defined in the template.");
            }
            return string.Format(CommandTemplates[command], args);
        }
    }
}