using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            foreach (string id in match.Groups[4].Value.Split('|'))
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
