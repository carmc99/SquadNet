using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    public class ShowNextMapParser : ICommandParser<NextMapInfo>
    {
        private static readonly Regex NextMapRegex = RegexPatternHelper.GetRegex<NextMapInfo>();

        public NextMapInfo Parse(string input)
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

            return DictionaryModelConverter.ConvertDictionaryToModel<NextMapInfo>(parsedValues);
        }
    }
}
