// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ChatMessageParser : IParser<ChatMessageEventModel>
    {
        private static readonly Regex ChatMessageRegex = RegexPatternHelper.GetRegex<ChatMessageEventModel>();

        public ChatMessageEventModel Parse(string input)
        {
            input = input.SanitizeInput();

            Match match = ChatMessageRegex.Match(input);
            if (!match.Success || match.Groups.Count < 6)
            {
                return null;
            }

            string eosId = match.Groups[2].Value;
            ulong steamId = ulong.Parse(match.Groups[3].Value);
            CreatorOnlineModel creatorIds = new(eosId, steamId);

            Dictionary<string, string> parsedValues = new()
            {
                { "Channel", match.Groups[1].Value },
                { "PlayerName", match.Groups[4].Value },
                { "Message", match.Groups[5].Value }
            };

            ChatMessageEventModel chatMessage = DictionaryModelConverter.ConvertDictionaryToModel<ChatMessageEventModel>(parsedValues);
            chatMessage.CreatorIds = creatorIds;

            return chatMessage;
        }
    }
}