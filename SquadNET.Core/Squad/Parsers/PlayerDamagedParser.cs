using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                { "AttackerController", match.Groups[7].Value },
                { "Weapon", match.Groups[8].Value }
            };

            PlayerDamagedEventModel model = DictionaryModelConverter.ConvertDictionaryToModel<PlayerDamagedEventModel>(parsedValues);

            foreach (var id in match.Groups[6].Value.Split('|'))
            {
                string[] parts = id.Split(':');
                if (parts.Length == 2)
                {
                    model.AttackerIDs[parts[0]] = parts[1];
                }
            }

            return model;
        }
    }
}
