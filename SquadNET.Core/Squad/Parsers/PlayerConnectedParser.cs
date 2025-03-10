// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerConnectedParser : IParser<PlayerConnectedEventModel>
    {
        private static readonly Regex PlayerConnectedRegex = RegexPatternHelper.GetRegex<PlayerConnectedEventModel>();

        public PlayerConnectedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerConnectedRegex.Match(input);

            if (!match.Success || match.Groups.Count < 4)
            {
                ParserLogger.LogInvalidInput(nameof(PlayerConnectedParser), input);
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Name", match.Groups[1].Value },
                { "IP", match.Groups[2].Value },
            };

            PlayerConnectedEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerConnectedEventModel>(parsedValues);
            model.CreatorIds = CreatorOnlineModel.FromString($"EOS: {match.Groups[3].Value} steam: {match.Groups[4].Value}");

            return model;
        }
    }
}