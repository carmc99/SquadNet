// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerPossessParser : IParser<PlayerPossessEventModel>
    {
        private static readonly Regex PlayerPossessRegex = RegexPatternHelper.GetRegex<PlayerPossessEventModel>();

        public PlayerPossessEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerPossessRegex.Match(input);

            if (!match.Success || match.Groups.Count < 6)
            {
                ParserLogger.LogInvalidInput(nameof(PlayerPossessParser), input);
                return null;
            }

            string time = match.Groups[1].Value.NormalizeTime();

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", time },
                { "ChainID", match.Groups[2].Value },
                { "PlayerName", match.Groups[3].Value },
                { "PossessClassname", match.Groups[6].Success ? match.Groups[6].Value : match.Groups[5].Value }
            };

            PlayerPossessEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerPossessEventModel>(parsedValues);
            model.CreatorIds = CreatorOnlineModel.FromString($"EOS: {match.Groups[4].Value} steam: {match.Groups[5].Value}");

            return model;
        }
    }
}