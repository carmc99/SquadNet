using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class SquadCreatedMessageParser : IParser<SquadCreatedInfo>
    {
        private static readonly Regex SquadCreatedRegex = RegexPatternHelper.GetRegex<SquadCreatedInfo>();

        public SquadCreatedInfo Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = SquadCreatedRegex.Match(input);
            if (!match.Success || match.Groups.Count < 7)
            {
                return null;
            }

            string eosId = match.Groups[2].Value;
            ulong steamId = ulong.Parse(match.Groups[3].Value);
            CreatorOnlineIds creatorIds = new(eosId, steamId);

            Dictionary<string, string> parsedValues = new()
        {
            { "PlayerName", match.Groups[1].Value },
            { "SquadId", match.Groups[4].Value },
            { "SquadName", match.Groups[5].Value },
            { "TeamName", match.Groups[6].Value }
        };

            SquadCreatedInfo squadCreated = DictionaryModelConverter.ConvertDictionaryToModel<SquadCreatedInfo>(parsedValues);
            squadCreated.CreatorIds = creatorIds;

            return squadCreated;
        }
    }

}
