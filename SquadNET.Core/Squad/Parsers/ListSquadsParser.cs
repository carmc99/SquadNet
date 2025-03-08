using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListSquadsParser : IParser<List<SquadModel>>
    {
        private const string Header = "----- Active Squads -----";

        public List<SquadModel> Parse(string input)
        {
            input = input
                .SanitizeInput()
                .Replace(Header, "");

            string[] lines = input.Split('\n');
            TeamType team = TeamType.Team1;
            string teamName = string.Empty;

            List<SquadModel> squads = [];

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                Match teamMatch = RegexPatternHelper.GetRegex<TeamModel>().Match(line);
                if (teamMatch.Success)
                {
                    team = (TeamType)int.Parse(teamMatch.Groups[1].Value);
                    teamName = teamMatch.Groups[2].Value;
                    continue;
                }

                Match squadMatch = RegexPatternHelper.GetRegex<SquadModel>().Match(line);
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
                CreatorOnlineModel creatorIds = new(eosId, steamId);

                SquadModel squad = DictionaryModelConverter.ConvertDictionaryToModel<SquadModel>(parsedValues);
                squad.CreatorIds = creatorIds;
                squads.Add(squad);
            }

            return squads;
        }
    }
}