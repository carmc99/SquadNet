using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Core.Squad.Parsers
{
    internal class RoundTicketsParser : IParser<RoundTicketsEventModel>
    {
        private static readonly Regex RoundTicketsRegex = RegexPatternHelper.GetRegex<RoundTicketsEventModel>();

        public RoundTicketsEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = RoundTicketsRegex.Match(input);

            if (!match.Success || match.Groups.Count < 10)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "Team", match.Groups[3].Value },
                { "Subfaction", match.Groups[4].Value },
                { "Faction", match.Groups[5].Value },
                { "Action", match.Groups[6].Value },
                { "Tickets", match.Groups[7].Value },
                { "Layer", match.Groups[8].Value },
                { "Level", match.Groups[9].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<RoundTicketsEventModel>(parsedValues);
        }
    }
}
