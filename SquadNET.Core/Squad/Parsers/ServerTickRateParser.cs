using SquadNET.Core.Squad.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ServerTickRateParser : IParser<ServerTickRateEventModel>
    {
        private static readonly Regex ServerTickRateRegex = RegexPatternHelper.GetRegex<ServerTickRateEventModel>();

        public ServerTickRateEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = ServerTickRateRegex.Match(input);

            if (!match.Success || match.Groups.Count < 4)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "TickRate", match.Groups[3].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<ServerTickRateEventModel>(parsedValues);
        }
    }
}
