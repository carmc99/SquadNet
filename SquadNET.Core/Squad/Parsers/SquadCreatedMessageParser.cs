using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class SquadCreatedMessageParser : ICommandParser<SquadCreatedInfo>
    {
        public SquadCreatedInfo Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = RegexPatternHelper.GetRegex<SquadCreatedInfo>().Match(input);
            if (!match.Success || match.Groups.Count < 6)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "PlayerNameWithoutPrefix", match.Groups[1].Value },
                { "PlayerSteamId", match.Groups[2].Value },
                { "SquadId", match.Groups[3].Value },
                { "SquadName", match.Groups[4].Value },
                { "TeamName", match.Groups[5].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<SquadCreatedInfo>(parsedValues);
        }
    }
}
