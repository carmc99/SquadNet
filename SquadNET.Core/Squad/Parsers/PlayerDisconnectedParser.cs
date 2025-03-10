// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerDisconnectedParser : IParser<PlayerDisconnectedEventModel>
    {
        private static readonly Regex PlayerDisconnectedRegex = RegexPatternHelper.GetRegex<PlayerDisconnectedEventModel>();

        public PlayerDisconnectedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerDisconnectedRegex.Match(input);

            if (!match.Success || match.Groups.Count < 7)
            {
                ParserLogger.LogInvalidInput(nameof(PlayerDisconnectedParser), input);
                return null;
            }

            bool isServer = match.Groups[4].Value == "YES";

            Dictionary<string, string> parsedValues = new()
            {
                { "RemoteAddress", match.Groups[1].Value },
                { "ConnectionName", match.Groups[2].Value },
                { "Driver", match.Groups[3].Value },
                { "IsServer", isServer.ToString() },
                { "PlayerController", match.Groups[5].Value },
                { "Owner", match.Groups[6].Value },
                { "EosId", match.Groups[7].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<PlayerDisconnectedEventModel>(parsedValues);
        }
    }
}