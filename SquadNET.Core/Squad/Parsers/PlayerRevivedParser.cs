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
            string time = NormalizeTime(match.Groups[1].Value);

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", time },
                { "ChainID", match.Groups[2].Value },
                { "ReviverName", match.Groups[3].Value },
                { "VictimName", match.Groups[6].Value }
            };

            PlayerRevivedEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerRevivedEventModel>(parsedValues);
            model.ReviverIds = CreatorOnlineModel.FromString($"EOS: {match.Groups[4].Value} steam: {match.Groups[5].Value}");
            model.VictimIds = CreatorOnlineModel.FromString($"EOS: {match.Groups[7].Value} steam: {match.Groups[8].Value}");

            return model;
        }

        private static string NormalizeTime(string time)
        {
            string[] parts = time.Split('-');
            if (parts.Length != 2)
            {
                return time;
            }

            string datePart = parts[0];
            string timePart = parts[1];

            timePart = timePart.Replace('.', ':');

            return $"{datePart}-{timePart}";
        }
    }
}