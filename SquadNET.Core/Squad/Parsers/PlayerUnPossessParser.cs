// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class PlayerUnPossessParser : IParser<PlayerUnPossessEventModel>
    {
        private static readonly Regex PlayerUnPossessRegex = RegexPatternHelper.GetRegex<PlayerUnPossessEventModel>();

        public PlayerUnPossessEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = PlayerUnPossessRegex.Match(input);

            if (!match.Success || match.Groups.Count < 5)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "PlayerSuffix", match.Groups[3].Value }
            };

            PlayerUnPossessEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerUnPossessEventModel>(parsedValues);

            string idsRaw = match.Groups[4].Value;
            if (idsRaw.Contains("INVALID"))
            {
                return null;
            }

            foreach (string id in idsRaw.Split('|'))
            {
                string[] parts = id.Split(':');
                if (parts.Length == 2)
                {
                    model.PlayerIDs[parts[0]] = parts[1];
                }
            }

            return model;
        }
    }
}