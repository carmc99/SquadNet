using SquadNET.Core.Squad.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Parsers
{
    internal class AdminBroadcastParser : IParser<AdminBroadcastEventModel>
    {
        private static readonly Regex AdminBroadcastRegex = RegexPatternHelper.GetRegex<AdminBroadcastEventModel>();

        public AdminBroadcastEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = AdminBroadcastRegex.Match(input);

            if (!match.Success || match.Groups.Count < 5)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "Message", match.Groups[3].Value },
                { "From", match.Groups[4].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<AdminBroadcastEventModel>(parsedValues);
        }
    }
}
