// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerDamagedParser : IParser<PlayerDamagedEventModel>
    {
        private static readonly Regex PlayerDamagedRegex = RegexPatternHelper.GetRegex<PlayerDamagedEventModel>();

        public PlayerDamagedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerDamagedRegex.Match(input);

            if (!match.Success || match.Groups.Count < 9)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "VictimName", match.Groups[3].Value },
                { "Damage", match.Groups[4].Value },
                { "AttackerName", match.Groups[5].Value },
                { "AttackerPlayerController", match.Groups[7].Value },
                { "Weapon", match.Groups[8].Value }
            };

            PlayerDamagedEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerDamagedEventModel>(parsedValues);
            model.AttackerIds = CreatorOnlineIds.FromString(match.Groups[6].Value);

            return model;
        }
    }
}