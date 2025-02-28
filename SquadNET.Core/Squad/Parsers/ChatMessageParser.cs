using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ChatMessageParser : IParser<ChatMessageInfo>
    {
        private static readonly Regex ChatMessageRegex = RegexPatternHelper.GetRegex<ChatMessageInfo>();

        public ChatMessageInfo Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = ChatMessageRegex.Match(input);
            if (!match.Success || match.Groups.Count < 6)
            {
                return null;
            }

            string eosId = match.Groups[2].Value;
            ulong steamId = ulong.Parse(match.Groups[3].Value);
            CreatorOnlineIds creatorIds = new(eosId, steamId);

            Dictionary<string, string> parsedValues = new()
            {
                { "Channel", match.Groups[1].Value },
                { "PlayerName", match.Groups[4].Value },
                { "Message", match.Groups[5].Value }
            };

            ChatMessageInfo chatMessage = DictionaryModelConverter.ConvertDictionaryToModel<ChatMessageInfo>(parsedValues);
            chatMessage.CreatorIds = creatorIds;

            return chatMessage;
        }
    }
}
