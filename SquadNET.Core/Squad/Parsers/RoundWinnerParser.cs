using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Parsers
{
    internal class RoundWinnerParser : IParser<RoundWinnerEventModel>
    {
        private static readonly Regex RoundWinnerRegex = RegexPatternHelper.GetRegex<RoundWinnerEventModel>();

        public RoundWinnerEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = RoundWinnerRegex.Match(input);

            if (!match.Success || match.Groups.Count < 5)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "Winner", match.Groups[3].Value },
                { "Layer", match.Groups[4].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<RoundWinnerEventModel>(parsedValues);
        }
    }
}
