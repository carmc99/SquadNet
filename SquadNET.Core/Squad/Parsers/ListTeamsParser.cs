using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListTeamsParser : IParser<List<TeamInfo>>
    {
        private const string Header = "----- Active Squads -----";

        public List<TeamInfo> Parse(string input)
        {
            input = input
                .SanitizeInput()
                .Replace(Header, "");

            string[] lines = input.Split('\n');
            List<TeamInfo> teams = [];

            foreach (string line in lines)
            {
                Match match = RegexPatternHelper.GetRegex<TeamInfo>().Match(line);
                if (!match.Success)
                {
                    continue;
                }

                Dictionary<string, string> parsedValues = new()
                {
                    { "Id", match.Groups[1].Value },
                    { "Name", match.Groups[2].Value }
                };

                TeamInfo team = DictionaryModelConverter.ConvertDictionaryToModel<TeamInfo>(parsedValues);
                teams.Add(team);
            }

            return teams;
        }
    }
}