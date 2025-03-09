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

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "PlayerSuffix", match.Groups[3].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<PlayerJoinSucceededEventModel>(parsedValues);
        }
    }
}