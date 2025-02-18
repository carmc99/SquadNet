using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListSquadsParser : ICommandParser<List<SquadInfo>>
    {
        private const string Header = "----- Active Squads -----";

        public List<SquadInfo> Parse(string input)
        {
            input = input
                .SanitizeInput()
                .Replace(Header, "");

            string[] lines = input.Split('\n');
            TeamId team = TeamId.Team1;
            string teamName = string.Empty;

            List<SquadInfo> squads = [];

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                Match teamMatch = RegexPatternHelper.GetRegex<TeamInfo>().Match(line);
                if (teamMatch.Success)
                {
                    team = (TeamId)int.Parse(teamMatch.Groups[1].Value);
                    teamName = teamMatch.Groups[2].Value;
                    continue;
                }

                Match squadMatch = RegexPatternHelper.GetRegex<SquadInfo>().Match(line);
                if (!squadMatch.Success)
                {
                    continue;
                }

                Dictionary<string, string> parsedValues = new()
                {
                    { "Id", squadMatch.Groups[1].Value },
                    { "TeamId", ((int)team).ToString() },
                    { "TeamName", teamName },
                    { "Name", squadMatch.Groups[2].Value },
                    { "Size", squadMatch.Groups[3].Value },
                    { "IsLocked", squadMatch.Groups[4].Value },
                    { "CreatorName", squadMatch.Groups[5].Value }
                };

                string eosId = squadMatch.Groups[6].Value;
                ulong steamId = ulong.Parse(squadMatch.Groups[7].Value);
                CreatorOnlineIds creatorIds = new(eosId, steamId);

                SquadInfo squad = DictionaryModelConverter.ConvertDictionaryToModel<SquadInfo>(parsedValues);
                squad.CreatorIds = creatorIds;
                squads.Add(squad);
            }

            return squads;
        }
    }
}