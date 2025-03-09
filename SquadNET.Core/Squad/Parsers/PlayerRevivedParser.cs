// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerRevivedParser : IParser<PlayerRevivedEventModel>
    {
        private static readonly Regex PlayerRevivedRegex = RegexPatternHelper.GetRegex<PlayerRevivedEventModel>();

        public PlayerRevivedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerRevivedRegex.Match(input);

            if (!match.Success || match.Groups.Count < 7)
            {
                ParserLogger.LogInvalidInput(nameof(PlayerRevivedParser), input);
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "ReviverName", match.Groups[3].Value },
                { "VictimName", match.Groups[5].Value }
            };

            PlayerRevivedEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerRevivedEventModel>(parsedValues);
            model.ReviverIds = CreatorOnlineModel.FromString(match.Groups[4].Value);
            model.VictimIds = CreatorOnlineModel.FromString(match.Groups[6].Value);

            return model;
        }
    }
}