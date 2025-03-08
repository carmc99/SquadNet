// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Parsers
{
    internal class RoundEndedParser : IParser<RoundEndedEventModel>
    {
        private static readonly Regex RoundEndedRegex = RegexPatternHelper.GetRegex<RoundEndedEventModel>();

        public RoundEndedEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = RoundEndedRegex.Match(input);

            if (!match.Success || match.Groups.Count < 3)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<RoundEndedEventModel>(parsedValues);
        }
    }
}