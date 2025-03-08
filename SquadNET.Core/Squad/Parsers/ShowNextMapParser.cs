// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Parsers
{
    public class ShowNextMapParser : IParser<NextMapModel>
    {
        private static readonly Regex NextMapRegex = RegexPatternHelper.GetRegex<NextMapModel>();

        public NextMapModel Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = NextMapRegex.Match(input);
            if (!match.Success || match.Groups.Count < 3)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Level", match.Groups[1].Value },
                { "Name", match.Groups[2].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<NextMapModel>(parsedValues);
        }
    }
}