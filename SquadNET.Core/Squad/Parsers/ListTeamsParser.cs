using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListTeamsParser : IParser<List<TeamModel>>
    {
        private const string Header = "----- Active Squads -----";

        public List<TeamModel> Parse(string input)
        {
            input = input
                .SanitizeInput()
                .Replace(Header, "");

            string[] lines = input.Split('\n');
            List<TeamModel> teams = [];

            foreach (string line in lines)
            {
                Match match = RegexPatternHelper.GetRegex<TeamModel>().Match(line);
                if (!match.Success)
                {
                    continue;
                }

                Dictionary<string, string> parsedValues = new()
                {
                    { "Id", match.Groups[1].Value },
                    { "Name", match.Groups[2].Value }
                };

                TeamModel team = DictionaryModelConverter.ConvertDictionaryToModel<TeamModel>(parsedValues);
                teams.Add(team);
            }

            return teams;
        }
    }
}