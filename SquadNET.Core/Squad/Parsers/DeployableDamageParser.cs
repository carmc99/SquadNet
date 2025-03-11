// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class DeployableDamageParser : IParser<DeployableDamagedEventModel>
    {
        private static readonly Regex DeployableDamageRegex = RegexPatternHelper.GetRegex<DeployableDamagedEventModel>();

        public DeployableDamagedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = DeployableDamageRegex.Match(input);

            if (!match.Success || match.Groups.Count < 8)
            {
                ParserLogger.LogInvalidInput(nameof(DeployableDamageParser), input);
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "Deployable", match.Groups[3].Value },
                { "Damage", match.Groups[4].Value },
                { "Weapon", match.Groups[5].Value },
                { "AttackerName", match.Groups[6].Value },
                { "DamageType", match.Groups[7].Value },
                { "HealthRemaining", match.Groups[8].Value }
            };

            DeployableDamagedEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<DeployableDamagedEventModel>(parsedValues);

            return model;
        }
    }
}