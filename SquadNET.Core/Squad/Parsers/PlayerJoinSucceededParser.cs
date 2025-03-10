// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerJoinSucceededParser : IParser<PlayerJoinSucceededEventModel>
    {
        private static readonly Regex PlayerJoinSucceededRegex = RegexPatternHelper.GetRegex<PlayerJoinSucceededEventModel>();

        public PlayerJoinSucceededEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerJoinSucceededRegex.Match(input);

            if (!match.Success || match.Groups.Count < 4)
            {
                ParserLogger.LogInvalidInput(nameof(PlayerJoinSucceededParser), input);
                return null;
            }

            string time = (match.Groups[1].Value.NormalizeTime());

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", time },
                { "ChainID", match.Groups[2].Value },
                { "PlayerName", match.Groups[3].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<PlayerJoinSucceededEventModel>(parsedValues);
        }
    }
}