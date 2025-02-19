using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    public class ShowCurrentMapParser : ICommandParser<CurrentMapInfo>
    {
        private static readonly Regex CurrentMapRegex = RegexPatternHelper.GetRegex<CurrentMapInfo>();

        public CurrentMapInfo Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = CurrentMapRegex.Match(input);
            if (!match.Success || match.Groups.Count < 3)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Level", match.Groups[1].Value },
                { "Name", match.Groups[2].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<CurrentMapInfo>(parsedValues);
        }
    }
}
