// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerWoundedParser : IParser<PlayerWoundedEventModel>
    {
        private static readonly Regex PlayerWoundedRegex = RegexPatternHelper.GetRegex<PlayerWoundedEventModel>();

        public PlayerWoundedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerWoundedRegex.Match(input);

            if (!match.Success || match.Groups.Count < 7)
            {
                ParserLogger.LogInvalidInput(nameof(PlayerWoundedParser), input);
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "VictimName", match.Groups[1].Value },
                { "KillingDamage", match.Groups[2].Value },
                { "KillerName", match.Groups[3].Value },
                { "KillerEosId", match.Groups[4].Value },
                { "KillerSteamId", match.Groups[5].Value },
                { "KillerControllerId", match.Groups[6].Value },
                { "Weapon", match.Groups[7].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<PlayerWoundedEventModel>(parsedValues);
        }
    }
}