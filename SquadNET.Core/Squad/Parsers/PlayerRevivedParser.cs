using SquadNET.Core.Squad.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            foreach (string id in match.Groups[4].Value.Split('|'))
            {
                string[] parts = id.Split(':');
                if (parts.Length == 2)
                {
                    model.ReviverIDs[parts[0]] = parts[1];
                }
            }

            foreach (string id in match.Groups[6].Value.Split('|'))
            {
                string[] parts = id.Split(':');
                if (parts.Length == 2)
                {
                    model.VictimIDs[parts[0]] = parts[1];
                }
            }

            return model;
        }
    }
}
