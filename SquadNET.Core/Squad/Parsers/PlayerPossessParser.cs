// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
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
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "PlayerSuffix", match.Groups[3].Value },
                { "PossessClassname", match.Groups[5].Value }
            };

            PlayerPossessEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerPossessEventModel>(parsedValues);
            model.CreatorIds = CreatorOnlineIds.FromString(match.Groups[4].Value);

            return model;
        }
    }
}