// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    public class ShowCurrentMapParser : IParser<CurrentMapModel>
    {
        private static readonly Regex CurrentMapRegex = RegexPatternHelper.GetRegex<CurrentMapModel>();

        public CurrentMapModel Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = CurrentMapRegex.Match(input);
            if (!match.Success || match.Groups.Count < 3)
            {
                ParserLogger.LogInvalidInput(nameof(ShowCurrentMapParser), input);
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Level", match.Groups[1].Value },
                { "Name", match.Groups[2].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<CurrentMapModel>(parsedValues);
        }
    }
}