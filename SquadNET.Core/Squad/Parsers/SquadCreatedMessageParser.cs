using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Core.Squad.Parsers
{
    internal class SquadCreatedMessageParser : IParser<SquadCreatedEventModel>
    {
        private static readonly Regex SquadCreatedRegex = RegexPatternHelper.GetRegex<SquadCreatedEventModel>();

        public SquadCreatedEventModel Parse(string input)
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

            SquadCreatedEventModel squadCreated = DictionaryModelConverter.ConvertDictionaryToModel<SquadCreatedEventModel>(parsedValues);
            squadCreated.CreatorIds = creatorIds;

            return squadCreated;
        }
    }

}
