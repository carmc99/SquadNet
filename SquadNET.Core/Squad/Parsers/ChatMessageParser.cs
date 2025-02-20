using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ChatMessageParser : ICommandParser<ChatMessageInfo>
    {
        private static readonly Regex ChatMessageRegex = RegexPatternHelper.GetRegex<ChatMessageInfo>();

        public ChatMessageInfo Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = ChatMessageRegex.Match(input);
            if (!match.Success || match.Groups.Count < 5)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Channel", match.Groups[1].Value },
                { "PlayerSteamId64", match.Groups[2].Value },
                { "PlayerName", match.Groups[3].Value },
                { "Message", match.Groups[4].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<ChatMessageInfo>(parsedValues);
        }
    }
}
